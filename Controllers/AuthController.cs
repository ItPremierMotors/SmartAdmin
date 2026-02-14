using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartAdmin.Interfaces;
using SmartAdmin.Models.Account;

namespace SmartAdmin.Controllers
{
    public class AuthController : Controller
    {
        private readonly IApiClient _apiClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(IApiClient apiClient, IHttpContextAccessor httpContextAccessor)
        {
            _apiClient = apiClient;
            _httpContextAccessor = httpContextAccessor;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // Si ya está autenticado, redirigir al dashboard
            if (User.Identity?.IsAuthenticated ?? false)
            {
                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Usar el DTO con JsonPropertyName
            var request = new LoginRequest
            {
                Email = model.Email,
                Password = model.Password
            };
            // Llamar a la API
            var result = await _apiClient.PostAsync<LoginResponse>("/api/Auth/login", request);

            if (result.Success && result.Data != null)
            {
                // Guardar token en cookie
                var cookieOptions = new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddHours(8)
                };

                Response.Cookies.Append("jwt", result.Data.Token, cookieOptions);

                // Redirigir
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                TempData["SuccessMessage"] = $"Bienvenido {result.Data.User.UserName} ";
                return RedirectToAction("Index", "Dashboard");
            }
            // Mostrar errores
            ModelState.AddModelError(string.Empty, result.Message ?? "Error al iniciar sesión");

            if (result.Errors != null)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            return View(model);
        }
        public IActionResult Logout()
        {
            Response.Cookies.Delete("jwt");
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
