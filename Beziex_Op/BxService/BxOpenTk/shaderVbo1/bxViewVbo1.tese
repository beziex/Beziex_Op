#version 400

layout( quads ) in;

// --------

in vec3  tesePnt0[];
in vec3  tesePnt1[];
in vec3  tesePnt2[];
in vec3  tesePnt3[];
in vec3  tesePnt4[];
in vec3  tesePnt5[];
in vec3  tesePnt6[];
in vec3  tesePnt7[];
in vec3  tesePnt8[];
in vec3  tesePnt9[];
in vec3  tesePnt10[];
in vec3  tesePnt11[];
in vec3  tesePnt12[];
in vec3  tesePnt13[];

// --------

uniform mat4  viewMatrix;
uniform mat4  projMatrix;
uniform mat3  normalMatrix;

uniform vec3   mtlAmb;
uniform vec3   mtlDif;
uniform vec3   mtlSpec;
uniform float  mtlShin;

uniform vec3  lightAmb;
uniform vec3  lightDif;
uniform vec3  lightSpec;
uniform vec3  lightVec;

out vec3  fragFrontColor;
out vec3  fragBackColor;


// --------

const float  kEps = 1e-6;

// ------------------------------------------------------------------------------------------

vec3 posBez3( vec3 bez[4], float t )
{
    float  t2  = t  * t;
    float  t3  = t2 * t;
    float  mt  = 1.0 - t;
    float  mt2 = mt  * mt;
    float  mt3 = mt2 * mt;

    return            mt3 * bez[ 0 ]
         + 3.0 * t  * mt2 * bez[ 1 ]
         + 3.0 * t2 * mt  * bez[ 2 ]
         +       t3       * bez[ 3 ];
}

vec3 posBez6( vec3 bez[7], float t )
{
    float  t2  = t  * t;
    float  t3  = t2 * t;
    float  t4  = t3 * t;
    float  t5  = t4 * t;
    float  t6  = t5 * t;
    float  mt  = 1.0 - t;
    float  mt2 = mt  * mt;
    float  mt3 = mt2 * mt;
    float  mt4 = mt3 * mt;
    float  mt5 = mt4 * mt;
    float  mt6 = mt5 * mt;

    return             mt6 * bez[ 0 ]
         +  6.0 * t  * mt5 * bez[ 1 ]
         + 15.0 * t2 * mt4 * bez[ 2 ]
         + 20.0 * t3 * mt3 * bez[ 3 ]
         + 15.0 * t4 * mt2 * bez[ 4 ]
         +  6.0 * t5 * mt  * bez[ 5 ]
         +        t6       * bez[ 6 ];
}

void getPosH( vec3 hPosBez0[4], vec3 hPosBez1[7], vec3 hPosBez2[7], vec3 hPosBez3[4], float u, float v, out vec3 posH )
{
    vec3  bezV[ 4 ];

    bezV[ 0 ] = posBez3( hPosBez0, u );
    bezV[ 1 ] = posBez6( hPosBez1, u );
    bezV[ 2 ] = posBez6( hPosBez2, u );
    bezV[ 3 ] = posBez3( hPosBez3, u );

    posH = posBez3( bezV, v );
}

void getPosV( vec3 vPosBez0[4], vec3 vPosBez1[7], vec3 vPosBez2[7], vec3 vPosBez3[4], float u, float v, out vec3 posV )
{
    vec3  bezU[ 4 ];

    bezU[ 0 ] = posBez3( vPosBez0, v );
    bezU[ 1 ] = posBez6( vPosBez1, v );
    bezU[ 2 ] = posBez6( vPosBez2, v );
    bezU[ 3 ] = posBez3( vPosBez3, v );

    posV = posBez3( bezU, u );
}

vec3 getPosMixMain( vec3 posH, vec3 posV, float u, float v )
{
    float  pu = u * ( 1.0 - u );
    float  pv = v * ( 1.0 - v );

    return ( pu * posH + pv * posV ) / ( pu + pv );
}

