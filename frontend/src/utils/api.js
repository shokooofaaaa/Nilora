// JavaScript source code


import { API_BASE_URL } from '../config.js';

async function request(endpoint, options = {}) {
    const { token, ...fetchOptions } = options;

    const headers = {
        'Content-Type': 'application/json',
        ...fetchOptions.headers,
    };

    if (token) {
        headers.Authorization = `Bearer ${token}`;
    }

    let response;

    try {
        response = await fetch(`${API_BASE_URL}${endpoint}`, {
            ...fetchOptions,
            headers,
        });
    } catch {
        throw new Error(
            'ارتباط با سرور برقرار نشد. اتصال اینترنت و وضعیت API را بررسی کنید.'
        );
    }

    const responseText = await response.text();

    let data = null;

    if (responseText) {
        try {
            data = JSON.parse(responseText);
        } catch {
            data = responseText;
        }
    }

    if (!response.ok) {
        const message =
            typeof data === 'object' && data !== null
                ? data.message || data.title || data.detail
                : null;

        throw new Error(
            message || `خطای سرور: ${response.status}`
        );
    }

    return data;
}

export const authApi = {
    register: (userData) =>
        request('/api/Auth/register', {
            method: 'POST',
            body: JSON.stringify(userData),
        }),

    login: (credentials) =>
        request('/api/Auth/login', {
            method: 'POST',
            body: JSON.stringify(credentials),
        }),

    getMe: (token) =>
        request('/api/Auth/me', {
            method: 'GET',
            token,
        }),
};
