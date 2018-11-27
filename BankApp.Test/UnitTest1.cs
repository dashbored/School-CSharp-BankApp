using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BankApp;
using System.IO;
using System.Collections.Generic;

namespace BankApp.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestWithdrawNegativeAmount()
        {
            // Arrange
            Account account = new Account();
            account.AddStartBalance(100m);
            decimal negativeAmount = -100m;
            bool expected = false;

            // Act
            bool result = account.Withdraw(negativeAmount);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestTransferMoreThanAvailable()
        {
            // Arrange
            Account withdrawAccount = new Account();
            Account depositAccount = new Account();
            Bank bank = new Bank();

            withdrawAccount.AddStartBalance(100m);
            decimal transferAmount = 200m;
            bool expected = false;

            // Act
            bool result = bank.Transfer(withdrawAccount, depositAccount, transferAmount);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestWithdrawMoreThanAvailable()
        {
            // Arrange
            Account account = new Account();
            account.AddStartBalance(100m);
            decimal amount = 200m;
            bool expected = false;

            // Act
            bool result = account.Withdraw(amount);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestDepositNegativeAmount()
        {
            // Arrange
            Account account = new Account();
            decimal amount = -200m;
            bool expected = false;

            // Act
            bool result = account.Deposit(amount);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestWithdrawMoreThanBalanceOnCreditAccount()
        {
            // Arrange
            Account account = new Account();
            account.AddStartBalance(300m);
            decimal amount = 500m;
            bool expected = true;
            account.MakeCreditAccount();
            account.SetCreditLimit(1000m);
            account.SetCreditRate(15);

            // Act
            bool result = account.Deposit(amount);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCheckThatSavingRateIsCorrect()
        {
            // Arrange
            Account account = new Account();
            account.AddStartBalance(82m);
            decimal expected = 2.46m/365;
            
            account.SetSavingsRate(3);
            

            // Act
            
            decimal result = account.AddInterest();

            // Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestCheckThatDebtRateIsCorrect()
        {
            // Arrange
            Account account = new Account();
            account.AddStartBalance(-82m);
            account.MakeCreditAccount();
            account.SetCreditLimit(100m);
            account.SetCreditRate(3);
            decimal expected = -2.46m / 365;

            account.SetSavingsRate(3);


            // Act

            decimal result = account.AddInterest();

            // Assert
            Assert.AreEqual(expected, result);
        }
    }
}
