using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Beziex_Op {

    public class BxGlShadeGL3_Face : BxGlShadeGL3 {

        public BxGlShadeGL3_Face( Func<GLControl> glControl, BxGlProgContainer progContainer, BxGlMain parent ) : base( glControl, parent )
        {
            fProgramObj = ( BxGlProgramBase )progContainer.ViewProgGL3_FaceObj();
        }

        // ------

        protected override void  DrawMain_NonPreDiv()
        {
            if( fNumSurface_NonPreDiv == 0 )
                return;

            SetUniform( 0, 1 );

            int  m = TessLevel * TessLevel * 3 * 2;
            GL.DrawElementsInstanced( PrimitiveType.Triangles, m, DrawElementsType.UnsignedInt, IntPtr.Zero, ( int )fNumSurface_NonPreDiv );
        }

        protected override void  DrawMain_PreDiv()
        {
            uint  numSurface_PreDiv = fNumSurface - fNumSurface_NonPreDiv;
            if( numSurface_PreDiv == 0 )
                return;

            SetUniform( fNumSurface_NonPreDiv, 2 );

            byte  tessCount = ( byte )( TessLevel / 2 );

            int  m = tessCount * tessCount * 3 * 2;
            GL.DrawElementsInstanced( PrimitiveType.Triangles, m, DrawElementsType.UnsignedInt, IntPtr.Zero, ( int )numSurface_PreDiv );
        }

        // ------

        protected override void  GenIndexAry( out int[] indexAry )
        {
            indexAry = new int[ fNumTessMax * fNumTessMax * 3 * 2 ];

            ushort  start = 0;
            byte    numTessMaxP = ( byte )( fNumTessMax + 1 );

            for( byte i=0; i<fNumTessMax; i++ ) {
                for( byte u=0; u<i; u++ ) {
                    GenIndexAryMain( indexAry, numTessMaxP, start, u, i );
                    start += 6;
                }

                for( byte v=0; v<i; v++ ) {
                    GenIndexAryMain( indexAry, numTessMaxP, start, i, v );
                    start += 6;
                }

                GenIndexAryMain( indexAry, numTessMaxP, start, i, i );
                start += 6;
            }
        }

        private void  GenIndexAryMain( int[] indexAry, byte numTessP, ushort start, byte u, byte v )
        {
            ushort  vNo0 = ( ushort )( ( v + 0 ) * numTessP + ( u + 0 ) );
            ushort  vNo1 = ( ushort )( ( v + 0 ) * numTessP + ( u + 1 ) );
            ushort  vNo2 = ( ushort )( ( v + 1 ) * numTessP + ( u + 1 ) );
            ushort  vNo3 = ( ushort )( ( v + 1 ) * numTessP + ( u + 0 ) );

            indexAry[ start + 0 ] = vNo0;
            indexAry[ start + 1 ] = vNo1;
            indexAry[ start + 2 ] = vNo2;

            indexAry[ start + 3 ] = vNo2;
            indexAry[ start + 4 ] = vNo3;
            indexAry[ start + 5 ] = vNo0;
        }
    }
}
