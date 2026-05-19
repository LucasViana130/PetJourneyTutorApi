# Roteiro de testes pelo Swagger

## 1. Conferir dados existentes

Teste:

```text
GET /api/clinicas
GET /api/tutores
GET /api/pets
GET /api/lembretes
```

## 2. Criar tutor sem clínica

```text
POST /api/tutores
```

```json
{
  "nmTutor": "Joao Teste",
  "dsEmail": "joao.teste@email.com",
  "nrTelefone": "11955554444",
  "dsPlano": "FREE"
}
```

## 3. Criar pet sem clínica

Troque `idTutor`, `idEspecie` e `idRaca` por IDs existentes no seu banco.

```text
POST /api/pets
```

```json
{
  "nmPet": "Bolt",
  "dtNascimento": "2022-01-10T00:00:00",
  "dsSexo": "M",
  "idTutor": 1,
  "idEspecie": 1,
  "idRaca": 1,
  "idClinica": null
}
```

## 4. Criar pet já afiliado a uma clínica

```json
{
  "nmPet": "Nina",
  "dtNascimento": "2021-05-15T00:00:00",
  "dsSexo": "F",
  "idTutor": 1,
  "idEspecie": 1,
  "idRaca": 1,
  "idClinica": 1
}
```

## 5. Afiliar tutor a uma clínica

Essa rota não grava em TBTUTOR. Ela atualiza os pets do tutor em TBPET.IDCLINICA.

```text
PUT /api/tutores/1/clinica/1
```

## 6. Ver clínicas do tutor

```text
GET /api/tutores/1/clinicas
```

## 7. Remover afiliação do tutor

```text
DELETE /api/tutores/1/clinica
```

## 8. Criar lembrete

```text
POST /api/lembretes
```

```json
{
  "idPet": 1,
  "dsTipo": "Vacina",
  "dsDescricao": "Vacina V10",
  "dtLembrete": "2026-05-20T10:00:00",
  "dsStatus": "PENDENTE"
}
```

## 9. Testar rotas principais

```text
GET /api/tutores/1
GET /api/tutores/1/pets
GET /api/pets/1/lembretes
GET /api/pets/1/timeline?mes=5&ano=2026
```
