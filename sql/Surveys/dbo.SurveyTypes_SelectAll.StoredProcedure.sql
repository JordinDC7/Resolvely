USE [Resolvely]
GO
/****** Object:  StoredProcedure [dbo].[SurveyTypes_SelectAll]    Script Date: 11/21/2023 6:11:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jordin Camp
-- Create date: 11/16/2023
-- Description:	Select all from the SurveyTypes table
-- Code Reviewer: Joseph Bryan

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
CREATE proc [dbo].[SurveyTypes_SelectAll]
	
	

as


/*

Execute dbo.SurveyTypes_SelectAll

*/
Begin

Select [Id]
	   ,[Name]

	From dbo.SurveyTypes

End
GO
