SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION fnGetPrice
(
	@RoomId int, @TimeStart int, @TimeEnd int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Price int

	SET @Price = (SELECT TOP 1 r.Price FROM Rooms r WHERE r.Id = @RoomId);

    -- Insert statements for procedure here
	IF @Price IS NULL
		SET @Price = (SELECT TOP 1 p.Sum FROM Prices p WHERE p.RoomId = @RoomId AND p.StartTime < @TimeEnd OR p.EndTime > @TimeStart);
	
	RETURN @Price;

END
GO

