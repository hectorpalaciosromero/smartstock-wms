import axios from 'axios';

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5000/api';

const api = axios.create({
  baseURL: API_URL,
  headers: { 'Content-Type': 'application/json' }
});

// Add JWT token to requests
api.interceptors.request.use((config) => {
  const token = localStorage.getItem('smartstock_token');
  if (token) config.headers.Authorization = `Bearer ${token}`;
  return config;
});

// Handle 401 responses — auto-logout on expired/invalid token
api.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === 401) {
      const currentPath = window.location.pathname;
      // Don't redirect if already on login page or if it's a login/register request
      if (currentPath !== '/login' && !error.config?.url?.includes('/Auth/')) {
        localStorage.removeItem('smartstock_token');
        localStorage.removeItem('smartstock_user');
        window.location.href = '/login';
      }
    }
    return Promise.reject(error);
  }
);

// Auth
export const authAPI = {
  login: (data) => api.post('/Auth/login', data),
  register: (data) => api.post('/Auth/register', data),
  getUsers: () => api.get('/Auth/users'),
  updateRole: (userId, data) => api.put(`/Auth/users/${userId}/role`, data),
  toggleStatus: (userId) => api.put(`/Auth/users/${userId}/toggle-status`),
  getProfile: () => api.get('/Auth/profile'),
  updateProfile: (data) => api.put('/Auth/profile', data),
  changePassword: (data) => api.post('/Auth/change-password', data),
};

// Products
export const productsAPI = {
  getAll: (params) => api.get('/Products', { params }),
  getById: (id) => api.get(`/Products/${id}`),
  getByBarcode: (barcode) => api.get(`/Products/barcode/${barcode}`),
  create: (data) => api.post('/Products', data),
  update: (id, data) => api.put(`/Products/${id}`, data),
  delete: (id) => api.delete(`/Products/${id}`),
  export: (params) => api.get('/Products/export', { params, responseType: 'blob' }),
  restore: (id) => api.put(`/Products/${id}/restore`),
  batchAction: (data) => api.post('/Products/batch', data),
};

// Categories
export const categoriesAPI = {
  getAll: (params) => api.get('/Categories', { params }),
  getById: (id) => api.get(`/Categories/${id}`),
  create: (data) => api.post('/Categories', data),
  update: (id, data) => api.put(`/Categories/${id}`, data),
  delete: (id) => api.delete(`/Categories/${id}`),
  restore: (id) => api.put(`/Categories/${id}/restore`),
};

// Suppliers
export const suppliersAPI = {
  getAll: (params) => api.get('/Suppliers', { params }),
  create: (data) => api.post('/Suppliers', data),
  update: (id, data) => api.put(`/Suppliers/${id}`, data),
  delete: (id) => api.delete(`/Suppliers/${id}`),
  restore: (id) => api.put(`/Suppliers/${id}/restore`),
};

// Warehouses
export const warehousesAPI = {
  getAll: (params) => api.get('/Warehouses', { params }),
  create: (data) => api.post('/Warehouses', data),
  update: (id, data) => api.put(`/Warehouses/${id}`, data),
  delete: (id) => api.delete(`/Warehouses/${id}`),
  restore: (id) => api.put(`/Warehouses/${id}/restore`),
};

// Movements
export const movementsAPI = {
  getAll: (params) => api.get('/Movements', { params }),
  create: (data) => api.post('/Movements', data),
  export: (params) => api.get('/Movements/export', { params, responseType: 'blob' }),
};

// Scan
export const scanAPI = {
  process: (data) => api.post('/Scan', data),
  getRecent: (count = 20) => api.get('/Scan/recent', { params: { count } }),
};

// Dashboard
export const dashboardAPI = {
  get: () => api.get('/Dashboard'),
};

export default api;
