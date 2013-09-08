USE [db00757cc4ea1a4c4fbaada1f700fed8fd]
GO

/****** Object:  StoredProcedure [dbo].[spCheckRepetitionTime]    Script Date: 06.09.2013 21:32:00 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
-- ак это будет работать
--ќсобенно касаетс€ посто€нных репетиций
--ƒень недели посто€нки определ€етс€ по первой дате.
--—начала ищем среди обычных реп
--«атем среди посто€нок
--≈сли на посто€нку есть отмененна€ репа от этих чуваков в этот день - значит временна€ отмена, возвращаем тру


CREATE PROCEDURE [dbo].[spCheckRepetitionTime]
	-- Add the parameters for the stored procedure here
	@TimeStart int,
	@TimeEnd int,
	@Date datetime,
	@RoomId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @WeekDay int

	--проверка на обычные
	IF EXISTS(SELECT Id FROM Repetitions 
			  WHERE 
				RoomId = @RoomId 
				AND Date = @Date
				AND ((TimeStart > @TimeStart AND TimeStart > @TimeEnd)
				OR  (TimeEnd > @TimeStart AND TimeEnd < @TimeEnd))
				AND Status = 1)
		SELECT cast(0 as bit)

	SET @WeekDay = datepart(dw, @Date)

	--проверка, есть ли посто€нка
	IF EXISTS(SELECT Id FROM Repetitions
			  WHERE
				RoomId = @RoomId
				AND @WeekDay = datepart(dw, Date)
				AND ((TimeStart > @TimeStart AND TimeStart > @TimeEnd)
				OR  (TimeEnd > @TimeStart AND TimeEnd < @TimeEnd))
				AND Status = 2)
	BEGIN
	--проверка, не отменили ли посто€нку
		IF EXISTS(SELECT Id FROM Repetitions
			  WHERE
				RoomId = @RoomId
				AND Date = @Date
				AND ((TimeStart > @TimeStart AND TimeStart > @TimeEnd)
				OR  (TimeEnd > @TimeStart AND TimeEnd < @TimeEnd))
				AND Status = 3)
			SELECT cast(1 as bit)
		ELSE
			SELECT cast(0 as bit)
	END
	SELECT cast(1 as bit)
END


GO


