const UsersPage = (function () {

    const API = {
        getAll: '/api/Auth/GetAllUsers',
        getById: (id) => `/api/Auth/GetUserById/${id}`,      // ✅ Template literal correcto
        create: '/api/Auth/create-user',
        update: (id) => `/api/Auth/Update-User/${id}`,       // ✅ Template literal correcto
        resetPassword: (id) => `/api/Auth/Reset-Password/${id}`  // ✅ Template literal correcto
    };

    let dataTable;
    let allRoles = [];

    async function init() {
        await loadRoles();
        initDataTable();
    }

    async function loadRoles() {
        const result = await Api.get('/api/Roles');
        if (result.success && result.data) {
            // ✅ Verificar que data sea un array
            allRoles = Array.isArray(result.data)
                ? result.data.map(r => r.name)
                : [];
            console.log('Roles cargados:', allRoles);
        } else {
            console.warn('No se pudieron cargar los roles:', result.message);
            allRoles = [];
        }
    }


    function initDataTable() {
        dataTable = $('#dt-users').DataTable({
            ajax: {
                url: API.getAll,
                dataSrc: function (json) {
                    console.log('Respuesta API:', json);  // Debug
                    if (json.success && json.data) {
                        return json.data;
                    }
                    return [];
                },
                error: function (xhr, error, thrown) {
                    console.error('DataTable Error:', error, thrown);
                    Toast.error('Error al cargar usuarios');
                }
            },
            columns: [
                { data: 'nombreCompleto' },
                { data: 'email' },
                { data: 'cargo', defaultContent: 'N/A' },
                {
                    data: 'roles',
                    render: (data) => {
                        if (!data?.length) return '<span class="badge bg-secondary">Sin roles</span>';
                        return data.map(r => `<span class="badge bg-primary me-1">${r}</span>`).join('');
                    }
                },
                {
                    data: 'activo',
                    render: (data) => `<span class="badge bg-${data ? 'success' : 'danger'}">${data ? 'Activo' : 'Inactivo'}</span>`
                },
                {
                    data: null,
                    orderable: false,
                    className: 'text-center',
                    render: (data) => `
                        <div class="btn-group btn-group-sm">
                            <button class="btn btn-outline-warning" onclick="UsersPage.edit('${data.id}')" title="Editar">
                                <i class="fas fa-edit"></i>
                            </button>
                            <button class="btn btn-outline-info" onclick="UsersPage.resetPassword('${data.id}', '${data.nombreCompleto}')" title="Reset Password">
                                <i class="fas fa-key"></i>
                            </button>
                            <button class="btn btn-outline-${data.activo ? 'danger' : 'success'}" 
                                    onclick="UsersPage.toggleStatus('${data.id}', '${data.nombreCompleto}', ${data.activo})" 
                                    title="${data.activo ? 'Desactivar' : 'Activar'}">
                                <i class="fas fa-${data.activo ? 'user-slash' : 'user-check'}"></i>
                            </button>
                        </div>`
                }
            ],
            language: { url: '//cdn.datatables.net/plug-ins/1.13.7/i18n/es-ES.json' },
            responsive: true
        });
    }
    //crear
    function create() {
        Modal.form({
            title: 'Nuevo Usuario',
            icon: 'fa-user-plus',
            size: 'lg',
            headerClass: 'bg-primary',
            fields: [
                { name: 'nombreCompleto', label: 'Nombre Completo', type: 'text', required: true, col: 6 },
                { name: 'email', label: 'Email', type: 'email', required: true, col: 6 },
                { name: 'password', label: 'Contraseña', type: 'password', required: true, col: 6 },
                { name: 'cargo', label: 'Cargo', type: 'text', col: 6 },
                { name: 'departamento', label: 'Departamento', type: 'text', col: 6 },
                { name: 'roles', label: 'Roles', type: 'select', multiple: true, options: allRoles, col: 6 },
                { name: 'activo', label: 'Usuario Activo', type: 'checkbox', checked: true, col: 12 }
            ],
            onSubmit: async (data, modalId) => {
                const result = await Api.post(API.create, {
                    email: data.email,
                    password: data.password,
                    nombreCompleto: data.nombreCompleto,
                    cargo: data.cargo || null,
                    departamento: data.departamento || null,
                    activo: data.activo,
                    roles: data.roles || []
                });

                if (result.success) {
                    Toast.success('Usuario creado correctamente');
                    Modal.close(modalId);
                    dataTable.ajax.reload();
                } else {
                    Toast.error(result.message);
                }
            }
        });
    }
    async function edit(id) {
        const result = await Api.get(API.getById(id));
        if (!result.success) {
            Toast.error(result.message);
            return;
        }

        const user = result.data;
        Modal.form({
            title: 'Editar Usuario',
            icon: 'fa-user-edit',
            size: 'lg',
            headerClass: 'bg-warning',
            fields: [
                { name: 'id', type: 'hidden', value: user.id },
                { name: 'nombreCompleto', label: 'Nombre Completo', type: 'text', value: user.nombreCompleto, col: 6 },
                { name: 'email', label: 'Email', type: 'email', value: user.email, col: 6, readonly: true },
                { name: 'roles', label: 'Roles', type: 'select', multiple: true, options: allRoles, value: user.roles, col: 6 },
                { name: 'activo', label: 'Usuario Activo', type: 'checkbox', checked: user.activo, col: 6 }
            ],
            confirmText: 'Actualizar',
            confirmClass: 'btn-warning',
            onSubmit: async (data, modalId) => {
                const result = await Api.put(API.update(data.id), {
                    nombreCompleto: data.nombreCompleto,
                    activo: data.activo,
                    roles: data.roles || []
                });

                if (result.success) {
                    Toast.success('Usuario actualizado correctamente');
                    Modal.close(modalId);
                    dataTable.ajax.reload();
                } else {
                    Toast.error(result.message);
                }
            }
        });
    }

    function resetPassword(id, nombre) {
        Modal.form({
            title: 'Resetear Contraseña',
            icon: 'fa-key',
            headerClass: 'bg-info',
            bodyPrefix: `
                <div class="alert alert-warning">
                    <i class="fal fa-exclamation-triangle me-2"></i>
                    Resetear contraseña para: <strong>${nombre}</strong>
                </div>`,
            fields: [
                { name: 'userId', type: 'hidden', value: id },
                { name: 'newPassword', label: 'Nueva Contraseña', type: 'password', required: true }
            ],
            confirmText: 'Resetear',
            confirmClass: 'btn-info',
            confirmIcon: 'fa-key',
            onSubmit: async (data, modalId) => {
                const result = await Api.post(API.resetPassword(data.userId), {
                    newPassword: data.newPassword
                });

                if (result.success) {
                    Toast.success('Contraseña reseteada correctamente');
                    Modal.close(modalId);
                } else {
                    Toast.error(result.message);
                }
            }
        });
    }

    async function toggleStatus(id, nombre, currentStatus) {
        const action = currentStatus ? 'desactivar' : 'activar';
        const confirmed = await Modal.confirm(
            `¿Está seguro de ${action} al usuario "${nombre}"?`,
            {
                title: currentStatus ? 'Desactivar Usuario' : 'Activar Usuario',
                headerClass: currentStatus ? 'bg-danger' : 'bg-success',
                confirmText: `Sí, ${action}`,
                confirmClass: currentStatus ? 'btn-danger' : 'btn-success'
            }
        );

        if (confirmed) {
            const result = await Api.put(API.update(id), { activo: !currentStatus });
            if (result.success) {
                Toast.success(`Usuario ${action}do correctamente`);
                dataTable.ajax.reload();
            } else {
                Toast.error(result.message);
            }
        };
    }

    return { init, create, edit, resetPassword, toggleStatus };
    })
    ();

$(document).ready(() => UsersPage.init());