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
  sh """
    echo "➡️ Criando diretório de destino em: ${config.envPath}"
    mkdir -p ${config.envPath}

    echo "📁 Copiando arquivos para ${config.envPath}..."

    if [ -f ${config.composeFile} ]; then
      cp ${config.composeFile} ${config.envPath}/
    else
      echo "❌ Arquivo ${config.composeFile} não encontrado no repositório!"
      exit 1
    fi

    [ -d ./backend ] && cp -r ./backend ${config.envPath}/ || echo "⚠️ Pasta ./backend não encontrada"
    [ -d ./nginx ] && cp -r ./nginx ${config.envPath}/ || echo "⚠️ Pasta ./nginx não encontrada"
    [ -d ./frontend/dist ] && cp -r ./frontend ${config.envPath}/ || echo "⚠️ Pasta ./frontend/dist não encontrada (frontend não será servido)"
    [ -d ./publish ] && cp -r ./publish ${config.envPath}/ || echo "ℹ️ Pasta ./publish não existe, ignorando"

  """

  dir(config.envPath) {
    sh """
      echo "🧹 Finalizando containers anteriores (se houver)..."
      docker-compose -f ${config.composeFile} down || true

      echo "🚀 Subindo nova stack..."
      docker-compose -f ${config.composeFile} up -d --build
    """
  }
}
