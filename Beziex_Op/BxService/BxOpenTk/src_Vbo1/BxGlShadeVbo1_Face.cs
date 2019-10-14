using System;
using OpenTK;

namespace Beziex_Op {

    public class BxGlShadeVbo1_Face : BxGlShadeVbo1 {

        public  BxGlShadeVbo1_Face( Func<GLControl> glControl, BxGlProgContainer progContainer, BxGlMain parent ) : base( glControl, parent )
        {
            fProgramObj = ( BxGlProgramBase )progContainer.ViewProgVbo1_FaceObj();
        }
    }
}
