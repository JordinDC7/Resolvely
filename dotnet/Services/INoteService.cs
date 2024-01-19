using Sabio.Models;
using Sabio.Models.Domain.Notes;
using Sabio.Models.Requests;

namespace Sabio.Services.Services
{
    public interface INoteService
    {
        int Add(NoteAddRequest model);
        void DeleteById(int id);
        Notes Get(int id);
        Paged<Notes> SelectAllPaginated(int pageIndex, int pageSize);
        Paged<Notes> SelectByCreatedBy(int pageIndex, int pageSize, int createdBy);
        void Update(NoteUpdateRequest model);
    }
}