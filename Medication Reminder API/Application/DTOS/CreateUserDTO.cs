public class CreateUserDTO
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public string? Specialty { get; set; } = ""; // للـ Doctor
    public int Age { get; set; } = 0;
    public string? ChronicConditions { get; set; } = "";
    public string? Gender { get; set; } = "NotSpecified";
    public string? RelationToPatient { get; set; } = "Unknown"; // للـ Caregiver
}