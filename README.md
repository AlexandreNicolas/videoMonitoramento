# videoMonitoramento
Desenvolvimento back-end de uma API REST para o contexto de vídeo-monitoramento 

## Configurações iniciais
Instalar docker e docker-compose para executar aplicação.

### Compilar Projeto
`docker-compose build`
### Subir aplicação
`docker-compose up`
### Adicionar banco de dados
Criar banco de dados "videoMonit" via Adminer para salvar dados da aplicação. Acessar Adminer pelo endereço:
`http://localhost:8080`
Acessar com:
`Usuário: root`
`Senha: password`
Clicar no botão "Comando SQL" e adicionar banco de dados contido no arquivo "video_monit_db.txt".

## Exemplo de utilização
As requisições da API pode ser realizadas via Postman. A coleção do Postman utilizada está salva em "VideoMonitoramento.postman_collection.json" contendo todas as rotas mapeadas.

### Verificar Upload de Videos

Os videos são salvos na pasta /videos, pasta criada quando executada a aplicação. A visualização dos videos pode ser atravéz do bash do docker:

`docker exec -it server_web bash`

Listar videos dentro do bash:
* `root@af3b38fb4fe1:/app# ls videos/`
* `25d28839-756e-4963-9f5b-2c3e83457e26  ac02cd6f-af02-4ccd-ba01-c540dbbb9364  b608c7cd-ba86-496b-a7f4-32a133930214`

Os nomes dos videos são salvos com seu Id.

## API
* Criar um novo servidor
* ▪ /api/server

* Remover um servidor existente
* ▪ /api/servers/{serverId}

* Recuperar um servidor existente
* ▪ /api/servers/{serverId}

* Checar disponibilidade de um servidor
* ▪ /api/servers/available/{serverId}

* Listar todos os servidores
* ▪ /api/servers

* Adicionar um novo vídeo à um servidor
* ▪ /api/servers/{serverId}/videos

* Remover um vídeo existente
* ▪ /api/servers/{serverId}/videos/{videoId}

* Recuperar dados cadastrais de um vídeo
* ▪ /api/servers/{serverId}/videos/{videoId}

* Download do conteúdo binário de um vídeo
* ▪ /api/servers/{serverId}/videos/{videoId}/binary

* Listar todos os vídeos de um servidor
* ▪ /api/servers/{serverId}/videos

* Reciclar vídeos antigos
* ▪ /api/recycler/process/{days}
* ▪ /api/recycler/status
