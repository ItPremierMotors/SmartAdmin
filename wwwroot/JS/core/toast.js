const Toast = (function () {

    function getContainer() {
        let container = document.getElementById('toast-container');
        if (!container) {
            container = document.createElement('div');
            container.id = 'toast-container';
            container.className = 'position-fixed top-0 end-0 p-3';
            container.style.zIndex = '9999';
            document.body.appendChild(container);
        }
        return container;
    }

    function show(type, title, message) {
        const icons = {
            success: 'fa-check-circle text-success',
            error: 'fa-times-circle text-danger',
            warning: 'fa-exclamation-triangle text-warning',
            info: 'fa-info-circle text-info'
        };

        const id = `toast_${Date.now()}`;
        const html = `
        <div id="${id}" class="toast" role="alert">
            <div class="toast-header">
                <i class="fal ${icons[type]} me-2"></i>
                <strong class="me-auto">${title}</strong>
                <button type="button" class="btn-close" data-bs-dismiss="toast"></button>
            </div>
            <div class="toast-body">${message}</div>
        </div>`;

        getContainer().insertAdjacentHTML('beforeend', html);

        const el = document.getElementById(id);
        const toast = new bootstrap.Toast(el, { delay: 4000 });
        el.addEventListener('hidden.bs.toast', () => el.remove());
        toast.show();
    }

    return {
        success: (msg, title = 'Éxito') => show('success', title, msg),
        error: (msg, title = 'Error') => show('error', title, msg),
        warning: (msg, title = 'Advertencia') => show('warning', title, msg),
        info: (msg, title = 'Información') => show('info', title, msg)
    };
})();

window.Toast = Toast;