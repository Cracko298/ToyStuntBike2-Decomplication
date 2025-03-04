using System.IO;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace AppNamespace;

public class TopScoreListContainer : IOnlineSyncTarget
{
	public const byte MARKER_ENTRY = 0;

	private const byte MARKER_CONTAINER_END = 85;

	private TopScoreList[] mScoreLists;

	private bool mChanged;

	private int mTransferCurrentListIndex;

	private int mTransferCurrentEntryIndex;

	private readonly object SYNC = new object();

	public TopScoreListContainer(int listCount, int listMaxSize)
	{
		mScoreLists = new TopScoreList[listCount];
		for (int i = 0; i < listCount; i++)
		{
			mScoreLists[i] = new TopScoreList(listMaxSize);
		}
	}

	public TopScoreListContainer(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		mScoreLists = new TopScoreList[num];
		for (int i = 0; i < num; i++)
		{
			mScoreLists[i] = new TopScoreList(reader);
		}
	}

	public bool containsEntryForGamertag(int listIndex, string gamertag)
	{
		return mScoreLists[listIndex].containsEntryForGamertag(gamertag);
	}

	public void fillPageFromFullList(int listIndex, int pageNumber, TopScoreEntry[] page)
	{
		mScoreLists[listIndex].fillPageFromFullList(pageNumber, page);
	}

	public void fillPageFromFilteredList(int listIndex, int pageNumber, TopScoreEntry[] page, SignedInGamer gamer)
	{
		mScoreLists[listIndex].fillPageFromFilteredList(pageNumber, page, gamer);
	}

	public int fillPageThatContainsGamertagFromFullList(int listIndex, TopScoreEntry[] page, string gamertag)
	{
		return mScoreLists[listIndex].fillPageThatContainsGamertagFromFullList(page, gamertag);
	}

	public int fillPageThatContainsGamertagFromFilteredList(int listIndex, TopScoreEntry[] page, SignedInGamer gamer)
	{
		return mScoreLists[listIndex].fillPageThatContainsGamertagFromFilteredList(page, gamer);
	}

	public void save(BinaryWriter writer)
	{
		writer.Write(mScoreLists.Length);
		TopScoreList[] array = mScoreLists;
		foreach (TopScoreList topScoreList in array)
		{
			topScoreList.write(writer);
		}
	}

	public void addEntry(int listIndex, TopScoreEntry entry, OnlineDataSyncManager manager)
	{
		lock (SYNC)
		{
			if (mScoreLists[listIndex].addEntry(entry))
			{
				manager?.notifyAboutNewLocalEntry();
			}
		}
	}

	public int getFullListSize(int listIndex)
	{
		return mScoreLists[listIndex].getFullCount();
	}

	public int getFilteredListSize(int listIndex, SignedInGamer gamer)
	{
		return mScoreLists[listIndex].getFilteredCount(gamer);
	}

	public void startSynchronization()
	{
		mChanged = false;
		lock (SYNC)
		{
			TopScoreList[] array = mScoreLists;
			foreach (TopScoreList topScoreList in array)
			{
				topScoreList.initForTransfer();
			}
		}
	}

	public void endSynchronization()
	{
		if (mChanged)
		{
			if ((Program.m_App.m_State == App.STATE.MAINMENU || Program.m_App.m_State == App.STATE.FRONTENDSCORES || Program.m_App.m_State == App.STATE.HELP || Program.m_App.m_State == App.STATE.CREDITS) && Program.m_App.IsIdle())
			{
				Program.m_LoadSaveManager.SaveGame();
			}
			mChanged = false;
		}
	}

	public void prepareForSending()
	{
		mTransferCurrentListIndex = 0;
		mTransferCurrentEntryIndex = 0;
	}

	public bool readTransferRecord(PacketReader reader)
	{
		switch (((BinaryReader)(object)reader).ReadByte())
		{
		case 0:
		{
			int num = ((BinaryReader)(object)reader).ReadByte();
			if (num >= 0 && num < mScoreLists.Length)
			{
				lock (SYNC)
				{
					mChanged |= mScoreLists[num].readTransferEntry(reader);
				}
			}
			break;
		}
		case 85:
			return true;
		}
		return false;
	}

	public bool writeTransferRecord(PacketWriter writer)
	{
		while (true)
		{
			TopScoreList topScoreList = mScoreLists[mTransferCurrentListIndex];
			mTransferCurrentEntryIndex = topScoreList.writeNextTransferEntry(writer, mTransferCurrentListIndex, mTransferCurrentEntryIndex);
			if (mTransferCurrentEntryIndex > -1)
			{
				return false;
			}
			mTransferCurrentListIndex++;
			if (mTransferCurrentListIndex >= mScoreLists.Length)
			{
				break;
			}
			mTransferCurrentEntryIndex = 0;
		}
		((BinaryWriter)(object)writer).Write((byte)85);
		return true;
	}
}
