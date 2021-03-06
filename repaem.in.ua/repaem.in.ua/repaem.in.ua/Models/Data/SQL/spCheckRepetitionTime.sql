USE [db00757cc4ea1a4c4fbaada1f700fed8fd]
GO
/****** Object:  StoredProcedure [dbo].[spCheckRepetitionTime]    Script Date: 10/24/2013 2:41:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--Как это будет работать
--Особенно касается постоянных репетиций
--День недели постоянки определяется по первой дате.
--Сначала ищем среди обычных реп
--Затем среди постоянок
--Если на постоянку есть отмененная репа от этих чуваков в этот день - значит временная отмена, возвращаем тру


CREATE PROCEDURE [dbo].[spCheckRepetitionTime]
	-- Add the parameters for the stored procedure here
	@TimeStart int,
	@TimeEnd int,
	@Date datetime,
	@RoomId int,
  @RepetitionId int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	--проверка на обычные
	IF EXISTS(SELECT Id FROM Repetitions 
			  WHERE 
				RoomId = @RoomId 
        AND Id != @RepetitionId
				AND Date = @Date
				AND ((TimeStart >= @TimeStart AND TimeStart < @TimeEnd) OR  (TimeEnd > @TimeStart AND TimeEnd <= @TimeEnd))
				AND Status = 1)
		RETURN cast(0 as bit)

  --DECLARE @WeekDay int
	--SET @WeekDay = datepart(dw, @Date)

	--проверка, есть ли постоянка
	--IF EXISTS(SELECT Id FROM Repetitions
	--		  WHERE
	--			RoomId = @RoomId
	--			AND @WeekDay = datepart(dw, Date)
	--			AND ((TimeStart > @TimeStart AND TimeStart > @TimeEnd)
	--			OR  (TimeEnd > @TimeStart AND TimeEnd < @TimeEnd))
	--			AND Status = 2)
	--BEGIN
	----проверка, не отменили ли постоянку
	--	IF EXISTS(SELECT Id FROM Repetitions
	--		  WHERE
	--			RoomId = @RoomId
	--			AND Date = @Date
	--			AND ((TimeStart > @TimeStart AND TimeStart > @TimeEnd)
	--			OR  (TimeEnd > @TimeStart AND TimeEnd < @TimeEnd))
	--			AND Status = 3)
	--		SELECT cast(1 as bit)
	--	ELSE
	--		SELECT cast(0 as bit)
	--END
	RETURN cast(1 as bit)
END



