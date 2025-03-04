using System.IO;

namespace AppNamespace;

public class TopScoreEntry
{
	private string mGamertag;

	private int mScore;

	private bool mIsLocalEntry;

	private int mRankAtLastPageFill;

	public string Gamertag => mGamertag;

	public int Score => mScore;

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

	public TopScoreEntry(string gamertag, int score)
	{
		mGamertag = gamertag;
		mScore = score;
		mIsLocalEntry = true;
	}

	public TopScoreEntry(BinaryReader reader, bool isLocalEntry)
	{
		mGamertag = reader.ReadString();
		mScore = reader.ReadInt32();
		mIsLocalEntry = isLocalEntry;
	}

	public int compareTo(TopScoreEntry other)
	{
		if (mScore > other.mScore)
		{
			return -1;
		}
		if (mScore < other.mScore)
		{
			return 1;
		}
		return 0;
	}

	public void write(BinaryWriter writer)
	{
		writer.Write(mGamertag);
		writer.Write(mScore);
	}

	public bool isLegal()
	{
		if (mGamertag == null || mGamertag.Length < 1 || mGamertag.Length > 15 || mScore < 0)
		{
			return false;
		}
		for (int i = 0; i < mGamertag.Length; i++)
		{
			char c = mGamertag[i];
			switch (c)
			{
			case ' ':
			case 'A':
			case 'B':
			case 'C':
			case 'D':
			case 'E':
			case 'F':
			case 'G':
			case 'H':
			case 'I':
			case 'J':
			case 'K':
			case 'L':
			case 'M':
			case 'N':
			case 'O':
			case 'P':
			case 'Q':
			case 'R':
			case 'S':
			case 'T':
			case 'U':
			case 'V':
			case 'W':
			case 'X':
			case 'Y':
			case 'Z':
				continue;
			}
			if ((c < 'a' || c > 'z') && (c < '0' || c > '9'))
			{
				return false;
			}
		}
		return true;
	}
}
