uniform float time;
uniform vec2 resolution;
uniform vec3 cam_pos;
uniform vec3 cam_lookat;
uniform vec3 offset;
uniform int world_size;
uniform int chunk_size;

uniform sampler2DArray textures;
uniform sampler3D data;

float[] textureFaces = {
	0,	0,	0,	0,	0,	0,
	1,	1,	0,	3,	1,	1,
	2,	2,	2,	2,	2,	2,
	3,	3,	3,	3,	3,	3,
	4,	4,	4,	4,	4,	4,
	5,	5,	5,	5,	5,	5,
	6,	6,	6,	6,	6,	6,
	7,	7,	7,	7,	7,	7,
	8,	8,	9,	9,	8,	8
};

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

bool GetArrayIndex(vec3 index, out ivec3 arrayIndex)
{
	
	if(
		(index.x < -(world_size / 2) * chunk_size) ||
		(index.x > (world_size / 2) * chunk_size + 31) ||
		(index.y < -(world_size / 2) * chunk_size) ||
		(index.y > (world_size / 2) * chunk_size + 31) ||
		(index.z < -(world_size / 2) * chunk_size) || 
		(index.z > (world_size / 2) * chunk_size + 31)
		)
	{
		return false; // Check if block is invalid, i.e. outside the world
	}


	//index -= offset;
	ivec3 chunk;
	chunk.x = int(floor(index.x / chunk_size));
	chunk.y = int(floor(index.y / chunk_size));
	chunk.z = int(floor(index.z / chunk_size));

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

	chunk.x = eMod(chunk.x, world_size);
	chunk.y = eMod(chunk.y, world_size);
	chunk.z = eMod(chunk.z, world_size);

	ivec3 block = ivec3(index);
	
	block.x = eMod(block.x, chunk_size);
	block.y = eMod(block.y, chunk_size);
	block.z = eMod(block.z, chunk_size);

	arrayIndex = (chunk * chunk_size + block);
	return true;
}

int GetVoxel(vec3 voxel)
{
	ivec3 arrayIndex;
	if(GetArrayIndex(voxel, arrayIndex))
	{
		return int(floor(texelFetch(data, arrayIndex, 0).g * 255));
	}

	return 255; // Return vacuum if block is invalid
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
	float texture;
	if(maxX <= min(maxY, maxZ))
	{
		voxel.x += sign(ray.Direction.x);
		int block = GetVoxel(voxel);

		if(block != 0)
		{
			texture = (ray.Direction.x > 0) ? textureFaces[block*6 + 0] : textureFaces[block*6 + 1];
			hitPoint = fract(ray.Origin + ray.Direction * maxX).zy;
			colorAccum = texture2DArray(textures, vec3(1.0 - abs(hitPoint), texture));
			colorAccum.xyz *= 0.9;
		}
	}
	if(maxY <= min(maxX, maxZ))
	{
		voxel.y += sign(ray.Direction.y);
		int block = GetVoxel(voxel);

		if(block != 0)
		{
			texture = (ray.Direction.y > 0) ? textureFaces[block*6 + 3] : textureFaces[block*6 + 2];
			hitPoint = fract(ray.Origin + ray.Direction * maxY).xz;
			colorAccum = texture2DArray(textures, vec3(1.0 - abs(hitPoint), texture));
			colorAccum.xyz *= 1.0;
		}
	}
	if(maxZ <= min(maxX, maxY))
	{
		voxel.z += sign(ray.Direction.z);
		int block = GetVoxel(voxel);

		if(block != 0)
		{
			texture = (ray.Direction.z > 0) ? textureFaces[block*6 + 4] : textureFaces[block*6 + 5];
			hitPoint = fract(ray.Origin + ray.Direction * maxZ).xy;
			colorAccum = texture2DArray(textures, vec3(1.0 - abs(hitPoint), texture));
			colorAccum.xyz *= 0.8;
		}
	}
}
	
vec4 GetRayColor(Ray ray)
{
	vec3 voxel = ray.Origin - fract(ray.Origin);
	vec4 colorAccum = vec4(0.0);
	vec3 voxelOffset;

	int safety = 0;

	while(safety < 200)
	{
		voxelOffset = floor(voxel) + vec3(0.5) - offset;

		/*if(
			(voxelOffset.x < -(world_size / 2) * chunk_size) ||
			(voxelOffset.x > (world_size / 2) * chunk_size) ||
			(voxelOffset.y < -(world_size / 2) * chunk_size) ||
			(voxelOffset.y > (world_size / 2) * chunk_size) ||
			(voxelOffset.z < -(world_size / 2) * chunk_size) || 
			(voxelOffset.z > (world_size / 2) * chunk_size)
			)
		{
			break;
		}*/

		IterateVoxel(voxel, ray, colorAccum);

		if(colorAccum.a >= 1.0)
		{
			return colorAccum;
		}
		safety++;
	}
	
	return vec4(0.39, 0.58, 0.93, 1.0);
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

	vec3 adjustment = vec3(0.3, 1.5, 0.3);
	GetCameraRay(cam_pos + adjustment, cam_pos + cam_lookat + adjustment, currentRay);

	gl_FragColor = GetRayColor(currentRay);
}