using System.Collections.Generic;
using System.IO;

namespace AppNamespace;

public class RatingList
{
	public List<Rating> m_Rating;

	public RatingList()
	{
		m_Rating = new List<Rating>();
	}

	public void Add(float rating)
	{
		Rating rating2 = new Rating();
		rating2.m_LevelName = Program.m_App.m_PlayingUserLevelName;
		rating2.m_Rating = rating;
		m_Rating.Add(rating2);
	}

	public void Load(BinaryReader reader)
	{
		m_Rating.Clear();
		m_Rating.TrimExcess();
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			Rating rating = new Rating();
			rating.m_LevelName = reader.ReadString();
			rating.m_Rating = reader.ReadSingle();
			m_Rating.Add(rating);
		}
	}

	public void Save(BinaryWriter writer)
	{
		writer.Write(m_Rating.Count);
		for (int i = 0; i < m_Rating.Count; i++)
		{
			writer.Write(m_Rating[i].m_LevelName);
			writer.Write(m_Rating[i].m_Rating);
		}
	}
}
