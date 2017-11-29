#version 110

varying vec4 texcoord;
//varying vec4 position; //position is equal to texture coordinate :/ (one image plane)

void main()
{
    texcoord = gl_MultiTexCoord0;
    
    gl_Position = gl_ProjectionMatrix * gl_Vertex;
    
}