using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Owin.Security.OAuth;
using System.Security.Claims;
using System.Threading.Tasks;
using JuditeBot.BBL;

namespace JuditeBot.Providers
{
    public class ApplicationOAuthProvider : OAuthAuthorizationServerProvider
    {

        private IDictionary<string, string> retornoJson;

        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext c)
        {
            c.Validated();

            return Task.FromResult<object>(null);
        }

        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext c)
        {
            try{
                retornoJson = new Dictionary<string, string>();
                Login login = new Login();

                var retorno = login.Valida(c.UserName, c.Password);

                if (retorno.isValid)
                {
                    Claim claim1 = new Claim(ClaimTypes.NameIdentifier, retorno.pizzariaId);
                    Claim[] claims = new Claim[] { claim1 };
                    ClaimsIdentity claimsIdentity =
                        new ClaimsIdentity(
                           claims, OAuthDefaults.AuthenticationType);

                    retornoJson.Add("mensagem", "Operação Realizada com sucesso");
                    c.Validated(claimsIdentity);
                }
                else
                {
                    retornoJson.Add("mensagem", retorno.mensagem);
                    c.Response.StatusCode = 401;
                }

                return Task.FromResult<object>(null);
            }
            catch(Exception e){
                retornoJson.Add("mensagem", "Erro ao realizar a operação");
                c.Response.StatusCode = 401;
                return Task.FromResult<object>(null);
            }
            
        }

        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in retornoJson)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
    }
}