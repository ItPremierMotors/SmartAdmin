// Capacidad del Taller - Page JS
// Requiere: URLS (definido en la vista), FullCalendar, AppModal, AppAlert, Toast

let calendar;
let _fechaActualDetalle = null;

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
            // Restaurar seleccion de localStorage o usar la primera
            const saved = localStorage.getItem('sucursalCapacidad');
            if (saved && $sel.find(`option[value="${saved}"]`).length) {
                $sel.val(saved);
            }
        } else {
            $sel.empty().append('<option value="">Sin sucursales</option>');
        }
        initCalendar();
    }).fail(function () {
        $('#filtroSucursal').empty().append('<option value="">Error</option>');
        initCalendar();
    });

    $('#filtroSucursal').on('change', function () {
        localStorage.setItem('sucursalCapacidad', $(this).val());
        calendar.refetchEvents();
    });
}

// ─── Calendar ───

function initCalendar() {
    const calendarEl = document.getElementById('calendario');
    calendar = new FullCalendar.Calendar(calendarEl, {
        locale: 'es',
        initialView: 'dayGridMonth',
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: ''
        },
        height: 'auto',
        selectable: true,
        dateClick: function (info) {
            abrirDetalleDia(info.dateStr);
        },
        events: function (fetchInfo, successCallback, failureCallback) {
            const params = {
                fechaInicio: fetchInfo.startStr.split('T')[0],
                fechaFin: fetchInfo.endStr.split('T')[0]
            };
            const sId = getSucursalId();
            if (sId) params.sucursalId = sId;
            $.get(URLS.getRango, params, function (response) {
                if (response.success && response.data) {
                    const events = response.data.map(function (c) {
                        const pct = c.porcentajeOcupacion;
                        let cssClass = 'ocupacion-baja';
                        if (!c.permiteAgendamiento) cssClass = 'bloqueado';
                        else if (pct >= 90) cssClass = 'ocupacion-llena';
                        else if (pct >= 70) cssClass = 'ocupacion-alta';
                        else if (pct >= 40) cssClass = 'ocupacion-media';

                        const horas = Math.floor(c.minutosDisponibles / 60);
                        return {
                            id: c.capacidadId,
                            start: c.fecha.split('T')[0],
                            title: pct.toFixed(0) + '% | ' + horas + 'h | ' + c.tecnicosDisponibles + 'T',
                            classNames: ['capacidad-event', cssClass],
                            extendedProps: c
                        };
                    });
                    successCallback(events);
                } else {
                    successCallback([]);
                }
                $('#loading').addClass('d-none');
            }).fail(function () {
                failureCallback();
                $('#loading').addClass('d-none');
            });
        }
    });
    calendar.render();
    $('#loading').addClass('d-none');
}

// ─── Detalle Dia ───

function abrirDetalleDia(fecha) {
    _fechaActualDetalle = fecha;
    const data = { fecha: fecha };
    const sId = getSucursalId();
    if (sId) data.sucursalId = sId;
    AppModal.open({
        title: '<i class="fas fa-calendar-day me-2"></i>Capacidad — ' + formatearFecha(fecha),
        url: URLS.detalleDia,
        data: data,
        size: 'lg'
    });
}

function _recargarDetalleDia() {
    if (_fechaActualDetalle) abrirDetalleDia(_fechaActualDetalle);
    calendar.refetchEvents();
}

// ─── Generar Semana ───

function abrirGenerarSemana() {
    AppModal.open({
        title: '<i class="fas fa-bolt me-2"></i>Generar Capacidad Semanal',
        url: URLS.generarSemana,
        size: 'md'
    });
}

function confirmarGenerarSemana() {
    const btn = document.getElementById('btnGenerarSemana');
    const btnOriginal = btn.innerHTML;
    const form = document.getElementById('formGenerarSemana');
    if (!form.checkValidity()) { form.reportValidity(); return; }

    const data = {
        fechaInicio: $('#SemanaFechaInicio').val(),
        plantilla: {
            fecha: $('#SemanaFechaInicio').val(),
            turno: parseInt($('#SemanaTurno').val()),
            tecnicosDisponibles: parseInt($('#SemanaTecnicos').val()),
            bahiasDisponibles: parseInt($('#SemanaBahias').val()),
            minutosDisponibles: parseInt($('#SemanaMinutos').val()),
            permiteSobretiempo: $('#SemanaPermiteSobretiempo').is(':checked'),
            permiteAgendamiento: true,
            sucursalId: getSucursalId() ? parseInt(getSucursalId()) : null
        }
    };

    $.ajax({
        url: URLS.postGenerarSemana,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: () => $(btn).prop('disabled', true).html('<i class="fas fa-spinner fa-spin me-1"></i> Generando...'),
        success: function (response) {
            if (response.success) {
                AppModal.close();
                calendar.refetchEvents();
                Toast.success(response.message || 'Semana generada exitosamente');
            } else {
                Toast.error(response.message || 'Error al generar');
            }
        },
        error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error al generar semana'); },
        complete: () => $(btn).prop('disabled', false).html(btnOriginal)
    });
}

