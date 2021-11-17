using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace CharityCounter
{
    internal class FailHandler : IInitializable, IDisposable
    {
        private readonly GameEnergyCounter gameEnergyCounter;
        private readonly Counter counter;

        public FailHandler(GameEnergyCounter gameEnergyCounter, Counter counter)
        {
            this.gameEnergyCounter = gameEnergyCounter;
            this.counter = counter;
        }

        public void Initialize()
        {
            gameEnergyCounter.gameEnergyDidReach0Event += OnFail;
        }

        public void Dispose()
        {
            gameEnergyCounter.gameEnergyDidReach0Event -= OnFail;
        }

        private void OnFail() => counter.MapFailed();
    }
}
