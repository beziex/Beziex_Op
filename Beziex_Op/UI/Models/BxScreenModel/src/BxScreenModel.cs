using System;
using System.Reactive.Disposables;
using OpenTK;

namespace Beziex_Op.Model {

    public class BxScreenModel : IDisposable {

        private readonly CompositeDisposable  fDisposables = new CompositeDisposable();
        void IDisposable.Dispose() { this.fDisposables.Dispose(); }

        public BxScreenModel( BxRoot objRoot )
        {
            fObjRoot = objRoot;
        }

        // ------------------------------

        private readonly BxRoot  fObjRoot   = null;
        private Func<GLControl>  fGLControl = null;

        public void  Screen_Loaded()
        {
            fObjRoot.Init( GLControl );
        }

        public void  RegGLControl( Func<GLControl> glControl )
        {
            fGLControl = glControl;
        }

        public GLControl  GLControl()
        {
            return fGLControl();
        }

        public void  GLControl_Paint()
        {
            fObjRoot.Draw();
        }
    }
}
