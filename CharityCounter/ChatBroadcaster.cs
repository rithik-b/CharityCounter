using CharityCounter.Configuration;
using ChatCore;
using ChatCore.Interfaces;
using System;
using Zenject;

namespace CharityCounter
{
    internal class ChatBroadcaster : IInitializable, IDisposable
    {
        private readonly Counter counter;
        private readonly ChatCoreInstance chatCoreInstance;
        private IChatService chatService;

        public ChatBroadcaster(Counter counter)
        {
            this.counter = counter;
            chatCoreInstance = ChatCoreInstance.Create();
        }

        public void Initialize()
        {
            chatService = chatCoreInstance.RunAllServices();
            chatService.OnTextMessageReceived += OnMessageRecieved;
        }

        public void Dispose()
        {
            if (chatService != null)
                chatService.OnTextMessageReceived -= OnMessageRecieved;

            chatCoreInstance.StopAllServices();
        }

        private void OnMessageRecieved(IChatService service, IChatMessage msg)
        {
            if (!PluginConfig.Instance.ModEnabled)
            {
                return;
            }

            if (msg.Message.ToLower().StartsWith(PluginConfig.Instance.ChatCommand))
            {
                string content = Utils.FormatOutput(PluginConfig.Instance.ChatContent, counter.Dollars, counter.NotesMissed, counter.MapsFailed);
                try
                {
                    service.SendTextMessage(content, msg.Channel);
                }
                catch (Exception ){ }
            }
        }
    }
}
