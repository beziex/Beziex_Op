using System;
using System.Reactive.Disposables;
using Prism.Mvvm;
using Prism.Regions;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;
using Beziex_Op.Model;

namespace Beziex_Op.ViewModels {

    [RegionMemberLifetime(KeepAlive = false)]
    public class BxUiTabGeneralViewModel : BindableBase, IDisposable {

        private readonly CompositeDisposable  fDisposables = new CompositeDisposable();
        void IDisposable.Dispose() { this.fDisposables.Dispose(); }

        public BxUiTabGeneralViewModel( BxTabGeneralModel tabGeneralModel )
        {
            fTabGeneralModel = tabGeneralModel;

            TextOpenFileName_Value    = fTabGeneralModel.TextOpenFileName_Value.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            SliderTess_Value          = fTabGeneralModel.SliderTess_Value.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            TextTess_Value            = fTabGeneralModel.TextTess_Value.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            TextBenchmark_Value       = fTabGeneralModel.TextBenchmark_Value.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            RadioFaceType_IsChecked   = fTabGeneralModel.RadioFaceType_IsChecked.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            RadioWireType_IsChecked   = fTabGeneralModel.RadioWireType_IsChecked.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            RadioTessShader_IsChecked = fTabGeneralModel.RadioTessShader_IsChecked.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            RadioInsGeom_IsChecked    = fTabGeneralModel.RadioInsGeom_IsChecked.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );

            ButtonOpenFileName_Click     = new ReactiveCommand().AddTo( fDisposables );
            SliderTess_Changed           = new ReactiveCommand().AddTo( fDisposables );
            ButtonBenchmarkAnalyze_Click = new ReactiveCommand().AddTo( fDisposables );
            ButtonBenchmarkClear_Click   = new ReactiveCommand().AddTo( fDisposables );
            RadioFaceType_Checked        = new ReactiveCommand().AddTo( fDisposables );
            RadioWireType_Checked        = new ReactiveCommand().AddTo( fDisposables );
            RadioTessShader_Checked      = new ReactiveCommand().AddTo( fDisposables );
            RadioInsGeom_Checked         = new ReactiveCommand().AddTo( fDisposables );

            ButtonOpenFileName_Click.Subscribe     (() => { fTabGeneralModel.ButtonOpenFileName_Click(); });
            SliderTess_Changed.Subscribe           (() => { fTabGeneralModel.SliderTess_Changed(); });
            ButtonBenchmarkAnalyze_Click.Subscribe (() => { fTabGeneralModel.ButtonBenchmarkAnalyze_Click(); });
            ButtonBenchmarkClear_Click.Subscribe   (() => { fTabGeneralModel.ButtonBenchmarkClear_Click(); });
            RadioFaceType_Checked.Subscribe        (() => { fTabGeneralModel.RadioFaceType_Checked(); });
            RadioWireType_Checked.Subscribe        (() => { fTabGeneralModel.RadioWireType_Checked(); });
            RadioTessShader_Checked.Subscribe      (() => { fTabGeneralModel.RadioTessShader_Checked(); });
            RadioInsGeom_Checked.Subscribe         (() => { fTabGeneralModel.RadioInsGeom_Checked(); });

            SliderTess_Enable       = fTabGeneralModel.SliderTess_Enable.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            ButtonBenchmark_Enable  = fTabGeneralModel.ButtonBenchmark_Enable.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            RadioFaceWire_Enable    = fTabGeneralModel.RadioFaceWire_Enable.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
            RadioTesselation_Enable = fTabGeneralModel.RadioTesselation_Enable.ToReactivePropertyAsSynchronized( val => val.Value ).AddTo( fDisposables );
        }

        // -------------------

        private readonly BxTabGeneralModel  fTabGeneralModel;

        public ReactiveProperty<string>  TextOpenFileName_Value    { get; set; }
        public ReactiveProperty<double>  SliderTess_Value          { get; set; }
        public ReactiveProperty<string>  TextTess_Value            { get; set; }
        public ReactiveProperty<string>  TextBenchmark_Value       { get; set; }
        public ReactiveProperty<bool>    RadioFaceType_IsChecked   { get; set; }
        public ReactiveProperty<bool>    RadioWireType_IsChecked   { get; set; }
        public ReactiveProperty<bool>    RadioTessShader_IsChecked { get; set; }
        public ReactiveProperty<bool>    RadioInsGeom_IsChecked    { get; set; }

        public ReactiveCommand  ButtonOpenFileName_Click     { get; }
        public ReactiveCommand  SliderTess_Changed           { get; }
        public ReactiveCommand  ButtonBenchmarkAnalyze_Click { get; }
        public ReactiveCommand  ButtonBenchmarkClear_Click   { get; }
        public ReactiveCommand  RadioFaceType_Checked        { get; }
        public ReactiveCommand  RadioWireType_Checked        { get; }
        public ReactiveCommand  RadioTessShader_Checked      { get; }
        public ReactiveCommand  RadioInsGeom_Checked         { get; }

        public ReactiveProperty<bool>  SliderTess_Enable       { get; set; }
        public ReactiveProperty<bool>  ButtonBenchmark_Enable  { get; set; }
        public ReactiveProperty<bool>  RadioFaceWire_Enable    { get; set; }
        public ReactiveProperty<bool>  RadioTesselation_Enable { get; set; }
    }
}