void getPosMix( vec3 posH, vec3 posV, float u, float v, out vec3 pos )
{
    pos = ( ( kEps > u || u > ( 1.0 - kEps ) ) && ( kEps > v || v > ( 1.0 - kEps ) ) ) ? posH : getPosMixMain( posH, posV, u, v );
}

void getPosition( vec3 hPosBez0[4], vec3 hPosBez1[7], vec3 hPosBez2[7], vec3 hPosBez3[4], vec3 vPosBez0[4], vec3 vPosBez1[7], vec3 vPosBez2[7], vec3 vPosBez3[4],
                  float u, float v, out vec3 posH, out vec3 posV, out vec3 pos )
{
    getPosH( hPosBez0, hPosBez1, hPosBez2, hPosBez3, u, v, posH );
    getPosV( vPosBez0, vPosBez1, vPosBez2, vPosBez3, u, v, posV );

    getPosMix( posH, posV, u, v, pos );
}

// --------------------------------

vec3 posBez2( vec3 bez[3], float t )
{
    float  t2  = t * t;
    float  mt  = 1.0 - t;
    float  mt2 = mt * mt;

    return            mt2 * bez[ 0 ]
         + 2.0 * t  * mt  * bez[ 1 ]
         +       t2       * bez[ 2 ];
}

vec3 posBez5( vec3 bez[6], float t )
{
    float  t2  = t  * t;
    float  t3  = t2 * t;
    float  t4  = t3 * t;
    float  t5  = t4 * t;
    float  mt  = 1.0 - t;
    float  mt2 = mt  * mt;
    float  mt3 = mt2 * mt;
    float  mt4 = mt3 * mt;
    float  mt5 = mt4 * mt;

    return             mt5 * bez[ 0 ]
         +  5.0 * t  * mt4 * bez[ 1 ]
         + 10.0 * t2 * mt3 * bez[ 2 ]
         + 10.0 * t3 * mt2 * bez[ 3 ]
         +  5.0 * t4 * mt  * bez[ 4 ]
         +        t5       * bez[ 5 ];
}

vec3 diffBez3( vec3 bez[4], float t )
{
    float  t2  = t * t;
    float  mt  = 1.0 - t;
    float  mt2 = mt * mt;
    float  d3t = 3.0 * t;

    return 3.0 * ( -mt2                * bez[ 0 ]
                 +  mt * ( 1.0 - d3t ) * bez[ 1 ]
                 +  t  * ( 2.0 - d3t ) * bez[ 2 ]
                 +  t2                 * bez[ 3 ] );
}

void getDiffH_U( vec3 hDiffBez0_U[3], vec3 hDiffBez1_U[6], vec3 hDiffBez2_U[6], vec3 hDiffBez3_U[3], float u, float v, out vec3 diffH_U )
{
    vec3  bezV[ 4 ];

    bezV[ 0 ] = posBez2( hDiffBez0_U, u );
    bezV[ 1 ] = posBez5( hDiffBez1_U, u );
    bezV[ 2 ] = posBez5( hDiffBez2_U, u );
    bezV[ 3 ] = posBez2( hDiffBez3_U, u );

    diffH_U = posBez3( bezV, v );
}

void getDiffV_U( vec3 vPosBez0_U[4], vec3 vPosBez1_U[7], vec3 vPosBez2_U[7], vec3 vPosBez3_U[4], float u, float v, out vec3 diffV_U )
{
    vec3  bezU[ 4 ];

    bezU[ 0 ] = posBez3( vPosBez0_U, v );
    bezU[ 1 ] = posBez6( vPosBez1_U, v );
    bezU[ 2 ] = posBez6( vPosBez2_U, v );
    bezU[ 3 ] = posBez3( vPosBez3_U, v );

    diffV_U = diffBez3( bezU, u );
}

