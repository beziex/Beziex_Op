using System;
using System.Reactive.Disposables;
using Reactive.Bindings;
using Reactive.Bindings.Extensions;

namespace Beziex_Op.Model {

    public class BxTabTransformModel : IDisposable {

        private readonly CompositeDisposable  fDisposables = new CompositeDisposable();
        void IDisposable.Dispose() { this.fDisposables.Dispose(); }

        public BxTabTransformModel( BxRoot objRoot )
        {
            SliderRotH_Value  = new ReactivePropertySlim<double>( 0.5 ).AddTo( fDisposables );
            SliderRotV_Value  = new ReactivePropertySlim<double>( 0.5 ).AddTo( fDisposables );
            SliderRotR_Value  = new ReactivePropertySlim<double>( 0.5 ).AddTo( fDisposables );
            SliderMovH_Value  = new ReactivePropertySlim<double>( 0.5 ).AddTo( fDisposables );
            SliderMovV_Value  = new ReactivePropertySlim<double>( 0.5 ).AddTo( fDisposables );
            SliderScale_Value = new ReactivePropertySlim<double>( 0.5 ).AddTo( fDisposables );

            Transform_Enable = new ReactivePropertySlim<bool>( false ).AddTo( fDisposables );

            fObjRoot = objRoot;
        }

        // -----------------------------------------------------

        public ReactivePropertySlim<double>  SliderRotH_Value;
        public ReactivePropertySlim<double>  SliderRotV_Value;
        public ReactivePropertySlim<double>  SliderRotR_Value;
        public ReactivePropertySlim<double>  SliderMovH_Value;
        public ReactivePropertySlim<double>  SliderMovV_Value;
        public ReactivePropertySlim<double>  SliderScale_Value;

        public ReactivePropertySlim<bool>  Transform_Enable;

        private readonly BxRoot  fObjRoot = null;

        private const int  kFocusRotUnknown = -1;
        private const int  kFocusRotH = 0;
        private const int  kFocusRotV = 1;
        private const int  kFocusRotR = 2;

        private int  fFocusRot = kFocusRotUnknown;

        // -----------------------------------------------------

        public void  ButtonRotDefault_Click()
        {
            InitRot();
            fObjRoot.Draw();
        }

        public void  SliderRotH_Changed()
        {
            if( fFocusRot != kFocusRotH ) {
                fFocusRot = kFocusRotH;

                FixMatRot();
                SliderRotV_Value.Value = 0.5;
                SliderRotR_Value.Value = 0.5;
            }

            SetMatrixAndDraw();
        }

        public void  SliderRotV_Changed()
        {
            if( fFocusRot != kFocusRotV ) {
                fFocusRot = kFocusRotV;

                FixMatRot();
                SliderRotH_Value.Value = 0.5;
                SliderRotR_Value.Value = 0.5;
            }

            SetMatrixAndDraw();
        }

        public void  SliderRotR_Changed()
        {
            if( fFocusRot != kFocusRotR ) {
                fFocusRot = kFocusRotR;

                FixMatRot();
                SliderRotH_Value.Value = 0.5;
                SliderRotV_Value.Value = 0.5;
            }

            SetMatrixAndDraw();
        }

        public void  ButtonMovDefault_Click()
        {
            InitMove();
            fObjRoot.Draw();
        }

        public void  SliderMovH_Changed()
        {
            SetMatrixAndDraw();
        }

        public void  SliderMovV_Changed()
        {
            SetMatrixAndDraw();
        }

        public void  ButtonScaleDefault_Click()
        {
            InitScale();
            fObjRoot.Draw();
        }

        public void  SliderScale_Changed()
        {
            SetMatrixAndDraw();
        }

        // -----------------------------------------------------
 
        private void  SetMatrixAndDraw()
        {
            GetSliderValue( out float valRotH, out float valRotV, out float valRotAxisZ, out float valMoveH, out float valMoveV, out float valScale );
            fObjRoot.ObjGlRoot().ObjGlMain().SliderMatrix( valRotH, valRotV, valRotAxisZ, valMoveH, valMoveV, valScale );

            fObjRoot.Draw();
        }

        private void  InitRot()
        {
            fFocusRot = kFocusRotUnknown;
            InitMatRot();

            SliderRotH_Value.Value = 0.5;
            SliderRotV_Value.Value = 0.5;
            SliderRotR_Value.Value = 0.5;
        }

        private void  InitMove()
        {
            InitMatMove();

            SliderMovH_Value.Value = 0.5;
            SliderMovV_Value.Value = 0.5;
        }

        private void  InitScale()
        {
            InitMatScale();

            SliderScale_Value.Value = 0.5;
        }

        // -----------------------------------------------------

        private void  InitMatRot()
        {
            fObjRoot.ObjGlRoot().ObjGlMain().InitMatrix_Rot();
        }

        private void  InitMatMove()
        {
            fObjRoot.ObjGlRoot().ObjGlMain().InitMatrix_Move();
        }

        private void  InitMatScale()
        {
            fObjRoot.ObjGlRoot().ObjGlMain().InitMatrix_Scale();
        }

        private void  FixMatRot()
        {
            fObjRoot.ObjGlRoot().ObjGlMain().FixMatRot();
        }

        // -----------------------------------------------------

        public void  GetSliderValue( out float valRotH, out float valRotV, out float valRotAxisZ, out float valMoveH, out float valMoveV,
            out float valScale )
        {
            valRotH     = ( float )SliderRotH_Value.Value;
            valRotV     = ( float )SliderRotV_Value.Value;
            valRotAxisZ = ( float )SliderRotR_Value.Value;
            valMoveH    = ( float )SliderMovH_Value.Value;
            valMoveV    = ( float )SliderMovV_Value.Value;
            valScale    = ( float )SliderScale_Value.Value;
        }

        // -----------------------------------------------------

        public void  InitCintrol()
        {
            InitRot();
            InitMove();
            InitScale();
        }

        public void EnableControl()
        {
            Transform_Enable.Value = true;
        }
    }
}
