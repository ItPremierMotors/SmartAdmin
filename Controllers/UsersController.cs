using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PremierFlow.Application.Dtos.User;
using PremierFlow.Application.Interfaces.Auth;
using SmartAdmin.Interfaces;
using static SmartAdmin.Models.UserRole.RoleViewModels;
using static SmartAdmin.Models.UserRole.UserViewModels;

namespace SmartAdmin.Controllers
{

    [Authorize(Roles = "AdminTI")]
    public class UsersController : Controller
    {
        private readonly IApiClient _apiClient;

        public UsersController(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()

        {
            var result = await _apiClient.GetAsync<List<UserViewModel>>("api/Auth/GetAllUsers");

            if (!result.Success)
            {
                TempData["Error"] = result.Message ?? "Error al obtener usuarios";
                return View(new List<UserViewModel>());
            }

            return Json(result);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string id, [FromBody] EditUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            //Lógica de actualización
            var result = await _apiClient.PutAsync<bool>($"api/Auth/Update-User/{id}", model);

            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(string id, [FromBody] ResetPasswordViewModel model)
        {
            var result = await _apiClient.PostAsync<bool>($"/api/Auth/Reset-Password/{id}", model);
            return Json(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }
            var result = await _apiClient.PostAsync<string>("api/Auth/Create-User", model);
            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> DetailPartial(string id)
        {
            var User_result = await _apiClient.GetAsync<UserViewModel>($"/api/Auth/GetUserById/{id}");
            if (User_result.Success && User_result.Data != null)
                return PartialView("_DetailPartial", User_result.Data);
            return Content("<div class='alert alert-danger'>Usuario no encontrado</div>");
        }
        [HttpGet]
        public async Task<IActionResult> EditPartial(string id)
        {
            var User_result = await _apiClient.GetAsync<UserViewModel>($"/api/Auth/GetUserById/{id}");
            // Obtener roles disponibles
            var rolesResult = await _apiClient.GetAsync<List<RoleViewModel>>("/api/Roles");
            // Obtener todos los usuarios para extraer cargos únicos
            var allUsersResult = await _apiClient.GetAsync<List<UserViewModel>>("api/Auth/GetAllUsers");

            // Extraer cargos únicos (distinct)
            var cargosDisponibles = new List<string>();
            {
                cargosDisponibles = allUsersResult?.Data?
                    .Where(u => !string.IsNullOrWhiteSpace(u.Cargo))
                    .Select(u => u.Cargo!)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();
            }
            ViewBag.CargosDisponibles = cargosDisponibles;
            ViewBag.RolesDisponibles = rolesResult.Success ? rolesResult.Data : null;
            if (User_result.Success && User_result.Data != null)
                return PartialView("_Edit", User_result.Data);
            return Content("<div class='alert alert-danger'>Usuario no encontrado</div>");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordPartial(string id, string email)
        {
            return PartialView("_ResetPassword", (Id: id, Email: email));
        }

        [HttpGet]
        public async Task<IActionResult> CreatePartial()
        {
            var newUser = new CreateUserViewModel();
            // Obtener todos los usuarios para extraer cargos únicos
            var allUsersResult = await _apiClient.GetAsync<List<UserViewModel>>("api/Auth/GetAllUsers");
            var rolesResult = await _apiClient.GetAsync<List<RoleViewModel>>("/api/Roles");

            // Extraer cargos únicos (distinct)
            var cargosDisponibles=allUsersResult?.Data?
                .Where(u => !string.IsNullOrWhiteSpace(u.Cargo))
                .Select(u => u.Cargo!)
                .Distinct()
                .OrderBy(c => c)
                .ToList();

            var DepartamentoDisponible = allUsersResult?.Data?
               .Where(u => !string.IsNullOrWhiteSpace(u.Departamento))
               .Select(u => u.Departamento!)
               .Distinct()
               .OrderBy(c => c)
               .ToList();
            if (cargosDisponibles==null)
            {
                cargosDisponibles=new List<string>();
            }
            if (DepartamentoDisponible == null)
            {
                DepartamentoDisponible = new List<string>();
            }
            //converitmos la lista a ViewBag para usarla en la vista
            var cargoselectList = cargosDisponibles.Select(c => new SelectListItem
            {
                Value = c,
                Text = c
            });
            var DepartamentoselectList = DepartamentoDisponible.Select(c => new SelectListItem
            {
                Value = c,
                Text = c
            });

            ViewBag.CargosDisponibles = cargoselectList;
            ViewBag.DepartamentoDisponibles = DepartamentoselectList;
            ViewBag.RolesDisponibles = rolesResult.Success ? rolesResult.Data : null;

            return PartialView("_Create", newUser);
        }
    }
}
