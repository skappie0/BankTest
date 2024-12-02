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
   - Open the `Program.cs` file in Visual Studio 2022.
   - Verify the connection string to ensure it's pointing to the correct SQL Server instance.
   - Modify the connection string if necessary to match your local environment.

## How to Make a Deposit

To make a deposit to an account, you must first log in using the following credentials:

- **Login**: `bank@bank.com`
- **Password**: `bank111`
