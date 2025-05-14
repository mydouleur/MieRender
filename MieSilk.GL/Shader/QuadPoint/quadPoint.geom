#version 430 core
layout(points) in;
layout(triangle_strip, max_vertices = 4) out;
uniform mat4 uCameraMatrix;
uniform float Width;
uniform float HWRatio;
uniform float PointSize = 20;
void main()
{
    // Get the two input vertices
    vec4 v0 = gl_in[0].gl_Position;

    // Transform vertices to clip space
    vec4 projectedV0 = uCameraMatrix  * v0;

    // Calculate the direction of the line

    // Calculate the line width in clip space
    float halfSizeX =  PointSize/(2*Width);
    float halfSizeY = halfSizeX/HWRatio;

    gl_Position = projectedV0 + vec4(halfSizeX,halfSizeY,0,0);
    EmitVertex();

    gl_Position = projectedV0 +  vec4(halfSizeX,-halfSizeY,0,0);
    EmitVertex();

    gl_Position = projectedV0 +  vec4(-halfSizeX,halfSizeY,0,0);
    EmitVertex();

    gl_Position = projectedV0 + vec4(-halfSizeX,-halfSizeY,0,0);
    EmitVertex();

    EndPrimitive();

}