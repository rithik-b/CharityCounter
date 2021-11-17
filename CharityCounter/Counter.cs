using CharityCounter.Configuration;

namespace CharityCounter
{
    internal class Counter
    {
        private int NotesMissed
        {
            get => PluginConfig.Instance.NotesMissed;
            set
            {
                PluginConfig.Instance.NotesMissed = value;
            }
        }

        private int MapsFailed
        {
            get => PluginConfig.Instance.MapsFailed;
            set
            {
                PluginConfig.Instance.MapsFailed = value;
            }
        }

        public void NoteMissed() => NotesMissed++;

        public void MapFailed() => MapsFailed++;
    }
}
