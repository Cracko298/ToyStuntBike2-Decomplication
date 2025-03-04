using Microsoft.Xna.Framework;

namespace RoundLineCode;

public class Disc : RoundLine
{
	public Vector2 Pos
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.P0;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			base.P0 = value;
			base.P1 = value;
		}
	}

	public Disc(Vector2 p)
		: base(p, p)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)
	//IL_0002: Unknown result type (might be due to invalid IL or missing references)


	public Disc(float x, float y)
		: base(x, y, x, y)
	{
	}
}
