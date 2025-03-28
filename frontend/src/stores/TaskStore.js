import { defineStore } from 'pinia'
import ApiService from '@/services/ApiService'

export const useTaskStore = defineStore('task', {
  state: () => ({
    tasks: [],
    isLoading: false,
  }),
  getters: {},
  actions: {
    async fetchTasks() {
      this.isLoading = true
      try {
        const response = await ApiService.get('/task')
        this.tasks = response.data.data
        console.log('Tasks fetched successfully:', this.tasks)
      } catch (error) {
        console.error('Error fetching tasks:', error)
      } finally {
        this.isLoading = false
      }
    },
  },
})
