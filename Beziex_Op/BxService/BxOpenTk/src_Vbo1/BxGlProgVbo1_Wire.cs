namespace Beziex_Op {

    public class BxGlProgVbo1_Wire : BxGlProgramBase {

        protected override ProgShaderType  GetTessShaderType()
        {
            return ProgShaderType.KProgShaderType_Vbo1_Wire;
        }

        protected override void  GetShaderPath(
            out string vertexShaderPath, out string tessCtlShaderPath, out string tessEvalShaderPath, out string geomertyShaderPath, out string fragmentShaderPath )
        {
            string  shaderPath = GetExeDir() + "\\" + "shader\\shaderVbo1";

            vertexShaderPath   = shaderPath + "\\" + "bxViewVbo1.vert";
            tessCtlShaderPath  = shaderPath + "\\" + "bxViewVbo1.tesc";
            tessEvalShaderPath = shaderPath + "\\" + "bxViewVbo1.tese";
            geomertyShaderPath = shaderPath + "\\" + "bxViewVbo1.geom";
            fragmentShaderPath = shaderPath + "\\" + "bxViewVbo1_Wire.frag";
        }
    }
}
