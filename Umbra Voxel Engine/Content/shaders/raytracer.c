uniform float time;
uniform vec2 resolution;
uniform vec3 cam_pos;
uniform vec3 cam_lookat;
uniform vec3 offset;

uniform sampler2DArray textures;
uniform sampler3D data;

struct Ray
{
	vec3 Origin;
	vec3 Direction;
};

int eMod(int dividend, int divisor)
{
	int mod = dividend % divisor;
	return mod + (mod < 0 ? divisor : 0);
}

ivec3 GetArrayIndex(vec3 index)
{
	ivec3 chunk;
	chunk.x = int(floor(index.x / 32.0));
	chunk.y = int(floor(index.y / 32.0));
	chunk.z = int(floor(index.z / 32.0));

	if(index.x < 0)
	{
		chunk.x -= 1;
	}
	if(index.y < 0)
	{
		chunk.y -= 1;
	}
	if(index.z < 0)
	{
		chunk.z -= 1;
	}

	
	chunk.x = eMod(chunk.x, 5);
	chunk.y = eMod(chunk.y, 5);
	chunk.z = eMod(chunk.z, 5);

	ivec3 block = ivec3(index);
	
	block.x = eMod(block.x, 32);
	block.y = eMod(block.y, 32);
	block.z = eMod(block.z, 32);

	return (chunk * 32 + block); // zyx -- axes are mirrored?
}

int GetVoxel(vec3 voxel)
{
	return int(floor(texelFetch(data, GetArrayIndex(voxel), 0).g * 255));
}

bool ShouldDraw(vec3 voxel)
{
	return GetVoxel(voxel) != 0;
}

void IterateVoxel(inout vec3 voxel, Ray ray, inout vec4 colorAccum)
{
	float maxX = 0.0;
	float maxY = 0.0;
	float maxZ = 0.0;
	
		
	if(ray.Direction.x != 0.0)
	{
		maxX = max((voxel.x - ray.Origin.x) / ray.Direction.x, (voxel.x + 1.0 - ray.Origin.x) / ray.Direction.x);
	}
	if(ray.Direction.y != 0.0)
	{
		maxY = max((voxel.y - ray.Origin.y) / ray.Direction.y, (voxel.y + 1.0 - ray.Origin.y) / ray.Direction.y);
	}
	if(ray.Direction.z != 0.0)
	{
		maxZ = max((voxel.z - ray.Origin.z) / ray.Direction.z, (voxel.z + 1.0 - ray.Origin.z) / ray.Direction.z);
	}

	vec2 hitPoint;
	if(maxX <= min(maxY, maxZ))
	{
		voxel.x += sign(ray.Direction.x);
		int block = GetVoxel(voxel);
		if(block != 0)
		{
			hitPoint = fract(ray.Origin + ray.Direction * maxX).zy;
			colorAccum = vec4(texture2DArray(textures, vec3(1.0 - abs(hitPoint), block)).xyz  * 0.9, 1.0);
		}
	}
	if(maxY <= min(maxX, maxZ))
	{
		voxel.y += sign(ray.Direction.y);
		int block = GetVoxel(voxel);
		if(block != 0)
		{
			hitPoint = fract(ray.Origin + ray.Direction * maxY).xz;
			colorAccum = vec4(texture2DArray(textures, vec3(1.0 - abs(hitPoint), block)).xyz  * 1.0, 1.0);
		}
	}
	if(maxZ <= min(maxX, maxY))
	{
		voxel.z += sign(ray.Direction.z);
		int block = GetVoxel(voxel);
		if(block != 0)
		{
			hitPoint = fract(ray.Origin + ray.Direction * maxZ).xy;
			colorAccum = vec4(texture2DArray(textures, vec3(1.0 - abs(hitPoint), block)).xyz  * 0.8, 1.0);
		}
	}
	colorAccum += vec4(0.39, 0.58, 0.93, 1.0) * 0.005;
}
	
vec4 GetRayColor(Ray ray)
{
	vec3 voxel = ray.Origin - fract(ray.Origin);
	vec4 colorAccum = vec4(0.0);
	while(colorAccum.a < 1.0)
	{
		IterateVoxel(voxel, ray, colorAccum);
	}
	
	return colorAccum;
}

void GetCameraRay(const in vec3 position, const in vec3 lookAt, out Ray currentRay)
{
	vec3 worldUp = vec3(0.0, 1.0, 0.0);

	vec3 forward = normalize(lookAt - position);
	vec3 right = normalize(cross(forward, worldUp));
	vec3 up = cross(right, forward);

	vec2 coordNormalized = (gl_FragCoord.xy / resolution.xy * 2.0 - 1.0);
	coordNormalized.x *= (resolution.x / resolution.y);
	coordNormalized /= sqrt(3.0);
	
	currentRay.Origin = position;
		   
	currentRay.Direction = normalize( right * coordNormalized.x + up * coordNormalized.y + forward);
}

void main( void )
{
	Ray currentRay;

	vec3 offset = vec3(0.3, 1.5, 0.3);
	GetCameraRay(cam_pos + offset, cam_pos + cam_lookat + offset, currentRay);

	gl_FragColor = GetRayColor(currentRay);
}