vec3 getDiffMixMain_U( vec3 diffH_U, vec3 diffV_U, float u, float v, vec3 posH, vec3 posV )
{
    float  pu = u * ( 1.0 - u );
    float  du = -2.0 * u + 1.0;
    float  pv = v * ( 1.0 - v );

    float  posDenom   = pu + pv;
    float  diffDenomU = du;
    
    vec3  posNumer = pu * posH + pv * posV;

    vec3  diffNumerU0 = pu * diffH_U + du * posH;
    vec3  diffNumerU1 = pv * diffV_U;
    vec3  diffNumerU  = diffNumerU0 + diffNumerU1;

    vec3  numerU = diffNumerU * posDenom - posNumer * diffDenomU;

    return numerU / ( posDenom * posDenom );
}

void getDiffMix_U( vec3 diffH_U, vec3 diffV_U, float u, float v, vec3 posH, vec3 posV, out vec3 diffU )
{
    diffU = ( ( kEps > u || u > ( 1.0 - kEps ) ) && ( kEps > v || v > ( 1.0 - kEps ) ) ) ? diffH_U : getDiffMixMain_U( diffH_U, diffV_U, u, v, posH, posV );
}

void getDiff_U( vec3 hDiffBez0_U[3], vec3 hDiffBez1_U[6], vec3 hDiffBez2_U[6], vec3 hDiffBez3_U[3], vec3 vPosBez0_U[4], vec3 vPosBez1_U[7], vec3 vPosBez2_U[7], vec3 vPosBez3_U[4],
                float u, float v, vec3 posH, vec3 posV, out vec3 diffUE )
{
    vec3  diffH_U, diffV_U;

    getDiffH_U( hDiffBez0_U, hDiffBez1_U, hDiffBez2_U, hDiffBez3_U, u, v, diffH_U );
    getDiffV_U( vPosBez0_U, vPosBez1_U, vPosBez2_U, vPosBez3_U, u, v, diffV_U );

    vec3  diffU;
    getDiffMix_U( diffH_U, diffV_U, u, v, posH, posV, diffU );

    diffUE = normalize( diffU );
}

void getDiffH_V( vec3 hPosBez0_V[4], vec3 hPosBez1_V[7], vec3 hPosBez2_V[7], vec3 hPosBez3_V[4], float u, float v, out vec3 diffH_V )
{
    vec3  bezV[ 4 ];

    bezV[ 0 ] = posBez3( hPosBez0_V, u );
    bezV[ 1 ] = posBez6( hPosBez1_V, u );
    bezV[ 2 ] = posBez6( hPosBez2_V, u );
    bezV[ 3 ] = posBez3( hPosBez3_V, u );

    diffH_V = diffBez3( bezV, v );
}

void getDiffV_V( vec3 vDiffBez0_V[3], vec3 vDiffBez1_V[6], vec3 vDiffBez2_V[6], vec3 vDiffBez3_V[3], float u, float v, out vec3 diffV_V )
{
    vec3  bezU[ 4 ];

    bezU[ 0 ] = posBez2( vDiffBez0_V, v );
    bezU[ 1 ] = posBez5( vDiffBez1_V, v );
    bezU[ 2 ] = posBez5( vDiffBez2_V, v );
    bezU[ 3 ] = posBez2( vDiffBez3_V, v );

    diffV_V = posBez3( bezU, u );
}

vec3 getDiffMixMain_V( vec3 diffH_V, vec3 diffV_V, float u, float v, vec3 posH, vec3 posV )
{
    float  pu = u * ( 1.0 - u );
    float  pv = v * ( 1.0 - v );
    float  dv = -2.0 * v + 1.0;

    float  posDenom   = pu + pv;
    float  diffDenomV = dv;

    vec3  posNumer = pu * posH + pv * posV;

    vec3  diffNumerV0 = pu * diffH_V;
    vec3  diffNumerV1 = pv * diffV_V + dv * posV;
    vec3  diffNumerV  = diffNumerV0 + diffNumerV1;

    vec3  numerV = diffNumerV * posDenom - posNumer * diffDenomV;

    return numerV / ( posDenom * posDenom );
}

void getDiffMix_V( vec3 diffH_V, vec3 diffV_V, float u, float v, vec3 posH, vec3 posV, out vec3 diffV )
{
    diffV = ( ( kEps > u || u > ( 1.0 - kEps ) ) && ( kEps > v || v > ( 1.0 - kEps ) ) ) ? diffH_V : getDiffMixMain_V( diffH_V, diffV_V, u, v, posH, posV );
}

