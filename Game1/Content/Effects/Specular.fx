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

float4x4 pView;
float4x4 pProjection;
float4x4 TextureTransform;

texture BasicTextureA;
texture BasicTextureB;
texture ProjectionTexture;

sampler BasicTextureSamplerA = sampler_state {
	texture = <BasicTextureA>;
};
sampler BasicTextureSamplerB = sampler_state {
	texture = <BasicTextureB>;
};
sampler ProjectionTextureSampler = sampler_state {
	texture = <ProjectionTexture>;
};
bool TextureEnabled = false;
bool ScreenMode = false;

bool ProjectionTextureEnabled = true;

struct VertexShaderInput
{
  float4 Position : POSITION0;
  float3 Normal : NORMAL0;
  float3 UV : TEXCOORD;
};
struct VertexShaderOutput
{
  float4 Position : POSITION0;
  float3 Normal : TEXCOORD1;
  float4 WorldPosition : TEXCOORD2;
  float3 UV : TEXCOORD0;
  float2 UVProj : TEXCOORD3;
};

bool SkyBoxEnabled = false;
Texture SkyBoxTexture;
samplerCUBE SkyBoxSampler = sampler_state
{
	texture = <SkyBoxTexture>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = Mirror;
	AddressV = Mirror;
};


VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	//float4 transformedUV = input.Position;
	float4 pWorldPosition = mul(input.Position, World);
	float4 pViewPosition = mul(pWorldPosition, pView);
	float4 pUV = mul(pViewPosition, pProjection);
	output.UVProj = mul(pUV, TextureTransform);
	output.UV = input.UV;
if (SkyBoxEnabled) {
	output.UV = worldPosition - CameraPosition;
}
	//output.TextureCoordinate = input.TextureCoordinate;

  output.WorldPosition = worldPosition;
  output.Normal = mul(input.Normal, World);

  return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
	if (TextureEnabled) {
		float4 texA = tex2D(BasicTextureSamplerA, input.UV).rgba;
		float4 texB = tex2D(BasicTextureSamplerB, input.UV).rgba;
		SurfaceColor = texA *(1- texB[3]) + texB * texB[3];
	}
	 
if (SkyBoxEnabled) {
	return texCUBE(SkyBoxSampler, normalize(input.UV));
}
  float3 totalLight = AmbientColor * AmbientIntensity;
  for (int i = 0; i < POINT_LIGHTS_NUM; i++)
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
	

float3 col = DiffuseColor[i];

if (ProjectionTextureEnabled && i == 0) col = tex2D(ProjectionTextureSampler, input.UVProj).rgba;
if(ScreenMode) col = saturate( tex2D(BasicTextureSamplerA, input.UV).rgba + col);

	  totalLight += diffuse * att * SurfaceColor * col * spot * DiffuseIntensity;

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