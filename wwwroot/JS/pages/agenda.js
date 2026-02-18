// Agenda de Citas - Page JS
// Requiere: URLS (definido en la vista), AppModal, AppAlert, Toast

let todasLasCitas = [];
let filtroEstadoActual = null;

function getSucursalId() {
    return $('#filtroSucursal').val() || null;
}

$(document).ready(function () {
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
            const saved = localStorage.getItem('sucursalAgenda');
            if (saved && $sel.find(`option[value="${saved}"]`).length) {
                $sel.val(saved);
            }
        } else {
            $sel.empty().append('<option value="">Sin sucursales</option>');
        }
        actualizarFechaTitulo();
        cargarCitas();
    }).fail(function () {
        $('#filtroSucursal').empty().append('<option value="">Error</option>');
        actualizarFechaTitulo();
        cargarCitas();
    });

    $('#filtroSucursal').on('change', function () {
        localStorage.setItem('sucursalAgenda', $(this).val());
        cargarCitas();
    });

    $('#fechaSeleccionada').on('change', function () {
        actualizarFechaTitulo();
        cargarCitas();
    });
}

// ─── Data ───

function actualizarFechaTitulo() {
    const fecha = $('#fechaSeleccionada').val();
    const d = new Date(fecha + 'T12:00:00');
    $('#fechaTitulo').text(d.toLocaleDateString('es-HN', { weekday: 'long', day: '2-digit', month: 'long', year: 'numeric' }));
}

