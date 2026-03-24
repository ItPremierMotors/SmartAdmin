// ============================================================
// Historial de Citas - Page JS
// Vista de solo lectura para consultar citas por rango de fechas.
// Requiere: URLS (definido en la vista), AppModal, Toast
// ============================================================

let todasLasCitas = [];
let filtroEstadoActual = null;

// ─── Helpers ───

function getSucursalId() {
    return $('#filtroSucursal').val() || null;
}

function formatFecha(dateStr) {
    const d = new Date(dateStr + 'T12:00:00');
    return d.toLocaleDateString('es-HN', { day: '2-digit', month: 'short', year: 'numeric' });
}

function formatFechaLarga(dateStr) {
    const d = new Date(dateStr + 'T12:00:00');
    return d.toLocaleDateString('es-HN', { day: '2-digit', month: 'long', year: 'numeric' });
}

// ─── Inicializacion ───

$(document).ready(function () {
    // Por defecto: rango del mes actual
    setRangoPreset('mes');
    cargarSucursales();
});

// ─── Sucursales ───

function cargarSucursales() {
    $.get(URLS.getSucursales, function (response) {
        const $sel = $('#filtroSucursal');
        $sel.empty();
        if (response.success && response.data && response.data.length > 0) {
            response.data.forEach(s => {
                $sel.append(`<option value="${s.id}">${s.nombre}</option>`);
            });
            const saved = localStorage.getItem('sucursalHistorial');
            if (saved && $sel.find(`option[value="${saved}"]`).length) {
                $sel.val(saved);
            }
        } else {
            $sel.empty().append('<option value="">Sin sucursales</option>');
        }
        actualizarTituloRango();
        cargarCitas();
    }).fail(function () {
        $('#filtroSucursal').empty().append('<option value="">Error</option>');
        cargarCitas();
    });

    $('#filtroSucursal').on('change', function () {
        localStorage.setItem('sucursalHistorial', $(this).val());
        cargarCitas();
    });
}

// ─── Presets de rango rapido ───

function setRangoPreset(preset) {
    const hoy = new Date();
    let inicio, fin;

    switch (preset) {
        case 'hoy':
            inicio = fin = formatInputDate(hoy);
            break;
        case 'semana':
            const diaSemana = hoy.getDay();
            const lunes = new Date(hoy);
            lunes.setDate(hoy.getDate() - (diaSemana === 0 ? 6 : diaSemana - 1));
            inicio = formatInputDate(lunes);
            fin = formatInputDate(hoy);
            break;
        case 'mes':
            inicio = `${hoy.getFullYear()}-${String(hoy.getMonth() + 1).padStart(2, '0')}-01`;
            fin = formatInputDate(hoy);
            break;
        case 'mesAnterior':
            const mesAnt = new Date(hoy.getFullYear(), hoy.getMonth() - 1, 1);
            const finMesAnt = new Date(hoy.getFullYear(), hoy.getMonth(), 0);
            inicio = formatInputDate(mesAnt);
            fin = formatInputDate(finMesAnt);
            break;
    }

    $('#fechaInicio').val(inicio);
    $('#fechaFin').val(fin);
    actualizarTituloRango();
    cargarCitas();
}

function formatInputDate(d) {
    return `${d.getFullYear()}-${String(d.getMonth() + 1).padStart(2, '0')}-${String(d.getDate()).padStart(2, '0')}`;
}

// ─── Titulo del rango ───

function actualizarTituloRango() {
    const inicio = $('#fechaInicio').val();
    const fin = $('#fechaFin').val();
    if (inicio && fin) {
        if (inicio === fin) {
            $('#rangoTitulo').text(formatFechaLarga(inicio));
        } else {
            $('#rangoTitulo').text(formatFecha(inicio) + ' al ' + formatFecha(fin));
        }
    }
}

// ─── Carga de datos ───

function cargarCitas() {
    const fechaInicio = $('#fechaInicio').val();
    const fechaFin = $('#fechaFin').val();

    if (!fechaInicio || !fechaFin) return;
    if (fechaInicio > fechaFin) {
        Toast.warning('La fecha de inicio no puede ser mayor a la fecha fin');
        return;
    }

    actualizarTituloRango();
    $('#loadingCitas').removeClass('d-none');
    $('#sinCitas').addClass('d-none');
    $('#citasBody').empty();

    const params = { fechaInicio: fechaInicio, fechaFin: fechaFin };
    const sId = getSucursalId();
    if (sId) params.sucursalId = sId;

    $.get(URLS.getRango, params, function (response) {
        $('#loadingCitas').addClass('d-none');
        if (response.success && response.data && response.data.length > 0) {
            todasLasCitas = response.data;
            renderizarCitas();
            actualizarStats();
            $('#tablaCitas').removeClass('d-none');
        } else {
            todasLasCitas = [];
            actualizarStats();
            $('#tablaCitas').addClass('d-none');
            $('#sinCitas').removeClass('d-none');
        }
    }).fail(function () {
        $('#loadingCitas').addClass('d-none');
        Toast.error('Error al cargar citas');
    });
}

