using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.Modelo;

namespace SmartAdmin.Controllers
{
    public class ModelosController : Controller
    {
        private readonly IModelo modeloservices;
        public ModelosController(IModelo modeloservices)
        {
            this.modeloservices = modeloservices;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ModeloDetailPartial(string id)
        {

            var response = await modeloservices.GetDetails(id);
            if (response.Success && response.Data != null)
            {
                return PartialView("_DetailPartial", response.Data);
            }
            return Content("<div class='alert alert-danger'>Modelo no encontrado</div>");
        }
        [HttpGet]
        public IActionResult DeletePartial(int id)
        {
            ViewBag.ModeloId = id;
            return PartialView("_DeletePartial");
        }
        [HttpGet]
        public async Task<IActionResult> CreatePartial()
        {
            return PartialView("_CreatePartial", new CreateModeloViewModel());
        }
        [HttpGet]
        public async Task<IActionResult> EditPartial(string Id)
        {
            var response = await modeloservices.GetDetails(Id);
            if (response.Success && response.Data != null)
            {
                var editmodel = new EditModeloViewModel
                {
                    ModeloId = response.Data.ModeloId,
                    Codigo = response.Data.Codigo,
                    Nombre = response.Data.Nombre,
                    MarcaId = response.Data.MarcaId,
                    Segmento = response.Data.Segmento,
                    AnioInicio = response.Data.AnioInicio,
                    AnioFin = response.Data.AnioFin,
                    Descripcion = response.Data.Descripcion,
                    ImagenUrl = response.Data.ImagenUrl
                };
                return PartialView("_EditPartial", editmodel);
            }
            return Content("<div class='alert alert-danger'>Modelo no encontrado</div>");
        }
        //GetAll
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await modeloservices.GetAllAsync();
            if (!response.Success)
            {
                TempData["Error"] = response.Message ?? "Error al obtener Modelos";
                // return View(new List<ModeloViewModel>());
            }
            return StatusCode(response.StatusCode, response);
        }
        //GetById
        [HttpGet]
        public async Task<IActionResult> GetById(string modeloId)
        {
            var response = await modeloservices.GetDetails(modeloId);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateModeloViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await modeloservices.CreateAsync(model);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] EditModeloViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await modeloservices.EditAsync(model);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> Delete([FromBody] int id)
        {
            var response = await modeloservices.DeleteAsync(id);
            return StatusCode(response.StatusCode, response);
        }

        [HttpGet]
        public async Task<IActionResult> GetByMarca(string marcaId)
        {
            var response = await modeloservices.GetByMarcaAsync(marcaId);
            return StatusCode(response.StatusCode, response);
        }
    }
}
