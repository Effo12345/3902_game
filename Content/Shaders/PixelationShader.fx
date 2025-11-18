sampler2D TextureSampler : register(s0);
float2 PixelSize;

float4 PixelatePS(float2 texCoord : TEXCOORD0) : SV_Target
{
    float2 blockCoord = floor(texCoord / PixelSize) * PixelSize;

    float4 color = tex2D(TextureSampler, blockCoord);

    return color;
}

technique Pixelation
{
    pass P0
    {
        PixelShader = compile ps_3_0 PixelatePS();
    }
}