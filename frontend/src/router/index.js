import { createRouter, createWebHistory } from 'vue-router'
import LoginView from '../Views/LoginView.vue'
import TasksView from '../Views/TasksView.vue'
import RegisterView from '@/Views/RegisterView.vue'

const routes = [
  { path: '/', redirect: '/login' },
  { path: '/login', component: LoginView },
  { path: '/tasks', component: TasksView },
  { path: '/register', component: RegisterView },
]

const router = createRouter({
  history: createWebHistory(),
  routes,
})

export default router
