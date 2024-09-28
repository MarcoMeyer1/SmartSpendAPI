-- User Table with Salt Column
CREATE TABLE Users (
    UserID INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(100) NOT NULL,
    Surname NVARCHAR(100) NOT NULL,
    Email NVARCHAR(255) UNIQUE NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL, -- This stores the hashed password
    PasswordSalt NVARCHAR(255) NOT NULL, -- This stores the salt for hashing
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

-- Category Table with ColorCode and UserID
CREATE TABLE dbo.Categories (
    CategoryID INT PRIMARY KEY NOT NULL,   -- Primary key for the categories
    CategoryName NVARCHAR(255) NOT NULL,   -- Category name (e.g., Groceries, Rent)
    ColorCode NVARCHAR(7) NOT NULL,        -- Color code for UI representation (e.g., #FFFFFF)
    UserID INT NOT NULL,                   -- Reference to the user who created the category
    maxBudget DECIMAL(18, 2) NOT NULL      -- Max budget for the category
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

-- Detailed View Table (for managing category expenses and income)
CREATE TABLE DetailedView (
    DetailedViewID INT IDENTITY(1,1) PRIMARY KEY,
    UserID INT FOREIGN KEY REFERENCES Users(UserID),
    CategoryID INT FOREIGN KEY REFERENCES Categories(CategoryID),
    TotalExpense DECIMAL(18, 2),
    TotalIncome DECIMAL(18, 2),
    MonthYear NVARCHAR(20) NOT NULL -- E.g., "August 2024"
);
