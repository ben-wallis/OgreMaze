using Castle.Windsor;
using Castle.Windsor.Installer;

namespace OgreMaze.Core
{
    class Program
    {
        static void Main(string[] args)
        {
            IWindsorContainer container = new WindsorContainer();

            container.Install(
                FromAssembly.This()
                );

            var swampNavigator = container.Resolve<ISwampNavigator>();

            //swampNavigator.NavigateMap("C:\\ChallengeInput1.txt");
            swampNavigator.GenerateMapAndNavigate(100, 60);
        }
    }
}
