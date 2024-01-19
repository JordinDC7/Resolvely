using Sabio.Models.Domain.Modules;
using Sabio.Models.Requests;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IModuleService
    {
        Module Get(int id);
        List<Module> GetAll();
        int Add(ModuleAddRequest model, int userId);
        void Update(ModuleUpdateRequest model, int userId);
        void Delete(int id);
    }
}