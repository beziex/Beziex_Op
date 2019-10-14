using System;
using System.Reactive.Disposables;
using Prism.Mvvm;
using Prism.Regions;

namespace Beziex_Op.ViewModels {

    [RegionMemberLifetime(KeepAlive = false)]
    public class BxUiTabViewModel : BindableBase, IDisposable {

        private readonly CompositeDisposable  fDisposables = new CompositeDisposable();
        void IDisposable.Dispose() { this.fDisposables.Dispose(); }

        public BxUiTabViewModel() {
        }
    }
}
