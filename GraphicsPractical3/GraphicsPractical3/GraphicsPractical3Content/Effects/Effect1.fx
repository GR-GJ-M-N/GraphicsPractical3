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

// E2 ----------------------------------------------------------------
float3 AmbientLightColor = float3(.15, .15, .15);
float3 DiffuseColor2 = float3(.85, .85, .85);
//float3 LightPosition = float3(0, 5000, 0);
float3 LightPosition = float3(0, 500, 0);
float3 LightDirection = float3(0, -1, 0);
//float ConeAngle = 90;
float ConeAngle = 20;
float3 LightColor = float3(1, 1, 1);
float LightFalloff = 20;

texture E2textureObject;
sampler E2SceneSampler = sampler_state{ Texture = <E2textureObject>; };

//pixel shader doet de lighting calculations
float4 E2PixelShader(VertexShaderOutput input) : COLOR0
{
  float3 diffuseColor2 = DiffuseColor2;


  //diffuseColor2 *= tex2D(E2SceneSampler, input.UV).rgb;	//Moet misschien niet

  float3 totalLight = float3(0, 0, 0);
  totalLight += AmbientLightColor;
  float3 lightDir = normalize(LightPosition - input.WorldPosition);
  float diffuse = saturate(dot(normalize(input.Normal), lightDir));

  // (dot(p - lp, ld) / cos(a))^f
  float d = dot(-lightDir, normalize(LightDirection));
  float a = cos(ConeAngle);

  float att = 0;

  if (a < d)
    att = 1 - pow(clamp(a / d, 0, 1), LightFalloff);

  totalLight += diffuse * att * LightColor;

  return float4(diffuseColor2 * totalLight, 1);
}

technique E2Spotlight
{
     pass P0
     {
		  VertexShader = compile vs_2_0 VertexShaderFunction();
          PixelShader = compile ps_2_0 E2PixelShader();
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
float2 offsets[7];
float weights[7];

float4 E6PixelShader(float2 TextureCoord : TEXCOORD0) : COLOR0
{
	 float4 color = float4(0.0, 0.0, 0.0, 0.0);
	 float weight = 1.0/9.0;
	 //float weight = 1.0/3.0;
	 //float weight = 1.0;

	 /*color += tex2D(E6SceneSampler, TextureCoord + float2(-1, -1)) * weight;
	 color += tex2D(E6SceneSampler, TextureCoord + float2(0, -1)) * weight;
	 color += tex2D(E6SceneSampler, TextureCoord + float2(1, -1)) * weight;*/

	 /*color += tex2D(E6SceneSampler, TextureCoord + float2(-1, 0)) * weight;
	 color += tex2D(E6SceneSampler, TextureCoord + float2(0, 0)) * weight;
	 color += tex2D(E6SceneSampler, TextureCoord + float2(1, 0)) * weight;*/

	 /*color += tex2D(E6SceneSampler, TextureCoord + float2(-1, 1)) * weight;
	 color += tex2D(E6SceneSampler, TextureCoord + float2(0, 1)) * weight;
	 color += tex2D(E6SceneSampler, TextureCoord + float2(1, 1)) * weight;*/

	 //color = tex2D(E6SceneSampler, TextureCoord + float2(0.5f, 0));
	 //color = tex2D(E6SceneSampler, float4(1, 1, 1, 1));
	 //color = tex2D(E6SceneSampler, float4(TextureCoord.x, TextureCoord.x, TextureCoord.x, 1));
	 //color = tex2D(E6SceneSampler, TextureCoord);

	 //^ OFFSET WERKT NIET GOED? ^

	 for (int i = 0; i < 7; ++i)
		color += tex2D(E6SceneSampler, TextureCoord + offsets[i]) * weights[i]; 
		//color += tex2D(E6SceneSampler, TextureCoord + offsets[i]) * weight; //Werkt ongeveer

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

//---------------------------------------- Technique: 3 Texture ----------------------------------------
texture QuadTexture;

sampler2D TexturetextureSampler = sampler_state{
	Texture = <QuadTexture>;
	MagFilter = Linear;
	MinFilter = Linear;
	AddressU = Wrap;
	AddressV = Wrap;
};

struct TextureVertexShaderInput
{
	float4 Position3D : POSITION0;
	float4 Normal3D : NORMAL0;
	float4 Color : COLOR0;
	float2 TextureCoord : TEXCOORD0;
	float4 Place : TEXCOORD1;
};

VertexShaderOutput TextureVertexShader(TextureVertexShaderInput input)
{
	// Allocate an empty output struct
	VertexShaderOutput output = (VertexShaderOutput)0;

	// Do the matrix multiplications for perspective projection and the world transform
	float4 worldPosition = mul(input.Position3D, World);
    float4 viewPosition  = mul(worldPosition, View);
	output.Position2D    = mul(viewPosition, Projection);

	output.Normal = input.Normal3D.xyz;
	output.TextureCoord = input.TextureCoord;

	return output;
}

float4 TexturePixelShader(VertexShaderOutput input) : COLOR0
{
	float4 textureColor = tex2D(TexturetextureSampler, input.TextureCoord);

	return textureColor;
}

technique Texture
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 TextureVertexShader();
		PixelShader  = compile ps_2_0 TexturePixelShader();
	}
}