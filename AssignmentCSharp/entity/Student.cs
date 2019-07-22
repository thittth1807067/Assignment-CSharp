

namespace DemoCSharp.entity
{
    public class Student
    {
        internal string RollNumber { get; set; }
        internal string FullName { get; set; }
        internal string Email { get; set; }
        internal string Address { get; set; }

        public Student(string rollNumber, string fullName, string email, string address)
        {
            RollNumber = rollNumber;
            FullName = fullName;
            Email = email;
            Address = address;
        }

        public Student()
        {
           
        }

        public override string ToString()
        {
            return $"{RollNumber,-20} {FullName,-20} {Address,-30} {Email,-20}";
        }
        
    }


}