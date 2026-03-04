using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using System.Threading.Tasks;
using static SmartAdmin.Models.UserRole.RoleViewModels;

namespace SmartAdmin.Controllers
{
    [Authorize(Roles = "AdminTI")]
    public class RolesController : Controller
    {
       private readonly IApiClient _apiClient;
        public RolesController(IApiClient apiClient)
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

            var result =await _apiClient.GetAsync<List<RoleViewModel>>("api/Roles");
            if (!result.Success)
            {
                TempData["Error"] = result.Message ?? "Error al obtener roles";
                return View(new List<RoleViewModel>());
            }
            return Json(result);
        }

    //vista parcial para crear rol
        [HttpGet]
        public IActionResult CreatePartial()
        {
            return PartialView("_Create",new CreateRoleViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }
            var result = await _apiClient.PostAsync<string>("api/Roles", model);
            return Json(result);
        }
        //vistas parcial para editar
        [HttpGet]
        public async Task<IActionResult> EditPartial(string Id)
        {
            var result = await _apiClient.GetAsync<RoleViewModel>($"api/Roles/{Id}");

            
            if (!result.Success || result.Data == null)
            {
                TempData["Error"] = result.Message ?? "Error al obtener el rol";
                return RedirectToAction("Index");
            }
            var model = new UpdateRoleViewModel
            {
                Id = result.Data.Id,
                Name = result.Data.Name
            };
            return PartialView("_Edit", model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] UpdateRoleViewModel rol)
        {
            //validar que venga correctamente
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }
            var result = await _apiClient.PutAsync<bool>($"/api/Roles/{rol.Id}", rol);
            return Json(result);
        }

        // ============================================
        // PERMISOS
        // ============================================

        [HttpGet]
        public async Task<IActionResult> PermissionsPartial(string id)
        {
            // Obtener todos los permisos disponibles
            var allPermissions = await _apiClient.GetAsync<List<string>>("api/Permissions");
            // Obtener permisos asignados al rol
            var rolePermissions = await _apiClient.GetAsync<List<string>>($"api/Permissions/role/{id}");
            // Obtener info del rol
            var role = await _apiClient.GetAsync<RoleViewModel>($"api/Roles/{id}");

            ViewBag.RoleId = id;
            ViewBag.RoleName = role.Data?.Name ?? "Rol";
            ViewBag.AllPermissions = allPermissions.Data ?? new List<string>();
            ViewBag.RolePermissions = rolePermissions.Data ?? new List<string>();

            return PartialView("_Permissions");
        }

        [HttpPost]
        public async Task<IActionResult> SavePermissions([FromBody] SavePermissionsRequest request)
        {
            var result = await _apiClient.PutAsync<bool>($"api/Permissions/role/{request.RoleId}", request.Permissions);
            return Json(result);
        }
    }

    public class SavePermissionsRequest
    {
        public string RoleId { get; set; } = null!;
        public List<string> Permissions { get; set; } = new();
    }
}
