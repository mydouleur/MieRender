#version 430 core
in vec3 fNormal;
in vec3 fPos;
in float fragIndex;
uniform vec3 cLook;
uniform vec3 cUp;
uniform vec4[255] ObjectColor;
out vec4 FragColor;
const vec3 lightColor1 = vec3(0.8,0.8,0.8);
const vec3 lightColor2 = vec3(1,1,1);
const vec3 lightColor3 = vec3(0.686,0.686,0.686);
const vec3 lightColor4 = vec3(0.5,0.5,0.5);
void main()
{
        vec3 cRight = normalize(cross(cLook,cUp));
        vec3 dir1 = (cLook+cUp);
        vec3 dir2 = (cRight-cUp+cLook);
        vec3 dir3 = (-cRight-cUp+cLook);
        float l1 = max(dot(fNormal,dir1),0);
        float l2 = max(dot(fNormal,dir2),0);
        float l3 = max(dot(fNormal,dir3),0);
        int i = int(fragIndex);
        FragColor = ObjectColor[i]*vec4(l1*lightColor4+l2*lightColor4+l3*lightColor4,1);
      
}