declare @ID int, @Z nvarchar(2000)

DECLARE UC CURSOR FOR
SELECT P_ID 
from tpages
where p_type='P'

OPEN UC

FETCH NEXT FROM UC
INTO @ID

WHILE (@@FETCH_STATUS = 0)
BEGIN
	exec dbo.spPagesReplaceContent @ID, '{0}', '{1}'

	update tPagesFields 
	set pf_contentfinale=replace(pf_contentfinale, '{0}', '{1}')

    FETCH NEXT FROM UC
    INTO @ID
END

CLOSE UC

DEALLOCATE UC