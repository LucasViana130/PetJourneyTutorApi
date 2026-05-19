# PetJourney Tutor API

API RESTful desenvolvida em **ASP.NET Core Web API** para a área do tutor no sistema **PetJourney / Clyvo Vet**.

O objetivo desta API é permitir que o tutor gerencie seus dados, cadastre seus pets, acompanhe lembretes de cuidado e, opcionalmente, vincule seus pets a uma clínica veterinária.

Este projeto complementa o sistema principal desenvolvido em Java, ficando responsável apenas pelas funcionalidades voltadas ao tutor.

---

## Objetivo do projeto

A API foi criada para atender aos requisitos da disciplina **Advanced Business Development with .NET**, utilizando:

* CRUD completo das principais entidades do tutor;
* rotas parametrizadas;
* documentação com Swagger/OpenAPI;
* Entity Framework Core;
* DbContext;
* migrations;
* integração com Oracle Database;
* organização em camadas simples com Controllers, Services, Models e Data.

---

## Escopo da API

O sistema permite:

* cadastrar tutores;
* cadastrar pets;
* consultar espécies e raças pré-cadastradas;
* criar lembretes para pets;
* consultar clínicas disponíveis;
* vincular os pets de um tutor a uma clínica;
* remover vínculo com clínica;
* visualizar uma timeline mensal de lembretes do pet.

A clínica não possui CRUD completo neste projeto, pois o gerenciamento completo de clínicas pertence a outra parte do sistema.

---

## Tecnologias utilizadas

* C#
* ASP.NET Core Web API
* .NET 8
* Entity Framework Core
* Oracle Entity Framework Core
* Oracle Database
* Swagger / OpenAPI
* Migrations
* LINQ
* Postman

---

## Estrutura do projeto

```text
PetJourneyTutorApi
├── Controllers
│   ├── TutorsController.cs
│   ├── PetsController.cs
│   ├── RemindersController.cs
│   ├── ClinicsController.cs
│   ├── SpeciesController.cs
│   └── BreedsController.cs
│
├── Services
│   ├── TutorService.cs
│   ├── PetService.cs
│   ├── ReminderService.cs
│   └── ClinicService.cs
│
├── Data
│   └── AppDbContext.cs
│
├── Models
│   ├── Tutor.cs
│   ├── Pet.cs
│   ├── Reminder.cs
│   ├── Clinic.cs
│   ├── Species.cs
│   ├── Breed.cs
│   ├── TimelineItem.cs
│   └── TutorClinicRequest.cs
│
├── Migrations
├── Program.cs
├── appsettings.json
└── README.md
```

---

## Arquitetura utilizada

O projeto usa uma estrutura simples e adequada ao nível da disciplina:

```text
Controller -> Service -> AppDbContext -> Oracle Database
```

### Controllers

Responsáveis por receber as requisições HTTP, chamar os services e retornar respostas adequadas, como:

* `200 OK`
* `201 Created`
* `204 No Content`
* `400 Bad Request`
* `404 Not Found`

### Services

Responsáveis pelas regras simples de negócio, como:

* verificar se tutor existe;
* verificar se pet existe;
* validar espécie obrigatória;
* validar se raça pertence à espécie;
* validar se clínica existe;
* impedir remoção de pet com lembretes cadastrados;
* montar a timeline mensal do pet.

### AppDbContext

Responsável pela comunicação com o banco Oracle usando Entity Framework Core.

---

## DER do projeto

O projeto utiliza um recorte do DER geral do sistema PetJourney, focando apenas na parte do tutor.

> Adicione a imagem do DER na pasta `Docs` com o nome `der-petjourney.png`.

```md
![DER PetJourney](Docs/der-petjourney.png)
```

### Tabelas utilizadas nesta API

* `TBTUTOR`
* `TBPET`
* `TBLEMBRETE`
* `TBCLINICA`
* `TBESPECIE`
* `TBRACA`

### Relação entre as tabelas

```text
TBTUTOR 1 ---- N TBPET
TBPET 1 ---- N TBLEMBRETE
TBESPECIE 1 ---- N TBRACA
TBESPECIE 1 ---- N TBPET
TBRACA 1 ---- N TBPET
TBCLINICA 1 ---- N TBPET
```

### Observação sobre clínica

O tutor não precisa estar vinculado a uma clínica para usar o sistema.

A afiliação é opcional e acontece por meio dos pets:

* se `TBPET.IDCLINICA` estiver nulo, o pet não está afiliado a nenhuma clínica;
* se `TBPET.IDCLINICA` possuir valor, o pet está associado a uma clínica.

