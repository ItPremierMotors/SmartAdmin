// ─── orden-servicio-detalle.js — Detalle de Orden de Servicio ───

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

const estadoServicioBadge = {
    1: { cls: 'bg-secondary',        nombre: 'Pendiente' },
    2: { cls: 'bg-warning text-dark', nombre: 'En Proceso' },
    3: { cls: 'bg-success',           nombre: 'Completado' },
    4: { cls: 'bg-danger',            nombre: 'Cancelado' }
};

const estadoAsignacionBadge = {
    1: { cls: 'bg-info',              nombre: 'Asignado' },
    2: { cls: 'bg-warning text-dark', nombre: 'En Proceso' },
    3: { cls: 'bg-success',           nombre: 'Completado' },
    4: { cls: 'bg-secondary',         nombre: 'Pausado' }
};

$(document).ready(function () {
    renderAccionesOs();
    cargarServicios();
    cargarAsignaciones();
    actualizarEvidenciasCount();

    // Cargar tabs on show
    $('button[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
        const target = $(e.target).data('bs-target');
        if (target === '#tabTecnicos') cargarAsignaciones();
    });
});

// ═══════════════════════════════════════════
// ACCIONES OS (Header buttons)
// ═══════════════════════════════════════════

function renderAccionesOs() {
    const $container = $('#accionesOs');
    $container.empty();

    if (!OS_ESTA_ABIERTA) {
        let closedHtml = '<span class="text-muted"><i class="fas fa-lock me-1"></i> Orden cerrada/cancelada</span>';
        closedHtml += ` <a class="btn btn-outline-info btn-sm ms-2" href="${URLS.reporte}" target="_blank" title="Ver Reporte">
                            <i class="fas fa-file-alt me-1"></i> Reporte
                        </a>`;
        $container.html(closedHtml);
        $('#btnAgregarServicio, #btnAsignarTecnico').addClass('d-none');
        return;
    }

    // Agregar servicio: solo en DIAGNOSTICO
    if (OS_ESTADO_CODIGO !== 'DIAGNOSTICO') {
        $('#btnAgregarServicio').addClass('d-none');
    }

    // Asignar tecnico: en DIAGNOSTICO, COTIZADA, APROBADA, EN_TRABAJO
    const estadosAsignarTecnico = ['DIAGNOSTICO', 'COTIZADA', 'APROBADA', 'EN_TRABAJO'];
    if (!estadosAsignarTecnico.includes(OS_ESTADO_CODIGO)) {
        $('#btnAsignarTecnico').addClass('d-none');
    }

    let html = '<div class="btn-group btn-group-sm">';

    // Reporte - disponible a partir de COTIZADA
    const estadosConReporte = ['COTIZADA', 'APROBADA', 'EN_TRABAJO', 'PAUSADA', 'COMPLETADA', 'FACTURADA'];
    if (estadosConReporte.includes(OS_ESTADO_CODIGO)) {
        html += `<a class="btn btn-outline-info" href="${URLS.reporte}" target="_blank" title="Ver Reporte">
                    <i class="fas fa-file-alt me-1"></i> Reporte
                 </a>`;
    }

    // Cambiar estado - siempre disponible si la OS no esta cerrada/cancelada
    html += `<button class="btn btn-warning" onclick="abrirCambiarEstado()" title="Cambiar Estado">
                <i class="fas fa-exchange-alt me-1"></i> Estado
             </button>`;

    // Recalcular - disponible en DIAGNOSTICO y COTIZADA
    if (['DIAGNOSTICO', 'COTIZADA'].includes(OS_ESTADO_CODIGO)) {
        html += `<button class="btn btn-outline-secondary" onclick="recalcularTotales()" title="Recalcular Totales">
                    <i class="fas fa-calculator me-1"></i> Recalcular
                 </button>`;
    }

    // Cerrar - solo FACTURADA
    if (OS_ESTADO_CODIGO === 'FACTURADA') {
        html += `<button class="btn btn-success" onclick="cerrarOs()" title="Cerrar Orden">
                    <i class="fas fa-check-double me-1"></i> Cerrar
                 </button>`;
    }

    // Cancelar - no disponible en EN_TRABAJO, COMPLETADA, FACTURADA
    const puedeCancelar = !['EN_TRABAJO', 'COMPLETADA', 'FACTURADA'].includes(OS_ESTADO_CODIGO);
    if (puedeCancelar) {
        html += `<button class="btn btn-outline-danger" onclick="cancelarOs()" title="Cancelar Orden">
                    <i class="fas fa-ban me-1"></i> Cancelar
                 </button>`;
    }

    html += '</div>';
    $container.html(html);
}

