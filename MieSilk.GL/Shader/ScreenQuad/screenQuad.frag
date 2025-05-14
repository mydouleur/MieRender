#version 430 core

in vec2 vTexCoord;

out vec4 FragColor;


void main()
{
    vec4 topColor =  vec4(0.0, 0.3725, 0.9961, 1.0);
   // vec4 topColor =  vec4(0.5, 0.5, 0.5, 1.0);
    vec4 bottomColor = vec4(1.0, 1.0, 1.0, 1.0); 

    FragColor = mix(bottomColor, topColor, vTexCoord.y);
}