float4x4 World;
float4x4 View;
float4x4 Projection;

// TODO: add effect parameters here.

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Normal3D : NORMAL0;
	float4 Color : COLOR0;
    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position2D : POSITION0;
	float4 Color : COLOR0;
	float3 Normal : TEXCOORD0;
	float3 WorldPosition : TEXCOORD1;
	float2 TextureCoord : TEXCOORD2;
	float3 Place : TEXCOORD3;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position2D = mul(viewPosition, Projection);
	output.Normal = input.Normal3D;

    // TODO: add your vertex shader code here.

    return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	//return float4(1, 1, 1, 1);
    return float4(input.Normal.x, input.Normal.y, input.Normal.z, 1);
}

technique Normal
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
