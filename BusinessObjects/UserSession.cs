using Azure.Identity;
using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class UserSession
    {
        private static UserSession instance;
        public static UserSession Instance => instance ?? (instance = new UserSession());
        public UserSession() { }
        public int Id { get; set; }

        public string? Email { get; set; }
        public string? Username { get; set; }

        public string Password { get; set; } = null!;

        public int? Balance { get; set; }

        public string? AvatarPath { get; set; }
        
        public bool IsPro { get; set; } = false;

        public virtual ICollection<BudgetItem> BudgetItems { get; set; } = new List<BudgetItem>();

        public virtual ICollection<SavingGoal> SavingGoals { get; set; } = new List<SavingGoal>();

        public void SetUser(User user)
        {
            Id = user.Id;
            Email = user.Email;
            Username = user.Username;
            Password = user.Password;
            Balance = user.Balance;
            AvatarPath = user.AvatarPath;
            IsPro = user.IsPro;
        }
    }
}
