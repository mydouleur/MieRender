#version 430 core
layout(triangles) in;
layout(line_strip, max_vertices = 34) out;
uniform mat4 uCameraMatrix;
in float[] params;
void emitTotal(vec4[6] up,vec4[6] down,int count);
void emitTotal(vec4[16] up,vec4[16] down,int count);
vec3 rotatePointWithQuaternion(vec3 point, vec3 axis, float angle);
void main()
{
    vec3 p0 = gl_in[0].gl_Position.xyz;
    vec3 p1 = gl_in[1].gl_Position.xyz;
    vec3 p2 = gl_in[2].gl_Position.xyz;
    float colorIndex = mod(params[0],3);
    float holeX = params[1];
    float holeY = params[2];
    vec3 rotateAxis = p1-p0;
    vec3 startVec = p2-p0;
    if(params[0]<10)
    {
        vec4[6] up;
        vec4[6] down;
        for(int i = 0;i<6;i++)
        {
           vec3 tempVec = rotatePointWithQuaternion(startVec,rotateAxis,i*60);
           up[i] = uCameraMatrix*vec4((p0+tempVec),1);
           down[i] = uCameraMatrix*vec4((p1+tempVec),1);
        }
        emitTotal(up,down,6);
    }
    else if(params[0]<20)
    {
        vec4[6] up;
        vec4[6] down;
        for(int i = 0;i<6;i++)
        {
           vec3 tempVec = rotatePointWithQuaternion(startVec,rotateAxis,i*60);
           up[i] = uCameraMatrix*vec4((p0+tempVec*0.5),1);
           down[i] = uCameraMatrix*vec4((p1+tempVec),1);
        }
        emitTotal(up,down,6);
    }
    else if(holeX==0&&holeY==0)
    {
        vec4[16] up;
        vec4[16] down;
        for(int i = 0;i<12;i++)
        {
           vec3 tempVec = rotatePointWithQuaternion(startVec,rotateAxis,i*30);
           up[i] = uCameraMatrix*vec4((p0+tempVec),1);
           down[i] = uCameraMatrix*vec4((p1+tempVec),1);
        }
        emitTotal(up,down,12);
    }
    else
    {
        vec4[16] up;
        vec4[16] down;
        vec3 xAxis = normalize(startVec);
        vec3 yAxis = normalize( cross(rotateAxis,startVec));
        vec3 xOffset = xAxis*holeX;
        vec3 yOffset = yAxis*holeY;
        for(int i =0;i<4;i++)
        {
            vec3 tempVec = rotatePointWithQuaternion(startVec,rotateAxis,i*30);
            up[i] = uCameraMatrix*vec4((p0+tempVec+xOffset+yOffset),1);
            down[i] = uCameraMatrix*vec4((p1+tempVec+xOffset+yOffset),1);
        }
        for(int i =4;i<8;i++)
        {
            vec3 tempVec = rotatePointWithQuaternion(startVec,rotateAxis,(i-1)*30);
            up[i] = uCameraMatrix*vec4((p0+tempVec-xOffset+yOffset),1);
            down[i] = uCameraMatrix*vec4((p1+tempVec-xOffset+yOffset),1);
        }
        for(int i =8;i<12;i++)
        {
            vec3 tempVec = rotatePointWithQuaternion(startVec,rotateAxis,(i-2)*30);
            up[i] = uCameraMatrix*vec4((p0+tempVec-xOffset-yOffset),1);
            down[i] = uCameraMatrix*vec4((p1+tempVec-xOffset-yOffset),1);
        }
        for(int i =12;i<16;i++)
        {
            vec3 tempVec = rotatePointWithQuaternion(startVec,rotateAxis,(i-3)*30);
            up[i] = uCameraMatrix*vec4((p0+tempVec+xOffset-yOffset),1);
            down[i] = uCameraMatrix*vec4((p1+tempVec+xOffset-yOffset),1);
        }
        emitTotal(up,down,16);
    }
}
void emitTotal(vec4[6] up,vec4[6] down,int count)
{
    for(int i = 0;i<count;i++)
    {
        gl_Position=up[i];
        EmitVertex();
    }
    gl_Position=up[0];
    EmitVertex();
    EndPrimitive();
    for(int i = 0;i<count;i++)
    {
        gl_Position=down[i];
        EmitVertex();
    }
    gl_Position=down[0];
    EmitVertex();
    EndPrimitive();
     for(int i = 0;i<count;i++)
    {
        gl_Position=up[i];
        EmitVertex();
        gl_Position=down[i];
        EmitVertex();
        EndPrimitive();
    }
}
void emitTotal(vec4[16] up,vec4[16] down,int count)
{
     for(int i = 0;i<count;i++)
    {
        gl_Position=up[i];
        EmitVertex();
    }
    gl_Position=up[0];
    EmitVertex();
    EndPrimitive();
    for(int i = 0;i<count;i++)
    {
        gl_Position=down[i];
        EmitVertex();
    }
    gl_Position=down[0];
    EmitVertex();
    EndPrimitive();
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