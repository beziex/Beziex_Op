namespace Beziex_Op {

    public class BxGlProgGL3_Wire : BxGlProgramBase {

        protected override ProgShaderType  GetTessShaderType()
        {
            return ProgShaderType.KProgShaderType_GL3;
        }

        protected override void  GetShaderPath( out string vertexShaderPath, out string fragmentShaderPath )
        {
            string  shaderPath = GetExeDir() + "\\" + "shader\\shaderGL3";

            vertexShaderPath   = shaderPath + "\\" + "bxViewGL3.vert";
            fragmentShaderPath = shaderPath + "\\" + "bxViewGL3_Wire.frag";
        }
    }
}
