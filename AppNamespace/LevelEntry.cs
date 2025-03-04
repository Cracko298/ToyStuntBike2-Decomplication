using System.IO;

namespace AppNamespace;

public class LevelEntry
{
	private const int LEVELDATASIZE = 110;

	public string m_LevelName;

	public int m_Background;

	public float m_Rating;

	public int m_RatingCount;

	public int m_Version;

	public int m_TriggerCount;

	public LevelData[] m_LevelData;

	private bool mIsLocalEntry;

	private int mRankAtLastPageFill;

	public bool IsLocalEntry
	{
		get
		{
			return mIsLocalEntry;
		}
		set
		{
			mIsLocalEntry = value;
		}
	}

	public int RankAtLastPageFill
	{
		get
		{
			return mRankAtLastPageFill;
		}
		set
		{
			mRankAtLastPageFill = value;
		}
	}

	public LevelEntry(string levelName, int version, int background, float rating, int ratingCount)
	{
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
		mIsLocalEntry = true;
		m_LevelName = levelName;
		m_Version = version;
		m_Background = background;
		m_Rating = rating;
		m_RatingCount = ratingCount;
		if (m_RatingCount < 1)
		{
			m_RatingCount = 1;
		}
		m_LevelData = new LevelData[110];
		for (int i = 0; i < 110; i++)
		{
			m_LevelData[i] = new LevelData();
		}
		m_TriggerCount = 0;
		for (int j = 0; j < 512; j++)
		{
			if (Program.m_TriggerManager.m_Trigger[j].m_Id != -1)
			{
				m_TriggerCount++;
			}
		}
		int num = 0;
		for (int k = 0; k < 512; k++)
		{
			if (Program.m_TriggerManager.m_Trigger[k].m_Id != -1)
			{
				m_LevelData[num].m_Type = Program.m_TriggerManager.m_Trigger[k].m_Type;
				m_LevelData[num].m_Position = Program.m_TriggerManager.m_Trigger[k].m_Position;
				m_LevelData[num].m_Rotation = Program.m_TriggerManager.m_Trigger[k].m_Rotation;
				if (num < 109)
				{
					num++;
				}
			}
		}
	}

	public LevelEntry(BinaryReader reader, bool isLocalEntry)
	{
		mIsLocalEntry = isLocalEntry;
		m_LevelName = reader.ReadString();
		m_Background = reader.ReadInt32();
		m_Rating = reader.ReadSingle();
		m_RatingCount = reader.ReadInt32();
		m_Version = reader.ReadInt32();
		m_TriggerCount = reader.ReadInt32();
		if (m_TriggerCount > 109)
		{
			m_TriggerCount = 109;
		}
		m_LevelData = new LevelData[110];
		for (int i = 0; i < 110; i++)
		{
			m_LevelData[i] = new LevelData();
		}
		for (int j = 0; j < m_TriggerCount; j++)
		{
			m_LevelData[j].m_Type = reader.ReadInt32();
			m_LevelData[j].m_Position.X = reader.ReadSingle();
			m_LevelData[j].m_Position.Y = reader.ReadSingle();
			m_LevelData[j].m_Rotation = reader.ReadSingle();
		}
	}

	public int compareTo(LevelEntry other)
	{
		float num = m_Rating / (float)m_RatingCount;
		float num2 = other.m_Rating / (float)other.m_RatingCount;
		if (num < num2)
		{
			return -1;
		}
		if (num > num2)
		{
			return 1;
		}
		return 0;
	}

	public int compareByRatingCount(LevelEntry existing)
	{
		if (m_RatingCount < existing.m_RatingCount)
		{
			return -1;
		}
		if (m_RatingCount > existing.m_RatingCount)
		{
			return 1;
		}
		if (m_Version > existing.m_Version)
		{
			return 1;
		}
		return 0;
	}

	public void write(BinaryWriter writer)
	{
		writer.Write(m_LevelName);
		writer.Write(m_Background);
		writer.Write(m_Rating);
		writer.Write(m_RatingCount);
		writer.Write(m_Version);
		if (m_TriggerCount > 109)
		{
			m_TriggerCount = 109;
		}
		writer.Write(m_TriggerCount);
		for (int i = 0; i < m_TriggerCount; i++)
		{
			writer.Write(m_LevelData[i].m_Type);
			writer.Write(m_LevelData[i].m_Position.X);
			writer.Write(m_LevelData[i].m_Position.Y);
			writer.Write(m_LevelData[i].m_Rotation);
		}
	}

	public bool isLegal()
	{
		return true;
	}
}
