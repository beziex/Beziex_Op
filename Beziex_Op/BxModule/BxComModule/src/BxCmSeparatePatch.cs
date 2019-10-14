namespace Beziex_Op {

    public class BxCmSeparatePatch_Vertex {

        public BxVec3F  Pos { get; set; }

        // ------

        public BxCmSeparatePatch_Vertex()
        {
            Pos = null;
        }

        public BxCmSeparatePatch_Vertex( BxCmSeparatePatch_Vertex src )
        {
            Pos = new BxVec3F( src.Pos );
        }

        // ------

        public BxCmSeparatePatch_Vertex  Copy {
            get {
                return new BxCmSeparatePatch_Vertex( this );
            }
        }

        public void  Alloc()
        {
            Pos = new BxVec3F();
        }

        public void  SetPos( BxVec3F data )
        {
            Pos.Set( data );
        }
    }

    // -------------------------------------------------------

    public class BxCmSeparatePatch_SurfaceEdge {

        public BxBezier6Line3F  Inner { get; set; }

        // ------

        public BxCmSeparatePatch_SurfaceEdge()
        {
            Inner = null;
        }

        public BxCmSeparatePatch_SurfaceEdge( BxCmSeparatePatch_SurfaceEdge src )
        {
            Inner = src.Inner.Copy;
        }

        // ------

        public BxCmSeparatePatch_SurfaceEdge  Copy {
            get {
                return new BxCmSeparatePatch_SurfaceEdge( this );
            }
        }

        public void  Alloc()
        {
            Inner = new BxBezier6Line3F();
        }
    }

    // -------------------------------------------------------

    public class BxCmSeparatePatch_Surface {

        public BxCmSeparatePatch_Vertex[/*4*/]              Vertex        { get; set; }
        public BxCmSeparatePatch_SurfaceEdge[/*2*/][/*2*/]  SurfaceEdge   { get; set; }
        public uint?                                        OrgSurfaceNo  { get; set; }
        public uint?[/*2*/][/*2*/]                          OrgEdgeNo     { get; set; }
        public BxTNodeType[/*2*/][/*2*/]                    TNodeType     { get; set; }
        public uint?[/*4*/]                                 OrgVertexNo   { get; set; }
        public bool?                                        PreDivided    { get; set; }

        // ------

        public BxCmSeparatePatch_Surface()
        {
            Vertex       = null;
            SurfaceEdge  = null;
            OrgSurfaceNo = null;
            OrgEdgeNo    = null;
            TNodeType    = null;
            OrgVertexNo  = null;
            PreDivided   = null;
        }

        public BxCmSeparatePatch_Surface( BxCmSeparatePatch_Surface src )
        {
            Vertex = new BxCmSeparatePatch_Vertex[ 4 ];
            for( byte i=0; i<4; i++ )
                Vertex[ i ] = src.Vertex[ i ].Copy;

            SurfaceEdge = new BxCmSeparatePatch_SurfaceEdge[ 2 ][];
            for( byte i=0; i<2; i++ ) {
                SurfaceEdge[ i ] = new BxCmSeparatePatch_SurfaceEdge[ 2 ];

                for( byte j=0; j<2; j++ )
                    SurfaceEdge[ i ][ j ] = src.SurfaceEdge[ i ][ j ].Copy;
            }

            OrgSurfaceNo = src.OrgSurfaceNo;

            OrgEdgeNo = new uint?[ 2 ][];
            for( byte i=0; i<2; i++ ) {
                OrgEdgeNo[ i ] = new uint?[ 2 ];

                for( byte j=0; j<2; j++ )
                    OrgEdgeNo[ i ][ j ] = src.OrgEdgeNo[ i ][ j ];
            }

            TNodeType = new BxTNodeType[ 2 ][];
            for( byte i=0; i<2; i++ ) {
                TNodeType[ i ] = new BxTNodeType[ 2 ];

                for( byte j=0; j<2; j++ )
                    TNodeType[ i ][ j ] = src.TNodeType[ i ][ j ];
            }

            OrgVertexNo = new uint?[ 4 ];
            for( byte i=0; i<4; i++ )
                OrgVertexNo[ i ] = src.OrgVertexNo[ i ];

            PreDivided = src.PreDivided;
        }

        // ------

        public BxCmSeparatePatch_Surface  Copy {
            get {
                return new BxCmSeparatePatch_Surface( this );
            }
        }

        public void  Alloc()
        {
            AllocVertex();
            AllocSurfaceEdge();
            AllocOrgEdgeNo();
            AllocTNodeType();
            AllocOrgVertexNo();
        }

        public void  AllocVertex()
        {
            Vertex = new BxCmSeparatePatch_Vertex[ 4 ];
            for( byte i=0; i<4; i++ ) {
                Vertex[ i ] = new BxCmSeparatePatch_Vertex();
                Vertex[ i ].Alloc();
            }
        }

        public void  AllocSurfaceEdge()
        {
            SurfaceEdge = new BxCmSeparatePatch_SurfaceEdge[ 2 ][];
            for( byte i=0; i<2; i++ ) {
                SurfaceEdge[ i ] = new BxCmSeparatePatch_SurfaceEdge[ 2 ];

                for( byte j=0; j<2; j++ ) {
                    SurfaceEdge[ i ][ j ] = new BxCmSeparatePatch_SurfaceEdge();
                    SurfaceEdge[ i ][ j ].Alloc();
                }
            }
        }

        public void  AllocOrgEdgeNo()
        {
            OrgEdgeNo = new uint?[ 2 ][];
            for( byte i=0; i<2; i++ )
                OrgEdgeNo[ i ] = new uint?[ 2 ];
        }

        public void  AllocTNodeType()
        {
            TNodeType = new BxTNodeType[ 2 ][];
            for( byte i=0; i<2; i++ )
                TNodeType[ i ] = new BxTNodeType[ 2 ];
        }

        public void  AllocOrgVertexNo()
        {
            OrgVertexNo = new uint?[ 4 ];
        }

        public void  SetVertex( byte surfaceEdgeNo, BxCmSeparatePatch_Vertex data )
        {
            Vertex[ surfaceEdgeNo ].SetPos( data.Pos );
        }

        public void  SetSurfaceEdge( byte hvId, byte hvOfs, BxCmSeparatePatch_SurfaceEdge data )
        {
            SurfaceEdge[ hvId ][ hvOfs ].Inner.Set( data.Inner );
        }
    }

    // -------------------------------------------------------

    public class BxCmSeparatePatch_Object {

        private BxCmSeparatePatch_Surface[]  fSurface;

        // ------

        public BxCmSeparatePatch_Object()
        {
            fSurface = null;
        }

        public BxCmSeparatePatch_Object( BxCmSeparatePatch_Object src )
        {
            fSurface = null;
            if( src.fSurface != null && src.fSurface.Length > 0 ) {
                fSurface = new BxCmSeparatePatch_Surface[ src.fSurface.Length ];
                for( uint i=0; i<src.fSurface.Length; i++ )
                    fSurface[ i ] = src.fSurface[ i ].Copy;
            }
        }

        // ------

        public BxCmSeparatePatch_Object  Copy {
            get {
                return new BxCmSeparatePatch_Object( this );
            }
        }

        public void  Alloc( uint numSurface )
        {
            fSurface = null;
            if( numSurface > 0 ) {
                fSurface = new BxCmSeparatePatch_Surface[ numSurface ];
                for( uint i=0; i<numSurface; i++ )
                    fSurface[ i ] = new BxCmSeparatePatch_Surface();
            }
        }

        public void  Set( uint index, BxCmSeparatePatch_Surface data )
        {
            for( byte i=0; i<4; i++ )
                fSurface[ index ].SetVertex( i, data.Vertex[ i ] );

            for( byte hvId=0; hvId<2; hvId++ ) {
                for( byte hvOfs=0; hvOfs<2; hvOfs++ )
                    fSurface[ index ].SetSurfaceEdge( hvId, hvOfs, data.SurfaceEdge[ hvId ][ hvOfs ] );
            }
        }

        public uint  NumSurface {
            get {
                if( fSurface == null )
                    return 0;

                return ( uint )fSurface.Length;
            }
        }

        // ------

        public BxCmSeparatePatch_Surface  this[ int idx ] {
            get {
                return fSurface[ idx ];
            }
            set {
                fSurface[ idx ] = value;
            }
        }

        public BxCmSeparatePatch_Surface  this[ uint idx ] {
            get {
                return fSurface[ idx ];
            }
            set {
                fSurface[ idx ] = value;
            }
        }
    }
}
