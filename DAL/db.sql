-- 1. Reset database
USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'CafeteriaDb')
BEGIN
    ALTER DATABASE CafeteriaDb SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE CafeteriaDb;
END
GO

CREATE DATABASE CafeteriaDb;
GO
USE CafeteriaDb;
GO

-- 2. Create Tables

CREATE TABLE UserTypes (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL UNIQUE -- 'Admin', 'Customer'
);

CREATE TABLE Users (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    IdCardNo VARCHAR(50) NOT NULL UNIQUE, 
    Email VARCHAR(100) NOT NULL UNIQUE,
    Password VARCHAR(255) NOT NULL,
    UserTypeId INT NOT NULL, 
    WalletBalance FLOAT NOT NULL DEFAULT 0.0,
    ProfileStatus VARCHAR(500) NULL,
    CONSTRAINT FK_Users_UserTypes FOREIGN KEY (UserTypeId) REFERENCES UserTypes(Id)
);

CREATE TABLE MenuItems (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ItemName VARCHAR(100) NOT NULL,
    Price FLOAT NOT NULL,
    Category VARCHAR(50) NOT NULL, 
    AvailableQuantity INT NOT NULL DEFAULT 0
);

-- এখানে OrderedQuantity এবং TotalPrice কলাম দুটি যোগ করা হয়েছে
CREATE TABLE MealBookings (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    MenuItemId INT NOT NULL,
    OrderedQuantity INT NOT NULL DEFAULT 1, -- কয়টি মিল অর্ডার করা হয়েছে
    TotalPrice FLOAT NOT NULL,              -- (Price * OrderedQuantity) এর মোট হিসাব
    BookingDate DATETIME NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    Status VARCHAR(20) NOT NULL, 
    CONSTRAINT FK_MealBookings_Users FOREIGN KEY (UserId) REFERENCES Users(Id),
    CONSTRAINT FK_MealBookings_MenuItems FOREIGN KEY (MenuItemId) REFERENCES MenuItems(Id)
);

CREATE TABLE WalletTransactions (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL, 
    TransactionType VARCHAR(50) NOT NULL, 
    TransactionDate DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_WalletTransactions_Users FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE
);

CREATE TABLE SystemLogs (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    Message TEXT NOT NULL,
    CreatedAt DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_SystemLogs_Users FOREIGN KEY (UserId) REFERENCES Users(Id)
);
GO

-- 3. Insert Sample Data

INSERT INTO UserTypes (Name) VALUES 
('Admin'),      -- Id: 1
('Customer');   -- Id: 2

--INSERT INTO Users (Name, IdCardNo, Email, Password, UserTypeId, WalletBalance, ProfileStatus) VALUES
--('Anik', '23-54110-3', 'anik@uni.edu', 'pass123', 2, 550.0, 'Loving the tech life!'),
--('Sajid', '24-11223-2', 'sajid@uni.edu', 'pass123', 2, 70.0, 'Foodie | CSE Student'),
--('Rifat', '22-33445-1', 'rifat@uni.edu', 'pass123', 2, 1200.0, 'Code, Eat, Sleep, Repeat.'),
--('Nadia', '21-99887-3', 'nadia@uni.edu', 'pass123', 2, 340.0, 'Always hungry for knowledge.'),
--('Admin', '1-11111-1', 'admin@cafeteria.com', 'admin123', 1, 0.0, 'Cafeteria Administrator');

INSERT INTO Users (Name, IdCardNo, Email, Password, UserTypeId, WalletBalance, ProfileStatus) VALUES
('Anik', '23-54110-3', 'anik@uni.edu', '32250170a0dca92d53ec9624f336ca24', 2, 550.0, 'Loving the tech life!'),
('Sajid', '24-11223-2', 'sajid@uni.edu', '32250170a0dca92d53ec9624f336ca24', 2, 70.0, 'Foodie | CSE Student'),
('Rifat', '22-33445-1', 'rifat@uni.edu', '32250170a0dca92d53ec9624f336ca24', 2, 1200.0, 'Code, Eat, Sleep, Repeat.'),
('Nadia', '21-99887-3', 'nadia@uni.edu', '32250170a0dca92d53ec9624f336ca24', 2, 340.0, 'Always hungry for knowledge.'),
('Admin', '1-11111-1', 'admin@cafeteria.com', '0192023a7bbd73250516f069df18b500', 1, 0.0, 'Cafeteria Administrator');

INSERT INTO MenuItems (ItemName, Price, Category, AvailableQuantity) VALUES
('Khichuri', 60.0, 'Breakfast', 50),
('Biryani', 120.0, 'Lunch', 100),
('Rice', 50.0, 'Lunch', 150),
('Samosa', 20.0, 'Snacks', 80),
('Burger', 80.0, 'Snacks', 40);

-- নতুন কলামের ডেটা স্ট্রাকচার মিলিয়ে স্যাম্পল ডেটা অ্যাডজাস্ট করা হলো
INSERT INTO MealBookings (UserId, MenuItemId, OrderedQuantity, TotalPrice, BookingDate, Status) VALUES
(1, 2, 1, 120.0, GETDATE(), 'Booked'),       -- ১টি বিরিয়ানি = ১২০ টাকা
(2, 1, 2, 120.0, GETDATE(), 'Booked'),       -- ২টি খিচুড়ি = ১২০ টাকা
(3, 2, 1, 120.0, GETDATE(), 'Cancelled'),    -- ১টি বিরিয়ানি = ১২০ টাকা
(4, 3, 3, 150.0, GETDATE(), 'Booked'),       -- ৩টি ভাত = ১৫০ টাকা
(1, 4, 5, 100.0, GETDATE(), 'Booked');       -- ৫টি সমুচা = ১০০ টাকা

INSERT INTO WalletTransactions (UserId, Amount, TransactionType) VALUES
(1, 600.00, 'Deposit'),
(1, -50.00, 'Meal_Purchase'),
(2, 100.00, 'Deposit'),
(3, 1200.00, 'Deposit'),
(3, 120.00, 'Refund');

INSERT INTO SystemLogs (UserId, Message) VALUES
(2, 'Your balance is below 100 TK. Please recharge.'),
(3, 'Your meal booking has been cancelled and refunded.'),
(1, 'Welcome to University Cafeteria System!'),
(4, 'Your wallet has been credited with 340 TK.'),
(5, 'System initialized successfully by Admin.');
GO


