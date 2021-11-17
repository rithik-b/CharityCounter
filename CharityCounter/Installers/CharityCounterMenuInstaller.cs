using CharityCounter.UI;
using Zenject;

namespace CharityCounter.Installers
{
    internal class CharityCounterMenuInstaller : Installer
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<ModifierViewController>().AsSingle();
        }
    }
}
