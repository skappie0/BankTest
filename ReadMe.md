# API Setup Guide

## Prerequisites

To successfully run this API, ensure the following software is installed on your system:

- **SQL Server** (version 16.0.4108.0 or later)
- **.NET 8.0**

## Setting up the Project

Follow these steps to set up and run the API via Visual Studio 2022:

1. **Download and Install Required Software**
   - Install **SQL Server** (16.0.4108.0 or later).
   - Install **.NET 8.0**.
   - Ensure **Visual Studio 2022** is installed with the necessary workloads for .NET development.

2. **Clone or Download the Project Files**
   - Clone or download this repository to your local machine.

3. **Check Connection String**
   - Open the `appsettings.json` file in Visual Studio.
   - Verify the connection string in 'ConnectionStrings.DefaultConnection' to ensure it's pointing to the correct SQL Server instance.
   - Modify the connection string if necessary to match your local environment.
## Authorization
- Open **[POST] /Login/UserLogin**
- Use
```
{
  "email": "bank@bank.com",
  "password": "bank111"
}
``` 
### Or
```
{
  "email": "user1@bank.com",
  "password": "user111"
}
```
## Transactions
### Default
- Created User 'bank' and Bank Account for this user.
- Created User 'user1' and Bank Account for this user.
- Created Transaction from 'bank' to 'user1' for 100 conventional units.

## API Transaction Methods

This API supports three different transaction types. Each transaction is handled by a specific method with the following parameters:

### 1) CreateDeposit

This method allows you to create a deposit transaction.

#### Parameters:
- **eMail**: The email of the account holder making the deposit.
- **depositAccount**: The account number where the deposit will be made.
- **amount**: The amount to deposit.
- **text**: A description or note related to the deposit.

#### Example Usage:
```json
{
  "eMail": "user@example.com",
  "depositAccount": "123456789",
  "amount": 1000.00,
  "text": "Deposit for savings"
}
```

### 2) CreateWithdraw

This method allows you to create a withdrawal transaction.

#### Parameters:
- **eMail**: The email of the account holder making the withdrawal.
- **withdrawAccount**: The account number from which the withdrawal will be made.
- **amount**: The amount to withdraw.
- **text**: A description or note related to the withdrawal.

#### Example Usage:
```json
{
  "eMail": "user@example.com",
  "withdrawAccount": "987654321",
  "amount": 500.00,
  "text": "ATM withdrawal"
}
```

### 3) CreateTransfer

This method allows you to create a transfer transaction between two accounts.

#### Parameters:
- **accountDebit**: The account number from which funds will be debited.
- **accountCredit**: The account number that will receive the funds.
- **amount**: The amount to transfer.
- **text**: A description or note related to the transfer.

#### Example Usage:
```json
{
  "accountDebit": "123456789",
  "accountCredit": "987654321",
  "amount": 1500.00,
  "text": "Transfer to friend"
}
```
