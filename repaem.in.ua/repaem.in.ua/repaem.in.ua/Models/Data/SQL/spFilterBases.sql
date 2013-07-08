USE [repaem]
GO
/****** Object:  StoredProcedure [dbo].[spGetRepBases]    Script Date: 08.07.2013 17:58:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Anatoliy Stegniy>
-- Create date: <27.06.13>
-- Description:	<Эта процедура возвращает список баз, на которых есть свободное время для заданных параметров>
-- =============================================
ALTER PROCEDURE [dbo].[spGetRepBases]
	 @Name nvarchar(256), @PriceStart int, @PriceEnd int, @CityId int, @DistinctId int, @Date datetime, @TimeStart int, @TimeEnd int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @DateStart datetime;
	SET @DateStart = DATEADD("hh", @TimeStart, @Date);

	DECLARE @DateEnd datetime;
	SET @DateEnd = DATEADD("hh", @TimeEnd, @Date);

	--CREATE TABLE #tmpBases1 (
	--	Id int
	--)

	--CREATE TABLE #tmpBases2 (
	--	Id int, Price int
	--)

	--ищем базы по критерию Город, Район, Название
	--учитываем, что параметры могут отсутствовать, тогда их не учитываем
	SELECT RepBases.Id AS Id
		INTO #tmpBases1 
		FROM RepBases 
		WHERE 
		RepBases.NAME LIKE
			CASE WHEN(LEN(@Name) > 0) THEN '%' + @Name + '%'
			ELSE '' 
			END 
		AND CityId = COALESCE(NULLIF(@CityId,0), CityId) 
		AND DistinctId = COALESCE(NULLIF(@DistinctId,0), DistinctId);
	
	--ищем среди комнат найденных баз свободное время и подходящую цену
	SELECT DISTINCT r.RepBaseId as Id, [repaem].[dbo].[fnGetPrice](r.Id, @TimeStart, @TimeEnd) as iPrice
		INTO #tmpBases2
		FROM Rooms r
		LEFT JOIN Orders o ON o.RoomId = r.Id
		LEFT JOIN Prices p ON p.RoomId = r.Id
		--только для найденных баз
		WHERE 
			r.RepBaseId IN (SELECT * FROM #tmpBases1)
			--ищем пустые промежутки
			AND (o.TimeEnd >= @DateStart AND o.TimeStart <= @DateEnd)
			--если запись в таблице Orders не найдена, значит время свободно
			AND o.Id is NULL
			--ищем подходящую цену
			AND ([repaem].[dbo].[fnGetPrice](r.Id, @TimeStart, @TimeEnd) <= @PriceEnd AND [repaem].[dbo].[fnGetPrice](r.Id, @TimeStart, @TimeEnd) >= @PriceStart)

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
				ph.ThumbnailSrc as ImageSrc,
				rp.Address as Address
	FROM RepBases rp 
	LEFT JOIN PhotoToRepBase phrb ON phrb.RepBaseId = rp.Id
	LEFT JOIN Photos ph ON ph.Id = phrb.PhotoId AND ph.IsLogo = 1
	WHERE rp.Id IN (SELECT #tmpBases2.Id FROM #tmpBases2)
	ORDER BY rp.CreationDate DESC

	DROP TABLE #tmpBases1
	DROP TABLE #tmpBases2
END
