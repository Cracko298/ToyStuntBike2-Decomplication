using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace AppNamespace;

public class SoundManager
{
	public enum SFX
	{
		MenuUpSound,
		MenuDownSound,
		MenuSelectSound,
		MenuBackSound,
		MapMoveSound,
		MapUnlockSound,
		SpeechSound,
		SpeechEndSound,
		BikeIdle1Sound,
		BikeAccel1Sound,
		Body1Sound,
		Body2Sound,
		Body3Sound,
		Body4Sound,
		Body5Sound,
		Crash1Sound,
		Crash2Sound,
		Crash3Sound,
		Crash4Sound,
		Crash5Sound,
		Crash6Sound,
		Crash7Sound,
		Crash8Sound,
		Crash9Sound,
		Crash10Sound,
		Crash11Sound,
		Ready,
		Go,
		Land1,
		Land2,
		CollectFlag,
		Bump3,
		Bump4,
		Skid1,
		Skid2,
		Finished,
		NewLevel,
		Hiss,
		Horn,
		Checkpoint,
		Flush,
		Beep,
		Unlock,
		UnlockLevel,
		END
	}

	public Sound[] m_Sound;

	public SoundManager()
	{
		m_Sound = new Sound[44];
		for (int i = 0; i < 44; i++)
		{
			m_Sound[i].m_Instance = (SoundEffectInstance[])(object)new SoundEffectInstance[5];
		}
	}

	public SoundEffectInstance Play(int id)
	{
		return m_Sound[id].Play();
	}

	public SoundEffectInstance Play(int id, float vol)
	{
		return m_Sound[id].Play(vol);
	}

	public SoundEffectInstance Play3D(int id, Vector2 pos)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return m_Sound[id].Play3D(pos);
	}

	public SoundEffectInstance Play3D(int id, Vector2 pos, float vol)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		return m_Sound[id].Play3D(pos, vol);
	}

	public SoundEffectInstance PlayLooped(int id)
	{
		return m_Sound[id].PlayLooped();
	}

	public void Add(int id, SoundEffect s)
	{
		m_Sound[id].Add(s);
	}

	public bool IsPlaying(int id)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 5; i++)
		{
			if ((int)m_Sound[id].m_Instance[i].State == 0)
			{
				return true;
			}
		}
		return false;
	}

	public void Stop(int id)
	{
		m_Sound[id].Stop();
	}
}
