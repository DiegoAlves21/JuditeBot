﻿using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace JuditeBot
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.EnableSwagger(c => c.SingleApiVersion("v1", "JuditeBot")).EnableSwaggerUi();

            //var corsAttr = new EnableCorsAttribute("http://localhost:49935/swagger/docs/v1", "*", "*");
            //config.EnableCors(corsAttr);

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "JuditeBot/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
