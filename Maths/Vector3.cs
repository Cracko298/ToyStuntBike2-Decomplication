using System;

namespace Maths;

public struct Vector3
{
	public float x;

	public float y;

	public float z;

	public Vector3(float inx, float iny, float inz)
	{
		x = inx;
		y = iny;
		z = inz;
	}

	public static Vector3 operator *(float t, Vector3 v)
	{
		return new Vector3(t * v.x, t * v.y, t * v.z);
	}

	public static Vector3 operator /(float t, Vector3 v)
	{
		return new Vector3(v.x / t, v.y / t, v.z / t);
	}

	public float Dot(Vector3 v)
	{
		return x * v.x + y * v.y + z * v.z;
	}

	public Vector3 Cross(Vector3 v)
	{
		return new Vector3(y * v.z - z * v.y, z * v.x - x * v.z, x * v.y - y * v.x);
	}

	public float MagnitudeSquared()
	{
		return x * x + y * y + z * z;
	}

	public float Magnitude()
	{
		return (float)Math.Sqrt(x * x + y * y + z * z);
	}

	public void Normalise()
	{
		float num = Fn.InvSqrt(x * x + y * y + z * z);
		x *= num;
		y *= num;
		z *= num;
	}

	public float NormaliseLen()
	{
		float num = Fn.InvSqrt(x * x + y * y + z * z);
		x *= num;
		y *= num;
		z *= num;
		return 1f / num;
	}
}
