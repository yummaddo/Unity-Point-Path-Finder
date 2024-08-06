using PointMove.Core;
using UnityEngine;
using Zenject;

namespace PointMove.Boot.Installer
{
    public class SessionInstaller : MonoInstaller
    {
        public SessionCore sessionCore;
        public InputService inputService;
        public PointMovementService pointMovementService;
        public PathDrawerService pathDrawerService;
        public override void InstallBindings()
        {
            Container.Bind<SessionCore>().FromInstance(sessionCore).AsSingle();
            Container.Bind<InputService>().FromInstance(inputService).AsSingle();
            Container.Bind<PointMovementService>().FromInstance(pointMovementService).AsSingle();            
            Container.Bind<PathDrawerService>().FromInstance(pathDrawerService).AsSingle();
        }
    }
}