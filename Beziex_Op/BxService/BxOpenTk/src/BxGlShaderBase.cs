using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Beziex_Op {

    abstract public class BxGlShaderBase {

        public class BufInfo {

            public int[]  VboID       { get; set; }
            public int[]  EboID       { get; set; }
            public int    NumVertices { get; set; }
            public int    NumIndices  { get; set; }
            public int[]  VaoHandle   { get; set; }

            public BufInfo()
            {
                VboID = new int[ 2 ];
                VboID[ 0 ] = int.MaxValue;
                VboID[ 1 ] = int.MaxValue;

                EboID = new int[ 1 ];
                EboID[ 0 ] = int.MaxValue;

                NumVertices = int.MaxValue;
                NumIndices =  int.MaxValue;

                VaoHandle = new int[ 1 ];
                VaoHandle[ 0 ] = int.MaxValue;
            }

            // ------

            public bool
            HasVertexBuffer()
            {
                if( VboID[ 0 ] != int.MaxValue )
                    return true;

                return false;
            }

            public void
            ReleaseVertexBuffer()
            {
                GL.DeleteBuffers( 2, VboID );

                VboID[ 0 ] = int.MaxValue;
                VboID[ 1 ] = int.MaxValue;
            }

            public void
            ReleaseVaoHandle()
            {
                GL.DeleteBuffers( 1, VaoHandle );

                VaoHandle[ 0 ] = int.MaxValue;
            }

            // ------

            public bool
            HasIndexBuffer()
            {
                if( EboID[ 0 ] != int.MaxValue )
                    return true;

                return false;
            }

            public void
            ReleaseIndexBuffer()
            {
                GL.DeleteBuffers( 1, EboID );

                EboID[ 0 ] = int.MaxValue;
                NumIndices = int.MaxValue;
            }
        }

        // -------------------------------------------------------

        private const float  KPi = 3.141592f;

        // -------------------------------------------------------

        protected BxGlProgramBase  fProgramObj = null;

        public byte      TessLevel { get; set; } = 8;
        public BxGlMain  Parent    { get; set; }
        public BufInfo   Buf       { get; set; } = null;

        // -------------------------------------------------------

        abstract protected void  SetVertexBuffer( BxCmSeparatePatch_Object patch );
        abstract protected void  SetVAO();
        abstract protected void  PatchParameter();

        // -------------------------------------------------------

        public BxGlShaderBase( Func<GLControl> glControl, BxGlMain parent )
        {
            Parent = parent;
            Buf    = new BufInfo();
        }

        virtual public void  GenBuf( BxCmSeparatePatch_Object patch, BxCmUiParam param )
        {
            TessLevel = param.NumTess;

            SetVertexBuffer( patch );
            SetVAO();

            if( param.Min != null && param.Max != null ) {
                Vector3  min = new Vector3( ( float )param.Min.X, ( float )param.Min.Y, ( float )param.Min.Z );
                Vector3  max = new Vector3( ( float )param.Max.X, ( float )param.Max.Y, ( float )param.Max.Z );

                Parent.SetObjSize( min, max );
            }
        }

        virtual public void  Draw( BxCmUiParam param )
        {
            GL.UseProgram( fProgramObj.ShaderProgramID() );

            TessLevel = param.NumTess;

            SetTessLevel();
            SetProject();

            SetView( 0.0 );
            SetLight();

            DrawMain( param );
        }

        virtual public bool  IsEmpty()
        {
            if( Buf.NumVertices == int.MaxValue )
                return true;

            return false;
        }

        virtual public void  ReleaseBufInfo()
        {
            Buf.ReleaseVertexBuffer();
            Buf.ReleaseVaoHandle();
        }

        virtual protected void  DrawMain( BxCmUiParam param )
        {
            SetFaceColor();

            PatchParameter();

            GL.BindVertexArray( Buf.VaoHandle[ 0 ] );
            BeginBenchmark( param );

            GL.DrawArrays( PrimitiveType.Patches, 0, Buf.NumVertices );

            EndBenchmark( param );
            GL.BindVertexArray( 0 );
        }

        // ------

        protected void  SetFaceColor()
        {
            Vector3  ambient   = new Vector3( 0.2f,  0.2f, 0.2f );
            Vector3  diffuse   = new Vector3( 0.75f, 0.0f, 0.0f );
            Vector3  specular  = new Vector3( 0.4f,  0.4f, 0.4f );
            float    shininess = 50.0f;

            int  ambientLocation   = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "mtlAmb" );
            int  diffuseLocation   = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "mtlDif" );
            int  specularLocation  = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "mtlSpec" );
            int  shininessLocation = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "mtlShin" );

            GL.Uniform3( ambientLocation,  ref ambient );
            GL.Uniform3( diffuseLocation,  ref diffuse );
            GL.Uniform3( specularLocation, ref specular );
            GL.Uniform1( shininessLocation, shininess );
        }

        // ------

        private void  SetView( double ofsZ )
        {
            Vector3  eye    = new Vector3( 0.0f, 0.0f, ( float )( 10.0 + ofsZ ) );
            Vector3  center = new Vector3( 0.0f, 0.0f, ( float )( 0.0 + ofsZ ) );
            Vector3  up     = new Vector3( 0.0f, 1.0f, 0.0f );

            Matrix4  transformMatrix = TransformMatrix();
            Matrix4  cameraMatrix    = Matrix4.LookAt( eye, center, up );
            Matrix4  viewMatrix      = transformMatrix * cameraMatrix;

            Matrix3  normalMatrix = NormalMatrix();

            int  viewMatrixLocation = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "viewMatrix" );
            GL.UniformMatrix4( viewMatrixLocation, false, ref viewMatrix );

            int  normalMatrixLocation = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "normalMatrix" );
            GL.UniformMatrix3( normalMatrixLocation, false, ref normalMatrix );
        }

        private Matrix4  TransformMatrix()
        {
            return Parent.TransformMatrix();
        }

        private Matrix3  NormalMatrix()
        {
            return Parent.NormalMatrix();
        }

        // -------------------------------------------------------

        private void  SetTessLevel()
        {
            int  tessLevelLocation = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "tessLevel" );
            GL.Uniform1( tessLevelLocation, ( float )TessLevel );
        }

        private void  SetProject()
        {
            GLControl  control = Parent.GLControl();

            GL.Viewport( 0, 0, control.Size.Width, control.Size.Height );

            float fieldOfViewY;
            float fieldOfViewMin = ( float )( 30.0 * ( KPi / 180.0 ) );
            if( control.Size.Width < control.Size.Height ) {
                double  zLength = ( control.Size.Width / 2.0 ) * Math.Tan( ( ( KPi / 180.0 ) * 90.0 ) - ( fieldOfViewMin / 2.0 ) );
                fieldOfViewY = ( float )( Math.Atan( ( control.Size.Height / 2.0 ) / zLength ) * 2.0 );
            }
            else
                fieldOfViewY = fieldOfViewMin;

            float    aspect     = ( float )( ( double )control.Size.Width / control.Size.Height );
            Matrix4  projMatrix = Matrix4.CreatePerspectiveFieldOfView( fieldOfViewY, aspect, 1.0f, 100.0f );

            int  projMatrixLocation = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "projMatrix" );
            GL.UniformMatrix4( projMatrixLocation, false, ref projMatrix );
        }

        private void  SetLight()
        {
            SetLight0();
        }

        private void  SetLight0()
        {
            Vector3  ambient  = new Vector3( 0, 0, 0 );
            Vector3  diffuse  = new Vector3( 1, 1, 1 );
            Vector3  specular = new Vector3( 1, 1, 1 );
            Vector3  lightDir = new Vector3( -0.5f, -3.0f, -4.0f );

            Vector3  lightVec = Vector3.Normalize( -lightDir );

            int  ambientLocation  = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "lightAmb" );
            int  diffuseLocation  = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "lightDif" );
            int  specularLocation = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "lightSpec" );
            int  lightVecLocation = GL.GetUniformLocation( fProgramObj.ShaderProgramID(), "lightVec" );

            GL.Uniform3( ambientLocation,  ref ambient );
            GL.Uniform3( diffuseLocation,  ref diffuse );
            GL.Uniform3( specularLocation, ref specular );
            GL.Uniform3( lightVecLocation, ref lightVec );
        }

        // -------------------------------------------------------

        protected void  BeginBenchmark( BxCmUiParam param )
        {
            Parent.BeginBenchmark( param );
        }

        protected void  EndBenchmark( BxCmUiParam param )
        {
            Parent.EndBenchmark( param );
        }
    }
}
