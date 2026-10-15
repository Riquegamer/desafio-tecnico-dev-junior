namespace api_desafio_tecnico_dev_junior.src.Domain.Entities
{
    public class Professor
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<string> Idiomas { get; set; } = [];
        public bool AgendaBloqueada { get; set; } = false;
        public List<Dispoibilidade> Disponibilidade { get; set; } = [];
        public List<AulaMarcada> AulasMarcadas { get; set; } = [];
    }

}
