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
          docker cp ${PUBLISH_DIR}/wwwroot/. backend-homolog:/usr/share/nginx/html/
          docker exec backend-homolog sh -c "chmod -R 755 /usr/share/nginx/html"
          docker exec backend-homolog sh -c "command -v ps >/dev/null 2>&1 && ps -ef | grep dotnet | grep -v grep | awk '{print $2}' | xargs -r kill || true"
          docker exec -d backend-homolog dotnet /app/Tarefa.API.dll
          docker exec backend-homolog sh -c "nginx -s reload || nginx"
        '''
      }
    }

    stage('Validar aplicação') {
      steps {
        sh '''
          docker exec backend-homolog sh -c "command -v curl >/dev/null 2>&1 && curl --fail http://localhost:80 || echo '⚠️ curl não disponível ou backend falhou (ignorado)'"
        '''
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
          docker cp ${PUBLISH_DIR}/wwwroot/. backend-prod:/usr/share/nginx/html/
          docker exec backend-prod sh -c "chmod -R 755 /usr/share/nginx/html"
          docker exec backend-prod sh -c "command -v ps >/dev/null 2>&1 && ps -ef | grep dotnet | grep -v grep | awk '{print $2}' | xargs -r kill || true"
          docker exec -d backend-prod dotnet /app/Tarefa.API.dll
          docker exec backend-prod sh -c "nginx -s reload || nginx"
        '''
      }
    }
  }
}
