using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.GameplaySetup;
using CharityCounter.Configuration;
using System;
using System.ComponentModel;
using Zenject;

namespace CharityCounter.UI
{
    internal class ModifierViewController : IInitializable, IDisposable, INotifyPropertyChanged
    {
        private readonly Counter counter;
        public event PropertyChangedEventHandler PropertyChanged;

        public ModifierViewController(Counter counter)
        {
            this.counter = counter;
        }

        public void Initialize()
        {
            GameplaySetup.instance.AddTab(nameof(CharityCounter), "CharityCounter.UI.ModifierView.bsml", this);
        }

        public void Dispose()
        {
            if (GameplaySetup.IsSingletonAvailable)
            {
                GameplaySetup.instance.RemoveTab(nameof(CharityCounter));
            }
        }

        [UIAction("reset-counters")]
        private void ResetCounters() => counter.ResetValues();

        [UIValue("enabled")]
        private bool ModEnabled
        {
            get => PluginConfig.Instance.ModEnabled;
            set
            {
                PluginConfig.Instance.ModEnabled = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ModEnabled)));
            }
        }

        [UIValue("file-content")]
        private string FileContent
        {
            get => PluginConfig.Instance.FileContent;
            set
            {
                PluginConfig.Instance.FileContent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FileContent)));
            }
        }

        [UIValue("chat-command")]
        private string ChatCommand
        {
            get => PluginConfig.Instance.ChatCommand;
            set
            {
                PluginConfig.Instance.ChatCommand = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChatCommand)));
            }
        }

        [UIValue("chat-content")]
        private string ChatContent
        {
            get => PluginConfig.Instance.ChatContent;
            set
            {
                PluginConfig.Instance.ChatContent = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ChatContent)));
            }
        }
    }
}
