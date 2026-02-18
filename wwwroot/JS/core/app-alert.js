const AppAlert = (function () {

    const defaults = {
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Confirmar',
        cancelButtonText: 'Cancelar',
        customClass: {
            popup: 'shadow-lg',
            confirmButton: 'btn btn-primary px-4',
            cancelButton: 'btn btn-secondary px-4'
        },
        buttonsStyling: false
    };

    function confirm(options) {
        const config = {
            title: options.title || 'Confirmar accion',
            text: options.text || '',
            html: options.html || '',
            icon: options.icon || 'question',
            showCancelButton: true,
            confirmButtonText: options.confirmText || defaults.confirmButtonText,
            cancelButtonText: options.cancelText || defaults.cancelButtonText,
            ...defaults
        };

        return Swal.fire(config).then(result => {
            if (result.isConfirmed && options.onConfirm) {
                return options.onConfirm();
            }
            return result;
        });
    }

    function eliminar(options) {
        const config = {
            title: options.title || 'Eliminar',
            text: options.text || 'Esta accion no se puede deshacer.',
            html: options.html || '',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: options.confirmText || 'Si, eliminar',
            cancelButtonText: options.cancelText || 'Cancelar',
            ...defaults,
            confirmButtonColor: '#dc3545',
            customClass: {
                ...defaults.customClass,
                confirmButton: 'btn btn-danger px-4'
            }
        };

        return Swal.fire(config).then(result => {
            if (result.isConfirmed && options.onConfirm) {
                return options.onConfirm();
            }
            return result;
        });
    }

    function input(options) {
        const config = {
            title: options.title || 'Ingrese valor',
            input: options.inputType || 'text',
            inputLabel: options.label || '',
            inputPlaceholder: options.placeholder || '',
            inputValue: options.value || '',
            showCancelButton: true,
            confirmButtonText: options.confirmText || 'Aceptar',
            cancelButtonText: options.cancelText || 'Cancelar',
            inputValidator: options.required !== false ? (value) => {
                if (!value) return 'Este campo es requerido';
            } : undefined,
            ...defaults
        };

        return Swal.fire(config).then(result => {
            if (result.isConfirmed && options.onConfirm) {
                return options.onConfirm(result.value);
            }
            return result;
        });
    }

    function success(title, text) {
        return Swal.fire({
            icon: 'success',
            title: title || 'Operacion exitosa',
            text: text || '',
            timer: 2000,
            showConfirmButton: false,
            ...defaults
        });
    }

    function error(title, text) {
        return Swal.fire({
            icon: 'error',
            title: title || 'Error',
            text: text || '',
            ...defaults
        });
    }

    return {
        confirm: confirm,
        eliminar: eliminar,
        input: input,
        success: success,
        error: error
    };
})();

window.AppAlert = AppAlert;
