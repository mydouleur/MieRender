#version 430 core
layout(triangles) in;
layout(triangle_strip, max_vertices = 3) out;
uniform vec4 Plane1 = vec4(0,0,-1,-100);

in float[] geomColor;

uniform mat4 uCameraMatrix;
out vec3 fNormal;
out float fragIndex;
void main()
{
       vec3 v0 = gl_in[0].gl_Position.xyz;
       vec3 v1 = gl_in[1].gl_Position.xyz;
       vec3 v2 = gl_in[2].gl_Position.xyz;


       vec3 edge1 = v1 - v0;
       vec3 edge2 = v2 - v0;


       vec3 normal = normalize(cross(edge2, edge1));

       fNormal = normal;

       
       for (int i = 0; i < 3; i++) {
           vec3 vPos = gl_in[i].gl_Position.xyz;
           gl_Position = uCameraMatrix* gl_in[i].gl_Position;
           fragIndex = geomColor[i];
           gl_ClipDistance[0] =Plane1[3] - Plane1[0]*vPos.x - Plane1[1]*vPos.y - Plane1[2]*vPos.z;
           EmitVertex();
       }
       EndPrimitive();
}