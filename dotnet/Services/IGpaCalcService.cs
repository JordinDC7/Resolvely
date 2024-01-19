using Sabio.Models.Domain;
using Sabio.Models.Requests;
using System.Collections.Generic;

namespace Sabio.Services.Services
{
    public interface IGpaCalcService
    {
        void AddGpaCalc(GpaCalcsAddRequest model, int userId);
        void UpdateGpaCalc(GpaCalcUpdateRequest model, int userId);
        List<GpaCalc> GetByLvlTypeId(int id);
        List<GpaCalc> GetAll(int id);
        void DeleteCalc(int id);
    }
}