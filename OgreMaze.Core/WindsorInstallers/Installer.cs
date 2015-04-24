using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using OgreMaze.Core.Services;

namespace OgreMaze.Core.WindsorInstallers
{
    public class Installer : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IMapService>()
                    .ImplementedBy<MapService>()
                    .LifestyleSingleton()
                );

            container.Register(
                Component
                    .For<IFileSystemService>()
                    .ImplementedBy<FileSystemService>()
                    .LifestyleSingleton()
                );

            container.Register(
                Component
                    .For<ITileService>()
                    .ImplementedBy<TileService>()
                    .LifestyleSingleton()
                );

            container.Register(
                Component
                    .For<ISwampNavigator>()
                    .ImplementedBy<SwampNavigator>()
                    .LifestyleSingleton()
                );
        }
    }
}
