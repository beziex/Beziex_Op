using System;
using System.Diagnostics;
using System.Reflection;
using System.IO;
using OpenTK.Graphics.OpenGL;

namespace Beziex_Op {

    abstract public class BxGlProgramBase : BxGlProgramObject {

        public enum ProgShaderType : byte {
            KProgShaderType_Vbo1      = 0,
            KProgShaderType_Vbo1_Wire = 1,
            KProgShaderType_GL3       = 2
        }

        private int  fShaderProgram;

        // -------------------------------------------------------
        
        abstract protected ProgShaderType  GetTessShaderType();

        virtual protected void  GetShaderPath(
            out string vertexShaderPath, out string tessCtlShaderPath, out string tessEvalShaderPath, out string fragmentShaderPath )
        {
            vertexShaderPath   = null;
            tessCtlShaderPath  = null;
            tessEvalShaderPath = null;
            fragmentShaderPath = null;
        }

        virtual protected void  GetShaderPath(
            out string vertexShaderPath, out string tessCtlShaderPath, out string tessEvalShaderPath, out string geomertyShaderPath, out string fragmentShaderPath )
        {
            vertexShaderPath   = null;
            tessCtlShaderPath  = null;
            tessEvalShaderPath = null;
            geomertyShaderPath = null;
            fragmentShaderPath = null;
        }

        virtual protected void  GetShaderPath( out string vertexShaderPath, out string fragmentShaderPath )
        {
            vertexShaderPath   = null;
            fragmentShaderPath = null;
        }

        public BxGlProgramBase()
        {
            fShaderProgram = 0;
            CreateShaderProgram();
        }

        public override void  ReleaseShaderProgram()
        {
            if( fShaderProgram > 0 ) {
                GL.DeleteProgram( fShaderProgram );
                fShaderProgram = 0;
            }
        }

        public int  ShaderProgramID()
        {
            return fShaderProgram;
        }

        // -------------------------------------------------------

        protected string  GetExeDir()
        {
            return Path.GetDirectoryName( Assembly.GetEntryAssembly().Location );
        }

        // -------------------------------------------------------

        private void  CreateShaderProgram()
        {
            switch( GetTessShaderType() ) {
            case ProgShaderType.KProgShaderType_Vbo1:
                CreateShaderProgram_Vbo1();
                break;
            case ProgShaderType.KProgShaderType_Vbo1_Wire:
                CreateShaderProgram_Vbo1_Wire();
                break;
            default:
                Debug.Assert( GetTessShaderType() == ProgShaderType.KProgShaderType_GL3 );
                CreateShaderProgram_GL3();
                break;
            }
        }

        private void  CreateShaderProgram_Vbo1()
        {
            string  vertexShaderPath, tessCtlShaderPath, tessEvalShaderPath, fragmentShaderPath;
            GetShaderPath( out vertexShaderPath, out tessCtlShaderPath, out tessEvalShaderPath, out fragmentShaderPath );

            int  vertexShader, tessCtlShader, tessEvalShader, fragmentShader;
            CreateVertexShader(   vertexShaderPath,   out vertexShader );
            CreateTessCtlShader(  tessCtlShaderPath,  out tessCtlShader );
            CreateTessEvalShader( tessEvalShaderPath, out tessEvalShader );
            CreateFragmentShader( fragmentShaderPath, out fragmentShader );

            fShaderProgram = GL.CreateProgram();
            GL.AttachShader( fShaderProgram, vertexShader );
            GL.AttachShader( fShaderProgram, tessCtlShader );
            GL.AttachShader( fShaderProgram, tessEvalShader );
            GL.AttachShader( fShaderProgram, fragmentShader );

            GL.DeleteShader( vertexShader );
            GL.DeleteShader( tessCtlShader );
            GL.DeleteShader( tessEvalShader );
            GL.DeleteShader( fragmentShader );

            GL.LinkProgram( fShaderProgram );

            string  info = GL.GetProgramInfoLog( fShaderProgram );
            Debug.Assert( string.IsNullOrWhiteSpace( info ) == true );
        }

        private void  CreateShaderProgram_Vbo1_Wire()
        {
            string  vertexShaderPath, tessCtlShaderPath, tessEvalShaderPath, geomertyShaderPath, fragmentShaderPath;
            GetShaderPath( out vertexShaderPath, out tessCtlShaderPath, out tessEvalShaderPath, out geomertyShaderPath, out fragmentShaderPath );

            int  vertexShader, tessCtlShader, tessEvalShader, geomertyShader, fragmentShader;
            CreateVertexShader(   vertexShaderPath,   out vertexShader );
            CreateTessCtlShader(  tessCtlShaderPath,  out tessCtlShader );
            CreateTessEvalShader( tessEvalShaderPath, out tessEvalShader );
            CreateGeomertyShader( geomertyShaderPath, out geomertyShader );
            CreateFragmentShader( fragmentShaderPath, out fragmentShader );

            fShaderProgram = GL.CreateProgram();
            GL.AttachShader( fShaderProgram, vertexShader );
            GL.AttachShader( fShaderProgram, tessCtlShader );
            GL.AttachShader( fShaderProgram, tessEvalShader );
            GL.AttachShader( fShaderProgram, geomertyShader );
            GL.AttachShader( fShaderProgram, fragmentShader );

            GL.DeleteShader( vertexShader );
            GL.DeleteShader( tessCtlShader );
            GL.DeleteShader( tessEvalShader );
            GL.DeleteShader( geomertyShader );
            GL.DeleteShader( fragmentShader );

            GL.LinkProgram( fShaderProgram );

            string  info = GL.GetProgramInfoLog( fShaderProgram );
            Debug.Assert( string.IsNullOrWhiteSpace( info ) == true );
        }

