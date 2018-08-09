using System;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Automation.IoC;
using AMKDownloadManager.Core.Api;
using AMKDownloadManager.Core.Api.Threading;
using AMKsGear.Core.Automation.IoC.TypeBindings;

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
        public IApplicationContext Pool { get; private set; }

        private bool _initialized;

        /// <summary>
        /// Initialize this instance of ApplicationHost. only first call has effects.
        /// </summary>
        public IApplicationContext Initialize (IThreadFactory threadFactory)
        {
            if (threadFactory == null) throw new ArgumentNullException(nameof(threadFactory));

            if (_initialized)
            {
                return Pool;
            }
            _initialized = true;
	        
            var pool = new ApplicationContext();
            Pool = pool;
            ApplicationContext.Instance = pool;

			var container = new TypeResolverContainer ();
			//MainWindow win = new MainWindow ();
			//win.Show ();

			_inject (container);

            TypeResolver.SetWideResolver (container);
            pool.SetTypeResolver (container);

	        pool.AddFeature<IThreadFactory>(threadFactory);
	        
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
	}
}