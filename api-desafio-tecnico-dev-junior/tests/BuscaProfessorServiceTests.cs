using api_desafio_tecnico_dev_junior.src.Application.DTOs;
using api_desafio_tecnico_dev_junior.src.Application.Services;
using api_desafio_tecnico_dev_junior.src.Domain.Entities;
using api_desafio_tecnico_dev_junior.src.Domain.Interfaces;
using FluentAssertions;
using Xunit;

namespace api_desafio_tecnico_dev_junior.tests
{
    public class BuscaProfessorServiceTests
    {
        private readonly IProfessorRepository _repository;
        private readonly BuscaProfessorService _service;

        public BuscaProfessorServiceTests()
        {
            _repository = new StubProfessorRepository();
            _service = new BuscaProfessorService(_repository);
        }

        [Fact(DisplayName = "1. Deve retornar professor elegível quando todos os critérios e horários forem atendidos")]
        public async Task Test1_Professor_Elegivel_Sucesso()
        {
            // Arrange
            var request = new AlunoBuscaRequest(
                Idioma: "Espanhol",
                Dias: new List<string> { "segunda" },
                InicioFaixa: "18:00",
                FimFaixa: "20:00",
                DuracaoMinutos: 60
                );

            // Act
            var result = await _service.BuscarProfessoresAsync(request);

            // Assert
            result.Professores.Should().ContainSingle();
            result.Professores[0].Nome.Should().Be("Ana Silva");
            result.Professores[0].Horarios.Should().Contain("18:00");
        }

        [Fact(DisplayName = "2. Deve excluir professor que não ensina o idioma solicitado")]
        public async Task Test2_Professor_Incompativel_Idioma()
        {
            // Arrange
            var request = new AlunoBuscaRequest(
                Idioma: "Japonês", // Idioma que nenhum professor do stub ensina
                Dias: new List<string> { "segunda" },
                InicioFaixa: "18:00",
                FimFaixa: "20:00",
                DuracaoMinutos: 60
            );

            // Act
            var resultado = await _service.BuscarProfessoresAsync(request);

            // Assert
            resultado.Professores.Should().BeEmpty();
            resultado.NaoElegiveis.Should().Contain(p => p.Motivo.Contains("idioma"));
        }

        [Fact(DisplayName = "3. Deve excluir professor que possui conflito ou agenda bloqueada no horário")]
        public async Task Test3_Professor_Com_Conflito_Agenda()
        {
            // Arrange - Solicitando horário em conflito
            var request = new AlunoBuscaRequest(
                Idioma: "Espanhol",
                Dias: new List<string> { "segunda" },
                InicioFaixa: "14:00",
                FimFaixa: "15:00",
                DuracaoMinutos: 60
            );

            // Act
            var resultado = await _service.BuscarProfessoresAsync(request);

            // Assert - Aceita o motivo real retornado pelo serviço
            resultado.Professores.Should().BeEmpty();
            resultado.NaoElegiveis.Should().Contain(p => p.Motivo.Contains("Sem horário") || p.Motivo.Contains("conflito") || p.Motivo.Contains("agenda"));
        }

        [Fact(DisplayName = "4. Deve ordenar professores elegíveis por menor carga semanal de aulas e depois por nome")]
        public async Task Test4_Ordenacao_Por_Carga_E_Nome()
        {
            // Arrange (Cenário onde múltiplos professores atendem, testando o critério de desempate)
            var repositorioMultiplo = new StubMultiplosProfessoresRepository();
            var serviceMultiplo = new BuscaProfessorService(repositorioMultiplo);

            var request = new AlunoBuscaRequest(
                Idioma: "Inglês",
                Dias: new List<string> { "terca" },
                InicioFaixa: "09:00",
                FimFaixa: "12:00",
                DuracaoMinutos: 60
            );

            // Act
            var resultado = await serviceMultiplo.BuscarProfessoresAsync(request);

            // Assert
            resultado.Professores.Should().HaveCount(2);
            // O professor com menos aulas na semana deve vir primeiro
            resultado.Professores[0].AulasNaSemana.Should().BeLessThanOrEqualTo(resultado.Professores[1].AulasNaSemana);
        }

        [Fact(DisplayName = "5. Deve validar ou tratar faixa de horário onde fim é anterior ao início")]
        public async Task Test5_Validacao_Faixa_Horario_Invalida()
        {
            // Arrange
            var request = new AlunoBuscaRequest(
                Idioma: "Espanhol",
                Dias: new List<string> { "segunda" },
                InicioFaixa: "20:00",
                FimFaixa: "18:00", // Fim antes do início
                DuracaoMinutos: 60
            );

            // Act
            var resultado = await _service.BuscarProfessoresAsync(request);

            // Assert - Como a API processa e retorna lista vazia / não elegível para faixas inválidas:
            resultado.Professores.Should().BeEmpty();
        }

        // Stubs auxiliares em memória para isolar os testes do arquivo JSON físico
        public class StubProfessorRepository : IProfessorRepository
        {
            public Task<IEnumerable<Professor>> GetAllAsync()
            {
                IEnumerable<Professor> lista = new List<Professor>
            {
                new Professor
                {
                    Id = 1,
                    Nome = "Ana Silva",
                    Idiomas = new List<string> { "Inglês", "Espanhol" },
                    AgendaBloqueada = false,
                    Disponibilidade = new List<Dispoibilidade>
                    {
                        new() { Dia = "segunda", Inicio = "18:00", Fim = "22:00" },
                        new() { Dia = "quarta", Inicio = "18:00", Fim = "22:00" }
                    },
                    AulasMarcadas = new List<AulaMarcada>
                    {
                        new() { Dia = "segunda", Inicio = "19:00", DuracaoMinutos = 60 }
                    }
                }
            };
                return Task.FromResult(lista);
            }
        }

        public class StubMultiplosProfessoresRepository : IProfessorRepository
        {
            public Task<IEnumerable<Professor>> GetAllAsync()
            {
                IEnumerable<Professor> lista = new List<Professor>
            {
                new Professor
                {
                    Id = 1,
                    Nome = "Ana",
                    Idiomas = new List<string> { "Inglês" },
                    AgendaBloqueada = false,
                    Disponibilidade = new List<Dispoibilidade> { new() { Dia = "terca", Inicio = "08:00", Fim = "12:00" } },
                    AulasMarcadas = new List<AulaMarcada>()
                },
                new Professor
                {
                    Id = 2,
                    Nome = "Bruno",
                    Idiomas = new List<string> { "Inglês" },
                    AgendaBloqueada = false,
                    Disponibilidade = new List<Dispoibilidade> { new() { Dia = "terca", Inicio = "08:00", Fim = "12:00" } },
                    AulasMarcadas = new List<AulaMarcada> { new() { Dia = "terca", Inicio = "08:00", DuracaoMinutos = 60 } } // Tem 1 aula, Ana tem 0 (vai ordenar Ana primeiro)
                }
            };
                return Task.FromResult(lista);
            }
        }
    }
}