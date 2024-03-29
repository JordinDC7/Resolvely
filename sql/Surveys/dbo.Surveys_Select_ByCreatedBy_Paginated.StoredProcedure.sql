USE [Resolvely]
GO
/****** Object:  StoredProcedure [dbo].[Surveys_Select_ByCreatedBy_Paginated]    Script Date: 11/27/2023 12:14:39 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jordin Camp
-- Create date: 11/16/2023
-- Description: Select records by who it was CreatedBy paginated
-- Code Reviewer: Joseph Bryan

-- MODIFIED BY:   JordinCamp
-- MODIFIED DATE: 11/27/2023
-- Code Reviewer: Thane Thompson
-- Note:
-- =============================================
CREATE proc [dbo].[Surveys_Select_ByCreatedBy_Paginated]

			@CreatedBy int
			,@PageIndex int
			,@PageSize int 

as
/*
Declare @CreatedBy int = 8
	   ,@PageIndex int = 0
	   ,@PageSize int = 5
		
Execute dbo.Surveys_Select_ByCreatedBy_Paginated 
		@CreatedBy
		,@PageIndex
		,@PageSize

*/
Begin
DECLARE @Offset int = @PageIndex * @PageSize;

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
     

 FROM dbo.Surveys as s
    INNER JOIN dbo.SurveyStatus st ON s.StatusId = st.Id
    INNER JOIN dbo.SurveyTypes sst ON s.SurveyTypeId = sst.Id
    INNER JOIN dbo.Users u ON s.CreatedBy = u.Id
	Where s.CreatedBy = @CreatedBy
		ORDER BY s.Id 
		OFFSET @Offset ROWS
		FETCH NEXT @PageSize ROWS ONLY;

End


GO
