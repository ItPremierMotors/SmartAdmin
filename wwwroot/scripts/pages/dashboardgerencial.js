import ApexCharts from '../thirdparty/apexchartsWrapper.js';

// Dashboard Gerencial - PremierFlow
document.addEventListener('DOMContentLoaded', function () {
    'use strict';

    // Chart instances
    const charts = {};
    let refreshTimer = null;

    // =============================================
    // API Helper
    // =============================================
    async function fetchDashboard(endpoint, params = {}) {
        const url = new URL(`/Dashboard/${endpoint}`, window.location.origin);
        Object.entries(params).forEach(([k, v]) => url.searchParams.set(k, v));

        const resp = await fetch(url, { credentials: 'same-origin' });
        if (!resp.ok) throw new Error(`HTTP ${resp.status}`);
        const json = await resp.json();
        if (!json.success) throw new Error(json.message || 'Error en la API');
        return json.data;
    }

    function getDateRange() {
        const days = parseInt(document.getElementById('dateRange').value);
        const fin = new Date();
        const inicio = new Date();
        inicio.setDate(inicio.getDate() - days);
        return {
            fechaInicio: inicio.toISOString().split('T')[0],
            fechaFin: fin.toISOString().split('T')[0]
        };
    }

    function formatCurrency(value) {
        return 'L ' + Number(value).toLocaleString('es-HN', {
            minimumFractionDigits: 2,
            maximumFractionDigits: 2
        });
    }

    function destroyChart(name) {
        if (charts[name]) {
            charts[name].destroy();
            delete charts[name];
        }
    }

    function getColor(path, fallback) {
        try {
            if (window.colorMap) {
                const parts = path.split('.');
                let obj = window.colorMap;
                for (const p of parts) obj = obj[p];
                return obj.hex || obj;
            }
        } catch { /* fallback */ }
        return fallback;
    }

    // Colores del tema
    function colors() {
        return {
            primary: getColor('primary.500', '#886ab5'),
            success: getColor('success.500', '#1dc9b7'),
            warning: getColor('warning.500', '#ffc241'),
            danger: getColor('danger.500', '#fd3995'),
            info: getColor('info.500', '#2196F3'),
            secondary: getColor('primary.300', '#a88cc5'),
            dark: getColor('primary.700', '#5f4b8b'),
            body: '#ccc'
        };
    }

    // =============================================
    // Seccion 1: KPIs
    // =============================================
    async function loadResumen() {
        try {
            const data = await fetchDashboard('Resumen');

            document.getElementById('kpi-ventas-count').textContent = data.ventasMesCount;
            document.getElementById('kpi-ventas-revenue').textContent = formatCurrency(data.ventasMesRevenue);
            document.getElementById('kpi-ordenes-activas').textContent = data.ordenesActivasCount;
            document.getElementById('kpi-citas-total').textContent =
                data.citasAgendadas + data.citasEnProceso + data.citasCompletadas + data.citasNoShow;
            document.getElementById('kpi-citas-completadas').textContent = data.citasCompletadas;
            document.getElementById('kpi-citas-noshow').textContent = data.citasNoShow;
            document.getElementById('kpi-ocupacion').textContent = data.ocupacionTallerPorcentaje + '%';
            document.getElementById('kpi-minutos-info').textContent =
                `${data.minutosReservados} / ${data.minutosDisponibles} min`;

            // Badges de vehiculos por estado
            const container = document.getElementById('kpi-vehiculos-estado');
            container.innerHTML = '<span class="fs-sm fw-500 text-muted me-2">Inventario:</span>';
            const estadoColors = {
                'EnTransito': 'secondary',
                'EnAduana': 'warning',
                'EnBodega': 'info',
                'EnExhibicion': 'primary',
                'Reservado': 'danger',
                'Vendido': 'success',
                'Entregado': 'dark'
            };
            const estadoLabels = {
                'EnTransito': 'En Transito',
                'EnAduana': 'En Aduana',
                'EnBodega': 'En Bodega',
                'EnExhibicion': 'En Exhibicion',
                'Reservado': 'Reservado',
                'Vendido': 'Vendido',
                'Entregado': 'Entregado'
            };
            (data.vehiculosPorEstado || []).forEach(function (v) {
                const badge = document.createElement('span');
                badge.className = 'badge bg-' + (estadoColors[v.estado] || 'secondary') + ' fs-sm';
                badge.textContent = (estadoLabels[v.estado] || v.estado) + ': ' + v.cantidad;
                container.appendChild(badge);
            });
        } catch (e) {
            console.error('Error cargando resumen:', e);
            document.getElementById('kpi-ventas-count').textContent = '-';
            document.getElementById('kpi-ordenes-activas').textContent = '-';
            document.getElementById('kpi-citas-total').textContent = '-';
            document.getElementById('kpi-ocupacion').textContent = '-';
        }
    }

    // =============================================
    // Seccion 2: Taller y Tecnicos
    // =============================================
    async function loadTaller() {
        try {
            const range = getDateRange();
            const data = await fetchDashboard('Taller', range);
            const c = colors();

            // --- Ordenes por Estado (bar horizontal) ---
            destroyChart('ordenes-estado');
            if (data.ordenesPorEstado && data.ordenesPorEstado.length > 0) {
                charts['ordenes-estado'] = new ApexCharts(
                    document.getElementById('chart-ordenes-estado'), {
                    series: [{
                        name: 'Ordenes',
                        data: data.ordenesPorEstado.map(function (o) { return o.cantidad; })
                    }],
                    chart: { type: 'bar', height: 300, toolbar: { show: false } },
                    plotOptions: { bar: { horizontal: true, borderRadius: 4 } },
                    colors: [c.primary],
                    xaxis: {
                        categories: data.ordenesPorEstado.map(function (o) { return o.estado; })
                    },
                    tooltip: { theme: 'dark' }
                });
                charts['ordenes-estado'].render();
            } else {
                document.getElementById('chart-ordenes-estado').innerHTML =
                    '<div class="text-center text-muted py-5">Sin datos de ordenes en este periodo</div>';
            }

            // --- Productividad por Tecnico (bar agrupado) ---
            destroyChart('productividad');
            if (data.productividadTecnicos && data.productividadTecnicos.length > 0) {
                charts['productividad'] = new ApexCharts(
                    document.getElementById('chart-productividad'), {
                    series: [
                        {
                            name: 'Completadas',
                            data: data.productividadTecnicos.map(function (t) { return t.ordenesCompletadas; })
                        },
                        {
                            name: 'En Proceso',
                            data: data.productividadTecnicos.map(function (t) { return t.ordenesEnProceso; })
                        }
                    ],
                    chart: { type: 'bar', height: 300, stacked: false, toolbar: { show: false } },
                    plotOptions: { bar: { horizontal: false, borderRadius: 4, columnWidth: '60%' } },
                    colors: [c.success, c.warning],
                    xaxis: {
                        categories: data.productividadTecnicos.map(function (t) { return t.nombreCompleto; })
                    },
                    tooltip: { theme: 'dark' }
                });
                charts['productividad'].render();
            } else {
                document.getElementById('chart-productividad').innerHTML =
                    '<div class="text-center text-muted py-5">Sin datos de tecnicos en este periodo</div>';
            }

            // --- Capacidad del Taller (area chart) ---
            destroyChart('capacidad');
            if (data.capacidadDiaria && data.capacidadDiaria.length > 0) {
                charts['capacidad'] = new ApexCharts(
                    document.getElementById('chart-capacidad'), {
                    series: [
                        {
                            name: 'Disponibles',
                            data: data.capacidadDiaria.map(function (cd) { return cd.minutosDisponibles; })
                        },
                        {
                            name: 'Reservados',
                            data: data.capacidadDiaria.map(function (cd) { return cd.minutosReservados; })
                        },
                        {
                            name: 'Utilizados',
                            data: data.capacidadDiaria.map(function (cd) { return cd.minutosUtilizados; })
                        }
                    ],
                    chart: { type: 'area', height: 300, toolbar: { show: false } },
                    colors: [c.info, c.warning, c.success],
                    stroke: { curve: 'smooth', width: 2 },
                    fill: { type: 'gradient', gradient: { opacityFrom: 0.4, opacityTo: 0.05 } },
                    xaxis: {
                        categories: data.capacidadDiaria.map(function (cd) {
                            return new Date(cd.fecha).toLocaleDateString('es', { month: 'short', day: 'numeric' });
                        })
                    },
                    yaxis: {
                        title: { text: 'Minutos' },
                        labels: { formatter: function (val) { return Math.round(val) + ' min'; } }
                    },
                    tooltip: { theme: 'dark' }
                });
                charts['capacidad'].render();
            } else {
                document.getElementById('chart-capacidad').innerHTML =
                    '<div class="text-center text-muted py-5">Sin datos de capacidad en este periodo</div>';
            }

            // --- Eficiencia del Taller (radialBar gauge) ---
            destroyChart('eficiencia');
            var eficiencia = data.eficienciaTaller || 0;
            var efColor = eficiencia >= 80 ? c.success : (eficiencia >= 50 ? c.warning : c.danger);
            charts['eficiencia'] = new ApexCharts(
                document.getElementById('chart-eficiencia'), {
                series: [eficiencia],
                chart: { type: 'radialBar', height: 250 },
                plotOptions: {
                    radialBar: {
                        startAngle: -135,
                        endAngle: 135,
                        hollow: { size: '65%' },
                        track: { strokeWidth: '100%' },
                        dataLabels: {
                            name: { show: true, offsetY: -10, fontSize: '13px' },
                            value: {
                                fontSize: '30px',
                                fontWeight: 700,
                                offsetY: 5,
                                formatter: function (val) { return val + '%'; }
                            }
                        }
                    }
                },
                colors: [efColor],
                labels: ['Eficiencia']
            });
            charts['eficiencia'].render();

        } catch (e) {
            console.error('Error cargando taller:', e);
            ['chart-ordenes-estado', 'chart-productividad', 'chart-capacidad', 'chart-eficiencia'].forEach(function (id) {
                var el = document.getElementById(id);
                if (el) el.innerHTML = '<div class="text-center text-muted py-5">Error cargando datos</div>';
            });
        }
    }

    // =============================================
    // Seccion 3: Ventas e Inventario
    // =============================================
    async function loadVentas() {
        try {
            const range = getDateRange();
            const data = await fetchDashboard('Ventas', range);
            const c = colors();

            destroyChart('ventas-mes');
            if (data.ventasPorMes && data.ventasPorMes.length > 0) {
                charts['ventas-mes'] = new ApexCharts(
                    document.getElementById('chart-ventas-mes'), {
                    series: [
                        {
                            name: 'Unidades',
                            type: 'column',
                            data: data.ventasPorMes.map(function (v) { return v.cantidad; })
                        },
                        {
                            name: 'Revenue',
                            type: 'line',
                            data: data.ventasPorMes.map(function (v) { return v.revenue; })
                        }
                    ],
                    chart: { height: 300, toolbar: { show: false } },
                    colors: [c.primary, c.success],
                    stroke: { width: [0, 3], curve: 'smooth' },
                    plotOptions: { bar: { borderRadius: 4, columnWidth: '50%' } },
                    xaxis: {
                        categories: data.ventasPorMes.map(function (v) { return v.mesNombre + ' ' + v.anio; })
                    },
                    yaxis: [
                        { title: { text: 'Unidades' } },
                        {
                            opposite: true,
                            title: { text: 'Revenue (L)' },
                            labels: { formatter: function (val) { return formatCurrency(val); } }
                        }
                    ],
                    tooltip: { theme: 'dark' }
                });
                charts['ventas-mes'].render();
            } else {
                document.getElementById('chart-ventas-mes').innerHTML =
                    '<div class="text-center text-muted py-5">Sin datos de ventas en este periodo</div>';
            }
        } catch (e) {
            console.error('Error cargando ventas:', e);
            document.getElementById('chart-ventas-mes').innerHTML =
                '<div class="text-center text-muted py-5">Error cargando datos</div>';
        }
    }

    async function loadInventario() {
        try {
            const data = await fetchDashboard('Inventario');
            const c = colors();

            // --- Pipeline de Inventario ---
            destroyChart('pipeline');
            var pipelineOrder = ['EnTransito', 'EnAduana', 'EnBodega', 'EnExhibicion', 'Reservado', 'Vendido', 'Entregado'];
            var pipelineLabels = {
                'EnTransito': 'En Transito',
                'EnAduana': 'En Aduana',
                'EnBodega': 'En Bodega',
                'EnExhibicion': 'Exhibicion',
                'Reservado': 'Reservado',
                'Vendido': 'Vendido',
                'Entregado': 'Entregado'
            };
            var pipelineColors = [c.secondary, c.warning, c.info, c.primary, c.danger, c.success, c.dark];

            var sorted = pipelineOrder.map(function (estado) {
                return data.pipeline.find(function (p) { return p.estado === estado; }) || { estado: estado, cantidad: 0 };
            });

            charts['pipeline'] = new ApexCharts(
                document.getElementById('chart-pipeline'), {
                series: [{ name: 'Vehiculos', data: sorted.map(function (p) { return p.cantidad; }) }],
                chart: { type: 'bar', height: 300, toolbar: { show: false } },
                plotOptions: { bar: { distributed: true, borderRadius: 4, columnWidth: '65%' } },
                colors: pipelineColors,
                xaxis: {
                    categories: sorted.map(function (p) { return pipelineLabels[p.estado] || p.estado; })
                },
                legend: { show: false },
                tooltip: { theme: 'dark' }
            });
            charts['pipeline'].render();

            // --- Envejecimiento de Inventario ---
            destroyChart('envejecimiento');
            if (data.envejecimiento && data.envejecimiento.length > 0) {
                var envLabels = {
                    'EnTransito': 'En Transito',
                    'EnAduana': 'En Aduana',
                    'EnBodega': 'En Bodega',
                    'EnExhibicion': 'Exhibicion',
                    'Reservado': 'Reservado'
                };
                charts['envejecimiento'] = new ApexCharts(
                    document.getElementById('chart-envejecimiento'), {
                    series: [{
                        name: 'Dias Promedio',
                        data: data.envejecimiento.map(function (e) { return Math.round(e.diasPromedio); })
                    }],
                    chart: { type: 'bar', height: 280, toolbar: { show: false } },
                    plotOptions: { bar: { horizontal: true, borderRadius: 4 } },
                    colors: [c.warning],
                    xaxis: {
                        categories: data.envejecimiento.map(function (e) {
                            return (envLabels[e.estado] || e.estado) + ' (' + e.cantidad + ')';
                        })
                    },
                    tooltip: {
                        theme: 'dark',
                        y: { formatter: function (val) { return val + ' dias promedio'; } }
                    }
                });
                charts['envejecimiento'].render();
            } else {
                document.getElementById('chart-envejecimiento').innerHTML =
                    '<div class="text-center text-muted py-5">Sin datos de envejecimiento</div>';
            }
        } catch (e) {
            console.error('Error cargando inventario:', e);
            ['chart-pipeline', 'chart-envejecimiento'].forEach(function (id) {
                var el = document.getElementById(id);
                if (el) el.innerHTML = '<div class="text-center text-muted py-5">Error cargando datos</div>';
            });
        }
    }

    // =============================================
    // Carga completa y auto-refresh
    // =============================================
    async function loadAll() {
        await Promise.all([
            loadResumen(),
            loadTaller(),
            loadVentas(),
            loadInventario()
        ]);
    }

    function startAutoRefresh() {
        if (refreshTimer) clearInterval(refreshTimer);
        refreshTimer = setInterval(loadAll, 5 * 60 * 1000); // cada 5 min
    }

    // Event listeners
    document.getElementById('dateRange').addEventListener('change', loadAll);
    document.getElementById('btnRefresh').addEventListener('click', loadAll);

    // Init
    loadAll();
    startAutoRefresh();
});
