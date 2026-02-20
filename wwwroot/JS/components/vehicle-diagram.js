/**
 * VehicleDiagram - Componente SVG interactivo para marcar danos en vehiculo
 * Uso: VehicleDiagram.init('containerId', segmento);
 *      VehicleDiagram.getMarkers(); // [{tipo, x, y, descripcion}]
 */
const VehicleDiagram = (() => {
    let _container = null;
    let _svgEl = null;
    let _markers = [];
    let _pendingClick = null;
    let _onMarkersChange = null;

    // Mapeo de segmento a archivo SVG
    const SVG_MAP = {
        1: '/images/vehicles/sedan-top.svg',   // Sedan
        2: '/images/vehicles/suv-top.svg',     // SUV
        3: '/images/vehicles/pickup-top.svg',  // Pickup
        4: '/images/vehicles/van-top.svg',     // Van
        5: '/images/vehicles/sedan-top.svg',   // Coupe
        6: '/images/vehicles/sedan-top.svg',   // Hatchback
        7: '/images/vehicles/sedan-top.svg',   // Deportivo
        8: '/images/vehicles/sedan-top.svg',   // Electrico
        9: '/images/vehicles/sedan-top.svg',   // Hibrido
        99: '/images/vehicles/sedan-top.svg'   // Otro
    };

    const DAMAGE_COLORS = {
        1: '#dc3545',  // Golpe - rojo
        2: '#fd7e14',  // Rayadura - naranja
        3: '#6f42c1',  // Abolladura - morado
        4: '#0dcaf0',  // Faltante - cyan
        5: '#ffc107',  // Rotura - amarillo
        99: '#6c757d'  // Otro - gris
    };

    const DAMAGE_NAMES = {
        1: 'Golpe',
        2: 'Rayadura',
        3: 'Abolladura',
        4: 'Faltante',
        5: 'Rotura',
        99: 'Otro'
    };

    async function loadSvg(segmento) {
        const url = SVG_MAP[segmento] || SVG_MAP[1];
        try {
            const response = await fetch(url);
            if (!response.ok) throw new Error('SVG not found');
            return await response.text();
        } catch {
            // Fallback: dibujar rectangulo simple
            return `<svg viewBox="0 0 400 200" xmlns="http://www.w3.org/2000/svg">
                <rect x="60" y="30" width="280" height="140" rx="30" fill="#f8f9fa" stroke="#dee2e6" stroke-width="2"/>
                <text x="200" y="105" text-anchor="middle" fill="#adb5bd" font-size="14">Vista superior del vehiculo</text>
            </svg>`;
        }
    }

    async function init(containerId, segmento, onMarkersChange) {
        _container = document.getElementById(containerId);
        if (!_container) return;
        _markers = [];
        _onMarkersChange = onMarkersChange;

        const svgText = await loadSvg(segmento);
        _container.innerHTML = svgText;
        _svgEl = _container.querySelector('svg');

        if (_svgEl) {
            _svgEl.style.width = '100%';
            _svgEl.style.maxWidth = '260px';
            _svgEl.style.height = 'auto';
            _svgEl.style.cursor = 'crosshair';

            _svgEl.addEventListener('click', (e) => {
                const rect = _svgEl.getBoundingClientRect();
                const viewBox = _svgEl.viewBox.baseVal;
                const scaleX = viewBox.width / rect.width;
                const scaleY = viewBox.height / rect.height;
                const x = (e.clientX - rect.left) * scaleX;
                const y = (e.clientY - rect.top) * scaleY;

                _pendingClick = { x: Math.round(x * 10) / 10, y: Math.round(y * 10) / 10 };

                // Abrir modal de dano
                const modal = new bootstrap.Modal(document.getElementById('modalDano'));
                document.getElementById('tipoDano').value = '1';
                document.getElementById('descripcionDano').value = '';
                modal.show();
            });
        }

        renderMarkersList();
    }

    function addMarker(tipo, x, y, descripcion) {
        const marker = { tipo: parseInt(tipo), x, y, descripcion: descripcion || '' };
        _markers.push(marker);
        renderMarkerOnSvg(marker, _markers.length - 1);
        renderMarkersList();
        if (_onMarkersChange) _onMarkersChange(_markers);
    }

    function confirmPendingMarker(tipo, descripcion) {
        if (!_pendingClick) return;
        addMarker(tipo, _pendingClick.x, _pendingClick.y, descripcion);
        _pendingClick = null;
    }

    function removeMarker(index) {
        _markers.splice(index, 1);
        rerenderAllMarkers();
        renderMarkersList();
        if (_onMarkersChange) _onMarkersChange(_markers);
    }

    function renderMarkerOnSvg(marker, index) {
        if (!_svgEl) return;
        const viewBox = _svgEl.viewBox.baseVal;
        const color = DAMAGE_COLORS[marker.tipo] || DAMAGE_COLORS[99];

        const circle = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
        circle.setAttribute('cx', marker.x);
        circle.setAttribute('cy', marker.y);
        circle.setAttribute('r', '8');
        circle.setAttribute('fill', color);
        circle.setAttribute('stroke', 'white');
        circle.setAttribute('stroke-width', '2');
        circle.setAttribute('class', 'svg-damage-marker');
        circle.setAttribute('data-index', index);
        circle.style.cursor = 'pointer';
        circle.style.filter = 'drop-shadow(0 1px 2px rgba(0,0,0,0.3))';

        const label = document.createElementNS('http://www.w3.org/2000/svg', 'text');
        label.setAttribute('x', marker.x);
        label.setAttribute('y', marker.y + 4);
        label.setAttribute('text-anchor', 'middle');
        label.setAttribute('font-size', '9');
        label.setAttribute('font-weight', 'bold');
        label.setAttribute('fill', 'white');
        label.setAttribute('class', 'svg-damage-marker');
        label.setAttribute('pointer-events', 'none');
        label.textContent = index + 1;

        _svgEl.appendChild(circle);
        _svgEl.appendChild(label);
    }

    function rerenderAllMarkers() {
        if (!_svgEl) return;
        _svgEl.querySelectorAll('.svg-damage-marker').forEach(el => el.remove());
        _markers.forEach((m, i) => renderMarkerOnSvg(m, i));
    }

    function renderMarkersList() {
        const container = document.getElementById('listaMarkers');
        const sinDanos = document.getElementById('sinDanos');
        if (!container) return;

        // Limpiar items existentes (no el mensaje sinDanos)
        container.querySelectorAll('.marker-item').forEach(el => el.remove());

        if (_markers.length === 0) {
            if (sinDanos) sinDanos.style.display = '';
            return;
        }

        if (sinDanos) sinDanos.style.display = 'none';

        _markers.forEach((m, i) => {
            const color = DAMAGE_COLORS[m.tipo] || DAMAGE_COLORS[99];
            const nombre = DAMAGE_NAMES[m.tipo] || 'Otro';
            const item = document.createElement('div');
            item.className = 'list-group-item list-group-item-action d-flex justify-content-between align-items-center py-2 marker-item';
            item.innerHTML = `
                <div>
                    <span class="badge me-1" style="background:${color}">${i + 1}</span>
                    <strong class="small">${nombre}</strong>
                    ${m.descripcion ? `<br><small class="text-muted">${m.descripcion}</small>` : ''}
                </div>
                <button class="btn btn-outline-danger btn-sm" onclick="VehicleDiagram.removeMarker(${i})" title="Eliminar">
                    <i class="fas fa-times"></i>
                </button>
            `;
            container.appendChild(item);
        });
    }

    function getMarkers() { return [..._markers]; }
    function clear() { _markers = []; rerenderAllMarkers(); renderMarkersList(); }

    return { init, addMarker, confirmPendingMarker, removeMarker, getMarkers, clear, DAMAGE_NAMES, DAMAGE_COLORS };
})();
