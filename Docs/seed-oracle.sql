-- Dados de teste para o banco Oracle do PetJourney
-- Este script não informa valores nas colunas IDENTITY.
-- Ele usa os IDs gerados pelo Oracle com RETURNING INTO.

DECLARE
    v_clinica1 NUMBER;
    v_clinica2 NUMBER;
    v_tutor1   NUMBER;
    v_tutor2   NUMBER;
    v_especie1 NUMBER;
    v_especie2 NUMBER;
    v_raca1    NUMBER;
    v_raca2    NUMBER;
    v_pet1     NUMBER;
    v_pet2     NUMBER;
BEGIN
    INSERT INTO TBCLINICA
    (NMCLINICA, DSENDERECO, NRTELEFONE, DSEMAIL, DSPLANO, DSSTATUS, DTCADASTRO)
    VALUES
    ('Clinica Pet Care', 'Rua das Flores, 100 - Sao Paulo', '11999999999', 'contato@petcare.com', 'BASICO', 'ATIVA', SYSDATE)
    RETURNING IDCLINICA INTO v_clinica1;

    INSERT INTO TBCLINICA
    (NMCLINICA, DSENDERECO, NRTELEFONE, DSEMAIL, DSPLANO, DSSTATUS, DTCADASTRO)
    VALUES
    ('Hospital Veterinario Vida Animal', 'Av Paulista, 500', '11888888888', 'vidaanimal@email.com', 'AVANCADO', 'ATIVA', SYSDATE)
    RETURNING IDCLINICA INTO v_clinica2;

    INSERT INTO TBTUTOR
    (NMTUTOR, DSEMAIL, NRTELEFONE, DSPLANO, DTCADASTRO)
    VALUES
    ('Lucas Viana', 'lucas@email.com', '11977777777', 'FREE', SYSDATE)
    RETURNING IDTUTOR INTO v_tutor1;

    INSERT INTO TBTUTOR
    (NMTUTOR, DSEMAIL, NRTELEFONE, DSPLANO, DTCADASTRO)
    VALUES
    ('Maria Oliveira', 'maria@email.com', '11966666666', 'FREE', SYSDATE)
    RETURNING IDTUTOR INTO v_tutor2;

    INSERT INTO TBESPECIE (NMESPECIE)
    VALUES ('Cachorro')
    RETURNING IDESPECIE INTO v_especie1;

    INSERT INTO TBESPECIE (NMESPECIE)
    VALUES ('Gato')
    RETURNING IDESPECIE INTO v_especie2;

    INSERT INTO TBRACA (NMRACA, IDESPECIE)
    VALUES ('Labrador', v_especie1)
    RETURNING IDRACA INTO v_raca1;

    INSERT INTO TBRACA (NMRACA, IDESPECIE)
    VALUES ('Siamese', v_especie2)
    RETURNING IDRACA INTO v_raca2;

    -- Pet afiliado a uma clínica
    INSERT INTO TBPET
    (NMPET, DTNASCIMENTO, DSSEXO, IDTUTOR, IDESPECIE, IDRACA, IDCLINICA)
    VALUES
    ('Thor', DATE '2021-03-10', 'M', v_tutor1, v_especie1, v_raca1, v_clinica1)
    RETURNING IDPET INTO v_pet1;

    -- Pet sem clínica: tutor usa o sistema sem afiliação
    INSERT INTO TBPET
    (NMPET, DTNASCIMENTO, DSSEXO, IDTUTOR, IDESPECIE, IDRACA, IDCLINICA)
    VALUES
    ('Luna', DATE '2020-07-15', 'F', v_tutor1, v_especie1, v_raca1, NULL)
    RETURNING IDPET INTO v_pet2;

    INSERT INTO TBLEMBRETE
    (IDPET, DSTIPO, DSDESCRICAO, DTLEMBRETE, DTNOTIFICADO, DSSTATUS, DTCADASTRO)
    VALUES
    (v_pet1, 'Vacina', 'Aplicar vacina V10', DATE '2026-05-20', NULL, 'PENDENTE', SYSDATE);

    INSERT INTO TBLEMBRETE
    (IDPET, DSTIPO, DSDESCRICAO, DTLEMBRETE, DTNOTIFICADO, DSSTATUS, DTCADASTRO)
    VALUES
    (v_pet1, 'Banho', 'Banho mensal', DATE '2026-05-25', NULL, 'PENDENTE', SYSDATE);

    INSERT INTO TBLEMBRETE
    (IDPET, DSTIPO, DSDESCRICAO, DTLEMBRETE, DTNOTIFICADO, DSSTATUS, DTCADASTRO)
    VALUES
    (v_pet2, 'Consulta', 'Retorno veterinario', DATE '2026-06-10', NULL, 'PENDENTE', SYSDATE);

    COMMIT;
END;
/

SELECT * FROM TBCLINICA;
SELECT * FROM TBTUTOR;
SELECT * FROM TBPET;
SELECT * FROM TBLEMBRETE;
