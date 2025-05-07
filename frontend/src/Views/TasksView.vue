<template>
  <div>
    <h1 class="text-3xl text-center font-bold mb-4">Tasks</h1>

    <div class="flex flex-col md:flex-row gap-4 mb-4 justify-around">
      <div class="flex flex-col md:flex-row gap-4 mb-4 w-[300px] items-center">
        <p class="">Filtro:</p>
        <input type="date" v-model="filters.date" class="input input-bordered" placeholder="Filtrar por data" />

        <select v-model="filters.status" class="select select-bordered w-32">
          <option value="">Todos</option>
          <option value="open">Abertos</option>
          <option value="closed">Fechados</option>
        </select>

        <button class="btn btn-secondary" @click="clearFilter">Limpar</button>
      </div>


      <!-- Botão de abrir modal -->
      <div class="mb-4 text-right">
        <button class="btn btn-primary" @click="openModal">+ Nova Tarefa</button>
      </div>
    </div>

    <!-- Modal -->
    <dialog id="task_modal" class="modal" ref="modalRef">
      <div class="modal-box">
        <h3 class="font-bold text-lg mb-4">
          {{ form.id ? 'Editar Tarefa' : 'Criar Tarefa' }}
        </h3>

        <form @submit.prevent="handleSubmit" class="flex flex-col gap-4">
          <input v-model="form.description" type="text" placeholder="Descrição" class="input input-bordered" required />
          <input v-model="form.expectedDate" type="datetime-local" class="input input-bordered" required />

          <div class="modal-action">
            <button type="button" class="btn" @click="closeModal">Cancelar</button>
            <button type="submit" class="btn btn-primary">
              {{ form.id ? 'Atualizar' : 'Criar' }}
            </button>
          </div>
        </form>
      </div>
    </dialog>

    <!-- Tabela -->
    <div class="overflow-x-auto">
      <table class="table table-zebra w-full">
        <thead>
          <tr>
            <th>Id</th>
            <th>Descrição</th>
            <th>Criado em</th>
            <th>Data esperada</th>
            <th>Concluído em</th>
            <th>Situação</th>
            <th>Ações</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="task in filteredTasks" :key="task.id">
            <td>{{ task.id }}</td>
            <td>{{ task.description }}</td>
            <td>{{ formatDate(task.createdAt) }}</td>
            <td>{{ formatDate(task.expectedDate) }}</td>
            <td>{{ task.closedAt ? formatDate(task.closedAt) : '---' }}</td>
            <td>{{ task.status ? 'Fechado' : 'Aberto' }}</td>
            <td class="flex gap-2">
              <button class="btn btn-sm btn-success" @click="store.toggleTaskStatus(task.id)" v-if="!task.status">
                Fechar
              </button>
              <button class="btn btn-sm btn-info" @click="editTask(task)">Editar</button>
              <button class="btn btn-sm btn-error" @click="deleteTask(task.id)">Excluir</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
  <ActionFloatingButton @download-pdf="handleDownloadPdf" @logout="handleLogout" />
</template>

<script setup>
import { ref, onMounted, computed } from 'vue'
import { useTaskStore } from '@/stores/TaskStore'
import { toast } from 'vue3-toastify';
import ActionFloatingButton from '@/Components/ActionFloatingButton.vue'

const store = useTaskStore()
const modalRef = ref(null)

const form = ref({
  id: null,
  description: '',
  expectedDate: ''
})

onMounted(async () => {
  await store.fetchTasks()
})

const filters = ref({
  date: '',
  status: ''
})

const filteredTaskIds = computed(() => filteredTasks.value.map(t => t.id))
const filteredTasks = computed(() => {
  return store.tasks.filter(task => {
    const matchesDate = filters.value.date
      ? task.expectedDate.slice(0, 10) === filters.value.date
      : true

    const matchesStatus =
      filters.value.status === 'open'
        ? task.status === false
        : filters.value.status === 'closed'
          ? task.status === true
          : true

    return matchesDate && matchesStatus
  })
})

const openModal = () => {
  modalRef.value?.showModal()
}

const closeModal = () => {
  modalRef.value?.close()
  resetForm()
}

const handleSubmit = async () => {
  if (form.value.id) {
    const taskData = {
        ...form.value,
        expectedDate: new Date(form.value.expectedDate).toISOString()
      }
    await store.updateTask(taskData)
    toast.success('Tarefa atualizada com sucesso!')
    form.value = {
      id: null,
      description: '',
      expectedDate: ''
    }
    closeModal()
  } else {
    try {
      const taskData = {
        ...form.value,
        expectedDate: new Date(form.value.expectedDate).toISOString()
      }
      await store.createTask(taskData);
      toast.success('Tarefa criada com sucesso!');
      closeModal()
    } catch (err) {
      toast.error(err);
    }
  }
};

const handleDownloadPdf = async () => {
  await store.downloadTaskPDF(filteredTaskIds.value)
};

const handleLogout = () => {
  localStorage.removeItem('token');
  window.location.href = '/';
};

function clearFilter() {
  filters.value = {
    date: '',
    status: ''
  }
}

const editTask = (task) => {
  form.value = {
    id: task.id,
    description: task.description,
    expectedDate: task.expectedDate.slice(0, 16)
  }
  openModal()
}

const deleteTask = async (id) => {
  if (confirm('Deseja realmente excluir?')) {
    await store.deleteTask(id)
    await store.fetchTasks()
  }
}

const resetForm = () => {
  form.value = {
    id: null,
    description: '',
    expectedDate: ''
  }
}

const formatDate = (iso) => {
  return new Date(iso).toLocaleString()
}
</script>
