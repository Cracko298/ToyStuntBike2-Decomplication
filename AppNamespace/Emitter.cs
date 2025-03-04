using System;
using Microsoft.Xna.Framework;

namespace AppNamespace;

public class Emitter
{
	public const int MAX_PARTICLES = 512;

	public const uint EMITTER_FLAG_LINE_TRAIL = 1u;

	public const uint EMITTER_FLAG_RANDOM_COLOUR = 2u;

	public const uint EMITTER_FLAG_INFINITE = 4u;

	public const uint EMITTER_FLAG_STRING = 8u;

	public const uint EMITTER_FLAG_SHAKE = 16u;

	public const uint EMITTER_FLAG_RANDOM_ROTATION = 32u;

	public const uint EMITTER_FLAG_MOVE_EMITTER = 64u;

	public const uint EMITTER_FLAG_SCALE_UP = 128u;

	public const uint EMITTER_FLAG_NORMAL_FADE = 256u;

	public const uint EMITTER_FLAG_SCROLL = 512u;

	public const uint EMITTER_FLAG_DOUBLE_SCALE = 1024u;

	public const uint EMITTER_FLAG_MUSHROOM = 2048u;

	public const uint EMITTER_FLAG_TRACKED_VEL = 4096u;

	public const uint EMITTER_FLAG_POS_IS_3D = 8192u;

	public Particle[] m_Particle;

	public int m_Id;

	public float m_fNextParticleTime;

	public int m_NextParticleId;

	public bool m_Pauseable;

	public int m_MaxParticles;

	public Vector2 m_Velocity;

	public Vector2 m_Direction;

	public EmitterDef m_Def;

	public Emitter()
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		m_Id = -1;
		m_fNextParticleTime = 0f;
		m_NextParticleId = 0;
		m_Pauseable = true;
		m_Velocity = Vector2.Zero;
		m_Particle = new Particle[512];
		for (int i = 0; i < 512; i++)
		{
			m_Particle[i] = default(Particle);
		}
	}

	public void SetPauseable(bool p)
	{
		m_Pauseable = p;
	}

	public void Update()
	{
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		if (m_Pauseable && Program.m_App.m_Paused)
		{
			return;
		}
		if ((m_Def.m_Flags & 0x40) != 0)
		{
			MoveEmitter();
		}
		if ((m_Def.m_Flags & 0x200) != 0)
		{
			ref Vector2 position = ref m_Def.m_Position;
			position.X -= 3f;
		}
		float num = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds;
		while (m_fNextParticleTime <= num && m_Def.m_NumParticles > 0)
		{
			m_Particle[m_NextParticleId].m_bActive = true;
			if ((m_Def.m_Flags & 0x1000) != 0 && m_Def.m_Track != null)
			{
				Vector2 val = m_Def.m_Velocity + RandXY(m_Def.m_VelocityVar);
				Matrix val2 = Matrix.CreateFromYawPitchRoll(m_Def.m_Track.m_Rotation.Y + (float)Math.PI / 2f, 0f, 0f - m_Def.m_Track.m_Rotation.X);
				m_Particle[m_NextParticleId].m_Velocity = Vector2.Transform(val, val2);
			}
			else
			{
				m_Particle[m_NextParticleId].m_Velocity = m_Def.m_Velocity + RandXY(m_Def.m_VelocityVar);
			}
			Vector3 zero = Vector3.Zero;
			if (m_Def.m_Track != null)
			{
				((Vector3)(ref zero))._002Ector(m_Def.m_Track.m_Position.X, m_Def.m_Track.m_Position.Y, m_Def.m_Track.m_ZDistance);
				Vector3 val3 = Program.m_CurrentCamera.WorldToScreen(zero);
				m_Def.m_Position = new Vector2(val3.X, val3.Y);
			}
			m_Particle[m_NextParticleId].m_Position = m_Def.m_Position + GetOffset();
			m_Particle[m_NextParticleId].m_fLife = num + m_Def.m_fLife;
			if (m_Def.m_fRotation != 0f)
			{
				m_Particle[m_NextParticleId].m_fRotation = (float)(Program.m_App.m_Rand.NextDouble() * Math.PI * 2.0);
			}
			else
			{
				m_Particle[m_NextParticleId].m_fRotation = 0f;
			}
			if ((m_Def.m_Flags & 0x20) != 0)
			{
				m_Particle[m_NextParticleId].m_fRotationAmount = m_Def.m_fRotation * (0.5f - (float)Program.m_App.m_Rand.NextDouble());
			}
			else
			{
				m_Particle[m_NextParticleId].m_fRotationAmount = m_Def.m_fRotation;
			}
			m_Particle[m_NextParticleId].m_fScale = m_Def.m_fScale;
			m_Particle[m_NextParticleId].m_EmitterId = m_Id;
			m_NextParticleId++;
			if (m_NextParticleId >= 512)
			{
				m_NextParticleId = 0;
			}
			if ((m_Def.m_Flags & 4) == 0)
			{
				m_Def.m_NumParticles--;
			}
			m_fNextParticleTime = num + m_Def.m_fRate;
		}
		bool flag = false;
		for (int i = 0; i < 512; i++)
		{
			if (m_Particle[i].m_bActive)
			{
				flag = true;
				m_Particle[i].Update();
			}
		}
		if (!flag)
		{
			m_Id = -1;
		}
	}

	public void MoveEmitter()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		ref EmitterDef def = ref m_Def;
		def.m_EmitterVelocity += m_Def.m_EmitterAcceleration;
		ref EmitterDef def2 = ref m_Def;
		def2.m_Position += m_Def.m_EmitterVelocity;
	}

	public Vector2 GetOffset()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		if (m_Def.m_Track == null)
		{
			return m_Def.m_Offset;
		}
		Vector2 offset = m_Def.m_Offset;
		Matrix val = Matrix.CreateFromYawPitchRoll(m_Def.m_Track.m_Rotation.Y + (float)Math.PI / 2f, 0f, 0f - m_Def.m_Track.m_Rotation.X);
		return Vector2.Transform(offset, val);
	}

	public void Render()
	{
		for (int i = 0; i < 512; i++)
		{
			if (m_Particle[i].m_bActive)
			{
				m_Particle[i].Render();
			}
		}
	}

	public static Vector2 RandXY(Vector2 var)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Vector2.Zero;
		float num = ((Vector2)(ref var)).Length();
		if ((float)Program.m_App.m_Rand.NextDouble() > 0.5f)
		{
			val.X = (float)Program.m_App.m_Rand.NextDouble() * (0f - var.X);
		}
		else
		{
			val.X = (float)Program.m_App.m_Rand.NextDouble() * var.X;
		}
		if ((float)Program.m_App.m_Rand.NextDouble() > 0.5f)
		{
			val.Y = (float)Program.m_App.m_Rand.NextDouble() * (0f - var.Y);
		}
		else
		{
			val.Y = (float)Program.m_App.m_Rand.NextDouble() * var.Y;
		}
		if (((Vector2)(ref val)).Length() > num * 0.707f)
		{
			val *= 0.707f;
		}
		return val;
	}
}
