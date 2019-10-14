#version 400

layout( vertices = 6 ) out;

in vec3  tescPnt0[];
in vec3  tescPnt1[];
in vec3  tescPnt2[];
in vec3  tescPnt3[];
in vec3  tescPnt4[];
in vec3  tescPnt5[];
in vec3  tescPnt6[];
in vec3  tescPnt7[];
in vec3  tescPnt8[];
in vec3  tescPnt9[];
in vec3  tescPnt10[];
in vec3  tescPnt11[];
in vec3  tescPnt12[];
in vec3  tescPnt13[];

out vec3  tesePnt0[];
out vec3  tesePnt1[];
out vec3  tesePnt2[];
out vec3  tesePnt3[];
out vec3  tesePnt4[];
out vec3  tesePnt5[];
out vec3  tesePnt6[];
out vec3  tesePnt7[];
out vec3  tesePnt8[];
out vec3  tesePnt9[];
out vec3  tesePnt10[];
out vec3  tesePnt11[];
out vec3  tesePnt12[];
out vec3  tesePnt13[];

uniform float  tessLevel;


void main()
{
	tesePnt0[  gl_InvocationID ] = tescPnt0[  gl_InvocationID ];
	tesePnt1[  gl_InvocationID ] = tescPnt1[  gl_InvocationID ];
	tesePnt2[  gl_InvocationID ] = tescPnt2[  gl_InvocationID ];
	tesePnt3[  gl_InvocationID ] = tescPnt3[  gl_InvocationID ];
	tesePnt4[  gl_InvocationID ] = tescPnt4[  gl_InvocationID ];
	tesePnt5[  gl_InvocationID ] = tescPnt5[  gl_InvocationID ];
	tesePnt6[  gl_InvocationID ] = tescPnt6[  gl_InvocationID ];
	tesePnt7[  gl_InvocationID ] = tescPnt7[  gl_InvocationID ];
	tesePnt8[  gl_InvocationID ] = tescPnt8[  gl_InvocationID ];
	tesePnt9[  gl_InvocationID ] = tescPnt9[  gl_InvocationID ];
	tesePnt10[ gl_InvocationID ] = tescPnt10[ gl_InvocationID ];
	tesePnt11[ gl_InvocationID ] = tescPnt11[ gl_InvocationID ];
	tesePnt12[ gl_InvocationID ] = tescPnt12[ gl_InvocationID ];
	tesePnt13[ gl_InvocationID ] = tescPnt13[ gl_InvocationID ];

	if( gl_InvocationID == 0 ) {
        float  tessDenom = tescPnt12[ 2 ].x;

		gl_TessLevelOuter[ 0 ] = tessLevel / tessDenom;
		gl_TessLevelOuter[ 1 ] = tessLevel / tessDenom;
		gl_TessLevelOuter[ 2 ] = tessLevel / tessDenom;
		gl_TessLevelOuter[ 3 ] = tessLevel / tessDenom;

		gl_TessLevelInner[ 0 ] = tessLevel / tessDenom;
		gl_TessLevelInner[ 1 ] = tessLevel / tessDenom;
	}
}
