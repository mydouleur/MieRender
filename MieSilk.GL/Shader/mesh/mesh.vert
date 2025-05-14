#version 430 core
layout (location = 0) in vec3 vPos;
layout (location = 1) in float colorIndex;


out float geomColor;
void main()
{
    gl_Position = vec4(vPos, 1.0);
    geomColor = colorIndex;
} 