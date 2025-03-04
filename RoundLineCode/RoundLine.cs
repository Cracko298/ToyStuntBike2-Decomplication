using System;
using Microsoft.Xna.Framework;

namespace RoundLineCode;

public class RoundLine
{
	private Vector2 p0;

	private Vector2 p1;

	private float rho;

	private float theta;

	public float alpha;

	public Vector2 P0
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return p0;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			p0 = value;
		}
	}

	public Vector2 P1
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return p1;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			p1 = value;
		}
	}

	public float Rho => rho;

	public float Theta => theta;

	public void Move(Vector2 p1, Vector2 p2)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		p0 = p1;
		this.p1 = p2;
		RecalcRhoTheta();
	}

	public RoundLine(Vector2 p0, Vector2 p1)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		this.p0 = p0;
		this.p1 = p1;
		RecalcRhoTheta();
	}

	public RoundLine(float x0, float y0, float x1, float y1)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		p0 = new Vector2(x0, y0);
		p1 = new Vector2(x1, y1);
		RecalcRhoTheta();
	}

	protected void RecalcRhoTheta()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = P1 - P0;
		rho = ((Vector2)(ref val)).Length();
		theta = (float)Math.Atan2(val.Y, val.X);
	}
}
