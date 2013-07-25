using aspdev.repaem.Models.Data;
using Dapper.Data.SqlClient;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace repaemTest
{
    class TestModule : NinjectModule
    {
        public override void Load()
	    {
            Bind<IDatabase>().To<Database>().InSingletonScope();
        }
    }
}
