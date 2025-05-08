<template>
    <div class="flex justify-center items-center h-screen bg">
        <form @submit.prevent="handleRegister" class="bg-fuchsia-950 p-6 rounded shadow-md w-full max-w-sm">
            <h2 class="text-2xl font-bold mb-4 text-center text-white">Cadastro</h2>

            <div class="mb-4">
                <label for="name" class="block text-sm font-medium text-white">Nome</label>
                <input v-model="form.name" type="text" id="name" required class="w-full px-3 py-2 border rounded" />
            </div>

            <div class="mb-4">
                <label for="email" class="block text-sm font-medium text-white">Email</label>
                <input v-model="form.email" type="email" id="email" required class="w-full px-3 py-2 border rounded" />
            </div>

            <div class="mb-6">
                <label for="password" class="block text-sm font-medium text-white">Senha</label>
                <input v-model="form.password" type="password" id="password" required
                    class="w-full px-3 py-2 border rounded" />
            </div>

            <button type="submit" :disabled="isLoading" class="w-full btn btn-secondary text-white py-2 px-4 rounded">
                {{ isLoading ? 'Cadastrando...' : 'Cadastrar' }}
            </button>
        </form>
    </div>
</template>

<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '@/stores/AuthStore'
import { toast } from 'vue3-toastify';

const AuthStore = useAuthStore()
const router = useRouter()
const isLoading = ref(false)

const form = ref({
    name: '',
    email: '',
    password: '',
})

const handleRegister = async () => {
    isLoading.value = true
    try {
        await AuthStore.createUser(form.value)
        alert('Usuário criado com sucesso!')
        router.push('/')
    } catch (error) {
        console.log(error)
        alert('Erro ao cadastrar usuário. Email já utilizado.')
    } finally {
        isLoading.value = false
    }
}

</script>