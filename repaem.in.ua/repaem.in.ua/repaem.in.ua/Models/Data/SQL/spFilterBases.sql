USE [db00757cc4ea1a4c4fbaada1f700fed8fd]
GO
/****** Object:  StoredProcedure [dbo].[spGetRepBases]    Script Date: 13.09.2013 21:26:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



-- =============================================
-- Author:		<Anatoliy Stegniy>
-- Create date: <27.06.13>
-- Description:	<Ёта процедура возвращает список баз, на которых есть свободное врем€ дл€ заданных параметров>
-- =============================================
CREATE PROCEDURE [dbo].[spGetRepBases]
	 @Name nvarchar(256), @PriceStart int, @PriceEnd int, @CityId int, @Date datetime, @TimeStart int, @TimeEnd int
AS
BEGIN
	SET NOCOUNT ON;

	--ищем базы по критерию √ород, –айон, Ќазвание
	--учитываем, что параметры могут отсутствовать, тогда их не учитываем
	SELECT RepBases.Id AS Id
		INTO #tmpBases1 
		FROM RepBases 
		WHERE 
		RepBases.NAME LIKE
			CASE WHEN(LEN(@Name) > 0) THEN '%' + @Name + '%'
			ELSE '%%' 
			END 
		AND CityId = COALESCE(NULLIF(@CityId,0), CityId);
	
	--ищем среди комнат найденных баз свободное врем€ и подход€щую цену
	SELECT DISTINCT r.RepBaseId as Id, dbo.fnGetPrice(r.Id, @TimeStart, @TimeEnd) as iPrice
		INTO #tmpBases2
		FROM Rooms r
		LEFT JOIN Repetitions o ON o.RoomId = r.Id 
			--присоедин€ем только те, что подход€т по времени
			AND (o.TimeEnd <= @TimeStart OR o.TimeStart >= @TimeEnd)
			AND o.Date = @Date
		--только дл€ найденных баз
		WHERE 
			r.RepBaseId IN (SELECT * FROM #tmpBases1)
			--если запись в таблице Orders не найдена, значит врем€ свободно
			AND o.Id is NULL
			--ищем подход€щую цену
			AND (dbo.fnGetPrice(r.Id, @TimeStart, @TimeEnd) <= @PriceEnd AND dbo.fnGetPrice(r.Id, @TimeStart, @TimeEnd) >= @PriceStart)

	--теперь выбираем эти базы так, как нам удобно
	SELECT rp.Id as Id, 
		rp.Name as Name, 
		CAST(rp.Description as nvarchar(256)) + '...'  as Description,
		(SELECT CONVERT(nvarchar(50), AVG(cm.Rating))
				FROM Comments cm 
				WHERE cm.RepBaseId = rp.Id 
				GROUP BY cm.RepBaseId) as Rating,
		(SELECT COUNT(cm.RepBaseId) FROM Comments cm 
				WHERE cm.RepBaseId = rp.Id 
				GROUP BY cm.RepBaseId) as RatingCount,
		(SELECT ph.ThumbnailSrc FROM PhotoToRepBase phrb 
		INNER JOIN Photos ph ON ph.Id = phrb.PhotoId
		WHERE phrb.RepBaseId = rp.Id AND ph.IsLogo = 1) as ImageSrc,
		rp.Address as Address
	FROM RepBases rp 
	WHERE rp.Id IN (SELECT Id FROM #tmpBases2)

	DROP TABLE #tmpBases1
	DROP TABLE #tmpBases2
END