        private void  CreateShaderProgram_GL3()
        {
            string  vertexShaderPath, fragmentShaderPath;
            GetShaderPath( out vertexShaderPath, out fragmentShaderPath );

            int  vertexShader, fragmentShader;
            CreateVertexShader(   vertexShaderPath,   out vertexShader );
            CreateFragmentShader( fragmentShaderPath, out fragmentShader );

            fShaderProgram = GL.CreateProgram();
            GL.AttachShader( fShaderProgram, vertexShader );
            GL.AttachShader( fShaderProgram, fragmentShader );

            GL.DeleteShader( vertexShader );
            GL.DeleteShader( fragmentShader );

            GL.LinkProgram( fShaderProgram );

            string  info = GL.GetProgramInfoLog( fShaderProgram );
            Debug.Assert( string.IsNullOrWhiteSpace( info ) == true );
        }

        private void  CreateVertexShader( string vertexShaderPath, out int vertexShader )
        {
            vertexShader = GL.CreateShader(OpenTK.Graphics.OpenGL.ShaderType.VertexShader );

            try {
                GL.ShaderSource( vertexShader, File.ReadAllText( vertexShaderPath ) );
            }
            catch( Exception ) {
                Debug.Assert( false );
            }

            GL.CompileShader( vertexShader );

            int  status;
            try {
                GL.GetShader( vertexShader, ShaderParameter.CompileStatus, out status );

                string  log;
                if( status == 0 )
                    log = GL.GetShaderInfoLog( vertexShader );

                Debug.Assert( status != 0 );
            }
            catch( Exception ) {
                Debug.Assert( false );
            }
        }

        private void  CreateTessCtlShader( string tessCtlShaderPath, out int tessCtlShader )
        {
            tessCtlShader = GL.CreateShader(OpenTK.Graphics.OpenGL.ShaderType.TessControlShader );

            try {
                GL.ShaderSource( tessCtlShader, File.ReadAllText( tessCtlShaderPath ) );
            }
            catch( Exception ) {
                Debug.Assert( false );
            }

            GL.CompileShader( tessCtlShader );

            int  status;
            try {
                GL.GetShader( tessCtlShader, ShaderParameter.CompileStatus, out status );

                string  log;
                if( status == 0 )
                    log = GL.GetShaderInfoLog( tessCtlShader );

                Debug.Assert( status != 0 );
            }
            catch( Exception ) {
                Debug.Assert( false );
            }
        }

        private void  CreateTessEvalShader( string tessEvalShaderPath, out int tessEvalShader )
        {
            tessEvalShader = GL.CreateShader(OpenTK.Graphics.OpenGL.ShaderType.TessEvaluationShader );

            try {
                GL.ShaderSource( tessEvalShader, File.ReadAllText( tessEvalShaderPath ) );
            }
            catch( Exception ) {
                Debug.Assert( false );
            }

            GL.CompileShader( tessEvalShader );

            int  status;
            try {
                GL.GetShader( tessEvalShader, ShaderParameter.CompileStatus, out status );

                string  log;
                if( status == 0 )
                    log = GL.GetShaderInfoLog( tessEvalShader );

                Debug.Assert( status != 0 );
            }
            catch( Exception ) {
                Debug.Assert( false );
            }
        }

        private void  CreateGeomertyShader( string geomertyShaderPath, out int geomertyShader )
        {
            geomertyShader = GL.CreateShader(OpenTK.Graphics.OpenGL.ShaderType.GeometryShader );

            try {
                GL.ShaderSource( geomertyShader, File.ReadAllText( geomertyShaderPath ) );
            }
            catch( Exception ) {
                Debug.Assert( false );
            }

            GL.CompileShader( geomertyShader );

            int  status;
            try {
                GL.GetShader( geomertyShader, ShaderParameter.CompileStatus, out status );

                string  log;
                if( status == 0 )
                    log = GL.GetShaderInfoLog( geomertyShader );

                Debug.Assert( status != 0 );
            }
            catch( Exception ) {
                Debug.Assert( false );
            }
        }

        private void  CreateFragmentShader( string fragmentShaderPath, out int fragmentShader )
        {
            fragmentShader = GL.CreateShader(OpenTK.Graphics.OpenGL.ShaderType.FragmentShader );

            try {
                GL.ShaderSource( fragmentShader, File.ReadAllText( fragmentShaderPath ) );
            }
            catch( Exception ) {
                Debug.Assert( false );
            }

            GL.CompileShader( fragmentShader );

            int  status;
            try {
                GL.GetShader( fragmentShader, ShaderParameter.CompileStatus, out status );

                string  log;
                if( status == 0 )
                    log = GL.GetShaderInfoLog( fragmentShader );

                Debug.Assert( status != 0 );
            }
            catch( Exception ) {
                Debug.Assert( false );
            }
        }
    }
}


////
//	End of code.
////
