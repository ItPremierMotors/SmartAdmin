/**
 * FuelGauge - Componente visual de nivel de combustible semicircular
 * Uso: FuelGauge.init('containerId', 50);
 *      FuelGauge.getValue(); // 0-100
 */
const FuelGauge = (() => {
    let _value = 50;
    let _container = null;
    let _arc = null;
    let _needle = null;
    let _valueText = null;
    let _isDragging = false;

    const WIDTH = 280;
    const HEIGHT = 160;
    const CX = 140;
    const CY = 145;
    const RADIUS = 110;
    const START_ANGLE = Math.PI;
    const END_ANGLE = 0;

    function getColor(val) {
        if (val <= 15) return '#dc3545';
        if (val <= 30) return '#fd7e14';
        if (val <= 50) return '#ffc107';
        return '#198754';
    }

    function valueToAngle(val) {
        return START_ANGLE - (val / 100) * Math.PI;
    }

    function angleToValue(angle) {
        let val = ((START_ANGLE - angle) / Math.PI) * 100;
        return Math.max(0, Math.min(100, Math.round(val)));
    }

    function polarToCartesian(angle) {
        return {
            x: CX + RADIUS * Math.cos(angle),
            y: CY - RADIUS * Math.sin(angle)
        };
    }

    function describeArc(startAngle, endAngle) {
        const start = polarToCartesian(startAngle);
        const end = polarToCartesian(endAngle);
        const sweep = Math.abs(startAngle - endAngle);
        const largeArc = sweep > Math.PI ? 1 : 0;
        return `M ${start.x} ${start.y} A ${RADIUS} ${RADIUS} 0 ${largeArc} 1 ${end.x} ${end.y}`;
    }

    function createSvg() {
        const svg = document.createElementNS('http://www.w3.org/2000/svg', 'svg');
        svg.setAttribute('viewBox', `0 0 ${WIDTH} ${HEIGHT}`);
        svg.setAttribute('width', '100%');
        svg.style.maxWidth = WIDTH + 'px';
        svg.style.cursor = 'pointer';

        // Background arc (gray)
        const bgArc = document.createElementNS('http://www.w3.org/2000/svg', 'path');
        bgArc.setAttribute('d', describeArc(START_ANGLE, END_ANGLE));
        bgArc.setAttribute('fill', 'none');
        bgArc.setAttribute('stroke', '#e9ecef');
        bgArc.setAttribute('stroke-width', '20');
        bgArc.setAttribute('stroke-linecap', 'round');
        svg.appendChild(bgArc);

        // Value arc (colored)
        _arc = document.createElementNS('http://www.w3.org/2000/svg', 'path');
        _arc.setAttribute('fill', 'none');
        _arc.setAttribute('stroke-width', '20');
        _arc.setAttribute('stroke-linecap', 'round');
        svg.appendChild(_arc);

        // Tick marks and labels
        const labels = ['E', '1/4', '1/2', '3/4', 'F'];
        for (let i = 0; i <= 4; i++) {
            const angle = START_ANGLE - (i / 4) * Math.PI;
            const outer = { x: CX + (RADIUS + 15) * Math.cos(angle), y: CY - (RADIUS + 15) * Math.sin(angle) };
            const inner = { x: CX + (RADIUS - 15) * Math.cos(angle), y: CY - (RADIUS - 15) * Math.sin(angle) };

            const tick = document.createElementNS('http://www.w3.org/2000/svg', 'line');
            tick.setAttribute('x1', inner.x);
            tick.setAttribute('y1', inner.y);
            tick.setAttribute('x2', outer.x);
            tick.setAttribute('y2', outer.y);
            tick.setAttribute('stroke', '#adb5bd');
            tick.setAttribute('stroke-width', '1.5');
            svg.appendChild(tick);

            const label = document.createElementNS('http://www.w3.org/2000/svg', 'text');
            const labelPos = { x: CX + (RADIUS + 28) * Math.cos(angle), y: CY - (RADIUS + 28) * Math.sin(angle) };
            label.setAttribute('x', labelPos.x);
            label.setAttribute('y', labelPos.y);
            label.setAttribute('text-anchor', 'middle');
            label.setAttribute('dominant-baseline', 'middle');
            label.setAttribute('font-size', '11');
            label.setAttribute('fill', '#6c757d');
            label.textContent = labels[i];
            svg.appendChild(label);
        }

        // Needle
        _needle = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        _needle.setAttribute('x1', CX);
        _needle.setAttribute('y1', CY);
        _needle.setAttribute('stroke', '#343a40');
        _needle.setAttribute('stroke-width', '2.5');
        _needle.setAttribute('stroke-linecap', 'round');
        svg.appendChild(_needle);

        // Center circle
        const center = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
        center.setAttribute('cx', CX);
        center.setAttribute('cy', CY);
        center.setAttribute('r', '6');
        center.setAttribute('fill', '#343a40');
        svg.appendChild(center);

        // Value text
        _valueText = document.createElementNS('http://www.w3.org/2000/svg', 'text');
        _valueText.setAttribute('x', CX);
        _valueText.setAttribute('y', CY - 30);
        _valueText.setAttribute('text-anchor', 'middle');
        _valueText.setAttribute('font-size', '24');
        _valueText.setAttribute('font-weight', 'bold');
        svg.appendChild(_valueText);

        // Events
        svg.addEventListener('mousedown', startDrag);
        svg.addEventListener('mousemove', onDrag);
        svg.addEventListener('mouseup', stopDrag);
        svg.addEventListener('mouseleave', stopDrag);
        svg.addEventListener('click', onClick);

        // Touch events
        svg.addEventListener('touchstart', e => { e.preventDefault(); startDrag(e.touches[0]); }, { passive: false });
        svg.addEventListener('touchmove', e => { e.preventDefault(); onDrag(e.touches[0]); }, { passive: false });
        svg.addEventListener('touchend', stopDrag);

        return svg;
    }

    function getAngleFromEvent(evt) {
        const rect = _container.querySelector('svg').getBoundingClientRect();
        const scaleX = WIDTH / rect.width;
        const x = (evt.clientX - rect.left) * scaleX - CX;
        const y = CY - (evt.clientY - rect.top) * (HEIGHT / rect.height);
        let angle = Math.atan2(y, x);
        if (angle < 0) angle = 0;
        if (angle > Math.PI) angle = Math.PI;
        return angle;
    }

    function startDrag() { _isDragging = true; }
    function stopDrag() { _isDragging = false; }

    function onDrag(evt) {
        if (!_isDragging) return;
        const angle = getAngleFromEvent(evt);
        setValue(angleToValue(angle));
    }

    function onClick(evt) {
        const angle = getAngleFromEvent(evt);
        setValue(angleToValue(angle));
    }

    function updateVisual() {
        const angle = valueToAngle(_value);
        const color = getColor(_value);

        // Arc
        if (_value > 0) {
            _arc.setAttribute('d', describeArc(START_ANGLE, angle));
            _arc.setAttribute('stroke', color);
        } else {
            _arc.setAttribute('d', '');
        }

        // Needle
        const needleTip = polarToCartesian(angle);
        _needle.setAttribute('x2', needleTip.x);
        _needle.setAttribute('y2', needleTip.y);

        // Text
        _valueText.textContent = _value + '%';
        _valueText.setAttribute('fill', color);

        // Hidden input
        const input = document.getElementById('nivelCombustible');
        if (input) input.value = _value;
    }

    function init(containerId, initialValue = 50) {
        _container = document.getElementById(containerId);
        if (!_container) return;
        _value = initialValue;
        _container.innerHTML = '';
        _container.appendChild(createSvg());
        updateVisual();
    }

    function getValue() { return _value; }

    function setValue(val) {
        _value = Math.max(0, Math.min(100, Math.round(val)));
        updateVisual();
    }

    return { init, getValue, setValue };
})();
