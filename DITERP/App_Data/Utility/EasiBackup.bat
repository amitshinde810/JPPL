sqlcmd -S DITPL-LAP-14 -U sa -P abcd1234 -i EasiBack.sql



 D:\Projects\pcpl_erp\Source\DITERP\App_Data\Utility\7z.exe a D:\Projects\pcpl_erp\Source\DITERP\App_Data\Utility\Backup\SimyaDB_%date%_%time:~0,2%-%time:~3,2%_.zip D:\Projects\pcpl_erp\Source\DITERP\App_Data\Utility\Backup\db_backup_*.bak
del D:\Projects\pcpl_erp\Source\DITERP\App_Data\Utility\Backup\db_backup_*.bak

