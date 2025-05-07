<template>
    <div class="fixed bottom-6 right-6 z-50">
      <div class="dropdown dropdown-top dropdown-end">
        <label tabindex="0" class="btn btn-primary btn-circle text-2xl">
          ⋮
        </label>
        <ul tabindex="0" class="dropdown-content menu p-2 shadow bg-base-200 rounded-box w-52">
          <li>
            <a @click="$emit('download-pdf')">
              📄 Download PDF
            </a>
          </li>
          <li>
            <a @click="$emit('logout')">
              🔒 Deslogar
            </a>
          </li>
        </ul>
      </div>
    </div>
  </template>
  
  <script setup>
  defineEmits(['download-pdf', 'logout']);
  </script>
  