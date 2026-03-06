using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Controllers
{
    public class RecepcionController : Controller
    {
        private readonly IRecepcion recepcionServices;

        public RecepcionController(IRecepcion recepcionServices)
        {
            this.recepcionServices = recepcionServices;
        }

        // GET: /Recepcion/Wizard?citaId=123
        public async Task<IActionResult> Wizard(int citaId)
        {
            var response = await recepcionServices.GetDatosCitaAsync(citaId);
            if (!response.Success || response.Data == null)
                return RedirectToAction("Index", "Agenda");
            return View(response.Data);
        }

        // POST: /Recepcion/IniciarRecepcion
        [HttpPost]
        public async Task<IActionResult> IniciarRecepcion([FromBody] IniciarRecepcionRequest model)
        {
            try
            {
                var response = await recepcionServices.IniciarDesdeCitaAsync(model);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, statusCode = 500 });
            }
        }

        // GET: /Recepcion/WizardWalkIn?sucursalId=2
        public IActionResult WizardWalkIn(int? sucursalId = null)
        {
            var model = new RecepcionWizardViewModel
            {
                IsWalkIn = true,
                CitaId = 0,
                CodigoCita = "WALK-IN",
                ClienteNombre = "",
                VehiculoDescripcion = "",
                TipoServicioNombre = "",
                MotivoVisita = "",
                KilometrajeActual = 0,
                SegmentoVehiculo = 1,
                SucursalId = sucursalId ?? 0
            };
            return View("Wizard", model);
        }

        // POST: /Recepcion/IniciarRecepcionWalkIn
        [HttpPost]
        public async Task<IActionResult> IniciarRecepcionWalkIn([FromBody] IniciarRecepcionWalkInRequest model)
        {
            try
            {
                var response = await recepcionServices.IniciarWalkInAsync(model);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, statusCode = 500 });
            }
        }

        // POST: /Recepcion/SubirEvidencia
        [HttpPost]
        public async Task<IActionResult> SubirEvidencia([FromBody] SubirEvidenciaRequest model)
        {
            try
            {
                var response = await recepcionServices.SubirEvidenciaBase64Async(model);
                return StatusCode(response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = ex.Message, statusCode = 500 });
            }
        }
    }
}
