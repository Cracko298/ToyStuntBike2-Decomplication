using Microsoft.Xna.Framework.Net;

namespace AppNamespace;

public class SyncTargetContainer : IOnlineSyncTarget
{
	private int mSendStepIndex;

	private int mReceiveStepIndex;

	private TopScoreListContainer mScoreContainer;

	private LevelListContainer mLevelsContainer;

	public SyncTargetContainer(TopScoreListContainer scoreContainer, LevelListContainer levelsContainer)
	{
		mScoreContainer = scoreContainer;
		mLevelsContainer = levelsContainer;
	}

	public void startSynchronization()
	{
		mSendStepIndex = 0;
		mReceiveStepIndex = 0;
		mScoreContainer.startSynchronization();
		mLevelsContainer.startSynchronization();
	}

	public void endSynchronization()
	{
		mScoreContainer.endSynchronization();
		mLevelsContainer.endSynchronization();
	}

	public void prepareForSending()
	{
		mScoreContainer.prepareForSending();
		mLevelsContainer.prepareForSending();
	}

	public bool writeTransferRecord(PacketWriter writer)
	{
		if (mSendStepIndex == 0)
		{
			if (mScoreContainer.writeTransferRecord(writer))
			{
				mSendStepIndex++;
			}
			return false;
		}
		return mLevelsContainer.writeTransferRecord(writer);
	}

	public bool readTransferRecord(PacketReader reader)
	{
		if (mReceiveStepIndex == 0)
		{
			if (mScoreContainer.readTransferRecord(reader))
			{
				mReceiveStepIndex++;
			}
			return false;
		}
		return mLevelsContainer.readTransferRecord(reader);
	}
}
