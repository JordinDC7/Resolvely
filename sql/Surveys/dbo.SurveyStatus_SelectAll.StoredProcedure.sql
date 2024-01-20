USE [Resolvely]
GO
/****** Object:  StoredProcedure [dbo].[SurveyStatus_SelectAll]    Script Date: 11/21/2023 6:11:48 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jordin Camp
-- Create date: 11/16/2023
-- Description:	Insert a status type into the SurveyStatus Table.
-- Code Reviewer: Joseph Bryan

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================
CREATE proc [dbo].[SurveyStatus_SelectAll]
	
	
as


/*

Execute dbo.SurveyStatus_SelectAll

*/
Begin

Select [Id]
	   ,[Name]

	From dbo.SurveyStatus

End
GO
