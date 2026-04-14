using SeguroVeiculos.Domain.Common;

namespace SeguroVeiculos.Domain.Entities;

public class Segurado
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Cpf { get; private set; } = string.Empty;
    public int Idade { get; private set; }

    private Segurado() { }

    private Segurado(string nome, string cpf, int idade)
    {
        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = cpf;
        Idade = idade;
    }

    public static Result<Segurado> Criar(string nome, string cpf, int idade)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result<Segurado>.Validation("O nome do segurado é obrigatório.");
        if (string.IsNullOrWhiteSpace(cpf))
            return Result<Segurado>.Validation("O CPF do segurado é obrigatório.");
        if (idade <= 0 || idade > 120)
            return Result<Segurado>.Validation("A idade do segurado deve ser válida.");

        return Result<Segurado>.Ok(new Segurado(nome, cpf, idade));
    }

    public Result<Segurado> Atualizar(string nome, string cpf, int idade)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result<Segurado>.Validation("O nome do segurado é obrigatório.");
        if (string.IsNullOrWhiteSpace(cpf))
            return Result<Segurado>.Validation("O CPF do segurado é obrigatório.");
        if (idade <= 0 || idade > 120)
            return Result<Segurado>.Validation("A idade do segurado deve ser válida.");

        Nome = nome;
        Cpf = cpf;
        Idade = idade;

        return Result<Segurado>.Ok(this);
    }
}