// ─── Generar Bloques ───

function abrirGenerarBloques(capacidadId) {
    AppModal.open({
        title: '<i class="fas fa-th me-2"></i>Generar Bloques Horarios',
        url: URLS.generarBloques,
        data: { capacidadId: capacidadId },
        size: 'md'
    });
}

function confirmarGenerarBloques() {
    const btn = document.getElementById('btnGenerarBloques');
    const btnOriginal = btn.innerHTML;
    const form = document.getElementById('formGenerarBloques');
    if (!form.checkValidity()) { form.reportValidity(); return; }

    const data = {
        capacidadId: parseInt($('#BloquesCapacidadId').val()),
        horaInicioJornada: $('#BloquesHoraInicio').val() + ':00',
        horaFinJornada: $('#BloquesHoraFin').val() + ':00',
        duracionBloqueMinutos: parseInt($('#BloquesDuracion').val()),
        capacidadPorBloque: parseInt($('#BloquesCapacidad').val()),
        tipoBloque: parseInt($('#BloquesTipo').val())
    };

    $.ajax({
        url: URLS.postGenerarBloques,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: () => $(btn).prop('disabled', true).html('<i class="fas fa-spinner fa-spin me-1"></i> Generando...'),
        success: function (response) {
            if (response.success) {
                Toast.success(response.message || 'Bloques generados exitosamente');
                _recargarDetalleDia();
            } else {
                Toast.error(response.message || 'Error al generar bloques');
            }
        },
        error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error al generar bloques'); },
        complete: () => $(btn).prop('disabled', false).html(btnOriginal)
    });
}

// ─── Crear Capacidad (desde _DetalleDiaPartial cuando no existe) ───

function confirmarCrearCapacidad() {
    const btn = document.getElementById('btnCrearCapacidad');
    const btnOriginal = btn.innerHTML;
    const form = document.getElementById('formCrearCapacidad');
    if (!form.checkValidity()) { form.reportValidity(); return; }

    const data = {
        fecha: $('#NuevaCapFecha').val(),
        turno: parseInt($('#NuevaCapTurno').val()),
        tecnicosDisponibles: parseInt($('#NuevaCapTecnicos').val()),
        bahiasDisponibles: parseInt($('#NuevaCapBahias').val()),
        minutosDisponibles: parseInt($('#NuevaCapMinutos').val()),
        permiteSobretiempo: $('#NuevaCapSobretiempo').is(':checked'),
        permiteAgendamiento: true,
        sucursalId: getSucursalId() ? parseInt(getSucursalId()) : null
    };

    $.ajax({
        url: URLS.postCreate,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: () => $(btn).prop('disabled', true).html('<i class="fas fa-spinner fa-spin me-1"></i> Creando...'),
        success: function (response) {
            if (response.success) {
                AppModal.close();
                calendar.refetchEvents();
                Toast.success('Capacidad creada exitosamente');
            } else {
                Toast.error(response.message || 'Error al crear');
            }
        },
        error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error al crear capacidad'); },
        complete: () => $(btn).prop('disabled', false).html(btnOriginal)
    });
}

// ─── Bloquear / Desbloquear Dia ───

function bloquearDia(capacidadId) {
    AppAlert.input({
        title: 'Bloquear Dia',
        label: 'Motivo del bloqueo',
        placeholder: 'Escriba el motivo...',
        inputType: 'textarea',
        onConfirm: function (motivo) {
            $.ajax({
                url: URLS.postBloquear,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify({ capacidadId: capacidadId, motivo: motivo }),
                success: function (response) {
                    if (response.success) { AppModal.close(); calendar.refetchEvents(); Toast.success('Dia bloqueado'); }
                    else { Toast.error(response.message); }
                },
                error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error'); }
            });
        }
    });
}

