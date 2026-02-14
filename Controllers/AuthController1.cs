//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Identity.Data;
//using Microsoft.AspNetCore.Mvc;
//using PremierFlow.Application.Dtos.Auth;
//using SmartAdmin.Models.Account;

//using System.Net.Http;

//namespace Smartadmin.Controllers
//    {
//    [AllowAnonymous]
//    public class AuthController1 : Controller
//        {
//        private readonly IHttpClientFactory  httpClientFactory;
//        public AuthController1(IHttpClientFactory _httpClientFactory)
//        {
//            httpClientFactory = _httpClientFactory; //Para consumir una API externa: usa HttpClient
//        }
//        public IActionResult Twofactor()
//        {
//            return View();
//        }

//        public IActionResult Login()
//        {
//            return View();
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Login(LoginViewModel model)
//        {
//            if (!ModelState.IsValid)
//                return View(model);
//            // 1. mapear al dto
//            var request = new PremierFlow.Application.Dtos.Auth.LoginRequest
//            {
//                Email = model.Email,
//                Password = model.Password
//            };
//            // 2. llamar a la api
//            var client = httpClientFactory.CreateClient("PremierFlowApi");
//            var response = await client.PostAsJsonAsync("/api/Auth/login", request);

//            if (!response.IsSuccessStatusCode)
//            {
//                var mensajeError = await response.Content.ReadFromJsonAsync<ErrorResponseDTO>();
//                ModelState.AddModelError(string.Empty, mensajeError?.message?? "ERROR");
//                return View(model);
//            }

//            // 3) Leer la respuesta (JWT, expiración, etc.)
//            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>(); // 
            
//            if(loginResponse ==null || string.IsNullOrEmpty(loginResponse.Token))
//            {
//                //1. hubo error 
//                ModelState.AddModelError(string.Empty, "Error al procesar el login");
//                return View(model);
//            }
//            // 4) Guardar el token en una cookie segura
//            Response.Cookies.Append("JWT", loginResponse.Token, new CookieOptions
//            {
//                HttpOnly=true,
//                Secure=true,
//                SameSite=SameSiteMode.Strict,
//                Expires = DateTime.UtcNow.AddMinutes(60)

//            });
//            // 5) Redirigir al Home (dashboard)
//            return RedirectToAction("ControlCenter", "Dashboard");
//        }

//        public IActionResult Logout()
//        {
//            Response.Cookies.Delete("JWT");
//            return RedirectToAction("Login", "Auth");
//        }

//        public IActionResult Forgetpassword()
//        {
//            return View();
//        }
//        [AllowAnonymous]
//        public IActionResult Register()
//        {
//            //ViewBag.SuccessMessage = "Cuenta creada";
//            //ViewBag.ErrorMessage = "Cuenta creada";
//            //ViewBag.InfoMessage = "Cuenta creada";
//            //ViewBag.WarningMessage = "Cuenta creada";

//            return View();
//        }

//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Register(RegisterViewModel model)
//        {
//            //1. confirmar que datos seam correctos
//            if (!ModelState.IsValid)
//            {
//                var messege = string.Join("</br>",
//                    ModelState.Values
//                    .SelectMany(v => v.Errors)
//                    .Select(e => e.ErrorMessage));
//                ViewBag.ErrorMessage=messege;
//                return View(model);
//            }
//            // 2. mapeo al dto
//            var request = new PremierFlow.Application.Dtos.User.CreateUserRequest
//            {
//                Email = model.Email,
//                Password = model.Password,
//                NombreCompleto=model.FullName,
//                Activo = true,
//                Departamento = "General",
//                Cargo = "Usuario",
//            };

//            //3. llamar la api
//            var client = httpClientFactory.CreateClient("PremierFlowApi");
//            var response = await client.PostAsJsonAsync("/api/Auth/create-user", request);

//            //3.1 validar exito
//            if (!response.IsSuccessStatusCode) {
//                var mensajeError= await response.Content.ReadFromJsonAsync<ProblemDetails>();
//                ModelState.AddModelError(string.Empty, mensajeError?.Detail ?? "ERROR");
//                return View(model);
//            }
//            //4. rederigir al login con mensaje de usuario creado
//            TempData["SuccessMessage"] = "Cuenta creada exitosamente. Por favor inicia sesión.";
//            return RedirectToAction("Login","Auth");
            

//        }
//        public IActionResult Lockscreen()
//        {
//            return View();
//        }

        
//        }
//    }