using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootyPunditsBL.Models
{
    public partial class FootyPunditsDBContext
    {
        //scaffold-dbcontext "Server=localhost\sqlexpress;Database=FootyPunditsDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force
        public UserAccount Login(string email, string pass)
        {
            UserAccount user = this.UserAccounts
                .Where(u => u.Email == email && u.Upass == pass).FirstOrDefault();

            return user;
        }
    }
}
