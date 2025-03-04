using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppNamespace;

public struct EmitterDef
{
	public Vector2 m_Acceleration;

	public Vector2 m_Friction;

	public Texture2D m_Image;

	public int m_NumFrames;

	public float m_fFrameRate;

	public Vector2 m_Origin;

	public Vector2 m_Position;

	public float m_fLife;

	public float m_fRate;

	public Vector2 m_Velocity;

	public Vector2 m_VelocityVar;

	public int m_NumParticles;

	public Color m_Colour;

	public float m_fRotation;

	public Actor m_Track;

	public uint m_Flags;

	public Vector2 m_Offset;

	public float m_fScale;

	public Text m_Font;

	public string m_String;

	public Vector2 m_EmitterVelocity;

	public Vector2 m_EmitterAcceleration;
}
