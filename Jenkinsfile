pipeline {
  agent any

  environment {
    IMAGE_NAME = "tarefas-app"
  }

  stages {
    stage('Clone') {
      steps {
        checkout scm
      }
    }

    stage('Build Docker image') {
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
