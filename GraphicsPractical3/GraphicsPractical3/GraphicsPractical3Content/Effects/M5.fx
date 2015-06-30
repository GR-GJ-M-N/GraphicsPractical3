float4x4 World;
float4x4 View;
float4x4 Projection;
float3 Camera;
TextureCube CubeMap;

samplerCUBE ReflectionSampler = sampler_state {
	Texture = CubeMap;
	MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
	AddressU = clamp;
	AddressV = clamp;
	AddressW = clamp;
};

// TODO: add effect parameters here.

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Normal : NORMAL0;

    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Position3D : TEXCOORD0;
	float4 Normal : TEXCOORD1;
	float3 TexCoords : TEXCOORD2;

    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	float3 ViewDir = normalize(Camera - worldPosition);
	float3 Normal = normalize(mul(input.Normal, World));

	output.TexCoords = reflect(ViewDir, Normal);
	//output.TexCoords = worldPosition;
	//output.TexCoords = input.Normal;
	//output.TexCoords = Normal;

	//output.TexCoords = reflect(CtoVertex, Normal);

    // TODO: add your vertex shader code here.

    return output;
}

float4 CubeMapLookup(float3 Point)
{
	//return float4((float3)texCUBE(ReflectionSampler, Point), 0.5f) + float4(Point, 0.5);
	return texCUBE(ReflectionSampler, Point);
	//return float4(0, 0.145f, 0.984f, 1);
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
	//return float4(input.TexCoords, 1);
	return CubeMapLookup(float3(0, 0.145f, 0.984f));
	//return CubeMapLookup(normalize(input.TexCoords));
	//return texCUBE(ReflectionSampler, normalize(input.TexCoords));
    //return float4(input.Normal.x, input.Normal.y, input.Normal.z, 1);
}

technique Technique1
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}
