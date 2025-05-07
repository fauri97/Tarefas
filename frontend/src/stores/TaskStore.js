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
        const response = await ApiService.get('/task', {
          headers: {
            requiresToken: true,
          }
        })
        this.tasks = response.data.data
      } catch (error) {
        console.error('Error fetching tasks:', error)
      } finally {
        this.isLoading = false
      }
    },

    async createTask(task) {
      try {
        await ApiService.post('/task', task, {
          headers: { requiresToken: true },
        });
        await this.fetchTasks();
      } catch (error) {
        throw error.response?.data?.errors?.[0] || 'Erro ao criar tarefa';
      }
    },

    async updateTask(task) {
      try {
        await ApiService.put(`/task/${task.id}`, task, {
          headers: { requiresToken: true },
        });
        await this.fetchTasks();
      } catch (error) {
        throw error.response?.data?.errors?.[0] || 'Erro ao atualizar tarefa';
      }
    },

    async deleteTask(id) {
      await ApiService.delete(`/task/${id}`, {
        headers: { requiresToken: true },
      })
      await this.fetchTasks();
    },

    async toggleTaskStatus(id) {
      await ApiService.put(`/task/close/${id}`,{}, {
        headers: { requiresToken: true },
      })
      await this.fetchTasks();
    },

    async downloadTaskPDF(ids) {
      await ApiService.downloadPDF('/task/export/pdf', {
        headers: { requiresToken: true },
        params: { ids },
      })
    }
  }
})
