select name+' ('+product+' '+cast(serverproperty('productversion') as nvarchar(800))+' '+
cast(serverproperty('productlevel') as nvarchar(800))+' '+cast(serverproperty('edition') as nvarchar(800))+' '+ provider+')' 
from sys.servers where server_id = 0