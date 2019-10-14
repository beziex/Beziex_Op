namespace Beziex_Op {

    public class BxGlProgVbo1_Face : BxGlProgramBase {

        protected override ProgShaderType  GetTessShaderType()
        {
            return ProgShaderType.KProgShaderType_Vbo1;
        }

        protected override void  GetShaderPath( out string vertexShaderPath, out string tessCtlShaderPath, out string tessEvalShaderPath, out string fragmentShaderPath )
        {
            string  shaderPath = GetExeDir() + "\\" + "shader\\shaderVbo1";

            vertexShaderPath   = shaderPath + "\\" + "bxViewVbo1.vert";
            tessCtlShaderPath  = shaderPath + "\\" + "bxViewVbo1.tesc";
            tessEvalShaderPath = shaderPath + "\\" + "bxViewVbo1.tese";
            fragmentShaderPath = shaderPath + "\\" + "bxViewVbo1.frag";
        }
    }
}
