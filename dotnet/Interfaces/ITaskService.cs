using Sabio.Models.Domain.Tasks;
using Sabio.Models.Requests;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface ITaskService
    {
        Task Get(int id);
        List<Task> GetAll(int moduleId);
        int Add(TaskAddRequest model, int userId);
        void Update(TaskUpdateRequest model, int userId);
        void Delete(int id);
    }
}