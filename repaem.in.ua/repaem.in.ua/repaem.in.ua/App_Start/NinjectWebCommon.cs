using System;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using Griffin.MvcContrib.Localization;
using Griffin.MvcContrib.Localization.Types;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using NLog;
using Ninject;
using Ninject.Web.Common;
using OAuth2;
using WebActivator;
using aspdev.repaem.App_GlobalResources;
using aspdev.repaem.App_Start;
using aspdev.repaem.Infrastructure.Exceptions;
using aspdev.repaem.Models.Data;
using aspdev.repaem.Security;
using aspdev.repaem.Services;
using aspdev.repaem.Areas.Admin.Services;

[assembly: WebActivator.PreApplicationStartMethod(typeof (NinjectWebCommon), "Start")]
[assembly: ApplicationShutdownMethod(typeof (NinjectWebCommon), "Stop")]

namespace aspdev.repaem.App_Start
{
	public static class NinjectWebCommon
	{
		private static readonly Bootstrapper bootstrapper = new Bootstrapper();

		/// <summary>
		///   Starts the application
		/// </summary>
		public static void Start()
		{
			DynamicModuleUtility.RegisterModule(typeof (OnePerRequestHttpModule));
			DynamicModuleUtility.RegisterModule(typeof (NinjectHttpModule));
			bootstrapper.Initialize(CreateKernel);
		}

		/// <summary>
		///   Stops the application.
		/// </summary>
		public static void Stop()
		{
			bootstrapper.ShutDown();
		}

		/// <summary>
		///   Creates the kernel that will manage your application.
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
		///   Load your modules or register your services here!
		/// </summary>
		/// <param name="kernel">The kernel.</param>
		private static void RegisterServices(IKernel kernel)
		{
			//Database
			kernel.Bind<Database>().ToSelf();

			//Front logic
			kernel.Bind<RepaemLogicProvider>().ToSelf();

			//Admin logic
			kernel.Bind<RepaemManagerLogicProvider>().ToSelf().InSingletonScope();

			//User data
			kernel.Bind<RepaemUserService>().ToSelf().InSingletonScope();

			//Session
			kernel.Bind<ISession>().To<DatabaseSession>().InSingletonScope();
			kernel.Bind<DatabaseSession>().ToSelf();

      //Messages
      kernel.Bind<IMessagesProvider>().To<RepaemMessagesProvider>().InSingletonScope();

			//SMS
			kernel.Bind<ISmsSender>().To<TestSmsSender>().InSingletonScope();

			//Email
			kernel.Bind<IEmailSender>().To<EmailSender>().InSingletonScope();

			//ResourceStringProvider
			kernel.Bind<ILocalizedStringProvider>().To<ResourceStringProvider>()
				.InSingletonScope().WithConstructorArgument("resourceManager", new ResourceManager[] { MetadataLocalization.ResourceManager });

			//RepetitionRepo
			kernel.Bind<RepetitionRepo>().ToSelf();

			kernel.Bind<AuthorizationRoot>().ToSelf();
		}
	}
}