function desbloquearDia(capacidadId) {
    AppAlert.confirm({
        title: 'Desbloquear Dia',
        text: 'Se habilitara el agendamiento para este dia.',
        icon: 'info',
        confirmText: 'Si, desbloquear',
        onConfirm: function () {
            $.ajax({
                url: URLS.postDesbloquear,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(capacidadId),
                success: function (response) {
                    if (response.success) { AppModal.close(); calendar.refetchEvents(); Toast.success('Dia desbloqueado'); }
                    else { Toast.error(response.message); }
                },
                error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error'); }
            });
        }
    });
}

// ─── Editar Capacidad ───

function abrirEditarCapacidad(capacidadId) {
    AppModal.open({
        title: '<i class="fas fa-edit me-2"></i>Editar Capacidad',
        url: URLS.editPartial,
        data: { capacidadId: capacidadId },
        size: 'md'
    });
}

function confirmarEditarCapacidad() {
    const btn = document.getElementById('btnEditarCapacidad');
    const btnOriginal = btn.innerHTML;
    const form = document.getElementById('formEditarCapacidad');
    if (!form.checkValidity()) { form.reportValidity(); return; }

    const data = {
        capacidadId: parseInt($('#EditCapacidadId').val()),
        turno: parseInt($('#EditTurno').val()),
        tecnicosDisponibles: parseInt($('#EditTecnicos').val()),
        bahiasDisponibles: parseInt($('#EditBahias').val()),
        minutosDisponibles: parseInt($('#EditMinutos').val()),
        permiteSobretiempo: $('#EditPermiteSobretiempo').is(':checked'),
        permiteAgendamiento: $('#EditPermiteAgendamiento').is(':checked'),
        observaciones: $('#EditObservaciones').val() || null
    };

    $.ajax({
        url: URLS.postUpdate,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: () => $(btn).prop('disabled', true).html('<i class="fas fa-spinner fa-spin me-1"></i> Guardando...'),
        success: function (response) {
            if (response.success) {
                Toast.success('Capacidad actualizada exitosamente');
                _recargarDetalleDia();
            } else {
                Toast.error(response.message || 'Error al actualizar');
            }
        },
        error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error al actualizar capacidad'); },
        complete: () => $(btn).prop('disabled', false).html(btnOriginal)
    });
}

// ─── Bloques CRUD ───

function abrirEditarBloque(bloqueId, horaInicio, horaFin, capacidad, tipo) {
    const html = `
        <form id="formEditBloque">
            <input type="hidden" id="EditBloqueId" value="${bloqueId}" />
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label class="form-label">Hora Inicio</label>
                    <input type="time" class="form-control" id="EditBloqueHoraInicio" value="${horaInicio}" required />
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label">Hora Fin</label>
                    <input type="time" class="form-control" id="EditBloqueHoraFin" value="${horaFin}" required />
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label class="form-label">Vehiculos Max</label>
                    <input type="number" class="form-control" id="EditBloqueCapacidad" min="1" max="20" value="${capacidad}" required />
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label">Tipo</label>
                    <select class="form-select" id="EditBloqueTipo" required>
                        <option value="1" ${tipo === 1 ? 'selected' : ''}>Estandar</option>
                        <option value="2" ${tipo === 2 ? 'selected' : ''}>Rapido</option>
                        <option value="3" ${tipo === 3 ? 'selected' : ''}>Colision / Pintura</option>
                    </select>
                </div>
            </div>
        </form>
        <div class="d-flex justify-content-end gap-2 pt-3 border-top">
            <button class="btn btn-secondary" data-bs-dismiss="modal"><i class="fas fa-times me-1"></i>Cancelar</button>
            <button class="btn btn-primary" id="btnGuardarBloque" onclick="confirmarEditarBloque()"><i class="fas fa-save me-1"></i>Guardar</button>
        </div>`;
    AppModal.open({
        title: '<i class="fas fa-edit me-2"></i>Editar Bloque ' + horaInicio + ' - ' + horaFin,
        html: html,
        size: 'md'
    });
}

