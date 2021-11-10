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

        //[Route("Login")]
        //[HttpGet]
        //public UserAccount Login([FromQuery] string email, [FromQuery] string Upass)
        //{
        //    UserAccount user = context.Login(email, Upass);

        //    //Check user name and password
        //    if (user != null)
        //    {
        //        HttpContext.Session.SetObject("theUser", user);

        //        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

        //        //Important! Due to the Lazy Loading, the user will be returned with all of its contects!!
        //        return user;
        //    }
        //    else
        //    {

        //        Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
        //        return null;
        //    }
        //}

    }
}
