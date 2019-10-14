using System;
using System.Diagnostics;

namespace Beziex_Op {

    public class BxMathF {

        public const byte  KElmX = 0;
        public const byte  KElmY = 1;
        public const byte  KElmZ = 2;

        public const byte  KNumVec3F = 3;

        public const float  KNearZeroF = 1.0E-6F;

        public const float  KDirectionEpsRatioF        = 1.0E-4F;
        public const float  KDirectionEpsRatioSquaredF = 1.0E-8F;
    }

    // -------------------------------------------------------

    public class BxVec3F : BxMathF {

        private readonly float[]  fDt;

        // ------

        public BxVec3F()
        {
            fDt = new float[ KNumVec3F ];

            X = 0.0F;
            Y = 0.0F;
            Z = 0.0F;
        }

        public BxVec3F( BxVec3F a )
        {
            fDt = new float[ KNumVec3F ];

            X = a.X;
            Y = a.Y;
            Z = a.Z;
        }

        public BxVec3F( float x, float y, float z )
        {
            fDt = new float[ KNumVec3F ];

            X = x;
            Y = y;
            Z = z;
        }

        // ------

        public float  this[ int idx ] {
            get {
                return fDt[ idx ];
            }
            set {
                fDt[ idx ] = value;
            }
        }

        public float  this[ uint idx ] {
            get {
                return fDt[ idx ];
            }
            set {
                fDt[ idx ] = value;
            }
        }

        public float  X {
            get {
                return fDt[ KElmX ];
            }
            set {
                fDt[ KElmX ] = value;
            }
        }

        public float  Y {
            get {
                return fDt[ KElmY ];
            }
            set {
                fDt[ KElmY ] = value;
            }
        }

        public float  Z {
            get {
                return fDt[ KElmZ ];
            }
            set {
                fDt[ KElmZ ] = value;
            }
        }

        // ------

        public BxVec3F  Copy {
            get {
                return new BxVec3F( this );
            }
        }

        public void  Set( BxVec3F src )
        {
            X = src.X;
            Y = src.Y;
            Z = src.Z;
        }

        // ------

        public static BxVec3F  operator -( BxVec3F a )
        {
            return new BxVec3F( -a.X, -a.Y, -a.Z );
        }

        public static BxVec3F  operator +( BxVec3F a, BxVec3F b )
        {
            return new BxVec3F( a.X + b.X, a.Y + b.Y, a.Z + b.Z );
        }

        public static BxVec3F  operator -( BxVec3F a, BxVec3F b )
        {
            return new BxVec3F( a.X - b.X, a.Y - b.Y, a.Z - b.Z );
        }

        public static BxVec3F  operator *( BxVec3F a, float b )
        {
            return new BxVec3F( a.X * b, a.Y * b, a.Z * b );
        }

        public static BxVec3F  operator *( float a, BxVec3F b )
        {
            return new BxVec3F( a * b.X, a * b.Y, a * b.Z );
        }

        public static BxVec3F  operator /( BxVec3F a, float b )
        {
            return new BxVec3F( a.X / b, a.Y / b, a.Z / b );
        }

        // ------

        public float  Length {
            get {
                return ( float )Math.Sqrt( X * X + Y * Y + Z * Z );
            }
        }

        public float  LengthSquared {
            get {
                return X * X + Y * Y + Z * Z;
            }
        }

        // ------

        public BxVec3F  Normalize()
        {
            BxErr  errResult;

            BxVec3F  result = Normalize( out errResult );
            Debug.Assert( errResult == BxErr.KBxErrSuccess );

            return result;
        }

        public BxVec3F  Normalize( out BxErr errResult )
        {
            float  len = Length;

            if( len < KNearZeroF ) {
                errResult = BxErr.KBxErrFailure;
                return null;
            }
            else {
                errResult = BxErr.KBxErrSuccess;
                return new BxVec3F( this / len );
            }
        }
    }

    // -------------------------------------------------------

    public class BxBezier2Line3F : BxMathF {

        private readonly BxVec3F[]  fDt;

        // ------

        public BxBezier2Line3F()
        {
            fDt = new BxVec3F[ 3 ];

            for( byte i=0; i<3; i++ )
                fDt[ i ] = new BxVec3F();
        }

