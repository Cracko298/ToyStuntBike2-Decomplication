using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace AppNamespace;

public class OnlineDataSyncManager : GameComponent
{
	public enum Mode
	{
		UNKNOWN,
		SERVER,
		CLIENT
	}

	public enum State
	{
		IDLE,
		SEND,
		RECEIVE,
		WAIT_FOR_END
	}

	private enum Action
	{
		NONE,
		PROCESS,
		EXIT_THREAD,
		DISABLE
	}

	private enum SessionProperty
	{
		VERSION
	}

	private class HostInfo
	{
		public string mHostGamertag;

		public double mWaitTime;

		public HostInfo(string hostGamertag, double waitTime)
		{
			mHostGamertag = hostGamertag;
			mWaitTime = waitTime;
		}
	}

	private const int MILLIS_IN_MINUTE = 60000;

	private const double WAIT_TIME_FOR_RECENT_HOST = 1800000.0;

	private const int MINIMUM_SERVER_TIME = 240000;

	private const int MAXIMUM_SERVER_TIME = 420000;

	private const int SLEEP_DURATION = 12;

	public const NetworkSessionType SESSION_TYPE = 2;

	private Action mAction;

	private NetworkSession mSession;

	private string mHostGamertag;

	private List<HostInfo> mRecentHosts;

	private Dictionary<string, string> mRecentHostsLookup;

	private double mServerMillisToLive;

	private Random mRandom;

	private IOnlineSyncTarget mSyncTarget;

	private PacketWriter mWriter;

	private PacketReader mReader;

	private bool mHasNewLocalEntry;

	private AfterStopDelegate mAfterStopDelegate;

	private LocalNetworkGamer mLocalGamerForSendingAndReceiving;

	private NetworkGamer mRemoteGamerForSending;

	private readonly object SYNC = new object();

	private List<SignedInGamer> mHostGamer;

	private Mode mMode;

	private State mState;

	private int mVersion;

	public SignedInGamer HostGamer
	{
		get
		{
			if (mHostGamer.Count > 0)
			{
				return mHostGamer[0];
			}
			return null;
		}
	}

	public Mode MyMode => mMode;

	public State MyState => mState;

	public int MyVersion => mVersion;

