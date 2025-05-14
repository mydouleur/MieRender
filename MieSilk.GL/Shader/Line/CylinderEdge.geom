#version 430 core
layout(triangles) in;
layout(line_strip , max_vertices = 4 ) out;
in float[] geomW;
out float fragIndex;
uniform mat4 uCameraMatrix;
uniform mat4 uView;
vec3 rotatePointWithQuaternion(vec3 point, vec3 axis, float angle);
void main()
{
       vec3 up = gl_in[0].gl_Position.xyz;
       vec3 down = gl_in[1].gl_Position.xyz;
       vec3 refer = gl_in[2].gl_Position.xyz;
       int edgeCount = int(geomW[0]);
       int end = edgeCount/2;
       float color = geomW[2];
           fragIndex = color;
       vec3 rotateAxis = up-down;
       float angle = 360/edgeCount;
       vec3 referVec = refer-up;
       vec3[2] maxPoints;
       maxPoints[0] = refer;
       maxPoints[1] = refer-2*referVec;
       vec2 max0 = (uView*vec4(maxPoints[0],1)).xy;
       vec2 max1 = (uView*vec4(maxPoints[1],1)).xy;
       float maxLength = length(max0-max1);
       for(int i = 1 ;i<end;i++)
       {
            vec3 tempVec = rotatePointWithQuaternion(referVec,rotateAxis,angle*i);
            vec3 p0 = up+tempVec;
            vec3 p1 = up-tempVec;
            vec2 max0 = (uView*vec4(p0,1)).xy;
            vec2 max1 = (uView*vec4(p1,1)).xy;
            float tempLength  =length(max0-max1);
            if(tempLength>maxLength)
            {
                  maxPoints[0] =p0;
                  maxPoints[1] = p1;
                  maxLength=tempLength;
            }
       }
       for (int i = 0; i <2; i++)
       {
           gl_Position = uCameraMatrix* vec4(maxPoints[i],1);
           EmitVertex();
           gl_Position = uCameraMatrix* vec4(maxPoints[i]-rotateAxis,1);
           EmitVertex();
           EndPrimitive();
       }
}
vec4 quatMultiply(vec4 q1, vec4 q2) {
    return vec4(
        q1.w * q2.x + q1.x * q2.w + q1.y * q2.z - q1.z * q2.y,
        q1.w * q2.y - q1.x * q2.z + q1.y * q2.w + q1.z * q2.x,
        q1.w * q2.z + q1.x * q2.y - q1.y * q2.x + q1.z * q2.w,
        q1.w * q2.w - q1.x * q2.x - q1.y * q2.y - q1.z * q2.z
    );
}
vec3 rotatePointWithQuaternion(vec3 point, vec3 axis, float angle) {
    axis = normalize(axis);
    angle = radians(angle) * 0.5; 

    float s = sin(angle);
    vec4 q = vec4(axis * s, cos(angle));

    vec4 p = vec4(point, 0.0);
    vec4 q_inv = vec4(-q.xyz, q.w); 
    vec4 rotated = quatMultiply(quatMultiply(q, p), q_inv);

    return rotated.xyz;
}