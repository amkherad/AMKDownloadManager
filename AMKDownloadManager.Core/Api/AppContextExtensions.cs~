using System;
using System.Collections.Generic;
using System.Linq;
using ir.amkdp.gear.core.Patterns.AppModel;

namespace AMKDownloadManager.Core.Api
{
    public static class AppContextExtensions
    {
        public static IEnumerable<T> GetFeatures<T>(this IAppContext app) where T : IFeature
        {
            return app.GetValues<T>().OrderByDescending(x => x.Order);
        }

        public static T GetFeature<T>(this IAppContext app) where T : IFeature
        {
            return app.GetValues<T>().OrderByDescending(x => x.Order).FirstOrDefault();
        }

        public static IEnumerable<T> SetFeatures<T>(this IAppContext app, params T[] features) where T : IFeature
        {
            return app.SetValues<T>(features);
        }

        public static IEnumerable<T> SetFeatures<T>(this IAppContext app, IEnumerable<T> features) where T : IFeature
        {
            return app.SetValues<T>(features);
        }

        public static IEnumerable<T> AddFeature<T>(this IAppContext app, T feature) where T : IFeature
        {
            return app.AddValues<T>(new [] { feature });
        }

        public static IEnumerable<T> AddFeatures<T>(this IAppContext app, IEnumerable<T> features) where T : IFeature
        {
            return app.AddValues<T>(features);
        }

        public static IEnumerable<T> RemoveFeature<T>(this IAppContext app, T feature) where T : IFeature
        {
            return app.RemoveValues<T>(new [] { feature });
        }

        public static IEnumerable<T> RemoveFeatures<T>(this IAppContext app, IEnumerable<T> features) where T : IFeature
        {
            return app.RemoveValues<T>(features);
        }
    }
}