USE [Resolvely]
GO
/****** Object:  StoredProcedure [dbo].[Surveys_Select_ById]    Script Date: 11/28/2023 9:27:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jordin Camp
-- Create date: 11/16/2023
-- Description:	Select a Survey by Id and also returns the createdBy as a JSON Object.
-- Code Reviewer: Joseph Bryan

-- MODIFIED BY:   Jordin Camp
-- MODIFIED DATE: 11/21/2023
-- Code Reviewer: Than thompson
-- Note:
-- =============================================
CREATE proc [dbo].[Surveys_Select_ById]

			@Id int

as
/*
Declare @Id int = 54
Execute dbo.Surveys_Select_ById @Id

*/
Begin


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
    WHERE s.Id = @Id



End


GO
