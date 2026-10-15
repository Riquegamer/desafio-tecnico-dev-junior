using api_desafio_tecnico_dev_junior.src.Domain.Entities;
using api_desafio_tecnico_dev_junior.src.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace api_desafio_tecnico_dev_junior.src.Infraestructure.Repositories
{
    public class JsonProfessorRepository : IProfessorRepository
    {
        private readonly string _filePath;
        private List<Professor>? _cacheProfessores;

        public JsonProfessorRepository(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath, "src/Infraestructure/Data", "professores.json");
        }

        public async Task<IEnumerable<Professor>> GetAllAsync()
        {
            if(_cacheProfessores != null)
            {
                return _cacheProfessores;
            }

            if (!File.Exists(_filePath))
            {
                throw new FileNotFoundException($"O arquivo '{_filePath}' não foi encontrado.");
            }

            var jsonString = await File.ReadAllTextAsync(_filePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            using var doc = await JsonDocument.ParseAsync(File.OpenRead(_filePath));

            if (doc.RootElement.TryGetProperty("professores", out var professoresElement))
            {
                _cacheProfessores = professoresElement.Deserialize<List<Professor>>(options) ?? new List<Professor>();

                Console.WriteLine($"[DEBUG] Professores carregados do JSON: {_cacheProfessores.Count}");
            }
            else
            {
                _cacheProfessores = new List<Professor>();
            }

            return _cacheProfessores;

        }
    }
}
