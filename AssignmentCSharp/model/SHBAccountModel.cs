using System;
using System.Transactions;
using DemoCSharp.entity;
using MySql.Data.MySqlClient;

namespace DemoCSharp.model
{
    public class SHBAccountModel
    {
        public bool Save(SHBAccount shbAccount)
        {

            var queryString = "insert into `shbaccount` (accountNumber, username, password, balance)" +
                              " values (@accountNumber, @username, @password, @balance)";
            var cmd = new MySqlCommand(queryString, ConnectionHelper.GetConnection());
            cmd.Parameters.AddWithValue("@accountNumber", shbAccount.AccountNumber);
            cmd.Parameters.AddWithValue("@username", shbAccount.UserName);
            cmd.Parameters.AddWithValue("@password", shbAccount.Password);
            cmd.Parameters.AddWithValue("@balance", shbAccount.Balance);
            var result = cmd.ExecuteNonQuery();
            ConnectionHelper.CloseConnection();
            return result == 1;

        }

        public SHBAccount GetAccountByUsername(string username)
        {
            var queryString = "select * from `shbaccount` where `username` = @username";
            var cmd = new MySqlCommand(queryString, ConnectionHelper.GetConnection());
            cmd.Parameters.AddWithValue("@username", username);
            var reader = cmd.ExecuteReader();
            SHBAccount shbAccount = null;
            if (reader.Read())
            {
                shbAccount = new SHBAccount();
                shbAccount.UserName = reader.GetString("username");
                shbAccount.Balance = reader.GetInt32("balance");
            }

            reader.Close();
            ConnectionHelper.GetConnection();
            return shbAccount;
        }
        public SHBAccount GetAccountByAccountNumber(string accountNumber)
        {
            var queryString = "select * from `shbAccount` where `accountNumber` = @accountNumber ";
            var cmd = new MySqlCommand(queryString, ConnectionHelper.GetConnection());
            cmd.Parameters.AddWithValue("@accountNumber", accountNumber);
            var reader = cmd.ExecuteReader();
            SHBAccount shbAccount = null;
            if (reader.Read())
            {
                shbAccount = new SHBAccount
                {
                    AccountNumber = reader.GetString("accountNumber"),
                    UserName = reader.GetString("username"),
                    Password = reader.GetString("password"),
                    Balance = reader.GetInt32("balance"),
                };
            }
            reader.Close();
           ConnectionHelper.CloseConnection();
            return   shbAccount;
        }
         public bool UpdateBalance(SHBAccount currentLoggedInAccount, TransactionHistory transactionHistory)
        {

            try
            {
                // Kiểm tra số dư tài khoản.
                var selectBalance =
                    "select balance from `shbaccount` where accountNumber = @accountNumber ";
                var cmdSelect = new MySqlCommand(selectBalance, ConnectionHelper.GetConnection());
                cmdSelect.Parameters.AddWithValue("@accountNumber", currentLoggedInAccount.AccountNumber);
                var reader = cmdSelect.ExecuteReader();
                decimal currentAccountBalance = 0;
                if (reader.Read())
                {
                    currentAccountBalance = reader.GetDecimal("balance");
                }

                reader.Close(); // important.
                if (transactionHistory.Type == TransactionHistory.TransactionType.WITHDRAW &&
                    currentAccountBalance < transactionHistory.Amount)
                {
                    throw new Exception("Không đủ tiền trong tài khoản.");
                }

                if (transactionHistory.Type == TransactionHistory.TransactionType.WITHDRAW)
                {
                    currentAccountBalance -= transactionHistory.Amount;
                }
                else if (transactionHistory.Type == TransactionHistory.TransactionType.DEPOSIT)
                {
                    currentAccountBalance += transactionHistory.Amount;
                }

                // Update tài khoản.
                var updateQuery =
                    "update `shbaccount` set `balance` = @balance where accountNumber = @accountNumber ";
                var sqlCmd = new MySqlCommand(updateQuery, ConnectionHelper.GetConnection());
                sqlCmd.Parameters.AddWithValue("@balance", currentAccountBalance);
                sqlCmd.Parameters.AddWithValue("@accountNumber", currentLoggedInAccount.AccountNumber);
                var updateResult = sqlCmd.ExecuteNonQuery();

                // Lưu lịch sử giao dịch.
                var historyTransactionQuery =
                    "insert into `shbtransaction` (TransactionId,Type, SenderAccountNumber, ReceiverAccountNumber, Amount, Message ) " +
                    "values (@id, @type, @senderAccountNumber, @receiverAccountNumber, @amount @content)";
                var historyTransactionCmd =
                    new MySqlCommand(historyTransactionQuery,ConnectionHelper.GetConnection());
                historyTransactionCmd.Parameters.AddWithValue("@id", transactionHistory.Id);
                historyTransactionCmd.Parameters.AddWithValue("@amount", transactionHistory.Amount);
                historyTransactionCmd.Parameters.AddWithValue("@type", transactionHistory.Type);
                historyTransactionCmd.Parameters.AddWithValue("@content", transactionHistory.Content);
                historyTransactionCmd.Parameters.AddWithValue("@senderAccountNumber",
                    transactionHistory.SenderAccountNumber);
                historyTransactionCmd.Parameters.AddWithValue("@receiverAccountNumber",
                    transactionHistory.ReceiverAccountNumber);
                var historyResult = historyTransactionCmd.ExecuteNonQuery();

                if (updateResult != 1 || historyResult != 1)
                {
                    throw new Exception("Không thể thêm giao dịch hoặc update tài khoản.");
                }

                transaction.Comit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                transaction.Rollback(); // lưu giao dịch vào.                
                return false;
            }

            ConnectionHelper.CloseConnection();
            return true;
        }

