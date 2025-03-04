using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class Title
{
	private App m_App;

	public GamePadState m_TitleGamepadState;

	public Title(App app)
	{
		m_App = app;
	}

	public void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_PlayerOnePadId = (PlayerIndex)(-1);
		Program.m_PlayerManager.DeleteAll();
		if (Program.m_PlayerManager.m_Player[0].m_Id == -1)
		{
			Program.m_PlayerManager.Create();
			Program.m_PlayerManager.m_Player[0].Reset();
		}
		Program.m_LoadSaveManager.LoadGame();
		if (Program.m_App.mSyncManager != null)
		{
			Program.m_App.mSyncManager.stop(null);
		}
	}

	public void Update()
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Invalid comparison between Unknown and I4
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Invalid comparison between Unknown and I4
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		if (m_App.Debounce((Keys)13) || m_App.Debounce((Keys)32) || (int)m_App.m_PlayerOnePadId != -1)
		{
			if ((int)m_App.m_PlayerOnePadId == -1)
			{
				m_App.m_PlayerOnePadId = (PlayerIndex)0;
			}
			m_App.m_NextState = App.STATE.MAINMENU;
			m_App.m_bFadeMusic = true;
			m_App.StartFade(up: false);
			Program.m_SoundManager.Play(2);
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			m_TitleGamepadState = GamePad.GetState((PlayerIndex)i);
			if (!((GamePadState)(ref m_TitleGamepadState)).IsButtonDown((Buttons)4096))
			{
				continue;
			}
			GamePadCapabilities capabilities = GamePad.GetCapabilities((PlayerIndex)i);
			if ((int)((GamePadCapabilities)(ref capabilities)).GamePadType == 1)
			{
				m_App.m_PlayerOnePadId = (PlayerIndex)i;
				Program.m_PlayerManager.m_Player[0].m_PadId = m_App.m_PlayerOnePadId;
				if (m_App.mScores == null)
				{
					m_App.mScores = new TopScoreListContainer(13, 1000);
				}
				if (m_App.m_SharedLevels == null)
				{
					m_App.m_SharedLevels = new LevelListContainer(1, 500);
				}
				if (m_App.m_SyncTargets == null)
				{
					m_App.m_SyncTargets = new SyncTargetContainer(m_App.mScores, m_App.m_SharedLevels);
				}
				m_App.StartOnlineScores();
			}
		}
	}

	public void Draw()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.Draw(m_App.m_TitleBackground, new Vector2(0f, 0f), Color.White);
		m_App.m_SpriteBatch.End();
		m_App.m_SpriteBatch.Begin();
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.START);
		m_App.m_GameText.Position = new Vector2(0f, 605f);
		m_App.m_GameText.mColor = new Color(1f, 1f, 1f, 1f);
		m_App.m_GameText.Center(new Rectangle(0, 0, 1280, 720), Text.Alignment.Horizonatal);
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		m_App.m_SpriteBatch.End();
		m_App.RenderLines();
	}
}
