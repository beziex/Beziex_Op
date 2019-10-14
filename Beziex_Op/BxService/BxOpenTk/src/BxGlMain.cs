using System;
using System.Diagnostics;
using OpenTK;

namespace Beziex_Op {

    public class BxGlMain : BxGlMainBase {

        private BxGlProgContainer         fProgContainer = null;

        private BxCmSeparatePatch_Object  fPatch = null;
        private BxGlShaderBase            fShade = null;
        private BxGlShaderBase            fWire  = null;

        // -------------------------------------------------------

        public BxGlMain( Func<GLControl> glControl, BxGlProgContainer progContainer ) : base( glControl )
        {
            fProgContainer = progContainer;

            ChangeShaderVbo1();
        }

        protected override void  SetModelData( BxCmUiParam param )
        {
            GenBufInner( param );
        }

        protected override void  GenBufMain( BxCmUiParam param )
        {
            GenBufInner( param );
        }

        protected override bool  HasBufferMain()
        {
            return ( fPatch != null );
        }

        // ------

        protected override void  ReleaseBufInfo()
        {
            if( fShade != null )
                fShade.ReleaseBufInfo();

            if( fWire != null )
                fWire.ReleaseBufInfo();
        }

        protected override bool?  IsEmptyMain()
        {
            return fShade?.IsEmpty();
        }

        protected override void  DrawMain( BxCmUiParam param )
        {
            if( fShade != null )
                fShade.Draw( param );

            if( fWire != null )
                fWire.Draw( param );
        }

        protected override void  ChangeShaderMain( BxCmUiParam param )
        {
            switch( param.ShaderMode ) {
            case BxCmUiParam.EnumShaderMode.KShaderMode_Vbo1:
                ChangeShaderVbo1();
                break;
            default:
                Debug.Assert( param.ShaderMode == BxCmUiParam.EnumShaderMode.KShaderMode_GL3 );
                ChangeShaderGL3();
                break;
            }
        }

        private void  ChangeShaderVbo1()
        {
            fShade = new BxGlShadeVbo1_Face( fGLControl, fProgContainer, this );
            fWire  = null;
        }

        private void  ChangeShaderGL3()
        {
            fShade = new BxGlShadeGL3_Face( fGLControl, fProgContainer, this );
            fWire  = null;
        }

        protected override void  ChangeShaderMain_Wire( BxCmUiParam param )
        {
            switch( param.ShaderMode ) {
            case BxCmUiParam.EnumShaderMode.KShaderMode_Vbo1:
                ChangeShaderVbo1_Wire();
                break;
            default:
                Debug.Assert( param.ShaderMode == BxCmUiParam.EnumShaderMode.KShaderMode_GL3 );
                ChangeShaderGL3_Wire();
                break;
            }
        }

        private void  ChangeShaderVbo1_Wire()
        {
            fShade = null;
            fWire  = new BxGlShadeVbo1_Wire( fGLControl, fProgContainer, this );
        }

        private void  ChangeShaderGL3_Wire()
        {
            fShade = null;
            fWire  = new BxGlShadeGL3_Wire( fGLControl, fProgContainer, this );
        }

        protected override void  SetPatchMain( BxCmSeparatePatch_Object patch )
        {
            fPatch = patch;
        }

        // -------------------------------------------------------

        private void  GenBufInner( BxCmUiParam param )
        {
            if( fPatch == null )
                return;

            BxCmSeparatePatch_Object  correctPatch;
            CorrectProc( fPatch, out correctPatch );

            if( fShade != null )
                fShade.GenBuf( correctPatch, param );

            if( fWire != null )
                fWire.GenBuf( correctPatch, param );
        }

        private void  CorrectProc( BxCmSeparatePatch_Object src, out BxCmSeparatePatch_Object dst )
        {
            BxCmCorrectSeparator  objCorrect = new BxCmCorrectSeparator();
            objCorrect.Exec( src, out dst );
        }
    }
}
