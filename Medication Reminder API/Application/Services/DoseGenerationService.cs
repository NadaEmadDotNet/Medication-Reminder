
namespace Medication_Reminder_API.Services
{
    public class TestDoseGenerationService
    {
        public class DoseGenerationBackgroundService : BackgroundService
        {
            private readonly IServiceScopeFactory _scopeFactory;

            public DoseGenerationBackgroundService(IServiceScopeFactory scopeFactory)
            {
                _scopeFactory = scopeFactory;
            }

            protected override async Task ExecuteAsync(CancellationToken stoppingToken)
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                    var activeMedications = await db.Medications
                        .Where(m => m.Status == MedicationStatus.Ongoing)
                        .ToListAsync(stoppingToken);

                    var today = DateTime.Today;

                    foreach (var med in activeMedications)
                    {
                        var patientMeds = await db.PatientMedications
                            .Include(pm => pm.Patient)
                            .Where(pm => pm.MedicationID == med.MedicationID)
                            .ToListAsync(stoppingToken);

                        foreach (var pm in patientMeds)
                        {
                            bool alreadyGenerated = await db.DoseLogs
                                .AnyAsync(d => d.MedicationID == med.MedicationID
                                               && d.PatientID == pm.Patient.PatientID
                                               && d.ScheduledTime.Date == today,
                                          stoppingToken);

                            if (!alreadyGenerated)
                            {
                                double interval = 24.0 / med.Frequency;
                                for (int i = 0; i < med.Frequency; i++)
                                {
                                    db.DoseLogs.Add(new DoseLog
                                    {
                                        MedicationID = med.MedicationID,
                                        PatientID = pm.Patient.PatientID,
                                        Status = DoseStatus.Scheduled,
                                        ScheduledTime = today.AddHours(i * interval),
                                        PatientName= pm.Patient.Name
                                    });
                                }
                            }
                        }
                    }

                    await db.SaveChangesAsync(stoppingToken);

                    // هينتظر لحد اليوم التالي
                    var nextRun = DateTime.Today.AddDays(1).AddHours(0); // 12AM
                    var delay = nextRun - DateTime.Now;
                    if (delay < TimeSpan.Zero) delay = TimeSpan.Zero;

                    await Task.Delay(delay, stoppingToken);
                }
            }
        }
    }
}