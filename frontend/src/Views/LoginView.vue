<template>
  <div class="flex justify-center items-center h-screen bg">
    <form @submit.prevent="handleLogin" class="bg-fuchsia-950 p-6 rounded shadow-md w-full max-w-sm">
      <h2 class="text-2xl font-bold mb-4 text-center">Login</h2>

      <div class="mb-4">
        <label for="email" class="block text-sm font-medium">Email</label>
        <input v-model="form.email" type="email" id="email" required class="w-full px-3 py-2 border rounded" />
      </div>

      <div class="mb-6">
        <label for="password" class="block text-sm font-medium">Senha</label>
        <input v-model="form.password" type="password" id="password" required class="w-full px-3 py-2 border rounded" />
      </div>

      <button type="submit" :disabled="AuthStore.isLoading"
        class="w-full  btn btn-secondary text-white py-2 px-4 rounded">
        {{ AuthStore.isLoading ? 'Entrando...' : 'Entrar' }}
      </button>
      <p class="mt-4 text-center text-sm text-black">
        Ainda não tem uma conta?
        <router-link to="/register" class="text-blue-400 underline hover:text-blue-300">Cadastre-se</router-link>
      </p>

    </form>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import { useAuthStore } from '@/stores/AuthStore'
import { useRouter } from 'vue-router'

const router = useRouter()
const AuthStore = useAuthStore()

const form = ref({
  email: '',
  password: '',
})

const handleLogin = async () => {
  const success = await AuthStore.login(form.value)
  if (success) {
    router.push('/tasks')
  } else {
    alert('Erro ao fazer login. Verifique suas credenciais.')
  }
}
</script>