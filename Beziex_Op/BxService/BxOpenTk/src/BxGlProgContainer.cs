namespace Beziex_Op {

    abstract public class BxGlProgramObject {

        abstract public void  ReleaseShaderProgram();
    }

    public class BxGlProgContainer {

        private readonly BxGlProgramObject  fViewProgGL3_FaceObj;
        private readonly BxGlProgramObject  fViewProgGL3_WireObj;
        private readonly BxGlProgramObject  fViewProgVbo1_FaceObj;
        private readonly BxGlProgramObject  fViewProgVbo1_WireObj;

        // -------------------------------------------------------

        public BxGlProgContainer(
            BxGlProgramObject viewProgGL3_FaceObj, BxGlProgramObject viewProgGL3_WireObj, BxGlProgramObject viewProgVbo1_FaceObj, BxGlProgramObject viewProgVbo1_WireObj )
        {
            fViewProgGL3_FaceObj  = viewProgGL3_FaceObj;
            fViewProgGL3_WireObj  = viewProgGL3_WireObj;
            fViewProgVbo1_FaceObj = viewProgVbo1_FaceObj;
            fViewProgVbo1_WireObj = viewProgVbo1_WireObj;
        }

        public void  ReleaseShaderProgram()
        {
            if( fViewProgGL3_FaceObj != null )
                fViewProgGL3_FaceObj.ReleaseShaderProgram();

            if( fViewProgGL3_WireObj != null )
                fViewProgGL3_WireObj.ReleaseShaderProgram();

            if( fViewProgVbo1_FaceObj != null )
                fViewProgVbo1_FaceObj.ReleaseShaderProgram();

            if( fViewProgVbo1_WireObj != null )
                fViewProgVbo1_WireObj.ReleaseShaderProgram();
        }

        public BxGlProgramObject  ViewProgGL3_FaceObj()
        {
            return fViewProgGL3_FaceObj;
        }

        public BxGlProgramObject  ViewProgGL3_WireObj()
        {
            return fViewProgGL3_WireObj;
        }

        public BxGlProgramObject  ViewProgVbo1_FaceObj()
        {
            return fViewProgVbo1_FaceObj;
        }

        public BxGlProgramObject  ViewProgVbo1_WireObj()
        {
            return fViewProgVbo1_WireObj;
        }
    }
}
