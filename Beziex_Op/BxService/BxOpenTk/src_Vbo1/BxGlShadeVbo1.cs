using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Beziex_Op {

    abstract public class BxGlShadeVbo1 : BxGlShaderBase {

        protected struct VertexInfo
        {
            public Vector3  pnt0;
            public Vector3  pnt1;
            public Vector3  pnt2;
            public Vector3  pnt3;
            public Vector3  pnt4;
            public Vector3  pnt5;
            public Vector3  pnt6;
            public Vector3  pnt7;
            public Vector3  pnt8;
            public Vector3  pnt9;
            public Vector3  pnt10;
            public Vector3  pnt11;
            public Vector3  pnt12;
            public Vector3  pnt13;

            public static readonly int  Length = Marshal.SizeOf( default( VertexInfo ) );

            public void  SetCtrlPos( int idx, Vector3 val )
            {
                switch( idx ) {
                case 0:  pnt0  = val; break;
                case 1:  pnt1  = val; break;
                case 2:  pnt2  = val; break;
                case 3:  pnt3  = val; break;
                case 4:  pnt4  = val; break;
                case 5:  pnt5  = val; break;
                case 6:  pnt6  = val; break;
                case 7:  pnt7  = val; break;
                case 8:  pnt8  = val; break;
                case 9:  pnt9  = val; break;
                case 10: pnt10 = val; break;
                case 11: pnt11 = val; break;
                case 12: pnt12 = val; break;
                 default:
                    Debug.Assert( idx == 13 );
                    pnt13 = val;
                    break;
                }
            }
        }

        // -------------------------------------------------------

        public  BxGlShadeVbo1( Func<GLControl> glControl, BxGlMain parent ) : base( glControl, parent )
        {
        }

        // ------

        protected override void  SetVertexBuffer( BxCmSeparatePatch_Object patch )
        {
            if( Buf.HasVertexBuffer() == true )
                Buf.ReleaseVertexBuffer();

            Buf.NumVertices = ( int )( patch.NumSurface * 6 );

            VertexInfo[] vertexAry = new VertexInfo[ Buf.NumVertices ];
            for( uint i=0; i<patch.NumSurface; i++ ) {
                GetPosDiff( patch, i, out BxBezier3Line3F hPosBez0, out BxBezier6Line3F hPosBez1, out BxBezier6Line3F hPosBez2, out BxBezier3Line3F hPosBez3, out BxBezier3Line3F vPosBez0,
                    out BxBezier6Line3F vPosBez1, out BxBezier6Line3F vPosBez2, out BxBezier3Line3F vPosBez3, out BxBezier2Line3F hDiffBez0, out BxBezier5Line3F hDiffBez1,
                    out BxBezier5Line3F hDiffBez2, out BxBezier2Line3F hDiffBez3, out BxBezier2Line3F vDiffBez0, out BxBezier5Line3F vDiffBez1, out BxBezier5Line3F vDiffBez2,
                    out BxBezier2Line3F vDiffBez3 );

                int  tessDenom = GetTessDenom( patch, i );

                SetVertexBufferOne( hPosBez0, hPosBez1, hPosBez2, hPosBez3, vPosBez0, vPosBez1, vPosBez2, vPosBez3, hDiffBez0, hDiffBez1, hDiffBez2, hDiffBez3, vDiffBez0, vDiffBez1,
                                    vDiffBez2, vDiffBez3, tessDenom, i, vertexAry );
            }

            GL.GenBuffers( 1, Buf.VboID );

            GL.BindBuffer( BufferTarget.ArrayBuffer, Buf.VboID[ 0 ] );
            GL.BufferData( BufferTarget.ArrayBuffer, new IntPtr( Buf.NumVertices * VertexInfo.Length ), vertexAry, BufferUsageHint.StaticDraw );

            GL.BindBuffer( BufferTarget.ArrayBuffer, 0 );
        }

        protected override void  SetVAO()
        {
            GL.GenVertexArrays( 1, out Buf.VaoHandle[ 0 ] );
            GL.BindVertexArray( Buf.VaoHandle[ 0 ] );

            GL.BindBuffer( BufferTarget.ArrayBuffer, Buf.VboID[ 0 ] );

            int  pnt0Location  = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt0"  );
            int  pnt1Location  = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt1"  );
            int  pnt2Location  = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt2"  );
            int  pnt3Location  = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt3"  );
            int  pnt4Location  = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt4"  );
            int  pnt5Location  = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt5"  );
            int  pnt6Location  = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt6"  );
            int  pnt7Location  = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt7"  );
            int  pnt8Location  = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt8"  );
            int  pnt9Location  = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt9"  );
            int  pnt10Location = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt10" );
            int  pnt11Location = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt11" );
            int  pnt12Location = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt12" );
            int  pnt13Location = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertPnt13" );

            GL.EnableVertexAttribArray( pnt0Location  );
            GL.EnableVertexAttribArray( pnt1Location  );
            GL.EnableVertexAttribArray( pnt2Location  );
            GL.EnableVertexAttribArray( pnt3Location  );
            GL.EnableVertexAttribArray( pnt4Location  );
            GL.EnableVertexAttribArray( pnt5Location  );
            GL.EnableVertexAttribArray( pnt6Location  );
            GL.EnableVertexAttribArray( pnt7Location  );
            GL.EnableVertexAttribArray( pnt8Location  );
            GL.EnableVertexAttribArray( pnt9Location  );
            GL.EnableVertexAttribArray( pnt10Location );
            GL.EnableVertexAttribArray( pnt11Location );
            GL.EnableVertexAttribArray( pnt12Location );
            GL.EnableVertexAttribArray( pnt13Location );

            GL.VertexAttribPointer( pnt0Location,  3, VertexAttribPointerType.Float, false, VertexInfo.Length, 0 );
            GL.VertexAttribPointer( pnt1Location,  3, VertexAttribPointerType.Float, false, VertexInfo.Length, ( Vector3.SizeInBytes * 1  ) );
            GL.VertexAttribPointer( pnt2Location,  3, VertexAttribPointerType.Float, false, VertexInfo.Length, ( Vector3.SizeInBytes * 2  ) );
            GL.VertexAttribPointer( pnt3Location,  3, VertexAttribPointerType.Float, false, VertexInfo.Length, ( Vector3.SizeInBytes * 3  ) );
            GL.VertexAttribPointer( pnt4Location,  3, VertexAttribPointerType.Float, false, VertexInfo.Length, ( Vector3.SizeInBytes * 4  ) );
            GL.VertexAttribPointer( pnt5Location,  3, VertexAttribPointerType.Float, false, VertexInfo.Length, ( Vector3.SizeInBytes * 5  ) );
            GL.VertexAttribPointer( pnt6Location,  3, VertexAttribPointerType.Float, false, VertexInfo.Length, ( Vector3.SizeInBytes * 6  ) );
            GL.VertexAttribPointer( pnt7Location,  3, VertexAttribPointerType.Float, false, VertexInfo.Length, ( Vector3.SizeInBytes * 7  ) );
            GL.VertexAttribPointer( pnt8Location,  3, VertexAttribPointerType.Float, false, VertexInfo.Length, ( Vector3.SizeInBytes * 8  ) );
            GL.VertexAttribPointer( pnt9Location,  3, VertexAttribPointerType.Float, false, VertexInfo.Length, ( Vector3.SizeInBytes * 9  ) );
            GL.VertexAttribPointer( pnt10Location, 3, VertexAttribPointerType.Float, false, VertexInfo.Length, ( Vector3.SizeInBytes * 10 ) );
            GL.VertexAttribPointer( pnt11Location, 3, VertexAttribPointerType.Float, false, VertexInfo.Length, ( Vector3.SizeInBytes * 11 ) );
            GL.VertexAttribPointer( pnt12Location, 3, VertexAttribPointerType.Float, false, VertexInfo.Length, ( Vector3.SizeInBytes * 12 ) );
            GL.VertexAttribPointer( pnt13Location, 3, VertexAttribPointerType.Float, false, VertexInfo.Length, ( Vector3.SizeInBytes * 13 ) );

            GL.BindVertexArray( 0 );
        }

        protected override void  PatchParameter()
        {
            GL.PatchParameter( PatchParameterInt.PatchVertices, 6 );
        }

        // ------

        private void  SetVertexBufferOne(
            BxBezier3Line3F hPosBez0, BxBezier6Line3F hPosBez1, BxBezier6Line3F hPosBez2, BxBezier3Line3F hPosBez3, BxBezier3Line3F vPosBez0, BxBezier6Line3F vPosBez1,
            BxBezier6Line3F vPosBez2, BxBezier3Line3F vPosBez3, BxBezier2Line3F hDiffBez0, BxBezier5Line3F hDiffBez1, BxBezier5Line3F hDiffBez2, BxBezier2Line3F hDiffBez3,
            BxBezier2Line3F vDiffBez0, BxBezier5Line3F vDiffBez1, BxBezier5Line3F vDiffBez2, BxBezier2Line3F vDiffBez3, int tessDenom, uint surfaceNo, VertexInfo[] vertexAry )
        {
            SetVertexBuffer0( hPosBez0,  hPosBez1,  hDiffBez0, surfaceNo, vertexAry );
            SetVertexBuffer1( hPosBez2,  hPosBez3,  hDiffBez3, surfaceNo, vertexAry );
            SetVertexBuffer2( hDiffBez1, hDiffBez2, tessDenom, surfaceNo, vertexAry );
            SetVertexBuffer3( vPosBez0,  vPosBez1,  vDiffBez0, surfaceNo, vertexAry );
            SetVertexBuffer4( vPosBez2,  vPosBez3,  vDiffBez3, surfaceNo, vertexAry );
            SetVertexBuffer5( vDiffBez1, vDiffBez2, surfaceNo, vertexAry );
        }

        private void  SetVertexBuffer0( BxBezier3Line3F hPosBez0, BxBezier6Line3F hPosBez1, BxBezier2Line3F hDiffBez0, uint surfaceNo, VertexInfo[] vertexAry )
        {
            uint  dstIndex = ( surfaceNo * 6 ) + 0;

            ToVector3( hPosBez0[ 0 ],  ref vertexAry[ dstIndex ].pnt0  );
            ToVector3( hPosBez0[ 1 ],  ref vertexAry[ dstIndex ].pnt1  );
            ToVector3( hPosBez0[ 2 ],  ref vertexAry[ dstIndex ].pnt2  );
            ToVector3( hPosBez0[ 3 ],  ref vertexAry[ dstIndex ].pnt3  );

            ToVector3( hPosBez1[ 0 ],  ref vertexAry[ dstIndex ].pnt4  );
            ToVector3( hPosBez1[ 1 ],  ref vertexAry[ dstIndex ].pnt5  );
            ToVector3( hPosBez1[ 2 ],  ref vertexAry[ dstIndex ].pnt6  );
            ToVector3( hPosBez1[ 3 ],  ref vertexAry[ dstIndex ].pnt7  );
            ToVector3( hPosBez1[ 4 ],  ref vertexAry[ dstIndex ].pnt8  );
            ToVector3( hPosBez1[ 5 ],  ref vertexAry[ dstIndex ].pnt9  );
            ToVector3( hPosBez1[ 6 ],  ref vertexAry[ dstIndex ].pnt10 );

            ToVector3( hDiffBez0[ 0 ], ref vertexAry[ dstIndex ].pnt11 );
            ToVector3( hDiffBez0[ 1 ], ref vertexAry[ dstIndex ].pnt12 );
            ToVector3( hDiffBez0[ 2 ], ref vertexAry[ dstIndex ].pnt13 );
        }

        private void  SetVertexBuffer1( BxBezier6Line3F hPosBez2, BxBezier3Line3F hPosBez3, BxBezier2Line3F hDiffBez3, uint surfaceNo, VertexInfo[] vertexAry )
        {
            uint  dstIndex = ( surfaceNo * 6 ) + 1;

            ToVector3( hPosBez2[ 0 ],  ref vertexAry[ dstIndex ].pnt0  );
            ToVector3( hPosBez2[ 1 ],  ref vertexAry[ dstIndex ].pnt1  );
            ToVector3( hPosBez2[ 2 ],  ref vertexAry[ dstIndex ].pnt2  );
            ToVector3( hPosBez2[ 3 ],  ref vertexAry[ dstIndex ].pnt3  );
            ToVector3( hPosBez2[ 4 ],  ref vertexAry[ dstIndex ].pnt4  );
            ToVector3( hPosBez2[ 5 ],  ref vertexAry[ dstIndex ].pnt5  );
            ToVector3( hPosBez2[ 6 ],  ref vertexAry[ dstIndex ].pnt6  );

            ToVector3( hPosBez3[ 0 ],  ref vertexAry[ dstIndex ].pnt7  );
            ToVector3( hPosBez3[ 1 ],  ref vertexAry[ dstIndex ].pnt8  );
            ToVector3( hPosBez3[ 2 ],  ref vertexAry[ dstIndex ].pnt9  );
            ToVector3( hPosBez3[ 3 ],  ref vertexAry[ dstIndex ].pnt10 );

            ToVector3( hDiffBez3[ 0 ], ref vertexAry[ dstIndex ].pnt11 );
            ToVector3( hDiffBez3[ 1 ], ref vertexAry[ dstIndex ].pnt12 );
            ToVector3( hDiffBez3[ 2 ], ref vertexAry[ dstIndex ].pnt13 );
        }

        private void  SetVertexBuffer2( BxBezier5Line3F hDiffBez1, BxBezier5Line3F hDiffBez2, int tessDenom, uint surfaceNo, VertexInfo[] vertexAry )
        {
            uint  dstIndex = ( surfaceNo * 6 ) + 2;

            ToVector3( hDiffBez1[ 0 ], ref vertexAry[ dstIndex ].pnt0  );
            ToVector3( hDiffBez1[ 1 ], ref vertexAry[ dstIndex ].pnt1  );
            ToVector3( hDiffBez1[ 2 ], ref vertexAry[ dstIndex ].pnt2  );
            ToVector3( hDiffBez1[ 3 ], ref vertexAry[ dstIndex ].pnt3  );
            ToVector3( hDiffBez1[ 4 ], ref vertexAry[ dstIndex ].pnt4  );
            ToVector3( hDiffBez1[ 5 ], ref vertexAry[ dstIndex ].pnt5  );

            ToVector3( hDiffBez2[ 0 ], ref vertexAry[ dstIndex ].pnt6  );
            ToVector3( hDiffBez2[ 1 ], ref vertexAry[ dstIndex ].pnt7  );
            ToVector3( hDiffBez2[ 2 ], ref vertexAry[ dstIndex ].pnt8  );
            ToVector3( hDiffBez2[ 3 ], ref vertexAry[ dstIndex ].pnt9  );
            ToVector3( hDiffBez2[ 4 ], ref vertexAry[ dstIndex ].pnt10 );
            ToVector3( hDiffBez2[ 5 ], ref vertexAry[ dstIndex ].pnt11 );

            ToVector3( tessDenom, ref vertexAry[ dstIndex ].pnt12 );
        }

        private void  SetVertexBuffer3( BxBezier3Line3F vPosBez0, BxBezier6Line3F vPosBez1, BxBezier2Line3F vDiffBez0, uint surfaceNo, VertexInfo[] vertexAry )
        {
            uint  dstIndex = ( surfaceNo * 6 ) + 3;

            ToVector3( vPosBez0[ 0 ],  ref vertexAry[ dstIndex ].pnt0  );
            ToVector3( vPosBez0[ 1 ],  ref vertexAry[ dstIndex ].pnt1  );
            ToVector3( vPosBez0[ 2 ],  ref vertexAry[ dstIndex ].pnt2  );
            ToVector3( vPosBez0[ 3 ],  ref vertexAry[ dstIndex ].pnt3  );

            ToVector3( vPosBez1[ 0 ],  ref vertexAry[ dstIndex ].pnt4  );
            ToVector3( vPosBez1[ 1 ],  ref vertexAry[ dstIndex ].pnt5  );
            ToVector3( vPosBez1[ 2 ],  ref vertexAry[ dstIndex ].pnt6  );
            ToVector3( vPosBez1[ 3 ],  ref vertexAry[ dstIndex ].pnt7  );
            ToVector3( vPosBez1[ 4 ],  ref vertexAry[ dstIndex ].pnt8  );
            ToVector3( vPosBez1[ 5 ],  ref vertexAry[ dstIndex ].pnt9  );
            ToVector3( vPosBez1[ 6 ],  ref vertexAry[ dstIndex ].pnt10 );

            ToVector3( vDiffBez0[ 0 ], ref vertexAry[ dstIndex ].pnt11 );
            ToVector3( vDiffBez0[ 1 ], ref vertexAry[ dstIndex ].pnt12 );
            ToVector3( vDiffBez0[ 2 ], ref vertexAry[ dstIndex ].pnt13 );
        }

        private void  SetVertexBuffer4( BxBezier6Line3F vPosBez2, BxBezier3Line3F vPosBez3, BxBezier2Line3F vDiffBez3, uint surfaceNo, VertexInfo[] vertexAry )
        {
            uint  dstIndex = ( surfaceNo * 6 ) + 4;

            ToVector3( vPosBez2[ 0 ],  ref vertexAry[ dstIndex ].pnt0  );
            ToVector3( vPosBez2[ 1 ],  ref vertexAry[ dstIndex ].pnt1  );
            ToVector3( vPosBez2[ 2 ],  ref vertexAry[ dstIndex ].pnt2  );
            ToVector3( vPosBez2[ 3 ],  ref vertexAry[ dstIndex ].pnt3  );
            ToVector3( vPosBez2[ 4 ],  ref vertexAry[ dstIndex ].pnt4  );
            ToVector3( vPosBez2[ 5 ],  ref vertexAry[ dstIndex ].pnt5  );
            ToVector3( vPosBez2[ 6 ],  ref vertexAry[ dstIndex ].pnt6  );

            ToVector3( vPosBez3[ 0 ],  ref vertexAry[ dstIndex ].pnt7  );
            ToVector3( vPosBez3[ 1 ],  ref vertexAry[ dstIndex ].pnt8  );
            ToVector3( vPosBez3[ 2 ],  ref vertexAry[ dstIndex ].pnt9  );
            ToVector3( vPosBez3[ 3 ],  ref vertexAry[ dstIndex ].pnt10 );

            ToVector3( vDiffBez3[ 0 ], ref vertexAry[ dstIndex ].pnt11 );
            ToVector3( vDiffBez3[ 1 ], ref vertexAry[ dstIndex ].pnt12 );
            ToVector3( vDiffBez3[ 2 ], ref vertexAry[ dstIndex ].pnt13 );
        }

        private void  SetVertexBuffer5( BxBezier5Line3F vDiffBez1, BxBezier5Line3F vDiffBez2, uint surfaceNo, VertexInfo[] vertexAry )
        {
            uint  dstIndex = ( surfaceNo * 6 ) + 5;

            ToVector3( vDiffBez1[ 0 ], ref vertexAry[ dstIndex ].pnt0  );
            ToVector3( vDiffBez1[ 1 ], ref vertexAry[ dstIndex ].pnt1  );
            ToVector3( vDiffBez1[ 2 ], ref vertexAry[ dstIndex ].pnt2  );
            ToVector3( vDiffBez1[ 3 ], ref vertexAry[ dstIndex ].pnt3  );
            ToVector3( vDiffBez1[ 4 ], ref vertexAry[ dstIndex ].pnt4  );
            ToVector3( vDiffBez1[ 5 ], ref vertexAry[ dstIndex ].pnt5  );

            ToVector3( vDiffBez2[ 0 ], ref vertexAry[ dstIndex ].pnt6  );
            ToVector3( vDiffBez2[ 1 ], ref vertexAry[ dstIndex ].pnt7  );
            ToVector3( vDiffBez2[ 2 ], ref vertexAry[ dstIndex ].pnt8  );
            ToVector3( vDiffBez2[ 3 ], ref vertexAry[ dstIndex ].pnt9  );
            ToVector3( vDiffBez2[ 4 ], ref vertexAry[ dstIndex ].pnt10 );
            ToVector3( vDiffBez2[ 5 ], ref vertexAry[ dstIndex ].pnt11 );
        }

        private void  ToVector3( BxVec3F src, ref Vector3 dst )
        {
            dst.X = src.X;
            dst.Y = src.Y;
            dst.Z = src.Z;
        }

        private void  ToVector3( int tessScale, ref Vector3 dst )
        {
            dst.X = tessScale;
            dst.Y = 0.0f;
            dst.Z = 0.0f;
        }

        // -------------------------------------------------------

        private void  GetPosDiff(
            BxCmSeparatePatch_Object patch, uint surfaceNo, out BxBezier3Line3F hPosBez0, out BxBezier6Line3F hPosBez1, out BxBezier6Line3F hPosBez2, out BxBezier3Line3F hPosBez3,
            out BxBezier3Line3F vPosBez0, out BxBezier6Line3F vPosBez1, out BxBezier6Line3F vPosBez2, out BxBezier3Line3F vPosBez3, out BxBezier2Line3F hDiffBez0,
            out BxBezier5Line3F hDiffBez1, out BxBezier5Line3F hDiffBez2, out BxBezier2Line3F hDiffBez3, out BxBezier2Line3F vDiffBez0, out BxBezier5Line3F vDiffBez1,
            out BxBezier5Line3F vDiffBez2, out BxBezier2Line3F vDiffBez3 )
        {
            GetPosBezierH( patch, surfaceNo, out hPosBez0, out hPosBez1, out hPosBez2, out hPosBez3 );
            GetPosBezierV( patch, surfaceNo, out vPosBez0, out vPosBez1, out vPosBez2, out vPosBez3 );
            GetDiffBezierH( patch, surfaceNo, out hDiffBez0, out hDiffBez1, out hDiffBez2, out hDiffBez3 );
            GetDiffBezierV( patch, surfaceNo, out vDiffBez0, out vDiffBez1, out vDiffBez2, out vDiffBez3 );
        }

        // ------

        private void  GetPosBezierH(
            BxCmSeparatePatch_Object src, uint surfaceNo, out BxBezier3Line3F hPosBez0, out BxBezier6Line3F hPosBez1, out BxBezier6Line3F hPosBez2, out BxBezier3Line3F hPosBez3 )
        {
            GetPosBezierMain( src, surfaceNo, 0, out hPosBez0, out hPosBez1, out hPosBez2, out hPosBez3 );
        }

        private void  GetPosBezierV(
            BxCmSeparatePatch_Object src, uint surfaceNo, out BxBezier3Line3F vPosBez0, out BxBezier6Line3F vPosBez1, out BxBezier6Line3F vPosBez2, out BxBezier3Line3F vPosBez3 )
        {
            GetPosBezierMain( src, surfaceNo, 1, out vPosBez0, out vPosBez1, out vPosBez2, out vPosBez3 );
        }

        private void  GetPosBezierMain(
            BxCmSeparatePatch_Object src, uint surfaceNo, byte hvId, out BxBezier3Line3F posBez0, out BxBezier6Line3F posBez1, out BxBezier6Line3F posBez2, out BxBezier3Line3F posBez3 )
        {
            byte  hvOfs, crossIdx;

            hvOfs    = 0;
            crossIdx = 0;
            GetPosBezierOuter( src, surfaceNo, hvId, hvOfs, crossIdx, out posBez0 );
            GetPosBezierInner( src, surfaceNo, hvId, hvOfs, out posBez1 );

            hvOfs    = 1;
            crossIdx = 6;
            GetPosBezierOuter( src, surfaceNo, hvId, hvOfs, crossIdx, out posBez3 );
            GetPosBezierInner( src, surfaceNo, hvId, hvOfs, out posBez2 );
        }

        private void  GetPosBezierOuter( BxCmSeparatePatch_Object src, uint surfaceNo, byte hvId, byte hvOfs, byte crossIdx, out BxBezier3Line3F posBez )
        {
            byte  vNo0, vNo1;
            HVtoVertexNo( hvId, hvOfs, out vNo0, out vNo1 );

            byte  wingHvId = ( byte )( 1 - hvId );

            posBez = new BxBezier3Line3F();
            posBez[ 0 ].Set( src[ surfaceNo ].Vertex[ vNo0 ].Pos );
            posBez[ 3 ].Set( src[ surfaceNo ].Vertex[ vNo1 ].Pos );
            posBez[ 1 ].Set( src[ surfaceNo ].SurfaceEdge[ wingHvId ][ 0 ].Inner[ crossIdx ] );
            posBez[ 2 ].Set( src[ surfaceNo ].SurfaceEdge[ wingHvId ][ 1 ].Inner[ crossIdx ] );
        }

        private void  HVtoVertexNo( byte hvId, byte hvOfs, out byte vertexNo0, out byte vertexNo1 )
        {
            if( hvId == 0 ) {
                if( hvOfs == 0 ) {
                    vertexNo0 = 0;
                    vertexNo1 = 1;
                }
                else {
                    Debug.Assert( hvOfs == 1 );
                    vertexNo0 = 3;
                    vertexNo1 = 2;
                }
            }
            else {
                Debug.Assert( hvId == 1 );
                if( hvOfs == 0 ) {
                    vertexNo0 = 0;
                    vertexNo1 = 3;
                }
                else {
                    Debug.Assert( hvOfs == 1 );
                    vertexNo0 = 1;
                    vertexNo1 = 2;
                }
            }
        }

        private void  GetPosBezierInner( BxCmSeparatePatch_Object src, uint surfaceNo, byte hvId, byte hvOfs, out BxBezier6Line3F posBezInner )
        {
            posBezInner = src[ surfaceNo ].SurfaceEdge[ hvId ][ hvOfs ].Inner.Copy;
        }

        // ------

        private void  GetDiffBezierH(
            BxCmSeparatePatch_Object src, uint surfaceNo, out BxBezier2Line3F hDiffBez0, out BxBezier5Line3F hDiffBez1, out BxBezier5Line3F hDiffBez2, out BxBezier2Line3F hDiffBez3 )
        {
            GetDiffBezierMain( src, surfaceNo, 0, out hDiffBez0, out hDiffBez1, out hDiffBez2, out hDiffBez3 );
        }

        private void  GetDiffBezierV(
            BxCmSeparatePatch_Object src, uint surfaceNo, out BxBezier2Line3F vDiffBez0, out BxBezier5Line3F vDiffBez1, out BxBezier5Line3F vDiffBez2, out BxBezier2Line3F vDiffBez3 )
        {
            GetDiffBezierMain( src, surfaceNo, 1, out vDiffBez0, out vDiffBez1, out vDiffBez2, out vDiffBez3 );
        }

        private void  GetDiffBezierMain(
            BxCmSeparatePatch_Object src, uint surfaceNo, byte hvId, out BxBezier2Line3F diffBez0, out BxBezier5Line3F diffBez1, out BxBezier5Line3F diffBez2, out BxBezier2Line3F diffBez3 )
        {
            byte  hvOfs, crossIdx;

            hvOfs    = 0;
            crossIdx = 0;
            GetDiffBezierOuter( src, surfaceNo, hvId, hvOfs, crossIdx, out diffBez0 );
            GetDiffBezierInner( src, surfaceNo, hvId, hvOfs, out diffBez1 );

            hvOfs    = 1;
            crossIdx = 6;
            GetDiffBezierOuter( src, surfaceNo, hvId, hvOfs, crossIdx, out diffBez3 );
            GetDiffBezierInner( src, surfaceNo, hvId, hvOfs, out diffBez2 );
        }

        private void  GetDiffBezierOuter( BxCmSeparatePatch_Object src, uint surfaceNo, byte hvId, byte hvOfs, byte crossIdx, out BxBezier2Line3F diffBez )
        {
            byte  vNo0, vNo1;
            HVtoVertexNo( hvId, hvOfs, out vNo0, out vNo1 );

            byte  wingHvId = ( byte )( 1 - hvId );

            BxBezier3Line3F  posBez = new BxBezier3Line3F();
            posBez[ 0 ].Set( src[ surfaceNo ].Vertex[ vNo0 ].Pos );
            posBez[ 3 ].Set( src[ surfaceNo ].Vertex[ vNo1 ].Pos );
            posBez[ 1 ].Set( src[ surfaceNo ].SurfaceEdge[ wingHvId ][ 0 ].Inner[ crossIdx ] );
            posBez[ 2 ].Set( src[ surfaceNo ].SurfaceEdge[ wingHvId ][ 1 ].Inner[ crossIdx ] );

            diffBez = posBez.Diff();

            Debug.Assert( diffBez[ 0 ].Length >= ( BxMathF.KNearZeroF * 10.0 ) );
            Debug.Assert( diffBez[ 2 ].Length >= ( BxMathF.KNearZeroF * 10.0 ) );
        }

        private void  GetDiffBezierInner( BxCmSeparatePatch_Object src, uint surfaceNo, byte hvId, byte hvOfs, out BxBezier5Line3F diffBezInner )
        {
            diffBezInner = src[ surfaceNo ].SurfaceEdge[ hvId ][ hvOfs ].Inner.Diff();
        }

        // -------------------------------------------------------

        private int  GetTessDenom( BxCmSeparatePatch_Object src, uint surfaceNo )
        {
            int  tessDenom = 1;
            if( src[ surfaceNo ].PreDivided == true )
                tessDenom = 2;

            return tessDenom;
        }
    }
}
