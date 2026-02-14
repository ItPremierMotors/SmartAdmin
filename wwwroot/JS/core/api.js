window.Api = (function (w) {  // Patrón módulo - encapsula todo

    /**
     * Función principal que hace las peticiones HTTP
     * @param {string} url - URL del endpoint
     * @param {object} options - Opciones de fetch (method, body, headers)
    */


    const API_BASE = w.API_BASE || 'http://localhost:5036';

    function toAbsoluteUrl(url) {
        if (!url) throw new Error('url requerida');
        try {
            return new URL(url, API_BASE).toString();
        } catch (e) {
            throw new Error(`URL inválida: ${url}`);
        }
    }

    async function request(url, options = {}) {

        // Headers por defecto que se envían en TODAS las peticiones
        const defaultOptions = {
            headers: {
                'Content-Type': 'application/json',        // Enviamos JSON
                'X-Requested-With': 'XMLHttpRequest'       // Indica que es AJAX
            },
             credentials: 'include' // Descomenta si usas cookies/autenticación basada en cookies      // credentials: 'include' // Descomenta si usas cookies/autenticación basada en cookies
        };

        // Combinar opciones por defecto con las que se pasen
        const config = {
            ...defaultOptions,
            ...options,
            headers: { ...defaultOptions.headers, ...options.headers }
        };

        // Si el body es un objeto, convertirlo a JSON string
        // Convertir body a JSON si es objeto
        if (options.body && typeof options.body === 'object') {
            try {
                config.body = JSON.stringify(options.body);
            } catch (e) {
                return {
                    success: false,
                    statusCode: 0,
                    message: 'No se pudo convertir el body a JSON',
                    error: e.message
                };
            }
        }
        const finalUrl = toAbsoluteUrl(url);

        try {
            const response = await fetch(finalUrl, config);
            const contentType = response.headers.get('content-type') || '';

            // Intentar parsear JSON si aplica
            let data = null;
            if (contentType.includes('application/json')) {
                try {
                    data = await response.json();
                } catch (e) {
                    return {
                        success: false,
                        statusCode: response.status,
                        message: 'JSON inválido recibido del servidor',
                        error: e.message
                    };
                }
            } else {
                // Texto plano
                data = await response.text();
            }

            // Manejar status HTTP de error
            if (!response.ok) {
                return {
                    success: false,
                    statusCode: response.status,
                    message: data?.message || response.statusText,
                    data
                };
            }
            // ✅ Si la API devuelve ApiResponse, usar esa estructura
            if (data && typeof data === 'object' && 'success' in data) {
                return data;
            }


            return {
                success: true,
                statusCode: response.status,
                data
            };

        } catch (error) {
            console.error('API Error:', error);
            return {
                success: false,
                statusCode: 500,
                message: 'Error de conexión con el servidor',
                error: error.message
            };
        }
    }

    // API PÚBLICA - Métodos que puedes usar desde afuera
    return {
        // GET - Obtener datos
        get(url) {
            return request(url, { method: 'GET' });
        },

        // POST - Crear datos
        post(url, data) {
            return request(url, { method: 'POST', body: data });
        },

        // PUT - Actualizar datos
        put(url, data) {
            return request(url, { method: 'PUT', body: data });
        },

        // DELETE - Eliminar datos
        delete(url) {
            return request(url, { method: 'DELETE' });
        },

        // POST con FormData (para archivos)
        postForm(url, formData) {
            const finalUrl = toAbsoluteUrl(url);
            return fetch(finalUrl, {
                method: 'POST',
                body: formData,
                headers: { 'X-Requested-With': 'XMLHttpRequest' },
                credentials: 'include'
            })
                .then(async r => {
                    let data;
                    const contentType = r.headers.get('content-type') || '';
                    if (contentType.includes('application/json')) {
                        try { data = await r.json(); }
                        catch { data = await r.text(); }
                    } else {
                        data = await r.text();
                    }

                    if (!r.ok) {
                        return { success: false, statusCode: r.status, message: r.statusText, data };
                    }
                    return { success: true, statusCode: r.status, data };
                })
                .catch(error => ({
                    success: false,
                    statusCode: 500,
                    message: 'Error de conexión con el servidor',
                    error: error.message
                }));
        }
    };

})(window);

/*
// Obtener todos los usuarios
const result = await Api.get('/api/Auth/GetAllUsers');

if (result.success) {
    console.log('Usuarios:', result.data);
} else {
    console.log('Error:', result.message);
}

// Obtener un usuario por ID
const user = await Api.get('/api/Auth/GetUserById/abc123');

// Obtener todos los roles
const roles = await Api.get('/api/Roles');

// Crear un usuario
const result = await Api.post('/api/Auth/create-user', {
    email: 'juan@test.com',
    password: 'Password123!',
    nombreCompleto: 'Juan Pérez',
    cargo: 'Desarrollador',
    activo: true,
    roles: ['User', 'Manager']
});

if (result.success) {
    console.log('Usuario creado con ID:', result.data);
} else {
    console.log('Error:', result.message);
}

// Crear un rol
const result = await Api.post('/api/Roles', {
    name: 'Supervisor'
});

// Actualizar un usuario
const result = await Api.put('/api/Auth/Update-User/abc123', {
    nombreCompleto: 'Juan Carlos Pérez',
    activo: true,
    roles: ['Admin', 'Manager']
});

// Actualizar un rol
const result = await Api.put('/api/Roles/rol123', {
    name: 'SupervisorGeneral'
});

*/