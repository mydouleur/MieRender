#version 430 core
in float fragIndex;
out vec4 FragColor;
uniform vec4[255] ObjectColor;

void main()
{
       int i = int(fragIndex);
        FragColor =ObjectColor[i];
}