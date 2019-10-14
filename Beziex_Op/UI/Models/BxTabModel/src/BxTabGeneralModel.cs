using System;
using System.IO;
using System.Reactive.Disposables;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Beziex_Op.Model {

    public class BxTabGeneralModel : IDisposable {

        private readonly CompositeDisposable  fDisposables = new CompositeDisposable();
        void IDisposable.Dispose() { this.fDisposables.Dispose(); }

        public BxTabGeneralModel( BxRoot objRoot, BxTabTransformModel tabTransform )
        {
            TextOpenFileName_Value    = new ReactivePropertySlim<string>( "" ).AddTo( fDisposables );
            SliderTess_Value          = new ReactivePropertySlim<double>( KSliderTess_Default ).AddTo( fDisposables );
            TextTess_Value            = new ReactivePropertySlim<string>( KSliderTess_Default.ToString() ).AddTo( fDisposables );
            TextBenchmark_Value       = new ReactivePropertySlim<string>( "" ).AddTo( fDisposables );
            RadioFaceType_IsChecked   = new ReactivePropertySlim<bool>( true ).AddTo( fDisposables );
            RadioWireType_IsChecked   = new ReactivePropertySlim<bool>( false ).AddTo( fDisposables );
            RadioTessShader_IsChecked = new ReactivePropertySlim<bool>( true ).AddTo( fDisposables );
            RadioInsGeom_IsChecked    = new ReactivePropertySlim<bool>( false ).AddTo( fDisposables );

            SliderTess_Enable       = new ReactivePropertySlim<bool>( false ).AddTo( fDisposables );
            ButtonBenchmark_Enable  = new ReactivePropertySlim<bool>( false ).AddTo( fDisposables );
            RadioFaceWire_Enable    = new ReactivePropertySlim<bool>( false ).AddTo( fDisposables );
            RadioTesselation_Enable = new ReactivePropertySlim<bool>( false ).AddTo( fDisposables );

            fObjRoot      = objRoot;
            fTabTransform = tabTransform;
        }

        // -----------------------------------------------------
 
        private readonly byte  KSliderTess_Default = 8;

        public ReactivePropertySlim<string>  TextOpenFileName_Value;
        public ReactivePropertySlim<double>  SliderTess_Value;
        public ReactivePropertySlim<string>  TextTess_Value;
        public ReactivePropertySlim<string>  TextBenchmark_Value;
        public ReactivePropertySlim<bool>    RadioFaceType_IsChecked;
        public ReactivePropertySlim<bool>    RadioWireType_IsChecked;
        public ReactivePropertySlim<bool>    RadioTessShader_IsChecked;
        public ReactivePropertySlim<bool>    RadioInsGeom_IsChecked;

        public ReactivePropertySlim<bool>  SliderTess_Enable;
        public ReactivePropertySlim<bool>  ButtonBenchmark_Enable;
        public ReactivePropertySlim<bool>  RadioFaceWire_Enable;
        public ReactivePropertySlim<bool>  RadioTesselation_Enable;

        private readonly BxRoot  fObjRoot = null;

        public void  ButtonOpenFileName_Click()
        {
            string  fileName;
            BxCmSeparatePatch_Object  patch;
            fObjRoot.ObjReadRoot().Exec( fObjRoot.Param(), out fileName, out patch );
            if( patch == null )
                return;

            TextOpenFileName_Value.Value = Path.GetFileName( fileName );

            InitCintrol();
            fTabTransform.InitCintrol();

            fObjRoot.SetPatch( patch );
            fObjRoot.GenBuf();
            fObjRoot.Draw();

            EnableControl();
            fTabTransform.EnableControl();
        }

        public void  SliderTess_Changed()
        {
            TextTess_Value.Value = ( ( int )SliderTess_Value.Value ).ToString();

            fObjRoot.Param().NumTess = ( byte )SliderTess_Value.Value;
            fObjRoot.Draw();
        }

        public void  ButtonBenchmarkAnalyze_Click()
        {
            TextBenchmark_Value.Value = fObjRoot.ObjGlRoot().ObjGlMain().ExecBenchmark( fObjRoot.Param() );

        }

        public void  ButtonBenchmarkClear_Click()
        {
            TextBenchmark_Value.Value = "";
        }

        public void  RadioFaceType_Checked()
        {
            fObjRoot.Param().IsWire = false;

            fObjRoot.ChangeShader();
            fObjRoot.GenBuf();
            fObjRoot.Draw();
        }

        public void  RadioWireType_Checked()
        {
            fObjRoot.Param().IsWire = true;

            fObjRoot.ChangeShader();
            fObjRoot.GenBuf();
            fObjRoot.Draw();
        }

        public void  RadioTessShader_Checked()
        {
            fObjRoot.Param().ShaderMode = BxCmUiParam.EnumShaderMode.KShaderMode_Vbo1;

            fObjRoot.ChangeShader();
            fObjRoot.GenBuf();
            fObjRoot.Draw();
        }

        public void  RadioInsGeom_Checked()
        {
            fObjRoot.Param().ShaderMode = BxCmUiParam.EnumShaderMode.KShaderMode_GL3;

            fObjRoot.ChangeShader();
            fObjRoot.GenBuf();
            fObjRoot.Draw();
        }

        // -----------------------------------------------------

        private readonly BxTabTransformModel  fTabTransform = null;

        public void  InitCintrol()
        {
            SliderTess_Value.Value = KSliderTess_Default;
            TextTess_Value.Value   = ( ( int )KSliderTess_Default ).ToString();

            TextBenchmark_Value.Value = "";

            RadioFaceType_IsChecked.Value = true;
            fObjRoot.Param().IsWire = false;

            RadioTessShader_IsChecked.Value = true;
            fObjRoot.Param().ShaderMode = BxCmUiParam.EnumShaderMode.KShaderMode_Vbo1;
            fObjRoot.ChangeShader();
        }

        private void EnableControl()
        {
            SliderTess_Enable.Value       = true;
            ButtonBenchmark_Enable.Value  = true;
            RadioFaceWire_Enable.Value    = true;
            RadioTesselation_Enable.Value = true;
        }
    }
}