// ═══════════════════════════════════════════
// TAB: SERVICIOS
// ═══════════════════════════════════════════

function cargarServicios() {
    $('#loadingServicios').removeClass('d-none');
    $('#sinServicios, #tablaServicios').addClass('d-none');

    $.get(URLS.getServicios, { osId: OS_ID }, function (r) {
        $('#loadingServicios').addClass('d-none');
        if (r.success && r.data && r.data.length > 0) {
            renderServicios(r.data);
            $('#tablaServicios').removeClass('d-none');
            $('#countServicios').text(r.data.length);
        } else {
            $('#sinServicios').removeClass('d-none');
            $('#countServicios').text('0');
        }
    }).fail(function () {
        $('#loadingServicios').addClass('d-none');
        Toast.error('Error al cargar servicios');
    });
}

function renderServicios(servicios) {
    const $body = $('#serviciosBody');
    $body.empty();

    servicios.forEach(s => {
        const badge = estadoServicioBadge[s.estado] || { cls: 'bg-secondary', nombre: s.estadoNombre || s.estado };
        const acciones = obtenerAccionesServicio(s);

        $body.append(`
            <tr>
                <td><strong>${s.tipoServicioNombre}</strong></td>
                <td>${s.descripcionTrabajo || '<span class="text-muted">-</span>'}</td>
                <td>${s.tecnicoNombre || '<span class="text-muted">Sin asignar</span>'}</td>
                <td class="text-center"><span class="badge ${badge.cls}">${badge.nombre}</span></td>
                <td class="text-end">L ${s.precioUnitario.toLocaleString('es-HN', { minimumFractionDigits: 2 })}</td>
                <td class="text-center">${s.cantidad}</td>
                <td class="text-end fw-bold">L ${s.subtotal.toLocaleString('es-HN', { minimumFractionDigits: 2 })}</td>
                <td class="text-center">${acciones}</td>
            </tr>
        `);
    });
}

function obtenerAccionesServicio(s) {
    // Solo permitir quitar servicio en DIAGNOSTICO y si esta Pendiente
    if (OS_ESTADO_CODIGO === 'DIAGNOSTICO' && s.estado === 1) {
        return `<button class="btn btn-outline-danger btn-sm" onclick="quitarServicio(${s.osServicioId})" title="Quitar">
                    <i class="fas fa-trash"></i>
                </button>`;
    }
    return '';
}

function accionServicio(accion, osServicioId) {
    let url;
    let confirmMsg;

    switch (accion) {
        case 'iniciar':
            url = URLS.postIniciarServicio;
            confirmMsg = 'Iniciar trabajo en este servicio?';
            break;
        case 'completar':
            url = URLS.postCompletarServicio;
            confirmMsg = 'Marcar este servicio como completado?';
            break;
        case 'cancelar':
            url = URLS.postCancelarServicio;
            confirmMsg = 'Cancelar este servicio?';
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
                data: JSON.stringify(osServicioId),
                success: function (r) {
                    if (r.success) {
                        Toast.success('Servicio actualizado');
                        cargarServicios();
                        refrescarDetalle();
                    } else {
                        Toast.error(r.message || 'Error al actualizar servicio');
                    }
                },
                error: function (xhr) {
                    Toast.error(xhr.responseJSON?.message || 'Error al actualizar servicio');
                }
            });
        }
    });
}

function quitarServicio(osServicioId) {
    AppAlert.eliminar({
        title: 'Quitar servicio',
        text: 'Esta seguro de quitar este servicio?',
        onConfirm: function () {
            $.ajax({
                url: URLS.postQuitarServicio,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(osServicioId),
                success: function (r) {
                    if (r.success) {
                        Toast.success('Servicio eliminado');
                        cargarServicios();
                        refrescarDetalle();
                    } else {
                        Toast.error(r.message || 'Error al quitar servicio');
                    }
                },
                error: function (xhr) {
                    Toast.error(xhr.responseJSON?.message || 'Error al quitar servicio');
                }
            });
        }
    });
}

