#version 430 core
layout(points) in;
layout(line_strip, max_vertices = 66) out;
uniform mat4 uCameraMatrix;
uniform float Width;
uniform float HWRatio;
uniform float SignSize;
void main()
{
    vec4 v0 = gl_in[0].gl_Position;
    vec4 projectedV0 = uCameraMatrix  * v0;
    float unitX= SignSize*2/Width;
    float unitY= unitX/HWRatio;
    //midPoint
    gl_Position = projectedV0+vec4(0,0.5*unitY,0,0);
    EmitVertex();
    gl_Position = projectedV0+vec4(0,-0.5*unitY,0,0);
    EmitVertex();
    EndPrimitive();
    //circle
    for(int i = 0;i<48;i++)
    {
       float rad = radians(i*7.5);
       gl_Position = projectedV0+vec4(sin(rad)*40*unitX,cos(rad)*40*unitY,0,0);
       EmitVertex();
       float temp = mod(i,2);
       if(temp==1)
       {
           EndPrimitive();
       }
    }
    //doubleLine
    gl_Position = projectedV0+vec4(16*unitX,24*unitY,0,0);
    EmitVertex();
    gl_Position = projectedV0+vec4(28*unitX,35*unitY,0,0);
    EmitVertex();
    EndPrimitive();
    gl_Position = projectedV0+vec4(20*unitX,20*unitY,0,0);
    EmitVertex();
    gl_Position = projectedV0+vec4(31*unitX,31*unitY,0,0);
    EmitVertex();
    EndPrimitive();
    gl_Position = projectedV0+vec4(-16*unitX,24*unitY,0,0);
    EmitVertex();
    gl_Position = projectedV0+vec4(-28*unitX,35*unitY,0,0);
    EmitVertex();
    EndPrimitive();
    gl_Position = projectedV0+vec4(-20*unitX,20*unitY,0,0);
    EmitVertex();
    gl_Position = projectedV0+vec4(-31*unitX,31*unitY,0,0);
    EmitVertex();
    EndPrimitive();
    gl_Position = projectedV0+vec4(-16*unitX,-24*unitY,0,0);
    EmitVertex();
    gl_Position = projectedV0+vec4(-28*unitX,-35*unitY,0,0);
    EmitVertex();
    EndPrimitive();
    gl_Position = projectedV0+vec4(-20*unitX,-20*unitY,0,0);
    EmitVertex();
    gl_Position = projectedV0+vec4(-31*unitX,-31*unitY,0,0);
    EmitVertex();
    EndPrimitive();
    gl_Position = projectedV0+vec4(16*unitX,-24*unitY,0,0);
    EmitVertex();
    gl_Position = projectedV0+vec4(28*unitX,-35*unitY,0,0);
    EmitVertex();
    EndPrimitive();
    gl_Position = projectedV0+vec4(20*unitX,-20*unitY,0,0);
    EmitVertex();
    gl_Position = projectedV0+vec4(31*unitX,-31*unitY,0,0);
    EmitVertex();
    EndPrimitive();
    
}


