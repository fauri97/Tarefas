pipeline {
  agent any

  environment {
    PUBLISH_DIR = "build/publish"
  }

  stages {
    stage('Clonar o repositório') {
      steps {
        checkout scm
      }
    }

    stage('Testar backend') {
      steps {
        sh 'dotnet restore backend/Tarefas.sln'
        sh 'dotnet test backend/Tarefas.sln --no-restore'
      }
    }

    stage('Build frontend e backend') {
      steps {
        sh 'rm -rf ${PUBLISH_DIR}'
        sh 'npm install --prefix frontend'
        sh 'npm run build --prefix frontend'
        sh 'dotnet publish backend/Tarefa.API/Tarefa.API.csproj -c Release -o ${PUBLISH_DIR}'
        sh 'cp -r frontend/dist ${PUBLISH_DIR}/wwwroot'
      }
    }

    stage('Deploy para homologação') {
      steps {
        sh '''
          docker cp ${PUBLISH_DIR}/. backend-homolog:/app
          docker exec backend-homolog pkill -f "dotnet" || true
          docker exec -d backend-homolog dotnet /app/Tarefa.API.dll
        '''
      }
    }

    stage('Validar aplicação') {
      steps {
        sh 'curl --fail http://localhost:8082 || (echo "❌ Backend falhou em homologação" && exit 1)'
      }
    }

    stage('Aprovar produção') {
      steps {
        input message: 'Deploy para produção?', ok: 'Sim, pode subir'
      }
    }

    stage('Deploy para produção') {
      steps {
        sh '''
          docker cp ${PUBLISH_DIR}/. backend-prod:/app
          docker exec backend-prod pkill -f "dotnet" || true
          docker exec -d backend-prod dotnet /app/Tarefa.API.dll
        '''
      }
    }
  }
}
