// ─── portal-tecnico.js — Portal del Tecnico ───

const estadoAsignacionBadge = {
    1: { cls: 'bg-info',              nombre: 'Asignado',   icon: 'fal fa-clock' },
    2: { cls: 'bg-warning text-dark', nombre: 'En Proceso', icon: 'fal fa-cog fa-spin' },
    3: { cls: 'bg-success',           nombre: 'Completado', icon: 'fal fa-check' },
    4: { cls: 'bg-secondary',         nombre: 'Pausado',    icon: 'fal fa-pause' }
};

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

$(document).ready(function () {
    cargarAsignaciones();
});

// ─── Cargar Asignaciones ───

function cargarAsignaciones() {
    $('#loadingAsignaciones').removeClass('d-none');
    $('#sinAsignaciones, #asignacionesContainer').addClass('d-none');

    $.get(URLS.getAsignacionesActivas, { tecnicoId: TECNICO_ID }, function (r) {
        $('#loadingAsignaciones').addClass('d-none');
        if (r.success && r.data && r.data.length > 0) {
            renderAsignaciones(r.data);
            $('#asignacionesContainer').removeClass('d-none');
        } else {
            $('#sinAsignaciones').removeClass('d-none');
        }
    }).fail(function () {
        $('#loadingAsignaciones').addClass('d-none');
        Toast.error('Error al cargar asignaciones');
    });
}

function renderAsignaciones(asignaciones) {
    var $container = $('#asignacionesContainer');
    $container.empty();

    asignaciones.forEach(function (a) {
        var badge = estadoAsignacionBadge[a.estado] || { cls: 'bg-secondary', nombre: 'Desconocido', icon: 'fal fa-question' };
        var tiempo = a.tiempoTrabajadoMinutos != null ? a.tiempoTrabajadoMinutos + ' min' : '-';
        var vehiculo = a.vehiculoDescripcion || 'Vehiculo no disponible';
        var placa = a.vehiculoPlaca ? '(' + a.vehiculoPlaca + ')' : '';
        var acciones = obtenerBotonesAccion(a);

        $container.append(
            '<div class="col-md-6 col-lg-4">' +
                '<div class="card border-start border-4 ' + getBorderColor(a.estado) + ' h-100">' +
                    '<div class="card-body">' +
                        '<div class="d-flex justify-content-between align-items-start mb-2">' +
                            '<h6 class="card-title mb-0">' +
                                '<i class="fal fa-file-alt me-1 text-primary"></i>' + a.numeroOs +
                            '</h6>' +
                            '<span class="badge ' + badge.cls + '">' +
                                '<i class="' + badge.icon + ' me-1"></i>' + badge.nombre +
                            '</span>' +
                        '</div>' +
                        '<p class="text-muted small mb-1">' +
                            '<i class="fal fa-car me-1"></i>' + vehiculo + ' ' + placa +
                        '</p>' +
                        '<p class="mb-1">' +
                            '<strong>Servicio:</strong> ' + (a.servicioDescripcion || '<span class="text-muted">General</span>') +
                        '</p>' +
                        '<p class="small text-muted mb-2">' +
                            '<i class="fal fa-stopwatch me-1"></i>Tiempo: ' + tiempo +
                        '</p>' +
                        '<div class="d-flex justify-content-between align-items-center">' +
                            '<div class="btn-group btn-group-sm">' +
                                acciones +
                            '</div>' +
                            '<button class="btn btn-outline-info btn-sm" onclick="verHistorialVehiculo(' + a.vehiculoId + ', \'' + escapeHtml(vehiculo + ' ' + placa) + '\')" title="Ver historial del vehiculo">' +
                                '<i class="fal fa-history me-1"></i>Historial' +
                            '</button>' +
                        '</div>' +
                    '</div>' +
                '</div>' +
            '</div>'
        );
    });
}

function escapeHtml(text) {
    return text.replace(/'/g, "\\'").replace(/"/g, '&quot;');
}

function getBorderColor(estado) {
    switch (estado) {
        case 1: return 'border-info';
        case 2: return 'border-warning';
        case 4: return 'border-secondary';
        default: return 'border-primary';
    }
}

