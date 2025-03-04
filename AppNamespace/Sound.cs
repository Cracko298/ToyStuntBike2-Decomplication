using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace AppNamespace;

public struct Sound
{
	public const int MAX_INSTANCES_PER_SOUND = 5;

	public SoundEffectInstance[] m_Instance;

	public int m_Index;

	public void Add(SoundEffect s)
	{
		for (int i = 0; i < 5; i++)
		{
			m_Instance[i] = s.CreateInstance();
		}
	}

	public SoundEffectInstance Play()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		m_Index++;
		if (m_Index >= 5)
		{
			m_Index = 0;
		}
		if ((int)m_Instance[m_Index].State == 0)
		{
			return null;
		}
		m_Instance[m_Index].Volume = 1f;
		m_Instance[m_Index].Play();
		return m_Instance[m_Index];
	}

	public SoundEffectInstance Play(float vol)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		m_Index++;
		if (m_Index >= 5)
		{
			m_Index = 0;
		}
		if ((int)m_Instance[m_Index].State == 0)
		{
			return null;
		}
		m_Instance[m_Index].Volume = vol;
		m_Instance[m_Index].Play();
		return m_Instance[m_Index];
	}

	public SoundEffectInstance Play3D(Vector2 pos)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		m_Index++;
		if (m_Index >= 5)
		{
			m_Index = 0;
		}
		if ((int)m_Instance[m_Index].State == 0)
		{
			return null;
		}
		float num = (pos.X - (Program.m_CameraManager3D.m_CameraPosition.X + CameraManager3D.CAMERA_POSITION_OFFSET_X)) / 50f;
		num = MathHelper.Clamp(num, -0.5f, 0.5f);
		m_Instance[m_Index].Play();
		m_Instance[m_Index].Pan = num;
		m_Instance[m_Index].Volume = 1f;
		return m_Instance[m_Index];
	}

	public SoundEffectInstance Play3D(Vector2 pos, float vol)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		m_Index++;
		if (m_Index >= 5)
		{
			m_Index = 0;
		}
		if ((int)m_Instance[m_Index].State == 0)
		{
			return null;
		}
		float num = (pos.X - (Program.m_CameraManager3D.m_CameraPosition.X + CameraManager3D.CAMERA_POSITION_OFFSET_X)) / 50f;
		num = MathHelper.Clamp(num, -0.5f, 0.5f);
		m_Instance[m_Index].Play();
		m_Instance[m_Index].Pan = num;
		m_Instance[m_Index].Volume = vol;
		return m_Instance[m_Index];
	}

	public SoundEffectInstance PlayLooped()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		m_Index++;
		if (m_Index >= 5)
		{
			m_Index = 0;
		}
		if ((int)m_Instance[m_Index].State == 0)
		{
			return null;
		}
		if (!m_Instance[m_Index].IsLooped)
		{
			m_Instance[m_Index].IsLooped = true;
		}
		m_Instance[m_Index].Resume();
		return m_Instance[m_Index];
	}

	public void StopLooped(SoundEffectInstance s)
	{
		for (int i = 0; i < 5; i++)
		{
			if (m_Instance[i] == s)
			{
				m_Instance[i].Stop();
			}
		}
	}

	public void Stop()
	{
		for (int i = 0; i < 5; i++)
		{
			m_Instance[i].Stop();
		}
	}
}
