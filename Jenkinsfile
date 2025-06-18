pipeline {
  agent any

  environment {
    LANG = 'pt_BR.UTF-8'
    LC_ALL = 'pt_BR.UTF-8'
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT = 'false'
    IMAGE_NAME = 'tarefas-api:latest'
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
        sh "docker build -t ${IMAGE_NAME} ."
      }
    }

    stage('Deploy Homologação') {
      steps {
        script {
          deployApp(
            envPath: '/home/univates/apps/homolog',
            composeFile: 'docker-compose.homolog.yml'
          )
        }
      }
    }

    stage('Deploy Produção') {
      when {
        branch 'main'
      }
      steps {
        input message: 'Deseja implantar em produção?', ok: 'Sim, implantar'
        script {
          deployApp(
            envPath: '/home/univates/apps/producao',
            composeFile: 'docker-compose.producao.yml'
          )
        }
      }
    }
  }
}

def deployApp(Map config) {
  sh "mkdir -p ${config.envPath}"

  sh """
    cp ${config.composeFile} ${config.envPath}/
    cp -r ./backend ${config.envPath}/
    cp -r ./publish ${config.envPath}/ || true
  """

  dir(config.envPath) {
    sh "docker-compose -f ${config.composeFile} down || true"
    sh "docker-compose -f ${config.composeFile} up -d --build"
  }
}