        public BxBezier2Line3F( BxBezier2Line3F a )
        {
            fDt = new BxVec3F[ 3 ];

            for( byte i=0; i<3; i++ )
                fDt[ i ] = new BxVec3F( a[ i ] );
        }

        public BxBezier2Line3F( BxVec3F[/*3*/] a )
        {
            fDt = new BxVec3F[ 3 ];

            for( byte i=0; i<3; i++ )
                fDt[ i ] = new BxVec3F( a[ i ] );
        }

        // ------

        public BxVec3F  this[ int idx ] {
            get {
                return fDt[ idx ];
            }
            set {
                fDt[ idx ].Set( value );
            }
        }

        public BxVec3F  this[ uint idx ] {
            get {
                return fDt[ idx ];
            }
            set {
                fDt[ idx ].Set( value );
            }
        }

        // ------

        public BxBezier2Line3F  Copy {
            get {
                return new BxBezier2Line3F( this );
            }
        }

        public void  Set( BxBezier2Line3F src )
        {
            for( byte i=0; i<3; i++ )
                fDt[ i ].Set( src[ i ] );
        }
    }

    // -------------------------------------------------------

    public class BxBezier3Line3F : BxMathF {

        private readonly BxVec3F[]  fDt;

        // ------

        public BxBezier3Line3F()
        {
            fDt = new BxVec3F[ 4 ];

            for( byte i=0; i<4; i++ )
                fDt[ i ] = new BxVec3F();
        }

        public BxBezier3Line3F( BxBezier3Line3F a )
        {
            fDt = new BxVec3F[ 4 ];

            for( byte i=0; i<4; i++ )
                fDt[ i ] = new BxVec3F( a[ i ] );
        }

        public BxBezier3Line3F( BxVec3F[/*4*/] a )
        {
            fDt = new BxVec3F[ 4 ];

            for( byte i=0; i<4; i++ )
                fDt[ i ] = new BxVec3F( a[ i ] );
        }

        // ------

        public BxVec3F  this[ int idx ] {
            get {
                return fDt[ idx ];
            }
            set {
                fDt[ idx ].Set( value );
            }
        }

        public BxVec3F  this[ uint idx ] {
            get {
                return fDt[ idx ];
            }
            set {
                fDt[ idx ].Set( value );
            }
        }

        // ------

        public BxBezier3Line3F  Copy {
            get {
                return new BxBezier3Line3F( this );
            }
        }

        public void  Set( BxBezier3Line3F src )
        {
            for( byte i=0; i<4; i++ )
                fDt[ i ].Set( src[ i ] );
        }

        // ------

        public BxBezier2Line3F  Diff()
        {
            BxBezier2Line3F  diff = new BxBezier2Line3F();

            for( byte i=0; i<BxVec3F.KNumVec3F; i++ ) {
                for( byte j=0; j<3; j++ )
                    diff[ j ][ i ] = 3.0F * ( this[ j+1 ][ i ] - this[ j ][ i ] );
            }

            return diff;
        }

        public void  GetIdxNoHandleZero( out sbyte lIdxNoHandleZero, out sbyte rIdxNoHandleZero )
        {
            float  lenBetweenVtx     = ( this[ 3 ] - this[ 0 ] ).LengthSquared;
            float  minimumLenSquared = lenBetweenVtx * KDirectionEpsRatioSquaredF;
            if( minimumLenSquared < ( KNearZeroF * KNearZeroF ) )
                minimumLenSquared = KNearZeroF * KNearZeroF;

            lIdxNoHandleZero = 0;
            for( byte i=0; i<3; i++ ) {
                if( ( this[ i+1 ] - this[ i ] ).LengthSquared >= minimumLenSquared )
                    break;

                lIdxNoHandleZero++;
            }

            rIdxNoHandleZero = 2;
            for( byte i=0; i<3; i++ ) {
                byte  j = ( byte )( 2 - i );
                if( ( this[ j+1 ] - this[ j ] ).LengthSquared >= minimumLenSquared )
                    break;

                rIdxNoHandleZero--;
            }

            if( lIdxNoHandleZero == 3 )
                rIdxNoHandleZero = 2;
        }

