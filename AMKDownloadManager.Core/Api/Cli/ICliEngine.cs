namespace AMKDownloadManager.Core.Api.Cli
{
    public interface ICliEngine : IFeature
    {
        /// <summary>
        /// Tries to execute a given command if fails or unknown command given it must return null as a result indicating execution must fall to next CliEngine.
        /// </summary>
        /// <param name="appContext"></param>
        /// <param name="parser"></param>
        /// <param name="interface"></param>
        /// <param name="commandName"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        ICommandResult Execute(
            IAppContext appContext,
            CommandParser parser,
            ICliInterface @interface,
            string commandName,
            object[] parameters);
    }
}