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
float LightAngle = 3.14/4;
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
	float3 Light : TEXCOORD1;
	float3 Camera : TEXCOORD2;
	float4 LightDistance : TEXCOORD3;
};

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
	VertexShaderOutput output;

	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

	float4 normal = normalize(mul(input.Normal, World));
	float3 lightDirection = normalize(LightPosition - worldPosition);
	float lightIntensity = dot(normal.xyz, lightDirection);
	output.Color = saturate(DiffuseColor * DiffuseIntensity * lightIntensity);

	output.Normal = normal.xyz;

	output.Light = lightDirection;

	output.Camera = normalize(worldPosition - ViewVector);

	output.LightDistance = length(LightPosition - worldPosition);

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : SV_TARGET0
{

	float3 lightDirection = float3(0.0f, 1.0f, 0.0f);
	float theta = 10.0f;
	float phi = 30.0f;

	float f = dot(input.Normal,input.Light);
	float angle = acos(dot(input.Light,normalize(lightDirection)));
	if (angle > radians(phi)) f = 0.0f;
	else if (angle < radians(theta)) f = 1.0f;
	else f = smoothstep(radians(phi), radians(theta), angle);






	float3 normal = normalize(input.Normal);
	float3 r = normalize(2 * dot(input.Light, normal) * normal - input.Light);
	float3 v = input.Camera;

	float dotProduct = dot(r, v);
	float4 specular = SpecularIntensity * SpecularColor * max(pow(abs(dotProduct), Shininess), 0) * length(input.Color);

	return saturate(AmbientColor * AmbientIntensity + (input.Color + specular) /** f*/ * min(1 , 1/(input.LightDistance*0.025f)));
}

technique Specular
{
	pass Pass1
	{
		VertexShader = compile vs_4_0_level_9_1 VertexShaderFunction();
		PixelShader = compile ps_4_0_level_9_1 PixelShaderFunction();
	}
}