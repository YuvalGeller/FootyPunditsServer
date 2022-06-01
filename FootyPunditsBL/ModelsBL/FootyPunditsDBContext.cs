﻿using System;
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
            catch(Exception e)
            {
                return null;
            }
        }

        // returns true if username exists otherwise returns false
        public bool UsernameExists(string username) => this.UserAccounts.Any(a => a.Username == username);

        public UserAccount GetAccountByID(int id) => this.UserAccounts.FirstOrDefault(a => a.AccountId == id);

        // returns true if email exists otherwise returns false
        public bool EmailExists(string email) => this.UserAccounts.Any(a => a.Email == email);

        //scaffold-dbcontext "Server=localhost\sqlexpress;Database=FootyPunditsDB;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -force
        public UserAccount Login(string email, string pass)
        {
            UserAccount user = this.UserAccounts
                .Where(u => u.Email == email && u.Upass == pass).FirstOrDefault();

            return user;
        }

        public bool UpdateUserPfp(string path, int id)
        {
            UserAccount account = this.UserAccounts.FirstOrDefault(a => a.AccountId == id);
            if (account != null)
            {
                account.ProfilePicture = path;
                this.SaveChanges();
                return true;
            }

            return false;
        }

        public AccMessage AddMsg(AccMessage m)
        {
            try
            {
                this.AccMessages.Add(m);
                this.SaveChanges();
                return m;
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
