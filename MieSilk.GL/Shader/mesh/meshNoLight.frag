#version 430 core
in float fragIndex;
uniform vec4[255] ObjectColor;

out vec4 FragColor;

void main()
{
    int i = int(fragIndex);
//    FragColor =vec4(1-ObjectColor[i].x,1-ObjectColor[i].y,1-ObjectColor[i].z,ObjectColor[i].w);
    FragColor =ObjectColor[i];
}