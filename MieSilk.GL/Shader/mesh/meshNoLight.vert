#version 430 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in float colorIndex;
out float fragIndex;


uniform mat4 uCameraMatrix;

void main()
{
    gl_Position =  uCameraMatrix* vec4(vPos, 1.0);
    fragIndex = colorIndex;
} 