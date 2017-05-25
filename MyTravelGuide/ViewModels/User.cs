using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Servicios;
using RepositorioClases;

namespace ViewModels
{
    public class UserViewModel
    {
        [Key]
        public Int32 Id { get; set; }

        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public UserState Estado { get; set; }

    }

    public class CreateUserModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repetir contraseña")]
        [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
        public string repeatPassword { get; set; }
    }

    public class Rol
    {
        public int Id { get; set; }

        public string Description { get; set; }
    }

    public class ListUserViewModel
    {
        [Display(Name = "Id")]
        public long Id { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required]
        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Estado")]
        public UserState Estado { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Correo electrónico")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Tiene que ser un correo electrónico válido")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de la contraseña debe ser al menos de {2}.", MinimumLength = 6)]
        [Display(Name = "Contraseña")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "¿Recordarme?")]
        public bool Recordarme { get; set; }
    }

    public class EditPasswordModel
    {
        [Required]
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña actual")]
        public string actualPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El número de caracteres de {0} debe ser al menos {2}.", MinimumLength = 6)]
        [Display(Name = "Nueva contraseña")]
        [DataType(DataType.Password)]
        public string newPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Repetir contraseña nueva")]
        [Compare("newPassword", ErrorMessage = "La nueva contraseña y la contraseña de confirmación no coinciden.")]
        public string repeatPassword { get; set; }
    }

    public class ChangesModel
    {
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }

        [Display(Name = "Correo electrónico")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Cambio realizado")]
        public Changes Cambio { get; set; }

        [Required]
        [Display(Name = "¿Exitoso?")]
        public bool Success { get; set; }
    }

    public enum Changes
    {
        Cambio_Contraseña = 1,
        Verificacion_Cuenta = 2,
        Edito_Cuenta_Personal = 3,
        Edito_Cuenta = 4,
        Elimino_Cuenta = 5,
        Cambio_Estado_Cuenta = 6
    }
}