function abrirAgregarServicio() {
    AppModal.open({
        title: '<i class="fas fa-plus me-2"></i>Agregar Servicio',
        url: URLS.agregarServicioPartial,
        data: { osId: OS_ID },
        size: 'lg'
    });
}

function confirmarAgregarServicio() {
    const tipoServicioId = parseInt($('#AgregarTipoServicioId').val());
    if (!tipoServicioId) {
        Toast.error('Seleccione un tipo de servicio');
        return;
    }

    const data = {
        osId: OS_ID,
        tipoServicioId: tipoServicioId,
        descripcionTrabajo: $('#AgregarDescripcion').val() || null,
        precioUnitario: $('#AgregarPrecioUnitario').val() ? parseFloat($('#AgregarPrecioUnitario').val()) : null,
        cantidad: parseInt($('#AgregarCantidad').val()) || 1,
        tecnicoAsignadoId: $('#AgregarTecnicoId').val() ? parseInt($('#AgregarTecnicoId').val()) : null,
        observaciones: $('#AgregarObservaciones').val() || null
    };

    const btn = document.getElementById('btnConfirmarAgregarServicio');
    const btnOriginal = btn.innerHTML;

    $.ajax({
        url: URLS.postAgregarServicio,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: () => $(btn).prop('disabled', true).html('<i class="fas fa-spinner fa-spin me-1"></i> Agregando...'),
        success: function (r) {
            if (r.success) {
                AppModal.close();
                Toast.success('Servicio agregado exitosamente');
                cargarServicios();
                refrescarDetalle();
            } else {
                Toast.error(r.message || 'Error al agregar servicio');
            }
        },
        error: function (xhr) {
            Toast.error(xhr.responseJSON?.message || 'Error al agregar servicio');
        },
        complete: () => $(btn).prop('disabled', false).html(btnOriginal)
    });
}

// ═══════════════════════════════════════════
// TAB: TECNICOS / ASIGNACIONES
// ═══════════════════════════════════════════

function cargarAsignaciones() {
    $('#loadingTecnicos').removeClass('d-none');
    $('#sinTecnicos, #tablaTecnicos').addClass('d-none');

    $.get(URLS.getAsignaciones, { osId: OS_ID }, function (r) {
        $('#loadingTecnicos').addClass('d-none');
        if (r.success && r.data && r.data.length > 0) {
            renderAsignaciones(r.data);
            $('#tablaTecnicos').removeClass('d-none');
            $('#countTecnicos').text(r.data.length);
        } else {
            $('#sinTecnicos').removeClass('d-none');
            $('#countTecnicos').text('0');
        }
    }).fail(function () {
        $('#loadingTecnicos').addClass('d-none');
        Toast.error('Error al cargar asignaciones');
    });
}

function renderAsignaciones(asignaciones) {
    const $body = $('#tecnicosBody');
    $body.empty();

    asignaciones.forEach(a => {
        const badge = estadoAsignacionBadge[a.estado] || { cls: 'bg-secondary', nombre: a.estadoNombre || a.estado };
        const acciones = obtenerAccionesAsignacion(a);
        const tiempo = a.tiempoTrabajadoMinutos != null ? `${a.tiempoTrabajadoMinutos} min` : '-';
        const fechaAsignacion = a.fechaAsignacion ? new Date(a.fechaAsignacion).toLocaleDateString('es-HN') : '';

        $body.append(`
            <tr>
                <td><strong>${a.tecnicoNombre}</strong></td>
                <td>${a.tecnicoCodigo || ''}</td>
                <td>${a.servicioDescripcion || '<span class="text-muted">General</span>'}</td>
                <td class="text-center"><span class="badge ${badge.cls}">${badge.nombre}</span></td>
                <td>${fechaAsignacion}</td>
                <td>${tiempo}</td>
                <td class="text-center">${acciones}</td>
            </tr>
        `);
    });
}