// ─── Render de tabla ───

function renderizarCitas() {
    const citas = filtroEstadoActual
        ? todasLasCitas.filter(c => c.estado === filtroEstadoActual)
        : todasLasCitas;

    const tbody = $('#citasBody');
    tbody.empty();

    if (citas.length === 0) {
        tbody.append('<tr><td colspan="8" class="text-center text-muted py-3">No hay citas con este filtro</td></tr>');
        return;
    }

    // Ordenar por fecha y hora
    citas.sort((a, b) => new Date(a.fechaHoraInicio) - new Date(b.fechaHoraInicio));

    citas.forEach(function (c) {
        const badgeClass = obtenerBadgeEstado(c.estado);
        const estadoNombre = obtenerNombreEstado(c.estado);
        const fechaCita = new Date(c.fechaHoraInicio);
        const fechaFormateada = fechaCita.toLocaleDateString('es-HN', { day: '2-digit', month: 'short' });

        const row = `
            <tr class="cita-row" ondblclick="abrirDetalleCita(${c.citaId})">
                <td><small class="text-muted">${fechaFormateada}</small></td>
                <td>
                    <i class="fas fa-clock me-1 text-muted"></i>
                    <strong>${c.horaInicioFormateada}</strong> - ${c.horaFinFormateada}
                </td>
                <td><code>${c.codigoCita}</code></td>
                <td>
                    <div>${c.clienteNombre}</div>
                    ${c.clienteTelefono ? '<small class="text-muted">' + c.clienteTelefono + '</small>' : ''}
                </td>
                <td>
                    <div>${c.vehiculoDescripcion}</div>
                    ${c.vehiculoPlaca ? '<small class="text-muted">' + c.vehiculoPlaca + '</small>' : ''}
                </td>
                <td>${c.tipoServicioNombre}</td>
                <td class="text-center"><span class="badge badge-estado ${badgeClass}">${estadoNombre}</span></td>
                <td class="text-center">
                    <button class="btn btn-sm btn-outline-secondary" onclick="abrirDetalleCita(${c.citaId})" title="Ver Detalle">
                        <i class="fas fa-eye"></i>
                    </button>
                </td>
            </tr>`;
        tbody.append(row);
    });
}

// ─── Stats ───

function actualizarStats() {
    $('#statTotal').text(todasLasCitas.length);
    $('#statAgendadas').text(todasLasCitas.filter(c => c.estado === 1).length);
    $('#statConfirmadas').text(todasLasCitas.filter(c => c.estado === 2).length);
    $('#statEnProceso').text(todasLasCitas.filter(c => c.estado === 3).length);
    $('#statCompletadas').text(todasLasCitas.filter(c => c.estado === 4).length);
    $('#statCanceladas').text(todasLasCitas.filter(c => c.estado === 5 || c.estado === 6).length);
}

// ─── Estados ───

function obtenerBadgeEstado(estado) {
    switch (estado) {
        case 1: return 'bg-primary';
        case 2: return 'bg-info';
        case 3: return 'bg-warning text-dark';
        case 4: return 'bg-success';
        case 5: return 'bg-danger';
        case 6: return 'bg-dark';
        default: return 'bg-secondary';
    }
}

function obtenerNombreEstado(estado) {
    switch (estado) {
        case 1: return 'Agendada';
        case 2: return 'Confirmada';
        case 3: return 'En Proceso';
        case 4: return 'Completada';
        case 5: return 'Cancelada';
        case 6: return 'No Show';
        default: return 'Desconocido';
    }
}

// ─── Filtro por estado ───

function filtrarEstado(estado, btn) {
    filtroEstadoActual = estado;
    $('.btn-group .btn').removeClass('active');
    $(btn).addClass('active');
    renderizarCitas();
}

// ─── Modal detalle ───

function abrirDetalleCita(citaId) {
    AppModal.open({
        title: '<i class="fas fa-calendar-check me-2"></i>Detalle de Cita',
        url: URLS.detallePartial,
        data: { citaId: citaId },
        size: 'lg'
    });
}
