// ─── orden-servicio.js — Index de Ordenes de Servicio ───

const estadoOsBadge = {
    'ABIERTA':      'bg-primary',
    'DIAGNOSTICO':  'bg-info',
    'COTIZADA':     'bg-secondary',
    'APROBADA':     'bg-success',
    'EN_TRABAJO':   'bg-warning text-dark',
    'PAUSADA':      'bg-secondary',
    'COMPLETADA':   'bg-success',
    'FACTURADA':    'bg-dark',
    'CERRADA':      'bg-dark',
    'CANCELADA':    'bg-danger'
};

let dtOs = null;
let datosOriginales = [];

$(document).ready(function () {
    cargarFiltros();
    cargarAbiertas();

    $('#filtroEstado').select2({ theme: 'bootstrap-5', allowClear: true, placeholder: 'Todos', width: '100%' });
    $('#filtroSucursal').select2({ theme: 'bootstrap-5', allowClear: true, placeholder: 'Todas', width: '100%' });
});

// ─── Carga de filtros ───

function cargarFiltros() {
    $.get(URLS.getEstados, function (r) {
        if (r.success && r.data) {
            r.data.forEach(e => {
                $('#filtroEstado').append(`<option value="${e.estadoId}">${e.nombre}</option>`);
            });
        }
    });
    $.get(URLS.getSucursales, function (r) {
        if (r.success && r.data) {
            r.data.forEach(s => {
                $('#filtroSucursal').append(`<option value="${s.id}">${s.nombre}</option>`);
            });
        }
    });
}

// ─── Carga de datos ───

function cargarAbiertas() {
    const sucursalId = $('#filtroSucursal').val() || null;
    const params = sucursalId ? { sucursalId } : {};
    cargarDatos(URLS.getAbiertas, params);
}

function cargarTodas() {
    cargarDatos(URLS.getAll);
}

function cargarPorFecha(fecha) {
    const sucursalId = $('#filtroSucursal').val() || null;
    const params = { fecha };
    if (sucursalId) params.sucursalId = sucursalId;
    cargarDatos(URLS.getByFecha, params);
}

function cargarPorRango(desde, hasta) {
    const sucursalId = $('#filtroSucursal').val() || null;
    const params = { fechaInicio: desde, fechaFin: hasta };
    if (sucursalId) params.sucursalId = sucursalId;
    cargarDatos(URLS.getByRango, params);
}

function cargarPorEstado(estadoId) {
    const sucursalId = $('#filtroSucursal').val() || null;
    const params = { estadoId };
    if (sucursalId) params.sucursalId = sucursalId;
    cargarDatos(URLS.getByEstado, params);
}

function cargarDatos(url, params) {
    $('#loading').removeClass('d-none');
    $('#tablaOs').addClass('d-none');

    $.get(url, params || {}, function (response) {
        if (response.success && response.data) {
            datosOriginales = response.data;
            renderizarTabla(datosOriginales);
        } else {
            Toast.error(response.message || 'Error al cargar ordenes');
        }
        $('#loading').addClass('d-none');
    }).fail(function () {
        Toast.error('Error al conectar con el servidor');
        $('#loading').addClass('d-none');
    });
}

// ─── DataTable ───

