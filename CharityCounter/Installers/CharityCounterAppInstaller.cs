using Zenject;

namespace CharityCounter.Installers
{
    internal class CharityCounterAppInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesAndSelfTo<Counter>().AsSingle();
        }
    }
}
