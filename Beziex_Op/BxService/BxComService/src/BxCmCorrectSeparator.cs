using System.Diagnostics;

namespace Beziex_Op {

    public class BxCmCorrectSeparator {

        public void  Exec( BxCmSeparatePatch_Object src, out BxCmSeparatePatch_Object dst )
        {
            dst = new BxCmSeparatePatch_Object( src );

            ExecMain( src, dst );
        }

        // -------------------------------------------------------

        private void  ExecMain( BxCmSeparatePatch_Object src, BxCmSeparatePatch_Object dst )
        {
            for( uint i=0; i<src.NumSurface; i++ ) {
                ExecBezU( src, i, dst );
                ExecBezV( src, i, dst );

                dst[ i ].OrgSurfaceNo = src[ i ].OrgSurfaceNo;

                ExecOrgVertexNo( src, i, dst );
                ExecOrgEdgeNo( src, i, dst );
            }
        }

        private void  ExecBezU( BxCmSeparatePatch_Object src, uint surfaceNo, BxCmSeparatePatch_Object dst )
        {
            BxBezier6Line3F[]  bezU6 = new BxBezier6Line3F[ 4 ];
            BxBezier3Line3F[]  bezV3 = new BxBezier3Line3F[ 7 ];

            for( byte i=0; i<4; i++ )
                bezU6[ i ] = new BxBezier6Line3F();
            for( byte i=0; i<7; i++ )
                bezV3[ i ] = new BxBezier3Line3F();

            byte  hvId = 0;

            GetBezier( src, surfaceNo, hvId, bezU6 );
            TransBezier( bezU6, bezV3 );
            Correct( bezV3, surfaceNo, hvId, dst );
        }

        private void  ExecBezV( BxCmSeparatePatch_Object src, uint surfaceNo, BxCmSeparatePatch_Object dst )
        {
            BxBezier6Line3F[]  bezV6 = new BxBezier6Line3F[ 4 ];
            BxBezier3Line3F[]  bezU3 = new BxBezier3Line3F[ 7 ];

            for( byte i=0; i<4; i++ )
                bezV6[ i ] = new BxBezier6Line3F();
            for( byte i=0; i<7; i++ )
                bezU3[ i ] = new BxBezier3Line3F();

            byte  hvId = 1;

            GetBezier( src, surfaceNo, hvId, bezV6 );
            TransBezier( bezV6, bezU3 );
            Correct( bezU3, surfaceNo, hvId, dst );
        }

        // ------

        private void  GetBezier( BxCmSeparatePatch_Object src, uint surfaceNo, byte hvId, BxBezier6Line3F[/*4*/] tmpBez6 )
        {
            byte  hvOfs, crossIdx, idxPosBez0, idxPosBez1;

            hvOfs      = 0;
            crossIdx   = 0;
            idxPosBez0 = 0;
            idxPosBez1 = 1;
            GetBezierOuter( src, surfaceNo, hvId, hvOfs, crossIdx, idxPosBez0, tmpBez6 );
            GetBezierInner( src, surfaceNo, hvId, hvOfs, idxPosBez1, tmpBez6 );

            hvOfs      = 1;
            crossIdx   = 6;
            idxPosBez0 = 3;
            idxPosBez1 = 2;
            GetBezierOuter( src, surfaceNo, hvId, hvOfs, crossIdx, idxPosBez0, tmpBez6 );
            GetBezierInner( src, surfaceNo, hvId, hvOfs, idxPosBez1, tmpBez6 );
        }

        private void  GetBezierOuter( BxCmSeparatePatch_Object src, uint surfaceNo, byte hvId, byte hvOfs, byte crossIdx, byte idxPosBez, BxBezier6Line3F[/*4*/] tmpBez6 )
        {
            byte  vNo0, vNo1;
            HVtoVertexNo( hvId, hvOfs, out vNo0, out vNo1 );

            byte  wingHvId = ( byte )( 1 - hvId );

            BxBezier3Line3F  bez3 = new BxBezier3Line3F();

            bez3[ 0 ].Set( src[ surfaceNo ].Vertex[ vNo0 ].Pos );
            bez3[ 3 ].Set( src[ surfaceNo ].Vertex[ vNo1 ].Pos );
            bez3[ 1 ].Set( src[ surfaceNo ].SurfaceEdge[ wingHvId ][ 0 ].Inner[ crossIdx ] );
            bez3[ 2 ].Set( src[ surfaceNo ].SurfaceEdge[ wingHvId ][ 1 ].Inner[ crossIdx ] );

            tmpBez6[ idxPosBez ] = bez3.UpperTo6();
        }

        private void  HVtoVertexNo( byte hvId, byte hvOfs, out byte vertexNo0, out byte vertexNo1 )
        {
            if( hvId == 0 ) {
                if( hvOfs == 0 ) {
                    vertexNo0 = 0;
                    vertexNo1 = 1;
                }
                else {
                    Debug.Assert( hvOfs == 1 );
                    vertexNo0 = 3;
                    vertexNo1 = 2;
                }
            }
            else {
                Debug.Assert( hvId == 1 );
                if( hvOfs == 0 ) {
                    vertexNo0 = 0;
                    vertexNo1 = 3;
                }
                else {
                    Debug.Assert( hvOfs == 1 );
                    vertexNo0 = 1;
                    vertexNo1 = 2;
                }
            }
        }