 public bool Transfer(SHBAccount currentLoggedInAccount, TransactionHistory transactionHistory)
 {
     try
            {
                // Kiểm tra số dư tài khoản.
                var selectBalance =
                    "select balance from `shbaccount` where accountNumber = @accountNumber";
                var cmdSelect = new MySqlCommand(selectBalance, ConnectionHelper.GetConnection());
                cmdSelect.Parameters.AddWithValue("@accountNumber", currentLoggedInAccount.AccountNumber);
                var reader = cmdSelect.ExecuteReader();
                decimal currentAccountBalance = 0;
                if (reader.Read())
                {
                    currentAccountBalance = reader.GetDecimal("balance");
                }

                reader.Close(); // important.
                if (currentAccountBalance < transactionHistory.Amount)
                {
                    throw new Exception("Không đủ tiền trong tài khoản.");
                }

                currentAccountBalance -= transactionHistory.Amount;

                // Update tài khoản.
                var updateQuery =
                    "update `shbaccount` set `balance` = @balance where accountNumber = @accountNumber ";
                var sqlCmd = new MySqlCommand(updateQuery, ConnectionHelper.GetConnection());
                sqlCmd.Parameters.AddWithValue("@balance", currentAccountBalance);
                sqlCmd.Parameters.AddWithValue("@accountNumber", currentLoggedInAccount.AccountNumber);
                var updateResult = sqlCmd.ExecuteNonQuery();


                // Kiểm tra số dư tài khoản.
                var selectBalanceReceiver =
                    "select balance from `shbaccount` where accountNumber = @accountNumber ";
                var cmdSelectReceiver = new MySqlCommand(selectBalanceReceiver, ConnectionHelper.GetConnection());
                cmdSelectReceiver.Parameters.AddWithValue("@accountNumber", transactionHistory.ReceiverAccountNumber);
                var readerReceiver = cmdSelectReceiver.ExecuteReader();
                decimal receiverBalance = 0;
                if (readerReceiver.Read())
                {
                    receiverBalance = readerReceiver.GetDecimal("balance");
                }

                readerReceiver.Close(); // important.                
                receiverBalance += transactionHistory.Amount;

                // Update tài khoản.
                var updateQueryReceiver =
                    "update `shbaccount` set `balance` = @balance where accountNumber = @accountNumber ";
                var sqlCmdReceiver = new MySqlCommand(updateQueryReceiver,ConnectionHelper.GetConnection());
                sqlCmdReceiver.Parameters.AddWithValue("@balance", receiverBalance);
                sqlCmdReceiver.Parameters.AddWithValue("@accountNumber", transactionHistory.ReceiverAccountNumber);
                var updateResultReceiver = sqlCmdReceiver.ExecuteNonQuery();

                // Lưu lịch sử giao dịch.
                var historyTransactionQuery =
                    "insert into `shbtransaction` (TransactionId,Type, SenderAccountNumber, ReceiverAccountNumber, Amount, Message) " +
                    "values (@@id, @type, @senderAccountNumber, @receiverAccountNumber, @amount @content)";
                var historyTransactionCmd =
                    new MySqlCommand(historyTransactionQuery,ConnectionHelper.GetConnection());
                historyTransactionCmd.Parameters.AddWithValue("@id", transactionHistory.Id);
                historyTransactionCmd.Parameters.AddWithValue("@amount", transactionHistory.Amount);
                historyTransactionCmd.Parameters.AddWithValue("@type", transactionHistory.Type);
                historyTransactionCmd.Parameters.AddWithValue("@content", transactionHistory.Content);
                historyTransactionCmd.Parameters.AddWithValue("@senderAccountNumber",
                    transactionHistory.SenderAccountNumber);
                historyTransactionCmd.Parameters.AddWithValue("@receiverAccountNumber",
                    transactionHistory.ReceiverAccountNumber);
                var historyResult = historyTransactionCmd.ExecuteNonQuery();

                if (updateResult != 1 || historyResult != 1 || updateResultReceiver != 1)
                {
                    throw new Exception("Không thể thêm giao dịch hoặc update tài khoản.");
                }

                mySqlTransaction.Commit();
                return true;
            }
            catch (Exception e)
            {
                mySqlTransaction.Rollback();
                return false;
            }
            finally
            {                
               ConnectionHelper.CloseConnection();
            }
 }
    }
}