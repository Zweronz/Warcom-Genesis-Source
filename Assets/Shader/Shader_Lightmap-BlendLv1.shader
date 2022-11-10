Shader "Devil/Shader_Lightmap-BlendLv1" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _BlendTex1 ("BlendMap (RGBA)", 2D) = "black" {}
 _MainTex1 ("BlendTexA (R)", 2D) = "black" {}
 _MainTex ("MainTexE", 2D) = "" {}
 _LightMap ("Lightmap (RGB)", 2D) = "white" {}
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "RenderType"="Opaque" }
  BindChannels {
   Bind "vertex", Vertex
   Bind "texcoord", TexCoord0
   Bind "texcoord", TexCoord1
   Bind "texcoord", TexCoord2
   Bind "texcoord1", TexCoord3
   Bind "texcoord1", TexCoord4
  }
  Color [_Color]
  Fog { Mode Off }
  SetTexture [_MainTex] { combine texture * primary }
  SetTexture [_BlendTex1] { combine previous, texture alpha }
  SetTexture [_MainTex1] { combine texture lerp(previous) previous }
  SetTexture [_LightMap] { combine texture * previous double }
 }
}
}