﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DI;
using Domain.DI;
using Lamar;

namespace DI
{
    public class AutosystemRegistry : ServiceRegistry
    {
        public AutosystemRegistry()
        {
            //Register domain DI
            IncludeRegistry<DomainRegistry>();
            IncludeRegistry<RepositoryRegistry>();
        }
    }
}