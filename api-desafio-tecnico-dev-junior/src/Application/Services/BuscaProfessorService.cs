using api_desafio_tecnico_dev_junior.src.Application.DTOs;
using api_desafio_tecnico_dev_junior.src.Domain.Entities;
using api_desafio_tecnico_dev_junior.src.Domain.Interfaces;

namespace api_desafio_tecnico_dev_junior.src.Application.Services
{
    public class BuscaProfessorService
    {
        private readonly IProfessorRepository _repository;

        public BuscaProfessorService(IProfessorRepository repository)
        {
            _repository = repository;
        }

        public async Task<BuscaProfessorResponseDto> BuscarProfessoresAsync(AlunoBuscaRequest req)
        {
            var professores = await _repository.GetAllAsync();

            var listaElegiveis = new List<ProfessorElegivelDto>();
            var listaNaoElegiveis = new List<ProfessorNaoElegivelDto>();

            var alunoInicioFaixa = TimeOnly.Parse(req.InicioFaixa);
            var alunoFimFaixa = TimeOnly.Parse(req.FimFaixa);

            foreach (var prof in professores)
            {

                var dominaIdioma = prof.Idiomas.Any(i => i.Equals(req.Idioma, StringComparison.OrdinalIgnoreCase));
                if (!dominaIdioma)
                {
                    listaNaoElegiveis.Add(new ProfessorNaoElegivelDto(prof.Id, prof.Nome, "Não ensina o idioma"));
                    continue;
                }

                if (prof.AgendaBloqueada)
                {
                    listaNaoElegiveis.Add(new ProfessorNaoElegivelDto(prof.Id, prof.Nome, "Agenda bloqueada"));
                    continue;
                }

                var horariosComuns = new List<TimeOnly>();
                bool primeiroDia = true;

                foreach (var diaSolicitado in req.Dias)
                {
                    var horariosDisponiveisNoDia = new List<TimeOnly>();

                    var disponibilidadesDoDia = prof.Disponibilidade
                        .Where(h => h.Dia.Equals(diaSolicitado, StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    foreach (var disp in disponibilidadesDoDia)
                    {
                        var profInicio = TimeOnly.Parse(disp.Inicio);
                        var profFim = TimeOnly.Parse(disp.Fim);

                        var janelaInicio = alunoInicioFaixa > profInicio ? alunoInicioFaixa : profInicio;
                        var janelaFim = alunoFimFaixa < profFim ? alunoFimFaixa : profFim;

                        if (janelaInicio < janelaFim)
                        {
                            var slotAtual = ArredondarParaHoraOuMeiaHora(janelaInicio);
                            if (slotAtual < janelaInicio) slotAtual = slotAtual.AddMinutes(30);

                            while (slotAtual.AddMinutes(req.DuracaoMinutos) <= janelaFim)
                            {
                                var slotFim = slotAtual.AddMinutes(req.DuracaoMinutos);

                                var aulasDoDia = prof.AulasMarcadas != null
                                    ? prof.AulasMarcadas.Where(a => a.Dia.Equals(diaSolicitado, StringComparison.OrdinalIgnoreCase))
                                    : new List<AulaMarcada>();

                                var temConflito = aulasDoDia.Any(aula =>
                                    HouveConflitoDeHorario(slotAtual, slotFim, TimeOnly.Parse(aula.Inicio), aula.DuracaoMinutos)
                                );

                                if (!temConflito)
                                {
                                    if (!horariosDisponiveisNoDia.Contains(slotAtual))
                                    {
                                        horariosDisponiveisNoDia.Add(slotAtual);
                                    }
                                }

                                slotAtual = slotAtual.AddMinutes(30);
                            }
                        }
                    }


                    if (primeiroDia)
                    {
                        horariosComuns = horariosDisponiveisNoDia;
                        primeiroDia = false;
                    }
                    else
                    {
                        horariosComuns = horariosComuns.Intersect(horariosDisponiveisNoDia).ToList();
                    }

                    if (horariosComuns.Count == 0) break;
                }
                if (horariosComuns.Count > 0)
                {
                    var horariosFormatados = horariosComuns
                        .OrderBy(h => h)
                        .Select(h => h.ToString("HH:mm"))
                        .ToList();

                    listaElegiveis.Add(new ProfessorElegivelDto(
                        prof.Id,
                        prof.Nome,
                        prof.AulasMarcadas?.Count ?? 0,
                        horariosFormatados
                    ));
                }

                else
                {
                    listaNaoElegiveis.Add(new ProfessorNaoElegivelDto(
                        prof.Id,
                        prof.Nome,
                        "Sem horário em comum nos dias pedidos"
                    ));
                }

            }
            var professoresOrdenados = listaElegiveis
            .OrderBy(p => p.AulasNaSemana)
            .ThenBy(p => p.Nome)
            .ToList();

            return new BuscaProfessorResponseDto(professoresOrdenados, listaNaoElegiveis);
        }



        private TimeOnly ArredondarParaHoraOuMeiaHora(TimeOnly hora)
        {
            int minuto = hora.Minute;
            if (minuto > 0 && minuto < 30)
                return new TimeOnly(hora.Hour, 30);
            if (minuto > 30)
                return new TimeOnly((hora.Hour + 1) % 24, 0);
            return new TimeOnly(hora.Hour, minuto);
        }
        private bool HouveConflitoDeHorario(TimeOnly inicio1, TimeOnly fim1, TimeOnly inicio2, int duracaoMinutos2)
        {
            var fim2 = inicio2.AddMinutes(duracaoMinutos2);
            return inicio1 < fim2 && fim1 > inicio2;
        }

    
        public async Task<IEnumerable<string>> ObterIdiomasDisponiveisAsync()
        {
            var professores = await _repository.GetAllAsync();
            var idiomasUnicos = professores
                .SelectMany(p => p.Idiomas)
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .OrderBy(i => i)
                .ToList();
            return idiomasUnicos;
        }

    }
}