namespace AppNamespace;

public class ParticleManager
{
	public const int MAX_EMITTERS = 64;

	public Emitter[] m_Emitter;

	public int m_NextId;

	public ParticleManager()
	{
		m_Emitter = new Emitter[64];
		for (int i = 0; i < 64; i++)
		{
			m_Emitter[i] = new Emitter();
		}
	}

	public int Create(EmitterDef def)
	{
		//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		if (m_NextId >= 64)
		{
			m_NextId = 0;
		}
		int num = -1;
		bool flag = false;
		for (int i = m_NextId; i < 64; i++)
		{
			if (m_Emitter[i].m_Id == -1)
			{
				num = i;
				flag = true;
				m_NextId = i + 1;
				break;
			}
		}
		if (!flag)
		{
			for (int j = 0; j < 64; j++)
			{
				if (m_Emitter[j].m_Id == -1)
				{
					num = j;
					flag = true;
					m_NextId = j + 1;
					break;
				}
			}
		}
		if (flag)
		{
			m_Emitter[num].m_Id = num;
			m_Emitter[num].m_Def = def;
			m_Emitter[num].m_fNextParticleTime = 0f;
			m_Emitter[num].m_NextParticleId = 0;
			m_Emitter[num].m_Pauseable = true;
			m_Emitter[num].m_MaxParticles = def.m_NumParticles;
			m_Emitter[num].m_Velocity = def.m_Velocity + Emitter.RandXY(def.m_VelocityVar);
			return num;
		}
		return -1;
	}

	public void DeleteAll()
	{
		for (int i = 0; i < 64; i++)
		{
			m_Emitter[i].m_Id = -1;
			for (int j = 0; j < 512; j++)
			{
				m_Emitter[i].m_Particle[j].m_bActive = false;
			}
		}
	}

	public void Delete(int id)
	{
		m_Emitter[id].m_Id = -1;
		for (int i = 0; i < 512; i++)
		{
			m_Emitter[id].m_Particle[i].m_bActive = false;
		}
	}

	public void ClrInfinite(int id)
	{
		m_Emitter[id].m_Def.m_Flags &= 4294967291u;
	}

	public void Update()
	{
		for (int i = 0; i < 64; i++)
		{
			if (m_Emitter[i].m_Id != -1)
			{
				m_Emitter[i].Update();
			}
		}
	}

	public void Render()
	{
		for (int i = 0; i < 64; i++)
		{
			if (m_Emitter[i].m_Id != -1)
			{
				m_Emitter[i].Render();
			}
		}
	}
}
