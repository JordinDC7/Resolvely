using Sabio.Data.Providers;
using Sabio.Models.Requests;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using Sabio.Data;
using Sabio.Models;
using Sabio.Models.Domain.Notes;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Users;
using System.Runtime.CompilerServices;

namespace Sabio.Services.Services
{
    public class NoteService : INoteService
    {
        IDataProvider _data = null;

        public NoteService(IDataProvider data)
        {
            _data = data;

        }
        public int Add(NoteAddRequest model)
        {
            int Id = 0;

            string procName = "[dbo].[Notes_Insert]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);

                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);

            }, returnParameters: delegate (SqlParameterCollection returnCollection)
            {
                object oId = returnCollection["@Id"].Value;

                int.TryParse(oId.ToString(), out Id);
            });
            return Id;
        }

        public Paged<Notes> SelectAllPaginated(int pageIndex, int pageSize)
        {

            string procName = "[dbo].[Notes_SelectAll]";

            Paged<Notes> pagedList = null;
            List<Notes> theNotes = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection param)
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
            },

                delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Notes notes = MapSingleNote(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }
                    if (theNotes == null)
                    {
                        theNotes = new List<Notes>();
                    }
                    theNotes.Add(notes);
                });

            if (theNotes != null)
            {
                pagedList = new Paged<Notes>(theNotes, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }

        public void Update(NoteUpdateRequest model)
        {
            string procName = "[dbo].[Notes_Update]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col);
                col.AddWithValue("@Id", model.Id);

            }, returnParameters: null);
        }

        public void DeleteById(int id)
        {
            string procName = "[dbo].[Notes_Delete_ById]";

            _data.ExecuteNonQuery(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);
            });
        }

        public Notes Get(int id)
        {
            string procName = "[dbo].[Notes_Select_ById]";

            Notes notes = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCollection)
            {
                paramCollection.AddWithValue("@Id", id);

            }, delegate (IDataReader reader, short set)

            {
                int startingIndex = 0;

                notes = MapSingleNote(reader, ref startingIndex);
            });

            return notes;
        }
        public Paged<Notes> SelectByCreatedBy(int pageIndex, int pageSize, int createdBy)
        {
            string procName = "[dbo].[Notes_Select_ByCreatedBy]";

            Paged<Notes> pagedList = null;
            List<Notes> aNote = null;
            int totalCount = 0;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection param)
            {
                param.AddWithValue("@PageIndex", pageIndex);
                param.AddWithValue("@PageSize", pageSize);
                param.AddWithValue("@CreatedBy", createdBy);
            },

                delegate (IDataReader reader, short set)
                {
                    int startingIndex = 0;

                    Notes note = MapSingleNote(reader, ref startingIndex);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetSafeInt32(startingIndex++);
                    }

                    if (aNote == null)
                    {
                        aNote = new List<Notes>();
                    }
                    aNote.Add(note);
                });

            if (aNote != null)
            {
                pagedList = new Paged<Notes>(aNote, pageIndex, pageSize, totalCount);
            }
            return pagedList;
        }
        private static void AddCommonParams(NoteAddRequest model, SqlParameterCollection col)
        {
            col.AddWithValue("@Notes", model.Note);
            col.AddWithValue("@TaskId", model.TaskId);
        }

        private Notes MapSingleNote(IDataReader reader, ref int startingIndex)
        {
            Notes note = new Notes();

            note.Id = reader.GetSafeInt32(startingIndex++);
            note.Note = reader.GetSafeString(startingIndex++);
            note.TaskId = reader.GetSafeInt32(startingIndex++);

            return note;
        }
    }
}

