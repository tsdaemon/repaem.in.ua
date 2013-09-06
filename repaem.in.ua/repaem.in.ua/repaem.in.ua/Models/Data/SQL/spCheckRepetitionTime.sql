SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


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

    -- Insert statements for procedure here

	IF EXISTS(SELECT r.Id FROM Repetitions r 
			  WHERE r.RoomId = @RoomId AND r.Date = @Date
				AND ((r.TimeStart > @TimeStart AND r.TimeStart > @TimeEnd)
        OR  (r.TimeEnd > @TimeStart AND r.TimeEnd < @TimeEnd)))
		SELECT cast(0 as bit)
	ELSE 
		SELECT CAST(1 as bit)
END

