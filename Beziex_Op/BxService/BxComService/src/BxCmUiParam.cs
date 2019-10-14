namespace Beziex_Op {

    public class BxCmUiParam {

        public enum EnumShaderMode : byte {
            KShaderMode_Vbo1 = 0,
            KShaderMode_GL3  = 1
        }

        public byte            NumTess           { get; set; }
        public BxVec3F         Min               { get; set; }
        public BxVec3F         Max               { get; set; }
        public bool            IsWire            { get; set; }
        public EnumShaderMode  ShaderMode        { get; set; }
        public bool            BenchmarkStarted  { get; set; }
        public long            ElapsedTime       { get; set; }

        public BxCmUiParam()
        {
            NumTess          = 8;
            Min              = null;
            Max              = null;
            IsWire           = false;
            ShaderMode       = EnumShaderMode.KShaderMode_Vbo1;
            BenchmarkStarted = false;
            ElapsedTime      = 0;
        }
    }
}
