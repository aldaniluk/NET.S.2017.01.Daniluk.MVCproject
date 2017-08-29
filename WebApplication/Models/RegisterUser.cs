using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class RegisterUser
    {
        [Required]
        [StringLength(50)]
        [Display(Name = "Имя")]
        public string Name { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required]
        [StringLength(30)]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(30)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Пароль")]
        public string ConfirmPassword { get; set; }

        [Required]
        [StringLength(50)]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Почта")]
        public string Email { get; set; }

        [Display(Name = "Пол")]
        public Gender Gender { get; set; } //?
    }
}