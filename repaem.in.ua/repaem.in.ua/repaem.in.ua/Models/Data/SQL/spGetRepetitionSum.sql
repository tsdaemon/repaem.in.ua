SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE spGetRepetitionSum 
	-- Add the parameters for the stored procedure here
	@RoomId int, 
	@TimeStart int,
	@TimeEnd int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @HourPrice int
	DECLARE @Price int

	SET @HourPrice = (SELECT [dbo].[fnGetPrice](@RoomId, @TimeStart, @TimeEnd))
	SET @Price = @HourPrice * (@TimeEnd - @TimeStart)

	IF @Price IS NULL
		SET @Price = 0
	SELECT @Price
END
GO
