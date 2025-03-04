using Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppNamespace;

public struct Particle
{
	public Vector2 m_Position;

	public Vector2 m_Velocity;

	public int m_Frame;

	public float m_fNextFrame;

	public int m_EmitterId;

	public float m_fRotation;

	public float m_fRotationAmount;

	public float m_fScale;

	public float m_fLife;

	public bool m_bActive;

	public float m_fAlpha;

	public void Update()
	{
		if ((Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Flags & 0x800) != 0)
		{
			float num = m_fLife - (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds;
			num /= Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_fLife;
			num = 1f - num;
			num *= num;
			float num2 = 0.01f;
			if (m_fRotationAmount > 0f)
			{
				num2 = 0f - num2;
			}
			Fn.Rotate(ref m_Velocity, num2 * (num * 30f));
		}
		ref Vector2 velocity = ref m_Velocity;
		velocity.X += Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Acceleration.X;
		ref Vector2 velocity2 = ref m_Velocity;
		velocity2.Y += Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Acceleration.Y;
		ref Vector2 velocity3 = ref m_Velocity;
		velocity3.X *= Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Friction.X;
		ref Vector2 velocity4 = ref m_Velocity;
		velocity4.Y *= Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Friction.Y;
		ref Vector2 position = ref m_Position;
		position.X += m_Velocity.X;
		ref Vector2 position2 = ref m_Position;
		position2.Y += m_Velocity.Y;
		if ((Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Flags & 0x200) != 0)
		{
			ref Vector2 position3 = ref m_Position;
			position3.X -= 3f;
		}
		if (m_fLife < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
		{
			m_bActive = false;
		}
	}

	public void Render()
	{
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_04eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0473: Unknown result type (might be due to invalid IL or missing references)
		//IL_0475: Unknown result type (might be due to invalid IL or missing references)
		//IL_047c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Unknown result type (might be due to invalid IL or missing references)
		//IL_040d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0412: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		float num = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds;
		float num2 = m_fLife - num;
		if (m_fNextFrame < num)
		{
			m_Frame++;
			if (m_Frame >= Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_NumFrames)
			{
				m_Frame = 0;
			}
		}
		if (!Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Pauseable || !Program.m_App.m_Paused)
		{
			if ((Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Flags & 0x100) != 0)
			{
				m_fAlpha = num2 / Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_fLife;
				if (m_fAlpha < 0f)
				{
					m_fAlpha = 0f;
				}
				if (m_fAlpha > 1f)
				{
					m_fAlpha = 1f;
				}
			}
			else if (num2 < Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_fLife * 0.25f)
			{
				m_fAlpha = num2 / (Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_fLife * 0.25f);
				if (m_fAlpha < 0f)
				{
					m_fAlpha = 0f;
				}
				if (m_fAlpha > 1f)
				{
					m_fAlpha = 1f;
				}
			}
			else
			{
				m_fAlpha = 1f;
			}
			m_fRotation += m_fRotationAmount;
		}
		Color colour = Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Colour;
		if (!Program.m_App.m_Paused && (Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Flags & 2) != 0)
		{
			((Color)(ref colour)).R = (byte)(Program.m_App.m_Rand.NextDouble() * 255.0);
			((Color)(ref colour)).G = (byte)(Program.m_App.m_Rand.NextDouble() * 255.0);
			((Color)(ref colour)).B = (byte)(Program.m_App.m_Rand.NextDouble() * 255.0);
		}
		((Color)(ref colour)).A = (byte)(m_fAlpha * 255f * ((float)(int)((Color)(ref colour)).A / 255f));
		((Color)(ref colour)).R = (byte)((float)(int)((Color)(ref colour)).R * ((float)(int)((Color)(ref colour)).A / 255f));
		((Color)(ref colour)).G = (byte)((float)(int)((Color)(ref colour)).G * ((float)(int)((Color)(ref colour)).A / 255f));
		((Color)(ref colour)).B = (byte)((float)(int)((Color)(ref colour)).B * ((float)(int)((Color)(ref colour)).A / 255f));
		float num3 = m_fScale;
		if (!Program.m_App.m_Paused)
		{
			if ((Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Flags & 0x80) != 0)
			{
				num3 *= 1f - num2 / Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_fLife;
			}
			if ((Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Flags & 0x400) != 0)
			{
				num3 = Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_fScale + (1f - num2 / Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_fLife);
			}
		}
		Texture2D image = Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Image;
		if (image != null)
		{
			Rectangle value = default(Rectangle);
			((Rectangle)(ref value))._002Ector(m_Frame * image.Height, 0, image.Height, image.Height);
			Vector2 position = m_Position;
			if ((Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Flags & 0x2000) != 0)
			{
				Vector3 zero = Vector3.Zero;
				zero.X = m_Position.X;
				zero.Y = m_Position.Y;
				zero.Z = 0f;
				Vector3 val = Program.m_CurrentCamera.WorldToScreen(zero);
				((Vector2)(ref position))._002Ector(val.X, val.Y);
			}
			Program.m_App.m_SpriteBatch.Draw(image, position, (Rectangle?)value, colour, m_fRotation, Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Origin, num3, (SpriteEffects)0, 0f);
		}
		if ((Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Flags & 1) != 0)
		{
			Program.m_App.DrawLine(m_Position, m_Position - m_Velocity * 2f, colour, 2f);
		}
		if ((Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Flags & 8) != 0)
		{
			Program.m_App.m_SpriteBatch.DrawString(Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Font.mFont, Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_String, m_Position, colour, 0f, Vector2.Zero, num3, (SpriteEffects)0, 0f);
		}
		if (!Program.m_App.m_Paused && (Program.m_ParticleManager.m_Emitter[m_EmitterId].m_Def.m_Flags & 0x10) != 0)
		{
			ref Vector2 position2 = ref m_Position;
			position2.X += (float)Program.m_App.m_Rand.Next(-7, 7);
			ref Vector2 position3 = ref m_Position;
			position3.Y += (float)Program.m_App.m_Rand.Next(-7, 7);
		}
	}
}
