-- Database creation
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'FinanceManagementApplication')
CREATE DATABASE [FinanceManagementApplication];
GO
USE [FinanceManagementApplication];
GO

-- Reset Database: Drop constraints and tables
DECLARE @Sql NVARCHAR(500);
DECLARE @Cursor CURSOR;

SET @Cursor = CURSOR FAST_FORWARD FOR
SELECT DISTINCT sql = 'ALTER TABLE [' + tc2.TABLE_SCHEMA + '].[' +  tc2.TABLE_NAME + '] DROP [' + rc1.CONSTRAINT_NAME + '];'
FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS rc1
LEFT JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS tc2 ON tc2.CONSTRAINT_NAME =rc1.CONSTRAINT_NAME;

OPEN @Cursor;
FETCH NEXT FROM @Cursor INTO @Sql;

WHILE (@@FETCH_STATUS = 0)
BEGIN
    EXEC sp_executesql @Sql;
    FETCH NEXT FROM @Cursor INTO @Sql;
END

CLOSE @Cursor;
DEALLOCATE @Cursor;
GO

DECLARE @tableName NVARCHAR(MAX);
DECLARE tableCursor CURSOR FOR
SELECT name FROM sys.tables;

OPEN tableCursor;
FETCH NEXT FROM tableCursor INTO @tableName;

WHILE @@FETCH_STATUS = 0
BEGIN
    DECLARE @sql NVARCHAR(MAX);
    SET @sql = N'DROP TABLE ' + QUOTENAME(@tableName);
    EXEC sp_executesql @sql;
    FETCH NEXT FROM tableCursor INTO @tableName;
END

CLOSE tableCursor;
DEALLOCATE tableCursor;
GO

-- Table Definitions
CREATE TABLE [User] (
    [id] INT IDENTITY(1, 1) PRIMARY KEY,
    [username] NVARCHAR(50),
    [email] NVARCHAR(50),
    [password] VARCHAR(32) NOT NULL,
    [balance] INT DEFAULT(0),
    [avatarPath] NVARCHAR(MAX)
);

CREATE TABLE [FinanceRecord] (
    [userId] INT,
    [totalIncome] INT,
    [totalExpense] INT,
    [month] INT,
    [year] INT,
    CONSTRAINT PK_FinanceRecord PRIMARY KEY ([userId], [month], [year]),
    CONSTRAINT FK_FinanceRecord FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE
);

CREATE TABLE [IncomeSource] (
    [id] INT IDENTITY(1, 1) PRIMARY KEY,
    [sourceName] NVARCHAR(50),
    [isDelete] BIT DEFAULT(0)
);

CREATE TABLE [IncomeTransaction] (
    [userId] INT,
    [sourceId] INT,
    [amount] INT,
    [date] DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_IncomeTransaction_User FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE,
    CONSTRAINT FK_IncomeTransaction_IncomeSource FOREIGN KEY ([sourceId]) REFERENCES [IncomeSource]([id])
);

CREATE TABLE [BudgetItem] (
    [id] INT IDENTITY(1, 1) PRIMARY KEY,
    [userId] INT,
    [budgetName] NVARCHAR(50),
    [limitAmount] INT,
    [isOverBudget] BIT DEFAULT(0),
    [isDelete] BIT DEFAULT(0),
    CONSTRAINT FK_BudgetItem_User FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE
);

CREATE TABLE [ExpenseTransaction] (
    [userId] INT,
    [budgetId] INT,
    [note] NVARCHAR(100),
    [amount] MONEY NOT NULL,
    [date] DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_ExpenseTransaction_User FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE,
    CONSTRAINT FK_ExpenseTransaction_BudgetItem FOREIGN KEY ([budgetId]) REFERENCES BudgetItem([id])
);

CREATE TABLE [SavingGoal] (
    [id] INT IDENTITY(1, 1) PRIMARY KEY,
    [userId] INT,
    [title] NVARCHAR(100),
    [description] NVARCHAR(500),
    [currentAmount] INT DEFAULT(0),
    [goalAmount] INT,
    [goalDate] DATE,
    [isCompleted] BIT DEFAULT(0),
    [isDelete] BIT DEFAULT(0),
    CONSTRAINT FK_SavingGoal_User FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE
);

