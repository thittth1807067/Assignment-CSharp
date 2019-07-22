using System;

namespace DemoCSharp.entity
{
    public class SHBAccount
    {
        public string AccountNumber { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public decimal Balance { get; set; }

        public SHBAccount()
        {
            GenerateAccountNumber();
        }

        private void GenerateAccountNumber()
        {
            AccountNumber = Guid.NewGuid().ToString();
        }

        public SHBAccount(string userName, string password, decimal balance)
        {
            GenerateAccountNumber();
            UserName = userName;
            Password = password;
            Balance = balance;
        }
    }
}