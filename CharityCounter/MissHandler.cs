using Zenject;

namespace CharityCounter
{
    internal class MissHandler : IInitializable
    {
        private readonly ScoreController scoreController;
        private readonly Counter counter;

        public MissHandler(ScoreController scoreController, Counter counter)
        {
            this.scoreController = scoreController;
            this.counter = counter;
        }

        public void Initialize()
        {
            scoreController.noteWasCutEvent += OnNoteCut;
            scoreController.noteWasMissedEvent += OnNoteMiss;
        }

        public void Dispose()
        {
            scoreController.noteWasCutEvent -= OnNoteCut;
            scoreController.noteWasMissedEvent -= OnNoteMiss;
        }

        private void OnNoteCut(NoteData noteData, in NoteCutInfo noteCutInfo, int _)
        {
            if (noteData.colorType != ColorType.None && !noteCutInfo.allIsOK)
            {
                counter.NoteMissed();
            }
        }

        private void OnNoteMiss(NoteData _, int __) => counter.NoteMissed();
    }
}
