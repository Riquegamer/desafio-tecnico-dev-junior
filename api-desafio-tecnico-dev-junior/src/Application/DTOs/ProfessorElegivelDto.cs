namespace api_desafio_tecnico_dev_junior.src.Application.DTOs
{
    public record ProfessorElegivelDto
    (
        int Id,
        string Nome,
        int AulasNaSemana,
        List<string> Horarios
    );
}
