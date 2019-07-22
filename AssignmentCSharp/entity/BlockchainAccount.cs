using System;

namespace DemoCSharp.entity
{
    public class BlockchainAccount
    {
        public string AccountNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }

        public BlockchainAccount()
        {
            GenerateAccountNumber();
        }

        private void GenerateAccountNumber()
        {
            AccountNumber = Guid.NewGuid().ToString();
        }

        public BlockchainAccount(string userName, string password, decimal balance)
        {
            GenerateAccountNumber();
            UserName = userName;
            Password = password;
            Balance = balance;
        }
    }
}