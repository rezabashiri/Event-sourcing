using System.Threading.Tasks;

namespace Shared.Core.IntegrationServices.Application
{
    public interface IEntityReferenceService
    {
        public Task<string> TrackAsync(string entityName);
    }
}