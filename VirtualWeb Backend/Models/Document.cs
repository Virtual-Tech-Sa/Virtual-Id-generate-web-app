using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VID.Models
{
    public class Document{
    
        [Key]
        [Column("DOCUMENT_ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DocumentId { get; set; }

        [Column("APPLICATION_ID")]  //FK that references the Appplication table using the column named IDENTITY_ID
        public string? ApplicationId { get; set; }

        [Column("DOCUMENT_CODE")]
        public String? DocumentCode {get; set;}

        [Column("DOCUMENT_BIRTHCERTIFICATE")]
        public byte[]? BirthCertificate { get; set; }

        [Column("DOCUMENT_IDENTITY_NUMBER_COPY")]
        public  byte[]? IdCopy { get; set; }
    }
}