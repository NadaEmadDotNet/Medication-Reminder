UPDATE AspNetUsers
SET CreatedAt = GETDATE()
WHERE CreatedAt IS NULL OR CreatedAt = '0001-01-01'