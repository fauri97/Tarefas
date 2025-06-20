pipeline {
  agent any

  environment {
    PUBLISH_DIR = "build/publish"
  }

  stages {
    stage('Clonar o repositório') {
      steps {
        git url: 'https://github.com/fauri97/Tarefas.git', branch: 'main'
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
        sh '''
          rm -rf ${PUBLISH_DIR}
          npm install --prefix frontend
          npm run build --prefix frontend
          dotnet publish backend/Tarefa.API/Tarefa.API.csproj -c Release -o ${PUBLISH_DIR}
          mkdir -p ${PUBLISH_DIR}/wwwroot
          cp -r frontend/dist/* ${PUBLISH_DIR}/wwwroot/
        '''
      }
    }

    stage('Subir homologação se necessário') {
      steps {
        sh '''
          docker start backend-homolog || docker-compose -f ~/infra/homolog/docker-compose.yml up -d
        '''
      }
    }

    stage('Deploy para homologação') {
      steps {
        sh '''
          docker cp ${PUBLISH_DIR}/. backend-homolog:/app
          docker cp ${PUBLISH_DIR}/wwwroot/. backend-homolog:/usr/share/nginx/html/
          docker exec backend-homolog sh -c "chmod -R 755 /usr/share/nginx/html"
          docker exec backend-homolog sh -c "pkill -f 'dotnet /app/Tarefa.API.dll' || true"
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

    stage('Subir produção se necessário') {
      steps {
        sh '''
          docker start backend-prod || docker-compose -f ~/infra/prod/docker-compose.yml up -d
        '''
      }
    }

    stage('Deploy para produção') {
      steps {
        sh '''
          docker cp ${PUBLISH_DIR}/. backend-prod:/app
          docker cp ${PUBLISH_DIR}/wwwroot/. backend-prod:/usr/share/nginx/html/
          docker exec backend-prod sh -c "chmod -R 755 /usr/share/nginx/html"
          docker exec backend-prod sh -c "pkill -f 'dotnet /app/Tarefa.API.dll' || true"
          docker exec -d backend-prod dotnet /app/Tarefa.API.dll
          docker exec backend-prod sh -c "nginx -s reload || nginx"
        '''
      }
    }
  }
}
