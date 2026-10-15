using api_desafio_tecnico_dev_junior.src.Application.DTOs;
using System.Globalization;

namespace api_desafio_tecnico_dev_junior.src.Application.Validators
{
    public static class AlunoBuscaValidator
    {
        private static readonly HashSet<string> DiasValidos = new(StringComparer.OrdinalIgnoreCase)
        {
            "segunda", "terça", "terca", "quarta", "quinta", "sexta", "sábado", "sabado", "domingo"
        };

        public static string? Validar(AlunoBuscaRequest req)
        {
            if (req == null)
                return "O corpo da requisição não pode estar vazio.";

            // 1. Idioma vazio
            if (string.IsNullOrWhiteSpace(req.Idioma))
                return "O campo 'idioma' é obrigatório.";

            // 2. Nenhum dia informado
            if (req.Dias == null || req.Dias.Count == 0)
                return "Informe ao menos um dia da semana em 'dias'.";

            // 3. Algum dia é inválido
            foreach (var dia in req.Dias)
            {
                if (string.IsNullOrWhiteSpace(dia) || !DiasValidos.Contains(dia.Trim()))
                    return $"O dia '{dia}' é inválido. Utilize: segunda, terca, quarta, quinta, sexta, sabado ou domingo.";
            }

            // 4. Horário fora do formato HH:mm
            if (!TimeOnly.TryParseExact(req.InicioFaixa, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var inicio))
                return "O horário de início da faixa ('inicioFaixa') está em um formato inválido. Utilize o formato HH:mm.";

            if (!TimeOnly.TryParseExact(req.FimFaixa, "HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var fim))
                return "O horário de fim da faixa ('fimFaixa') está em um formato inválido. Utilize o formato HH:mm.";

            // 5. O fim da faixa é igual ou anterior ao início
            if (fim <= inicio)
                return "O horário de término ('fimFaixa') deve ser posterior ao horário de início ('inicioFaixa').";

            // 6. A duração é diferente de 30, 60 ou 90 minutos
            if (req.DuracaoMinutos != 30 && req.DuracaoMinutos != 60 && req.DuracaoMinutos != 90)
                return "A duração da aula ('duracaoMinutos') deve ser exatamente de 30, 60 ou 90 minutos.";

            return null; // Tudo válido!
        }
    }
}