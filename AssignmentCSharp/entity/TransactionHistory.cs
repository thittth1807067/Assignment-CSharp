using System;

namespace DemoCSharp.entity
{
    public class TransactionHistory
    {
        public enum TransactionType
        {
            WITHDRAW = 1,
            DEPOSIT = 2, 
            TRANSFER = 3
        }
        public enum TransactionStatus
        {
            DONE = 1,
            PROTECTED = 2,
            DELETED = 3
        }
        private string _id;
        private decimal _amount;
        private TransactionType _type; // 1. rút tiền, 2. gửi tiền, 3. chuyển khoản.
        private string _content;
        private string _createdAt;
        private string _senderAccountNumber;
        private string _receiverAccountNumber;
        private TransactionStatus _status; // 1. hoàn thành, 2. đang thực hiện. 0. đã xoá.
        
        public TransactionHistory()
        {
        }
        public TransactionHistory(decimal amount, TransactionType type, string content, string createdAt, string senderAccountNumber, string receiverAccountNumber, TransactionStatus status)
        {
            _id = Guid.NewGuid().ToString();
            _amount = amount;
            _type = type;
            _content = content;
            _createdAt = createdAt;
            _senderAccountNumber = senderAccountNumber;
            _receiverAccountNumber = receiverAccountNumber;
            _status = status;
        }

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public decimal Amount
        {
            get => _amount;
            set => _amount = value;
        }

        public TransactionType Type
        {
            get => _type;
            set => _type = value;
        }

        public string Content
        {
            get => _content;
            set => _content = value;
        }

        public string CreatedAt
        {
            get => _createdAt;
            set => _createdAt = value;
        }

        public string SenderAccountNumber
        {
            get => _senderAccountNumber;
            set => _senderAccountNumber = value;
        }

        public string ReceiverAccountNumber
        {
            get => _receiverAccountNumber;
            set => _receiverAccountNumber = value;
        }

        public TransactionStatus Status
        {
            get => _status;
            set => _status = value;
        }
    }
}