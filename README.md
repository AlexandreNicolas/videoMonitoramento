# videoMonitoramento
Desenvolvimento back-end de uma API REST para o contexto de vídeo-monitoramento 

## Configurações iniciais
Instalar docker e docker-compose para executar aplicação.

### Compilar Projeto
* `$ docker-compose build`
### Subir aplicação
* `$ docker-compose up`
### Adicionar banco de dados
Criar banco de dados "videoMonit" via Adminer para salvar dados da aplicação. Acessar Adminer pelo endereço:
`http://localhost:8080`
Acessar com:
`Usuário: root`
`Senha: password`
Clicar no botão "Comando SQL" e adicionar banco de dados contido no arquivo "video_monit_db.txt".

## Exemplo de utilização
As requisições da API pode ser realizadas via Postman. A coleção do Postman utilizada está salva em "VideoMonitoramento.postman_collection.json" contendo todas as rotas mapeadas.

### Verificar Upload de Vídeos

Os vídeos são salvos na pasta /videos dentro de seu respectivo servidor /{serverId}. Essa pasta é criada quando adicionado um novo servidor no banco de dados. A visualização dos vídeos pode ser atravéz do bash do docker:

* `$ docker exec -it server_web bash`

Listar vídeos dentro do bash:
* `root@af3b38fb4fe1:/app# ls videos/{serverId}`
* `25d28839-756e-4963-9f5b-2c3e83457e26  ac02cd6f-af02-4ccd-ba01-c540dbbb9364  b608c7cd-ba86-496b-a7f4-32a133930214`

Os nomes dos vídeos são salvos com seu Id.

### Executar localmente

Alterar "Server" de "ConnectionStrings" do arquivo appsettings.json para "127.0.0.1":
* `Server=127.0.0.1`

Retirar o serviço "web" do conteiner em docker-compose.yml:
* `service: web`

Criar pasta videos dentro do repositório:
* `mkdir videos`

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