function cargarCitas() {
    const fecha = $('#fechaSeleccionada').val();
    $('#loadingCitas').removeClass('d-none');
    $('#sinCitas').addClass('d-none');
    $('#citasBody').empty();

    const params = { fecha: fecha };
    const sId = getSucursalId();
    if (sId) params.sucursalId = sId;

    $.get(URLS.getByFecha, params, function (response) {
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

// ─── Render ───

function renderizarCitas() {
    const citas = filtroEstadoActual
        ? todasLasCitas.filter(c => c.estado === filtroEstadoActual)
        : todasLasCitas;

    const tbody = $('#citasBody');
    tbody.empty();

    if (citas.length === 0) {
        tbody.append('<tr><td colspan="7" class="text-center text-muted py-3">No hay citas con este filtro</td></tr>');
        return;
    }

    citas.sort((a, b) => new Date(a.fechaHoraInicio) - new Date(b.fechaHoraInicio));

    citas.forEach(function (c) {
        const badgeClass = obtenerBadgeEstado(c.estado);
        const estadoNombre = obtenerNombreEstado(c.estado);
        const acciones = obtenerAcciones(c);

        const row = `
            <tr class="cita-row" ondblclick="abrirDetalleCita(${c.citaId})">
                <td>
                    <i class="fas fa-clock me-1 text-muted"></i>
                    <strong>${c.horaInicioFormateada}</strong> - ${c.horaFinFormateada}
                </td>
                <td><code>${c.codigoCita}</code>${c.esTransferencia ? ' <i class="fas fa-exchange-alt text-info" title="Transferida"></i>' : ''}</td>
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
                <td class="text-center">${acciones}</td>
            </tr>`;
        tbody.append(row);
    });
}

function actualizarStats() {
    const total = todasLasCitas.length;
    const agendadas = todasLasCitas.filter(c => c.estado === 1).length;
    const confirmadas = todasLasCitas.filter(c => c.estado === 2).length;
    const enProceso = todasLasCitas.filter(c => c.estado === 3).length;
    const completadas = todasLasCitas.filter(c => c.estado === 4).length;
    const canceladas = todasLasCitas.filter(c => c.estado === 5 || c.estado === 6).length;

    $('#statTotal').text(total);
    $('#statAgendadas').text(agendadas);
    $('#statConfirmadas').text(confirmadas);
    $('#statEnProceso').text(enProceso);
    $('#statCompletadas').text(completadas);
    $('#statCanceladas').text(canceladas);
}

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

function obtenerAcciones(cita) {
    let btns = '';
    switch (cita.estado) {
        case 1: // Agendada
            btns = `
                <button class="btn btn-sm btn-outline-info" onclick="accionCita('confirmar', ${cita.citaId})" title="Confirmar">
                    <i class="fas fa-check"></i>
                </button>
                <button class="btn btn-sm btn-outline-danger" onclick="cancelarCita(${cita.citaId})" title="Cancelar">
                    <i class="fas fa-times"></i>
                </button>`;
            break;
        case 2: // Confirmada
            btns = `
                <button class="btn btn-sm btn-outline-warning" onclick="accionCita('iniciar', ${cita.citaId})" title="Iniciar Atencion">
                    <i class="fas fa-play"></i>
                </button>
                <button class="btn btn-sm btn-outline-dark" onclick="accionCita('noshow', ${cita.citaId})" title="No Show">
                    <i class="fas fa-user-slash"></i>
                </button>
                <button class="btn btn-sm btn-outline-danger" onclick="cancelarCita(${cita.citaId})" title="Cancelar">
                    <i class="fas fa-times"></i>
                </button>`;
            break;
        case 3: // En Proceso
            btns = `
                <button class="btn btn-sm btn-outline-success" onclick="accionCita('completar', ${cita.citaId})" title="Completar">
                    <i class="fas fa-check-double"></i>
                </button>
                <button class="btn btn-sm btn-outline-primary" onclick="abrirTransferirCita(${cita.citaId})" title="Transferir a manana">
                    <i class="fas fa-exchange-alt"></i>
                </button>`;
            break;
    }

    btns += `
        <button class="btn btn-sm btn-outline-secondary ms-1" onclick="abrirDetalleCita(${cita.citaId})" title="Ver Detalle">
            <i class="fas fa-eye"></i>
        </button>`;
    return `<div class="btn-group">${btns}</div>`;
}

function filtrarEstado(estado, btn) {
    filtroEstadoActual = estado;
    $('.btn-group .btn').removeClass('active');
    $(btn).addClass('active');
    renderizarCitas();
}

// ─── Modals ───

function abrirAgendarCita() {
    const fecha = $('#fechaSeleccionada').val();
    const hoy = new Date().toISOString().split('T')[0];
    if (fecha < hoy) {
        Toast.error('No se pueden agendar citas en fechas pasadas');
        return;
    }
    const data = { fecha: fecha };
    const sId = getSucursalId();
    if (sId) data.sucursalId = sId;
    AppModal.open({
        title: '<i class="fas fa-calendar-plus me-2"></i>Agendar Nueva Cita',
        url: URLS.agendarPartial,
        data: data,
        size: 'lg'
    });
}

function abrirDetalleCita(citaId) {
    AppModal.open({
        title: '<i class="fas fa-calendar-check me-2"></i>Detalle de Cita',
        url: URLS.detallePartial,
        data: { citaId: citaId },
        size: 'lg'
    });
}

// ─── CRUD ───

function confirmarAgendar() {
    const btn = document.getElementById('btnAgendar');
    const btnOriginal = btn.innerHTML;
    const form = document.getElementById('formAgendarCita');
    if (!form.checkValidity()) { form.reportValidity(); return; }

    const fecha = $('#CitaFecha').val();
    const $bloqueOption = $('#CitaBloqueHorario option:selected');
    const bloqueId = $('#CitaBloqueHorario').val();
    let fechaHoraInicio;
    let bloqueHorarioId = null;

    if (bloqueId && bloqueId !== '__manual__') {
        const horaBloque = $bloqueOption.data('hora');
        fechaHoraInicio = fecha + 'T' + horaBloque;
        bloqueHorarioId = parseInt(bloqueId);
    } else {
        fechaHoraInicio = fecha + 'T' + ($('#CitaHoraManual').val() || '08:00') + ':00';
    }

    const data = {
        clienteId: parseInt($('#CitaClienteId').val()),
        vehiculoId: parseInt($('#CitaVehiculoId').val()),
        tipoServicioId: parseInt($('#CitaTipoServicioId').val()),
        fechaHoraInicio: fechaHoraInicio,
        tipoIngreso: parseInt($('#CitaTipoIngreso').val()),
        motivoVisita: $('#CitaMotivoVisita').val(),
        observaciones: $('#CitaObservaciones').val() || null,
        sucursalId: getSucursalId() ? parseInt(getSucursalId()) : null,
        bloqueHorarioId: bloqueHorarioId
    };

    $.ajax({
        url: URLS.postAgendar,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: () => $(btn).prop('disabled', true).html('<i class="fas fa-spinner fa-spin me-1"></i> Agendando...'),
        success: function (response) {
            if (response.success) {
                AppModal.close();
                cargarCitas();
                Toast.success('Cita agendada exitosamente — ' + (response.data?.codigoCita || ''));
            } else {
                Toast.error(response.message || 'Error al agendar');
            }
        },
        error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error al agendar cita'); },
        complete: () => $(btn).prop('disabled', false).html(btnOriginal)
    });
}

function accionCita(accion, citaId) {
    let url, msg;
    switch (accion) {
        case 'confirmar': url = URLS.postConfirmar; msg = 'Cita confirmada'; break;
        case 'iniciar': url = URLS.postIniciar; msg = 'Atencion iniciada'; break;
        case 'completar': url = URLS.postCompletar; msg = 'Cita completada'; break;
        case 'noshow': url = URLS.postNoShow; msg = 'Marcada como No Show'; break;
        default: return;
    }

    $.ajax({
        url: url,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(citaId),
        success: function (response) {
            if (response.success) {
                cargarCitas();
                AppModal.close();
                Toast.success(msg);
            } else {
                Toast.error(response.message || 'Error');
            }
        },
        error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error en la operacion'); }
    });
}

function cancelarCita(citaId) {
    AppAlert.input({
        title: 'Cancelar Cita',
        label: 'Motivo de cancelacion',
        placeholder: 'Escriba el motivo...',
        inputType: 'textarea',
        onConfirm: function (motivo) {
            $.ajax({
                url: URLS.postCancelar,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ citaId: citaId, motivoCancelacion: motivo }),
                success: function (response) {
                    if (response.success) {
                        cargarCitas();
                        AppModal.close();
                        Toast.success('Cita cancelada');
                    } else {
                        Toast.error(response.message || 'Error al cancelar');
                    }
                },
                error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error al cancelar'); }
            });
        }
    });
}

// ─── Transferir ───

function abrirTransferirCita(citaId) {
    AppModal.open({
        title: '<i class="fas fa-exchange-alt me-2"></i>Transferir Cita a Manana',
        url: URLS.transferirPartial,
        data: { citaId: citaId },
        size: 'md'
    });
}

function confirmarTransferirCita() {
    const form = document.getElementById('formTransferirCita');
    if (!form.checkValidity()) { form.reportValidity(); return; }

    const btn = document.getElementById('btnTransferir');
    const btnOriginal = btn.innerHTML;
    const citaId = parseInt($('#TransferirCitaId').val());
    const minutosTrabajadosHoy = parseInt($('#TransferirMinutosTrabajados').val());

    $.ajax({
        url: URLS.postTransferir,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({ citaId: citaId, minutosTrabajadosHoy: minutosTrabajadosHoy }),
        beforeSend: () => $(btn).prop('disabled', true).html('<i class="fas fa-spinner fa-spin me-1"></i> Transfiriendo...'),
        success: function (response) {
            if (response.success) {
                AppModal.close();
                cargarCitas();
                Toast.success(response.message || 'Cita transferida a manana');
            } else {
                Toast.error(response.message || 'Error al transferir');
            }
        },
        error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error al transferir cita'); },
        complete: () => $(btn).prop('disabled', false).html(btnOriginal)
    });
}
