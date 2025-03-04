using System.IO;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace AppNamespace;

public class LevelListContainer : IOnlineSyncTarget
{
	public const byte MARKER_ENTRY = 0;

	private const byte MARKER_CONTAINER_END = 85;

	private LevelList[] mScoreLists;

	private bool mChanged;

	private int mTransferCurrentListIndex;

	private int mTransferCurrentEntryIndex;

	private readonly object SYNC = new object();

	public LevelListContainer(int listCount, int listMaxSize)
	{
		mScoreLists = new LevelList[listCount];
		for (int i = 0; i < listCount; i++)
		{
			mScoreLists[i] = new LevelList(listMaxSize);
		}
	}

	public LevelListContainer(BinaryReader reader)
	{
		int num = reader.ReadInt32();
		mScoreLists = new LevelList[num];
		for (int i = 0; i < num; i++)
		{
			mScoreLists[i] = new LevelList(reader);
		}
	}

	public bool containsEntryForGamertag(int listIndex, string gamertag)
	{
		return mScoreLists[listIndex].containsEntryForGamertag(gamertag);
	}

	public bool containsEntryForGamertag(int listIndex, string gamertag, out int version, out int background, out float rating, out int ratingCount)
	{
		return mScoreLists[listIndex].containsEntryForGamertag(gamertag, out version, out background, out rating, out ratingCount);
	}

	public void fillPageFromFullList(int listIndex, int pageNumber, LevelEntry[] page)
	{
		mScoreLists[listIndex].fillPageFromFullList(pageNumber, page);
	}

	public void fillPageFromFilteredList(int listIndex, int pageNumber, LevelEntry[] page, SignedInGamer gamer)
	{
		mScoreLists[listIndex].fillPageFromFilteredList(pageNumber, page, gamer);
	}

	public int fillPageThatContainsGamertagFromFullList(int listIndex, LevelEntry[] page, string gamertag)
	{
		return mScoreLists[listIndex].fillPageThatContainsGamertagFromFullList(page, gamertag);
	}

	public int fillPageThatContainsGamertagFromFilteredList(int listIndex, LevelEntry[] page, SignedInGamer gamer)
	{
		return mScoreLists[listIndex].fillPageThatContainsGamertagFromFilteredList(page, gamer);
	}

	public void save(BinaryWriter writer)
	{
		writer.Write(mScoreLists.Length);
		LevelList[] array = mScoreLists;
		foreach (LevelList levelList in array)
		{
			levelList.write(writer);
		}
	}

	public void addEntry(int listIndex, LevelEntry entry, OnlineDataSyncManager manager)
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
			LevelList[] array = mScoreLists;
			foreach (LevelList levelList in array)
			{
				levelList.initForTransfer();
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
			LevelList levelList = mScoreLists[mTransferCurrentListIndex];
			mTransferCurrentEntryIndex = levelList.writeNextTransferEntry(writer, mTransferCurrentListIndex, mTransferCurrentEntryIndex);
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
