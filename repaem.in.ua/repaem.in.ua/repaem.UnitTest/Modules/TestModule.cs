using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.MockingKernel.Moq;
using aspdev.repaem.Models.Data;
using System.Web.Mvc;
using Ninject;

namespace repaem.UnitTest.Modules
{
    //public class NinjectControllerFactory : DefaultControllerFactory
    //{
    //    private readonly IKernel kernel;

    //    public NinjectControllerFactory(IKernel kernel)
    //    {
    //        this.kernel = kernel;
    //    }

    //    protected override IController GetControllerInstance(Type controllerType)
    //    {
    //        return (IController)kernel.Get(controllerType);
    //    }
    //}
}