CREATE TABLE [SavingTransaction] (
    [id] INT IDENTITY(1, 1) PRIMARY KEY,
    [userId] INT,
    [savingGoalId] INT,
    [note] NVARCHAR(100),
    [amount] INT NOT NULL,
    [date] DATETIME DEFAULT GETDATE(),
    CONSTRAINT FK_SavingTransaction_SavingGoal FOREIGN KEY ([savingGoalId]) REFERENCES [SavingGoal]([id]),
    CONSTRAINT FK_SavingTransaction_User FOREIGN KEY ([userId]) REFERENCES [User]([id]) ON DELETE CASCADE
);

-- Trigger Definitions
CREATE TRIGGER trg_UpdateBalanceAfterIncomeInsert ON [IncomeTransaction]
AFTER INSERT AS
BEGIN
    UPDATE [User] SET balance = balance + i.amount FROM [User] u JOIN inserted i ON u.id = i.userId;
END;
GO

CREATE TRIGGER trg_UpdateBalanceAfterIncomeDelete ON [IncomeTransaction]
AFTER DELETE AS
BEGIN
    UPDATE [User] SET balance = balance - d.amount FROM [User] u JOIN deleted d ON u.id = d.userId;
END;
GO

CREATE TRIGGER trg_UpdateBalanceAfterExpenseInsert ON [ExpenseTransaction]
AFTER INSERT AS
BEGIN
    UPDATE [User] SET balance = balance - i.amount FROM [User] u JOIN inserted i ON u.id = i.userId;
    
    UPDATE [BudgetItem] SET isOverBudget = CASE WHEN expenseTotal > limitAmount THEN 1 ELSE 0 END
    FROM [BudgetItem] b
    INNER JOIN (
        SELECT e.budgetId, SUM(e.amount) AS expenseTotal
        FROM [ExpenseTransaction] e
        WHERE e.budgetId IN (SELECT budgetId FROM inserted)
        GROUP BY e.budgetId
    ) AS totalExpense ON b.id = totalExpense.budgetId;
END;
GO

CREATE TRIGGER trg_UpdateBalanceAfterExpenseDelete ON [ExpenseTransaction]
AFTER DELETE AS
BEGIN
    UPDATE [User] SET balance = balance + d.amount FROM [User] u JOIN deleted d ON u.id = d.userId;
END;
GO

CREATE TRIGGER trg_UpdateSavingGoalAfterInsert ON [SavingTransaction]
AFTER INSERT AS
BEGIN
    UPDATE [SavingGoal] SET currentAmount = currentAmount + i.amountSum,
        isCompleted = CASE WHEN currentAmount + i.amountSum >= goalAmount THEN 1 ELSE 0 END
    FROM [SavingGoal] s
    JOIN (SELECT savingGoalId, SUM(amount) AS amountSum FROM inserted GROUP BY savingGoalId) i ON s.id = i.savingGoalId;
END;
GO

CREATE TRIGGER trg_UpdateSavingGoalAfterDelete ON [SavingTransaction]
AFTER DELETE AS
BEGIN
    UPDATE [SavingGoal] SET currentAmount = currentAmount - d.amountSum,
        isCompleted = CASE WHEN currentAmount - d.amountSum >= goalAmount THEN 1 ELSE 0 END
    FROM [SavingGoal] s
    JOIN (SELECT savingGoalId, SUM(amount) AS amountSum FROM deleted GROUP BY savingGoalId) d ON s.id = d.savingGoalId;
END;
GO

CREATE TRIGGER trg_PreventUserDelete ON [User]
INSTEAD OF DELETE AS
BEGIN
    IF EXISTS (SELECT 1 FROM [FinanceRecord] WHERE userId IN (SELECT id FROM deleted))
    BEGIN
        RAISERROR ('Cannot delete user with active finance records.', 16, 1);
    END
    ELSE
    BEGIN
        DELETE FROM [User] WHERE id IN (SELECT id FROM deleted);
    END
END;
GO

-- Example Data Insertions
INSERT INTO [User] (username, email, password, balance, avatarPath)
VALUES ('hoanghung2', 'hunghvde180038@fpt.edu.vn', '123', 500, 'resources/test.jpg'), 
       ('quocanh', 'anhnqde180064@fpt.edu.vn', '123', 1000, 'resources/test.jpg');

-- [Additional insert statements for other tables]