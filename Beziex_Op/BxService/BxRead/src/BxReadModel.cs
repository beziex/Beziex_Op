using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Beziex_Op {

    public class BxReadModel {

        public class JsonPatchInfo_SurfaceEdge {
            public List<float>  bez00;
            public List<float>  bez01;
            public List<float>  bez10;
            public List<float>  bez11;
        }

        public class JsonPatchInfo_TNodeType {
            public string  edge00;
            public string  edge01;
            public string  edge10;
            public string  edge11;
        }

        public class JsonPatchInfo_Surface {
            public List<float>                vertex;
            public JsonPatchInfo_SurfaceEdge  surfaceEdge;
            public JsonPatchInfo_TNodeType    tNodeType;
            public string                     preDivided;
        }

        public class JsonPatchInfo {
            public List<float>                  minMax;
            public List<JsonPatchInfo_Surface>  surface;
        }

        // -------------------------------------------------------

        public void  Exec( string fileName, out BxVec3F min, out BxVec3F max, out BxCmSeparatePatch_Object patch )
        {
            JsonPatchInfo  json;

            ReadPatch( fileName, out json );
            FromJson( json, out min, out max, out patch );
        }

        // -----

        private void  ReadPatch( string gzipName, out JsonPatchInfo json )
        {
            json = null;

            string  jsonStr  = null;
            using( FileStream inStream = new FileStream( gzipName, FileMode.Open, FileAccess.Read ) ) {
                using( GZipStream decompStream = new GZipStream( inStream, CompressionMode.Decompress ) ) {
                    using( StreamReader reader = new StreamReader( decompStream ) )
                        jsonStr = reader.ReadToEnd();
                }
            }

            try {
                json = JsonConvert.DeserializeObject<JsonPatchInfo>( jsonStr );
            }
            catch( Exception ) {
                return;
            }
        }

        private void  FromJson( JsonPatchInfo json, out BxVec3F min, out BxVec3F max, out BxCmSeparatePatch_Object patch )
        {
            bool  success;

            min = null;
            max = null;

            patch = new BxCmSeparatePatch_Object();

            if( json == null || json.surface == null || json.minMax == null ) {
                patch = null;
                return;
            }

            FromJson_MinMax( json.minMax, out min, out max );

            uint  numSurface = 0;
            foreach( JsonPatchInfo_Surface jsonSurface in json.surface )
                numSurface++;

            patch = new BxCmSeparatePatch_Object();
            patch.Alloc( numSurface );

            uint  cntSurface = 0;
            foreach( JsonPatchInfo_Surface jsonSurface in json.surface ) {
                if( jsonSurface.vertex == null || jsonSurface.surfaceEdge == null ) {
                    patch = null;
                    return;
                }

                patch[ cntSurface ].Alloc();

                FromJson_Vertex( jsonSurface.vertex, patch[ cntSurface ] );
                success = FromJson_SurfaceEdge( jsonSurface.surfaceEdge, patch[ cntSurface ] );
                if( success == false ) {
                    patch = null;
                    return;
                }
                success = FromJson_TNodeType( jsonSurface.tNodeType, patch[ cntSurface ] );
                if( success == false ) {
                    patch = null;
                    return;
                }
                success = FromJson_PreDivided( jsonSurface.preDivided, patch[ cntSurface ] );
                if( success == false ) {
                    patch = null;
                    return;
                }

                cntSurface++;
            }
            Debug.Assert( cntSurface == numSurface );
        }

        private void  FromJson_MinMax( List<float> minMax, out BxVec3F min, out BxVec3F max )
        {
            min = new BxVec3F();
            max = new BxVec3F();

            for( byte i=0; i<3; i++ ) {
                min[ i ] = minMax[ 0 + i ];
                max[ i ] = minMax[ 3 + i ];
            }
        }

        private void  FromJson_Vertex( List<float> vertex, BxCmSeparatePatch_Surface surface )
        {
            ushort  dstCnt = 0;
            foreach( float val in vertex ) {
                byte  dstIdx = ( byte )( dstCnt / 3 );
                byte  dstMod = ( byte )( dstCnt % 3 );

                switch( dstMod ) {
                case 0:
                    surface.Vertex[ dstIdx ].Pos.X = val;
                    break;
                case 1:
                    surface.Vertex[ dstIdx ].Pos.Y = val;
                    break;
                default:
                    Debug.Assert( dstMod == 2 );
                    surface.Vertex[ dstIdx ].Pos.Z = val;
                    break;
                }

                dstCnt++;
            }
            Debug.Assert( dstCnt == ( 4 * 3 ) );
        }

        private bool  FromJson_SurfaceEdge( JsonPatchInfo_SurfaceEdge surfaceEdge, BxCmSeparatePatch_Surface surface )
        {
            if( surfaceEdge.bez00 == null || surfaceEdge.bez01 == null || surfaceEdge.bez10 == null || surfaceEdge.bez11 == null )
                return false;

            FromJson_SurfaceEdgeOne( surfaceEdge.bez00, surface.SurfaceEdge[ 0 ][ 0 ] );
            FromJson_SurfaceEdgeOne( surfaceEdge.bez01, surface.SurfaceEdge[ 0 ][ 1 ] );
            FromJson_SurfaceEdgeOne( surfaceEdge.bez10, surface.SurfaceEdge[ 1 ][ 0 ] );
            FromJson_SurfaceEdgeOne( surfaceEdge.bez11, surface.SurfaceEdge[ 1 ][ 1 ] );

            return true;
        }

        private void  FromJson_SurfaceEdgeOne(  List<float> bez, BxCmSeparatePatch_SurfaceEdge surfaceEdge )
        {
            ushort  dstCnt = 0;
            foreach( float val in bez ) {
                byte  dstIdx = ( byte )( dstCnt / 3 );
                byte  dstMod = ( byte )( dstCnt % 3 );

                switch( dstMod ) {
                case 0:
                    surfaceEdge.Inner[ dstIdx ].X = val;
                    break;
                case 1:
                    surfaceEdge.Inner[ dstIdx ].Y = val;
                    break;
                default:
                    Debug.Assert( dstMod == 2 );
                    surfaceEdge.Inner[ dstIdx ].Z = val;
                    break;
                }

                dstCnt++;
            }
            Debug.Assert( dstCnt == ( 7 * 3 ) );
        }

        private bool  FromJson_TNodeType( JsonPatchInfo_TNodeType tNodeType, BxCmSeparatePatch_Surface surface )
        {
            if( tNodeType.edge00 == null || tNodeType.edge01 == null || tNodeType.edge10 == null || tNodeType.edge11 == null )
                return false;

            surface.TNodeType[ 0 ][ 0 ] = FromJson_TNodeTypeOne( tNodeType.edge00 );
            surface.TNodeType[ 0 ][ 1 ] = FromJson_TNodeTypeOne( tNodeType.edge01 );
            surface.TNodeType[ 1 ][ 0 ] = FromJson_TNodeTypeOne( tNodeType.edge10 );
            surface.TNodeType[ 1 ][ 1 ] = FromJson_TNodeTypeOne( tNodeType.edge11 );

            return true;
        }

        private BxTNodeType  FromJson_TNodeTypeOne( string edge )
        {
            switch( edge ) {
            case "none":
                return BxTNodeType.KTNodeType_None;
            case "part":
                return BxTNodeType.KTNodeType_Part;
            default:
                Debug.Assert( edge == "all" );
                return BxTNodeType.KTNodeType_All;
            }
        }

        private bool  FromJson_PreDivided( string preDivided, BxCmSeparatePatch_Surface surface )
        {
            if( preDivided == null )
                return false;

            if( preDivided == "true" )
                surface.PreDivided = true;
            else {
                Debug.Assert( preDivided == "false" );
                surface.PreDivided = false;
            }

            return true;
        }
    }
}
