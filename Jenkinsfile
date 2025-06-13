pipeline {
  agent any

  environment {
    LANG = 'pt_BR.UTF-8'
    LC_ALL = 'pt_BR.UTF-8'
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT = 'false'
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
        sh 'docker build -t tarefas-api:latest .'
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
        input message: 'Deseja implantar em produção?', ok: 'Sim, implantar'
        dir('/home/univates/apps/producao') {
          sh 'docker compose down || true'
          sh 'docker compose up -d --build'
        }
      }
    }
  }
}
