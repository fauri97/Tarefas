<template>
  <div>
    <h1 class="text-3xl text-center"><strong>Tasks</strong></h1>
    <div class="overflow-x-auto">
      <table class="table table-zebra">
        <!-- head -->
        <thead>
          <tr>
            <th>Id</th>
            <th>Descrição</th>
            <th>Criado em</th>
            <th>Data esperada</th>
            <th>Data de conclusão</th>
            <th>Situação</th>
          </tr>
        </thead>
        <tbody>
          <!-- row 1 -->
          <tr v-for="task in store.tasks" :key="task.id">
            <td>{{ task.id }}</td>
            <td>{{ task.description }}</td>
            <td>{{ task.createdAt }}</td>
            <td>{{ task.expectedDate }}</td>
            <td>{{ task.closedAt }}</td>
            <td>{{ task.status ? 'Fechado' : 'Aberto' }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script setup>
import { onMounted } from 'vue'
import { useTaskStore } from './stores/taskStore'

const store = useTaskStore()
onMounted(async () => {
  await store.fetchTasks()

  console.log('Tasks fetched:', store.tasks)
})
</script>
