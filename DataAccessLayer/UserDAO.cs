using BusinessObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer
{
    public class UserDAO
    {
        public static void CreateNewUser(User newUser)
        {
            using var context = new FinanceManagementApplicationContext();
            context.Users.Add(newUser);
            context.SaveChanges();
        }

        public static void UpdateUser(User newUser)
        {
            try
            {
                using var context = new FinanceManagementApplicationContext();
                User user = context.Users.AsNoTracking().FirstOrDefault(u => u.Id == newUser.Id);
                user = newUser;
                if (user != null)
                {
                    context.Users.Update(user);
                    context.SaveChanges();
                }else
                {
                    throw new Exception("User does not exist");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void DeleteUser(int id) 
        {
            using var context = new FinanceManagementApplicationContext();
            User user = context.Users.FirstOrDefault(u => id == u.Id);
            context.Users.Remove(user);
            context.SaveChanges();
        }

        public static User? getUser(string email, string password)
        {
            using var context = new FinanceManagementApplicationContext(); 
            return (context.Users.AsNoTracking().FirstOrDefault(u => email.Equals(u.Email) && password.Equals(u.Password)));
        }

        public static User? getUser(string email)
        {
            using var context = new FinanceManagementApplicationContext();
            return (context.Users.AsNoTracking().FirstOrDefault(u => email.Equals(u.Email)));
        }
    }
}
