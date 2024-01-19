using Sabio.Models;
using Sabio.Models.Domain.Events;
using Sabio.Models.Requests.Events;

namespace Sabio.Services.Interfaces
{
    public interface IEventService
    {
        int Add(EventAddRequest request, int createdById);
        void Delete(int id);
        Event GetById(int id);
        Paged<Event> GetAll(int pageIndex, int pageSize);
        Paged<Event> GetByCreatedBy(int pageIndex, int pageSize, int userId);
        void Update(EventUpdateRequest model, int userId);
        public Paged<Event> GetByQuery(int pageIndex, int pageSize, string query);
    }
}