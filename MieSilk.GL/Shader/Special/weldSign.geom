#version 430 core
layout(points) in;
layout(line_strip, max_vertices = 3) out;
uniform mat4 uCameraMatrix;
uniform float Width;
uniform float HWRatio;
uniform float SignSize;
uniform float xLength = 80;
void main()
{
    vec4 v0 = gl_in[0].gl_Position;
    vec4 projectedV0 = uCameraMatrix  * v0;
    float halfSizeX = (SignSize*xLength) /(2*Width);
    float halfSizeY = halfSizeX/HWRatio;
    gl_Position = projectedV0;
    EmitVertex();
    gl_Position = projectedV0 + vec4(halfSizeX,halfSizeY,0,0);
    EmitVertex();
    gl_Position = projectedV0 + vec4(halfSizeX*2,halfSizeY,0,0);
    EmitVertex();


    EndPrimitive();

}