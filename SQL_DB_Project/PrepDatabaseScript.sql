IF EXISTS(SELECT * FROM sys.tables WHERE name = 'Country')
BEGIN
    DROP TABLE [dbo].[Country];
END