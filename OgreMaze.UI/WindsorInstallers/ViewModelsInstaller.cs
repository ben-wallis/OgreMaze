using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using OgreMaze.UI.ViewModels;

namespace OgreMaze.UI.WindsorInstallers
{
    public class ViewModelsInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Component
                    .For<IMainWindowViewModel>()
                    .ImplementedBy<MainWindowViewModel>()
                    .LifestyleSingleton()
                );

            container.Register(
                Component
                    .For<IMazeControlViewModel>()
                    .ImplementedBy<MazeControlViewModel>()
                    .LifestyleSingleton()
                );
        }
    }
}
