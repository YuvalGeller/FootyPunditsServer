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
                AccMessage message = this.AccMessages.FirstOrDefault(m => m.MessageId == vh.MessageId);
                if (message != null)
                    message.Upvotes++;
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
                AccMessage message = this.AccMessages.FirstOrDefault(m => m.MessageId == messageId);
                if (message != null)
                    message.Upvotes--;
                this.SaveChanges();
                return vh;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public Dictionary<int, int> GetLeaderboard()
        {
            List<AccMessage> messages = this.AccMessages.ToList();
            Dictionary<int, int> keyValuePairs = new Dictionary<int, int>();

            foreach (AccMessage message in messages)
            {
                try
                {
                    keyValuePairs[message.AccountId] += message.Upvotes;
                }
                catch
                {
                    keyValuePairs[message.AccountId] = message.Upvotes;
                }
            }

            Dictionary<int, int> accounts = new Dictionary<int, int>();
            var a = (from entry in keyValuePairs orderby entry.Value descending select entry).Take(20);
            foreach (KeyValuePair<int, int> result in a)
            {
                accounts[result.Key] = result.Value;
            }

            return accounts;
        }
    }
}
