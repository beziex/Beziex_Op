using Prism.Ioc;
using Prism.Modularity;
using Beziex_Op.Views;

namespace Beziex_Op {

    public class BxUiScreenModule : IModule {

        public void  OnInitialized( IContainerProvider containerProvider )
        {
        }

        public void  RegisterTypes( IContainerRegistry containerRegistry )
        {
            containerRegistry.RegisterForNavigation<BxUiScreen>();
        }
    }
}
