declare @T nvarchar(4000), @PID int

DECLARE UCX CURSOR FOR
SELECT p_ID
FROM tPages
WHERE P_MP_ID is not null

OPEN UCX

FETCH NEXT FROM UCX
INTO @PID

WHILE (@@FETCH_STATUS = 0)
BEGIN

	EXEC dbo.spPagesFieldsFormat @PID, @T out

	declare @Was varchar(255)
	set @Was = '??Content??'
	declare @Text varchar(5000)
	set @Text = @T
	DECLARE @PtrVar BINARY(16)
	DECLARE @InsertPos INT
	DECLARE @DeleteLen INT

	while exists (select * FROM tPagesPrepare
		WHERE PP_P_ID = @PID and PATINDEX('%'+@Was+'%', PP_ContentFinale)>0)
	begin

		SELECT @PtrVar = TEXTPTR(PP_ContentFinale),
			   @InsertPos = (PATINDEX('%'+@Was+'%', PP_ContentFinale)-1),
			   @DeleteLen = ( len(@Was) )
		                    
		FROM tPagesPrepare
		WHERE PP_P_ID = @PID

		UPDATETEXT tPagesPrepare.PP_ContentFinale
				   @PtrVar
				   @InsertPos
				   @DeleteLen
					@Text

	end

    FETCH NEXT FROM UCX
    INTO @PID
END

CLOSE UCX

DEALLOCATE UCX