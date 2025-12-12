public class CaregiverPatientDTO
{
    public int PatientID { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public string Gender { get; set; }
    public string ChronicConditions { get; set; }

    public List<PatientMedicationDTO> Medications { get; set; } = new();
}
