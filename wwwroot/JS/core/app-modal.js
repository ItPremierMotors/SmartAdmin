//// SmartAdmin/wwwroot/js/app-modal.js
//const AppModal = (function () {
//    let modalInstance = null;

//    function init() {
//        if (!modalInstance) {
//            modalInstance = new bootstrap.Modal(document.getElementById('globalModal'));
//        }
//    }

//    /**
//     * Abre el modal
//     * @param {Object} config
//     * @param {string} config.title - Título
//     * @param {string} config.url - URL de la vista parcial
//     * @param {string} config.size - 'sm' | 'md' | 'lg' | 'xl' (default: 'md')
//     * @param {Object} config.data - Datos para enviar a la vista parcial (opcional)
//     * @param {Function} config.onShown - Callback cuando se muestra (opcional)
//     * @param {Function} config.onHidden - Callback cuando se cierra (opcional)
//     */
//    function open(config) {
//        init();

//        const { title = '', url = '', size = 'md', data = null, onShown = null, onHidden = null } = config;

//        // Título
//        $('#globalModalTitle').html(title);

//        // Tamaño
//        const $dialog = $('#globalModalDialog');
//        $dialog.removeClass('modal-sm modal-lg modal-xl modal-fullscreen');
//        if (size !== 'md') {
//            $dialog.addClass('modal-' + size);
//        }

//        // Ocultar footer por defecto
//        $('#globalModalFooter').hide();

//        // Mostrar loading
//        showLoading();

//        // Mostrar modal
//        modalInstance.show();

//        // Cargar vista parcial
//        $.ajax({
//            url: url,
//            type: 'GET',
//            data: data,
//            success: function (html) {
//                $('#globalModalBody').html(html);
//            },
//            error: function () {
//                $('#globalModalBody').html(`
//                    <div class="alert alert-danger mb-0">
//                        <i class="fas fa-exclamation-circle me-2"></i>
//                        Error al cargar el contenido
//                    </div>
//                `);
//            }
//        });

//        // Eventos
//        const $modal = $('#globalModal');
//        $modal.off('shown.bs.modal hidden.bs.modal');

//        if (onShown) {
//            $modal.on('shown.bs.modal', onShown);
//        }
//        if (onHidden) {
//            $modal.on('hidden.bs.modal', onHidden);
//        }
//    }

//    function close() {
//        if (modalInstance) {
//            modalInstance.hide();
//        }
//    }

//    function showLoading() {
//        $('#globalModalBody').html(`
//            <div class="text-center py-4">
//                <div class="spinner-border text-primary"></div>
//                <p class="mt-2 mb-0 text-muted">Cargando...</p>
//            </div>
//        `);
//    }

//    function setFooter(html) {
//        if (html) {
//            $('#globalModalFooter').html(html).show();
//        } else {
//            $('#globalModalFooter').hide();
//        }
//    }

