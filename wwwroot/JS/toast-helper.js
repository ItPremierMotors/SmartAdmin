//// ============================================
//// PASO 1: Crear archivo wwwroot/js/toast-helper.js
//// ============================================

//const Toast = {
//    // Configuración por defecto
//    config: {
//        timeout: 5000,
//        position: 'topRight',
//        transitionIn: 'fadeInDown',
//        transitionOut: 'fadeOutUp',
//        progressBar: true,
//        closeOnClick: true,
//        pauseOnHover: true,
//        closeOnEscape: true,
//        resetOnHover: true
//    },



//    // Éxito
//    success: function (message, title = 'Éxito') {
//        iziToast.success({
//            title: title,
//            message: message,
//            icon: 'ico-success'
//        });
//    },

//    // Error
//    error: function (message, title = 'Error') {
//        iziToast.error({
//            title: title,
//            message: message,
//            icon: 'ico-error'
//        });
//    },

//    // Advertencia
//    warning: function (message, title = 'Advertencia') {
//        iziToast.warning({
//            title: title,
//            message: message,
//            icon: 'ico-warning'
//        });
//    },

//    // Información
//    info: function (message, title = 'Información') {
//        iziToast.info({
//            title: title,
//            message: message,
//            icon: 'ico-info'
//        });
//    },

//    // Pregunta/Confirmación
//    question: function (message, onConfirm, onCancel) {
//        iziToast.question({
//            timeout: false,
//            close: false,
//            overlay: true,
//            displayMode: 'once',
//            id: 'question',
//            zindex: 999,
//            title: 'Confirmar',
//            message: message,
//            position: 'center',
//            buttons: [
//                ['<button><b>SÍ</b></button>', function (instance, toast) {
//                    instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
//                    if (onConfirm) onConfirm();
//                }, true],
//                ['<button>NO</button>', function (instance, toast) {
//                    instance.hide({ transitionOut: 'fadeOut' }, toast, 'button');
//                    if (onCancel) onCancel();
//                }]
//            ]
//        });
//    },

//    // Loading (persistente hasta que se cierre manualmente)
//    loading: function (message = 'Procesando...', id = 'loading') {
//        iziToast.info({
//            id: id,
//            title: 'Cargando',
//            message: message,
//            timeout: false,
//            close: false,
//            overlay: true,
//            progressBar: false,
//            icon: 'ico-loading'
//        });
//    },

//    // Cerrar loading específico
//    hideLoading: function (id = 'loading') {
//        iziToast.hide({}, document.querySelector('#' + id));
//    },

//    // Cerrar todas las notificaciones
//    hideAll: function () {
//        iziToast.destroy();
//    },

//    // Toast personalizado
//    show: function (options) {
//        iziToast.show(options);
//    },

//    // Con botones personalizados
//    withButtons: function (message, buttons, title = 'Acción requerida') {
//        iziToast.show({
//            title: title,
//            message: message,
//            position: 'center',
//            timeout: false,
//            buttons: buttons
//        });
//    },

//    // Tema oscuro
//    dark: {
//        success: function (message, title = 'Éxito') {
//            iziToast.success({
//                title: title,
//                message: message,
//                theme: 'dark',
//                backgroundColor: '#1f2937',
//                titleColor: '#fff',
//                messageColor: '#fff'
//            });
//        },
//        error: function (message, title = 'Error') {
//            iziToast.error({
//                title: title,
//                message: message,
//                theme: 'dark',
//                backgroundColor: '#1f2937',
//                titleColor: '#fff',
//                messageColor: '#fff'
//            });
//        },
//        warning: function (message, title = 'Advertencia') {
//            iziToast.warning({
//                title: title,
//                message: message,
//                theme: 'dark',
//                backgroundColor: '#1f2937',
//                titleColor: '#fff',
//                messageColor: '#fff'
//            });
//        },
//        info: function (message, title = 'Información') {
//            iziToast.info({
//                title: title,
//                message: message,
//                theme: 'dark',
//                backgroundColor: '#1f2937',
//                titleColor: '#fff',
//                messageColor: '#fff'
//            });
//        }
//    }
//};



//// Exponer globalmente
//window.Toast = Toast;

// ================================
// Toast Helper Global (iziToast)
// ================================
window.Toast = {

    // Configuración global
    config: {
        timeout: 5000,
        position: 'topRight',
        transitionIn: 'fadeInDown',
        transitionOut: 'fadeOutUp',
        progressBar: true,
        closeOnClick: true,
        pauseOnHover: true,
        closeOnEscape: true,
        resetOnHover: true
    },

    // Mezcla config global + opciones locales
    _buildOptions: function (options) {
        return Object.assign({}, this.config, options);
    },

    // Métodos públicos
    success: function (message, title = 'Éxito') {
        iziToast.show(this._buildOptions({
            title: title,
            message: message,
            color: 'green',
            icon: 'ico-success'
        }));
    },

    error: function (message, title = 'Error') {
        iziToast.show(this._buildOptions({
            title: title,
            message: message,
            color: 'red',
            icon: 'ico-error'
        }));
    },

    warning: function (message, title = 'Advertencia') {
        iziToast.show(this._buildOptions({
            title: title,
            message: message,
            color: 'yellow',
            icon: 'ico-warning'
        }));
    },

    info: function (message, title = 'Información') {
        iziToast.show(this._buildOptions({
            title: title,
            message: message,
            color: 'blue',
            icon: 'ico-info'
        }));
    },

    // Método libre
    show: function (options) {
        iziToast.show(this._buildOptions(options));
    }
};
