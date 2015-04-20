using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Dynamic;    // this had to be manually added for CreateResponse() method

namespace ApiPractice
{
    public class GaAuthorizationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            // if the user is authenticated using forms authentication
            // no need to check header for basic authentication
            if (Thread.CurrentPrincipal.Identity.IsAuthenticated)
            {
                return;
            }

            if (ValidApiKey(actionContext))
            {
                var authHeader = actionContext.Request.Headers.Authorization;

                if (authHeader != null)
                {
                    if (authHeader.Scheme.Equals("basic", StringComparison.OrdinalIgnoreCase) &&
                    !String.IsNullOrWhiteSpace(authHeader.Parameter))
                    {
                        //var credArray = GetCredentials(authHeader);
                        //var username = credArray[0];
                        //var password = credArray[1];
                        dynamic creds = GetCredentials(authHeader);

                        // NOTE: if going back to the array, check for length == 2
                        //if (credArray.Length < 2)
                        //{
                        //    HandleUnauthorizedRequest(actionContext); ????
                        //}

                        //if (FakeLogin(username, password))
                        if (FakeLogin(creds.username, creds.password))
                        {
                            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(creds.username), null);
                            return;
                        }
                    }
                }

                HandleUnauthorizedRequest(actionContext); 
            }
        }

        private bool ValidApiKey(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            IEnumerable<string> values;
            if (actionContext.Request.Headers.TryGetValues("X-Api-Key", out values) && values.First() == "ABC123")
            {
                return true;
            }

            return false;
        }
        //private string[] GetCredentials(System.Net.Http.Headers.AuthenticationHeaderValue authHeader)
        //{
        //    //Base 64 encoded string
        //    var rawCred = authHeader.Parameter;
        //    var encoding = Encoding.GetEncoding("iso-8859-1");
        //    var cred = encoding.GetString(Convert.FromBase64String(rawCred));

        //    var credArray = cred.Split(':');

        //    return credArray;
        //}

        private dynamic GetCredentials(System.Net.Http.Headers.AuthenticationHeaderValue authHeader)
        {
            //Base 64 encoded string
            var rawCred = authHeader.Parameter;
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var cred = encoding.GetString(Convert.FromBase64String(rawCred));

            //var credArray = cred.Split(':');
            int splitOn = cred.IndexOf(':');   // just in case the password has a colon (:) in it
            string username = cred.Substring(0, splitOn);
            string password = cred.Substring(splitOn + 1);

            dynamic creds = new ExpandoObject();
            creds.username = username;
            creds.password = password;
            //creds.username = credArray[0];
            //creds.password = credArray[1];

            return creds;
        }

        private void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate",
            "Basic Scheme='bytemares' location='http://localhost/login'");
        }

        private bool FakeLogin(string username, string password)
        {
            return (username == "mickey" && password == "mouse");
        }
    }
}