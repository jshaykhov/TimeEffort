using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TimeEffort.Helper
{
    public static class Logger
    {
       
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static void Info(string user, OperationType type, string obj)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            var controllerName="";
            if (routeValues.ContainsKey("controller"))
            {
                controllerName = routeValues["controller"].ToString();
            }
            log.Info(" "+ user + " has " + type.ToString()+" "+ controllerName+obj);
        }
    }
    public enum OperationType
    {
        Inserted,
        Updated,
        Deleted
    }
}