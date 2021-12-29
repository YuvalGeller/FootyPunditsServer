using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace FootyPunditsBL.Models
{
    public partial class FootyPunditsDBContext
    {
        // receives an object of type Account and adds it to the DB. Returns the Account object.
        public UserAccount SignUp(UserAccount a)
        {
            try
            {
                this.UserAccounts.Add(a);
                this.SaveChanges();

                return a;
            }
            catch
            {
                return null;
            }
        }

        // returns true if username exists otherwise returns false
        public bool UsernameExists(string username) => this.UserAccounts.Any(a => a.Username == username);

        // returns true if email exists otherwise returns false
        public bool EmailExists(string email) => this.UserAccounts.Any(a => a.Email == email);

        //scaffold-dbcontext "Server=localhost\sqlexpress;Database=FootyPunditsDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force
        public UserAccount Login(string email, string pass)
        {
            UserAccount user = this.UserAccounts
                .Where(u => u.Email == email && u.Upass == pass).FirstOrDefault();

            return user;
        }
    }
}
