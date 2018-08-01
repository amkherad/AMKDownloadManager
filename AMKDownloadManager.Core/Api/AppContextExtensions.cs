using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AMKsGear.Core.Patterns.AppModel;
using AMKsGear.Core.Automation.IoC;
using System.Runtime.CompilerServices;
using AMKsGear.Architecture.Trace;
using AMKsGear.Core.Collections;
using AMKsGear.Core.Trace;

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

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> SignalFeatures<T>(this IAppContext app, Action<T> signalHandler) where T : IFeature
        {
            var features = app.GetFeatures<T>();
            features?.ForEach(signalHandler);
            return features;
        }
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> SignalFeaturesIgnoreExceptions<T>(this IAppContext app, Action<T> signalHandler) where T : IFeature
        {
            var features = app.GetFeatures<T>();
            try
            {
                features?.ForEach(signalHandler);
            }
            catch (Exception exception)
            {
                Logger.Default.Log(exception);
            }
            return features;
        }
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> SignalFeaturesIgnoreExceptions<T>(this IAppContext app, Action<T> signalHandler, ILogChannel logger) where T : IFeature
        {
            var features = app.GetFeatures<T>();
            try
            {
                features?.ForEach(signalHandler);
            }
            catch (Exception exception)
            {
                logger.Log(exception);
            }
            return features;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetFeatures<T>(this IAppContext app) where T : IFeature
        {
            return app.GetValues<T>()?.OrderByDescending(x => x.Order);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetFeature<T>(this IAppContext app) where T : class, IFeature
        {
            var result = app.GetValues<T>()?.OrderByDescending(x => x.Order).FirstOrDefault() /*??
                app.GetTypeResolver().Resolve<T>()*/;
            if (result == null)
            {
                var type = typeof(T);
                Debug.WriteLine($"Feature of type '{type.FullName}' does not found.");
                throw new InvalidOperationException();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T TryGetFeature<T>(this IAppContext app) where T : class, IFeature
        {
            return
                app.GetValues<T>()?.OrderByDescending(x => x.Order).FirstOrDefault() /*??
                app.GetTypeResolver().Resolve<T>()*/;
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
            return app.AddValues<T>(new[] {feature});
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> AddFeatures<T>(this IAppContext app, IEnumerable<T> features) where T : IFeature
        {
            return app.AddValues<T>(features);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> RemoveFeature<T>(this IAppContext app, T feature) where T : IFeature
        {
            return app.RemoveValues<T>(new[] {feature});
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> RemoveFeatures<T>(this IAppContext app, IEnumerable<T> features) where T : IFeature
        {
            return app.RemoveValues<T>(features);
        }
    }
}