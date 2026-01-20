DECLARE  @pathName NVARCHAR(512) 
SET @pathName = 'D:\Projects\pcpl_erp\Source\DITERP\App_Data\Utility\Backup\db_backup_' + Convert(varchar(8), GETDATE(), 112) + '.bak' 
BACKUP DATABASE [PCPL_LIVE] TO  DISK = @pathName WITH NOFORMAT, NOINIT, NAME = N'db_backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10
