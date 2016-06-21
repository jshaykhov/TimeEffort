using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;

namespace TimeEffort.App_Start
{
    public class Encrypt
    {
        public static void EncryptWebConfig(HttpRequestBase request)
        {
            Configuration config = WebConfigurationManager.OpenWebConfiguration(request.ApplicationPath);
            ConfigurationSection section = config.GetSection("connectionStrings");
            if (!section.SectionInformation.IsProtected)
            {
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                config.Save();
            }
        }
    }

}