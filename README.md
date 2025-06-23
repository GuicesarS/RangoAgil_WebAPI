# 🍽️ RangoÁgil API

API RESTful desenvolvida com **.NET 8**, utilizando o conceito de **Minimal APIs**, voltada para a gestão de pratos e ingredientes, com autenticação e autorização completas via JWT e Identity.

Este é um projeto pessoal criado com o objetivo de consolidar conhecimentos em desenvolvimento back-end utilizando uma **arquitetura enxuta**, seguindo boas práticas de organização, segurança e escalabilidade.

---

## 🛠️ Tecnologias Utilizadas

- .NET 8
- ASP.NET Core Minimal API
- Entity Framework Core
- AutoMapper
- ASP.NET Identity + JWT
- Swagger / Swashbuckle
- Endpoint Filters
- DTOs
- Git e GitHub

---

## 📌 Sobre a Arquitetura

A aplicação foi construída com **Minimal API**, uma abordagem mais direta e leve do ASP.NET Core, ideal para microserviços ou aplicações pequenas e específicas. 

Este projeto segue o princípio de separação de responsabilidades, organização modular dos handlers e aplicação de boas práticas RESTful, mesmo utilizando uma abordagem minimalista. A autenticação é baseada em JWT integrada ao ASP.NET Identity, garantindo segurança robusta para os endpoints protegidos.

---

## 🔒 Funcionalidades

- Cadastro e autenticação de usuários
- Login e geração de tokens JWT
- Confirmação de e-mail e recuperação de senha
- Gerenciamento de pratos (“rangos”)
- Adição e listagem de ingredientes por prato
- Documentação interativa com Swagger

---

## 📁 Observações

Este projeto representa um **serviço isolado**, com estrutura enxuta e escalável, podendo ser tratado como um **microserviço** em um sistema maior. Ideal para cenários de estudo, prova de conceito ou composição em arquitetura orientada a serviços.

---
