using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VID.Models
{
    public class Person{
    
        [Key]
        [Column("PERSON_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PersonId { get; set;}

        [Column("IDENTITY_ID")]
        [Required]
        public String? IdentityId {get; set;}

        [Column("FIRSTNAME")]
        public string? Firstname { get; set; }

        [Column("SURNAME")]
        public string? Surname { get; set; }

        [Column("DATEOFBIRTH")]
        public DateOnly? DateOfBirth { get; set; }

        [Column("Email")]
        public String? Email {get; set;}

        [Column("USER_PASSWORD")]
        public String? UserPassword { get; set; }

        [Column("GENDER")]
        public String? Gender { get; set; }
       public string? PasswordResetToken { get; set; }

        public DateTime? ResetTokenExpires{get; set;}
    }
}