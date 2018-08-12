namespace AMKDownloadManager.Core.Api.Messaging
{
    /// <summary>
    /// A messaging host.
    /// </summary>
    public interface IMessagingHost : IFeature
    {
        /// <summary>
        /// Subscribes a listener to a message.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="listener"></param>
        void Subscribe(string name, IMessageListener listener);

        /// <summary>
        /// Unsubscribes a listener from a message.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="listener"></param>
        void Unsubscribe(string name, IMessageListener listener);

        /// <summary>
        /// Unsubscribes a listener from all messages.
        /// </summary>
        /// <param name="listener"></param>
        void FreeListener(IMessageListener listener);


        /// <summary>
        /// Sends a message to all listeners for that message.
        /// </summary>
        /// <remarks>
        /// The state will be reused for local process targets but it will get serialized for inter-process targets.
        /// So the state for inter-process messages must be serializable (mainly using Json.net)
        /// </remarks>
        /// <param name="name"></param>
        /// <param name="state"></param>
        /// <param name="targetBoundary"></param>
        void Send(string name, object state, MessageSource targetBoundary);
    }
}