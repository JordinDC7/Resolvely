USE [Resolvely]
GO
/****** Object:  StoredProcedure [dbo].[StatusTypes_SelectAll]    Script Date: 11/20/2023 7:04:56 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Jordin Camp
-- Create date: 11/16/2023
-- Description:	Select all the Status Types from the StatusTypes Table.
-- Code Reviewer: Joseph Bryan

-- MODIFIED BY: 
-- MODIFIED DATE:
-- Code Reviewer:
-- Note:
-- =============================================

CREATE proc [dbo].[StatusTypes_SelectAll]
	
	

as


/*

Execute [dbo].StatusTypes_SelectAll

*/
Begin

Select [Id]
	   ,[Name]

	From dbo.StatusTypes

End
GO
