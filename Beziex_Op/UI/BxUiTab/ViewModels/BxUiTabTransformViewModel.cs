using System;
using System.Reactive.Disposables;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Beziex_Op.Model;

namespace Beziex_Op.ViewModels {

    [RegionMemberLifetime(KeepAlive = false)]
    public class BxUiTabTransformViewModel : BindableBase, IDisposable {

        private readonly CompositeDisposable  fDisposables = new CompositeDisposable();
        void IDisposable.Dispose() { this.fDisposables.Dispose(); }

        public BxUiTabTransformViewModel( BxTabTransformModel tabTransformModel )
        {
            fTabTransformModel = tabTransformModel;

            SliderRotH_Value  = fTabTransformModel.SliderRotH_Value.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            SliderRotV_Value  = fTabTransformModel.SliderRotV_Value.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            SliderRotR_Value  = fTabTransformModel.SliderRotR_Value.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            SliderMovH_Value  = fTabTransformModel.SliderMovH_Value.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            SliderMovV_Value  = fTabTransformModel.SliderMovV_Value.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            SliderScale_Value = fTabTransformModel.SliderScale_Value.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );

            ButtonRotDefault_Click   = new ReactiveCommand().AddTo( fDisposables );
            SliderRotH_Changed       = new ReactiveCommand().AddTo( fDisposables );
            SliderRotV_Changed       = new ReactiveCommand().AddTo( fDisposables );
            SliderRotR_Changed       = new ReactiveCommand().AddTo( fDisposables );
            ButtonMovDefault_Click   = new ReactiveCommand().AddTo( fDisposables );
            SliderMovH_Changed       = new ReactiveCommand().AddTo( fDisposables );
            SliderMovV_Changed       = new ReactiveCommand().AddTo( fDisposables );
            ButtonScaleDefault_Click = new ReactiveCommand().AddTo( fDisposables );
            SliderScale_Changed      = new ReactiveCommand().AddTo( fDisposables );

            ButtonRotDefault_Click.Subscribe   (() => { fTabTransformModel.ButtonRotDefault_Click(); });
            SliderRotH_Changed.Subscribe       (() => { fTabTransformModel.SliderRotH_Changed(); });
            SliderRotV_Changed.Subscribe       (() => { fTabTransformModel.SliderRotV_Changed(); });
            SliderRotR_Changed.Subscribe       (() => { fTabTransformModel.SliderRotR_Changed(); });
            ButtonMovDefault_Click.Subscribe   (() => { fTabTransformModel.ButtonMovDefault_Click(); });
            SliderMovH_Changed.Subscribe       (() => { fTabTransformModel.SliderMovH_Changed(); });
            SliderMovV_Changed.Subscribe       (() => { fTabTransformModel.SliderMovV_Changed(); });
            ButtonScaleDefault_Click.Subscribe (() => { fTabTransformModel.ButtonScaleDefault_Click(); });
            SliderScale_Changed.Subscribe      (() => { fTabTransformModel.SliderScale_Changed(); });

            Transform_Enable = fTabTransformModel.Transform_Enable.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
        }

        // -------------------

        private readonly BxTabTransformModel  fTabTransformModel;

        public ReactiveProperty<double>  SliderRotH_Value  { get; set; }
        public ReactiveProperty<double>  SliderRotV_Value  { get; set; }
        public ReactiveProperty<double>  SliderRotR_Value  { get; set; }
        public ReactiveProperty<double>  SliderMovH_Value  { get; set; }
        public ReactiveProperty<double>  SliderMovV_Value  { get; set; }
        public ReactiveProperty<double>  SliderScale_Value { get; set; }

        public ReactiveCommand  ButtonRotDefault_Click   { get; }
        public ReactiveCommand  SliderRotH_Changed       { get; }
        public ReactiveCommand  SliderRotV_Changed       { get; }
        public ReactiveCommand  SliderRotR_Changed       { get; }
        public ReactiveCommand  ButtonMovDefault_Click   { get; }
        public ReactiveCommand  SliderMovH_Changed       { get; }
        public ReactiveCommand  SliderMovV_Changed       { get; }
        public ReactiveCommand  ButtonScaleDefault_Click { get; }
        public ReactiveCommand  SliderScale_Changed      { get; }

        public ReactiveProperty<bool>  Transform_Enable { get; set; }
    }
}
