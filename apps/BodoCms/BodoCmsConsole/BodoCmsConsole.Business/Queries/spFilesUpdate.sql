declare '{1}'A datetime

set '{1}'A = getdate()

if exists (select * from tfiles where f_name='{0}')
begin
		update tFiles set F_exists=1, f_date='{1}'
		where f_name='{0}' and f_date<'{1}'

		if @@rowcount=0
		begin
			update tFiles set F_exists=1
			where f_name='{0}'
		end

end
else
begin
		insert into tFiles(f_name, f_exists,  f_date, f_typ)
		select '{0}', 1, '{1}'A, '{2}'

end