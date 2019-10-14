namespace Beziex_Op {

    public class BxReadRoot {

        private readonly BxReadDlog   fDlog  = new BxReadDlog();
        private readonly BxReadModel  fModel = new BxReadModel();

        public void Exec( BxCmUiParam param, out string fileName, out BxCmSeparatePatch_Object patch )
        {
            patch = null;

            fDlog.GetFname( out fileName );
            if( fileName == null )
                return;

            BxVec3F  min, max;
            fModel.Exec( fileName, out min, out max, out patch );

            param.Min = min;
            param.Max = max;
        }
    }
}
