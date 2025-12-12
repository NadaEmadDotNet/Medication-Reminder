using Medication_Reminder_API.Models;
using Medication_Reminder_API.Enums;
using Microsoft.EntityFrameworkCore;

namespace Medication_Reminder_API.Services
{
    public class DoseGenerationService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public DoseGenerationService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    await service.ExecuteAsync(CancellationToken.None); // للتجربة فقط

                    var activeMedications = await db.Medications
                        .Where(m => m.Status == MedicationStatus.Ongoing)
                        .ToListAsync(stoppingToken);

                    var today = DateTime.Today;
                    var tomorrow = today.AddDays(1);

                    foreach (var med in activeMedications)
                    {
                        var patients = await db.PatientMedications
                            .Where(pm => pm.MedicationID == med.MedicationID)
                            .Select(pm => pm.Patient)
                            .ToListAsync(stoppingToken);

                        foreach (var patient in patients)
                        {
                            // التحقق إذا تم توليد الجرعات لليوم بالفعل
                            bool alreadyGenerated = await db.DoseLogs
                                .AnyAsync(d => d.MedicationID == med.MedicationID
                                               && d.PatientID == patient.PatientID
                                               && d.ScheduledTime >= today
                                               && d.ScheduledTime < tomorrow, stoppingToken);

                            if (!alreadyGenerated)
                            {
                                double interval = 24.0 / med.Frequency; // توزيع أدق بالساعات

                                for (int i = 0; i < med.Frequency; i++)
                                {
                                    var scheduledTime = today.AddHours(i * interval);

                                    db.DoseLogs.Add(new DoseLog
                                    {
                                        MedicationID = med.MedicationID,
                                        PatientID = patient.PatientID,
                                        Status = DoseStatus.Scheduled,
                                        ScheduledTime = scheduledTime
                                    });
                                }
                            }
                        }
                    }

                    await db.SaveChangesAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error generating daily doses: {ex.Message}");
                }

                // انتظر 24 ساعة قبل الدورة التالية
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }
    }
}
