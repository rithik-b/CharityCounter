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

        [UIComponent("miss-keyboard")]
        private RectTransform missKeyboardFieldTransform;

        [UIComponent("fail-keyboard")]
        private RectTransform failKeyboardFieldTransform;

        [UIComponent("file-keyboard")]
        private RectTransform fileKeyboardFieldTransform;

        [UIComponent("chat-keyboard")]
        private RectTransform chatKeyboardFieldTransform;

        public ModifierViewController(Counter counter, FileWriter fileWriter)
        {
            this.counter = counter;
            this.fileWriter = fileWriter;
        }

        public void Initialize()
        {
            GameplaySetup.instance.AddTab(nameof(CharityCounter), "CharityCounter.UI.ModifierView.bsml", this);
            counter.CounterUpdatedEvent += OnCounterUpdated;
        }

        public void Dispose()
        {
            if (GameplaySetup.IsSingletonAvailable)
            {
                GameplaySetup.instance.RemoveTab(nameof(CharityCounter));
            }
            counter.CounterUpdatedEvent -= OnCounterUpdated;
        }

        public void OnCounterUpdated()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MissesText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DollarsText)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FailsText)));
        }

        [UIAction("#post-parse")]
        private void PostParse()
        {
            ModalKeyboard missKeyboard = missKeyboardFieldTransform.Find("BSMLModalKeyboard").GetComponent<ModalKeyboard>();
            ModalKeyboard failKeyboard = failKeyboardFieldTransform.Find("BSMLModalKeyboard").GetComponent<ModalKeyboard>();
            ModalKeyboard fileKeyboard = fileKeyboardFieldTransform.Find("BSMLModalKeyboard").GetComponent<ModalKeyboard>();
            ModalKeyboard chatKeyboard = chatKeyboardFieldTransform.Find("BSMLModalKeyboard").GetComponent<ModalKeyboard>();

            RectTransform missKeyboardTransform = missKeyboard.transform as RectTransform;
            missKeyboardTransform.sizeDelta = new Vector2(30, 42);
            GameObject.Destroy(missKeyboardTransform.Find("KeyboardParent").gameObject);
            RectTransform missParentTransform = new GameObject("KeyboardParent").AddComponent<RectTransform>();
            missParentTransform.SetParent(missKeyboardTransform, false);
            missKeyboard.keyboard = new KEYBOARD(missParentTransform, Utils.Numpad, true, 37, -11);
            missKeyboard.keyboard.EnterPressed += (string value) => missKeyboard.modalView.Hide(true);
            missKeyboard.modalView.blockerClickedEvent += () => missKeyboard.modalView.Hide(true);

            RectTransform failKeyboardTransform = failKeyboard.transform as RectTransform;
            failKeyboardTransform.sizeDelta = new Vector2(30, 42);
            GameObject.Destroy(failKeyboardTransform.Find("KeyboardParent").gameObject);
            RectTransform failParentTransform = new GameObject("KeyboardParent").AddComponent<RectTransform>();
            failParentTransform.SetParent(failKeyboardTransform, false);
            failKeyboard.keyboard = new KEYBOARD(failParentTransform, Utils.Numpad, true, 37, -11);
            failKeyboard.keyboard.EnterPressed += (string value) => failKeyboard.modalView.Hide(true);
            failKeyboard.modalView.blockerClickedEvent += () => failKeyboard.modalView.Hide(true);

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

        [UIValue("misses-text")]
        private string MissesText => $"Misses: {counter.NotesMissed}";

        [UIValue("dollars-text")]
        private string DollarsText => $"${counter.Dollars}";

        [UIValue("fails-text")]
        private string FailsText => $"Maps Failed: {counter.MapsFailed}";

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

        [UIValue("miss-weighting")]
        private string MissWeighting
        {
            get => $"{PluginConfig.Instance.MissWeighting}";
            set
            {
                PluginConfig.Instance.MissWeighting = float.Parse(value);
                fileWriter.WriteFile();
                OnCounterUpdated();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MissWeighting)));
            }
        }

        [UIValue("fail-weighting")]
        private string FailWeighting
        {
            get => $"{PluginConfig.Instance.FailWeighting}";
            set
            {
                PluginConfig.Instance.FailWeighting = float.Parse(value);
                fileWriter.WriteFile();
                OnCounterUpdated();
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FailWeighting)));
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
