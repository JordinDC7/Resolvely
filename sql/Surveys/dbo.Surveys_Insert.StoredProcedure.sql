USE [Resolvely]
GO
/****** Object:  StoredProcedure [dbo].[Surveys_Insert]    Script Date: 1/9/2024 2:10:51 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jordin Camp
-- Create date: 11/16/2023
-- Description:	Insert a record into the Surveys Table
-- Code Reviewer: Joseph Bryan

-- MODIFIED BY: Jordin Camp
-- MODIFIED DATE: 1/8/2024
-- Code Reviewer: Thane Thompson
-- Note: added taskId int = NULL to handle NULL value.
-- =============================================
CREATE proc [dbo].[Surveys_Insert]
			@Name nvarchar(100)
			,@Description nvarchar(2000)
			,@StatusId int
			,@SurveyTypeId int
			,@TaskId int = NULL
			,@CreatedBy int
			,@Id int OUTPUT


as
/*
Declare
			@Name nvarchar(100) = 'Customer Satisfaction'
			,@Description nvarchar(2000) = 'Customer Satisfaction Survey'
			,@StatusId int = 3
			,@SurveyTypeId int = 1
			,@TaskId int = 1
			,@CreatedBy int = 2
			,@Id int = 0;

Execute dbo.Surveys_INSERT
			@Name
			,@Description
			,@StatusId
			,@SurveyTypeId 
			,@TaskId 
			,@CreatedBy
			,@Id OUTPUT

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
Declare @DateCreated datetime2 = GETUTCDATE()
		,@DateModified datetime2 = GETUTCDATE()
INSERT INTO dbo.Surveys
			([Name]
			,[Description]
			,[StatusId]
			,[SurveyTypeId]
			,[TaskId]
			,[CreatedBy]
			,[DateCreated]
			,[DateModified])
		VALUES
			(@Name
			,@Description
			,@StatusId
			,@SurveyTypeId 
			,@TaskId 
			,@CreatedBy
			,@DateCreated
			,@DateModified)

		SET @Id = SCOPE_IDENTITY()

End
GO