void getDiff_V( vec3 hPosBez0_V[4], vec3 hPosBez1_V[7], vec3 hPosBez2_V[7], vec3 hPosBez3_V[4], vec3 vDiffBez0_V[3], vec3 vDiffBez1_V[6], vec3 vDiffBez2_V[6], vec3 vDiffBez3_V[3],
                float u, float v, vec3 posH, vec3 posV, out vec3 diffVE )
{
    vec3  diffH_V, diffV_V;

    getDiffH_V( hPosBez0_V, hPosBez1_V, hPosBez2_V, hPosBez3_V, u, v, diffH_V );
    getDiffV_V( vDiffBez0_V, vDiffBez1_V, vDiffBez2_V, vDiffBez3_V, u, v, diffV_V );

    vec3  diffV;
    getDiffMix_V( diffH_V, diffV_V, u, v, posH, posV, diffV );

    diffVE = normalize( diffV );
}

void getNormalE( vec3 hDiffBez0_U[3], vec3 hDiffBez1_U[6], vec3 hDiffBez2_U[6], vec3 hDiffBez3_U[3], vec3 vPosBez0_U[4], vec3 vPosBez1_U[7], vec3 vPosBez2_U[7], vec3 vPosBez3_U[4],
                 vec3 hPosBez0_V[4], vec3 hPosBez1_V[7], vec3 hPosBez2_V[7], vec3 hPosBez3_V[4], vec3 vDiffBez0_V[3], vec3 vDiffBez1_V[6], vec3 vDiffBez2_V[6], vec3 vDiffBez3_V[3],
                 float u, float v, vec3 posH, vec3 posV, out vec3 normalE )
{
    vec3  diffUE, diffVE;

    getDiff_U( hDiffBez0_U, hDiffBez1_U, hDiffBez2_U, hDiffBez3_U, vPosBez0_U, vPosBez1_U, vPosBez2_U, vPosBez3_U, u, v, posH, posV, diffUE );
    getDiff_V( hPosBez0_V, hPosBez1_V, hPosBez2_V, hPosBez3_V, vDiffBez0_V, vDiffBez1_V, vDiffBez2_V, vDiffBez3_V, u, v, posH, posV, diffVE );

    normalE = normalize( cross( diffUE, diffVE ) );
}

// --------------------------------

