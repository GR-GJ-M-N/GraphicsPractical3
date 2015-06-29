float4x4 View, Projection, World;
float3x3 InvTransposed;
float4 DiffuseColor, AmbientColor, SpecularColor;
float3 Light, Camera;
float AmbientIntensity, SpecularIntensity, SpecularPower, NormalMapIntensity;

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
    return float4(0, 0, 0, 1) + float4(input.Normal.x, input.Normal.y, input.Normal.z, 1);
}

float4 WhiteShaderFunction(VertexShaderOutput input) : COLOR0
{
	return float4(1, 1, 1, 1);
}

technique Normal
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

technique White
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 WhiteShaderFunction();
    }
}



// E5 ----------------------------------------------------------------
texture E5textureObject;
sampler E5SceneSampler = sampler_state{ Texture = <E5textureObject>; };

float4 E5PixelShader(float2 TextureCoord : TEXCOORD0) : COLOR0
{
	 float4 color = tex2D(E5SceneSampler, TextureCoord);    
	 float bw = color.r * 0.3 + color.g * 0.59 + color.b * 0.11;
     return float4(bw, bw, bw, 1);
} 

technique E5PostProcessing
{
     pass P0
     {
          PixelShader = compile ps_2_0 E5PixelShader();
     }
} 

// E6 ----------------------------------------------------------------
texture E6textureObject;
sampler E6SceneSampler = sampler_state{ Texture = <E6textureObject>; };

float4 E6PixelShader(float2 TextureCoord : TEXCOORD0) : COLOR0
{
	 float4 color = float4(0.0, 0.0, 0.0, 0.0);
	 float4 weight = float4(1.0/9.0, 1.0/9.0, 1.0/9.0, 1.0/9.0);

	 color += tex2D(E6SceneSampler, TextureCoord + float2(-1, -1)) * weight;
	 color += tex2D(E6SceneSampler, TextureCoord + float2(0, -1)) * weight;
	 color += tex2D(E6SceneSampler, TextureCoord + float2(1, -1)) * weight;

	 color += tex2D(E6SceneSampler, TextureCoord + float2(-1, 0)) * weight;
	 color += tex2D(E6SceneSampler, TextureCoord + float2(0, 0)) * weight;
	 color += tex2D(E6SceneSampler, TextureCoord + float2(1, 0)) * weight;

	 color += tex2D(E6SceneSampler, TextureCoord + float2(-1, 1)) * weight;
	 color += tex2D(E6SceneSampler, TextureCoord + float2(0, 1)) * weight;
	 color += tex2D(E6SceneSampler, TextureCoord + float2(1, 1)) * weight;

	 return color;
} 

technique E6PostProcessing
{
     pass P0
     {
          PixelShader = compile ps_2_0 E6PixelShader();
     }
} 

// E1 ----------------------------------------------------------------
VertexShaderOutput BlinnPhongVertexShader(VertexShaderInput input)
{
	// Allocate an empty output struct
	VertexShaderOutput output = (VertexShaderOutput)0;

	// Do the matrix multiplications for perspective projection and the world transform
	float4 worldPosition = mul(input.Position, World);
    float4 viewPosition  = mul(worldPosition, View);
	output.Position2D    = mul(viewPosition, Projection);
	output.Place = worldPosition.xyz;
	output.Normal = input.Normal3D.xyz;	

	return output;
}

float4 BlinnPhongPixelShader(VertexShaderOutput input) : COLOR0
{
	float3x3 rotationAndScale = (float3x3) World;
	float3 normal = input.Normal;
	normal = mul(normal, InvTransposed);

	//Normalize the normal
	normal = normalize(normal);

	//Calculate L
	float3 lVector = normalize(Light - input.Place);

	//Calculate v (the vector to the camera)
	float3 vVector = normalize(Camera - input.Place);
	float3 hVector = (vVector + lVector) / length(vVector + lVector);

	//Calculate n dot l, clamp to 0, 1
	float intensity = saturate(dot(normal, lVector));
	float spec = SpecularColor * SpecularIntensity * pow(saturate(dot(normal, hVector)), SpecularPower);

	return AmbientIntensity * AmbientColor + intensity * DiffuseColor + spec;

}

technique BlinnPhong
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 BlinnPhongVertexShader();
		PixelShader  = compile ps_2_0 BlinnPhongPixelShader();
	}
}