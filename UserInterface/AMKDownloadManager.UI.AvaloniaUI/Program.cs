﻿using System;
using System.Runtime.InteropServices;
using AMKDownloadManager.Core;
using AMKDownloadManager.UI.AvaloniaUI.Views.Main;
using AMKDownloadManager.UI.Business;
using AMKDownloadManager.UI.Business.ViewModels.Main;
using AMKsGear.Architecture.Automation.IoC;
using AMKsGear.Core.Automation.IoC;
using AMKsGear.Core.Patterns.AppModel;
using Avalonia;
using Avalonia.Logging.Serilog;

namespace AMKDownloadManager.UI.AvaloniaUI
{
    class Program
    {
        static void Main(string[] args)
        {
            if (AppInitializer.InitializeApplication(args, out var appContext))
            {
                if (!(appContext.TypeResolver is ITypeResolverContainer typeResolver))
                {
                    throw new InvalidOperationException();
                }

                DependencyBuilder.BuildContainer(appContext, typeResolver);

                var app = BuildAvaloniaApp(typeResolver);
                app.Start<MainWindow>(() => typeResolver.Resolve<MainWindowViewModel>());
            }
        }

        public static AppBuilder BuildAvaloniaApp(ITypeResolver typeResolver)
            => AppBuilder.Configure<App>() //(typeResolver.Resolve<App>())
                .UsePlatformDetect()
                .UseReactiveUI()
                .LogToDebug();
    }
}