        public BxBezier6Line3F  UpperTo6()
        {
            BxBezier6Line3F  result = new BxBezier6Line3F();

            result[ 0 ].Set( this[ 0 ] );
            result[ 1 ].Set( ( this[ 0 ] + this[ 1 ] ) / 2.0F );
            result[ 2 ].Set( ( this[ 0 ] + 3.0F * this[ 1 ] + this[ 2 ] ) / 5.0F );
            result[ 3 ].Set( ( this[ 0 ] + 9.0F * this[ 1 ] + 9.0F * this[ 2 ] + this[ 3 ] ) / 20.0F );
            result[ 4 ].Set( ( this[ 1 ] + 3.0F * this[ 2 ] + this[ 3 ] ) / 5.0F );
            result[ 5 ].Set( ( this[ 2 ] + this[ 3 ] ) / 2.0F );
            result[ 6 ].Set( this[ 3 ] );

            return result;
        }
    }

    // -------------------------------------------------------

    public class BxBezier5Line3F : BxMathF {

        private readonly BxVec3F[]  fDt;

        // ------

        public BxBezier5Line3F()
        {
            fDt = new BxVec3F[ 6 ];

            for( byte i=0; i<6; i++ )
                fDt[ i ] = new BxVec3F();
        }

        public BxBezier5Line3F( BxBezier5Line3F a )
        {
            fDt = new BxVec3F[ 6 ];

            for( byte i=0; i<6; i++ )
                fDt[ i ] = new BxVec3F( a[ i ] );
        }

        public BxBezier5Line3F( BxVec3F[/*6*/] a )
        {
            fDt = new BxVec3F[ 6 ];

            for( byte i=0; i<6; i++ )
                fDt[ i ] = new BxVec3F( a[ i ] );
        }

        // ------

        public BxVec3F  this[ int idx ] {
            get {
                return fDt[ idx ];
            }
            set {
                fDt[ idx ].Set( value );
            }
        }

        public BxVec3F  this[ uint idx ] {
            get {
                return fDt[ idx ];
            }
            set {
                fDt[ idx ].Set( value );
            }
        }

        // ------

        public BxBezier5Line3F  Copy {
            get {
                return new BxBezier5Line3F( this );
            }
        }

        public void  Set( BxBezier5Line3F src )
        {
            for( byte i=0; i<6; i++ )
                fDt[ i ].Set( src[ i ] );
        }
    }

    // -------------------------------------------------------

    public class BxBezier6Line3F : BxMathF {

        private readonly BxVec3F[]  fDt;

        // ------

        public BxBezier6Line3F()
        {
            fDt = new BxVec3F[ 7 ];

            for( byte i=0; i<7; i++ )
                fDt[ i ] = new BxVec3F();
        }

        public BxBezier6Line3F( BxBezier6Line3F a )
        {
            fDt = new BxVec3F[ 7 ];

            for( byte i=0; i<7; i++ )
                fDt[ i ] = new BxVec3F( a[ i ] );
        }

        public BxBezier6Line3F( BxVec3F[/*7*/] a )
        {
            fDt = new BxVec3F[ 7 ];

            for( byte i=0; i<7; i++ )
                fDt[ i ] = new BxVec3F( a[ i ] );
        }

        // ------

        public BxVec3F  this[ int idx ] {
            get {
                return fDt[ idx ];
            }
            set {
                fDt[ idx ].Set( value );
            }
        }

        public BxVec3F  this[ uint idx ] {
            get {
                return fDt[ idx ];
            }
            set {
                fDt[ idx ].Set( value );
            }
        }

        // ------

        public BxBezier6Line3F  Copy {
            get {
                return new BxBezier6Line3F( this );
            }
        }

        public void  Set( BxBezier6Line3F src )
        {
            for( byte i=0; i<7; i++ )
                fDt[ i ].Set( src[ i ] );
        }

        // ------

        public BxBezier5Line3F  Diff()
        {
            BxBezier5Line3F  diff = new BxBezier5Line3F();

            for( byte i=0; i<BxVec3F.KNumVec3F; i++ ) {
                for( byte j=0; j<6; j++ )
                    diff[ j ][ i ] = 6.0F * ( this[ j+1 ][ i ] - this[ j ][ i ] );
            }

            return diff;
        }
    }
}
