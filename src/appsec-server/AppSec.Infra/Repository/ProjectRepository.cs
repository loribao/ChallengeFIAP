using System.Data.Common;
using AppSec.Domain.Entities;
using AppSec.Domain.Interfaces.IRepository;
using Microsoft.Extensions.Logging;

namespace AppSec.Infra.Repository
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly ContextAppSec context;
        private readonly ILogger<ProjectRepository> _logger;

        public ProjectRepository(ContextAppSec _context, ILogger<ProjectRepository> _logger)
        {
            this.context = _context;
            this._logger = _logger;
        }

        public async Task Create(ProjectEntity project, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation($"Creating project: {project.Name}");
                await context.AddAsync(project);
                context.SaveChanges();
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public async Task Delete(ProjectEntity project, CancellationToken cancellationToken = default)
        {
            this._logger.LogInformation($"Deleting project: {project.Name}");
            this.context.Projects.Remove(project);
            this.context.SaveChanges();
        }
        public async Task Update(ProjectEntity project, CancellationToken cancellationToken = default)
        {
            try
            {
                this._logger.LogInformation($"Updating project: {project.Name}");
                this.context.Projects.Update(project);
                this.context.SaveChanges();
            }
            catch (System.Exception ex)
            {
                this._logger.LogError(ex.Message);
                throw;
            }

        }
        public ProjectEntity? GetById(int id)
        {
            this._logger.LogInformation($"Getting project by id: {id}");
            return this.context.Projects.FirstOrDefault(p => p.Id == id);
        }
    }
}
