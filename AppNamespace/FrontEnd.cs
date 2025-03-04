using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class FrontEnd
{
	public enum MENUSTATE
	{
		MAINMENU,
		OPTIONS
	}

	public const float SUBMENU_Y_OFFSET = 95f;

	public const float MENU_REPEAT_TIME = 250f;

	public const float VALUE_REPEAT_TIME = 40f;

	private App m_App;

	private App.TEXTID m_SubMenuState;

	private float m_MenuMoveTime;

	private Vector2 m_SubMenuPosition = Vector2.Zero;

	private Vector2 m_PrevSubMenuPosition = Vector2.Zero;

	public bool m_bGameStarted;

	public static Vector2 TEXT_TOP = new Vector2(400f, 150f);

	public static Vector2 TEXT_OFFSET = new Vector2(0f, 60f);

	public static Vector2 TEXT_ARE_YOU_SURE = new Vector2(500f, 250f);

	public static Vector2 TITLE_TOP_OFFSET = new Vector2(-50f, TEXT_TOP.Y - 60f);

	private float[] m_aSilkWidth = new float[6] { 0f, 364f, 286f, 182f, 494f, 286f };

	public Color TITLE_COL = new Color(1f, 1f, 0.4f, 1f);

	public Color HIGHLIGHT_COL = new Color(1f, 1f, 1f, 1f);

	public Color LOWLIGHT_COL = new Color(0.3f, 0.56f, 0.74f, 1f);

	public FrontEnd(App app)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		m_App = app;
	}

	public void Start()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		if (SaveExists() || m_bGameStarted)
		{
			m_SubMenuState = App.TEXTID.CONTINUE;
		}
		else if (m_App.m_PrevState == App.STATE.OPTIONS)
		{
			m_SubMenuState = App.TEXTID.OPTIONS;
		}
		else
		{
			m_SubMenuState = App.TEXTID.NEW;
		}
		Vector2 tEXT_TOP = TEXT_TOP;
		m_SubMenuPosition = tEXT_TOP + (float)(m_SubMenuState - 1) * TEXT_OFFSET;
		m_PrevSubMenuPosition = Vector2.Zero;
		Program.m_App.m_Fog = new Vector3(0.6627451f, 62f / 85f, 47f / 51f);
		Program.m_App.m_MenuIdleTime = 0f;
		Program.m_App.m_bSplitScreen = false;
	}

	public void Stop()
	{
	}

	private bool SaveExists()
	{
		return Program.m_App.m_bSaveExists;
	}

	public void Update()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.GetPrimaryPlayer() != null)
		{
			Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_App.m_PlayerOnePadId);
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer() != null)
		{
			UpdateMainMenu();
		}
		if (Program.m_App.m_bWasBought)
		{
			if (m_bGameStarted)
			{
				m_SubMenuState = App.TEXTID.CONTINUE;
			}
			else
			{
				m_SubMenuState = App.TEXTID.NEW;
			}
			m_SubMenuPosition = TEXT_TOP + (float)(m_SubMenuState - 1) * TEXT_OFFSET;
			m_PrevSubMenuPosition = Vector2.Zero;
			Program.m_App.m_bWasBought = false;
		}
		Program.m_App.m_PrevTrialMode = Guide.IsTrialMode;
	}

	private void UpdateMainMenu()
	{
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Invalid comparison between Unknown and I4
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Invalid comparison between Unknown and I4
		//IL_0169: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		Program.m_ParticleManager.Update();
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)4096))
		{
			Program.m_App.m_MenuIdleTime = 0f;
			OnSelectPressed();
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)8192))
		{
			Program.m_App.m_MenuIdleTime = 0f;
			OnBackPressed();
		}
		if (m_SubMenuState != App.TEXTID.ARE_YOU_SURE && m_SubMenuState != App.TEXTID.NEWGAME_ARE_YOU_SURE)
		{
			GamePadThumbSticks thumbSticks = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).ThumbSticks;
			float num = ((GamePadThumbSticks)(ref thumbSticks)).Left.Y;
			GamePadDPad dPad = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
			if ((int)((GamePadDPad)(ref dPad)).Up == 1)
			{
				num = 1f;
			}
			GamePadDPad dPad2 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
			if ((int)((GamePadDPad)(ref dPad2)).Down == 1)
			{
				num = -1f;
			}
			if (num != 0f)
			{
				Program.m_App.m_MenuIdleTime = 0f;
			}
			if (m_MenuMoveTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				if (num < -0.9f && m_SubMenuState < App.TEXTID.EXIT_GAME)
				{
					m_SubMenuState++;
					m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 250f;
					m_SubMenuPosition = GetSubMenuPosition(dir: true);
					Program.m_SoundManager.Play(1);
				}
				if (num > 0.9f)
				{
					int num2 = 0;
					if (!SaveExists() && !m_bGameStarted)
					{
						num2 = 1;
					}
					if ((int)m_SubMenuState > 1 + num2)
					{
						m_SubMenuState--;
						m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 250f;
						m_SubMenuPosition = GetSubMenuPosition(dir: false);
						Program.m_SoundManager.Play(0);
					}
				}
			}
		}
		m_PrevSubMenuPosition = m_SubMenuPosition;
		Program.m_PlayerManager.GetPrimaryPlayer().m_OldGamepadState = Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState;
	}

	private Vector2 GetSubMenuPosition(bool dir)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		Vector2 tEXT_TOP = TEXT_TOP;
		if (!Guide.IsTrialMode && m_SubMenuState == App.TEXTID.BUY)
		{
			if (dir)
			{
				m_SubMenuState++;
			}
			else
			{
				m_SubMenuState--;
			}
			if (m_SubMenuState < App.TEXTID.BUY)
			{
				return tEXT_TOP + (float)(m_SubMenuState - 1) * TEXT_OFFSET;
			}
			return tEXT_TOP + (float)(m_SubMenuState - 2) * TEXT_OFFSET;
		}
		return tEXT_TOP + (float)(m_SubMenuState - 1) * TEXT_OFFSET;
	}

	private void NewGame()
	{
		m_App.m_NextState = App.STATE.MAP;
		m_App.StartFade(up: false);
		m_App.m_Level = 1;
		m_App.m_LevelReached = 1;
		m_App.m_Map.m_LastLevelReached = 1;
		for (int i = 0; i < 14; i++)
		{
			Program.m_App.m_LevelsVisited[i] = 0;
		}
		Program.m_ItemManager.ClearFlagsCollected();
		Program.m_ItemManager.ClearCupsCollected();
		m_App.m_Tip = App.TEXTID.TOPSCORE;
	}

	private void OnSelectPressed()
	{
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		if (m_App.Fading())
		{
			return;
		}
		Program.m_SoundManager.Play(2);
		switch (m_SubMenuState)
		{
		case App.TEXTID.CONTINUE:
			m_App.m_NextState = App.STATE.MAP;
			m_App.StartFade(up: false);
			break;
		case App.TEXTID.NEW:
			if (SaveExists())
			{
				m_SubMenuState = App.TEXTID.NEWGAME_ARE_YOU_SURE;
			}
			else
			{
				NewGame();
			}
			break;
		case App.TEXTID.SPLITSCREEN:
			m_App.m_bSplitScreen = true;
			m_App.m_NextState = App.STATE.MAP;
			m_App.StartFade(up: false);
			break;
		case App.TEXTID.OPTIONS:
			m_App.m_NextState = App.STATE.OPTIONS;
			m_App.StartFade(up: false);
			m_App.m_Options.m_SubMenuState = App.TEXTID.MUSIC_VOL;
			break;
		case App.TEXTID.TRACKTIMES:
			if (!Guide.IsTrialMode)
			{
				m_App.m_NextState = App.STATE.FRONTENDSCORES;
				m_App.StartFade(up: false);
			}
			else
			{
				m_App.m_NotInTrialTime = (float)m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 7000f;
			}
			break;
		case App.TEXTID.LEVEL_EDITOR:
			if (!Guide.IsTrialMode)
			{
				m_App.m_NextState = App.STATE.FRONTEND_EDITOR_MENU;
				m_App.StartFade(up: false);
			}
			else
			{
				m_App.m_NotInTrialTime = (float)m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 7000f;
			}
			break;
		case App.TEXTID.BUY:
			if (!Guide.IsVisible)
			{
				if (m_App.CanPurchaseContent(m_App.m_PlayerOnePadId))
				{
					Guide.ShowMarketplace(m_App.m_PlayerOnePadId);
				}
				else if (Gamer.SignedInGamers[m_App.m_PlayerOnePadId] != null)
				{
					Program.m_App.m_PermissionTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 7000f;
				}
				else
				{
					Guide.ShowSignIn(1, true);
				}
			}
			break;
		case App.TEXTID.EXIT_GAME:
			m_SubMenuState = App.TEXTID.ARE_YOU_SURE;
			break;
		case App.TEXTID.ARE_YOU_SURE:
			m_App.m_NextState = App.STATE.EXITING;
			if (!Guide.IsTrialMode)
			{
				m_App.m_bFadeMusic = true;
			}
			m_App.StartFade(up: false);
			break;
		case App.TEXTID.NEWGAME_ARE_YOU_SURE:
			NewGame();
			break;
		}
	}

	private void OnBackPressed()
	{
		if (!m_App.Fading())
		{
			Program.m_SoundManager.Play(3);
			if (m_SubMenuState == App.TEXTID.NEWGAME_ARE_YOU_SURE)
			{
				m_SubMenuState = App.TEXTID.NEW;
			}
			else if (m_SubMenuState != App.TEXTID.ARE_YOU_SURE)
			{
				m_App.m_NextState = App.STATE.TITLE;
				m_App.m_bFadeMusic = true;
				m_App.StartFade(up: false);
			}
			else
			{
				m_SubMenuState = App.TEXTID.EXIT_GAME;
			}
		}
	}

	private void GetMenuColour(out Color c, App.TEXTID highlightedId)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (highlightedId == m_SubMenuState)
		{
			c = HIGHLIGHT_COL;
		}
		else
		{
			c = LOWLIGHT_COL;
		}
		if (highlightedId == App.TEXTID.CONTINUE && !SaveExists() && !m_bGameStarted)
		{
			c = new Color(0.2f, 0.175f, 0.15f, 0.25f);
		}
	}

	public void Draw()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_047d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0494: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_010f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_0232: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_027b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0317: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		//IL_035b: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_051a: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0442: Unknown result type (might be due to invalid IL or missing references)
		//IL_0443: Unknown result type (might be due to invalid IL or missing references)
		//IL_046e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0388: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0393: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0653: Unknown result type (might be due to invalid IL or missing references)
		//IL_0654: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0600: Unknown result type (might be due to invalid IL or missing references)
		//IL_0601: Unknown result type (might be due to invalid IL or missing references)
		//IL_072c: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0789: Unknown result type (might be due to invalid IL or missing references)
		//IL_078a: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.Draw(m_App.m_MenuBackground, new Vector2(0f, 0f), Color.White);
		m_App.m_SpriteBatch.End();
		m_App.m_SpriteBatch.Begin();
		Vector2 val = TEXT_TOP;
		if (m_SubMenuState != App.TEXTID.ARE_YOU_SURE && m_SubMenuState != App.TEXTID.NEWGAME_ARE_YOU_SURE)
		{
			m_App.m_MenuText.mColor = TITLE_COL;
			m_App.m_MenuText.Position = val - TITLE_TOP_OFFSET;
			m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.MAINMENU_TITLE);
			m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
			GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.CONTINUE);
			m_App.m_MenuText.Position = val;
			m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.CONTINUE);
			m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
			val += TEXT_OFFSET;
			GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.NEW);
			m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.NEW);
			m_App.m_MenuText.Position = val;
			m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
			val += TEXT_OFFSET;
			GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.SPLITSCREEN);
			m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.SPLITSCREEN);
			m_App.m_MenuText.Position = val;
			m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
			val += TEXT_OFFSET;
			GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.LEVEL_EDITOR);
			m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.LEVEL_EDITOR);
			m_App.m_MenuText.Position = val;
			m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
			val += TEXT_OFFSET;
			GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.TRACKTIMES);
			m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.TRACKTIMES);
			m_App.m_MenuText.Position = val;
			m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
			val += TEXT_OFFSET;
			GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.OPTIONS);
			m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.OPTIONS);
			m_App.m_MenuText.Position = val;
			m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
			if (Guide.IsTrialMode)
			{
				val += TEXT_OFFSET;
				GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.BUY);
				m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.BUY);
				m_App.m_MenuText.Position = val;
				m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
			}
			val += TEXT_OFFSET;
			GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.EXIT_GAME);
			m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.EXIT_GAME);
			m_App.m_MenuText.Position = val;
			m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
			Program.m_App.DrawMenuPointer(m_SubMenuPosition);
		}
		else
		{
			val = TEXT_ARE_YOU_SURE;
			m_App.m_MenuText.mColor = HIGHLIGHT_COL;
			if (m_SubMenuState == App.TEXTID.NEWGAME_ARE_YOU_SURE)
			{
				val.X -= 100f;
				m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.NEWGAME_ARE_YOU_SURE);
			}
			else
			{
				m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.ARE_YOU_SURE);
			}
			m_App.m_MenuText.Position = val;
			m_App.m_MenuText.Center(new Rectangle(0, 0, 1280, 720), Text.Alignment.Horizonatal);
			m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		}
		val.X = 120f;
		val.Y = 545f;
		if (m_SubMenuState != App.TEXTID.ARE_YOU_SURE && m_SubMenuState != App.TEXTID.NEWGAME_ARE_YOU_SURE)
		{
			m_App.m_GameText.mText = m_App.GetText(App.TEXTID.SELECT);
			m_App.m_GameText.Position = val;
			m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
			val.Y += 60f;
			m_App.m_GameText.mText = m_App.GetText(App.TEXTID.BACK);
			m_App.m_GameText.Position = val;
			m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		}
		else
		{
			m_App.m_GameText.mText = m_App.GetText(App.TEXTID.OK);
			m_App.m_GameText.Position = val;
			m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
			val.Y += 60f;
			m_App.m_GameText.mText = m_App.GetText(App.TEXTID.CANCEL);
			m_App.m_GameText.Position = val;
			m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		}
		if (m_App.m_NotInTrialTime > (float)m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
		{
			((Vector2)(ref val))._002Ector(150f, 105f);
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "               Not available in Trial Mode.", val, Color.Yellow);
		}
		if (m_App.m_PermissionTime > (float)m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
		{
			((Vector2)(ref val))._002Ector(220f, 130f);
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "No permissions to BUY on this Profile", val, Color.Yellow);
		}
		m_App.m_SpriteBatch.End();
		m_App.RenderLines();
	}
}