function obtenerBotonesAccion(a) {
    var html = '';

    // Estado 1 = Asignado -> Iniciar
    if (a.estado === 1) {
        html += '<button class="btn btn-success btn-sm" onclick="accion(\'iniciar\', ' + a.asignacionId + ')">' +
                    '<i class="fal fa-play me-1"></i>Iniciar' +
                '</button>';
    }
    // Estado 2 = EnProceso -> Pausar, Completar
    else if (a.estado === 2) {
        html += '<button class="btn btn-secondary btn-sm" onclick="accion(\'pausar\', ' + a.asignacionId + ')">' +
                    '<i class="fal fa-pause me-1"></i>Pausar' +
                '</button>';
        html += '<button class="btn btn-success btn-sm ms-1" onclick="accion(\'completar\', ' + a.asignacionId + ')">' +
                    '<i class="fal fa-check me-1"></i>Completar' +
                '</button>';
    }
    // Estado 4 = Pausado -> Reanudar
    else if (a.estado === 4) {
        html += '<button class="btn btn-primary btn-sm" onclick="accion(\'reanudar\', ' + a.asignacionId + ')">' +
                    '<i class="fal fa-play me-1"></i>Reanudar' +
                '</button>';
    }

    return html;
}

// ─── Acciones de Asignacion ───

function accion(tipo, asignacionId) {
    var url, confirmMsg;

    switch (tipo) {
        case 'iniciar':
            url = URLS.postIniciar;
            confirmMsg = 'Iniciar trabajo en esta asignacion?';
            break;
        case 'pausar':
            url = URLS.postPausar;
            confirmMsg = 'Pausar el trabajo actual?';
            break;
        case 'reanudar':
            url = URLS.postReanudar;
            confirmMsg = 'Reanudar el trabajo?';
            break;
        case 'completar':
            url = URLS.postCompletar;
            confirmMsg = 'Marcar trabajo como completado?';
            break;
        default: return;
    }

    AppAlert.confirm({
        title: 'Confirmar',
        text: confirmMsg,
        onConfirm: function () {
            $.ajax({
                url: url,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(asignacionId),
                success: function (r) {
                    if (r.success) {
                        Toast.success(r.message || 'Asignacion actualizada');
                        cargarAsignaciones();
                    } else {
                        Toast.error(r.message || 'Error al actualizar asignacion');
                    }
                },
                error: function (xhr) {
                    var msg = 'Error al actualizar asignacion';
                    if (xhr.responseJSON && xhr.responseJSON.message) {
                        msg = xhr.responseJSON.message;
                    }
                    Toast.error(msg);
                }
            });
        }
    });
}

// ─── Historial del Vehiculo ───

function verHistorialVehiculo(vehiculoId, vehiculoNombre) {
    if (!vehiculoId) {
        Toast.warning('Vehiculo no disponible para este trabajo');
        return;
    }

    $('#historialCard').removeClass('d-none');
    $('#historialVehiculoNombre').text(vehiculoNombre);
    $('#loadingHistorial').removeClass('d-none');
    $('#tablaHistorial, #sinHistorial').addClass('d-none');

    // Scroll al historial
    $('html, body').animate({ scrollTop: $('#historialCard').offset().top - 80 }, 300);

    $.get(URLS.getHistorialVehiculo, { vehiculoId: vehiculoId }, function (r) {
        $('#loadingHistorial').addClass('d-none');
        if (r.success && r.data && r.data.length > 0) {
            renderHistorial(r.data);
            $('#tablaHistorial').removeClass('d-none');
        } else {
            $('#sinHistorial').removeClass('d-none');
        }
    }).fail(function () {
        $('#loadingHistorial').addClass('d-none');
        Toast.error('Error al cargar historial');
    });
}

function renderHistorial(ordenes) {
    var $body = $('#historialBody');
    $body.empty();

    ordenes.forEach(function (os) {
        var cls = estadoOsBadge[os.estadoCodigo] || 'bg-secondary';
        var fechaApertura = os.fechaApertura ? new Date(os.fechaApertura).toLocaleDateString('es-HN') : '-';
        var fechaCierre = os.fechaCierre ? new Date(os.fechaCierre).toLocaleDateString('es-HN') : '-';
        var total = os.totalGeneral != null ? 'L ' + os.totalGeneral.toLocaleString('es-HN', { minimumFractionDigits: 2 }) : '-';

        $body.append(
            '<tr>' +
                '<td><strong>' + os.numeroOs + '</strong></td>' +
                '<td>' + fechaApertura + '</td>' +
                '<td>' + fechaCierre + '</td>' +
                '<td class="text-center"><span class="badge ' + cls + '">' + (os.estadoNombre || os.estadoCodigo) + '</span></td>' +
                '<td class="text-end">' + total + '</td>' +
            '</tr>'
        );
    });
}

function cerrarHistorial() {
    $('#historialCard').addClass('d-none');
}