function obtenerAccionesAsignacion(a) {
    if (!OS_ESTA_ABIERTA) return '';

    const osPuedeIniciar = ['APROBADA', 'EN_TRABAJO'].includes(OS_ESTADO_CODIGO);
    let html = '<div class="btn-group btn-group-sm">';

    // Asignado -> Iniciar (solo si OS APROBADA/EN_TRABAJO), Cancelar
    if (a.estado === 1) {
        if (osPuedeIniciar) {
            html += `<button class="btn btn-outline-success btn-sm" onclick="accionAsignacion('iniciar', ${a.asignacionId})" title="Iniciar">
                        <i class="fas fa-play"></i>
                     </button>`;
        }
        html += `<button class="btn btn-outline-danger btn-sm" onclick="cancelarAsignacion(${a.asignacionId})" title="Cancelar">
                    <i class="fas fa-trash"></i>
                 </button>`;
    }
    // EnProceso -> Pausar, Completar
    else if (a.estado === 2) {
        html += `<button class="btn btn-outline-secondary btn-sm" onclick="accionAsignacion('pausar', ${a.asignacionId})" title="Pausar">
                    <i class="fas fa-pause"></i>
                 </button>`;
        html += `<button class="btn btn-outline-success btn-sm" onclick="accionAsignacion('completar', ${a.asignacionId})" title="Completar">
                    <i class="fas fa-check"></i>
                 </button>`;
    }
    // Pausado -> Reanudar
    else if (a.estado === 4) {
        html += `<button class="btn btn-outline-primary btn-sm" onclick="accionAsignacion('reanudar', ${a.asignacionId})" title="Reanudar">
                    <i class="fas fa-play"></i>
                 </button>`;
    }

    html += '</div>';
    return html;
}

function accionAsignacion(accion, asignacionId) {
    let url;
    let confirmMsg;

    switch (accion) {
        case 'iniciar':
            url = URLS.postIniciarAsignacion;
            confirmMsg = 'Iniciar trabajo de este tecnico?';
            break;
        case 'pausar':
            url = URLS.postPausarAsignacion;
            confirmMsg = 'Pausar trabajo de este tecnico?';
            break;
        case 'reanudar':
            url = URLS.postReanudarAsignacion;
            confirmMsg = 'Reanudar trabajo de este tecnico?';
            break;
        case 'completar':
            url = URLS.postCompletarAsignacion;
            confirmMsg = 'Marcar como completado?';
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
                        Toast.success('Asignacion actualizada');
                        // Si se inicio trabajo y la OS estaba APROBADA, se auto-transiciono a EN_TRABAJO
                        if (accion === 'iniciar' && OS_ESTADO_CODIGO === 'APROBADA') {
                            setTimeout(() => location.reload(), 1000);
                        } else {
                            cargarAsignaciones();
                            cargarServicios();
                        }
                    } else {
                        Toast.error(r.message || 'Error al actualizar asignacion');
                    }
                },
                error: function (xhr) {
                    Toast.error(xhr.responseJSON?.message || 'Error al actualizar asignacion');
                }
            });
        }
    });
}

function cancelarAsignacion(asignacionId) {
    AppAlert.eliminar({
        title: 'Cancelar asignacion',
        text: 'Esta seguro de cancelar esta asignacion?',
        onConfirm: function () {
            $.ajax({
                url: URLS.postCancelarAsignacion,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(asignacionId),
                success: function (r) {
                    if (r.success) {
                        Toast.success('Asignacion cancelada');
                        cargarAsignaciones();
                        cargarServicios();
                    } else {
                        Toast.error(r.message || 'Error al cancelar asignacion');
                    }
                },
                error: function (xhr) {
                    Toast.error(xhr.responseJSON?.message || 'Error al cancelar asignacion');
                }
            });
        }
    });
}

function abrirAsignarTecnico() {
    AppModal.open({
        title: '<i class="fas fa-hard-hat me-2"></i>Asignar Tecnico',
        url: URLS.asignarTecnicoPartial,
        data: { osId: OS_ID },
        size: 'md'
    });
}

function confirmarAsignarTecnico() {
    const tecnicoId = parseInt($('#AsignarTecnicoId').val());
    if (!tecnicoId) {
        Toast.error('Seleccione un tecnico');
        return;
    }

    const osServicioId = $('#AsignarServicioId').val() ? parseInt($('#AsignarServicioId').val()) : null;
    if (!osServicioId) {
        Toast.error('Seleccione un servicio');
        return;
    }

    const data = {
        osId: OS_ID,
        tecnicoId: tecnicoId,
        osServicioId: osServicioId,
        observaciones: $('#AsignarObservaciones').val() || null
    };

    const btn = document.getElementById('btnConfirmarAsignarTecnico');
    const btnOriginal = btn.innerHTML;

    $.ajax({
        url: URLS.postAsignarTecnico,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: () => $(btn).prop('disabled', true).html('<i class="fas fa-spinner fa-spin me-1"></i> Asignando...'),
        success: function (r) {
            if (r.success) {
                AppModal.close();
                Toast.success('Tecnico asignado exitosamente');
                cargarAsignaciones();
                cargarServicios();
            } else {
                Toast.error(r.message || 'Error al asignar tecnico');
            }
        },
        error: function (xhr) {
            Toast.error(xhr.responseJSON?.message || 'Error al asignar tecnico');
        },
        complete: () => $(btn).prop('disabled', false).html(btnOriginal)
    });
}

