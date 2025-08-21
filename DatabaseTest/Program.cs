using BusinessObjects;
using DataAccessLayer;

namespace DatabaseTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using var context = new FinanceManagementApplicationContext();
            List<User> users= context.Users.ToList();
            foreach (User user in users)
            {
                Console.WriteLine($"{user.Username}; {user.Balance}");
            }
        }
    }
}
