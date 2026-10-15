using api_desafio_tecnico_dev_junior.src.Domain.Entities;

namespace api_desafio_tecnico_dev_junior.src.Domain.Interfaces
{
    public interface IProfessorRepository
    {
        Task<IEnumerable<Professor>> GetAllAsync();
    }
}
