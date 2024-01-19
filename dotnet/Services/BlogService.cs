using Sabio.Data.Providers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data;
using Sabio.Services.Interfaces;
using Sabio.Models.Domain;
using Sabio.Models;
using Sabio.Models.Requests;
using Sabio.Models.Domain.Users;

namespace Sabio.Services.Services
{
    public class BlogService : IBlogService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;
        public BlogService(IDataProvider data, ILookUpService lookUpService)
        {
            _data = data;
            _lookUpService = lookUpService;
        }

        public Blog GetBlog(int id)
        {
            string procName = "[dbo].[Blogs_Select_ById]";
            Blog blog = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)
            {
                int index = 0;
                blog = MapSingleBlog(reader, ref index);
            }
            );
            return blog;
        }
        public Paged<Blog> GetAllBlogsPaginated(int pageIndex, int pageSize)
        {
            List<Blog> list = null;
            Paged<Blog> pagedList = null;
            string procName = "[dbo].[Blogs_SelectAll_Paginated]";
            int totalCount = 0;
            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection param)
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            },
              delegate (IDataReader reader, short set)
              {
                  int index = 0;
                  Blog blog = MapSingleBlog(reader, ref index);
                  if (totalCount == 0)
                  {
                      totalCount = reader.GetSafeInt32(index++);
                  }
                  if (list == null)
                  {
                      list = new List<Blog>();
                  }

                  list.Add(blog);
              }
              );
            if (list != null)
            {
                pagedList = new Paged<Blog>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<Blog> GetBlogAuthorsPaginated(int pageIndex, int pageSize, int authorId)
        {
            Paged<Blog> pagedList = null;
            List<Blog> list = null;
            string procName = "[dbo].[Blogs_Select_ByCreatedBy_Paginated]";
            int totalCount = 0;
            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@AuthorId", authorId);
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            }, (reader, recordSetIndex) =>
            {
                int index = 0;
                Blog blog = MapSingleBlog(reader, ref index);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }
                if (list == null)
                {
                    list = new List<Blog>();
                }
                list.Add(blog);
            });
            if (list != null)
            {
                pagedList = new Paged<Blog>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public Paged<Blog> GetBlogCategory(int pageIndex, int pageSize, int categoryId)
        {

            List<Blog> list = null;
            Paged<Blog> pagedList = null;
            string procName = "[dbo].[Blogs_Select_BlogCategory_Paginated]";
            int totalCount = 0;
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@BlogCategoryId", categoryId);
                paramCollection.AddWithValue("@PageIndex", pageIndex);
                paramCollection.AddWithValue("@PageSize", pageSize);


            }, delegate (IDataReader reader, short set)
            {
                int index = 0;
                Blog blog = MapSingleBlog(reader, ref index);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }
                if (list == null)
                {
                    list = new List<Blog>();
                }
                list.Add(blog);
            }
            );
            if (list != null)
            {
                pagedList = new Paged<Blog>(list, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        public int AddBlog(BlogAddRequest model, int userId)
        {
            int id = 0;
            string procName = "[dbo].[Blogs_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {


                AddCommonParams(model, col);
                col.AddWithValue("@AuthorId", userId);
                //and 1 output

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);
            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@id"].Value;

                int.TryParse(oId.ToString(), out id);
            }
            );
            return id;
        }


        public void Update(BlogUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Blogs_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);
                col.AddWithValue("@AuthorId", userId);
            },
             returnParameters: null);
        }
        public void Delete(int id)
        {
            string procName = "[dbo].[Blogs_Delete]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            });
        }
        private Blog MapSingleBlog(IDataReader reader, ref int index)
        {
            Blog aBlog = new Blog();

            aBlog = new Blog();

            aBlog.Id = reader.GetSafeInt32(index++);
            aBlog.Category = _lookUpService.MapSingleLookUp(reader, ref index);
            aBlog.Author = reader.DeserializeObject<BaseUser>(index++);
            aBlog.Title = reader.GetSafeString(index++);
            aBlog.Subject = reader.GetSafeString(index++);
            aBlog.Content = reader.GetSafeString(index++);
            aBlog.IsPublished = reader.GetSafeBool(index++);
            aBlog.ImageUrl = reader.GetSafeString(index++);
            aBlog.DateCreated = reader.GetSafeDateTime(index++);
            aBlog.DateModified = reader.GetSafeDateTime(index++);
            aBlog.DatePublish = reader.GetSafeDateTime(index++);
            return aBlog;
        }
        private static void AddCommonParams(BlogAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@BlogCategoryId", model.BlogCategoryId);
            col.AddWithValue("@Title", model.Title);
            col.AddWithValue("@Subject", model.Subject);
            col.AddWithValue("@Content", model.Content);
            col.AddWithValue("@IsPublished", model.IsPublished);
            col.AddWithValue("@ImageUrl", model.ImageUrl);
        }
    }
}
