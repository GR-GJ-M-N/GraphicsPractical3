float4x4 World;
float4x4 View;
float4x4 Projection;
float3 Camera;
TextureCube CubeMap;

//Sampler for the Cube Map. Unfortunately there's somethign wrong with the mapping.
samplerCUBE ReflectionSampler = sampler_state {
	Texture = CubeMap;
	MipFilter = LINEAR;
    MinFilter = LINEAR;
    MagFilter = LINEAR;
	AddressU = clamp;
	AddressV = clamp;
	AddressW = clamp;
};

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float4 Normal : NORMAL0;
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float4 Position3D : TEXCOORD0;
	float4 Normal : TEXCOORD1;
	float3 TexCoords : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);
	//Get the unit vector from the point to the camera and the normal, and reflect the view vector on the normal to get the reflection vector
	float3 ViewDir = normalize(Camera - worldPosition);
	float3 Normal = normalize(mul(input.Normal, World));

	output.TexCoords = reflect(ViewDir, Normal);

    return output;
}

float4 CubeMapLookup(float3 Point)
{
	//Use texCUBE to get the correct color for the Point. Unfortunately there's seomthing wrong with the mapping
	return texCUBE(ReflectionSampler, Point);
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	return CubeMapLookup(input.TexCoords);
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
