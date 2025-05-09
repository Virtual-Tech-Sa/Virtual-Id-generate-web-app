using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VID.Models
{
    public class Applicant{
        [Key]
        [Column("APPLICANT_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ApplicantId { get; set;}

        [Column("PERSON_ID")]
        public string? PersonId {get; set;}

        [Column("USER_PHONE_NUMBER")]
        public String? UserPhoneNumber { get; set; }

        [Column("EMAIL")]
        public String? Email { get; set; }

    }
}