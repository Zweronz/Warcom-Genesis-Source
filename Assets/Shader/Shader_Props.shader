Shader "Devil/Shader_Props" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("MainTex", 2D) = "" {}
}
SubShader { 
 Tags { "RenderType"="Opaque" }
 Pass {
  Tags { "RenderType"="Opaque" }
  Color [_Color]
  Fog { Mode Off }
  SetTexture [_MainTex] { combine texture * primary double }
 }
}
}