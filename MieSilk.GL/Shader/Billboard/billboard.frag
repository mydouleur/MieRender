#version 430 core


in vec2 vTexCoord;
uniform sampler2D uTexture;
out vec4 FragColor;

void main()
{
        FragColor = texture(uTexture,vTexCoord);
//        FragColor = vec4(1,0,0,1);
}