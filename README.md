# PetJourney Tutor API

API RESTful em ASP.NET Core para a parte do tutor no projeto PetJourney / Clyvo Vet.

O objetivo é permitir que o tutor gerencie seus dados, cadastre seus pets, acompanhe lembretes e se vincule a uma clínica. A clínica não possui CRUD completo neste projeto, pois o gerenciamento de clínicas pertence a outra parte do sistema.

## Tecnologias

- ASP.NET Core Web API
- C#
- Entity Framework Core
- Oracle Entity Framework Core
- Oracle Database
- Migrations
- Swagger / OpenAPI

## Estrutura

```text
PetJourneyTutorApi
├── Controllers
│   ├── TutorsController.cs
│   ├── PetsController.cs
│   ├── RemindersController.cs
│   └── ClinicsController.cs
├── Services
│   ├── TutorService.cs
│   ├── PetService.cs
│   ├── ReminderService.cs
│   └── ClinicService.cs
├── Data
│   └── AppDbContext.cs
├── Models
│   ├── Tutor.cs
│   ├── Pet.cs
│   ├── Reminder.cs
│   ├── Clinic.cs
│   └── TimelineItem.cs
├── Migrations
├── Docs
├── Program.cs
└── appsettings.json
```

## Camadas

O projeto usa uma estrutura simples:

```text
Controller -> Service -> AppDbContext -> Oracle
```

Os controllers cuidam das rotas e retornos HTTP.  
As services cuidam das regras simples de negócio.  
O AppDbContext faz a comunicação com o Oracle via EF Core.

## Configuração do banco

No arquivo `appsettings.json`, configure sua conexão Oracle:

```json
{
  "ConnectionStrings": {
    "OracleConnection": "User Id=SEU_USUARIO;Password=SUA_SENHA;Data Source=oracle.fiap.com.br:1521/ORCL"
  }
}
```

Troque `SEU_USUARIO` e `SUA_SENHA` pelas suas credenciais.

## Como executar

```bash
dotnet restore
dotnet run
```

Depois acesse:

```text
https://localhost:7070/swagger
```

ou a URL exibida no terminal.

## Migrations

Se você já criou a migration e apenas mudou controllers/services, não precisa criar migration nova.

Use migration nova apenas quando mudar Models ou AppDbContext.

### Primeira execução

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Se já existe migration, mas as tabelas ainda não foram criadas

```bash
dotnet ef database update
```

### Se as tabelas já existem no banco

Não rode `Update-Database` novamente sem conferir, pois pode dar erro de tabela já existente.

## Rotas

### Tutores

```text
GET    /api/tutores
GET    /api/tutores/{id}
GET    /api/tutores/{id}/pets
POST   /api/tutores
PUT    /api/tutores/{id}
DELETE /api/tutores/{id}
PUT    /api/tutores/{tutorId}/clinica/{clinicaId}
```

### Pets

```text
GET    /api/pets
GET    /api/pets/{id}
GET    /api/pets/{petId}/lembretes
GET    /api/pets/{petId}/timeline?mes=5&ano=2026
POST   /api/pets
PUT    /api/pets/{id}
DELETE /api/pets/{id}
```

### Lembretes

```text
GET    /api/lembretes
GET    /api/lembretes/{id}
POST   /api/lembretes
PUT    /api/lembretes/{id}
DELETE /api/lembretes/{id}
```

### Clínicas

```text
GET /api/clinicas
GET /api/clinicas/{id}
```

## Ordem de teste no Swagger

1. Criar tutor.
2. Criar pet usando o `idTutor` retornado.
3. Criar lembrete usando o `idPet` retornado.
4. Testar `GET /api/tutores/{id}/pets`.
5. Testar `GET /api/pets/{petId}/lembretes`.
6. Testar `GET /api/pets/{petId}/timeline?mes=5&ano=2026`.
7. Testar PUT e DELETE.

## Observação sobre clínicas

Neste projeto, clínica é apenas consultada e usada para vínculo com tutor. O CRUD completo de clínica não foi incluído para evitar duplicidade com o restante do sistema.


## Regra de afiliação com clínica

O tutor não precisa estar vinculado a uma clínica para usar o sistema.

- Se `TBPET.IDCLINICA` estiver nulo, o pet/tutor usa o sistema sem afiliação.
- Se `TBPET.IDCLINICA` tiver valor, o pet está associado a uma clínica.
- A rota `PUT /api/tutores/{tutorId}/clinica/{clinicaId}` afilia os pets do tutor à clínica informada.
- A rota `DELETE /api/tutores/{tutorId}/clinica` remove a afiliação dos pets do tutor.
- A rota `GET /api/tutores/{id}/clinicas` lista as clínicas vinculadas aos pets daquele tutor.

Isso foi feito para respeitar o banco atual, onde `TBTUTOR` não possui a coluna `IDCLINICA`.

## Importante sobre migrations

Como o banco já existe e este ajuste respeita as tabelas atuais, não rode `Add-Migration` nem `Update-Database` para este ajuste.
