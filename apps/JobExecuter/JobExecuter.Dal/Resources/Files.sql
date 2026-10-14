SELECT
    db.name AS [Database name],
    type_desc AS [File type],
    Physical_Name AS Location,
	Size*8/1024 as [Size MB],
	mf.state_desc as [File state],
	db.state_desc as [DB state]
FROM
    sys.master_files mf
INNER JOIN 
    sys.databases db ON db.database_id = mf.database_id
where db.name='{0}'