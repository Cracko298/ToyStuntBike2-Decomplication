using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RoundLineCode;

public struct RoundLineVertex : IVertexType
{
	public Vector3 pos;

	public Vector2 rhoTheta;

	public Vector2 scaleTrans;

	public float index;

	public static int SizeInBytes = 32;

	public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration((VertexElement[])(object)new VertexElement[4]
	{
		new VertexElement(0, (VertexElementFormat)2, (VertexElementUsage)0, 0),
		new VertexElement(12, (VertexElementFormat)1, (VertexElementUsage)3, 0),
		new VertexElement(20, (VertexElementFormat)1, (VertexElementUsage)2, 0),
		new VertexElement(28, (VertexElementFormat)0, (VertexElementUsage)2, 1)
	});

	VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

	public RoundLineVertex(Vector3 pos, Vector2 norm, Vector2 tex, float index)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		this.pos = pos;
		rhoTheta = norm;
		scaleTrans = tex;
		this.index = index;
	}
}
