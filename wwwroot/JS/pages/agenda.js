// Agenda de Citas - Page JS
// Requiere: URLS (definido en la vista), AppModal, AppAlert, Toast

let todasLasCitas = [];
let filtroEstadoActual = null;

function getSucursalId() {
    return $('#filtroSucursal').val() || null;
}

$(document).ready(function () {
    cargarSucursales();

    // Si viene de cerrar OS, abrir modal de fotos de salida automáticamente
    const params = new URLSearchParams(window.location.search);
    const fotosSalidaCitaId = params.get('fotosSalida');
    if (fotosSalidaCitaId) {
        // Limpiar el parámetro de la URL sin recargar
        window.history.replaceState({}, '', window.location.pathname);
        setTimeout(function () {
            abrirEvidenciaSalida(parseInt(fotosSalidaCitaId));
        }, 500);
    }
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
                <td><code>${c.codigoCita}</code>${c.minutosTrabajados ? ' <i class="fas fa-clock text-info" title="' + c.minutosTrabajados + ' min trabajados"></i>' : ''}</td>
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

    // Verificar si la fecha de la cita ya paso
    const fechaCita = new Date(cita.fechaHoraInicio);
    fechaCita.setHours(0, 0, 0, 0);
    const hoy = new Date();
    hoy.setHours(0, 0, 0, 0);
    const esFechaPasada = fechaCita.getTime() < hoy.getTime();

    switch (cita.estado) {
        case 1: // Agendada
            if (esFechaPasada) {
                btns = `
                    <button class="btn btn-sm btn-outline-primary" onclick="abrirReprogramarCita(${cita.citaId})" title="Reprogramar">
                        <i class="fas fa-calendar-day"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" onclick="cancelarCita(${cita.citaId})" title="Cancelar">
                        <i class="fas fa-times"></i>
                    </button>`;
            } else {
                btns = `
                    <button class="btn btn-sm btn-outline-info" onclick="accionCita('confirmar', ${cita.citaId})" title="Confirmar">
                        <i class="fas fa-check"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" onclick="cancelarCita(${cita.citaId})" title="Cancelar">
                        <i class="fas fa-times"></i>
                    </button>`;
            }
            break;
        case 2: // Confirmada
            if (esFechaPasada) {
                btns = `
                    <button class="btn btn-sm btn-outline-primary" onclick="abrirReprogramarCita(${cita.citaId})" title="Reprogramar">
                        <i class="fas fa-calendar-day"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" onclick="cancelarCita(${cita.citaId})" title="Cancelar">
                        <i class="fas fa-times"></i>
                    </button>`;
            } else {
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
            }
            break;
        case 3: // En Proceso - solo transferir, completar se hace desde la OS
            btns = `
                <button class="btn btn-sm btn-outline-primary" onclick="abrirTransferirCita(${cita.citaId})" title="Transferir a manana">
                    <i class="fas fa-exchange-alt"></i>
                </button>`;
            break;
        case 4: // Completada - solo evidencia de salida si tiene OS
            if (cita.osId) {
                btns = `
                    <button class="btn btn-sm btn-outline-info" onclick="abrirEvidenciaSalida(${cita.citaId})" title="Evidencia de Salida">
                        <i class="fas fa-camera"></i>
                    </button>`;
            }
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
    // Iniciar atencion redirige al wizard de recepcion
    if (accion === 'iniciar') {
        window.location.href = URLS.wizardRecepcion + '?citaId=' + citaId;
        return;
    }

    let url, msg;
    switch (accion) {
        case 'confirmar': url = URLS.postConfirmar; msg = 'Cita confirmada'; break;
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

// ─── Evidencia de Salida ───

let fotosSalidaData = {};
let fotosSalidaInitialized = false;

function abrirEvidenciaSalida(citaId) {
    fotosSalidaData = {};
    fotosSalidaInitialized = false;
    AppModal.open({
        title: '<i class="fas fa-camera me-2"></i>Evidencia de Salida',
        url: URLS.completarPartial,
        data: { citaId: citaId },
        size: 'lg',
        onShown: function () {
            initFotosSalida();
        }
    });
}

function initFotosSalida() {
    // Evitar doble inicializacion
    if (fotosSalidaInitialized) return;

    // Reintentar si el contenido AJAX aun no cargo
    var btn = document.getElementById('btnCompletarConFotos');
    if (!btn) {
        setTimeout(initFotosSalida, 200);
        return;
    }

    fotosSalidaInitialized = true;

    btn.innerHTML = '<i class="fas fa-camera me-1"></i> Guardar Evidencia';
    btn.className = 'btn btn-info';

    document.querySelectorAll('.foto-salida-dropzone').forEach(function (zone) {
        var tipo = zone.dataset.tipo;
        var input = zone.querySelector('.foto-salida-input');

        zone.addEventListener('click', function (e) {
            // No abrir file dialog si se hizo click en el boton remover
            if (e.target.closest('.foto-remove')) return;
            // No abrir si el click viene del input (evita doble trigger)
            if (e.target === input) return;
            input.click();
        });

        input.addEventListener('change', function () {
            if (!input.files || input.files.length === 0) return;
            var file = input.files[0];
            if (!file.type.startsWith('image/')) return;
            var reader = new FileReader();
            reader.onload = function (ev) {
                fotosSalidaData[tipo] = { base64: ev.target.result, nombre: file.name };
                renderFotoSalidaPreview(tipo);
            };
            reader.readAsDataURL(file);
            input.value = '';
        });
    });

    btn.addEventListener('click', subirSoloEvidencia);
}

function renderFotoSalidaPreview(tipo) {
    const container = document.querySelector(`.foto-salida-preview[data-tipo="${tipo}"]`);
    const zone = document.querySelector(`.foto-salida-dropzone[data-tipo="${tipo}"]`);
    if (!container || !zone) return;

    const foto = fotosSalidaData[tipo];
    if (foto) {
        container.innerHTML = `
            <div class="position-relative d-inline-block">
                <img src="${foto.base64}" class="img-fluid rounded" style="max-height:80px;" />
                <button type="button" class="btn btn-sm btn-danger position-absolute top-0 end-0 foto-remove"
                        onclick="removerFotoSalida('${tipo}')" style="padding:1px 5px; font-size:10px;">
                    <i class="fas fa-times"></i>
                </button>
            </div>`;
        zone.querySelector('i').className = 'fas fa-check-circle fa-2x text-success mb-1';
        zone.querySelector('small').textContent = 'Capturada';
    } else {
        container.innerHTML = '';
        zone.querySelector('i').className = 'fas fa-camera fa-2x text-muted mb-1';
        zone.querySelector('small').textContent = tipo.replace('FotoSalida', '');
    }
}

function removerFotoSalida(tipo) {
    delete fotosSalidaData[tipo];
    renderFotoSalidaPreview(tipo);
}

async function subirSoloEvidencia() {
    const btn = document.getElementById('btnCompletarConFotos');
    const osId = parseInt(document.getElementById('completarOsId').value);

    if (!osId) {
        Toast.error('Esta cita no tiene una Orden de Servicio asociada.');
        return;
    }

    const totalFotos = Object.keys(fotosSalidaData).length;
    if (totalFotos === 0) {
        Toast.error('Debe capturar al menos una foto de salida.');
        return;
    }

    btn.disabled = true;
    btn.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span>Subiendo fotos...';

    try {
        let fotosSubidas = 0;
        for (const [tipo, foto] of Object.entries(fotosSalidaData)) {
            try {
                const response = await fetch(URLS.subirEvidencia, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify({
                        osId: osId,
                        recepcionId: null,
                        tipoEvidencia: tipo,
                        base64Data: foto.base64,
                        nombreArchivo: foto.nombre,
                        descripcion: 'Foto de salida - ' + tipo.replace('FotoSalida', '')
                    })
                });
                const result = await response.json();
                if (result.success) fotosSubidas++;
                else console.warn('Error subiendo foto ' + tipo + ':', result.message);
            } catch (err) {
                console.warn('Error subiendo foto ' + tipo + ':', err);
            }
        }

        if (fotosSubidas === 0) {
            Toast.error('No se pudieron subir las fotos. Intente de nuevo.');
        } else {
            AppModal.close();
            Toast.success(fotosSubidas + ' foto(s) de salida guardada(s)');
        }
    } catch (error) {
        console.error('Error:', error);
        Toast.error('Error inesperado al subir evidencia.');
    } finally {
        btn.disabled = false;
        btn.innerHTML = '<i class="fas fa-camera me-1"></i> Guardar Evidencia';
    }
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

// ─── Reprogramar ───

function abrirReprogramarCita(citaId) {
    AppModal.open({
        title: '<i class="fas fa-calendar-day me-2"></i>Reprogramar Cita',
        url: URLS.reprogramarPartial,
        data: { citaId: citaId },
        size: 'lg'
    });
}

function confirmarReprogramar() {
    const form = document.getElementById('formReprogramarCita');
    if (!form.checkValidity()) { form.reportValidity(); return; }

    const btn = document.getElementById('btnReprogramar');
    const btnOriginal = btn.innerHTML;
    const citaId = parseInt($('#ReprogramarCitaId').val());
    const fecha = $('#ReprogramarFecha').val();
    const $bloqueOption = $('#ReprogramarBloqueHorario option:selected');
    const bloqueId = $('#ReprogramarBloqueHorario').val();
    let fechaHoraInicio;
    let bloqueHorarioId = null;

    if (bloqueId && bloqueId !== '__manual__' && bloqueId !== '') {
        const horaBloque = $bloqueOption.data('hora');
        fechaHoraInicio = fecha + 'T' + horaBloque;
        bloqueHorarioId = parseInt(bloqueId);
    } else {
        fechaHoraInicio = fecha + 'T' + ($('#ReprogramarHoraManual').val() || '08:00') + ':00';
    }

    const data = {
        citaId: citaId,
        fechaHoraInicio: fechaHoraInicio,
        motivoVisita: $('#ReprogramarMotivoVisita').val(),
        observaciones: $('#ReprogramarObservaciones').val() || null,
        bloqueHorarioId: bloqueHorarioId
    };

    $.ajax({
        url: URLS.postReprogramar,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: () => $(btn).prop('disabled', true).html('<i class="fas fa-spinner fa-spin me-1"></i> Reprogramando...'),
        success: function (response) {
            if (response.success) {
                AppModal.close();
                cargarCitas();
                Toast.success('Cita reprogramada exitosamente');
            } else {
                Toast.error(response.message || 'Error al reprogramar');
            }
        },
        error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error al reprogramar cita'); },
        complete: () => $(btn).prop('disabled', false).html(btnOriginal)
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
