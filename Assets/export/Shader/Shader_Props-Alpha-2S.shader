Shader "Devil/Shader_Props-Alpha-2S" {
Properties {
 _Color ("Main Color", Color) = (1,1,1,1)
 _MainTex ("MainTex", 2D) = "" {}
}
SubShader { 
 Tags { "QUEUE"="Transparent+1" }
 Pass {
  Tags { "QUEUE"="Transparent+1" }
  Color [_Color]
  Cull Off
  Fog { Mode Off }
  Blend SrcAlpha OneMinusSrcAlpha
  Offset 1, 1
  SetTexture [_MainTex] { combine texture * primary double, texture alpha * primary alpha }
 }
}
}