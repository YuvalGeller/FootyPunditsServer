using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FootyPunditsBL.Models;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using FootyPundits.DTO;


namespace FootyPundits.Controllers
{
    [Route("FootyPunditsAPI")]
    [ApiController]
    public class FootyPunditsController : ControllerBase
    {
        #region Add connection to the db context using dependency injection
        FootyPunditsDBContext context;

        public FootyPunditsController(FootyPunditsDBContext context)
        {
            this.context = context;
        }
        #endregion

        [Route("Test")]
        [HttpGet]
        public UserAccount Test([FromQuery] int id)
        {
            UserAccount user = context.UserAccounts.Where(a => a.AccountId == id).FirstOrDefault();

            //Check user name and password
            if (user != null)
            {

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

                //Important! Due to the Lazy Loading, the user will be returned with all of its contects!!
                return user;
            }
            else
            {

                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }


        [Route("signup")]
        [HttpPost]
        public UserAccount SignUp([FromBody] UserAccount sentAccount)
        {
            UserAccount current = HttpContext.Session.GetObject<UserAccount>("theUser");
            // Check if user isn't logged in!
            if (current == null)
            {                           
                try
                {
                    bool exists = context.UsernameExists(sentAccount.Username) || context.EmailExists(sentAccount.Email);
                    if (!exists)
                    {
                        UserAccount a = context.SignUp(sentAccount);
                        if (a != null)
                        {
                            HttpContext.Session.SetObject("theUser", a);

                            Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                            return a;
                        }
                        else
                        {
                            Response.StatusCode = (int)System.Net.HttpStatusCode.Conflict;
                            return null;
                        }

                    }
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Conflict;
                }
                catch
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                }
            }
            else
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;

            return null;
        }

        [Route("login")]
        [HttpGet]
        public string Login([FromQuery] string email, [FromQuery] string password)
        {
            UserAccount user = context.Login(email, password);

            //Check user name and password
            if (user != null)
            {
                HttpContext.Session.SetObject("theUser", user);

                JsonSerializerSettings options = new JsonSerializerSettings
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.All
                };

                string json = JsonConvert.SerializeObject(user, options);

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return json;
            }
            else
            {

                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
                return null;
            }
        }

        [Route("email-exists")]
        [HttpGet]
        public bool? EmailExists([FromQuery] string email)
        {
            try
            {
                bool exists = context.EmailExists(email);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return exists;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
        }

        [Route("username-exists")]
        [HttpGet]
        public bool? UsernameExists([FromQuery] string username)
        {
            try
            {
                bool exists = context.UsernameExists(username);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return exists;
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
        }


        [Route("logout")]
        [HttpGet]
        public IActionResult LogOut()
        {
            UserAccount user = HttpContext.Session.GetObject<UserAccount>("theUser");
            if (user != null)
            {
                HttpContext.Session.Clear();
                return Ok();
            }
            else
            {
                return Forbid();
            }
        }

        [Route("uploadimage")]
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            UserAccount user = HttpContext.Session.GetObject<UserAccount>("theUser");

            if (user != null)
            {
                if (file == null)
                {
                    return BadRequest();
                }

                try
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imgs", file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    if (file.FileName.StartsWith("a"))
                        context.UpdateUserPfp(file.FileName, user.AccountId );

                    return Ok(new { length = file.Length, name = file.FileName });
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return BadRequest();
                }
            }
            return Forbid();
        }


        [Route("update-profile")]
        [HttpGet]
        public IActionResult UpdateProfile([FromQuery] string username, [FromQuery] string password)
        {
            UserAccount loggedInAccount = HttpContext.Session.GetObject<UserAccount>("theUser");

            if (loggedInAccount != null)
            {
                UserAccount account = context.UserAccounts.FirstOrDefault(a => a.AccountId == loggedInAccount.AccountId);
                if (account == null) return Forbid();
                if (username != null) account.Username = username;
                if (password.Length >= 8) account.Upass = password;
                context.SaveChanges();

                return Ok();
            }

            return Forbid();
        }

        [Route("get-account")]
        [HttpGet]
        public string GetAccount([FromQuery] int id)
        {
            UserAccount loggedInAccount = HttpContext.Session.GetObject<UserAccount>("theUser");

            if (loggedInAccount != null)
            {
                try
                {
                    UserAccount a = context.GetAccountByID(id);

                    if (a != null)
                    {
                        JsonSerializerSettings options = new JsonSerializerSettings
                        {
                            PreserveReferencesHandling = PreserveReferencesHandling.All
                        };

                        string json = JsonConvert.SerializeObject(a, options);

                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return json;
                    }

                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
                catch
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
            }

            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return null;
        }

        [Route("get-messages")]
        [HttpGet]
        public string GetMessages([FromQuery] int id)
        {
            UserAccount loggedInAccount = HttpContext.Session.GetObject<UserAccount>("theUser");

            if (loggedInAccount != null)
            {
                try
                {
                    List<AccMessage> m = context.GetMessagesByGameId(id);

                    if (m != null)
                    {
                        JsonSerializerSettings options = new JsonSerializerSettings
                        {
                            PreserveReferencesHandling = PreserveReferencesHandling.All
                        };

                        string json = JsonConvert.SerializeObject(m, options);

                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return json;
                    }

                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
                catch
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
            }

            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return null;
        }

        [Route("get-vote-history")]
        [HttpGet]
        public string GetUserVoteHistory([FromQuery] int id)
        {
            UserAccount loggedInAccount = HttpContext.Session.GetObject<UserAccount>("theUser");

            if (loggedInAccount != null)
            {
                try
                {
                    List<VotesHistory> v = context.GetUserVoteHistory(id);

                    if (v != null)
                    {
                        JsonSerializerSettings options = new JsonSerializerSettings
                        {
                            PreserveReferencesHandling = PreserveReferencesHandling.All
                        };

                        string json = JsonConvert.SerializeObject(v, options);

                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return json;
                    }

                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
                catch
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
            }

            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return null;
        }

        [Route("like-message")]
        [HttpGet]
        public VotesHistory LikeMessage([FromQuery] int messageId)
        {
            UserAccount loggedInAccount = HttpContext.Session.GetObject<UserAccount>("theUser");

            if (loggedInAccount != null)
            {
                try
                {
                    VotesHistory b = context.LikeMessage(messageId, loggedInAccount.AccountId);

                    if (b != null)
                    {
                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return b;
                    }

                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
                catch
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
            }

            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return null;
        }

        [Route("unlike-message")]
        [HttpGet]
        public VotesHistory UnlikeMessage([FromQuery] int voteId)
        {
            UserAccount loggedInAccount = HttpContext.Session.GetObject<UserAccount>("theUser");

            if (loggedInAccount != null)
            {
                try
                {
                    VotesHistory b = context.UnlikeMessage(voteId);

                    if (b != null)
                    {
                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return b;
                    }

                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
                catch
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                    return null;
                }
            }

            Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
            return null;
        }
    }
}
