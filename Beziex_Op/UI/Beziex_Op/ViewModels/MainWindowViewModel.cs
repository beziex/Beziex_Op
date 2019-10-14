using System;
using System.Windows;
using System.Reactive.Disposables;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Beziex_Op.Views;
 
namespace Beziex_Op.ViewModels {

    [RegionMemberLifetime(KeepAlive = false)]
    public class MainWindowViewModel : BindableBase, IDisposable {

        private readonly CompositeDisposable  fDisposables = new CompositeDisposable();
        void IDisposable.Dispose() { this.fDisposables.Dispose(); }

        public MainWindowViewModel(IRegionManager rm)
        {
            fRegionManager = rm;

            Loaded = new ReactiveCommand<RoutedEventArgs>().AddTo( fDisposables );
            Loaded.Subscribe(e => {
                ResizeWindow(e);
            });
        }

        // -------------------

        private IRegionManager  fRegionManager = null;

        public ReactiveCommand<RoutedEventArgs>  Loaded { get; }

        private void ResizeWindow(RoutedEventArgs e)
        {
            const double  ratio = 0.9;

            MainWindow  mainWindow = e.Source as MainWindow;
            mainWindow.GetWindowPosSize( out double sx, out double sy, out double sw, out double sh );

            double  dw = sw * ratio;
            double  dh = sh * ratio;
            double  dx = ( sw - dw ) / 2 + sx;
            double  dy = ( sh - dh ) / 2 + sy;

            mainWindow.SetWindowPosSize( dx, dy, dw, dh );

            fRegionManager.RequestNavigate( "ScreenArea", "BxUiScreen" );
            fRegionManager.RequestNavigate( "TabArea", "BxUiTab" );
        }
    }
}
