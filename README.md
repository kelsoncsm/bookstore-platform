# BookStore Platform

Plataforma de livraria online baseada em microservicos com Angular, .NET 8, RabbitMQ, PostgreSQL e Docker.

## Estrutura

- `src/backend`: microservicos, building blocks e gateway.
- `src/frontend`: aplicacao Angular.
- `deploy/docker`: compose e artefatos de infraestrutura local.
- `docs/architecture`: documentacao arquitetural da solucao.

## Proximos passos

1. Gerar a solucao `.NET 8` e os projetos base do backend.
2. Implementar `BuildingBlocks` compartilhados.
3. Implementar os microservicos na ordem: `Identity`, `Catalog`, `Inventory`, `Order`, `Cart`, `Notification`.
4. Gerar o `API Gateway`.
5. Gerar o frontend em Angular.
