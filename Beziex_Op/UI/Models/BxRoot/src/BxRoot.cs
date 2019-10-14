using System;
using OpenTK;

namespace Beziex_Op.Model {

    public class BxRoot {

        private BxGlRoot     fObjGlRoot;
        private BxCmUiParam  fParam;
        private BxReadRoot   fObjReadRoot;
        private bool         fIsInitialized;

        private Func<GLControl>  fGLControl = null;

        public BxRoot()
        {
            fObjGlRoot     = null;
            fParam         = null;
            fObjReadRoot   = null;
            fIsInitialized = false;
        }

        public void  Init( Func<GLControl> glControl )
        {
            fGLControl = glControl;

            fObjGlRoot   = new BxGlRoot( GLControl );
            fParam       = new BxCmUiParam();
            fObjReadRoot = new BxReadRoot();

            fObjGlRoot.Init( fParam );
            fIsInitialized = true;
        }

        public void  Draw()
        {
            if( fIsInitialized )
                fObjGlRoot.Draw( fParam );
        }

        public void  ChangeShader()
        {
            if( fIsInitialized )
                fObjGlRoot.ChangeShader( fParam );
        }

        public void  SetPatch( BxCmSeparatePatch_Object patch )
        {
            fObjGlRoot.SetPatch( patch );
        }

        public void  GenBuf()
        {
            if( fIsInitialized )
                fObjGlRoot.GenBuf( fParam );
        }

        public GLControl  GLControl()
        {
            return fGLControl();
        }

        public BxCmUiParam  Param()
        {
            return fParam;
        }

        public BxReadRoot  ObjReadRoot()
        {
            return fObjReadRoot;
        }

        public BxGlRoot  ObjGlRoot()
        {
            return fObjGlRoot;
        }
    }
}