        private void  GetBezierInner( BxCmSeparatePatch_Object src, uint surfaceNo, byte hvId, byte hvOfs, byte idxPosBez1, BxBezier6Line3F[/*4*/] tmpBez6 )
        {
            for( byte i=0; i<7; i++ )
                tmpBez6[ idxPosBez1 ][ i ] = src[ surfaceNo ].SurfaceEdge[ hvId ][ hvOfs ].Inner[ i ];

        }

        // ------

        private void  TransBezier( BxBezier6Line3F[/*4*/] srcBez6, BxBezier3Line3F[/*7*/] transBez3 )
        {
            for( byte i=0; i<7; i++ ) {
                for( byte j=0; j<4; j++ )
                    transBez3[ i ][ j ].Set( srcBez6[ j ][ i ] );
            }
        }

        private void  Correct( BxBezier3Line3F[/*7*/] transBez3, uint surfaceNo, byte hvId, BxCmSeparatePatch_Object dst )
        {
            BxVec3F[]  handle = new BxVec3F[ 2 ];

            for( byte i=0; i<7; i++ ) {
                CorrectMain( transBez3[ i ], handle );

                for( byte j=0; j<2; j++ ) {
                    if( handle[ j ] != null ) {
                        byte  srcBez6Idx = 0;
                        if( j == 1 )
                            srcBez6Idx = 3;

                        dst[ surfaceNo ].SurfaceEdge[ hvId ][ j ].Inner[ i ] = handle[ j ] + transBez3[ i ][ srcBez6Idx ];
                    }
                }
            }
        }

        private void  CorrectMain( BxBezier3Line3F transBez3, BxVec3F[/*2*/] dstHandle )
        {
            sbyte lIdxNoHandleZero, rIdxNoHandleZero;
            transBez3.GetIdxNoHandleZero( out lIdxNoHandleZero, out rIdxNoHandleZero );

            sbyte  dim = ( sbyte )( rIdxNoHandleZero - lIdxNoHandleZero + 1 );

            dstHandle[ 0 ] = null;
            dstHandle[ 1 ] = null;

            Debug.Assert( 1 <= dim && dim <= 3 );
            if( dim == 3 )
                return;

            float  lenBetweenVtx = ( transBez3[ 3 ] - transBez3[ 0 ] ).Length;
            float  minimumLen    = lenBetweenVtx * BxMathF.KDirectionEpsRatioF;

            if( minimumLen < BxMathF.KDirectionEpsRatioF )
                minimumLen = BxMathF.KDirectionEpsRatioF;

            if( dim == 2 ) {
                if( lIdxNoHandleZero > 0 )
                    dstHandle[ 0 ] = ( transBez3[ 2 ] - transBez3[ 0 ] ).Normalize() * minimumLen;
                else {
                    Debug.Assert( rIdxNoHandleZero < 2 );
                    dstHandle[ 1 ] = ( transBez3[ 1 ] - transBez3[ 3 ] ).Normalize() * minimumLen;

                }
            }
            else if( dim == 1 ) {
                if( rIdxNoHandleZero == 2 ) {
                    dstHandle[ 0 ] = ( transBez3[ 3 ] - transBez3[ 0 ] ).Normalize() * minimumLen;

                    BxVec3F  vecR = transBez3[ 2 ] - transBez3[ 3 ];
                    float    lenR = vecR.Length;
                    dstHandle[ 1 ] = vecR * ( ( lenR - minimumLen ) / lenR );
                }
                else if( lIdxNoHandleZero == 0 ) {
                    BxVec3F  vecL = transBez3[ 1 ] - transBez3[ 0 ];
                    float    lenL = vecL.Length;
                    dstHandle[ 0 ] = vecL * ( ( lenL - minimumLen ) / lenL );

                    dstHandle[ 1 ] = ( transBez3[ 0 ] - transBez3[ 3 ] ).Normalize() * minimumLen;
                }
                else {
                    BxVec3F  handle = ( transBez3[ 3 ] - transBez3[ 0 ] ).Normalize() * minimumLen;
                    dstHandle[ 0 ] = handle.Copy;
                    dstHandle[ 1 ] = -handle;
                }
            }
        }

        // ------

        private void  ExecOrgVertexNo( BxCmSeparatePatch_Object src, uint surfaceNo, BxCmSeparatePatch_Object dst )
        {
            for( byte i=0; i<4; i++ )
                dst[ surfaceNo ].OrgVertexNo[ i ] = src[ surfaceNo ].OrgVertexNo[ i ];
        }

        private void  ExecOrgEdgeNo( BxCmSeparatePatch_Object src, uint surfaceNo, BxCmSeparatePatch_Object dst )
        {
            for( byte hvId=0; hvId<2; hvId++ ) {
                for( byte hvOfs=0; hvOfs<2; hvOfs++ )
                    dst[ surfaceNo ].OrgEdgeNo[ hvId ][ hvOfs ] = src[ surfaceNo ].OrgEdgeNo[ hvId ][ hvOfs ];
            }
        }
    }
}
