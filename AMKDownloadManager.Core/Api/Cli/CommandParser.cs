using System.Linq;

namespace AMKDownloadManager.Core.Api.Cli
{
    public class CommandParser
    {
        public IAppContext AppContext { get; }
        
        public CommandParser(IAppContext appContext)
        {
            AppContext = appContext;
        }

        public string Execute(ICliInterface @interface)
        {
            var allCliEngines = AppContext
                .GetFeatures<ICliEngine>()
                .OrderByDescending(x => x.Order);

            foreach (var cli in allCliEngines)
            {
                var result = cli.Execute(
                    AppContext,
                    this,
                    @interface,
                    "",
                    null
                    );
            }
            
            return null;
        }
    }
}