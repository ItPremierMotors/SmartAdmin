/**
 * recepcion-wizard.js
 * Controla el wizard de recepcion de vehiculo (5 pasos)
 * Depende de: vehicle-diagram.js, fuel-gauge.js, signature_pad, Toast, WIZARD_DATA, URLS
 */
(function () {
    'use strict';

    const TOTAL_STEPS = 5;
    let currentStep = 1;
    let signaturePad = null;
    let fotosData = {}; // { tipo: [{base64, nombre}] }

    // ===================== INIT =====================
    document.addEventListener('DOMContentLoaded', () => {
        initStep1();
        initStep2();
        initStep4();
        initNavigation();
        initDanoModal();
    });

    // ===================== STEP 1: ENTREGA =====================
    function initStep1() {
        const toggle = document.getElementById('esPropietario');
        const camposProp = document.getElementById('camposPropietario');
        const camposNoProp = document.getElementById('camposNoPropietario');
        const hiddenEntregado = document.getElementById('entregadoPor');

        function actualizarEntrega() {
            if (toggle.checked) {
                // Es propietario: mostrar datos del cliente, ocultar campos tercero
                camposProp.style.display = '';
                camposNoProp.style.display = 'none';
                hiddenEntregado.value = WIZARD_DATA.clienteNombre;
            } else {
                // No es propietario: ocultar datos cliente, mostrar campos tercero
                camposProp.style.display = 'none';
                camposNoProp.style.display = '';
                hiddenEntregado.value = document.getElementById('entregadoPorTercero').value;
            }
        }

        if (toggle) {
            toggle.addEventListener('change', actualizarEntrega);
            actualizarEntrega();
        }

        // Sincronizar campo tercero con hidden
        const terceroInput = document.getElementById('entregadoPorTercero');
        if (terceroInput) {
            terceroInput.addEventListener('input', () => {
                if (!toggle.checked) {
                    hiddenEntregado.value = terceroInput.value;
                }
            });
        }

        // Fuel gauge
        FuelGauge.init('fuelGaugeContainer', 50);
    }

    // ===================== STEP 2: EXTERIOR =====================
    function initStep2() {
        VehicleDiagram.init('vehicleDiagramContainer', WIZARD_DATA.segmentoVehiculo);
    }

    function initDanoModal() {
        const btnConfirmar = document.getElementById('btnConfirmarDano');
        if (btnConfirmar) {
            btnConfirmar.addEventListener('click', () => {
                const tipo = document.getElementById('tipoDano').value;
                const desc = document.getElementById('descripcionDano').value;
                VehicleDiagram.confirmPendingMarker(tipo, desc);
                bootstrap.Modal.getInstance(document.getElementById('modalDano')).hide();
            });
        }
    }

    // ===================== STEP 4: FOTOS =====================
    function initStep4() {
        document.querySelectorAll('.foto-dropzone').forEach(zone => {
            const tipo = zone.dataset.tipo;
            const isMultiple = zone.dataset.multiple === 'true';
            const input = zone.querySelector('.foto-input');

            // Click para abrir file dialog
            zone.addEventListener('click', (e) => {
                if (e.target.closest('.foto-remove')) return;
                input.click();
            });

            // Drag & drop
            zone.addEventListener('dragover', (e) => { e.preventDefault(); zone.classList.add('drag-over'); });
            zone.addEventListener('dragleave', () => zone.classList.remove('drag-over'));
            zone.addEventListener('drop', (e) => {
                e.preventDefault();
                zone.classList.remove('drag-over');
                handleFiles(tipo, e.dataTransfer.files, isMultiple);
            });

            // File input change
            input.addEventListener('change', () => {
                handleFiles(tipo, input.files, isMultiple);
                input.value = '';
            });
        });
    }

    function handleFiles(tipo, files, isMultiple) {
        if (!files || files.length === 0) return;

        if (!isMultiple) {
            fotosData[tipo] = [];
        }
        if (!fotosData[tipo]) fotosData[tipo] = [];

        Array.from(files).forEach(file => {
            if (!file.type.startsWith('image/')) return;
            const reader = new FileReader();
            reader.onload = (e) => {
                fotosData[tipo].push({
                    base64: e.target.result,
                    nombre: file.name
                });
                renderFotoPreview(tipo);
            };
            reader.readAsDataURL(file);
        });
    }

    function renderFotoPreview(tipo) {
        const zone = document.querySelector(`.foto-dropzone[data-tipo="${tipo}"]`);
        if (!zone) return;
        const placeholder = zone.querySelector('.foto-placeholder');
        const preview = zone.querySelector('.foto-preview');
        const fotos = fotosData[tipo] || [];

        if (fotos.length === 0) {
            placeholder.style.display = '';
            preview.style.display = 'none';
            return;
        }

        placeholder.style.display = 'none';
        preview.style.display = '';
        preview.innerHTML = fotos.map((f, i) => `
            <div class="foto-item">
                <img src="${f.base64}" alt="${f.nombre}" title="${f.nombre}">
                <button type="button" class="foto-remove" onclick="event.stopPropagation(); removeFoto('${tipo}', ${i})">x</button>
            </div>
        `).join('');
    }

    // Global - llamado desde onclick
    window.removeFoto = function (tipo, index) {
        if (fotosData[tipo]) {
            fotosData[tipo].splice(index, 1);
            renderFotoPreview(tipo);
        }
    };

    // ===================== STEP 5: FIRMA =====================
    function initStep5() {
        if (signaturePad) return;
        const canvas = document.getElementById('signatureCanvas');
        if (!canvas) return;

        signaturePad = new SignaturePad(canvas, {
            backgroundColor: 'rgb(250,250,250)',
            penColor: 'rgb(0,0,0)'
        });

        // Resize handler
        function resizeCanvas() {
            const ratio = Math.max(window.devicePixelRatio || 1, 1);
            const rect = canvas.getBoundingClientRect();
            canvas.width = rect.width * ratio;
            canvas.height = rect.height * ratio;
            canvas.getContext('2d').scale(ratio, ratio);
            signaturePad.clear();
        }
        resizeCanvas();

        document.getElementById('btnLimpiarFirma').addEventListener('click', () => {
            signaturePad.clear();
        });

        // Generar resumen
        generarResumen();
    }

    function generarResumen() {
        const container = document.getElementById('resumenContainer');
        if (!container) return;

        const km = document.getElementById('kilometraje').value;
        const combustible = FuelGauge.getValue();
        const entregadoPor = document.getElementById('entregadoPor').value || '-';
        const esPropietario = document.getElementById('esPropietario').checked;
        const danos = VehicleDiagram.getMarkers();
        const checkItems = document.querySelectorAll('.checklist-item:checked').length;
        const totalCheckItems = document.querySelectorAll('.checklist-item').length;

        let fotosCount = 0;
        Object.values(fotosData).forEach(arr => fotosCount += arr.length);

        container.innerHTML = `
            <div class="mb-3">
                <h6 class="fw-bold text-primary"><i class="fas fa-handshake me-1"></i> Entrega</h6>
                <div class="small">
                    <div><strong>Entrega:</strong> ${entregadoPor} ${esPropietario ? '(Propietario)' : '(Tercero)'}</div>
                    <div><strong>Kilometraje:</strong> ${parseInt(km).toLocaleString()} km</div>
                    <div><strong>Combustible:</strong> ${combustible}%</div>
                </div>
            </div>
            <hr class="my-2">
            <div class="mb-3">
                <h6 class="fw-bold text-primary"><i class="fas fa-car-crash me-1"></i> Exterior</h6>
                <div class="small">
                    <div><strong>Danos registrados:</strong> ${danos.length}</div>
                    ${danos.map((d, i) => `<div class="text-muted">  ${i + 1}. ${VehicleDiagram.DAMAGE_NAMES[d.tipo] || 'Otro'}${d.descripcion ? ' - ' + d.descripcion : ''}</div>`).join('')}
                </div>
            </div>
            <hr class="my-2">
            <div class="mb-3">
                <h6 class="fw-bold text-primary"><i class="fas fa-clipboard-list me-1"></i> Inventario</h6>
                <div class="small"><strong>Items verificados:</strong> ${checkItems} / ${totalCheckItems}</div>
            </div>
            <hr class="my-2">
            <div class="mb-3">
                <h6 class="fw-bold text-primary"><i class="fas fa-camera me-1"></i> Fotos</h6>
                <div class="small"><strong>Total fotos:</strong> ${fotosCount}</div>
            </div>
        `;
    }

    // ===================== NAVIGATION =====================
    function initNavigation() {
        document.getElementById('btnSiguiente').addEventListener('click', () => {
            if (validarPasoActual()) {
                irAPaso(currentStep + 1);
            }
        });

        document.getElementById('btnAnterior').addEventListener('click', () => {
            irAPaso(currentStep - 1);
        });

        document.getElementById('btnFinalizar').addEventListener('click', finalizarRecepcion);
    }

    function irAPaso(step) {
        if (step < 1 || step > TOTAL_STEPS) return;

        // Ocultar paso actual
        document.getElementById(`step${currentStep}`).style.display = 'none';

        // Marcar paso actual como completado en nav
        const navActual = document.querySelector(`[data-step="${currentStep}"]`);
        if (navActual && step > currentStep) navActual.classList.add('completed');
        navActual.classList.remove('active');

        currentStep = step;

        // Mostrar nuevo paso
        document.getElementById(`step${currentStep}`).style.display = '';
        const navNuevo = document.querySelector(`[data-step="${currentStep}"]`);
        navNuevo.classList.add('active');

        // Actualizar progress bar
        document.getElementById('wizardProgress').style.width = (currentStep / TOTAL_STEPS * 100) + '%';

        // Botones
        document.getElementById('btnAnterior').style.display = currentStep > 1 ? '' : 'none';
        document.getElementById('btnSiguiente').style.display = currentStep < TOTAL_STEPS ? '' : 'none';
        document.getElementById('btnFinalizar').style.display = currentStep === TOTAL_STEPS ? '' : 'none';

        // Init paso 5 cuando se llega
        if (currentStep === 5) initStep5();

        // Scroll top
        window.scrollTo({ top: 0, behavior: 'smooth' });
    }

    // Fotos obligatorias (todas excepto Danos)
    const FOTOS_REQUERIDAS = ['FotoFrontal', 'FotoTrasera', 'FotoLateralIzq', 'FotoLateralDer', 'FotoInterior', 'FotoTablero'];

    function validarPasoActual() {
        if (currentStep === 1) {
            const esPropietario = document.getElementById('esPropietario').checked;
            if (!esPropietario) {
                const tercero = document.getElementById('entregadoPorTercero').value.trim();
                if (!tercero) {
                    Toast.warning('Ingrese el nombre de quien entrega el vehiculo.');
                    document.getElementById('entregadoPorTercero').focus();
                    return false;
                }
            }
            const km = parseInt(document.getElementById('kilometraje').value);
            if (isNaN(km) || km < WIZARD_DATA.kilometrajeActual) {
                Toast.warning(`El kilometraje debe ser al menos ${WIZARD_DATA.kilometrajeActual.toLocaleString()} km.`);
                document.getElementById('kilometraje').focus();
                return false;
            }
        }

        if (currentStep === 4) {
            const faltantes = FOTOS_REQUERIDAS.filter(tipo => !fotosData[tipo] || fotosData[tipo].length === 0);
            if (faltantes.length > 0) {
                // Resaltar las zonas faltantes
                faltantes.forEach(tipo => {
                    const zone = document.querySelector(`.foto-dropzone[data-tipo="${tipo}"]`);
                    if (zone) {
                        zone.closest('.card').classList.add('border-danger');
                        setTimeout(() => zone.closest('.card').classList.remove('border-danger'), 3000);
                    }
                });
                const nombres = {
                    'FotoFrontal': 'Frontal', 'FotoTrasera': 'Trasera',
                    'FotoLateralIzq': 'Lateral Izq.', 'FotoLateralDer': 'Lateral Der.',
                    'FotoInterior': 'Interior', 'FotoTablero': 'Tablero'
                };
                const lista = faltantes.map(t => nombres[t] || t).join(', ');
                Toast.warning(`Faltan fotos obligatorias: ${lista}`);
                return false;
            }
        }

        return true;
    }

    // ===================== RECOLECTAR DATOS =====================
    function recolectarDatos() {
        const data = {
            citaId: WIZARD_DATA.citaId,
            kilometraje: parseInt(document.getElementById('kilometraje').value),
            nivelCombustiblePorcentaje: FuelGauge.getValue(),
            observacionesApertura: document.getElementById('observacionesApertura').value || null,

            entregadoPor: document.getElementById('entregadoPor').value,
            esPropietarioQuienEntrega: document.getElementById('esPropietario').checked,
            relacionEntregante: document.getElementById('relacionEntregante')?.value || null,
            telefonoEntregante: document.getElementById('telefonoEntregante')?.value || null,

            danosExteriorJson: JSON.stringify(VehicleDiagram.getMarkers()),

            llantaRepuesto: document.getElementById('chkLlantaRepuesto').checked,
            gato: document.getElementById('chkGato').checked,
            triangulos: document.getElementById('chkTriangulos').checked,
            extintor: document.getElementById('chkExtintor').checked,
            herramientas: document.getElementById('chkHerramientas').checked,
            radio: document.getElementById('chkRadio').checked,
            tapetes: document.getElementById('chkTapetes').checked,

            antena: document.getElementById('chkAntena').checked,
            espejoIzquierdo: document.getElementById('chkEspejoIzquierdo').checked,
            espejoDerecho: document.getElementById('chkEspejoDerecho').checked,
            limpiaparabrisas: document.getElementById('chkLimpiaparabrisas').checked,
            placaDelantera: document.getElementById('chkPlacaDelantera').checked,
            placaTrasera: document.getElementById('chkPlacaTrasera').checked,
            tapaCombustible: document.getElementById('chkTapaCombustible').checked,

            manualVehiculo: document.getElementById('chkManualVehiculo').checked,
            segundaLlave: document.getElementById('chkSegundaLlave').checked,

            inspeccionRuedasJson: JSON.stringify(recolectarRuedas()),

            nivelAceiteOk: document.getElementById('chkNivelAceiteOk').checked,
            nivelRefrigeranteOk: document.getElementById('chkNivelRefrigeranteOk').checked,
            nivelLiquidoFrenosOk: document.getElementById('chkNivelLiquidoFrenosOk').checked,
            bateriaOk: document.getElementById('chkBateriaOk').checked,

            observacionesGenerales: document.getElementById('observacionesGenerales').value || null,
            firmaClienteBase64: signaturePad && !signaturePad.isEmpty() ? signaturePad.toDataURL() : null
        };

        return data;
    }

    function recolectarRuedas() {
        const posiciones = ['FI', 'FD', 'TI', 'TD'];
        return posiciones.map(pos => {
            const llantas = document.querySelectorAll(`.rueda-estado[data-pos="${pos}"]`);
            const checks = document.querySelectorAll(`.rueda-check[data-pos="${pos}"]`);
            return {
                posicion: pos,
                estadoLlanta: parseInt(llantas[0]?.value || 3),
                estadoRin: parseInt(llantas[1]?.value || 3),
                tuercasCompletas: checks[0]?.checked ?? true,
                tieneCopa: checks[1]?.checked ?? true
            };
        });
    }

    // ===================== SUBMIT =====================
    async function finalizarRecepcion() {
        const btn = document.getElementById('btnFinalizar');
        btn.disabled = true;
        btn.innerHTML = '<span class="spinner-border spinner-border-sm me-1"></span>Procesando...';

        try {
            const datos = recolectarDatos();

            // 1. Crear OS + Recepcion
            const response = await fetch(URLS.iniciarRecepcion, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(datos)
            });

            let result;
            try {
                result = await response.json();
            } catch (parseErr) {
                const text = await response.text().catch(() => '');
                console.error('Response no es JSON:', response.status, text);
                Toast.error(`Error del servidor (${response.status}). Revise la consola.`);
                btn.disabled = false;
                btn.innerHTML = '<i class="fas fa-check me-1"></i>Completar Recepcion';
                return;
            }

            if (!result.success) {
                Toast.error(result.message || 'Error al crear la recepcion.');
                btn.disabled = false;
                btn.innerHTML = '<i class="fas fa-check me-1"></i>Completar Recepcion';
                return;
            }

            // 2. Subir fotos al endpoint de evidencias existente
            const recepcionData = result.data;
            const osId = recepcionData?.osId;
            const recepcionId = recepcionData?.recepcionId;

            if (osId) {
                await subirFotos(osId, recepcionId);
            }

            Toast.success('Recepcion completada exitosamente.');
            setTimeout(() => {
                window.location.href = URLS.volverAgenda;
            }, 1500);

        } catch (error) {
            console.error('Error:', error);
            Toast.error('Error inesperado al procesar la recepcion.');
            btn.disabled = false;
            btn.innerHTML = '<i class="fas fa-check me-1"></i>Completar Recepcion';
        }
    }

    async function subirFotos(osId, recepcionId) {
        // Mapa de tipos de foto al enum TipoEvidencia
        const tipoMap = {
            'FotoFrontal': 'FotoFrontal',
            'FotoTrasera': 'FotoTrasera',
            'FotoLateralIzq': 'FotoLateralIzq',
            'FotoLateralDer': 'FotoLateralDer',
            'FotoInterior': 'FotoInterior',
            'FotoTablero': 'FotoTablero',
            'FotoDano': 'FotoDano'
        };

        for (const [tipo, fotos] of Object.entries(fotosData)) {
            for (const foto of fotos) {
                try {
                    await fetch(URLS.subirEvidencia, {
                        method: 'POST',
                        headers: { 'Content-Type': 'application/json' },
                        body: JSON.stringify({
                            osId: osId,
                            recepcionId: recepcionId,
                            tipoEvidencia: tipoMap[tipo] || 'Otro',
                            base64Data: foto.base64,
                            nombreArchivo: foto.nombre,
                            descripcion: `Foto de recepcion - ${tipo}`
                        })
                    });
                } catch (err) {
                    console.warn(`Error subiendo foto ${tipo}:`, err);
                }
            }
        }
    }

})();
