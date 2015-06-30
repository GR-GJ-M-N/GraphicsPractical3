float4x4 View, Projection, World;
float3x3 InvTransposed;
float4 DiffuseColor, AmbientColor, SpecularColor;
float3 Light, Camera;
float AmbientIntensity, SpecularIntensity, SpecularPower, NormalMapIntensity;
int ShadesCount;

struct VertexShaderInput
{
    float4 Position3D : POSITION0;
	float4 Normal3D : NORMAL0;
	float4 Color : COLOR0;
};

struct VertexShaderOutput
{
    float4 Position2D : POSITION0;
	float4 Color : COLOR0;
	float3 Normal : TEXCOORD0;
	float3 WorldPosition : TEXCOORD1;
	float2 TextureCoord : TEXCOORD2;
	float3 Position3D : TEXCOORD3;
};

VertexShaderOutput CellVertexShader (VertexShaderInput input)
{
	// Allocate an empty output struct
	VertexShaderOutput output = (VertexShaderOutput)0;

	// Do the matrix multiplications for perspective projection and the world transform
	float4 worldPosition = mul(input.Position3D, World);
    float4 viewPosition  = mul(worldPosition, View);
	output.Position2D    = mul(viewPosition, Projection);
	// Send the Normal and world position to the PS
	output.Normal = input.Normal3D.xyz;	
	output.Position3D = worldPosition.xyz;

	return output;
}

float4 CellPixelShader(VertexShaderOutput input) : COLOR0
{
	float3x3 rotationAndScale = (float3x3) World;
	float3 normal = input.Normal;

	//Normalize the normal
	normal = normalize(normal);

	//Calculate L
	float3 lVector = normalize(Light - input.Position3D);

	//Calculate v (the vector to the camera)
	float3 vVector = normalize(Camera - input.Position3D);
	float3 hVector = (vVector + lVector) / length(vVector + lVector);

	//Calculate n dot l, clamp to 0, 1
	float intensity = saturate(dot(normal, lVector));
	float specIntensity = SpecularIntensity * pow(saturate(dot(normal, hVector)), SpecularPower);

	//Use ceiling to create the distinct specular colors.
	specIntensity = ceil(specIntensity * ShadesCount)/ShadesCount;

	//Same, but for the diffuse color
	intensity = ceil(intensity * ShadesCount)/ShadesCount;

	//Add the respective intensities and colors
	return float4((float3)(intensity * DiffuseColor + specIntensity * SpecularColor), 1);
}

technique CellShading
{
    pass Pass1
    {
        VertexShader = compile vs_2_0 CellVertexShader();
        PixelShader = compile ps_2_0 CellPixelShader();
    }
}