---

## Regras de negócio

### Tutor

* Pode ser cadastrado sem clínica.
* Pode possuir vários pets.
* Não pode ser removido se possuir pets cadastrados.

### Pet

* Deve obrigatoriamente possuir um tutor.
* Deve obrigatoriamente possuir uma espécie.
* Pode ou não possuir raça.
* Pode ou não estar vinculado a uma clínica.
* Não pode ser removido se possuir lembretes cadastrados.

### Espécie

* É pré-cadastrada.
* É obrigatória no cadastro do pet.
* Não possui CRUD completo nesta API.

### Raça

* É pré-cadastrada.
* É opcional no cadastro do pet.
* Deve pertencer à espécie informada.
* Não possui CRUD completo nesta API.

### Clínica

* É apenas consultada nesta API.
* Pode ser vinculada aos pets de um tutor.
* O CRUD completo de clínicas pertence a outro módulo do sistema.

### Lembrete

* Deve estar vinculado a um pet existente.
* Pode ser usado para vacina, consulta, banho ou outros cuidados.
* É usado para montar a timeline mensal do pet.

---

## Configuração do banco Oracle

No arquivo `appsettings.json`, configure a connection string:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
  }
}
```

> Não deixe usuário e senha reais no GitHub.

---

## Como executar o projeto

### 1. Restaurar pacotes

```bash
dotnet restore
```

### 2. Aplicar migrations

```bash
dotnet ef database update
```

### 3. Executar a API

```bash
dotnet run
```

### 4. Acessar Swagger

Abra no navegador:

```text
https://localhost:7050/swagger
```

A porta pode variar conforme a configuração local do Visual Studio.

---

## Migrations

O projeto utiliza migrations do Entity Framework Core para criar e atualizar as tabelas no Oracle.

### Criar nova migration

Use apenas quando alterar Models ou AppDbContext:

```bash
dotnet ef migrations add NomeDaMigration
```

### Aplicar migrations

```bash
dotnet ef database update
```

### Observação

Se forem alterados apenas Controllers, Services ou README, não é necessário criar nova migration.

---

## Endpoints da API

## Tutores

```text
GET    /api/tutores
GET    /api/tutores/{id}
GET    /api/tutores/{id}/pets
GET    /api/tutores/{id}/clinicas
POST   /api/tutores
PUT    /api/tutores/{id}
DELETE /api/tutores/{id}
PUT    /api/tutores/{tutorId}/clinica/{clinicaId}
DELETE /api/tutores/{tutorId}/clinica
```

### Exemplo de criação de tutor

```json
{
  "nmTutor": "Lucas Viana",
  "dsEmail": "lucas@email.com",
  "nrTelefone": "11999999999",
  "dsPlano": "FREE"
}
```

---

## Pets

```text
GET    /api/pets
GET    /api/pets/{id}
GET    /api/pets/{petId}/lembretes
GET    /api/pets/{petId}/timeline?mes=5&ano=2026
POST   /api/pets
PUT    /api/pets/{id}
DELETE /api/pets/{id}
```

### Exemplo de criação de pet

```json
{
  "nmPet": "Thor",
  "dtNascimento": "2021-03-10T00:00:00",
  "dsSexo": "M",
  "idTutor": 1,
  "idEspecie": 1,
  "idRaca": 1,
  "idClinica": null
}
```

---

## Lembretes

```text
GET    /api/lembretes
GET    /api/lembretes/{id}
POST   /api/lembretes
PUT    /api/lembretes/{id}
DELETE /api/lembretes/{id}
```

### Exemplo de criação de lembrete

```json
{
  "idPet": 1,
  "dsTipo": "VACINA",
  "dsDescricao": "Aplicar vacina V10",
  "dtLembrete": "2026-05-20T10:00:00",
  "dtNotificado": null,
  "dsStatus": "PENDENTE"
}
```

---

## Clínicas

```text
GET /api/clinicas
GET /api/clinicas/{id}
```

A clínica é apenas consultada e utilizada para afiliação dos pets do tutor.

---

## Espécies

```text
GET /api/especies
GET /api/especies/{id}
GET /api/especies/{id}/racas
```

As espécies são pré-cadastradas e obrigatórias no cadastro do pet.

---

## Raças

```text
GET /api/racas
GET /api/racas/{id}
```

As raças são pré-cadastradas e opcionais no cadastro do pet.

---

## Timeline do pet

A timeline não é uma tabela do banco.

Ela é um modelo auxiliar de resposta usado para organizar os lembretes do pet por mês e ano.

### Endpoint

```text
GET /api/pets/{petId}/timeline?mes=5&ano=2026
```

### Exemplo de retorno

```json
[
  {
    "tipo": "VACINA",
    "descricao": "Aplicar vacina V10",
    "data": "2026-05-20T00:00:00",
    "status": "PENDENTE"
  },
  {
    "tipo": "BANHO",
    "descricao": "Banho mensal",
    "data": "2026-05-25T00:00:00",
    "status": "PENDENTE"
  }
]
```

---

## Ordem recomendada de testes

1. Cadastrar espécies e raças no banco.
2. Cadastrar uma clínica.
3. Cadastrar um tutor.
4. Cadastrar um pet usando `idTutor` e `idEspecie`.
5. Cadastrar um lembrete usando `idPet`.
6. Testar a listagem de pets do tutor.
7. Testar a listagem de lembretes do pet.
8. Testar a timeline mensal.
9. Testar afiliação do tutor a uma clínica.
10. Testar PUT e DELETE.

---

## Script básico de dados

```sql
INSERT INTO TBESPECIE (NMESPECIE) VALUES ('Cachorro');
INSERT INTO TBESPECIE (NMESPECIE) VALUES ('Gato');

