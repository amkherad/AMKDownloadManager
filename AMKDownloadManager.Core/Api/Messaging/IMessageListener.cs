namespace AMKDownloadManager.Core.Api.Messaging
{
    /// <summary>
    /// Defines a stub for messaging function.
    /// </summary>
    public interface IMessageListener
    {
        /// <summary>
        /// Callback to notify the listener that a message received.
        /// </summary>
        /// <param name="name">The name of message (it can be fqn of a type for typed messages).</param>
        /// <param name="state">An state for the message.</param>
        /// <param name="source">The source of the message (i.e. LocalProcess or InterProcess)</param>
        void MessageReceived(string name, object state, MessageSource source);
    }
}