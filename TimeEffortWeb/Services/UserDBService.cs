﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeEffortCore.Services
{
    public class UserDBService
    {
         private static dbEntities db;
        public UserService()
        {
            db = new dbEntities();
        }
    }
}