INSERT INTO TBRACA (NMRACA, IDESPECIE) VALUES ('Labrador', 1);
INSERT INTO TBRACA (NMRACA, IDESPECIE) VALUES ('Shih Tzu', 1);
INSERT INTO TBRACA (NMRACA, IDESPECIE) VALUES ('Persa', 2);

INSERT INTO TBCLINICA (NMCLINICA, DSENDERECO, NRTELEFONE, DSEMAIL)
VALUES ('Clinica Pet Care', 'Rua das Flores, 100 - Sao Paulo', '11999999999', 'contato@petcare.com');

INSERT INTO TBTUTOR (NMTUTOR, DSEMAIL, NRTELEFONE, DSPLANO, DTCADASTRO)
VALUES ('Lucas Viana', 'lucas@email.com', '11999999999', 'FREE', SYSDATE);

INSERT INTO TBPET (NMPET, DTNASCIMENTO, DSSEXO, IDTUTOR, IDESPECIE, IDRACA, IDCLINICA)
VALUES ('Thor', DATE '2021-03-10', 'M', 1, 1, 1, NULL);

INSERT INTO TBLEMBRETE (IDPET, DSTIPO, DSDESCRICAO, DTLEMBRETE, DSSTATUS)
VALUES (1, 'VACINA', 'Aplicar vacina V10', DATE '2026-05-20', 'PENDENTE');

COMMIT;
```

---

## Testes com Postman

O projeto pode ser testado pelo Swagger ou por uma collection do Postman.

A collection deve conter testes para:

* tutores;
* pets;
* lembretes;
* clínicas;
* espécies;
* raças;
* timeline;
* cenários de erro, como tutor inexistente, espécie inexistente e raça incompatível.

---

## Swagger / OpenAPI

A documentação Swagger está habilitada no projeto.

O Swagger permite testar todos os endpoints diretamente pelo navegador.

Além disso, os controllers possuem comentários XML para melhorar a descrição de cada rota na interface do Swagger.

---

## Checklist dos requisitos atendidos

* [x] API RESTful em ASP.NET Core.
* [x] CRUD completo de tutores.
* [x] CRUD completo de pets.
* [x] CRUD completo de lembretes.
* [x] Rotas parametrizadas por tutor.
* [x] Rotas parametrizadas por pet.
* [x] Consulta de clínicas.
* [x] Afiliação opcional com clínica.
* [x] Consulta de espécies.
* [x] Consulta de raças.
* [x] Timeline mensal do pet.
* [x] Swagger/OpenAPI.
* [x] Entity Framework Core.
* [x] DbContext.
* [x] Migrations.
* [x] Integração com Oracle.
* [x] Separação em Controllers e Services.
* [x] Retornos HTTP adequados.

---

## Observações finais

Este projeto foi desenvolvido com foco na parte do tutor dentro do ecossistema PetJourney / Clyvo Vet.

A API evita duplicar funcionalidades do módulo administrativo/clínico, mantendo o escopo adequado para a entrega da disciplina de .NET.

A clínica é usada apenas como consulta e vínculo opcional, enquanto a gestão completa da clínica pertence a outro módulo do sistema.
