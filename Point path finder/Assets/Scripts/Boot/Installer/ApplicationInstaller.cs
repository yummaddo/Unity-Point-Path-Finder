using Zenject;

namespace PointMove.Boot.Installer
{
    public class ApplicationInstaller : MonoInstaller
    {
        public ApplicationCore core;
        public override void InstallBindings()
        {
            Container.Bind<ApplicationCore>().FromInstance(core).AsSingle();
        }
    }
}