#version 430 core
layout(lines) in;
layout(line_strip, max_vertices = 2) out;
uniform mat4 uCameraMatrix;
in vec3 []VertPosition;
out vec3 StartPosition; 
out vec3 FragPosition;
void main()
{
       vec3 v0 = gl_in[0].gl_Position.xyz;

       StartPosition = v0;

       for (int i = 0; i < 2; i++) {
           FragPosition=VertPosition[i];
           gl_Position = uCameraMatrix* gl_in[i].gl_Position;
           EmitVertex();
       }
       EndPrimitive();
}