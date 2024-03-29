USE [Resolvely]
GO
/****** Object:  StoredProcedure [dbo].[Surveys_SelectAll_Paginated]    Script Date: 12/15/2023 11:14:48 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jordin Camp
-- Create date: 11/16/2023
-- Description: Select all records paginated
-- Code Reviewer: Mark Martinez

-- MODIFIED BY:   JordinCamp
-- MODIFIED DATE: 12/15/2023
-- Code Reviewer: Mark Martinez
-- Note:
-- =============================================
CREATE proc [dbo].[Surveys_SelectAll_Paginated]
		
			@PageIndex int 
			,@PageSize int 		
			,@StatusId int = NULL
			,@Excluded bit = 0
as
/*
Declare @PageIndex int = 0
		,@PageSize int = 10
		,@StatusId int = 0
		,@Excluded bit = 0
Execute dbo.Surveys_SelectAll_Paginated
		@PageIndex
		,@PageSize
		,@StatusId
		,@Excluded

*/
Begin
Declare @Offset int  = @PageIndex * @PageSize

SELECT	s.Id,
        s.[Name],
        s.[Description],
        s.TaskId,
        dbo.fn_GetUserJSON(s.CreatedBy) as CreatedBy,
        s.DateCreated,
        s.DateModified,
        st.Id as StatusId,
        st.[Name] as [Status],
        sst.Id as [SurveyTypeId],
        sst.[Name] as [SurveyType],
		TotalCount = COUNT(1) OVER()

From dbo.Surveys as s
  INNER JOIN dbo.SurveyStatus st ON s.StatusId = st.Id
  INNER JOIN dbo.SurveyTypes sst ON s.SurveyTypeId = sst.Id
  INNER JOIN dbo.Users u ON s.CreatedBy = u.Id

  --WHERE NOT s.StatusId = 2 
  --WHERE (@StatusIdFilter IS NULL OR s.StatusId = @StatusIdFilter)
  WHERE 
	(@Excluded = 0 AND s.StatusId <> 2)
    OR (@Excluded = 0 AND s.StatusId = @StatusId) 
    OR (@Excluded = 1 AND s.StatusId <> @StatusId)

		ORDER BY s.Id 
		OFFSET @Offset ROWS
		FETCH NEXT @PageSize ROWS ONLY;


End


GO
