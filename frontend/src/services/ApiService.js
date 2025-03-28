import axios from 'axios'

class ApiService {
  constructor(baseURL) {
    this.api = axios.create({
      baseURL,
      headers: {
        'Content-Type': 'application/json',
      },
    })

    this.isRefreshing = false
    this.refreshSubscribers = []
  }

  async get(endpoint, config = {}) {
    return await this.api.get(endpoint, config)
  }
}
const API_URL = import.meta.env.VITE_API_URL
export default new ApiService('https://localhost:7201')
