using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VID.Models
{
    public class Admin{
    
        [Key]
        [Column("ADMIN_ID")]
        public int AdminId { get; set; } 
        
        [Column("ADMIN_NAME")]
        public string? AdminName { get; set; }

        [Column("ADMIN_SURNAME")]
        public string? AdminSurname { get; set; }

        [Column("ADMIN_EMP_SURNAME")]
        public string? AdminEmpNo { get; set; }

        [Column("USER_ID")]
        public int UserId { get; set; }  // Auto-Generate UUID for UserId
    }
}