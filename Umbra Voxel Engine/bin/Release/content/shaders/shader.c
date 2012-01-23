uniform float time;
uniform vec2 resolution;
uniform vec3 cam_pos;
uniform vec3 cam_lookat;

uniform sampler2DArray textures;

const float[] block_Table = float[54](
//	 X+		 X-		 Y+		 Y-		 Z+		 Z-
	11.0,	11.0,	11.0,	11.0,	11.0,	11.0,	// Air
	2.0,	2.0,	1.0,	4.0,	2.0,	2.0,	// Grass
	3.0,	3.0,	3.0,	3.0,	3.0,	3.0,	// Stone
	4.0,	4.0,	4.0,	4.0,	4.0,	4.0,	// Dirt
	5.0,	5.0,	5.0,	5.0,	5.0,	5.0,	// Water
	6.0,	6.0,	6.0,	6.0,	6.0,	6.0,	// Sand
	7.0,	7.0,	7.0,	7.0,	7.0,	7.0,	// Leaves
	8.0,	8.0,	8.0,	8.0,	8.0,	8.0,	// Lava
	9.0,	9.0,	10.0,	10.0,	9.0,	9.0		// Log
	);

struct Ray
{
	vec3 Origin;
	vec3 Direction;
};

	
float GetType(vec3 voxel)
{
	if(voxel.y + 3 < sin(length(voxel.xz)) * length(voxel.xz) / 10.0)
	{
		return 2.0;
	}
	if(voxel.y + 1 < sin(length(voxel.xz)) * length(voxel.xz) / 10.0)
	{
		return 3.0;
	}
	else if(voxel.y < sin(length(voxel.xz)) * length(voxel.xz) / 10.0)
	{
		return 1.0;
	}
	
	return 0.0;
}

vec4 GetColorFromBlock(float face, float type, vec2 textCoords)
{
	return texture2DArray(textures, vec3(textCoords, block_Table[int(face + type * 6.0)]));
}

void IterateVoxel(inout vec3 voxel, Ray ray, inout vec4 currentColor)
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

	vec4 blockColor = vec4(0.0);
	float length = 0.0;
	
	if(maxX <= min(maxY, maxZ))
	{
		voxel.x += sign(ray.Direction.x);
		blockColor = GetColorFromBlock(0.0 + sign(ray.Direction.x) * 0.5 + 0.5, GetType(voxel), 1.0 - fract(ray.Origin + ray.Direction * maxX).zy) * 0.9;
		length = maxX;
	}
	else if(maxY <= min(maxX, maxZ))
	{
		voxel.y += sign(ray.Direction.y);
		blockColor = GetColorFromBlock(2.0 + sign(ray.Direction.y) * 0.5 + 0.5, GetType(voxel), 1.0 - fract(ray.Origin + ray.Direction * maxY).xz) * 1.0;
		length = maxY;
	}
	else if(maxZ <= min(maxX, maxY))
	{
		voxel.z += sign(ray.Direction.z);
		blockColor = GetColorFromBlock(4.0 + sign(ray.Direction.z) * 0.5 + 0.5, GetType(voxel), 1.0 - fract(ray.Origin + ray.Direction * maxZ).xy) * 0.8;
		length = maxZ;
	}
	
	currentColor = blockColor;
}
	

vec4 GetRayColor(Ray ray)
{
	vec3 voxel = ray.Origin - fract(ray.Origin);
	vec4 blockColor = vec4(0.0);

	for(int i = 0; i < 208; /*CAREFUL WITH THIS!!!*/ i++)
	{	
		IterateVoxel(voxel, ray, blockColor);

		if(blockColor.w != 0.0)
		{
			return blockColor;
		}
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
	coordNormalized /= sqrt(3);
	
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