USE [Resolvely]
GO
/****** Object:  StoredProcedure [dbo].[Surveys_Delete_ById]    Script Date: 11/21/2023 6:11:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author: Jordin Camp
-- Create date: 11/16/2023
-- Description: Stored procedure that Deletes by a given Id.
-- Code Reviewer: Joseph Bryan

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer: Thane thompson
-- Note:
-- =============================================
CREATE proc [dbo].[Surveys_Delete_ById]
			@Id int

as
/*

Declare @Id int = 51

Select  [Id],
		[Name],
		[Description],
		[StatusId],
		[SurveyTypeId],
		[TaskId],
		[CreatedBy],
		[DateCreated],
		[DateModified]

from dbo.Surveys
Where @Id = Id

Execute dbo.Surveys_Delete_ById @Id

Select  [Id],
		[Name],
		[Description],
		[StatusId],
		[SurveyTypeId],
		[TaskId],
		[CreatedBy],
		[DateCreated],
		[DateModified]

from dbo.Surveys
Where @Id = Id
*/
Begin


DELETE FROM [dbo].[Surveys]
      WHERE @Id = Id


End
GO