	public OnlineDataSyncManager(int version, Game game)
		: base(game)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		mVersion = version;
		mHostGamer = new List<SignedInGamer>();
		mRecentHosts = new List<HostInfo>();
		mRecentHostsLookup = new Dictionary<string, string>();
		mRandom = new Random();
		mWriter = new PacketWriter();
		mReader = new PacketReader();
		((GameComponent)this).Enabled = false;
		mAction = Action.NONE;
	}

	public override void Update(GameTime gameTime)
	{
		lock (SYNC)
		{
			if (mSession != null && !mSession.IsDisposed)
			{
				try
				{
					mSession.Update();
				}
				catch (Exception arg)
				{
					Console.WriteLine("{0} Exception caught.", arg);
				}
			}
			if (mAction == Action.DISABLE && (mSession == null || mSession.IsDisposed || mSession.BytesPerSecondSent == 0))
			{
				mAction = Action.NONE;
				((GameComponent)this).Enabled = false;
				endSession();
				if (mAfterStopDelegate != null)
				{
					mAfterStopDelegate();
				}
			}
		}
	}

	public void start(SignedInGamer hostGamer, IOnlineSyncTarget syncTarget)
	{
		lock (SYNC)
		{
			if (mAction == Action.NONE)
			{
				mAction = Action.PROCESS;
				((GameComponent)this).Enabled = true;
				mHostGamer.Clear();
				mHostGamer.Add(hostGamer);
				mHostGamertag = ((Gamer)hostGamer).Gamertag;
				mSyncTarget = syncTarget;
				mMode = Mode.UNKNOWN;
				mState = State.IDLE;
				mRecentHostsLookup.Clear();
				new Thread(run).Start();
			}
		}
	}

	public void stop(AfterStopDelegate afterStopDelegate)
	{
		if (mAction == Action.PROCESS)
		{
			mAfterStopDelegate = afterStopDelegate;
			mAction = Action.EXIT_THREAD;
			return;
		}
		mAction = Action.NONE;
		((GameComponent)this).Enabled = false;
		endSession();
		afterStopDelegate?.Invoke();
	}

	public void notifyAboutNewLocalEntry()
	{
		mHasNewLocalEntry = true;
	}

	public void run()
	{
		Thread.CurrentThread.SetProcessorAffinity(new int[1] { 5 });
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		TimeSpan elapsed = stopwatch.Elapsed;
		while (mAction == Action.PROCESS)
		{
			try
			{
				TimeSpan timeSpan = elapsed;
				elapsed = stopwatch.Elapsed;
				double totalMilliseconds = (elapsed - timeSpan).TotalMilliseconds;
				if (mHasNewLocalEntry)
				{
					mHasNewLocalEntry = false;
					mServerMillisToLive = 0.0;
					mRecentHosts.Clear();
					mRecentHostsLookup.Clear();
				}
				else
				{
					for (int num = mRecentHosts.Count - 1; num >= 0; num--)
					{
						HostInfo hostInfo = mRecentHosts[num];
						hostInfo.mWaitTime -= totalMilliseconds;
						if (hostInfo.mWaitTime <= 0.0)
						{
							mRecentHosts.RemoveAt(num);
							mRecentHostsLookup.Remove(hostInfo.mHostGamertag);
						}
					}
				}
				switch (mState)
				{
				case State.IDLE:
					switch (mMode)
					{
					case Mode.UNKNOWN:
						handleIdleUnknown();
						break;
					case Mode.SERVER:
						handleIdleServer(totalMilliseconds);
						break;
					case Mode.CLIENT:
						handleIdleClient();
						break;
					}
					break;
				case State.SEND:
					if (mSession != null)
					{
						sendNextEntry();
					}
					break;
				case State.RECEIVE:
					if (mSession != null)
					{
						receiveNextEntry();
					}
					break;
				case State.WAIT_FOR_END:
					if (mSession != null)
					{
						Thread.Sleep(12);
					}
					break;
				}
			}
			catch (Exception arg)
			{
				Console.WriteLine("{0} Exception caught.", arg);
				Thread.Sleep(12);
			}
		}
		if (mAction == Action.EXIT_THREAD)
		{
			mAction = Action.DISABLE;
		}
	}

	private bool findServer()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		NetworkSessionProperties val = new NetworkSessionProperties();
		val[0] = mVersion;
		AvailableNetworkSessionCollection val2;
		try
		{
			val2 = NetworkSession.Find((NetworkSessionType)2, (IEnumerable<SignedInGamer>)mHostGamer, val);
		}
		catch (Exception)
		{
			return false;
		}
		foreach (AvailableNetworkSession item in (ReadOnlyCollection<AvailableNetworkSession>)(object)val2)
		{
			if (!mRecentHostsLookup.ContainsKey(item.HostGamertag))
			{
				NetworkSession val3;
				try
				{
					val3 = NetworkSession.Join(item);
				}
				catch (Exception)
				{
					return false;
				}
				val3.SessionEnded += clientSide_serverHasDisconnected;
				((NetworkGamer)((ReadOnlyCollection<LocalNetworkGamer>)(object)val3.LocalGamers)[0]).IsReady = true;
				mMode = Mode.CLIENT;
				mSession = val3;
				mLocalGamerForSendingAndReceiving = null;
				mRemoteGamerForSending = null;
				disableVoice();
				return true;
			}
		}
		return false;
	}

	private void startServerSession()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		NetworkSessionProperties val = new NetworkSessionProperties();
		val[0] = mVersion;
		NetworkSession val2;
		try
		{
			val2 = NetworkSession.Create((NetworkSessionType)2, (IEnumerable<SignedInGamer>)mHostGamer, 2, 0, val);
		}
		catch (Exception)
		{
			return;
		}
		val2.AllowHostMigration = false;
		val2.AllowJoinInProgress = false;
		val2.GamerLeft += serverSide_clientHasDisconnected;
		((NetworkGamer)((ReadOnlyCollection<LocalNetworkGamer>)(object)val2.LocalGamers)[0]).IsReady = true;
		mServerMillisToLive = mRandom.Next(240000, 420000);
		mMode = Mode.SERVER;
		mSession = val2;
		mLocalGamerForSendingAndReceiving = null;
		mRemoteGamerForSending = null;
	}

	private void handleIdleUnknown()
	{
		if (!findServer())
		{
			startServerSession();
		}
	}

	private void handleIdleServer(double elapsedMillis)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Expected I4, but got Unknown
		if (mSession == null)
		{
			return;
		}
		NetworkSessionState sessionState = mSession.SessionState;
		switch ((int)sessionState)
		{
		case 0:
			mServerMillisToLive -= elapsedMillis;
			if (mServerMillisToLive > 0.0)
			{
				if (((ReadOnlyCollection<NetworkGamer>)(object)mSession.RemoteGamers).Count == 1 && mSession.IsEveryoneReady)
				{
					mSyncTarget.startSynchronization();
					mState = State.RECEIVE;
					disableVoice();
					mSession.StartGame();
				}
				else
				{
					Thread.Sleep(12);
				}
			}
			else
			{
				endSession();
			}
			break;
		case 2:
			endSession();
			break;
		case 1:
			break;
		}
	}

	private void handleIdleClient()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Expected I4, but got Unknown
		if (mSession != null)
		{
			NetworkSessionState sessionState = mSession.SessionState;
			switch ((int)sessionState)
			{
			case 0:
				Thread.Sleep(12);
				break;
			case 1:
				mSyncTarget.startSynchronization();
				mSyncTarget.prepareForSending();
				mState = State.SEND;
				break;
			case 2:
				endSession();
				break;
			}
		}
	}

	private void sendNextEntry()
	{
		if (mSession.BytesPerSecondSent == 0)
		{
			if (mLocalGamerForSendingAndReceiving == null)
			{
				GamerCollection<LocalNetworkGamer> localGamers = mSession.LocalGamers;
				if (((ReadOnlyCollection<LocalNetworkGamer>)(object)localGamers).Count > 0)
				{
					mLocalGamerForSendingAndReceiving = ((ReadOnlyCollection<LocalNetworkGamer>)(object)localGamers)[0];
				}
				return;
			}
			if (mRemoteGamerForSending == null)
			{
				GamerCollection<NetworkGamer> remoteGamers = mSession.RemoteGamers;
				if (((ReadOnlyCollection<NetworkGamer>)(object)remoteGamers).Count > 0)
				{
					mRemoteGamerForSending = ((ReadOnlyCollection<NetworkGamer>)(object)remoteGamers)[0];
					disableVoice();
				}
				return;
			}
			bool flag = mSyncTarget.writeTransferRecord(mWriter);
			mLocalGamerForSendingAndReceiving.SendData(mWriter, (SendDataOptions)3, mRemoteGamerForSending);
			Thread.Sleep(5);
			if (flag)
			{
				if (mMode == Mode.SERVER)
				{
					mState = State.WAIT_FOR_END;
				}
				else
				{
					mState = State.RECEIVE;
				}
			}
		}
		Thread.Sleep(12);
	}

	private void receiveNextEntry()
	{
		if (mLocalGamerForSendingAndReceiving == null)
		{
			GamerCollection<LocalNetworkGamer> localGamers = mSession.LocalGamers;
			if (((ReadOnlyCollection<LocalNetworkGamer>)(object)localGamers).Count > 0)
			{
				mLocalGamerForSendingAndReceiving = ((ReadOnlyCollection<LocalNetworkGamer>)(object)localGamers)[0];
			}
			return;
		}
		if (mLocalGamerForSendingAndReceiving.IsDataAvailable)
		{
			NetworkGamer val = default(NetworkGamer);
			mLocalGamerForSendingAndReceiving.ReceiveData(mReader, ref val);
			if (mSyncTarget.readTransferRecord(mReader))
			{
				mRecentHosts.Add(new HostInfo(((Gamer)val).Gamertag, 1800000.0));
				mRecentHostsLookup[((Gamer)val).Gamertag] = "";
				if (mMode == Mode.SERVER)
				{
					mSyncTarget.prepareForSending();
					mState = State.SEND;
				}
				else
				{
					endSession();
				}
			}
		}
		Thread.Sleep(12);
	}

	private void endSession()
	{
		lock (SYNC)
		{
			if (mSession != null && !mSession.IsDisposed)
			{
				mSession.Dispose();
				mSyncTarget.endSynchronization();
			}
			mSession = null;
			mMode = Mode.UNKNOWN;
			mState = State.IDLE;
		}
	}

	private void disableVoice()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		GamerCollectionEnumerator<LocalNetworkGamer> enumerator = mSession.LocalGamers.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				LocalNetworkGamer current = enumerator.Current;
				GamerCollectionEnumerator<NetworkGamer> enumerator2 = mSession.RemoteGamers.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						NetworkGamer current2 = enumerator2.Current;
						current.EnableSendVoice(current2, false);
					}
				}
				finally
				{
					((IDisposable)enumerator2/*cast due to .constrained prefix*/).Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)enumerator/*cast due to .constrained prefix*/).Dispose();
		}
	}

	public void serverSide_clientHasDisconnected(object sender, GamerLeftEventArgs args)
	{
		endSession();
	}

	public void clientSide_serverHasDisconnected(object sender, NetworkSessionEndedEventArgs args)
	{
		endSession();
	}

	public bool WaitStopped()
	{
		if (mAction == Action.NONE)
		{
			return true;
		}
		return false;
	}
}
