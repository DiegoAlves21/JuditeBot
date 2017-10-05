using DAO.BBL;
using Microsoft.Owin.Security;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Results;

namespace JuditeBot.Controllers
{
    [RoutePrefix("JuditeBot")]
    public class PizzariaController : ApiController
    {
        private IAuthenticationManager Authentication
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }

        [Route("payment-methods")]
        [HttpGet]
        public IHttpActionResult paymentMethods()
        {
            if (this.Authentication.User.Identity.IsAuthenticated)
            {
                try
                {
                    PizzariaRepositorio pizzariaRepositorio = new PizzariaRepositorio();
                    var pizzariaId = int.Parse(this.Authentication.User.Claims.SingleOrDefault().Value);
                    var pizzaria = pizzariaRepositorio.Get(p => p.PizzariaId == pizzariaId).SingleOrDefault();

                    List<dynamic> payments = new List<dynamic>();

                    foreach (CPaymentMethod pay in pizzaria.paymentMethods)
                    {
                        payments.Add(new { name = pay.paymentMethod.ToString(), value = pay.paymentMethod });
                    }

                    return Ok(payments);
                }
                catch(Exception e)
                {
                    return InternalServerError(e);
                }
                
            }
            else
            {
                return Unauthorized();
            }
            
        }
        

    }
}
