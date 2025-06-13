#version 430 core
layout (location = 0) in vec3 vPos;

uniform mat4 uCameraMatrix;
uniform vec4 Plane1 = vec4(1,0,0,-10);

void main()
{
    gl_Position =  uCameraMatrix* vec4(vPos, 1.0);
    gl_ClipDistance[0] =Plane1[3] - Plane1[0]*vPos.x - Plane1[1]*vPos.y - Plane1[2]*vPos.z;
} 