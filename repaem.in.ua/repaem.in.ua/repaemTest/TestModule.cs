using aspdev.repaem.Models.Data;
using Dapper.Data.SqlClient;
using Ninject;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace repaemTest
{
    class TestModule : NinjectModule
    {
        public override void Load()
	    {
            //if (ConfigurationManager.AppSettings["Environment"] == "Debug") //localhost connection string
            //{
                Bind<IDatabase>().To<Database>().InSingletonScope();
            //}
            //else
            //{
                //var uriString = ConfigurationManager.AppSettings["SQLSERVER_URI"];
                //var uri = new Uri(uriString);
                //SqlConnectionFactory factory = new SqlConnectionFactory(uri.Host, uri.AbsolutePath.Trim('/'), uri.UserInfo.Split(':').First(), uri.UserInfo.Split(':').Last());

                //Bind<IDatabase>().To<Database>().InSingletonScope().WithConstructorArgument("factory", factory);
            //}
        }
    }
}