void fromVbo( out vec3 hPosBez0[4], out vec3 hPosBez1[7], out vec3 hPosBez2[7], out vec3 hPosBez3[4], out vec3 vPosBez0[4], out vec3 vPosBez1[7], out vec3 vPosBez2[7], out vec3 vPosBez3[4],
               out vec3 hDiffBez0[3], out vec3 hDiffBez1[6], out vec3 hDiffBez2[6], out vec3 hDiffBez3[3], out vec3 vDiffBez0[3], out vec3 vDiffBez1[6], out vec3 vDiffBez2[6], out vec3 vDiffBez3[3] )
{
    hPosBez0[ 0 ] = tesePnt0[  0 ];
    hPosBez0[ 1 ] = tesePnt1[  0 ];
    hPosBez0[ 2 ] = tesePnt2[  0 ];
    hPosBez0[ 3 ] = tesePnt3[  0 ];

    hPosBez1[ 0 ] = tesePnt4[  0 ];
    hPosBez1[ 1 ] = tesePnt5[  0 ];
    hPosBez1[ 2 ] = tesePnt6[  0 ];
    hPosBez1[ 3 ] = tesePnt7[  0 ];
    hPosBez1[ 4 ] = tesePnt8[  0 ];
    hPosBez1[ 5 ] = tesePnt9[  0 ];
    hPosBez1[ 6 ] = tesePnt10[ 0 ];

    hPosBez2[ 0 ] = tesePnt0[  1 ];
    hPosBez2[ 1 ] = tesePnt1[  1 ];
    hPosBez2[ 2 ] = tesePnt2[  1 ];
    hPosBez2[ 3 ] = tesePnt3[  1 ];
    hPosBez2[ 4 ] = tesePnt4[  1 ];
    hPosBez2[ 5 ] = tesePnt5[  1 ];
    hPosBez2[ 6 ] = tesePnt6[  1 ];

    hPosBez3[ 0 ] = tesePnt7[  1 ];
    hPosBez3[ 1 ] = tesePnt8[  1 ];
    hPosBez3[ 2 ] = tesePnt9[  1 ];
    hPosBez3[ 3 ] = tesePnt10[ 1 ];

    vPosBez0[ 0 ] = tesePnt0[  3 ];
    vPosBez0[ 1 ] = tesePnt1[  3 ];
    vPosBez0[ 2 ] = tesePnt2[  3 ];
    vPosBez0[ 3 ] = tesePnt3[  3 ];

    vPosBez1[ 0 ] = tesePnt4[  3 ];
    vPosBez1[ 1 ] = tesePnt5[  3 ];
    vPosBez1[ 2 ] = tesePnt6[  3 ];
    vPosBez1[ 3 ] = tesePnt7[  3 ];
    vPosBez1[ 4 ] = tesePnt8[  3 ];
    vPosBez1[ 5 ] = tesePnt9[  3 ];
    vPosBez1[ 6 ] = tesePnt10[ 3 ];

    vPosBez2[ 0 ] = tesePnt0[  4 ];
    vPosBez2[ 1 ] = tesePnt1[  4 ];
    vPosBez2[ 2 ] = tesePnt2[  4 ];
    vPosBez2[ 3 ] = tesePnt3[  4 ];
    vPosBez2[ 4 ] = tesePnt4[  4 ];
    vPosBez2[ 5 ] = tesePnt5[  4 ];
    vPosBez2[ 6 ] = tesePnt6[  4 ];

    vPosBez3[ 0 ] = tesePnt7[  4 ];
    vPosBez3[ 1 ] = tesePnt8[  4 ];
    vPosBez3[ 2 ] = tesePnt9[  4 ];
    vPosBez3[ 3 ] = tesePnt10[ 4 ];

    hDiffBez0[ 0 ] = tesePnt11[ 0 ];
    hDiffBez0[ 1 ] = tesePnt12[ 0 ];
    hDiffBez0[ 2 ] = tesePnt13[ 0 ];

    hDiffBez1[ 0 ] = tesePnt0[  2 ];
    hDiffBez1[ 1 ] = tesePnt1[  2 ];
    hDiffBez1[ 2 ] = tesePnt2[  2 ];
    hDiffBez1[ 3 ] = tesePnt3[  2 ];
    hDiffBez1[ 4 ] = tesePnt4[  2 ];
    hDiffBez1[ 5 ] = tesePnt5[  2 ];

    hDiffBez2[ 0 ] = tesePnt6[  2 ];
    hDiffBez2[ 1 ] = tesePnt7[  2 ];
    hDiffBez2[ 2 ] = tesePnt8[  2 ];
    hDiffBez2[ 3 ] = tesePnt9[  2 ];
    hDiffBez2[ 4 ] = tesePnt10[ 2 ];
    hDiffBez2[ 5 ] = tesePnt11[ 2 ];

    hDiffBez3[ 0 ] = tesePnt11[ 1 ];
    hDiffBez3[ 1 ] = tesePnt12[ 1 ];
    hDiffBez3[ 2 ] = tesePnt13[ 1 ];

    vDiffBez0[ 0 ] = tesePnt11[ 3 ];
    vDiffBez0[ 1 ] = tesePnt12[ 3 ];
    vDiffBez0[ 2 ] = tesePnt13[ 3 ];

    vDiffBez1[ 0 ] = tesePnt0[  5 ];
    vDiffBez1[ 1 ] = tesePnt1[  5 ];
    vDiffBez1[ 2 ] = tesePnt2[  5 ];
    vDiffBez1[ 3 ] = tesePnt3[  5 ];
    vDiffBez1[ 4 ] = tesePnt4[  5 ];
    vDiffBez1[ 5 ] = tesePnt5[  5 ];

    vDiffBez2[ 0 ] = tesePnt6[  5 ];
    vDiffBez2[ 1 ] = tesePnt7[  5 ];
    vDiffBez2[ 2 ] = tesePnt8[  5 ];
    vDiffBez2[ 3 ] = tesePnt9[  5 ];
    vDiffBez2[ 4 ] = tesePnt10[ 5 ];
    vDiffBez2[ 5 ] = tesePnt11[ 5 ];

    vDiffBez3[ 0 ] = tesePnt11[ 4 ];
    vDiffBez3[ 1 ] = tesePnt12[ 4 ];
    vDiffBez3[ 2 ] = tesePnt13[ 4 ];
}

