using aspdev.repaem.Security;

[assembly: WebActivator.PreApplicationStartMethod(typeof(aspdev.repaem.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(aspdev.repaem.App_Start.NinjectWebCommon), "Stop")]

namespace aspdev.repaem.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using aspdev.repaem.Infrastructure.Logging;
    using System.Web.Http.Controllers;
    using aspdev.repaem.Infrastructure.Exceptions;
    using System.Web.Mvc;
    using aspdev.repaem.Models.Data;
    using aspdev.repaem.Services;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            //Database
            kernel.Bind<IDatabase>().To<Database>().InSingletonScope();

            //Front logic
            kernel.Bind<IRepaemLogicProvider>().To<RepaemLogicProvider>().InSingletonScope();
            kernel.Bind<RepaemLogicProvider>().ToSelf();

            //User data
            kernel.Bind<IUserService>().To<RepaemUserService>();
            kernel.Bind<RepaemUserService>().ToSelf();

            //Session
            kernel.Bind<ISession>().To<HttpSession>().InSingletonScope();

            //SMS
            kernel.Bind<ISmsSender>().To<TestSmsSender>().InSingletonScope();

            //Email
            kernel.Bind<IEmailSender>().To<TestEmailSender>().InSingletonScope();

            //NLog
            kernel.Bind<ILogger>().To<NLogLogger>().InSingletonScope();

            //ActionInvoker
            //kernel.Bind<IActionInvoker>().To<RepControllerActionInvoker>();
        }        
    }
}
