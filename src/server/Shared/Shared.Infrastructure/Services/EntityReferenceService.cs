using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Shared.Core.Entities;
using Shared.Core.IntegrationServices.Application;
using Shared.Core.Interfaces;

namespace Shared.Infrastructure.Services
{
    public class EntityReferenceService : IEntityReferenceService
    {
        private readonly IApplicationDbContext _context;
        private readonly IStringLocalizer<EntityReferenceService> _localizer;

        public EntityReferenceService(
            IApplicationDbContext context,
            IStringLocalizer<EntityReferenceService> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<string> TrackAsync(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName))
            {
                throw new ArgumentException(_localizer["Entity Name Can Not Be Empty"], nameof(entityName));
            }

            string referenceNumber;
            string monthYearString = DateTime.Now.ToString("MMyy");
            var record = _context.EntityReferences.FirstOrDefault(a => a.Entity == entityName && a.MonthYearString == monthYearString);
            if (record != null)
            {
                record.Increment();
                _context.EntityReferences.Update(record);
                referenceNumber = GenerateReferenceNumber(entityName, record.Count, monthYearString);
            }
            else
            {
                record = new EntityReference(entityName);
                _context.EntityReferences.Add(record);
                referenceNumber = GenerateReferenceNumber(entityName, record.Count, monthYearString);
            }

            await _context.SaveChangesAsync();
            return referenceNumber;
        }

        private string GenerateReferenceNumber(string entity, int count, string monthYearString)
        {
            return $"{entity[0]}-{monthYearString}-{count.ToString().PadLeft(5, '0')}";
        }
    }
}