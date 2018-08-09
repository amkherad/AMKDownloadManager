using System.Linq;

namespace AMKDownloadManager.Core.Api.Cli
{
    public class CommandParser
    {
        public IApplicationContext ApplicationContext { get; }
        
        public CommandParser(IApplicationContext applicationContext)
        {
            ApplicationContext = applicationContext;
        }

        public string Execute(ICliInterface @interface)
        {
            var allCliEngines = ApplicationContext
                .GetFeatures<ICliEngine>()
                .OrderByDescending(x => x.Order);

            foreach (var cli in allCliEngines)
            {
                var result = cli.Execute(
                    ApplicationContext,
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