// ═══════════════════════════════════════════
// COMANDOS OS
// ═══════════════════════════════════════════

function abrirCambiarEstado() {
    AppModal.open({
        title: '<i class="fas fa-exchange-alt me-2"></i>Cambiar Estado',
        url: URLS.cambiarEstadoPartial,
        data: { osId: OS_ID },
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
        success: function (r) {
            if (r.success) {
                AppModal.close();
                Toast.success('Estado actualizado correctamente');
                location.reload();
            } else {
                Toast.error(r.message || 'Error al cambiar estado');
            }
        },
        error: function (xhr) {
            Toast.error(xhr.responseJSON?.message || 'Error al cambiar estado');
        },
        complete: () => $(btn).prop('disabled', false).html(btnOriginal)
    });
}

function cancelarOs() {
    AppAlert.input({
        title: 'Cancelar Orden',
        label: 'Motivo de cancelacion',
        placeholder: 'Ingrese el motivo...',
        inputType: 'textarea',
        onConfirm: function (motivo) {
            if (!motivo || !motivo.trim()) {
                Toast.error('Debe ingresar un motivo');
                return;
            }

            $.ajax({
                url: URLS.postCancelar,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ osId: OS_ID, motivo: motivo.trim() }),
                success: function (r) {
                    if (r.success) {
                        Toast.success('Orden cancelada');
                        location.reload();
                    } else {
                        Toast.error(r.message || 'Error al cancelar');
                    }
                },
                error: function (xhr) {
                    Toast.error(xhr.responseJSON?.message || 'Error al cancelar');
                }
            });
        }
    });
}

function cerrarOs() {
    AppAlert.input({
        title: 'Cerrar Orden',
        label: 'Observaciones de cierre (opcional)',
        placeholder: 'Observaciones...',
        required: false,
        onConfirm: function (obs) {
            $.ajax({
                url: URLS.postCerrar,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ osId: OS_ID, observacionesCierre: obs || null, proximaRevision: null }),
                success: function (r) {
                    if (r.success) {
                        Toast.success('Orden cerrada exitosamente');
                        location.reload();
                    } else {
                        Toast.error(r.message || 'Error al cerrar');
                    }
                },
                error: function (xhr) {
                    Toast.error(xhr.responseJSON?.message || 'Error al cerrar');
                }
            });
        }
    });
}

function recalcularTotales() {
    $.ajax({
        url: URLS.postRecalcular,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(OS_ID),
        success: function (r) {
            if (r.success) {
                Toast.success('Totales recalculados');
                refrescarDetalle();
            } else {
                Toast.error(r.message || 'Error al recalcular');
            }
        },
        error: function (xhr) {
            Toast.error(xhr.responseJSON?.message || 'Error al recalcular');
        }
    });
}

// ═══════════════════════════════════════════
// UTILIDADES
// ═══════════════════════════════════════════

function refrescarDetalle() {
    $.get(URLS.getDetalle, { osId: OS_ID }, function (r) {
        if (r.success && r.data) {
            const d = r.data;
            $('#totalManoObra').text('L ' + d.totalManoObra.toLocaleString('es-HN', { minimumFractionDigits: 2 }));
            $('#totalRepuestos').text('L ' + d.totalRepuestos.toLocaleString('es-HN', { minimumFractionDigits: 2 }));
            $('#totalGeneral').text('L ' + d.totalGeneral.toLocaleString('es-HN', { minimumFractionDigits: 2 }));
            $('#countServicios').text(d.cantidadServicios);
            $('#countTecnicos').text(d.cantidadTecnicos);
            $('#countEvidencias').text(d.cantidadEvidencias);
            $('#evidenciasCount').text(d.cantidadEvidencias);
        }
    });
}

function actualizarEvidenciasCount() {
    // Se actualiza via refrescarDetalle, pero init desde el model
    const count = $('#countEvidencias').text();
    $('#evidenciasCount').text(count);
}
