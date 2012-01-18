sampler2D g_samSrcColor;

float timeraslow=1.0;

float4 MyShader( float2 Tex : TEXCOORD0 ) : COLOR0
{
float4 Color;

Color = tex2D( g_samSrcColor, Tex.xy);

Color -= float4(min(0,Color.r-0.5), min(0,Color.g-0.5), min(0,Color.b-0.5), 0.9);

return Color;
}


technique PostProcess
{
    pass p1
    {
        PixelShader = compile ps_2_0 MyShader();
    }

}