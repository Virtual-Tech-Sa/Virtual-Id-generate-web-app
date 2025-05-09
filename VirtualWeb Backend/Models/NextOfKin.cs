using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VID.Models
{
    public class NextOfKin{
    
        [Key]
        [Column("NEXT_OF_KIN_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NextOfKinId { get; set; } 

        [Column("PERSON_ID")]
        public String? PersonId { get; set; }

        //MOTHER
        
        [Column("MOTHER_NAME")]
        public string? MotherName { get; set; }

        [Column("MOTHER_IDENTITYID")]
        public string? MotherID { get; set; }
        
        //FATHER
        
        [Column("FATHER_NAME")]
        public string? FatherName { get; set; }

        [Column("FATHER_IDENTITYID")]
        public string? FatherId { get; set; }

    }
}