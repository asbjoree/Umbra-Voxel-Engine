using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Umbra.Structures.Graphics
{
	static public class FragmentShader
	{

		// READ: http://www.opengl.org/wiki/GLSL_:_common_mistakes

		static public readonly string Shader = @"
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

void IterateVoxel(inout vec3 voxel, Ray ray, out vec3 hitPoint)
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
	
	if(maxX <= min(maxY, maxZ))
	{
		voxel.x += sign(ray.Direction.x);
		hitPoint = fract(ray.Origin + ray.Direction * maxX);
		hitPoint.x = 0.0;
	}
	if(maxY <= min(maxX, maxZ))
	{
		voxel.y += sign(ray.Direction.y);
		hitPoint = fract(ray.Origin + ray.Direction * maxY);
		hitPoint.y = 0.0;
	}
	if(maxZ <= min(maxX, maxY))
	{
		voxel.z += sign(ray.Direction.z);
		hitPoint = fract(ray.Origin + ray.Direction * maxZ);
		hitPoint.z = 0.0;
	}
}
	
vec3 GetRayColor(Ray ray)
{
	vec3 voxel = ray.Origin - fract(ray.Origin);
	vec3 hitPoint;
	
	for(int i = 0; i < 208; /*CAREFUL WITH THIS!!!*/ i++)
	{	
		if(ShouldDraw(voxel))
		{
			if(hitPoint.x == 0.0)
			{
				return texture2DArray(textures, vec3(1.0 - abs(hitPoint.zy), 1.0)).xyz  * 0.9;
			}
			if(hitPoint.y == 0.0)
			{
				return texture2DArray(textures, vec3(1.0 - abs(hitPoint.xz), 0.0)).xyz  * 1.0;
			}
			if(hitPoint.z == 0.0)
			{
				return texture2DArray(textures, vec3(1.0 - abs(hitPoint.xy), 1.0)).xyz  * 0.8;
			}
		}

		IterateVoxel(voxel, ray, hitPoint);
	}
	
	return vec3(0.39, 0.58, 0.93);
}

void GetCameraRay(const in vec3 position, const in vec3 lookAt, out Ray currentRay)
{
	vec3 worldUp = vec3(0.0, 1.0, 0.0);

	vec3 forward = normalize(lookAt - position);
	vec3 right = normalize(cross(forward, worldUp));
	vec3 up = cross(right, forward);

	vec2 coordNormalized = (gl_FragCoord.xy / resolution.xy * 2.0 - 1.0);
	coordNormalized.x *= (resolution.x / resolution.y);
	coordNormalized /= 1.731;
	
	currentRay.Origin = position;
		   
	currentRay.Direction = normalize( right * coordNormalized.x + up * coordNormalized.y + forward);
}

void main( void )
{
	Ray currentRay;

	vec3 offset = vec3(0.3, 1.5, 0.3);
	GetCameraRay(cam_pos + offset, cam_pos + cam_lookat + offset, currentRay);

	gl_FragColor = vec4(GetRayColor(currentRay), 0.5);
}";
	}
}
