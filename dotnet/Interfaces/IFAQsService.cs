using Sabio.Models.Domain.FAQs;
using Sabio.Models.Requests.FAQs;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IFAQsService
    {
        void Delete(int id);

        int Add(FAQsAddRequest requestModel, int createdById);
        
        List<FAQ> GetAllFAQs();
        
        List<FAQ> GetByCategoryId(int id); 
        
        void UpdateFAQ(FAQUpdateRequest requestModel, int modifiedById);
    }
}
