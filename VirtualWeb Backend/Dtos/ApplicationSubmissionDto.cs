public class ApplicationSubmissionDto
{
    // Main applicant info
    public string PersonId { get; set; }
    public string FullName { get; set; }
    public string Gender { get; set; }
    public string DOB { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
    public string Nationality { get; set; }
    public string Citizenship { get; set; }
    public string Status { get; set; }
    public string CountryOfBirth { get; set; }
    public string Province { get; set; }
    public string address { get; set; }
    public string applicationType { get; set; }
    public string maritalStatus { get; set; }
    public string emergencyContact { get; set; }
    public string emergencyPhone { get; set; }
    public string disabilities { get; set; }

    // Next of kin
    public string NextOfKinFatherName { get; set; }
    public string NextOfKinFatherId { get; set; }
    public string NextOfKinMotherName { get; set; }
    public string NextOfKinMotherId { get; set; }

    // Applicant data
    public string ApplicantEmail { get; set; }
    public string ApplicantPhoneNumber { get; set; }

    // Profile photo
    public IFormFile ProfilePicture { get; set; }
}