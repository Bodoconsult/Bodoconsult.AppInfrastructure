
select p_id, isnull(p_sitemapkey, isnull(p_nameurl, p_name)) as P_Name, 
	P_URL , isnull(P_TitleURL, p_name) as P_TitleURL
from tPages
where p_sitemap=1

UNION

select 0, isnull(D_NameURL, D_Name) as P_Name, D_Name+'/index.htm' as P_URL
	, isnull(D_NameURL, D_Name) as P_TitleURL
from tDirectories
where d_freigabe=1

order by P_name