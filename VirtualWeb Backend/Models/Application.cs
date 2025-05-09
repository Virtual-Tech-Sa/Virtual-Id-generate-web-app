using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VID.Models
{
    public class Application{

        [Key]
        [Column("APPLICATION_ID")]  
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicationId { get; set; }

        [Column("FULLNAME")]
        public string? FullName {get; set;}

        // [Column("DATEOFBIRTH")]
        // public DateTime? DOB { get; set; }

        [Column(TypeName = "timestamp with time zone")]
        public DateTime? DOB { get; set; }

        [Column("GENDER")]
        public string? Gender {get; set;}
        
        [Column("APPLICANT_ID")]  
        public string? PersonId {get; set;}

        [Column("PHONENUMBER")]  
        public string? PhoneNumber {get; set;}

        [Column("EMAIL")]  
        public string? Email {get; set;}

        [Column("COUNTRYOFBIRTH")]
        public String? CountryOfBirth { get; set; }

        [Column("PROVINCE")]
        public string? Province {get; set;}

        [Column("FATHERNAME")]
        public string? FatherName {get; set;}

        [Column("FATHERID")]
        public string? FatherId {get; set;}

        [Column("MOTHERRNAME")]
        public string? MotherName {get; set;}

        [Column("MOTHERID")]
        public string? MotherId {get; set;}

        [Column("NATIONALITY")]
        [Required]
        public String? Nationality { get; set; }

        [Column("CITIZENSHIP")]
        [Required]
        public String? Citizenship { get; set; }

        [Column("STATUS")]
        public String? Status { get; set; }

        [Column("PROFILE_PICTURE")]
        public byte[]? ProfilePicture { get; set; } 
    }

}