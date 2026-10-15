namespace api_desafio_tecnico_dev_junior.src.Application.DTOs
{
    public record BuscaProfessorResponseDto
    (
        List<ProfessorElegivelDto> Professores,
        List<ProfessorNaoElegivelDto> NaoElegiveis
    );
}
