float4x4 World;
float4x4 View;
float4x4 Projection;

float4 AmbientColor = float4(1, 1, 1, 1);
float AmbientIntensity = 0.1;

float4x4 WorldInverseTranspose;

float3 LightPosition;

float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 1.0;

float Shininess = 200;
float4 SpecularColor = float4(1, 1, 1, 1);
float SpecularIntensity = 1;
float3 ViewVector = float3(0, 1, 0);

struct VertexShaderInput
{
	float4 Position : SV_POSITION;
	float4 Normal : NORMAL0;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float3 Normal : TEXCOORD0;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float4 ambient = AmbientColor * AmbientIntensity;

	float3 l = -normalize(worldPosition - LightPosition);
	float3 n = normalize(mul(input.Normal, WorldInverseTranspose));

	float4 diffuse = dot(n.xyz, l) * DiffuseIntensity * DiffuseColor;

	float3 r = normalize(2 * dot(l, n) * n - l);
	float3 v = normalize(ViewVector);

	float4 specular = SpecularIntensity * SpecularColor * max(pow(abs(dot(r, v)), Shininess), 0);

	output.Color = saturate(ambient + diffuse + specular);//saturate(output.Color + AmbientColor * AmbientIntensity + specular);
	output.Normal = n.xyz;

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_TARGET0
{
	return input.Color;
}

technique Specular
{
	pass Pass1
	{
		VertexShader = compile vs_4_0_level_9_1 VertexShaderFunction();
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}