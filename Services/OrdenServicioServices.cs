using SmartAdmin.Interfaces;
using SmartAdmin.Models.Catalogo.EstadoOs;
using SmartAdmin.Models.htpp;
using SmartAdmin.Models.Taller;

namespace SmartAdmin.Services
{
    public class OrdenServicioServices : IOrdenServicio
    {
        private readonly IApiClient apiClient;

        public OrdenServicioServices(IApiClient apiClient)
        {
            this.apiClient = apiClient;
        }

        // ─── OrdenesServicio ───

        public async Task<ApiResponse<List<OrdenServicioViewModel>>> GetAllAsync()
            => await apiClient.GetAsync<List<OrdenServicioViewModel>>("api/OrdenesServicio/GetAll");

        public async Task<ApiResponse<OrdenServicioViewModel>> GetByIdAsync(int osId)
            => await apiClient.GetAsync<OrdenServicioViewModel>($"api/OrdenesServicio/GetById/{osId}");

        public async Task<ApiResponse<OrdenServicioDetalleViewModel>> GetDetalleAsync(int osId)
            => await apiClient.GetAsync<OrdenServicioDetalleViewModel>($"api/OrdenesServicio/GetDetalle/{osId}");

        public async Task<ApiResponse<List<OrdenServicioViewModel>>> GetAbiertasAsync(int? sucursalId = null)
        {
            var url = "api/OrdenesServicio/GetAbiertas";
            if (sucursalId.HasValue) url += $"?sucursalId={sucursalId}";
            return await apiClient.GetAsync<List<OrdenServicioViewModel>>(url);
        }

        public async Task<ApiResponse<List<OrdenServicioViewModel>>> GetByFechaAsync(DateTime fecha, int? sucursalId = null)
        {
            var url = $"api/OrdenesServicio/GetByFecha/{fecha:yyyy-MM-dd}";
            if (sucursalId.HasValue) url += $"?sucursalId={sucursalId}";
            return await apiClient.GetAsync<List<OrdenServicioViewModel>>(url);
        }

        public async Task<ApiResponse<List<OrdenServicioViewModel>>> GetByRangoAsync(DateTime fechaInicio, DateTime fechaFin, int? sucursalId = null)
        {
            var url = $"api/OrdenesServicio/GetByRango?fechaInicio={fechaInicio:yyyy-MM-dd}&fechaFin={fechaFin:yyyy-MM-dd}";
            if (sucursalId.HasValue) url += $"&sucursalId={sucursalId}";
            return await apiClient.GetAsync<List<OrdenServicioViewModel>>(url);
        }

        public async Task<ApiResponse<List<OrdenServicioViewModel>>> GetByEstadoAsync(int estadoId, int? sucursalId = null)
        {
            var url = $"api/OrdenesServicio/GetByEstado/{estadoId}";
            if (sucursalId.HasValue) url += $"?sucursalId={sucursalId}";
            return await apiClient.GetAsync<List<OrdenServicioViewModel>>(url);
        }

        public async Task<ApiResponse<OrdenServicioViewModel>> CreateFromCitaAsync(CreateOsFromCitaViewModel model)
            => await apiClient.PostAsync<OrdenServicioViewModel>("api/OrdenesServicio/CreateFromCita", model);

        public async Task<ApiResponse<OrdenServicioViewModel>> CreateWalkInAsync(CreateOsWalkInViewModel model)
            => await apiClient.PostAsync<OrdenServicioViewModel>("api/OrdenesServicio/CreateWalkIn", model);

        public async Task<ApiResponse<OrdenServicioViewModel>> UpdateAsync(UpdateOrdenServicioViewModel model)
            => await apiClient.PostAsync<OrdenServicioViewModel>("api/OrdenesServicio/Update", model);

        public async Task<ApiResponse<bool>> CambiarEstadoAsync(CambiarEstadoOsViewModel model)
            => await apiClient.PostAsync<bool>("api/OrdenesServicio/cambiar-estado", model);

        public async Task<ApiResponse<int?>> CerrarAsync(CerrarOsViewModel model)
            => await apiClient.PostAsync<int?>("api/OrdenesServicio/cerrar", model);

