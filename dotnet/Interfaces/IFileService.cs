using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Files;
using Sabio.Models.Requests.Files;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IFileService
    {
        Task<List<BaseFile>> AddFile(List<IFormFile> files, int userId);
        Paged<Files> FilePagination(int pageIndex, int pageSize);
        Paged<Files> FileSearchPagination(int pageIndex, int pageSize, int createdByIdQuery);
        void FileDelete(int id);
    }
}