pipeline {
  agent any

  environment {
    IMAGE_NAME = "tarefas-app"
  }

  stages {
    stage('Clone do repositório') {
      steps {
        checkout scm
      }
    }

    stage('Restaurar dependências') {
      steps {
        sh 'dotnet restore ./backend/Tarefas.sln'
      }
    }

    stage('Rodar testes unitários') {
      steps {
        sh 'dotnet test ./backend/Tarefas.sln --no-restore --verbosity normal'
      }
    }

    stage('Build da imagem Docker') {
      steps {
        sh 'docker build -t ${IMAGE_NAME}:latest .'
      }
    }

    stage('Deploy Homologação') {
      steps {
        dir('/home/univates/apps/homolog') {
          sh 'docker compose down || true'
          sh 'docker compose up -d --build'
        }
      }
    }

    stage('Deploy Produção') {
      when {
        branch 'main'
      }
      steps {
        dir('/home/univates/apps/producao') {
          sh 'docker compose down || true'
          sh 'docker compose up -d --build'
        }
      }
    }
  }
}
