using BeatSaberMarkupLanguage.Attributes;
using BeatSaberMarkupLanguage.Components;
using BeatSaberMarkupLanguage.GameplaySetup;
using CharityCounter.Configuration;
using System;
using System.ComponentModel;
using UnityEngine;
using Zenject;

namespace CharityCounter.UI
{
    internal class ModifierViewController : IInitializable, IDisposable, INotifyPropertyChanged
    {
        private readonly Counter counter;
        private readonly FileWriter fileWriter;
        public event PropertyChangedEventHandler PropertyChanged;

        [UIComponent("file-keyboard")]
        private RectTransform fileKeyboardTransform;

        [UIComponent("chat-keyboard")]
        private RectTransform chatKeyboardTransform;

        public ModifierViewController(Counter counter, FileWriter fileWriter)
        {
            this.counter = counter;
            this.fileWriter = fileWriter;
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

        [UIAction("#post-parse")]
        private void PostParse()
        {
            ModalKeyboard fileKeyboard = fileKeyboardTransform.Find("BSMLModalKeyboard").GetComponent<ModalKeyboard>();
            ModalKeyboard chatKeyboard = chatKeyboardTransform.Find("BSMLModalKeyboard").GetComponent<ModalKeyboard>();

            KEYBOARD.KEY dollarsKey = new KEYBOARD.KEY(fileKeyboard.keyboard, new Vector2(-35, 11f), Utils.DollarsFormat, 18, 10, new Color(0.92f, 0.64f, 0));
            KEYBOARD.KEY missesKey = new KEYBOARD.KEY(fileKeyboard.keyboard, new Vector2(-25, 11f), Utils.MissesFormat, 18, 10, new Color(0.92f, 0.64f, 0));
            KEYBOARD.KEY failsKey = new KEYBOARD.KEY(fileKeyboard.keyboard, new Vector2(-15, 11f), Utils.FailsFormat, 15, 10, new Color(0.92f, 0.64f, 0));
            fileKeyboard.keyboard.keys.Add(dollarsKey);
            fileKeyboard.keyboard.keys.Add(missesKey);
            fileKeyboard.keyboard.keys.Add(failsKey);

            dollarsKey = new KEYBOARD.KEY(chatKeyboard.keyboard, new Vector2(-35, 11f), Utils.DollarsFormat, 18, 10, new Color(0.92f, 0.64f, 0));
            missesKey = new KEYBOARD.KEY(chatKeyboard.keyboard, new Vector2(-25, 11f), Utils.MissesFormat, 18, 10, new Color(0.92f, 0.64f, 0));
            failsKey = new KEYBOARD.KEY(chatKeyboard.keyboard, new Vector2(-15, 11f), Utils.FailsFormat, 15, 10, new Color(0.92f, 0.64f, 0));
            chatKeyboard.keyboard.keys.Add(dollarsKey);
            chatKeyboard.keyboard.keys.Add(missesKey);
            chatKeyboard.keyboard.keys.Add(failsKey);
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
                fileWriter.WriteFile();
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
