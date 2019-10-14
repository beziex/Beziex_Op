using System;
using OpenTK;

namespace Beziex_Op {

    public class BxGlRoot {

        private BxGlMain                  fObjGlMain     = null;
        private BxGlProgContainer         fProgContainer = null;
        private readonly Func<GLControl>  fGLControl     = null;

        public BxGlRoot( Func<GLControl> glControl )
        {
            fGLControl = glControl;
        }

        public void  Init( BxCmUiParam param )
        {
            if( fGLControl != null ) {
                fProgContainer = NewProgContainer();
                fObjGlMain     = new BxGlMain( fGLControl, fProgContainer );

                fObjGlMain.Draw( param );
            }
        }

        public void  Draw( BxCmUiParam param )
        {
            fObjGlMain.Draw( param );
        }

        public void  ChangeShader( BxCmUiParam param )
        {
            fObjGlMain.ChangeShader( param );
        }

        public void  SetPatch( BxCmSeparatePatch_Object patch )
        {
            fObjGlMain.SetPatch( patch );
        }

        public void  GenBuf( BxCmUiParam param )
        {
            fObjGlMain.GenBuf( param );
        }

        public GLControl  GLControl() {
            return fGLControl();
        }

        public BxGlMain  ObjGlMain() {
            return fObjGlMain;
        }

        // --------------------------

        private BxGlProgContainer  NewProgContainer()
        {
            BxGlProgramObject  viewProgGL3_FaceObj  = new BxGlProgGL3_Face();
            BxGlProgramObject  viewProgGL3_WireObj  = new BxGlProgGL3_Wire();
            BxGlProgramObject  viewProgVbo1_FaceObj = new BxGlProgVbo1_Face();
            BxGlProgramObject  viewProgVbo1_WireObj = new BxGlProgVbo1_Wire();

            return new BxGlProgContainer( viewProgGL3_FaceObj, viewProgGL3_WireObj, viewProgVbo1_FaceObj, viewProgVbo1_WireObj );
        }
    }
}