        public async Task<ApiResponse<bool>> CancelarAsync(int osId, string motivo)
            => await apiClient.PostAsync<bool>($"api/OrdenesServicio/cancelar/{osId}", motivo);

        public async Task<ApiResponse<bool>> RecalcularTotalesAsync(int osId)
            => await apiClient.PostAsync<bool>($"api/OrdenesServicio/{osId}/recalcular");

        public async Task<ApiResponse<List<EstadoOsViewModel>>> GetTransicionesValidasAsync(int osId)
            => await apiClient.GetAsync<List<EstadoOsViewModel>>($"api/OrdenesServicio/GetTransicionesValidas/{osId}");

        // ─── OsServicios ───

        public async Task<ApiResponse<List<OsServicioViewModel>>> GetServiciosByOsIdAsync(int osId)
            => await apiClient.GetAsync<List<OsServicioViewModel>>($"api/OsServicios/GetByOsId/{osId}");

        public async Task<ApiResponse<OsServicioViewModel>> AgregarServicioAsync(AgregarServicioViewModel model)
            => await apiClient.PostAsync<OsServicioViewModel>("api/OsServicios/Create", model);

        public async Task<ApiResponse<OsServicioViewModel>> UpdateServicioAsync(UpdateOsServicioViewModel model)
            => await apiClient.PutAsync<OsServicioViewModel>("api/OsServicios/Update", model);

        public async Task<ApiResponse<bool>> QuitarServicioAsync(int osServicioId)
            => await apiClient.DeleteAsync<bool>($"api/OsServicios/QuitarServicio{osServicioId}");

        public async Task<ApiResponse<bool>> IniciarTrabajoServicioAsync(int osServicioId)
            => await apiClient.PostAsync<bool>($"api/OsServicios/iniciarTrabajo/{osServicioId}");

        public async Task<ApiResponse<bool>> CompletarServicioAsync(int osServicioId)
            => await apiClient.PostAsync<bool>($"api/OsServicios/completar/{osServicioId}");

        public async Task<ApiResponse<bool>> CancelarServicioAsync(int osServicioId)
            => await apiClient.PostAsync<bool>($"api/OsServicios/cancelar/{osServicioId}");

        // ─── AsignacionesTecnico ───

        public async Task<ApiResponse<List<AsignacionTecnicoViewModel>>> GetAsignacionesByOsIdAsync(int osId)
            => await apiClient.GetAsync<List<AsignacionTecnicoViewModel>>($"api/AsignacionesTecnico/GetByOsId/{osId}");

        public async Task<ApiResponse<AsignacionTecnicoViewModel>> AsignarTecnicoAsync(CreateAsignacionViewModel model)
            => await apiClient.PostAsync<AsignacionTecnicoViewModel>("api/AsignacionesTecnico/Asignar", model);

        public async Task<ApiResponse<bool>> IniciarAsignacionAsync(int asignacionId)
            => await apiClient.PostAsync<bool>($"api/AsignacionesTecnico/iniciar/{asignacionId}");

        public async Task<ApiResponse<bool>> PausarAsignacionAsync(int asignacionId)
            => await apiClient.PostAsync<bool>($"api/AsignacionesTecnico/pausar/{asignacionId}");

        public async Task<ApiResponse<bool>> ReanudarAsignacionAsync(int asignacionId)
            => await apiClient.PostAsync<bool>($"api/AsignacionesTecnico/reanudar/{asignacionId}");

        public async Task<ApiResponse<bool>> CompletarAsignacionAsync(int asignacionId)
            => await apiClient.PostAsync<bool>($"api/AsignacionesTecnico/completar/{asignacionId}");

        public async Task<ApiResponse<AsignacionTecnicoViewModel>> ReasignarAsync(ReasignarViewModel model)
            => await apiClient.PostAsync<AsignacionTecnicoViewModel>("api/AsignacionesTecnico/reasignar", model);

        public async Task<ApiResponse<bool>> CancelarAsignacionAsync(int asignacionId)
            => await apiClient.DeleteAsync<bool>($"api/AsignacionesTecnico/Cancelar{asignacionId}");
    }
}
