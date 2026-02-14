using System.ComponentModel.DataAnnotations;

namespace SmartAdmin.Models.UserRole
{
    public class RoleViewModels
    {
        public class RoleViewModel
        {
            public string Id { get; set; } = null!;
            public string Name { get; set; } = null!;
            public int UsersCount { get; set; }
        }

        public class CreateRoleViewModel
        {
            [Required(ErrorMessage="Campo Obligatorio")]
            [Display(Name =("Ingresa el nuevo Rol"))]
            public string Name { get; set; } = null!;
        }

        public class UpdateRoleViewModel
        {
            [Required(ErrorMessage = "Campo Obligatorio")]
            [Display(Name = ("Ingresa el ID"))]
            public string Id { get; set; } =null!;

            [Required(ErrorMessage = "Campo Obligatorio")]
            [Display(Name = ("Ingresa el nuevo Rol"))]
            public string Name { get; set; } = null!;
        }
    }
}
