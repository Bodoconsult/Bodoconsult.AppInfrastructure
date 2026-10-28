declare @ID int

select @ID=p_p_id 
from tpages
where p_id={0} 

select p_id, isnull(p_nameurl, p_name) as P_Name, P_URL, cast(0 as int) as P_SortID, isnull(P_TitleURL, p_name) as P_TitleURL
from tpages
where p_id=@ID


union

select p_id, isnull(p_nameurl, p_name) as P_Name, P_URL, P_SortID, isnull(P_TitleURL, p_name) as P_TitleURL
from tpages
where p_p_id={0} and {0}<>1


union

select p_id, isnull(p_nameurl, p_name) as P_Name, P_URL, P_SortID, isnull(P_TitleURL, p_name) as P_TitleURL
from tpages p inner join tPagesLinks pl
	on p.p_id=pl.pl_to_id
where pl.pl_from_id={0} and {0}<>1
order by p_sortid



--exec [dbo].[spPagesSubLinks] 6