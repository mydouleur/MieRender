#version 430 core
layout(lines) in;
layout(triangle_strip, max_vertices = 4) out;
uniform mat4 uCameraMatrix;
uniform float Width;
uniform float LineWidth = 2;
void main()
{
    // Get the two input vertices
    vec4 v0 = gl_in[0].gl_Position;
    vec4 v1 = gl_in[1].gl_Position;

    // Transform vertices to clip space
    vec4 projectedV0 = uCameraMatrix  * v0;
    vec4 projectedV1 = uCameraMatrix  * v1;

    // Calculate the direction of the line
    vec2 dir = normalize((projectedV1.xy / projectedV1.w) - (projectedV0.xy / projectedV0.w));
    vec2 normal = vec2(-dir.y, dir.x); // Direction perpendicular to the line

    // Calculate the line width in clip space
    float width = LineWidth / Width; // Assume Height is the screen height

    // Generate the four vertices of the quadrilateral
    vec4 offset = vec4(normal * width, 0.0, 0.0);
    gl_Position = projectedV0 - offset;
    EmitVertex();

    gl_Position = projectedV0 + offset;
    EmitVertex();

    gl_Position = projectedV1 - offset;
    EmitVertex();

    gl_Position = projectedV1 + offset;
    EmitVertex();

    EndPrimitive();

}