using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Beziex_Op {

    public class BxGlShadeGL3_Wire : BxGlShadeGL3 {

        public BxGlShadeGL3_Wire( Func<GLControl> glControl, BxGlProgContainer progContainer, BxGlMain parent ) : base( glControl, parent )
        {
            fProgramObj = ( BxGlProgramBase )progContainer.ViewProgGL3_WireObj();
        }

        // ------

        protected override void  DrawMain_NonPreDiv()
        {
            if( fNumSurface_NonPreDiv == 0 )
                return;

            SetUniform( 0, 1 );

            int  m = TessLevel * TessLevel * 5 * 2;
            GL.DrawElementsInstanced( PrimitiveType.Lines, m, DrawElementsType.UnsignedInt, IntPtr.Zero, ( int )fNumSurface_NonPreDiv );
        }

        protected override void  DrawMain_PreDiv()
        {
            uint  numSurface_PreDiv = fNumSurface - fNumSurface_NonPreDiv;
            if( numSurface_PreDiv == 0 )
                return;

            SetUniform( fNumSurface_NonPreDiv, 2 );

            byte  tessCount = ( byte )( TessLevel / 2 );

            int  m = tessCount * tessCount * 5 * 2;
            GL.DrawElementsInstanced( PrimitiveType.Lines, m, DrawElementsType.UnsignedInt, IntPtr.Zero, ( int )numSurface_PreDiv );
        }

        // ------

        protected override void  GenIndexAry( out int[] indexAry )
        {
            indexAry = new int[ fNumTessMax * fNumTessMax * 5 * 2 ];

            ushort  start = 0;
            byte    numTessMaxP = ( byte )( fNumTessMax + 1 );

            for( byte i=0; i<fNumTessMax; i++ ) {
                for( byte u=0; u<i; u++ ) {
                    GenIndexAryMain( indexAry, numTessMaxP, start, u, i );
                    start += 10;
                }

                for( byte v=0; v<i; v++ ) {
                    GenIndexAryMain( indexAry, numTessMaxP, start, i, v );
                    start += 10;
                }

                GenIndexAryMain( indexAry, numTessMaxP, start, i, i );
                start += 10;
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

            indexAry[ start + 2 ] = vNo1;
            indexAry[ start + 3 ] = vNo2;

            indexAry[ start + 4 ] = vNo2;
            indexAry[ start + 5 ] = vNo3;

            indexAry[ start + 6 ] = vNo3;
            indexAry[ start + 7 ] = vNo0;

            indexAry[ start + 8 ] = vNo2;
            indexAry[ start + 9 ] = vNo0;
        }
    }
}
