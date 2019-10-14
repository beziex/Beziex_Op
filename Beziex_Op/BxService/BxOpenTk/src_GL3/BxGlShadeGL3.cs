using System;
using System.Diagnostics;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Beziex_Op {

    abstract public class BxGlShadeGL3 : BxGlShaderBase {

        private const int     KNumFloatInVector4 = 4;
        protected const byte  fNumTessMax        = 32;

        protected uint          fNumSurface;
        private readonly int[]  fTextureID;
        protected uint          fNumSurface_NonPreDiv;

        // -------------------------------------------------------

        public BxGlShadeGL3( Func<GLControl> glControl, BxGlMain parent ) : base( glControl, parent )
        {
            fTextureID = new int[ 1 ];
            fTextureID[ 0 ] = int.MaxValue;
        }

        public override void  ReleaseBufInfo()
        {
            base.ReleaseBufInfo();

            ReleaseVtf();
        }

        // ------

        public override void  GenBuf( BxCmSeparatePatch_Object patch, BxCmUiParam param )
        {
            fNumSurface = patch.NumSurface;
            TessLevel   = param.NumTess;

            SetVertexBuffer( patch );
            SetIndexBuffer( patch );
            SetVAO();

            Vector3?  min = null;
            Vector3?  max = null;

            if( param.Min != null )
                min = new Vector3( ( float )param.Min.X, ( float )param.Min.Y, ( float )param.Min.Z );
            if( param.Max != null )
                max = new Vector3( ( float )param.Max.X, ( float )param.Max.Y, ( float )param.Max.Z );

            if( min != null && max != null )
                Parent.SetObjSize( ( Vector3 )min, ( Vector3 )max );

            InitVtf( patch );
        }

        public override void  Draw( BxCmUiParam param )
        {
            DrawVtf();

            base.Draw( param );

            GL.BindTexture( TextureTarget.Texture2D, 0 );
        }

        protected override void  SetVertexBuffer( BxCmSeparatePatch_Object patch )
        {
            if( Buf.HasVertexBuffer() == true )
                Buf.ReleaseVertexBuffer();

            Vector3[]  vertexAry;
            GenVertexAry( out vertexAry );
            Buf.NumVertices = vertexAry.Length;

            float[]  instanceAry = new float[ fNumSurface ];
            for( uint i=0; i<fNumSurface; i++ )
                instanceAry[ i ] = ( float )( i + 0.5 );

            GL.GenBuffers( 2, Buf.VboID );

            int  sizeVertexAry = Buf.NumVertices * Vector3.SizeInBytes;
            GL.BindBuffer( BufferTarget.ArrayBuffer, Buf.VboID[ 0 ] );
            GL.BufferData( BufferTarget.ArrayBuffer, new IntPtr( sizeVertexAry ), vertexAry, BufferUsageHint.StaticDraw );

            int  sizeInstanceAry = ( int )( fNumSurface * sizeof( float ) );
            GL.BindBuffer( BufferTarget.ArrayBuffer, Buf.VboID[ 1 ] );
            GL.BufferData( BufferTarget.ArrayBuffer, new IntPtr( sizeInstanceAry ), instanceAry, BufferUsageHint.StaticDraw );

            GL.BindBuffer( BufferTarget.ArrayBuffer, 0 );
        }

        protected void  SetIndexBuffer( BxCmSeparatePatch_Object patch )
        {
            if( Buf.HasIndexBuffer() == true )
                Buf.ReleaseIndexBuffer();

            int[]  indexAry;
            GenIndexAry( out indexAry );
            Buf.NumIndices = indexAry.Length;

            int  sizeIndexAry = Buf.NumIndices * sizeof( int );

            GL.GenBuffers( 1, Buf.EboID );

            GL.BindBuffer( BufferTarget.ElementArrayBuffer, Buf.EboID[ 0 ] );
            GL.BufferData( BufferTarget.ElementArrayBuffer, new IntPtr( sizeIndexAry ), indexAry, BufferUsageHint.StaticDraw );

            GL.BindBuffer( BufferTarget.ElementArrayBuffer, 0 );
        }

        protected override void  SetVAO()
        {
            GL.GenVertexArrays( 1, out Buf.VaoHandle[ 0 ] );
            GL.BindVertexArray( Buf.VaoHandle[ 0 ] );

            GL.BindBuffer( BufferTarget.ArrayBuffer, Buf.VboID[ 0 ] );

            int  vertexUVLocation = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertVertexUV" );
            GL.EnableVertexAttribArray( vertexUVLocation );
            GL.VertexAttribPointer( vertexUVLocation, 3, VertexAttribPointerType.Float, false, Vector3.SizeInBytes, 0 );

            GL.BindBuffer( BufferTarget.ArrayBuffer, Buf.VboID[ 1 ] );

            int  instanceIdLocation = GL.GetAttribLocation( fProgramObj.ShaderProgramID(), "vertInstanceID" );
            GL.EnableVertexAttribArray( instanceIdLocation );
            GL.VertexAttribPointer( instanceIdLocation, 1, VertexAttribPointerType.Float, false, sizeof( float ), 0 );

            GL.VertexAttribDivisor( 0, 0 );
            GL.VertexAttribDivisor( 1, 1 );

            GL.BindVertexArray( 0 );
        }

        protected override void  PatchParameter()
        {
        }

        // ------

        abstract protected void  DrawMain_NonPreDiv();
        abstract protected void  DrawMain_PreDiv();
        abstract protected void  GenIndexAry( out int[] indexAry );

        // ------

        protected override void  DrawMain( BxCmUiParam param )
        {
            SetUniformForTexture();
            SetFaceColor();

            GL.BindVertexArray( Buf.VaoHandle[ 0 ] );
            GL.BindBuffer( BufferTarget.ElementArrayBuffer, Buf.EboID[ 0 ] );
            BeginBenchmark( param );

            DrawMain_NonPreDiv();
            DrawMain_PreDiv();

            EndBenchmark( param );
            GL.BindVertexArray( 0 );
        }

        // ------

        protected void  SetUniform( uint surfaceOfs, int tessDenom )
        {
            int  surfaceOfsLocation = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "surfaceOfs" );
            GL.Uniform1( surfaceOfsLocation, ( float )surfaceOfs );

            int  tessDenomLocation = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "tessDenom" );
            GL.Uniform1( tessDenomLocation, ( float )tessDenom );
        }

        // ------

        private void  GenVertexAry( out Vector3[] vertexAry )
        {
            byte  numTessMaxP = fNumTessMax + 1;

            vertexAry = new Vector3[ numTessMaxP * numTessMaxP ];
            for( byte v=0; v<numTessMaxP; v++ ) {
                for( byte u=0; u<numTessMaxP; u++ ) {
                    ushort  n = ( ushort )( v * numTessMaxP + u );

                    vertexAry[ n ].X = u;
                    vertexAry[ n ].Y = v;
                    vertexAry[ n ].Z = 0.0f;
                }
            }
        }

        // ------

        public bool  HasVtf()
        {
            if( fTextureID[ 0 ] != int.MaxValue )
                return true;

            return false;
        }

        public void  ReleaseVtf()
        {
            GL.DeleteTextures( 1, fTextureID );
            fTextureID[ 0 ] = int.MaxValue;
        }

        // ------

        private void  InitVtf( BxCmSeparatePatch_Object patch )
        {
            InitVtf_Pre( patch, out int width, out int height, out float[] vtfAry );

            uint  cnt = 0;
            InitVtf_NonPreDiv( patch, width, height, vtfAry, ref cnt );

            fNumSurface_NonPreDiv = cnt;
            InitVtf_PreDiv( patch, width, height, vtfAry, ref cnt );

            Debug.Assert( cnt == patch.NumSurface );
            InitVtf_Post( width, height, vtfAry );
        }

        private void
        InitVtf_Pre( BxCmSeparatePatch_Object patch, out int width, out int height, out float[] vtfAry )
        {
            if( HasVtf() == true )
                ReleaseVtf();

            int  division = 20;

            width  = 80 * division;
            height = ( int )( ( patch.NumSurface + division - 1 ) / division );

            vtfAry = new float[ ( width * KNumFloatInVector4 ) * height ];
        }

        private void
        InitVtf_Post( int width, int height, float[] vtfAry )
        {
            GL.ActiveTexture( TextureUnit.Texture0 );
            GL.GenTextures( 1, fTextureID );
            GL.BindTexture( TextureTarget.Texture2D, fTextureID[ 0 ] );
            GL.PixelStore( PixelStoreParameter.UnpackAlignment, 1 );
            GL.TexImage2D( TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, width, height, 0, PixelFormat.Rgba, PixelType.Float, vtfAry );

            GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, ( int )TextureMagFilter.Nearest );
            GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, ( int )TextureMinFilter.Nearest );
            GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureWrapS,     ( int )TextureWrapMode.ClampToEdge );
            GL.TexParameter( TextureTarget.Texture2D, TextureParameterName.TextureWrapT,     ( int )TextureWrapMode.ClampToEdge );

            GL.BindTexture( TextureTarget.Texture2D, 0 );
        }

        private void  SetUniformForTexture()
        {
            int  vtfLocation        = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "vtf" );
            int  numSurfaceLocation = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "numSurface" );

            GL.Uniform1( vtfLocation, 0 );
            GL.Uniform1( numSurfaceLocation, ( float )fNumSurface );
        }

        // ------

        private void  ConvVtfInfo( BxBezier3Line3F hPosBez0, BxBezier6Line3F hPosBez1, BxBezier6Line3F hPosBez2, BxBezier3Line3F hPosBez3, BxBezier3Line3F vPosBez0, BxBezier6Line3F vPosBez1,
            BxBezier6Line3F vPosBez2, BxBezier3Line3F vPosBez3, BxBezier2Line3F hDiffBez0, BxBezier5Line3F hDiffBez1, BxBezier5Line3F hDiffBez2, BxBezier2Line3F hDiffBez3,
            BxBezier2Line3F vDiffBez0, BxBezier5Line3F vDiffBez1, BxBezier5Line3F vDiffBez2, BxBezier2Line3F vDiffBez3, uint surfaceOfs, float[] vtfInfo )
        {
            FromBezier3( hPosBez0,  surfaceOfs,  0, vtfInfo );
            FromBezier6( hPosBez1,  surfaceOfs,  4, vtfInfo );
            FromBezier6( hPosBez2,  surfaceOfs, 11, vtfInfo );
            FromBezier3( hPosBez3,  surfaceOfs, 18, vtfInfo );

            FromBezier3( vPosBez0,  surfaceOfs, 22, vtfInfo );
            FromBezier6( vPosBez1,  surfaceOfs, 26, vtfInfo );
            FromBezier6( vPosBez2,  surfaceOfs, 33, vtfInfo );
            FromBezier3( vPosBez3,  surfaceOfs, 40, vtfInfo );

            FromBezier2( hDiffBez0, surfaceOfs, 44, vtfInfo );
            FromBezier5( hDiffBez1, surfaceOfs, 47, vtfInfo );
            FromBezier5( hDiffBez2, surfaceOfs, 53, vtfInfo );
            FromBezier2( hDiffBez3, surfaceOfs, 59, vtfInfo );

            FromBezier2( vDiffBez0, surfaceOfs, 62, vtfInfo );
            FromBezier5( vDiffBez1, surfaceOfs, 65, vtfInfo );
            FromBezier5( vDiffBez2, surfaceOfs, 71, vtfInfo );
            FromBezier2( vDiffBez3, surfaceOfs, 77, vtfInfo );
        }

        private void  FromBezier2( BxBezier2Line3F src, uint surfaceOfs, byte ofsBase, float[] dst )
        {
            uint  texelOfs = ( uint )( ( surfaceOfs * 80 * KNumFloatInVector4 ) + ( ofsBase * KNumFloatInVector4 ) );

            for( byte i=0; i<3; i++ ) {
                dst[ texelOfs + i * KNumFloatInVector4 + 0 ] = src[ i ].X;
                dst[ texelOfs + i * KNumFloatInVector4 + 1 ] = src[ i ].Y;
                dst[ texelOfs + i * KNumFloatInVector4 + 2 ] = src[ i ].Z;
            }
        }

        private void  FromBezier3( BxBezier3Line3F src, uint surfaceOfs, byte ofsBase, float[] dst )
        {
            uint  texelOfs = ( uint )( ( surfaceOfs * 80 * KNumFloatInVector4 ) + ( ofsBase * KNumFloatInVector4 ) );

            for( byte i=0; i<4; i++ ) {
                dst[ texelOfs + i * KNumFloatInVector4 + 0 ] = src[ i ].X;
                dst[ texelOfs + i * KNumFloatInVector4 + 1 ] = src[ i ].Y;
                dst[ texelOfs + i * KNumFloatInVector4 + 2 ] = src[ i ].Z;
            }
        }

        private void  FromBezier5( BxBezier5Line3F src, uint surfaceOfs, byte ofsBase, float[] dst )
        {
            uint  texelOfs = ( uint )( ( surfaceOfs * 80 * KNumFloatInVector4 ) + ( ofsBase * KNumFloatInVector4 ) );

            for( byte i=0; i<6; i++ ) {
                dst[ texelOfs + i * KNumFloatInVector4 + 0 ] = src[ i ].X;
                dst[ texelOfs + i * KNumFloatInVector4 + 1 ] = src[ i ].Y;
                dst[ texelOfs + i * KNumFloatInVector4 + 2 ] = src[ i ].Z;
            }
        }

        private void  FromBezier6( BxBezier6Line3F src, uint surfaceOfs, byte ofsBase, float[] dst )
        {
            uint  texelOfs = ( uint )( ( surfaceOfs * 80 * KNumFloatInVector4 ) + ( ofsBase * KNumFloatInVector4 ) );

            for( byte i=0; i<7; i++ ) {
                dst[ texelOfs + i * KNumFloatInVector4 + 0 ] = src[ i ].X;
                dst[ texelOfs + i * KNumFloatInVector4 + 1 ] = src[ i ].Y;
                dst[ texelOfs + i * KNumFloatInVector4 + 2 ] = src[ i ].Z;
            }
        }

        private void  DrawVtf()
        {
            GL.ActiveTexture( TextureUnit.Texture0 );
            GL.BindTexture( TextureTarget.Texture2D, fTextureID[ 0 ] );
        }

        // ------

        private void  InitVtf_NonPreDiv( BxCmSeparatePatch_Object patch, int width, int height, float[] vtfAry, ref uint cnt )
        {
            for( uint i=0; i<patch.NumSurface; i++ ) {
                if( patch[ i ].PreDivided != true ) {
                    GetPosDiff( patch, i, out BxBezier3Line3F hPosBez0, out BxBezier6Line3F hPosBez1, out BxBezier6Line3F hPosBez2, out BxBezier3Line3F hPosBez3, out BxBezier3Line3F vPosBez0,
                        out BxBezier6Line3F vPosBez1, out BxBezier6Line3F vPosBez2, out BxBezier3Line3F vPosBez3, out BxBezier2Line3F hDiffBez0, out BxBezier5Line3F hDiffBez1,
                        out BxBezier5Line3F hDiffBez2, out BxBezier2Line3F hDiffBez3, out BxBezier2Line3F vDiffBez0, out BxBezier5Line3F vDiffBez1, out BxBezier5Line3F vDiffBez2,
                        out BxBezier2Line3F vDiffBez3 );

                    ConvVtfInfo( hPosBez0, hPosBez1, hPosBez2, hPosBez3, vPosBez0, vPosBez1, vPosBez2, vPosBez3, hDiffBez0, hDiffBez1, hDiffBez2, hDiffBez3, vDiffBez0, vDiffBez1,
                                 vDiffBez2, vDiffBez3, cnt, vtfAry );

                    cnt++;
                }
            }
        }

        private void  InitVtf_PreDiv( BxCmSeparatePatch_Object patch, int width, int height, float[] vtfAry, ref uint cnt )
        {
            for( uint i=0; i<patch.NumSurface; i++ ) {
                if( patch[ i ].PreDivided == true ) {
                    GetPosDiff( patch, i, out BxBezier3Line3F hPosBez0, out BxBezier6Line3F hPosBez1, out BxBezier6Line3F hPosBez2, out BxBezier3Line3F hPosBez3, out BxBezier3Line3F vPosBez0,
                        out BxBezier6Line3F vPosBez1, out BxBezier6Line3F vPosBez2, out BxBezier3Line3F vPosBez3, out BxBezier2Line3F hDiffBez0, out BxBezier5Line3F hDiffBez1,
                        out BxBezier5Line3F hDiffBez2, out BxBezier2Line3F hDiffBez3, out BxBezier2Line3F vDiffBez0, out BxBezier5Line3F vDiffBez1, out BxBezier5Line3F vDiffBez2,
                        out BxBezier2Line3F vDiffBez3 );

                    ConvVtfInfo( hPosBez0, hPosBez1, hPosBez2, hPosBez3, vPosBez0, vPosBez1, vPosBez2, vPosBez3, hDiffBez0, hDiffBez1, hDiffBez2, hDiffBez3, vDiffBez0, vDiffBez1,
                                 vDiffBez2, vDiffBez3, cnt, vtfAry );

                    cnt++;
                }
            }
        }

        // -------------------------------------------------------

        private void  GetPosDiff( BxCmSeparatePatch_Object patch, uint surfaceNo, out BxBezier3Line3F hPosBez0, out BxBezier6Line3F hPosBez1, out BxBezier6Line3F hPosBez2, out BxBezier3Line3F hPosBez3,
            out BxBezier3Line3F vPosBez0, out BxBezier6Line3F vPosBez1, out BxBezier6Line3F vPosBez2, out BxBezier3Line3F vPosBez3, out BxBezier2Line3F hDiffBez0, out BxBezier5Line3F hDiffBez1,
            out BxBezier5Line3F hDiffBez2, out BxBezier2Line3F hDiffBez3, out BxBezier2Line3F vDiffBez0, out BxBezier5Line3F vDiffBez1, out BxBezier5Line3F vDiffBez2, out BxBezier2Line3F vDiffBez3 )
        {
            GetPosBezierH( patch, surfaceNo, out hPosBez0, out hPosBez1, out hPosBez2, out hPosBez3 );
            GetPosBezierV( patch, surfaceNo, out vPosBez0, out vPosBez1, out vPosBez2, out vPosBez3 );
            GetDiffBezierH( patch, surfaceNo, out hDiffBez0, out hDiffBez1, out hDiffBez2, out hDiffBez3 );
            GetDiffBezierV( patch, surfaceNo, out vDiffBez0, out vDiffBez1, out vDiffBez2, out vDiffBez3 );
        }

        // ------

        private void  GetPosBezierH( BxCmSeparatePatch_Object src, uint surfaceNo, out BxBezier3Line3F hPosBez0, out BxBezier6Line3F hPosBez1, out BxBezier6Line3F hPosBez2, out BxBezier3Line3F hPosBez3 )
        {
            GetPosBezierMain( src, surfaceNo, 0, out hPosBez0, out hPosBez1, out hPosBez2, out hPosBez3 );
        }

        private void  GetPosBezierV( BxCmSeparatePatch_Object src, uint surfaceNo, out BxBezier3Line3F vPosBez0, out BxBezier6Line3F vPosBez1, out BxBezier6Line3F vPosBez2, out BxBezier3Line3F vPosBez3 )
        {
            GetPosBezierMain( src, surfaceNo, 1, out vPosBez0, out vPosBez1, out vPosBez2, out vPosBez3 );
        }

        private void  GetPosBezierMain( BxCmSeparatePatch_Object src, uint surfaceNo, byte hvId, out BxBezier3Line3F posBez0, out BxBezier6Line3F posBez1, out BxBezier6Line3F posBez2, out BxBezier3Line3F posBez3 )
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

        private void  GetDiffBezierH( BxCmSeparatePatch_Object src, uint surfaceNo, out BxBezier2Line3F hDiffBez0, out BxBezier5Line3F hDiffBez1, out BxBezier5Line3F hDiffBez2, out BxBezier2Line3F hDiffBez3 )
        {
            GetDiffBezierMain( src, surfaceNo, 0, out hDiffBez0, out hDiffBez1, out hDiffBez2, out hDiffBez3 );
        }

        private void  GetDiffBezierV( BxCmSeparatePatch_Object src, uint surfaceNo, out BxBezier2Line3F vDiffBez0, out BxBezier5Line3F vDiffBez1, out BxBezier5Line3F vDiffBez2, out BxBezier2Line3F vDiffBez3 )
        {
            GetDiffBezierMain( src, surfaceNo, 1, out vDiffBez0, out vDiffBez1, out vDiffBez2, out vDiffBez3 );
        }

        private void  GetDiffBezierMain( BxCmSeparatePatch_Object src, uint surfaceNo, byte hvId, out BxBezier2Line3F diffBez0, out BxBezier5Line3F diffBez1, out BxBezier5Line3F diffBez2, out BxBezier2Line3F diffBez3 )
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
    }
}
