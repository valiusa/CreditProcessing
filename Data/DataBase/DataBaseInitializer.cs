using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DataBase
{
    public class DataBaseInitializer
    {
        public static void Initialize(IDbConnection dbConnection)
        {
            using (var command = dbConnection.CreateCommand())
            {
                command.CommandText = @"
                    CREATE TABLE IF NOT EXISTS Credits (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CreditNumber INTEGER NOT NULL UNIQUE,
                        ClientName TEXT NOT NULL,
                        CreditAmount REAL NOT NULL,
                        DateOfCreditApplication INTEGER NOT NULL, -- Store as Unix timestamp
                        CreditStatus INTEGER NOT NULL
                    );
                
                    CREATE TABLE IF NOT EXISTS Invoices (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        InvoiceNumber INTEGER NOT NULL UNIQUE,
                        CreditId INTEGER NOT NULL,
                        InvoiceAmount REAL NOT NULL,
                        FOREIGN KEY (CreditId) REFERENCES Credits(Id) ON DELETE CASCADE
                    );
                
                    -- Clear existing data to avoid duplicates during testing
                    DELETE FROM Invoices;
                    DELETE FROM Credits;
                
                    -- Insert Credits with status Created
                    INSERT INTO Credits (CreditNumber, ClientName, CreditAmount, DateOfCreditApplication, CreditStatus)
                    VALUES 
                        (1001, 'Alice', 5000.00, strftime('%s', '2024-01-01'), 1), -- Created
                        (1002, 'Bob', 3000.00, strftime('%s', '2024-02-15'), 1);  -- Created
                
                    -- Insert Credits with status AwaitingPayment and corresponding Invoices
                    INSERT INTO Credits (CreditNumber, ClientName, CreditAmount, DateOfCreditApplication, CreditStatus)
                    VALUES 
                        (1003, 'Judy', 2500.00, strftime('%s', '2024-09-01'), 2),  -- AwaitingPayment
                        (1004, 'Karl', 3000.00, strftime('%s', '2024-09-15'), 2),  -- AwaitingPayment
                        (1005, 'Laura', 4000.00, strftime('%s', '2024-10-01'), 2), -- AwaitingPayment
                        (1006, 'Mallory', 5000.00, strftime('%s', '2024-10-10'), 2); -- AwaitingPayment
                
                    INSERT INTO Invoices (InvoiceNumber, CreditId, InvoiceAmount)
                    VALUES 
                        (2013, 3, 1200.00),  -- Invoice for Judy
                        (2014, 3, 1500.00),  -- Invoice for Judy
                        (2015, 4, 3000.00),  -- Invoice for Karl
                        (2016, 5, 2000.00),  -- Invoice for Laura
                        (2017, 5, 3000.00),  -- Invoice for Laura
                        (2018, 6, 1000.00),  -- Invoice for Mallory
                        (2019, 6, 4000.00);  -- Invoice for Mallory
                
                    -- Insert Credits with status Paid and corresponding Invoices
                    INSERT INTO Credits (CreditNumber, ClientName, CreditAmount, DateOfCreditApplication, CreditStatus)
                    VALUES 
                        (1007, 'Charlie', 2000.00, strftime('%s', '2024-03-10'), 3), -- Paid
                        (1008, 'Dave', 3000.00, strftime('%s', '2024-04-01'), 3),    -- Paid
                        (1009, 'Eve', 1500.00, strftime('%s', '2024-04-15'), 3),      -- Paid
                        (1010, 'Frank', 2500.00, strftime('%s', '2024-05-20'), 3),    -- Paid
                        (1011, 'Grace', 4000.00, strftime('%s', '2024-06-10'), 3),    -- Paid
                        (1012, 'Heidi', 3500.00, strftime('%s', '2024-07-25'), 3),     -- Paid
                        (1013, 'Ivan', 4500.00, strftime('%s', '2024-08-30'), 3);      -- Paid
                
                    INSERT INTO Invoices (InvoiceNumber, CreditId, InvoiceAmount)
                    VALUES 
                        (2020, 7, 500.00),  -- Invoice for Charlie
                        (2021, 7, 1500.00), -- Invoice for Charlie
                        (2022, 8, 3000.00), -- Invoice for Dave
                        (2023, 9, 1000.00), -- Invoice for Eve
                        (2024, 9, 600.00),  -- Invoice for Eve
                        (2025, 10, 2500.00), -- Invoice for Frank
                        (2026, 11, 2000.00), -- Invoice for Grace
                        (2027, 11, 2500.00), -- Invoice for Grace
                        (2028, 12, 3500.00), -- Invoice for Heidi
                        (2029, 12, 1000.00), -- Invoice for Heidi
                        (2030, 13, 3000.00), -- Invoice for Ivan
                        (2031, 13, 1500.00); -- Invoice for Ivan
                ";

                command.ExecuteNonQuery();
            }
        }
    }
}
