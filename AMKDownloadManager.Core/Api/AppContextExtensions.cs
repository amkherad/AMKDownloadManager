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
    /// Extensions to ApplicationContext.
    /// </summary>
    public static class AppContextExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetService<T>(this IApplicationContext application)
        {
            return application.GetTypeResolver().Resolve<T>();
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> SignalFeatures<T>(this IApplicationContext application, Action<T> signalHandler) where T : IFeature
        {
            var features = application.GetFeatures<T>();
            features?.ForEach(signalHandler);
            return features;
        }
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public static IEnumerable<T> SignalFeaturesIgnoreExceptions<T>(this IApplicationContext application, Action<T> signalHandler) where T : IFeature
        {
            var features = application.GetFeatures<T>();
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
        public static IEnumerable<T> SignalFeaturesIgnoreExceptions<T>(this IApplicationContext application, Action<T> signalHandler, ILogChannel logger) where T : IFeature
        {
            var features = application.GetFeatures<T>();
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
        public static IEnumerable<T> GetFeatures<T>(this IApplicationContext application) where T : IFeature
        {
            return application.GetValues<T>()?.OrderByDescending(x => x.Order);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetFeature<T>(this IApplicationContext application) where T : class, IFeature
        {
            var result = application.GetValues<T>()?.OrderByDescending(x => x.Order).FirstOrDefault() /*??
                application.GetTypeResolver().Resolve<T>()*/;
            if (result == null)
            {
                var type = typeof(T);
                Debug.WriteLine($"Feature of type '{type.FullName}' does not found.");
                throw new InvalidOperationException();
            }
            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T TryGetFeature<T>(this IApplicationContext application) where T : class, IFeature
        {
            return
                application.GetValues<T>()?.OrderByDescending(x => x.Order).FirstOrDefault() /*??
                application.GetTypeResolver().Resolve<T>()*/;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SetFeatures<T>(this IApplicationContext application, params T[] features) where T : IFeature
        {
            return application.SetValues<T>(features);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> SetFeatures<T>(this IApplicationContext application, IEnumerable<T> features) where T : IFeature
        {
            return application.SetValues<T>(features);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> AddFeature<T>(this IApplicationContext application, T feature) where T : IFeature
        {
            return application.AddValues<T>(new[] {feature});
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> AddFeatures<T>(this IApplicationContext application, IEnumerable<T> features) where T : IFeature
        {
            return application.AddValues<T>(features);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> RemoveFeature<T>(this IApplicationContext application, T feature) where T : IFeature
        {
            return application.RemoveValues<T>(new[] {feature});
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> RemoveFeature<T>(this IApplicationContext application) where T : IFeature
        {
            return application.RemoveValues<T>(application.GetFeatures<T>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> RemoveFeatures<T>(this IApplicationContext application, IEnumerable<T> features) where T : IFeature
        {
            return application.RemoveValues<T>(features);
        }
    }
}