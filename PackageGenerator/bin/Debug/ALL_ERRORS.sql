select * from all_errors
 where owner = 'LF'
   and text not in ('PL/SQL: SQL Statement ignored','PL/SQL: Item ignored','PL/SQL: Statement ignored')
   and text not like 'PLS-00341%'
   and text not like 'PLS-00320%'
   and text not like 'PLS-00905%'
   and text not like 'PLS-00364%'
 order by type, name, sequence, line
/
