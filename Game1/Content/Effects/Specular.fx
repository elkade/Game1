float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;

float3 AmbientColor = float3(.15, .15, .15);
float AmbientIntensity = 0.1;

float3 DiffuseColor = float3(.85, .85, .85);
float DiffuseIntensity = 1.0;

float SpecularPower = 32;
float3 SpecularColor = float3(1, 1, 1);
float SpecularIntensity = 1.0;

float3 SurfaceColor = float3(1, 1, 1);

float3 CameraPosition = float3(0, 0, 0);

float3 LightPosition = float3(0, 0, 0);
float LightAttenuation = 100;
float LightFalloff = 2;

struct VertexShaderInput
{
  float4 Position : POSITION0;
  float3 Normal : NORMAL0;
};
struct VertexShaderOutput
{
  float4 Position : POSITION0;
  float3 Normal : TEXCOORD1;
  float4 WorldPosition : TEXCOORD2;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
  VertexShaderOutput output;
  
  float4 worldPosition = mul(input.Position, World);
  float4 viewPosition = mul(worldPosition, View);
  output.Position = mul(viewPosition, Projection);

  output.WorldPosition = worldPosition;
  output.Normal = mul(input.Normal, WorldInverseTranspose);

  return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
  float3 totalLight = float3(0, 0, 0);
  
  float d = distance(LightPosition, input.WorldPosition);
  float att = 1 - pow(clamp(d / LightAttenuation, 0, 1), LightFalloff);

  totalLight += AmbientColor * AmbientIntensity;
 
  float3 lightDir = normalize(LightPosition - input.WorldPosition);
  float diffuse = saturate(dot(normalize(input.Normal), lightDir)) * DiffuseIntensity * DiffuseColor;

  totalLight += diffuse * att * SurfaceColor;

  float3 normal = normalize(input.Normal);
  float3 viewDir = normalize(CameraPosition - input.WorldPosition);
  float3 refl = -reflect(lightDir, normal);
  float specular = pow(saturate(dot(refl, viewDir)), SpecularPower) * SpecularColor * SpecularIntensity;

  totalLight += specular * att;

  return float4( saturate(totalLight), 1);
}

technique Specular
{
	pass Pass1
	{
		VertexShader = compile vs_4_0 VertexShaderFunction();
		PixelShader = compile ps_4_0 PixelShaderFunction();
	}
}