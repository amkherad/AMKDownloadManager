using System;
using ir.amkdp.gear.arch.Automation.IoC;
using ir.amkdp.gear.core.Automation.IoC;
using ir.amkdp.gear.arch.Patterns;
using ir.amkdp.gear.core.Patterns.AppModel;
using AMKDownloadManager.Core.Impl;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.DownloadManagement;
using AMKDownloadManager.Core.Api.Network;
using AMKDownloadManager.Core.Api.Threading;

namespace AMKDownloadManager.Core
{
    /// <summary>
    /// Main application root.
    /// </summary>
	public class ApplicationHost
	{
		#region Singleton

        /// <summary>
        /// Gets the singleton instance of ApplicationHost.
        /// </summary>
        /// <value>The instance.</value>
		public static ApplicationHost Instance { get; private set; } = new ApplicationHost();

		#endregion

        /// <summary>
        /// Gets the application main inversion of control container.
        /// </summary>
        /// <value>The container.</value>
		public ITypeResolverContainer Container { get; }

        /// <summary>
        /// Gets the application service pool.
        /// </summary>
        /// <value>The pool.</value>
        public IAppContext Pool { get; private set; }

        private bool _initialized;

        /// <summary>
        /// Initialize this instance of ApplicationHost. only first call has effects.
        /// </summary>
        public IAppContext Initialize (IThreadFactory threadFactory)
        {
            if (threadFactory == null) throw new ArgumentNullException(nameof(threadFactory));

            if (_initialized)
            {
                return Pool;
            }
            _initialized = true;
            
            var pool = new AppContext();
            Pool = pool;
            AppContext.Context = pool;

			var container = new TypeResolverContainer ();
			//MainWindow win = new MainWindow ();
			//win.Show ();

			_inject (container);

            TypeResolver.SetWideResolver (container);
            pool.SetTypeResolver (container);

	        pool.AddFeature<IThreadFactory>(threadFactory);
	        
            _buildDefaults(Pool);

            return pool;
		}

        /// <summary>
        /// Unload's this instance of ApplicationHost. this leads to unloading entire application.
        /// </summary>
        public void Unload()
        {

        }


        /// <summary>
        /// Injects all services to IoC container.
        /// </summary>
        /// <param name="container">Container.</param>
		private void _inject (ITypeResolverContainer container)
		{
			//container.RegisterType<> ();
		}

        /// <summary>
        /// Injects default services to service pool.
        /// </summary>
        /// <param name="app">App.</param>
        private void _buildDefaults(IAppContext app)
        {
            app.AddFeature<IConfigProvider>(new DefaultConfigProvider());
            app.AddFeature<INetworkMonitor>(new DefaultNetworkMonitor());

            var scheduler = new DefaultJobScheduler(app);

            app.AddFeature<IScheduler>(scheduler);
            app.AddFeature<IDownloadManager>(new DefaultDownloadManager(app, scheduler));
        }
	}
}