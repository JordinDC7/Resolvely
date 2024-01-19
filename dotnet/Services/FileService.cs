using Sabio.Models.Requests.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabio.Data.Providers;
using System.Data.SqlClient;
using System.Data;
using Sabio.Services.Interfaces;
using Sabio.Data;
using Sabio.Models;
using Sabio.Models.Domain.Files;
using System.Net;
using System.Reflection.PortableExecutable;
using Microsoft.AspNetCore.Http.HttpResults;
using Sabio.Models.Domain;
using Microsoft.AspNetCore.Http;
using Sabio.Models.AppSettings;
using System.IO;
using System.Reflection;
using Amazon.S3.Transfer;
using Amazon.S3;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Sabio.Models.Enums;
using Amazon;
using Sabio.Models.Domain.Users;

namespace Sabio.Services.Services
{
    public class FileService : IFileService
    {
        IDataProvider _data = null;
        ILookUpService _lookUpService = null;
        private AwsKeys _awsKeys;
        private static IAmazonS3 s3Client;
        private readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest2;

        public FileService(IDataProvider data, ILookUpService lookUpService, IOptions<AwsKeys> awsKeys)
        {
            _data = data;
            _lookUpService = lookUpService;
            _awsKeys = awsKeys.Value;
        }

        public async Task<List<BaseFile>> AddFile(List<IFormFile> files, int userId)
        {
            string procName = "[dbo].[Files_Insert]";

            DataTable fileTable = null;

            List<BaseFile> resultList = null;

            List<FileAddRequest> models = new List<FileAddRequest>();

            FileAddRequest model = null;

            foreach (IFormFile file in files)
            {
                string fileGuid = Guid.NewGuid().ToString();

                bool uploadSuccess = await UploadFileAsync(file, fileGuid);

                if (uploadSuccess)
                {
                    model = new FileAddRequest();

                    model.Name = Path.GetFileNameWithoutExtension(file.FileName);
                    model.Url = $"{_awsKeys.Domain}{file.FileName}{fileGuid}";
                    model.FileTypeId = (int)GetFileType(Path.GetExtension(file.FileName).Trim('.'));
                    models.Add(model);

                }

            }

            fileTable = AddCommonParams(models);

            _data.ExecuteCmd(procName,
                     delegate (SqlParameterCollection col)
                     {
                         col.AddWithValue("@CreatedBy", userId);
                         col.AddWithValue("@BatchFiles", fileTable);

                     }, delegate (IDataReader reader, short set)
                     {
                         var result = BaseFileSingleMapper(reader);
                         if (resultList == null)
                         {
                             resultList = new List<BaseFile>();
                         }
                         resultList.Add(result);
                     });

            return resultList;
        }

        private async Task<bool> UploadFileAsync(IFormFile file, string fileGuid)
        {
            try
            {
                s3Client = new AmazonS3Client(_awsKeys.AccessKey, _awsKeys.Secret, bucketRegion);

                var fileTransferUtility = new TransferUtility(s3Client);

                await fileTransferUtility.UploadAsync(file.OpenReadStream(), _awsKeys.BucketName, $"{file.FileName}{fileGuid}");

            }
            catch (AmazonS3Exception e)
            {

                throw new Exception($"Error encountered on server. Message:'{0}' when writing an object: {e.Message}");

            }
            catch (Exception e)
            {

                throw new Exception($"Error encountered on server. Message:'{0}' when writing an object: {e.Message}");

            }

            return true;

        }

        public Paged<Files> FilePagination(int pageIndex, int pageSize)
        {
            Paged<Files> pageList = null;
            List<Files> list = null;
            string procName = "[dbo].[Files_SelectAll_Pagination]";
            int totalCount = 0;

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            },
            (reader, recordSetIndex) =>
            {
                int index = 0;
                Files file = MapSingleFile(reader, ref index);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);
                }

                if (list == null)
                {
                    list = new List<Files>();
                }
                list.Add(file);
            });
            if (list != null)
            {
                pageList = new Paged<Files>(list, pageIndex, pageSize, totalCount);
            }
            return pageList;
        }

        public Paged<Files> FileSearchPagination(int pageIndex, int pageSize, int createdByIdQuery)
        {
            Paged<Files> pageList = null;
            List<Files> list = null;
            string procName = "[dbo].[Files_Select_ByCreatedBy_Pagination]";
            int totalCount = 0;

            _data.ExecuteCmd(procName, (param) =>
            {
                param.AddWithValue("@CreatedBy", createdByIdQuery);
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            },
            (reader, recordSetIndex) =>
            {
                int index = 0;
                Files file = MapSingleFile(reader, ref index);
                if (totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index++);

                }

                if (list == null)
                {
                    list = new List<Files>();
                }
                list.Add(file);
            });
            if (list != null)
            {
                pageList = new Paged<Files>(list, pageIndex, pageSize, totalCount);
            }
            return pageList;
        }

        public void FileDelete(int id)
        {
            string procName = "[dbo].[Files_Delete_ById]";

            _data.ExecuteNonQuery(procName,
                inputParamMapper: delegate (SqlParameterCollection col)
                {

                    col.AddWithValue("@Id", id);

                }, returnParameters: null);
        }

        private static FileType GetFileType(string fileType)
        {
            switch (fileType.ToLower())
            {
                case "jpg":
                    return FileType.jpg;
                case "pdf":
                    return FileType.pdf;
                case "jpeg":
                    return FileType.jpeg;
                case "doc":
                    return FileType.doc;
                case "png":
                    return FileType.png;
                case "gif":
                    return FileType.gif;
                case "webp":
                    return FileType.webp;
                case "svg":
                    return FileType.svg;
                case "html":
                    return FileType.html;
                case "xhtml":
                    return FileType.xhtml;
                default:
                    return FileType.jpg;
            }
        }

        private static BaseFile BaseFileSingleMapper(IDataReader reader)
        {
            int index = 0;
            BaseFile file = new BaseFile();

            {
                file.Id = reader.GetSafeInt32(index++);
                file.Url = reader.GetString(index++);
            }

            return file;
        }

        private static DataTable AddCommonParams(List<FileAddRequest> model)
        {
            DataTable fileTable = new DataTable();

            fileTable.Columns.Add("Name", typeof(string));
            fileTable.Columns.Add("Url", typeof(string));
            fileTable.Columns.Add("FileType", typeof(int));

            foreach (var file in model)
            {
                DataRow fileRow = fileTable.NewRow();
                int startingIndex = 0;

                fileRow[startingIndex++] = file.Name;
                fileRow[startingIndex++] = file.Url;
                fileRow[startingIndex++] = file.FileTypeId;

                fileTable.Rows.Add(fileRow);
            }

            return fileTable;
        }

        private Files MapSingleFile(IDataReader reader, ref int index)
        {
            Files file = new Files();

            file.Id = reader.GetSafeInt32(index++);
            file.Name = reader.GetSafeString(index++);
            file.Url = reader.GetSafeString(index++);
            file.FileType = _lookUpService.MapSingleLookUp(reader, ref index);
            file.CreatedBy = reader.DeserializeObject<BaseUser>(index++);
            file.DateCreated = reader.GetSafeDateTime(index++);

            return file;
        }

    }
}