void getPosNormal( out vec3 pos, out vec3 normalE )
{
    float  u = gl_TessCoord.x;
    float  v = gl_TessCoord.y;

    vec3  hPosBez0[ 4 ],  hPosBez1[ 7 ],  hPosBez2[ 7 ],  hPosBez3[ 4 ],  vPosBez0[ 4 ],  vPosBez1[ 7 ],  vPosBez2[ 7 ],  vPosBez3[ 4 ];
    vec3  hDiffBez0[ 3 ], hDiffBez1[ 6 ], hDiffBez2[ 6 ], hDiffBez3[ 3 ], vDiffBez0[ 3 ], vDiffBez1[ 6 ], vDiffBez2[ 6 ], vDiffBez3[ 3 ];
    fromVbo( hPosBez0, hPosBez1, hPosBez2, hPosBez3, vPosBez0, vPosBez1, vPosBez2, vPosBez3, hDiffBez0, hDiffBez1, hDiffBez2, hDiffBez3, vDiffBez0, vDiffBez1, vDiffBez2, vDiffBez3 );
 
    vec3  posH, posV;
    getPosition( hPosBez0, hPosBez1, hPosBez2, hPosBez3, vPosBez0, vPosBez1, vPosBez2, vPosBez3, u, v, posH, posV, pos );

    getNormalE( hDiffBez0, hDiffBez1, hDiffBez2, hDiffBez3, vPosBez0, vPosBez1, vPosBez2, vPosBez3, hPosBez0, hPosBez1, hPosBez2, hPosBez3,
                vDiffBez0, vDiffBez1, vDiffBez2, vDiffBez3, u, v, posH, posV, normalE );
}

// ------------------------------------------------------------------------------------------

vec3 getSpecular( vec3 vertexPos, vec3 vertexNormal )
{
    vec3  viewVec = normalize( -vertexPos );

//  vec3   reflectVec = reflect( -lightVec, vertexNormal );
//  return lightSpec * mtlSpec * pow( max( dot( reflectVec, viewVec ), 0.0 ), mtlShin );

    vec3   halfVec = normalize( lightVec + viewVec );
    return lightSpec * mtlSpec * pow( max( dot( vertexNormal, halfVec ), 0.0 ), mtlShin );
}

vec3 getColor( vec3 vertexPos, vec3 vertexNormal )
{
    vec3   ambient = lightAmb * mtlAmb;

    float  difFactor = max( dot( lightVec, vertexNormal ), 0.0 );
    vec3   diffuse   = lightDif * mtlDif * difFactor;

    vec3   spec = ( difFactor <= 0.0 ) ? vec3( 0.0 ) : getSpecular( vertexPos, vertexNormal );

    return ambient + diffuse + spec;
}

void main()
{
    vec3  pos, normalE;
    getPosNormal( pos, normalE );

    vec4  pos4 = vec4( pos, 1.0 );

    gl_Position = projMatrix * viewMatrix * pos4;

    vec3  vertexPos    = ( viewMatrix * pos4 ).xyz;
    vec3  vertexNormal = normalize( normalMatrix * normalE );

    fragFrontColor = getColor( vertexPos,  vertexNormal );
    fragBackColor  = getColor( vertexPos, -vertexNormal );
}
