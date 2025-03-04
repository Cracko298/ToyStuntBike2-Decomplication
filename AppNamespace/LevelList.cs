using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace AppNamespace;

public class LevelList
{
	private int mMaxSize;

	private List<LevelEntry> mEntryList;

	private List<LevelEntry> mFilteredList;

	private Dictionary<string, LevelEntry> mEntryMap;

	private readonly object SYNC = new object();

	public LevelList(int maxSize)
	{
		mMaxSize = maxSize;
		mEntryList = new List<LevelEntry>();
		mFilteredList = new List<LevelEntry>();
		mEntryMap = new Dictionary<string, LevelEntry>();
	}

	public LevelList(BinaryReader reader)
		: this(reader.ReadInt32())
	{
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			LevelEntry levelEntry = new LevelEntry(reader, isLocalEntry: true);
			if (levelEntry.isLegal())
			{
				mEntryMap[levelEntry.m_LevelName] = levelEntry;
				mEntryList.Add(levelEntry);
			}
		}
	}

	public bool containsEntryForGamertag(string gamertag)
	{
		lock (SYNC)
		{
			for (int i = 0; i < mEntryList.Count; i++)
			{
				if (mEntryList[i].m_LevelName == gamertag)
				{
					return true;
				}
			}
			return false;
		}
	}

	public bool containsEntryForGamertag(string gamertag, out int version, out int background, out float rating, out int ratingCount)
	{
		lock (SYNC)
		{
			version = 0;
			background = 0;
			rating = 3f;
			ratingCount = 1;
			for (int i = 0; i < mEntryList.Count; i++)
			{
				if (mEntryList[i].m_LevelName == gamertag)
				{
					rating = mEntryList[i].m_Rating;
					ratingCount = mEntryList[i].m_RatingCount;
					version = mEntryList[i].m_Version;
					background = mEntryList[i].m_Background;
					return true;
				}
			}
			return false;
		}
	}

	public int getFullCount()
	{
		lock (SYNC)
		{
			return mEntryList.Count;
		}
	}

	public int getFilteredCount(SignedInGamer gamer)
	{
		lock (SYNC)
		{
			initFilteredList(gamer, initRank: false);
			return mFilteredList.Count;
		}
	}

	public void fillPageFromFullList(int pageNumber, LevelEntry[] page)
	{
		lock (SYNC)
		{
			fillPage(mEntryList, initRank: true, pageNumber, page);
		}
	}

	public void fillPageFromFilteredList(int pageNumber, LevelEntry[] page, SignedInGamer gamer)
	{
		lock (SYNC)
		{
			initFilteredList(gamer, initRank: true);
			fillPage(mFilteredList, initRank: false, pageNumber, page);
		}
	}

	public int fillPageThatContainsGamertagFromFullList(LevelEntry[] page, string gamertag)
	{
		lock (SYNC)
		{
			int num = 0;
			for (int i = 0; i < mEntryList.Count; i++)
			{
				if (mEntryList[i].m_LevelName == gamertag)
				{
					num = i;
					break;
				}
			}
			int num2 = num / page.Length;
			fillPage(mEntryList, initRank: true, num2, page);
			return num2;
		}
	}

	public int fillPageThatContainsGamertagFromFilteredList(LevelEntry[] page, SignedInGamer gamer)
	{
		lock (SYNC)
		{
			initFilteredList(gamer, initRank: true);
			int num = 0;
			for (int i = 0; i < mFilteredList.Count; i++)
			{
				if (mFilteredList[i].m_LevelName == ((Gamer)gamer).Gamertag)
				{
					num = i;
					break;
				}
			}
			int num2 = num / page.Length;
			fillPage(mFilteredList, initRank: false, num2, page);
			return num2;
		}
	}

	private void fillPage(List<LevelEntry> list, bool initRank, int pageNumber, LevelEntry[] page)
	{
		int num = pageNumber * page.Length;
		for (int i = 0; i < page.Length; i++)
		{
			if (num >= 0 && num < list.Count)
			{
				page[i] = list[num];
				if (initRank)
				{
					page[i].RankAtLastPageFill = num + 1;
				}
			}
			else
			{
				page[i] = null;
			}
			num++;
		}
	}

	private void initFilteredList(SignedInGamer gamer, bool initRank)
	{
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		string gamertag = ((Gamer)gamer).Gamertag;
		FriendCollection friends = gamer.GetFriends();
		mFilteredList.Clear();
		for (int i = 0; i < mEntryList.Count; i++)
		{
			LevelEntry levelEntry = mEntryList[i];
			if (levelEntry.m_LevelName == gamertag)
			{
				mFilteredList.Add(levelEntry);
				if (initRank)
				{
					levelEntry.RankAtLastPageFill = i + 1;
				}
				continue;
			}
			GamerCollectionEnumerator<FriendGamer> enumerator = ((GamerCollection<FriendGamer>)(object)friends).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					FriendGamer current = enumerator.Current;
					if (levelEntry.m_LevelName == ((Gamer)current).Gamertag)
					{
						mFilteredList.Add(levelEntry);
						if (initRank)
						{
							levelEntry.RankAtLastPageFill = i + 1;
						}
						break;
					}
				}
			}
			finally
			{
				((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
			}
		}
	}

	public void write(BinaryWriter writer)
	{
		lock (SYNC)
		{
			writer.Write(mMaxSize);
			writer.Write(mEntryList.Count);
			foreach (LevelEntry mEntry in mEntryList)
			{
				mEntry.write(writer);
			}
		}
	}

	public bool addEntry(LevelEntry entry)
	{
		if (!entry.isLegal())
		{
			return false;
		}
		lock (SYNC)
		{
			string levelName = entry.m_LevelName;
			if (mEntryMap.ContainsKey(levelName))
			{
				LevelEntry levelEntry = mEntryMap[levelName];
				int num = entry.compareByRatingCount(levelEntry);
				if (num < 0)
				{
					return false;
				}
				if (num == 0)
				{
					levelEntry.IsLocalEntry = entry.IsLocalEntry;
					return false;
				}
				mEntryList.Remove(levelEntry);
				addNewEntry(entry);
				return true;
			}
			return addNewEntry(entry);
		}
	}

	private bool addNewEntry(LevelEntry entry)
	{
		for (int i = 0; i < mEntryList.Count; i++)
		{
			if (entry.compareTo(mEntryList[i]) >= 0)
			{
				mEntryList.Insert(i, entry);
				mEntryMap[entry.m_LevelName] = entry;
				if (mEntryList.Count > mMaxSize)
				{
					LevelEntry levelEntry = mEntryList[mMaxSize];
					mEntryList.RemoveAt(mMaxSize);
					mEntryMap.Remove(levelEntry.m_LevelName);
				}
				return true;
			}
		}
		if (mEntryList.Count < mMaxSize)
		{
			mEntryList.Add(entry);
			mEntryMap[entry.m_LevelName] = entry;
			return true;
		}
		return false;
	}

	public void initForTransfer()
	{
		lock (SYNC)
		{
			foreach (LevelEntry mEntry in mEntryList)
			{
				mEntry.IsLocalEntry = true;
			}
		}
	}

	public int writeNextTransferEntry(PacketWriter writer, int myListIndex, int entryIndex)
	{
		lock (SYNC)
		{
			while (entryIndex < mEntryList.Count)
			{
				if (mEntryList[entryIndex].IsLocalEntry)
				{
					((BinaryWriter)(object)writer).Write((byte)0);
					((BinaryWriter)(object)writer).Write((byte)myListIndex);
					mEntryList[entryIndex].write((BinaryWriter)(object)writer);
					return entryIndex + 1;
				}
				entryIndex++;
				Thread.Sleep(1);
			}
			return -1;
		}
	}

	public bool readTransferEntry(PacketReader reader)
	{
		lock (SYNC)
		{
			return addEntry(new LevelEntry((BinaryReader)(object)reader, isLocalEntry: false));
		}
	}
}
