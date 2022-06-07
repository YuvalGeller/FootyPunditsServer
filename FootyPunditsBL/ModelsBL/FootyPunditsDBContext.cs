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

        public List<AccMessage> GetMessagesByGameId(int id)
        {
            try
            {
                List<AccMessage> messages = this.AccMessages.Where(m => m.ChatGameId == id).ToList();
                foreach (AccMessage message in messages)
                {
                    message.Account = this.GetAccountByID(message.AccountId);
                }
                return messages;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<VotesHistory> GetUserVoteHistory(int id)
        {
            try
            {
                List<VotesHistory> votesHistories = this.VotesHistories.Where(v => v.AccountIdfkey == id).ToList();
                //foreach (AccMessage message in messages)
                //{
                //    message.Account = this.GetAccountByID(message.AccountId);
                //}
                return votesHistories;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public VotesHistory LikeMessage(int messageId, int accountId)
        {
            try
            {
                VotesHistory vh = new VotesHistory()
                {
                    AccountIdfkey = accountId,
                    VotedDate = DateTime.Now,
                    VoteType = 0,
                    MessageId = messageId,
                    IsUpvote = true
                };
                this.VotesHistories.Add(vh);
                this.SaveChanges();
                return vh;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public VotesHistory UnlikeMessage(int messageId, int accountId)
        {
            try
            {
                VotesHistory vh = this.VotesHistories.FirstOrDefault(v => v.MessageId == messageId && v.AccountIdfkey == accountId);
                if (vh == null)
                    return null;
                this.VotesHistories.Remove(vh);
                this.SaveChanges();
                return vh;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}
