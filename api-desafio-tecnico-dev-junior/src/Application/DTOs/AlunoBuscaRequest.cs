namespace api_desafio_tecnico_dev_junior.src.Application.DTOs
{
    public record AlunoBuscaRequest
    (
        string Idioma,
        List<string> Dias,
        string InicioFaixa,
        string FimFaixa,
        int DuracaoMinutos
    );
}
