﻿using System;
using AMKDownloadManager.Core.Extensions;
using System.Composition;
using AMKDownloadManager.Core.Api;
using System.Reflection;
using AMKDownloadManager.Core.Api.Binders;
using AMKDownloadManager.HttpDownloader.ProtocolProvider;
using ir.amkdp.gear.core.Trace;

namespace AMKDownloadManager.HttpDownloader.AddIn
{
    [Export(typeof(IComponent))]
    public class Component : IComponent
    {
        public const string ComponentGUID = "fa443c67-0faa-4555-8347-55068ae5993f";

        private static bool _isLoaded = false;
        
        public string Name => "HttpDownloader";

        public string Description => "Download files over http(s)";

        public string Author => "Ali Mousavi Kherad";

        public Version Version
        {
            get
            {
                var assembly = typeof(Component).GetTypeInfo().Assembly;
                var assemblyName = new AssemblyName(assembly.FullName);
                return assemblyName.Version;
            }
        }



        #region IComponent implementation

        public void Install(IAppContext app)
        {
            var config = app.GetFeature<IConfigProvider>();

            config.InstallInt(this,
                ComponentGUID,
                KnownConfigs.DownloadManager.Download.MaxSimultaneousConnections,
                KnownConfigs.DownloadManager.Download.MaxSimultaneousConnections_DefaultValue
            );
            config.InstallInt(this,
                ComponentGUID,
                KnownConfigs.DownloadManager.Download.MaxSimultaneousJobs,
                KnownConfigs.DownloadManager.Download.MaxSimultaneousJobs_DefaultValue
            );
        }

        public void Uninstall(IAppContext app)
        {
            throw new NotImplementedException();
        }

        public void Initialize(IAppContext app)
        {
            if (_isLoaded)
            {
                return;
            }
            Logger.WriteLine("HttpProtocolProvider.Initialize");
            app.AddFeature<IProtocolProvider>(new HttpProtocolProvider());
        }

        public void Unload(IAppContext app)
        {
            app.RemoveFeature(new HttpProtocolProvider());
        }


        #endregion
    }
}