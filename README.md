# Sistema de Notas Fiscais

Este repositório contém a implementação de um sistema de emissão de notas fiscais, desenvolvido como parte de um desafio técnico. A solução foi projetada como um sistema distribuído, separando as responsabilidades de backend e frontend.

## 🚀 Arquitetura da Solução

A solução segue uma **arquitetura de microsserviços** desacoplada, composta por três projetos principais:

### 1. Backend: Microsserviços em C# (.NET 8)

O backend é dividido em duas APIs RESTful independentes, ambas construídas com **ASP.NET Core 8** e seguindo os princípios do **Domain-Driven Design (DDD)** para uma lógica de negócio clara e encapsulada.

* **`Servico.Estoque`**: Um microsserviço focado exclusivamente no gerenciamento de produtos, saldos e responsáveis.
* **`Servico.Faturamento`**: Um microsserviço focado na criação, gerenciamento e processamento (impressão) de notas fiscais.

Ambos os serviços utilizam **Entity Framework Core (EF Core)** e **LINQ** para comunicação com bancos de dados **SQL Server** dedicados e isolados. A comunicação entre os serviços (ex: Faturamento debitando o Estoque) é feita via chamadas HTTP síncronas (REST).

### 2. Frontend: Aplicação em Angular

O frontend é uma **Single Page Application (SPA)** construída com **Angular** (utilizando a arquitetura clássica de `NgModules` para demonstrar o domínio de conceitos como ciclos de vida e injeção de dependência).

* **Comunicação Reativa**: A interação com os dois microsserviços de backend é gerenciada de forma totalmente assíncrona usando **RxJS (Observables)**.
* **Interface do Usuário**: A UI foi desenvolvida com a biblioteca **Angular Material**, garantindo componentes visuais robustos e um design profissional adequado para um ERP, incluindo tabelas, paginação, ordenação e formulários reativos.
* **Gerenciamento de Estado**: O estado da UI (como listas de produtos e notas) é gerenciado dentro dos componentes, utilizando `Subscriptions` do RxJS que são devidamente tratadas com `ngOnDestroy` para evitar vazamentos de memória.

## 🛠 Como Executar

Para executar o projeto, são necessários 3 terminais rodando simultaneamente:

1.  **Backend 1 (`Servico.Estoque`)**:
    * `cd backend/Servico.Estoque`
    * `dotnet ef database update` (Apenas na primeira vez)
    * `dotnet run`
2.  **Backend 2 (`Servico.Faturamento`)**:
    * `cd backend/Servico.Faturamento`
    * `dotnet ef database update` (Apenas na primeira vez)
    * `dotnet run`
3.  **Frontend (`Angular`)**:
    * `cd frontend`
    * `npm install` (Apenas na primeira vez)
    * `ng serve`

A aplicação estará disponível em `http://localhost:4200`.
