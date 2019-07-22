using System;
using System.Collections.Generic;
using DemoCSharp.entity;
using MySql.Data.MySqlClient;

namespace DemoCSharp.model
{
    public class StudentModel
    {
        public static void SaveStudent( Student obj)
        {
            
            var cmd = new MySqlCommand($"insert into student (rollNumber, name, address , email ) " + "value(@rollNumber, @name, @address, @email ) ",ConnectionHelper.GetConnection());
            cmd.Parameters.AddWithValue("@rollNumber",obj.RollNumber);
            cmd.Parameters.AddWithValue("@name",obj.FullName);
            cmd.Parameters.AddWithValue("@address",obj.Address);
            cmd.Parameters.AddWithValue("@email",obj.Email);
            cmd.ExecuteNonQuery();
            ConnectionHelper.CloseConnection(); 
            Console.WriteLine("Save object success!");
        }
        
        
        
        
        public static List<Student> FindAll( )
        {
            var list = new List<Student>();
            var cmd = new MySqlCommand($"select * from student ",ConnectionHelper.GetConnection() );
             var dataReader =cmd.ExecuteReader();
             while (dataReader.Read())
             {
                 var obj = new Student()
                 {
                     RollNumber = dataReader.GetString("rollNumber"),
                     FullName = dataReader.GetString("name"),
                     Address = dataReader.GetString("address"),
                     Email = dataReader.GetString("email"),
                     
                 };
                 list.Add(obj);
             }
             
             ConnectionHelper.CloseConnection();
             return list;
        }
        
        
        public static Student FindById(string id)
        {
            var cmd = new MySqlCommand($"select * from student where rollNumber = @rollNumber", ConnectionHelper.GetConnection());
            cmd.Parameters.AddWithValue("@rollNumber", id);
            var dataReader = cmd.ExecuteReader();
            if (!dataReader.Read())
            {
                return null;
            }

            var obj = new Student
            {
                RollNumber = dataReader.GetString("rollNumber"),
                FullName = dataReader.GetString("name"),
                Address = dataReader.GetString("address"),
                Email = dataReader.GetString("email"),
            };
            ConnectionHelper.CloseConnection();
            return obj;
        }

        public static void UpdateStudent(Student obj )
        {
            var cmd = new MySqlCommand(
                $"update student set name = @name, address= @address ,email = @email where rollNumber = @rollNumber",ConnectionHelper.GetConnection());
            cmd.Parameters.AddWithValue("@rollNumber", obj.RollNumber);
            cmd.Parameters.AddWithValue("@name", obj.FullName);
            cmd.Parameters.AddWithValue("@address", obj.Address);
            cmd.Parameters.AddWithValue("@email", obj.Email);
            cmd.ExecuteNonQuery();
            ConnectionHelper.CloseConnection();
        }
        
        
         public static void Delete(string id)
        {
            var cmd = new MySqlCommand($"delete from student where rollNumber=@rollNumber",
                ConnectionHelper.GetConnection());
            cmd.Parameters.AddWithValue("@rollNumber", id);
            cmd.ExecuteNonQuery();
            ConnectionHelper.CloseConnection();
            Console.WriteLine("Delete object success!");
        }
    }
}