//    return {
//        open: open,
//        close: close,
//        showLoading: showLoading,
//        setFooter: setFooter
//    };
//})();
// SmartAdmin/wwwroot/js/app-modal.js
const AppModal = (function () {
    let modalInstance = null;

    function init() {
        if (!modalInstance) {
            modalInstance = new bootstrap.Modal(document.getElementById('globalModal'));
        }
    }

    /**
     * Inicializa Select2 en los elementos select dentro del modal
     * @param {string} selector - Selector CSS para los select (default: 'select')
     * @param {Object} options - Opciones personalizadas de Select2
     */
    function initSelect2(selector = 'select', options = {}) {
        const defaultOptions = {
            theme: 'bootstrap-5',
            width: '100%',
            dropdownParent: $('#globalModal'),
            language: 'es',
            placeholder: 'Seleccione una opción',
            allowClear: true
        };

        const mergedOptions = { ...defaultOptions, ...options };

        $('#globalModal').find(selector).each(function () {
            // Destruir instancia previa si existe
            if ($(this).hasClass('select2-hidden-accessible')) {
                $(this).select2('destroy');
            }
            $(this).select2(mergedOptions);
        });
    }

    /**
     * Abre el modal
     * @param {Object} config
     * @param {string} config.title - Título
     * @param {string} config.url - URL de la vista parcial
     * @param {string} config.size - 'sm' | 'md' | 'lg' | 'xl' | 'fullscreen' (default: 'md')
     * @param {Object} config.data - Datos para enviar a la vista parcial (opcional)
     * @param {Function} config.onShown - Callback cuando se muestra (opcional)
     * @param {Function} config.onHidden - Callback cuando se cierra (opcional)
     * @param {boolean|Object} config.select2 - Configuración de Select2 (opcional)
     *   - true: Inicializa con opciones por defecto
     *   - {selector: 'select', options: {...}}: Configuración personalizada
     */
    function open(config) {
        init();

        const {
            title = '',
            url = '',
            size = 'md',
            data = null,
            onShown = null,
            onHidden = null,
            select2 = false
        } = config;

        // Título
        $('#globalModalTitle').html(title);

        // Tamaño
        const $dialog = $('#globalModalDialog');
        $dialog.removeClass('modal-sm modal-lg modal-xl modal-fullscreen');
        if (size !== 'md') {
            $dialog.addClass('modal-' + size);
        }

        // Ocultar footer por defecto
        $('#globalModalFooter').hide();

        // Mostrar loading
        showLoading();

        // Mostrar modal
        modalInstance.show();

        // Cargar vista parcial
        $.ajax({
            url: url,
            type: 'GET',
            data: data,
            success: function (html) {
                $('#globalModalBody').html(html);

                // Inicializar Select2 si está configurado
                if (select2) {
                    if (select2 === true) {
                        // Configuración por defecto
                        initSelect2();
                    } else if (typeof select2 === 'object') {
                        // Configuración personalizada
                        const selector = select2.selector || 'select';
                        const options = select2.options || {};
                        initSelect2(selector, options);
                    }
                }
            },
            error: function () {
                $('#globalModalBody').html(`
                    <div class="alert alert-danger mb-0">
                        <i class="fas fa-exclamation-circle me-2"></i>
                        Error al cargar el contenido
                    </div>
                `);
            }
        });

        // Eventos
        const $modal = $('#globalModal');
        $modal.off('shown.bs.modal hidden.bs.modal');

        if (onShown) {
            $modal.on('shown.bs.modal', onShown);
        }

        if (onHidden) {
            $modal.on('hidden.bs.modal', function () {
                // Destruir instancias de Select2 al cerrar
                $('#globalModal').find('.select2-hidden-accessible').select2('destroy');
                if (onHidden) onHidden();
            });
        } else {
            // Destruir Select2 por defecto al cerrar
            $modal.on('hidden.bs.modal', function () {
                $('#globalModal').find('.select2-hidden-accessible').select2('destroy');
            });
        }
    }

    function close() {
        if (modalInstance) {
            modalInstance.hide();
        }
    }

    function showLoading() {
        $('#globalModalBody').html(`
            <div class="text-center py-4">
                <div class="spinner-border text-primary"></div>
                <p class="mt-2 mb-0 text-muted">Cargando...</p>
            </div>
        `);
    }

    function setFooter(html) {
        if (html) {
            $('#globalModalFooter').html(html).show();
        } else {
            $('#globalModalFooter').hide();
        }
    }

    return {
        open: open,
        close: close,
        showLoading: showLoading,
        setFooter: setFooter,
        initSelect2: initSelect2 // Exponer por si se necesita inicializar manualmente
    };
})();
//Ejemplos de uso:
// 1. Sin Select2
//AppModal.open({
//    title: 'Nuevo Usuario',
//    url: '/Users/Create'
//});

//// 2. Con Select2 (configuración por defecto)
//AppModal.open({
//    title: 'Nuevo Usuario',
//    url: '/Users/Create',
//    select2: true
//});

//// 3. Con Select2 personalizado (selector y opciones específicas)
//AppModal.open({
//    title: 'Nuevo Usuario',
//    url: '/Users/Create',
//    select2: {
//        selector: '.custom-select', // Solo select con clase específica
//        options: {
//            theme: 'bootstrap-5',
//            placeholder: 'Seleccione...',
//            allowClear: false,
//            minimumResultsForSearch: 10
//        }
//    }
//});

//// 4. Inicializar Select2 manualmente después de cargar contenido dinámico
//AppModal.open({
//    title: 'Formulario',
//    url: '/Form/Load',
//    onShown: function () {
//        // Cargar algo dinámicamente
//        $.get('/api/data', function (data) {
//            $('#dynamicContent').html(data);
//            // Inicializar Select2 en el nuevo contenido
//            AppModal.initSelect2('.dynamic-select');
//        });
//    }
//});