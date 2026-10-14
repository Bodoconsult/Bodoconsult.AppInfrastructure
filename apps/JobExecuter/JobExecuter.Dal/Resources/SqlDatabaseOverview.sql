select name, COMPATIBILITY_LEVEL, collation_name, state_desc, user_access_desc, is_auto_shrink_on, recovery_model_desc
from sys.databases WHERE name='{0}'