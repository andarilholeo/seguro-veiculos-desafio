namespace SeguroVeiculos.Domain.Entities;

public class Segurado
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Cpf { get; private set; } = string.Empty;
    public int Idade { get; private set; }

    private Segurado() { }

    public Segurado(string nome, string cpf, int idade)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do segurado é obrigatório.", nameof(nome));
        if (string.IsNullOrWhiteSpace(cpf))
            throw new ArgumentException("O CPF do segurado é obrigatório.", nameof(cpf));
        if (idade <= 0 || idade > 120)
            throw new ArgumentException("A idade do segurado deve ser válida.", nameof(idade));

        Id = Guid.NewGuid();
        Nome = nome;
        Cpf = cpf;
        Idade = idade;
    }
}

