#define POINT_LIGHTS_NUM 4

float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 WorldInverseTranspose;

float3 AmbientColor = float3(.15, .15, .15);
float AmbientIntensity = 0.1;

float3 DiffuseColor[POINT_LIGHTS_NUM];
float DiffuseIntensity = 1.0;

float SpecularPower = 32;
float3 SpecularColor[POINT_LIGHTS_NUM];
float SpecularIntensity = 1.0;

float3 SurfaceColor = float3(1, 1, 1);

float3 CameraPosition = float3(0, 0, 0);

float3 LightPosition[POINT_LIGHTS_NUM];
float3 LightDirection[POINT_LIGHTS_NUM];
float LightPhi[POINT_LIGHTS_NUM];
float LightTheta[POINT_LIGHTS_NUM];
float LightAttenuation = 50;
float LightFalloff = 2;

bool FogEnabled = true;
float FogStart = 0;
float FogEnd = 100;
float3 FogColor = float3(1, 1, 1);

bool TextureEnabled = false;

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

float4x4 pView;
float4x4 pProjection;
float4x4 TextureTransform;

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
  VertexShaderOutput output;
  
  float4 worldPosition = mul(input.Position, World);
  float4 viewPosition = mul(worldPosition, View);
  output.Position = mul(viewPosition, Projection);

  output.WorldPosition = worldPosition;
  output.Normal = mul(input.Normal, World);

  return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
  float3 totalLight = AmbientColor * AmbientIntensity;
  for (int i = 1; i < 2; i++)
  {
	  float d = distance(LightPosition[i], input.WorldPosition);
	  float att = 1 - pow(clamp(d / LightAttenuation, 0, 1), LightFalloff);

	  float3 lightDir = normalize(LightPosition[i] - input.WorldPosition);
	  float3 normal = normalize(input.Normal);
	  float3 viewDir = normalize(CameraPosition - input.WorldPosition);
	  float3 refl = -reflect(lightDir, normal);


	  float spot = dot(normal, refl);
	  float angle = acos(dot(lightDir, normalize(LightDirection[i])));
	  if (angle > LightPhi[i]) spot = 0.0f;
	  else if (angle < LightTheta[i]) spot = 1.0f;
	  else spot = smoothstep(LightPhi[i], LightTheta[i], angle);


	  float diffuse = saturate(dot(normalize(input.Normal), lightDir));

	  totalLight += diffuse * att * SurfaceColor * DiffuseColor[i] * spot * DiffuseIntensity;

	  float specular = pow(saturate(dot(refl, viewDir)), SpecularPower);

	  totalLight += specular * att * SpecularColor[i] * spot * SpecularIntensity;
  }

  float fog = 0;
  if (FogEnabled)
  {
	  float dist = length(CameraPosition - input.WorldPosition);
	  fog = clamp((dist - FogStart) / (FogEnd - FogStart), 0, 1);
	  totalLight = lerp(totalLight, FogColor, fog);
  }
  totalLight = float4(totalLight, 1);


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