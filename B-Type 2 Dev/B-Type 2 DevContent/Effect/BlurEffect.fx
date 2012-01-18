sampler2D tex : register(S0);

float PixelSize = 10;

// pixel shader function
float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 c = 0.2; // will get max of each value for c
    float alpha = 0; // alpha will be average

    float2 myuv = uv;
    for(int i = -7; i <= 7; i++)
    {
        myuv.x = uv.x + (i * PixelSize * 1.5);
        float4 sample = tex2D(tex, myuv);
        c = max(c, sample);
        alpha += sample.a;
    }

    //c.a = saturate(pow(abs(alpha / 6), 0.4));
    return(c);
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 main();
    }
}