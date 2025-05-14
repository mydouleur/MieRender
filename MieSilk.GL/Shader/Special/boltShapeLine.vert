#version 430 core
layout (location = 0) in vec3 vPos;
layout(location =1) in float param; 
out float params;
void main()
{
    gl_Position =  vec4(vPos, 1.0);
    params = param;
} 