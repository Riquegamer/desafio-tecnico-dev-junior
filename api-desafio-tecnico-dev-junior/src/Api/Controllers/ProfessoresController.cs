using api_desafio_tecnico_dev_junior.src.Application.DTOs;
using api_desafio_tecnico_dev_junior.src.Application.Services;
using api_desafio_tecnico_dev_junior.src.Application.Validators;
using Microsoft.AspNetCore.Mvc;

namespace api_desafio_tecnico_dev_junior.src.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ProfessoresController : ControllerBase
    {
        private readonly BuscaProfessorService _buscaProfessorService;

        public ProfessoresController(BuscaProfessorService buscaProfessorService)
        {
            _buscaProfessorService = buscaProfessorService;
        }

        [HttpPost("busca")]
        public async Task<ActionResult<BuscaProfessorResponseDto>> BuscarProfessores([FromBody] AlunoBuscaRequest req)
        {
            var erroValidacao = AlunoBuscaValidator.Validar(req);

            if (erroValidacao != null)
            {
                return BadRequest(new { message = erroValidacao });
            }

            try
            {
                var professores = await _buscaProfessorService.BuscarProfessoresAsync(req);
                return Ok(professores);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Ocorreu um erro ao processar a solicitação.", erro = ex.Message });
            }
        }

        [HttpGet("idiomas")]
        public async Task<ActionResult<IEnumerable<string>>> GetIdiomasDisponiveis()
        {
            var idiomasUnicos = await _buscaProfessorService.ObterIdiomasDisponiveisAsync();

            return Ok(idiomasUnicos);
        }
    }
}
