using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace AppNamespace;

public class TopScoreList
{
	private int mMaxSize;

	private List<TopScoreEntry> mEntryList;

	private List<TopScoreEntry> mFilteredList;

	private Dictionary<string, TopScoreEntry> mEntryMap;

	private readonly object SYNC = new object();

	public TopScoreList(int maxSize)
	{
		mMaxSize = maxSize;
		mEntryList = new List<TopScoreEntry>();
		mFilteredList = new List<TopScoreEntry>();
		mEntryMap = new Dictionary<string, TopScoreEntry>();
	}

	public TopScoreList(BinaryReader reader)
		: this(reader.ReadInt32())
	{
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			TopScoreEntry topScoreEntry = new TopScoreEntry(reader, isLocalEntry: true);
			if (topScoreEntry.isLegal())
			{
				mEntryMap[topScoreEntry.Gamertag] = topScoreEntry;
				mEntryList.Add(topScoreEntry);
			}
		}
	}

	public bool containsEntryForGamertag(string gamertag)
	{
		lock (SYNC)
		{
			for (int i = 0; i < mEntryList.Count; i++)
			{
				if (mEntryList[i].Gamertag == gamertag)
				{
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

	public void fillPageFromFullList(int pageNumber, TopScoreEntry[] page)
	{
		lock (SYNC)
		{
			fillPage(mEntryList, initRank: true, pageNumber, page);
		}
	}

	public void fillPageFromFilteredList(int pageNumber, TopScoreEntry[] page, SignedInGamer gamer)
	{
		lock (SYNC)
		{
			initFilteredList(gamer, initRank: true);
			fillPage(mFilteredList, initRank: false, pageNumber, page);
		}
	}

	public int fillPageThatContainsGamertagFromFullList(TopScoreEntry[] page, string gamertag)
	{
		lock (SYNC)
		{
			int num = 0;
			for (int i = 0; i < mEntryList.Count; i++)
			{
				if (mEntryList[i].Gamertag == gamertag)
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

	public int fillPageThatContainsGamertagFromFilteredList(TopScoreEntry[] page, SignedInGamer gamer)
	{
		lock (SYNC)
		{
			initFilteredList(gamer, initRank: true);
			int num = 0;
			for (int i = 0; i < mFilteredList.Count; i++)
			{
				if (mFilteredList[i].Gamertag == ((Gamer)gamer).Gamertag)
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

	private void fillPage(List<TopScoreEntry> list, bool initRank, int pageNumber, TopScoreEntry[] page)
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
			TopScoreEntry topScoreEntry = mEntryList[i];
			if (topScoreEntry.Gamertag == gamertag)
			{
				mFilteredList.Add(topScoreEntry);
				if (initRank)
				{
					topScoreEntry.RankAtLastPageFill = i + 1;
				}
				continue;
			}
			GamerCollectionEnumerator<FriendGamer> enumerator = ((GamerCollection<FriendGamer>)(object)friends).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					FriendGamer current = enumerator.Current;
					if (topScoreEntry.Gamertag == ((Gamer)current).Gamertag)
					{
						mFilteredList.Add(topScoreEntry);
						if (initRank)
						{
							topScoreEntry.RankAtLastPageFill = i + 1;
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
			foreach (TopScoreEntry mEntry in mEntryList)
			{
				mEntry.write(writer);
			}
		}
	}

	public bool addEntry(TopScoreEntry entry)
	{
		if (!entry.isLegal())
		{
			return false;
		}
		lock (SYNC)
		{
			string gamertag = entry.Gamertag;
			if (mEntryMap.ContainsKey(gamertag))
			{
				TopScoreEntry topScoreEntry = mEntryMap[gamertag];
				int num = entry.compareTo(topScoreEntry);
				if (num < 0)
				{
					return false;
				}
				if (num == 0)
				{
					topScoreEntry.IsLocalEntry = entry.IsLocalEntry;
					return false;
				}
				mEntryList.Remove(topScoreEntry);
				addNewEntry(entry);
				return true;
			}
			return addNewEntry(entry);
		}
	}

	private bool addNewEntry(TopScoreEntry entry)
	{
		for (int i = 0; i < mEntryList.Count; i++)
		{
			if (entry.compareTo(mEntryList[i]) >= 0)
			{
				mEntryList.Insert(i, entry);
				mEntryMap[entry.Gamertag] = entry;
				if (mEntryList.Count > mMaxSize)
				{
					TopScoreEntry topScoreEntry = mEntryList[mMaxSize];
					mEntryList.RemoveAt(mMaxSize);
					mEntryMap.Remove(topScoreEntry.Gamertag);
				}
				return true;
			}
		}
		if (mEntryList.Count < mMaxSize)
		{
			mEntryList.Add(entry);
			mEntryMap[entry.Gamertag] = entry;
			return true;
		}
		return false;
	}

	public void initForTransfer()
	{
		lock (SYNC)
		{
			foreach (TopScoreEntry mEntry in mEntryList)
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
			return addEntry(new TopScoreEntry((BinaryReader)(object)reader, isLocalEntry: false));
		}
	}
}
