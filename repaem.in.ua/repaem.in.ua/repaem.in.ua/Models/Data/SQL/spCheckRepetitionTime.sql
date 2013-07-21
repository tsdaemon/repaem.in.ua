
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE spCheckRepetitionTime
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

    -- Insert statements for procedure here
	DECLARE @DateStart datetime;
	SET @DateStart = DATEADD("hh", @TimeStart, @Date);

	DECLARE @DateEnd datetime;
	SET @DateEnd = DATEADD("hh", @TimeEnd, @Date);

	IF EXISTS(SELECT r.Id FROM Repetitions r 
			  WHERE r.RoomId = @RoomId 
				AND (r.TimeStart < @DateEnd OR r.TimeEnd > @DateStart))
		SELECT cast(0 as bit)
	ELSE 
		SELECT CAST(1 as bit)
END
GO
