USE [Resolvely]
GO
/****** Object:  StoredProcedure [dbo].[Surveys_Update]    Script Date: 11/21/2023 6:11:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jordin Camp
-- Create date: 11/16/2023
-- Description:	Update a record in the Surveys Table by Id
-- Code Reviewer: Joseph Bryan

-- MODIFIED BY: 
-- MODIFIED DATE: 11/21/2023
-- Code Reviewer: Thane thompson
-- Note:
-- =============================================
CREATE proc [dbo].[Surveys_Update]
			@Name nvarchar(100)
			,@Description nvarchar(2000)
			 ,@StatusId int
			 ,@surveyTypeId int
			 ,@taskId int
			,@Id int 


as
/*
Declare		
			@Name nvarchar(100) = 'Customer Satisfaction'
			,@Description nvarchar(2000) = 'Customer Satisfaction Survey'
			,@StatusId int = 1
			,@SurveyTypeId int = 1
			,@TaskId int = 1
			,@Id int = 3

Execute dbo.Surveys_Update
			@Name
			,@Description
			,@StatusId
			,@SurveyTypeId 
			,@TaskId 
			,@Id 

Select	[Id]
	   ,[Name]
	   ,[Description]
	   ,[StatusId]
	   ,[SurveyTypeId]
	   ,[TaskId]
	   ,[CreatedBy]
	   ,[DateCreated]
	   ,[DateModified]

FROM dbo.Surveys
WHERE Id = @Id


*/


Begin
Declare @DateModified datetime2 = GETUTCDATE()

Update dbo.Surveys
		Set	[Name] = @Name
			,[Description] = @Description
			,StatusId = @StatusId
			,SurveyTypeId = @surveyTypeId
			,TaskId = @taskId
			,[DateModified] = @DateModified
	
	Where Id = @Id

End
GO
