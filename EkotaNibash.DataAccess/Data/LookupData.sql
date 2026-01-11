INSERT INTO [Users] (
    [Name],
    [UserName],
    [Password],
    [Email],
    [UserRole],
    [IsDisable]
)
VALUES (
    'Super Admin',
    'SuperAdmin',
    'JxNGvi6if82fQBQEcrk+LpX5D5NpBRmBy2od3cp8SOU=', 
    'admin@example.com',
    '1',
    0
);

-- SuperAdmin
--Super@dmin


INSERT INTO CompanyInfos (
    Name,
    Phone,
    WhatsApp,
    WebSiteLink,
    Email,
    HeadOfficeAddress,
    SpecialNotice,
    FacebookPage,
    Logo
)
VALUES (
    'Ekota Nibash.',
    '0123456789',
    '0123456789',
    'https://www.boniyadi.com',
    'info@boniyadi.com',
    'Main Office Address',
    'Welcome to Boniyadi!',
    'https://facebook.com/boniyadi',
    NULL
);
GO

UPDATE CompanyInfos
SET Name = 'Ekota Nibash'
WHERE Id = 1;

GO

-- Insert only if not exists (for safety)
IF NOT EXISTS (SELECT 1 FROM DocumentType WHERE Id = 1)
    INSERT INTO DocumentType (Id, Name, Description) VALUES (1, 'ID Card', 'ID Card');

IF NOT EXISTS (SELECT 1 FROM DocumentType WHERE Id = 2)
    INSERT INTO DocumentType (Id, Name, Description) VALUES (2, 'Photo', 'Photo');

IF NOT EXISTS (SELECT 1 FROM DocumentType WHERE Id = 3)
    INSERT INTO DocumentType (Id, Name, Description) VALUES (3, 'Passport', 'Passport');

IF NOT EXISTS (SELECT 1 FROM DocumentType WHERE Id = 4)
    INSERT INTO DocumentType (Id, Name, Description) VALUES (4, 'Birth Certificate', 'Birth Certificate');

IF NOT EXISTS (SELECT 1 FROM DocumentType WHERE Id = 5)
    INSERT INTO DocumentType (Id, Name, Description) VALUES (5, 'Educational Certificate', 'Educational Certificate');

IF NOT EXISTS (SELECT 1 FROM DocumentType WHERE Id = 6)
    INSERT INTO DocumentType (Id, Name, Description) VALUES (6, 'Payment Invoice', 'Payment Invoice');

IF NOT EXISTS (SELECT 1 FROM DocumentType WHERE Id = 7)
    INSERT INTO DocumentType (Id, Name, Description) VALUES (7, 'Other', 'Other');

    -- Insert Expense Types



INSERT INTO ExpenseType (Id, Name, Description, IsDeleted)
VALUES
(1, 'Advertisement/Notice', 'Cost for publishing notices or ads in newspaper', 0),
(2, 'Property Registration', 'Legal fees and stamp duty for land/property registration', 0),
(3, 'Documentation & Legal', 'Lawyer fees, deed preparation, verification costs', 0),
(4, 'Land Surveyor Fee', 'Payment for surveying and marking the purchased land', 0),
(5, 'Meeting Food & Snacks', 'Cost of snacks, tea, meals during group meetings', 0),
(6, 'Meeting Venue Rent', 'Rent for a space if meetings are held outside', 0),
(7, 'Travel & Transportation', 'Fuel or travel costs for site visits or official work', 0),
(8, 'Bank Charges', 'Account maintenance, transaction fees, cheque book costs', 0),
(9, 'Office Supplies', 'Notebook, pen, files, receipts book, printing etc.', 0),
(10, 'Miscellaneous Expenses', 'Any other small unforeseen expenses', 0),
(11, 'Property Tax / Holding Tax', 'Annual tax payment for owned properties', 0),
(12, 'Site Security', 'Cost for guarding vacant land/property', 0),
(13, 'Festive Gathering', 'Special meal or event during festivals', 0),
(14, 'Emergency Fund Use', 'Expense from emergency fund for urgent needs', 0);

--DELETE FROM Customers;
--DBCC CHECKIDENT ('Customers', RESEED, 0);

--UPDATE Products
--SET StockQty = 0;

-- --These tables likely have foreign key constraints, so use DELETE + RESEED
--DELETE FROM TransactionHistories;
--DBCC CHECKIDENT ('TransactionHistories', RESEED, 0);

