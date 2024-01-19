using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IBlogService
    {
        int AddBlog(BlogAddRequest model, int userId);
        void Delete(int Id);
        Paged<Blog> GetAllBlogsPaginated(int pageIndex, int pageSize);
        Paged<Blog> GetBlogAuthorsPaginated(int pageIndex, int pageSize, int authorId);
        Blog GetBlog(int id);
        Paged<Blog> GetBlogCategory(int pageIndex, int pageSize, int categoryId);
        void Update(BlogUpdateRequest model, int userId);
    }
}