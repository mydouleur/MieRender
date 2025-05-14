#version 430 core


uniform float dashSize = 3;
uniform float gapSize = 2.0;
in vec3 StartPosition;
in vec3 FragPosition;

out vec4 FragColor;

void main()
{
    float dis = length(FragPosition-StartPosition);
    float modValue = mod(dis, dashSize + gapSize);
    if (modValue < dashSize)
    {
        FragColor = vec4(0, 0, 0, 1.0);
    }
    else
    {
        discard;
    }
}