import axios from 'axios';
import qs from 'qs';

class ApiService {
  constructor(baseURL) {
    this.api = axios.create({
      baseURL,
      headers: {
        'Content-Type': 'application/json',
      },
      paramsSerializer: params => qs.stringify(params, { arrayFormat: 'repeat' })
    });

    this.api.interceptors.request.use((config) => {
      const token = localStorage.getItem('token');
      if (config.headers.requiresToken && token) {
        config.headers['Authorization'] = `Bearer ${token}`;
      }
      delete config.headers.requiresToken;
      return config;
    });

    this.api.interceptors.response.use(
      (response) => response,
      (error) => {
        if (error.response && error.response.status === 401) {
          localStorage.removeItem('token');
          window.location.href = '/';
        }
        return Promise.reject(error);
      }
    );
  }

  // Método GET
  get(endpoint, config = {}) {
    return this.api.get(endpoint, config);
  }

  // Método POST
  post(endpoint, data, config = {}) {
    return this.api.post(endpoint, data, config);
  }

  // Método PUT
  put(endpoint, data, config = {}) {
    return this.api.put(endpoint, data, config);
  }

  // Método DELETE
  delete(endpoint, config = {}) {
    return this.api.delete(endpoint, config);
  }


  // Método para download de arquivos PDF
  async downloadPDF(endpoint, config = {}) {
    const downloadConfig = {
      ...config,
      responseType: 'blob',
    };

    return this.api.get(endpoint, downloadConfig).then(response => {
      const blob = new Blob([response.data], { type: 'application/pdf' });
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');

      const disposition = response.headers['content-disposition'];
      let filename = 'download.pdf';
      if (disposition) {
        const match = disposition.match(/filename\*?=['"]?(?:UTF-\d+''|)([^;\r\n]*)/i);
        if (match) filename = decodeURIComponent(match[1]);
      }

      link.href = url;
      link.setAttribute('download', filename);
      document.body.appendChild(link);
      link.click();
      link.remove();
      URL.revokeObjectURL(url);
    });
  }
}

const API_URL = import.meta.env.VITE_API_URL;

export default new ApiService(API_URL);