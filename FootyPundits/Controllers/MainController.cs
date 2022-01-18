using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FootyPunditsBL.Models;

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


        [Route("Login")]
        [HttpPost]
        public UserAccount Login(UserAccount u)
        {
            UserAccount user = context.Login(u.Email, u.Upass);

            //Check user name and password
            if (user != null)
            {
                HttpContext.Session.SetObject("theUser", user);

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


    }
}
