using Microsoft.EntityFrameworkCore.Migrations;
using static System.Runtime.InteropServices.JavaScript.JSType;

#nullable disable

namespace BankTest.API.Migrations
{
    /// <inheritdoc />
    public partial class AddGetBalanceFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Create the scalar function
            migrationBuilder.Sql(
            @"
                USE [BankTestDB]
                GO
                /****** Object:  UserDefinedFunction [dbo].[getBalance]    Script Date: 11/29/2024 4:06:08 PM ******/
                SET ANSI_NULLS ON
                GO
                SET QUOTED_IDENTIFIER ON
                GO
                -- =============================================
                -- Author:		Pavlo Skalozub
                -- Create date: 29.11.2024
                -- Description:	getBalance
                -- =============================================
                CREATE FUNCTION [dbo].[getBalance] 
                (
	                @pAccountId int
                )
                RETURNS decimal
                AS
                BEGIN
	                declare @debitSum decimal = isnull((select sum(Amount) from Transactions where AccountDebitId = @pAccountId), 0);
	                declare @creditSum decimal = isnull((select sum(Amount) from Transactions where AccountCreditId = @pAccountId), 0);
	                declare @resultSum decimal = @creditSum - @debitSum;

	                return @resultSum;
                END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop the scalar function
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS getBalance");
        }
    }
}
