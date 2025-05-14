#version 430 core

out vec4 FragColor;
in vec3 fFaceNormal;
uniform vec3 cLook;
uniform vec3 cUp;
const vec3 lightColor = vec3(0.5,0.5,0.5);
void main()
{
        vec3 cRight = normalize(cross(cLook,cUp));
        vec3 normal = normalize(fFaceNormal);
        vec3 dir1 = (cLook+cUp);
        vec3 dir2 = (cRight-cUp+cLook);
        vec3 dir3 = (-cRight-cUp+cLook);
        float l1 = max(dot(normal,dir1),0);
        float l2 = max(dot(normal,dir2),0);
        float l3 = max(dot(normal,dir3),0);
      //  int i = int(fragIndex);
        FragColor = vec4(0.5,0.5,0.5,1)*vec4(l1*lightColor+l2*lightColor+l3*lightColor,1);
        //FragColor = vec4(0.75,0.75,0.75,1);
}