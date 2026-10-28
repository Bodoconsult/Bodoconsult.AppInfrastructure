-- Ergänze fehlende Prepare-Datesätze
insert into tPagesPrepare(PP_P_ID)
select p_id
from tPages p
where not exists (select * from tPagesPrepare pp where p.p_id=pp.pp_p_id)



-- Seiten ohne Masterpage
update tpagesPrepare set pp_contentfinale=p.p_content
from tPagesPrepare pp, tPages p
where pp.pp_p_id=p.p_id and p_mp_id is null

-- Seiten mit Masterpage, aber ohne Zusatzcontent
update tpagesprepare 
set pp_contentfinale='??Content??'
from tPagesPrepare pp, tPages p
where pp.pp_p_id=p.p_id and  p.p_mp_id is not null and p.p_content is null

-- Seiten mit Masterpage und Zusatzcontent, aber ohne ??Content??-Tag
--update tpages 
--set p_contentfinale=p_content+'??Content??'
--where p_mp_id is not null and patindex('??Content??', p_content)=0

-- Seiten mit Masterpage, Zusatzcontent und ??Content??-Tag
update tpagesprepare 
set pp_contentfinale=p.p_content
from tPagesPrepare pp, tPages p
where pp.pp_p_id=p.p_id and  p.p_mp_id is not null and charindex('??Content??', p.p_content)<>0

-- Felder updaten
update tPagesFields
set pf_contentfinale=pf_string
from tPagesFields pf, tMasterPagesFields mpf
where pf.pf_f_id=mpf.f_id
	and mpf.f_dt_id IN (1, 4, 5)