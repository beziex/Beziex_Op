using Beziex_Op.Views;
using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using Beziex_Op.Model;

namespace Beziex_Op {

    public partial class App {

        protected override Window  CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void  RegisterTypes( IContainerRegistry containerRegistry )
        {
            containerRegistry.RegisterSingleton<BxRoot>();
            containerRegistry.RegisterSingleton<BxScreenModel>();
            containerRegistry.RegisterSingleton<BxTabGeneralModel>();
            containerRegistry.RegisterSingleton<BxTabTransformModel>();
        }

        protected override void  ConfigureModuleCatalog( IModuleCatalog moduleCatalog )
        {
            moduleCatalog.AddModule<BxUiScreenModule>(InitializationMode.WhenAvailable);
            moduleCatalog.AddModule<BxUiTabModule>(InitializationMode.WhenAvailable);
        }
    }
}
