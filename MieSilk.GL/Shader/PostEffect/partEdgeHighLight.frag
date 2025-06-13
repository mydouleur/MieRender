#version 430 core
in vec2 vTexCoord;
out vec4 FragColor;

uniform sampler2D uTexture;
uniform vec2 uTexelSize;
uniform vec4 uBackground = vec4(1,1,1,1);
void main()
{
    vec4 color = texture(uTexture, vTexCoord);

    float thickness = 5;

    vec4 colorLeft = texture(uTexture, vTexCoord + vec2(-uTexelSize.x*thickness, 0.0));
    vec4 colorRight = texture(uTexture, vTexCoord + vec2(uTexelSize.x*thickness, 0.0));
    vec4 colorTop = texture(uTexture, vTexCoord + vec2(0.0, uTexelSize.y*thickness));
    vec4 colorBottom = texture(uTexture, vTexCoord + vec2(0.0, -uTexelSize.y*thickness));


   float edge = 0.0;
//    edge += length(color.rgb - colorLeft.rgb);
//    edge += length(color.rgb - colorRight.rgb);
//    edge += length(color.rgb - colorTop.rgb);
//    edge += length(color.rgb - colorBottom.rgb);
    edge += length(uBackground.rgb-colorLeft.rgb);
    edge += length(uBackground.rgb-colorRight.rgb);
    edge += length(uBackground.rgb-colorTop.rgb);
    edge += length(uBackground.rgb-colorBottom.rgb);
    if (color == uBackground && edge!=0)
    {
        FragColor = vec4(1.0, 0.0, 0.0, 1.0);
    }
    else
    {
        FragColor =vec4(0.0, 0.0, 0.0, 0.0);
    }
}