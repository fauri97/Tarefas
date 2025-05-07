import { defineStore } from 'pinia'
import authService from '@/services/AuthService'

export const useAuthStore = defineStore('auth', {
    state: () => ({
        user: null,
        token: null,
        isLoading: false,
    }),
    getters: {},
    actions: {
        async login(data) {
            this.isLoading = true
            try {
                const response = await authService.login(data)

                this.token = response.data.accessToken
                this.user = response.data
                localStorage.setItem('authUser', JSON.stringify(this.authUser))
                localStorage.setItem('token', this.token)

                return true;
            } catch (error) {
                console.error('Error fetching user:', error)
            } finally {
                this.isLoading = false
            }
        },

        async createUser(data) {
            this.isLoading = true
            try {
              const response = await authService.createUser(data)
          
              return true
            } catch (error) {
              console.error('Erro ao criar usuário:', error)
              throw error
            } finally {
              this.isLoading = false
            }
          }          
    },
});