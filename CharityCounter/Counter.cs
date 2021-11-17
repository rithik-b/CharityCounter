using CharityCounter.Configuration;
using System;

namespace CharityCounter
{
    internal class Counter
    {
        public Action CounterUpdatedEvent;

        public float Dollars => (PluginConfig.Instance.MissWeighting * NotesMissed) + (PluginConfig.Instance.FailWeighting * MapsFailed);

        public int NotesMissed
        {
            get => PluginConfig.Instance.NotesMissed;
            private set
            {
                PluginConfig.Instance.NotesMissed = value;
                CounterUpdatedEvent?.Invoke();
            }
        }

        public int MapsFailed
        {
            get => PluginConfig.Instance.MapsFailed;
            private set
            {
                PluginConfig.Instance.MapsFailed = value;
                CounterUpdatedEvent?.Invoke();
            }
        }

        public void NoteMissed() => NotesMissed++;

        public void MapFailed() => MapsFailed++;

        public void ResetValues()
        {
            NotesMissed = 0;
            MapsFailed = 0;
        }
    }
}
