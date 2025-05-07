import ApiService from './ApiService'

export default {
  async login(data) {
    const response = await ApiService.post(`login`,
      data
    )
    if (response.status !== 201) {
      throw new Error('Error logging in')
    }
    return response.data
  },

  async createUser(data) {
    try {
      const response = await ApiService.post('user', data)
      return response.data
    } catch (error) {
      // Se a API retornar mensagens de erro, tente extrair e relançar
      const message =
        error.response?.data?.message ||
        error.response?.data?.title || // caso use Problem Details
        'Erro ao criar o usuário'

      throw new Error(message)
    }
  }

}