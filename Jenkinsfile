pipeline {
  agent any

  environment {
    IMAGE_NAME = "tarefas-app"
  }

  stages {
    stage('Clonar código') {
      steps {
        checkout scm
      }
    }

    stage('Build da imagem Docker') {
      steps {
        sh 'docker build -t ${IMAGE_NAME}:latest .'
      }
    }

    stage('Parar containers antigos') {
      steps {
        sh 'docker compose -f docker-compose.yml down || true'
      }
    }

    stage('Subir nova versão') {
      steps {
        sh 'docker compose -f docker-compose.yml up -d --build'
      }
    }
  }
}
