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

        public FileWriter(Counter counter)
        {
            this.counter = counter;
            fileSemaphore = new SemaphoreSlim(1, 1);
        }

        public void Initialize()
        {
            counter.CounterUpdatedEvent += WriteFile;
            WriteFile();
        }

        public void Dispose()
        {
            counter.CounterUpdatedEvent -= WriteFile;
        }

        public async void WriteFile()
        {
            string content = Utils.FormatOutput(PluginConfig.Instance.FileContent, counter.Dollars, counter.NotesMissed, counter.MapsFailed);
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
