using System;
using ir.amkdp.gear.arch.Automation.IoC;
using ir.amkdp.gear.core.Automation.IoC;
using ir.amkdp.gear.arch.Patterns;
using ir.amkdp.gear.core.Patterns.AppModel;
using AMKDownloadManager.Core.Impl;
using AMKDownloadManager.Core.Api;

namespace AMKDownloadManager.Core
{
	public class ApplicationHost
	{
		#region Singleton

		public static ApplicationHost Instance { get; private set; } = new ApplicationHost();

		#endregion


		public ITypeResolverContainer Container { get; }
        public IAppContext Pool { get; private set; }


		public void Initialize ()
        {
            Pool = new AppContext();
            AppContext.Context = Pool;

			var container = new TypeResolverContainer ();
			//MainWindow win = new MainWindow ();
			//win.Show ();

			_inject (container);

            TypeResolver.SetWideResolver (container);
            Pool.SetTypeResolver (container);

            _buildDefaults(Pool);
		}

        public void Unload()
        {

        }

		private void _inject (ITypeResolverContainer container)
		{
			//container.RegisterType<> ();
		}

        private void _buildDefaults(IAppContext app)
        {
            app.AddFeature(new DefaultConfigProvider());
            app.AddFeature(new DefaultDownloadManager());
        }
	}
}