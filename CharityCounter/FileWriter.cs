using CharityCounter.Configuration;
using IPA.Utilities;
using System;
using System.IO;
using System.Threading;
using Zenject;

namespace CharityCounter
{
    internal class FileWriter : IInitializable, IDisposable
    {
        private readonly Counter counter;
        private readonly SemaphoreSlim fileSemaphore;

        public const string DollarsFormat = "{dollars}";
        public const string MissesFormat = "{misses}";
        public const string FailsFormat = "{fails}";

        public FileWriter(Counter counter)
        {
            this.counter = counter;
            fileSemaphore = new SemaphoreSlim(1, 1);
        }

        public void Initialize()
        {
            counter.CounterUpdatedEvent += OnCounterUpdated;
        }

        public void Dispose()
        {
            counter.CounterUpdatedEvent -= OnCounterUpdated;
        }

        private async void OnCounterUpdated()
        {
            string content = PluginConfig.Instance.FileContent;
            content = content.Replace(DollarsFormat, $"{counter.Dollars}");
            content = content.Replace(MissesFormat, $"{counter.NotesMissed}");
            content = content.Replace(FailsFormat, $"{counter.MapsFailed}");

            await fileSemaphore.WaitAsync();
            using (StreamWriter file = new StreamWriter(Path.Combine(UnityGame.UserDataPath, $"{nameof(CharityCounter)}.txt")))
            {
                try
                {
                    await file.WriteAsync(content);
                }
                catch (Exception) { }
            }
            fileSemaphore.Release();
        }
    }
}
