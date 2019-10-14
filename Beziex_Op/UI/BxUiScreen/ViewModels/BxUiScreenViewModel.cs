using System;
using System.Windows;
using System.Reactive.Disposables;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using OpenTK;
using Beziex_Op.Model;
using Beziex_Op.Views;

namespace Beziex_Op.ViewModels {

    [RegionMemberLifetime(KeepAlive = false)]
    public class BxUiScreenViewModel : BindableBase, IDisposable {

        private readonly CompositeDisposable  fDisposables = new CompositeDisposable();
        void IDisposable.Dispose() { this.fDisposables.Dispose(); }

        public BxUiScreenViewModel( BxScreenModel screenModel ) {
            fScreenModel = screenModel;

            Loaded = new ReactiveCommand<RoutedEventArgs>().AddTo( fDisposables );
            Loaded.Subscribe (e => { OnLoaded( e ); });

            fScreenModel.RegGLControl( GLControl );
        }

        // -------------------

        private readonly BxScreenModel  fScreenModel;
        private BxUiScreen              fBxUiScreen;

        public ReactiveCommand<RoutedEventArgs>  Loaded { get; }

        private void  OnLoaded( RoutedEventArgs e )
        {
            fBxUiScreen = ( BxUiScreen )e.Source;

            RegActionGlPaint( fScreenModel.GLControl_Paint );
            fScreenModel.Screen_Loaded();
        }

        public GLControl  GLControl() {
            return fBxUiScreen.GLControl();
        }

        // -------------------

        private void  RegActionGlPaint( Action actionGlPaint )
        {
            fBxUiScreen.ActionGlPaint = actionGlPaint;
        }
    }
}
