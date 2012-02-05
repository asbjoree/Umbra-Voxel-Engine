uniform float time;
uniform vec2 resolution;
uniform vec3 cam_pos;
uniform vec3 cam_lookat;

uniform sampler2DArray textures;

struct Ray
{
	vec3 Origin;
	vec3 Direction;
};

	
bool ShouldDraw(vec3 voxel)
{
	return voxel.y < sin(length(voxel.xz)) * length(voxel.xz) / 10.0;
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
		if(ShouldDraw(voxel))
		{
			hitPoint = fract(ray.Origin + ray.Direction * maxX).zy;
			colorAccum += vec4(texture2DArray(textures, vec3(1.0 - abs(hitPoint), 1.0)).xyz  * 0.9, 1.0);
		}
	}
	if(maxY <= min(maxX, maxZ))
	{
		voxel.y += sign(ray.Direction.y);
		if(ShouldDraw(voxel))
		{
			hitPoint = fract(ray.Origin + ray.Direction * maxY).xz;
			colorAccum += vec4(texture2DArray(textures, vec3(1.0 - abs(hitPoint), 0.0)).xyz  * 1.0, 1.0);
		}
	}
	if(maxZ <= min(maxX, maxY))
	{
		voxel.z += sign(ray.Direction.z);
		if(ShouldDraw(voxel))
		{
			hitPoint = fract(ray.Origin + ray.Direction * maxZ).xy;
			colorAccum += vec4(texture2DArray(textures, vec3(1.0 - abs(hitPoint), 1.0)).xyz  * 0.8, 1.0);
		}
	}
	colorAccum += vec4(0.39, 0.58, 0.93, 1.0) * 0.01;
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