﻿using System;
using System.Collections.Generic;
using System.Linq;
using ir.amkdp.gear.core.Patterns.AppModel;
using ir.amkdp.gear.core.Automation.IoC;
using System.Runtime.CompilerServices;

namespace AMKDownloadManager.Core.Api
{
    /// <summary>
    /// Extensions to AppContext.
    /// </summary>
    public static class AppContextExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetService<T>(this IAppContext app)
        {
            return app.GetTypeResolver().Resolve<T>();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetFeatures<T>(this IAppContext app) where T : IFeature
        {
            return app.GetValues<T>()?.OrderByDescending(x => x.Order);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetFeature<T>(this IAppContext app) where T : class, IFeature
        {
            return
                app.GetValues<T>()?.OrderByDescending(x => x.Order).FirstOrDefault() ??
                app.GetTypeResolver().Resolve<T>();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SetFeatures<T>(this IAppContext app, params T[] features) where T : IFeature
        {
            return app.SetValues<T>(features);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SetFeatures<T>(this IAppContext app, IEnumerable<T> features) where T : IFeature
        {
            return app.SetValues<T>(features);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> AddFeature<T>(this IAppContext app, T feature) where T : IFeature
        {
            return app.AddValues<T>(new [] { feature });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> AddFeatures<T>(this IAppContext app, IEnumerable<T> features) where T : IFeature
        {
            return app.AddValues<T>(features);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> RemoveFeature<T>(this IAppContext app, T feature) where T : IFeature
        {
            return app.RemoveValues<T>(new [] { feature });
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> RemoveFeatures<T>(this IAppContext app, IEnumerable<T> features) where T : IFeature
        {
            return app.RemoveValues<T>(features);
        }
    }
}