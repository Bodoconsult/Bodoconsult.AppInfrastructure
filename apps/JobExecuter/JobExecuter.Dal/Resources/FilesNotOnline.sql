SELECT df.name, 
	df.size/128.0 FileSizeInMB,
	df.size/128.0 - CAST(FILEPROPERTY(df.name, 'SpaceUsed') AS int)/128.0 AS EmptySpaceInMB,
	df.type_desc as [File type],
	df.state_desc as [State],
	df.physical_name as [Physical path]
FROM sys.master_files as df
	INNER JOIN sys.databases db ON
	df.database_id=db.database_id 
WHERE db.database_id>4
	AND df.state<>0
	ORDER BY df.Name, df.type_desc