function renderizarTabla(datos) {
    if (dtOs) dtOs.destroy();

    dtOs = $('#tablaOs').DataTable({
        data: datos,
        columns: [
            {
                data: 'numeroOs',
                render: (data, type, row) =>
                    `<a href="${URLS.detalle}/${row.osId}" class="fw-bold text-primary">${data}</a>`
            },
            {
                data: 'fechaApertura',
                render: data => data ? new Date(data).toLocaleDateString('es-HN') : ''
            },
            {
                data: 'clienteNombre',
                render: data => data || '<span class="text-muted">N/A</span>'
            },
            {
                data: null,
                render: (data, type, row) => {
                    let txt = row.vehiculoDescripcion || '';
                    if (row.vehiculoPlaca) txt += ` <small class="text-muted">(${row.vehiculoPlaca})</small>`;
                    return txt || '<span class="text-muted">N/A</span>';
                }
            },
            {
                data: 'estadoCodigo',
                className: 'text-center',
                render: data => {
                    const cls = estadoOsBadge[data] || 'bg-secondary';
                    const nombre = data ? data.replace(/_/g, ' ') : '';
                    return `<span class="badge ${cls}">${nombre}</span>`;
                }
            },
            {
                data: 'tipoIngresoNombre',
                render: data => data || ''
            },
            {
                data: 'totalGeneral',
                className: 'text-end',
                render: data => data != null ? 'L ' + data.toLocaleString('es-HN', { minimumFractionDigits: 2 }) : ''
            },
            {
                data: 'osId',
                orderable: false,
                className: 'text-center',
                render: data => `
                    <div class="btn-group btn-group-sm">
                        <a href="${URLS.detalle}/${data}" class="btn btn-outline-info" title="Ver Detalle">
                            <i class="fas fa-eye"></i>
                        </a>
                        <button class="btn btn-outline-warning" onclick="abrirCambiarEstadoIndex(${data})" title="Cambiar Estado">
                            <i class="fas fa-exchange-alt"></i>
                        </button>
                    </div>
                `
            }
        ],
        language: { url: 'https://cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json' },
        pageLength: 15,
        lengthMenu: [10, 15, 25, 50],
        order: [[1, 'desc']],
        responsive: true
    });

    $('#tablaOs').removeClass('d-none');
}

// ─── Filtros ───

function aplicarFiltros() {
    const estado = $('#filtroEstado').val();
    const desde = $('#filtroFechaDesde').val();
    const hasta = $('#filtroFechaHasta').val();

    if (estado) {
        cargarPorEstado(estado);
    } else if (desde && hasta) {
        cargarPorRango(desde, hasta);
    } else if (desde) {
        cargarPorFecha(desde);
    } else {
        cargarAbiertas();
    }
}

function limpiarFiltros() {
    $('#filtroEstado').val('').trigger('change.select2');
    $('#filtroSucursal').val('').trigger('change.select2');
    $('#filtroFechaDesde').val('');
    $('#filtroFechaHasta').val('');
    // Restaurar boton activo
    $('.btn-group-sm .btn').removeClass('active');
    $('.btn-group-sm .btn:first-child').addClass('active');
    cargarAbiertas();
}

function filtroRapido(tipo, btn) {
    // Toggle active button
    $(btn).closest('.btn-group').find('.btn').removeClass('active');
    $(btn).addClass('active');

    // Limpiar filtros visuales
    $('#filtroEstado').val('').trigger('change.select2');
    $('#filtroFechaDesde').val('');
    $('#filtroFechaHasta').val('');

    switch (tipo) {
        case 'abiertas': cargarAbiertas(); break;
        case 'todas':    cargarTodas(); break;
        case 'hoy':      cargarPorFecha(new Date().toISOString().split('T')[0]); break;
    }
}

// ─── Acciones ───

function irADetalle(osId) {
    window.location.href = URLS.detalle + '/' + osId;
}

function abrirCambiarEstadoIndex(osId) {
    AppModal.open({
        title: '<i class="fas fa-exchange-alt me-2"></i>Cambiar Estado',
        url: URLS.cambiarEstadoPartial,
        data: { osId: osId },
        size: 'md'
    });
}

function confirmarCambiarEstado() {
    const osId = parseInt($('#CambiarEstadoOsId').val());
    const nuevoEstadoId = parseInt($('#NuevoEstadoId').val());
    const observaciones = $('#CambiarEstadoObservaciones').val() || null;
    const btn = document.getElementById('btnConfirmarCambioEstado');
    const btnOriginal = btn.innerHTML;

    if (!nuevoEstadoId) {
        Toast.error('Seleccione un estado');
        return;
    }

    $.ajax({
        url: URLS.postCambiarEstado,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ osId, nuevoEstadoId, observaciones }),
        beforeSend: () => $(btn).prop('disabled', true).html('<i class="fas fa-spinner fa-spin me-1"></i> Cambiando...'),
        success: function (response) {
            if (response.success) {
                AppModal.close();
                Toast.success('Estado actualizado correctamente');
                aplicarFiltros();
            } else {
                Toast.error(response.message || 'Error al cambiar estado');
            }
        },
        error: function (xhr) {
            Toast.error(xhr.responseJSON?.message || 'Error al cambiar estado');
        },
        complete: () => $(btn).prop('disabled', false).html(btnOriginal)
    });
}
