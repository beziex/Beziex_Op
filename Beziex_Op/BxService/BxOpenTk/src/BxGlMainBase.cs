using System;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Beziex_Op {

    abstract public class BxGlMainBase {

        abstract protected void   SetModelData( BxCmUiParam param );
        abstract protected void   GenBufMain( BxCmUiParam param );
        abstract protected bool   HasBufferMain();
        abstract protected void   ReleaseBufInfo();
        abstract protected bool?  IsEmptyMain();
        abstract protected void   DrawMain( BxCmUiParam param );
        abstract protected void   ChangeShaderMain( BxCmUiParam param );
        abstract protected void   ChangeShaderMain_Wire( BxCmUiParam param );
        abstract protected void   SetPatchMain( BxCmSeparatePatch_Object patch );

        // -------------------------------------------------------

        private const float  KPi            = 3.141592f;
        private const float  KDispSizeRatio = 2.5f;

        private float    fObjSize;
        private Vector3  fObjCenter;
        private Matrix4  fMatScaleSlider;
        private Matrix4  fMatMoveSlider;
        private Matrix4  fMatRotSlider;
        private Matrix4  fMatRotSlider_Org;

        // -------------------------------------------------------

        protected Func<GLControl>  fGLControl = null;

        private bool  fInitDrawFlag;
        public  bool  InitReadFlag { get; set; }

        private int  fBenchmarkID;

        // -------------------------------------------------------

        public BxGlMainBase( Func<GLControl> glControl )
        {
            fGLControl = glControl;

            fInitDrawFlag = false;
            InitReadFlag  = false;

            InitMatrix();
        }

        public void  Release( bool releasing )
        {
            if( releasing == true )
                ReleaseBufInfo();
        }

        // ------

        public GLControl  GLControl()
        {
            return fGLControl();
        }

        public void  Draw( BxCmUiParam param )
        {
            if( fInitDrawFlag == false ) {
                InitParams();
                fInitDrawFlag = true;
            }

            if( InitReadFlag == false ) {
                SetModelData( param );
                InitReadFlag = true;
            }

            if( IsEmpty() == true ) {
                ClrScreen();
                GLControl().SwapBuffers();
            }
            else {
                ClrScreen();
                DrawMain( param );
                GLControl().SwapBuffers();
            }
        }

        public void  ChangeShader( BxCmUiParam param )
        {
            if (param.IsWire)
                ChangeShaderMain_Wire( param );
            else
                ChangeShaderMain( param );
        }

        public void SetPatch( BxCmSeparatePatch_Object patch )
        {
            SetPatchMain( patch );
        }

        public void  GenBuf( BxCmUiParam param )
        {
            GenBufMain( param );
        }

        public bool  HasBuffer()
        {
            return HasBufferMain();
        }

        public void  SliderMatrix( float valRotH, float valRotV, float valRotAxisZ, float valMoveH, float valMoveV, float valScale )
        {
            SliderMatrix_Scale( valScale );
            SliderMatrix_Move( valMoveH, valMoveV );
            SliderMatrix_Rot( valRotH, valRotV, -valRotAxisZ );
        }

        public void  FixMatRot()
        {
            fMatRotSlider_Org = fMatRotSlider;
        }

        public bool?  IsEmpty()
        {
            return IsEmptyMain();
        }

        // ------

        public void  InitMatrix()
        {
            fObjCenter[ 0 ] = 0.0f;		fObjCenter[ 1 ] = 0.0f;		fObjCenter[ 2 ] = 0.0f;

            InitMatrix_Scale();
            InitMatrix_Move();
            InitMatrix_Rot();
        }

        public void  InitMatrix_Scale()
        {
            fMatScaleSlider = Matrix4.Identity;
        }

        public void  InitMatrix_Move()
        {
            fMatMoveSlider = Matrix4.Identity;
        }

        public void  InitMatrix_Rot()
        {
            fMatRotSlider     = Matrix4.Identity;
            fMatRotSlider_Org = Matrix4.Identity;
        }

        // ------

        public void  ClrScreen()
        {
            Color4  color = new Color4( 211, 211, 211, 255 );
            GL.ClearColor( color );

            GL.Clear( ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit );
        }

        // -------------------------------------------------------

        public void  GetMinMax( BxCmSeparatePatch_Object patch, out Vector3 min, out Vector3 max )
        {
            min = new Vector3();
            max = new Vector3();

            for( byte i=0; i<3; i++ )
                min[ i ] = max[ i ] = patch[ 0 ].Vertex[ 0 ].Pos[ i ];

            for( uint i=1; i<patch.NumSurface; i++ ) {
                for( byte j=0; j<4; j++ ) {
                    for( byte k=0; k<3; k++ ) {
                        if( min[ k ] > patch[ i ].Vertex[ j ].Pos[ k ] )
                            min[ k ] = patch[ i ].Vertex[ j ].Pos[ k ];
                        if( max[ k ] < patch[ i ].Vertex[ j ].Pos[ k ] )
                            max[ k ] = patch[ i ].Vertex[ j ].Pos[ k ];
                    }
                }
            }
        }

        public void  GetMinMax( float[] vertices, uint numSizeCalcVertices, out Vector3 min, out Vector3 max )
        {
            min = new Vector3();
            max = new Vector3();

            for( byte i=0; i<3; i++ )
                min[ i ] = max[ i ] = vertices[ i ];

            uint  numVertices = numSizeCalcVertices / 3;
            for( uint i=1; i<numVertices; i++ ) {
                for( byte j=0; j<3; j++ ) {
                    uint  k = i * 3 + j;
                    if( min[ j ] > vertices[ k ] )
                        min[ j ] = vertices[ k ];
                    if( max[ j ] < vertices[ k ] )
                        max[ j ] = vertices[ k ];
                }
            }
        }

        public void  SetObjSize( Vector3 min, Vector3 max )
        {
            float[]  tmp = new float[ 3 ];

            for( byte i=0; i<3; i++ ) {
                fObjCenter[ i ] = ( min[ i ] + max[ i ] ) / 2.0f;
                tmp[ i ] = max[ i ] - min[ i ];
            }

            fObjSize = ( float )Math.Sqrt( tmp[0]*tmp[0] + tmp[1]*tmp[1] + tmp[2]*tmp[2] );
        }

        // -------------------------------------------------------

        public Matrix4  TransformMatrix()
        {
            Matrix4  matMoveFit = Matrix4.CreateTranslation( -fObjCenter[ 0 ], -fObjCenter[ 1 ], -fObjCenter[ 2 ] );

            float   scale = 2.0f * KDispSizeRatio / fObjSize;

            Matrix4  matScaleFit = Matrix4.CreateScale( scale, scale, scale );

            return matMoveFit * fMatRotSlider * fMatMoveSlider * fMatScaleSlider * matScaleFit;
        }

        public Matrix3  NormalMatrix()
        {
            return new Matrix3( fMatRotSlider );
        }

        // -------------------------------------------------------

        private void  InitParams()
        {
            GL.Disable( EnableCap.CullFace );

            GL.Enable( EnableCap.DepthTest );
            GL.DepthFunc( DepthFunction.Lequal );

            GL.EnableClientState( ArrayCap.VertexArray );
            GL.EnableClientState( ArrayCap.NormalArray );
            GL.EnableClientState( ArrayCap.ColorArray );

            GL.Enable( EnableCap.Normalize );

            GL.Enable( EnableCap.ColorMaterial );
            GL.ColorMaterial( MaterialFace.FrontAndBack, ColorMaterialParameter.Diffuse );
        }

        // -------------------------------------------------------

        private void  SliderMatrix_Scale( float valScale )
        {
            float  scale;
            if( valScale >= 0.5f ) {
                float  t = ( valScale - 0.5f ) * 2.0f;
                scale = ( 1.0f - t ) * 1.0f + t * 10.0f;
            }
            else {
                float  t = valScale * 2.0f;
                scale = ( 1.0f - t ) * 0.1f + t * 1.0f;
            }

            fMatScaleSlider = Matrix4.CreateScale( scale, scale, scale );
        }

        private void  SliderMatrix_Move( float valMoveH, float valMoveV )
        {
            float  moveLen = fObjSize / KDispSizeRatio;

            float  moveH = ( 1.0f - valMoveH ) * -moveLen + valMoveH * moveLen;
            float  moveV = ( 1.0f - valMoveV ) * -moveLen + valMoveV * moveLen;

            fMatMoveSlider = Matrix4.CreateTranslation( moveH, moveV, 0.0f );
        }

        private void  SliderMatrix_Rot( float valRotH, float valRotV, float valRotAxisZ )
        {
            float  rotV     = ( 1.0f - valRotV     ) * -KPi + valRotV     *  KPi;
            float  rotH     = ( 1.0f - valRotH     ) *  KPi + valRotH     * -KPi;
            float  rotAxisZ = ( 1.0f - valRotAxisZ ) *  KPi + valRotAxisZ * -KPi;

            Matrix4  matRotV     = Matrix4.CreateRotationX( -rotV );
            Matrix4  matRotH     = Matrix4.CreateRotationY( -rotH );
            Matrix4  matRotAxisZ = Matrix4.CreateRotationZ( -rotAxisZ );

            fMatRotSlider = fMatRotSlider_Org * matRotAxisZ * matRotH * matRotV;
        }

        // -------------------------------------------------------

        public void  BeginBenchmark( BxCmUiParam param )
        {
            if( param.BenchmarkStarted == false )
                return;

            GL.GenQueries( 1, out fBenchmarkID );
            GL.BeginQuery( QueryTarget.TimeElapsed, fBenchmarkID );
        }

        public void  EndBenchmark( BxCmUiParam param )
        {
            if( param.BenchmarkStarted == false )
                return;

            GL.EndQuery( QueryTarget.TimeElapsed );

            int done = 0;
            while( done == 0 ) {
                GL.GetQueryObject( fBenchmarkID, GetQueryObjectParam.QueryResultAvailable, out done );
            }

            long  elapsedTime;
            GL.GetQueryObject( fBenchmarkID, GetQueryObjectParam.QueryResult, out elapsedTime );
            GL.DeleteQueries( 1, ref fBenchmarkID );

            param.ElapsedTime = elapsedTime;
        }

        public string  ExecBenchmark( BxCmUiParam param )
        {
            param.BenchmarkStarted = true;
            param.ElapsedTime      = 0;

            DrawMain( param );

            param.BenchmarkStarted = false;
            string fps = GetFPS( param.ElapsedTime );

            param.ElapsedTime = 0;

            return fps;
        }

        private string  GetFPS( long elapsedTime )
        {
            double  fps = 1.0 / ( elapsedTime * 1e-9 );

            return fps.ToString( "f4" );
        }
    }
}
