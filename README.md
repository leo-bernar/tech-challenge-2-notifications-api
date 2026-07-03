# FIAP Cloud Games - NotificationsAPI

Microsservico de notificacoes da Fase 2 do Tech Challenge FIAP.

O servico consome eventos do RabbitMQ e simula o envio de e-mails por logs no
console:

- `UserCreatedEvent`: registra e-mail de boas-vindas;
- `PaymentProcessedEvent` aprovado: registra confirmacao de compra;
- `PaymentProcessedEvent` rejeitado: nao registra confirmacao de compra.

Nao existe envio real de e-mail nem persistencia de dados.

## Tecnologias

- .NET 8
- ASP.NET Core Minimal APIs
- MassTransit 8.5.10
- RabbitMQ
- Serilog
- xUnit
- Docker
- Kubernetes

## Estrutura

```text
.
├── src/FCG.Notifications.Api/
├── tests/FCG.Notifications.Tests/
├── k8s/
├── Dockerfile
└── TechChallenge.Notifications.sln
```

## Variaveis de ambiente

| Variavel | Sensivel | Descricao |
|---|---|---|
| `RabbitMq__Host` | Nao | Host do RabbitMQ. |
| `RabbitMq__Port` | Nao | Porta AMQP, normalmente `5672`. |
| `RabbitMq__VirtualHost` | Nao | Virtual host do broker. |
| `RabbitMq__Username` | Sim | Usuario do RabbitMQ. |
| `RabbitMq__Password` | Sim | Senha do RabbitMQ. |
| `RabbitMq__UserCreatedQueue` | Nao | Fila que recebe `UserCreatedEvent`. |
| `RabbitMq__PaymentProcessedQueue` | Nao | Fila que recebe `PaymentProcessedEvent`. |

Credenciais nao devem ser adicionadas ao `appsettings.json`.

## Executar localmente

Defina as credenciais do broker:

```bash
export RabbitMq__Username=guest
export RabbitMq__Password=guest
```

Execute:

```bash
dotnet restore
dotnet run --project src/FCG.Notifications.Api/FCG.Notifications.Api.csproj
```

Endpoints operacionais:

- `GET /`
- `GET /health`
- `GET /swagger` em Development

Nao existem endpoints HTTP de negocio. Todas as requisicoes HTTP disponiveis sao
anonimas; o processamento funcional ocorre por eventos.

## Testes

```bash
dotnet run --project tests/FCG.Notifications.Tests/FCG.Notifications.Tests.csproj
```

O projeto usa o runner nativo do xUnit v3. Os testes unitarios exercitam
diretamente o comportamento dos consumidores e validam:

- boas-vindas apos `UserCreatedEvent`;
- confirmacao para pagamento aprovado;
- ausencia de confirmacao para pagamento rejeitado.

## Docker

Na raiz do repositorio:

```bash
docker build -t tech-challenge-2-notifications-api:local .
```

O Dockerfile usa build multi-stage e executa a imagem final com ASP.NET Core
Runtime 8.

## Kubernetes

Os manifests ficam em `/k8s`:

- `deployment.yaml`;
- `service.yaml`;
- `configmap.yaml`;
- `secret.example.yaml`.

Crie o arquivo local de Secret:

```bash
cp k8s/secret.example.yaml k8s/secret.local.yaml
```

Substitua os placeholders e aplique:

```bash
kubectl apply -f k8s/configmap.yaml
kubectl apply -f k8s/secret.local.yaml
kubectl apply -f k8s/deployment.yaml
kubectl apply -f k8s/service.yaml
```

O arquivo `secret.local.yaml` e ignorado pelo Git.
