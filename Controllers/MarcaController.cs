using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.Marca;
using System.Threading.Tasks;
using static SmartAdmin.Models.UserRole.RoleViewModels;
using static SmartAdmin.Models.UserRole.UserViewModels;

namespace SmartAdmin.Controllers
{
    public class MarcaController : Controller
    {
        private readonly IMarca marcaservices;
        public MarcaController(IMarca marcaservices)
        {
            this.marcaservices = marcaservices;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> MarcaDetailPartial(string id)
        {

            var response = await marcaservices.GetDetails(id);
            if (response.Success && response.Data != null)
            {
                return PartialView("_DetailPartial", response.Data);
            }
            return Content("<div class='alert alert-danger'>Marca no encontrada</div>");
        }
        [HttpGet]
        public IActionResult DeletePartial(int id)
        {
            ViewBag.MarcaId = id;
            return PartialView("_DeletePartial");
        }
        [HttpGet]
        public async Task<IActionResult> CreatePartial()
        {
            return PartialView("_CreatePartial", new CreateMarcaViewModel());
        }
        [HttpGet]
        public async Task<IActionResult> EditPartial(string Id)
        {
            var response = await marcaservices.GetDetails(Id);
            if (response.Success && response.Data != null)
            {
                var editmodel = new EditMarcaViewModel
                {
                    MarcaId = response.Data.MarcaId,
                    Codigo = response.Data.Codigo,
                    Nombre = response.Data.Nombre,
                    PaisOrigen = response.Data.PaisOrigen,
                    EsMarcaPropia = response.Data.EsMarcaPropia,
                    LogoUrl = response.Data.LogoUrl,
                    Observaciones = response.Data.Observaciones
                };
                return PartialView("_EditPartial", editmodel);
            }
            return Content("<div class='alert alert-danger'>Marca no encontrada</div>");
        }
        //GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await marcaservices.GetAllAsync();
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "Error al obtener Marcas";
                // return View(new List<MarcaViewModel>());
            }
            return StatusCode(response.StatusCode, response);
        }
        //GetById
        [HttpGet]
        public async Task<IActionResult> GetById(string marcaId)
        {
            var response = await marcaservices.GetDetails(marcaId);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMarcaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await marcaservices.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditMarcaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await marcaservices.EditAsync(model);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var response = await marcaservices.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }
    }
}
