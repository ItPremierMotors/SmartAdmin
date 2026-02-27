using Microsoft.AspNetCore.Authentication.JwtBearer;
using SmartAdmin.Interfaces;
using SmartAdmin.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options=>{
    options.Filters.Add(new Microsoft.AspNetCore.Mvc.Authorization.AuthorizeFilter()); //agregamos autorization a todos los controllers
});

// Leer configuracion de JWT
var jwtSection = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSection.GetValue<string>("Key")!);


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(Options=>
    {
       Options.RequireHttpsMetadata= false; //solo para desarrollo
       Options.SaveToken= true; //guardar el token en la solicitud
         Options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
         {
             ValidateIssuer = true, //validar emisor
             ValidateAudience = true, //validar audiencia
             ValidateLifetime = true, //validar tiempo de expiracion
             ValidateIssuerSigningKey = true, //validar la firma del token
             ValidIssuer = jwtSection.GetValue<string>("Issuer"), //emisor valido
             ValidAudience = jwtSection.GetValue<string>("Audience"), //audiencia valida
             IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key), //clave de firma
             ClockSkew = TimeSpan.Zero //eliminar el tiempo de tolerancia para la expiracion del token
         };

        //leer el token desde la cookie
        Options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Cookies["jwt"];
                if (!string.IsNullOrEmpty(accessToken))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                // Cuando el JWT expira o no es válido, redirigir al login
                context.HandleResponse();
                context.Response.Redirect("/Auth/Login");
                return Task.CompletedTask;
            }
        };

    });

// HttpClient para hablar con la API
builder.Services.AddHttpClient("PremierFlowApi", c =>
{
    var baseUrl = builder.Configuration["PremierFlowApi:BaseUrl"]
                 ?? throw new InvalidOperationException("PremierFlowApi:BaseUrl no configurado");

    c.BaseAddress = new Uri(baseUrl);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
});

//serivicios
builder.Services.AddScoped<IApiClient, ApiClientServices>();
builder.Services.AddScoped<IAuth, AuthServices>();
builder.Services.AddScoped<IMarca, MarcaServices>();
builder.Services.AddScoped<IModelo, ModeloServices>();
builder.Services.AddScoped<IVersionVehiculo, VersionVehiculoServices>();
builder.Services.AddScoped<ITipoServicio, TipoServicioServices>();
builder.Services.AddScoped<ITecnico, TecnicoServices>();
builder.Services.AddScoped<IEstadoOs, EstadoOsServices>();
builder.Services.AddScoped<ISucursal, SucursalServices>();
builder.Services.AddScoped<IUbicacion, UbicacionServices>();
builder.Services.AddScoped<ICliente, ClienteServices>();
builder.Services.AddScoped<IVehiculo, VehiculoServices>();
builder.Services.AddScoped<ICapacidadTaller, CapacidadTallerServices>();
builder.Services.AddScoped<IBloqueHorario, BloqueHorarioServices>();
builder.Services.AddScoped<ICita, CitaServices>();
builder.Services.AddScoped<IRecepcion, RecepcionServices>();
builder.Services.AddScoped<IOrdenServicio, OrdenServicioServices>();
// Para poder leer cookies en servicios
builder.Services.AddHttpContextAccessor(); 


var app = builder.Build();

//paaginas para errores
app.UseExceptionHandler("/Error/ServerError");// Errores 5xx (excepciones no controladas)

// Errores 4xx (404, 401, 403, etc.)
app.UseStatusCodePagesWithReExecute("/Error/Error4042/{0}"); 
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication(); //->primero autenticacion
app.UseAuthorization(); //->luego autorizacion

app.MapStaticAssets();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Dashboard}/{action=Index}/{id?}")
//    .WithStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");




app.Run();
