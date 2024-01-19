using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models.Domain;
using Sabio.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Sabio.Services.Services
{
    public class GpaCalcService : IGpaCalcService
    {
        IDataProvider _data = null;


        public GpaCalcService(IDataProvider data)
        {
            _data = data;
        }

        public void AddGpaCalc(GpaCalcsAddRequest model, int userId)
        {
            DataTable myParamValue = null;


            if (model.GpaCalc != null)
            {
                myParamValue = MapModelsToTable(model.GpaCalc, userId);
            }


            string procName = "[dbo].[GpaCalc_InsertBatch]";

            _data.ExecuteNonQuery(procName,
              inputParamMapper: delegate (SqlParameterCollection sqlParams)
              {
                  sqlParams.AddWithValue("@batchSemester", myParamValue);

              });



        }

        public void UpdateGpaCalc(GpaCalcUpdateRequest model, int userId)
        {
            string procName = "[dbo].[GpaCalc_Update]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection sqlParams)
            {
                sqlParams.AddWithValue("@LevelTypeId", model.LevelTypeId);
                sqlParams.AddWithValue("@CourseId", model.CourseId);
                sqlParams.AddWithValue("@GradeTypeId", model.GradeTypeId);
                sqlParams.AddWithValue("@CourseWeightTypeId", model.CourseWeightTypeId);
                sqlParams.AddWithValue("@Credits", model.Credits);
                sqlParams.AddWithValue("@Semester", model.Semester);
                sqlParams.AddWithValue("@CreatedById", userId);
                sqlParams.AddWithValue("@Id", model.Id);
            });
        }

        public List<GpaCalc> GetByLvlTypeId(int id)
        {

            string procName = "[dbo].[GpaCalc_SelectAll_ByLevelType]";
            List<GpaCalc> gpaCalc = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@LevelTypeId", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                GpaCalc calc = MapSingleCalc(reader);
                if (gpaCalc == null)
                {
                    gpaCalc = new List<GpaCalc>();
                }

                gpaCalc.Add(calc);
            });

            return gpaCalc;
        }

        public List<GpaCalc> GetAll(int id)
        {
            string procName = "[dbo].[GpaCalc_SelectAll]";
            List<GpaCalc> gpaCalc = null;

            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }, singleRecordMapper: delegate (IDataReader reader, short set)
            {
                GpaCalc calc = MapSingleCalc(reader);

                if (gpaCalc == null)
                {
                    gpaCalc = new List<GpaCalc>();
                }

                gpaCalc.Add(calc);


            });
            return gpaCalc;

        }

        public void DeleteCalc(int id)
        {

            string procName = "[dbo].[GpaCalc_Delete]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            });

        }

        private DataTable MapModelsToTable(List<GpaCalcAddRequest> model, int userId)
        {

            DataTable dt = new DataTable();


            dt.Columns.Add("LevelTypeId", typeof(int));
            dt.Columns.Add("CourseId", typeof(int));
            dt.Columns.Add("GradeTypeId", typeof(int));
            dt.Columns.Add("CourseWeightTypeId", typeof(int));
            dt.Columns.Add("Credits", typeof(decimal));
            dt.Columns.Add("Semester", typeof(int));
            dt.Columns.Add("CreatedById", typeof(int));

            foreach (GpaCalcAddRequest element in model)
            {

                DataRow dataRow = dt.NewRow();
                int startingIdex = 0;


                dataRow[startingIdex++] = element.LevelTypeId;
                dataRow[startingIdex++] = element.CourseId;
                dataRow[startingIdex++] = element.GradeTypeId;
                dataRow[startingIdex++] = element.CourseWeightTypeId;
                dataRow[startingIdex++] = element.Credits;
                dataRow[startingIdex++] = element.Semester;
                dataRow[startingIdex++] = userId;


                dt.Rows.Add(dataRow);
            }


            return dt;

        }
        private static GpaCalc MapSingleCalc(IDataReader reader)
        {
            GpaCalc calc = new GpaCalc();

            int index = 0;


            calc.Id = reader.GetInt32(index++);
            calc.LevelTypeId = reader.GetInt32(index++);
            calc.CourseId = reader.GetInt32(index++);
            calc.GradeTypeId = reader.GetInt32(index++);
            calc.CourseWeightTypeId = reader.GetInt32(index++);
            calc.Credits = reader.GetDecimal(index++);
            calc.Semester = reader.GetInt32(index++);
            calc.CreatedBy = reader.GetInt32(index++);
            return calc;
        }
    }
}
