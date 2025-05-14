#version 430 core
layout(points) in;
layout(triangle_strip, max_vertices = 4) out;
out vec2 vTexCoord;
uniform mat4 uCameraMatrix;
uniform float Width;
uniform float HWRatio;
uniform float BoardWidth = 200;
uniform float BoardHeight = 200;
void main()
{
    // Get the two input vertices
    vec4 v0 = gl_in[0].gl_Position;

    // Transform vertices to clip space
    vec4 projectedV0 = uCameraMatrix  * v0;

    // Calculate the direction of the line

    // Calculate the line width in clip space
    float halfSizeX =  BoardWidth/(2*Width);
    float halfSizeY = BoardHeight/(2*Width)/HWRatio;

    vTexCoord = vec2(0.0f,1.0f);
    gl_Position = projectedV0;
    EmitVertex();

    vTexCoord = vec2(0.0f,0.0f);
    gl_Position = projectedV0 +  vec4(0,halfSizeY,0,0);
    EmitVertex();

    vTexCoord = vec2(1.0f,1.0f);
    gl_Position = projectedV0 +  vec4(halfSizeX,0,0,0);
    EmitVertex();

    vTexCoord = vec2(1.0f,0.0f);
    gl_Position = projectedV0 + vec4(halfSizeX,halfSizeY,0,0);
    EmitVertex();

    EndPrimitive();

}