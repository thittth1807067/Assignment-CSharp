using System;
using Assignment_CSharp;
using DemoCSharp.entity;
using DemoCSharp.model;
using DemoCSharp.utility;

namespace DemoCSharp
{
    public class GiaoDichSHB: GiaoDich
    {
        private  SHBAccountModel model = new SHBAccountModel();

        public void Register()
        {
            Console.WriteLine("Vui lòng nhập đầy đủ các thông tin bên dưới.");
            Console.WriteLine("Tài khoản đăng nhập: ");
            var username = Console.ReadLine();
            Console.WriteLine("Mật khẩu: ");
            var password = Console.ReadLine();
            Console.WriteLine("Nhập lại mật khẩu: ");
            var cpassword = Console.ReadLine();
            Console.WriteLine("Số dư trong tài khoản: ");
            var balance = Utility.GetUnsignedDecimalNumber();
            SHBAccount shbAccount = new SHBAccount(username, password, balance);
            // validate trước khi save.
            if (model.Save(shbAccount))
            {
                Console.WriteLine(
                    "Đăng ký tài khoản thành công.");
            }
           

            Console.WriteLine("Ấn enter để tiếp tục.");
            Console.ReadLine();
        }
        public GiaoDichSHB()
        {
           
        }
        public void Login()
        {
            Console.WriteLine("Vui lòng nhập thông tin đăng nhập.");
            Console.WriteLine("Tài khoản: ");
            var username = Console.ReadLine();
            Console.WriteLine("Mật khẩu: ");
            var password = Console.ReadLine();
            var acc = model.GetAccountByUsername(username);
           
            if (acc != null )
            {
                MainThread.currentLoggedInAccount = acc;
                Console.WriteLine("Login thành công với tên là " + MainThread.currentLoggedInAccount.UserName);
                return;
            }

            Console.WriteLine("Sai thông tin đăng nhập.");
        }

        public void RutTien()
        {
            Console.WriteLine("Nhập số tiền cần rút: ");
            decimal amount = Utility.GetUnsignedDecimalNumber();
            // lấy thông tin tài khoản mới nhất trước khi kiểm tra số dư.
            MainThread.currentLoggedInAccount = model.GetAccountByUsername(MainThread.currentLoggedInAccount.UserName);
            if (amount > MainThread.currentLoggedInAccount.Balance)
            {
                Console.WriteLine("Không đủ tiền trong tài khoản.");
                return;
            }
            Console.WriteLine("Nhập nội dung giao dịch: ");
            var content = Console.ReadLine();
            var transactionHistory = new TransactionHistory()
            {
                Id = Guid.NewGuid().ToString(),
                Type = TransactionHistory.TransactionType.WITHDRAW,
                Amount = amount,
                Content = content,
                SenderAccountNumber = MainThread.currentLoggedInAccount.AccountNumber,
                ReceiverAccountNumber = MainThread.currentLoggedInAccount.AccountNumber
            };

            if (model.UpdateBalance(MainThread.currentLoggedInAccount, transactionHistory))
            {
                Console.WriteLine("Giao dịch thành công.");
            }
        }

        public void GuiTien()
        {
            Console.WriteLine("Nhập số tiền cần gửi: ");
            decimal amount = Utility.GetUnsignedDecimalNumber();
            // lấy thông tin tài khoản mới nhất trước khi kiểm tra số dư.
            MainThread.currentLoggedInAccount = model.GetAccountByUsername(MainThread.currentLoggedInAccount.UserName);            
            Console.WriteLine("Nhập nội dung giao dịch: ");
            var content = Console.ReadLine();
            var transactionHistory = new TransactionHistory()
            {
                Id = Guid.NewGuid().ToString(),
                Type = TransactionHistory.TransactionType.DEPOSIT,
                Amount = amount,
                Content = content,
                SenderAccountNumber = MainThread.currentLoggedInAccount.AccountNumber,
                ReceiverAccountNumber = MainThread.currentLoggedInAccount.AccountNumber
            };

            if (model.UpdateBalance(MainThread.currentLoggedInAccount, transactionHistory))
            {
                Console.WriteLine("Giao dịch thành công.");
            }

        }

        public void ChuyenKhoan()
        {

            Console.WriteLine("Vui lòng nhập số tài khoản chuyển tiền: ");
            var accountNumber = Console.ReadLine();
            var receiverAccount = model.GetAccountByAccountNumber(accountNumber);
            if (receiverAccount == null)
            {
                Console.WriteLine("Tài khoản nhận tiền không tồn tại hoặc đã bị khoá.");
                return;
            }

            Console.WriteLine("Tài khoản nhận tiền: " + accountNumber);
            Console.WriteLine("Chủ tài khoản: " + receiverAccount.UserName);
            Console.WriteLine("Nhập số tiền chuyển khoản: ");
            var amount = Utility.GetUnsignedDecimalNumber();
            MainThread.currentLoggedInAccount = model.GetAccountByUsername(MainThread.currentLoggedInAccount.UserName);
            if (amount > MainThread.currentLoggedInAccount.Balance)
            {
                Console.WriteLine("Số dư tài khoản không đủ thực hiện giao dịch.");
                return;
            }

            Console.WriteLine("Nhập nội dung giao dịch: ");
            var content = Console.ReadLine();
            var transactionHistory = new TransactionHistory()
            {
                Id = Guid.NewGuid().ToString(),
                Type = TransactionHistory.TransactionType.TRANSFER,
                Amount = amount,
                Content = content,
                SenderAccountNumber = MainThread.currentLoggedInAccount.AccountNumber,
                ReceiverAccountNumber = accountNumber
            };

            if (model.Transfer(MainThread.currentLoggedInAccount, transactionHistory))
            {
                Console.WriteLine("Giao dịch thành công.");
            }
            else
            {
                Console.WriteLine("Giao dịch thất bại, vui lòng thử lại.");
            }
        }
        


    }
}