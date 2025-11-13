using Microsoft.EntityFrameworkCore;
using Rentix.Domain.Entities;
using Rentix.Domain.Repositories;

namespace Rentix.Infrastructure.Persistence.Repositories
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public DocumentRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Document?> GetByIdAsync(int id)
        {
            return await _dbContext.Documents
                .Include(d => d.Property)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<Document> AddAsync(Document document)
        {
            await _dbContext.Documents.AddAsync(document);
            return document;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var doc = await _dbContext.Documents.FindAsync(id);
            if (doc == null)
                return false;

            _dbContext.Documents.Remove(doc);
            return true;
        }

        public async Task<List<Document>> GetByPropertyIdAsync(int propertyId)
        {
            return await _dbContext.Documents
                .Where(d => d.PropertyId == propertyId)
                .ToListAsync();
        }

        public async Task<List<Document>> GetByEntityAsync(DocumentEntityType entityType, int entityId)
        {
            return await _dbContext.Documents
                .Where(d => d.EntityType == entityType && d.EntityId == entityId)
                .ToListAsync();
        }
    }
}
