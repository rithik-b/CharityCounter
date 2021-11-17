using CharityCounter.Configuration;
using Zenject;

namespace CharityCounter.Installers
{
    internal class CharityCounterGameInstaller : Installer
    {
        public override void InstallBindings()
        {
            if (PluginConfig.Instance.ModEnabled)
            {
                Container.BindInterfacesTo<MissHandler>().AsSingle();
                Container.BindInterfacesTo<FailHandler>().AsSingle();
            }
        }
    }
}
