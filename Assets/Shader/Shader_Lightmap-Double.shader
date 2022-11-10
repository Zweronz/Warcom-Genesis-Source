Shader "Devil/Shader_Lightmap-Double" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("MainTex", 2D) = "" {}
 _LightMap ("Lightmap (RGBA)", 2D) = "white" {}
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "RenderType"="Opaque" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord0
   Bind "texcoord1", TexCoord1
   Bind "texcoord1", TexCoord2
  }
  Color [_Color]
  Fog { Mode Off }
  SetTexture [_MainTex] { combine texture * primary }
  SetTexture [_LightMap] { combine texture * previous double }
  SetTexture [_LightMap] { combine texture * previous }
 }
}
}