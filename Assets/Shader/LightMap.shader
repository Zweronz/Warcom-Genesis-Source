Shader "iPhone/LightMap" {
Properties {
 _Color ("Main Color", Color) = (0.8,0.8,0.8,1)
 _texBase ("MainTex", 2D) = "" {}
 _texLightmap ("LightMap", 2D) = "" {}
}
SubShader { 
 Pass {
  Tags { "RenderType"="Geometry" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord0
   Bind "texcoord1", TexCoord1
  }
  Color [_Color]
  SetTexture [_texBase] { combine texture }
  SetTexture [_texLightmap] { combine previous * texture }
 }
}
}