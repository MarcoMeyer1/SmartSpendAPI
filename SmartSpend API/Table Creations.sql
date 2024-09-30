-- User Table 
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    Surname NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL, 
    PasswordSalt NVARCHAR(255) NOT NULL, 
    PhoneNumber NVARCHAR(15),
    DateRegistered DATETIME DEFAULT GETDATE()
);


-- Expense Table
CREATE TABLE Expenses (
    ExpenseID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    ExpenseName NVARCHAR(255) NOT NULL,
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
    Amount DECIMAL(18, 2) NOT NULL,
    ExpenseDate DATETIME DEFAULT GETDATE()
);

-- Income Table
CREATE TABLE Income (
    IncomeID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    IncomeReference NVARCHAR(255) NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    IncomeDate DATETIME DEFAULT GETDATE()
);

-- Reminder Table
CREATE TABLE Reminders (
    ReminderID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    Description NVARCHAR(255) NOT NULL,
    DateDue DATETIME NOT NULL,
    NotificationDate DATETIME,
    IsEnabled BIT DEFAULT 1
);

-- Goal Table
CREATE TABLE Goals (
    GoalID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    GoalName NVARCHAR(255) NOT NULL,
    TotalAmount DECIMAL(18, 2) NOT NULL,
    SavedAmount DECIMAL(18, 2) DEFAULT 0,
    CompletionDate DATETIME NULL
);

-- Category Table 
CREATE TABLE dbo.Categories (
    CategoryID INT PRIMARY KEY NOT NULL,   
    CategoryName NVARCHAR(255) NOT NULL,  
    ColorCode NVARCHAR(7) NOT NULL,       
    UserID INT NOT NULL,                   
    maxBudget DECIMAL(18, 2) NOT NULL      
);

-- Notifications Table
CREATE TABLE Notifications (
    NotificationID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    NotificationText NVARCHAR(255) NOT NULL,
    NotificationDate DATETIME DEFAULT GETDATE()
);

-- Settings Table
CREATE TABLE Settings (
    SettingID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    AllowNotifications BIT DEFAULT 1,
    BiometricLogin BIT DEFAULT 0,
    Language NVARCHAR(50) DEFAULT 'English'
);

-- Detailed View Table 
CREATE TABLE DetailedView (
    DetailedViewID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
    TotalExpense DECIMAL(18, 2),
    TotalIncome DECIMAL(18, 2),
    MonthYear NVARCHAR(20) NOT NULL 
);