function confirmarEditarBloque() {
    const form = document.getElementById('formEditBloque');
    if (!form.checkValidity()) { form.reportValidity(); return; }
    const btn = document.getElementById('btnGuardarBloque');
    const btnOriginal = btn.innerHTML;

    const data = {
        bloqueId: parseInt($('#EditBloqueId').val()),
        horaInicio: $('#EditBloqueHoraInicio').val() + ':00',
        horaFin: $('#EditBloqueHoraFin').val() + ':00',
        capacidadMaximaVehiculos: parseInt($('#EditBloqueCapacidad').val()),
        tipoBloque: parseInt($('#EditBloqueTipo').val())
    };

    $.ajax({
        url: URLS.postUpdateBloque,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: () => $(btn).prop('disabled', true).html('<i class="fas fa-spinner fa-spin me-1"></i> ...'),
        success: function (response) {
            if (response.success) {
                Toast.success('Bloque actualizado');
                _recargarDetalleDia();
            } else {
                Toast.error(response.message || 'Error al actualizar');
            }
        },
        error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error al actualizar bloque'); },
        complete: () => $(btn).prop('disabled', false).html(btnOriginal)
    });
}

function abrirCrearBloque(capacidadId) {
    const html = `
        <form id="formCrearBloque">
            <input type="hidden" id="NuevoBloqueCapacidadId" value="${capacidadId}" />
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label class="form-label">Hora Inicio</label>
                    <input type="time" class="form-control" id="NuevoBloqueHoraInicio" value="08:00" required />
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label">Hora Fin</label>
                    <input type="time" class="form-control" id="NuevoBloqueHoraFin" value="09:00" required />
                </div>
            </div>
            <div class="row">
                <div class="col-md-6 mb-3">
                    <label class="form-label">Vehiculos Max</label>
                    <input type="number" class="form-control" id="NuevoBloqueCapacidad" min="1" max="20" value="2" required />
                </div>
                <div class="col-md-6 mb-3">
                    <label class="form-label">Tipo</label>
                    <select class="form-select" id="NuevoBloqueTipo" required>
                        <option value="1" selected>Estandar</option>
                        <option value="2">Rapido</option>
                        <option value="3">Colision / Pintura</option>
                    </select>
                </div>
            </div>
        </form>
        <div class="d-flex justify-content-end gap-2 pt-3 border-top">
            <button class="btn btn-secondary" data-bs-dismiss="modal"><i class="fas fa-times me-1"></i>Cancelar</button>
            <button class="btn btn-success" id="btnCrearBloque" onclick="confirmarCrearBloque()"><i class="fas fa-plus me-1"></i>Crear Bloque</button>
        </div>`;
    AppModal.open({
        title: '<i class="fas fa-plus me-2"></i>Nuevo Bloque Horario',
        html: html,
        size: 'md'
    });
}

function confirmarCrearBloque() {
    const form = document.getElementById('formCrearBloque');
    if (!form.checkValidity()) { form.reportValidity(); return; }
    const btn = document.getElementById('btnCrearBloque');
    const btnOriginal = btn.innerHTML;

    const data = {
        capacidadId: parseInt($('#NuevoBloqueCapacidadId').val()),
        horaInicio: $('#NuevoBloqueHoraInicio').val() + ':00',
        horaFin: $('#NuevoBloqueHoraFin').val() + ':00',
        capacidadMaximaVehiculos: parseInt($('#NuevoBloqueCapacidad').val()),
        tipoBloque: parseInt($('#NuevoBloqueTipo').val())
    };

    $.ajax({
        url: URLS.postCreateBloque,
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(data),
        beforeSend: () => $(btn).prop('disabled', true).html('<i class="fas fa-spinner fa-spin me-1"></i> ...'),
        success: function (response) {
            if (response.success) {
                Toast.success('Bloque creado');
                _recargarDetalleDia();
            } else {
                Toast.error(response.message || 'Error al crear');
            }
        },
        error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error al crear bloque'); },
        complete: () => $(btn).prop('disabled', false).html(btnOriginal)
    });
}

function eliminarBloque(bloqueId, rangoHora) {
    AppAlert.eliminar({
        title: 'Eliminar Bloque',
        text: 'Se eliminara el bloque ' + rangoHora + '. Esta accion no se puede deshacer.',
        onConfirm: function () {
            $.ajax({
                url: URLS.postDeleteBloque,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(bloqueId),
                success: function (response) {
                    if (response.success) {
                        Toast.success('Bloque eliminado');
                        _recargarDetalleDia();
                    } else {
                        Toast.error(response.message || 'Error al eliminar');
                    }
                },
                error: function (xhr) { Toast.error(xhr.responseJSON?.message || 'Error al eliminar bloque'); }
            });
        }
    });
}

// ─── Utils ───

function formatearFecha(fechaStr) {
    const d = new Date(fechaStr + 'T12:00:00');
    return d.toLocaleDateString('es-HN', { weekday: 'long', day: '2-digit', month: '2-digit', year: 'numeric' });
}
