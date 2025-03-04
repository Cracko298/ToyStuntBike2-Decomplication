using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class FrontEndEditorMenu
{
	public const float SUBMENU_Y_OFFSET = 105f;

	private App m_App;

	public App.TEXTID m_SubMenuState;

	private Vector2 m_SubMenuPosition = Vector2.Zero;

	private Vector2 m_PrevSubMenuPosition = Vector2.Zero;

	private float m_MenuMoveTime;

	public float m_ValueRepeatTime;

	public static Vector2 FRONTENDEDITORMENU_TOP = new Vector2(390f, 200f);

	public static Vector2 FRONTENDEDITORMENU_OFFSET = new Vector2(0f, 60f);

	public static Vector2 FRONTENDEDITORMENU_TOP_OFFSET = new Vector2(-80f, FRONTENDEDITORMENU_TOP.Y - 110f);

	public FrontEndEditorMenu(App app)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		m_App = app;
		m_SubMenuState = App.TEXTID.PLAY_USER_LEVELS;
	}

	public void Start()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		m_SubMenuState = App.TEXTID.PLAY_USER_LEVELS;
		m_SubMenuPosition = FRONTENDEDITORMENU_TOP + (float)(m_SubMenuState - 110) * FRONTENDEDITORMENU_OFFSET;
		m_PrevSubMenuPosition = Vector2.Zero;
	}

	public void Stop()
	{
	}

	public void Update()
	{
		//IL_02a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Invalid comparison between Unknown and I4
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Invalid comparison between Unknown and I4
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Invalid comparison between Unknown and I4
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Invalid comparison between Unknown and I4
		//IL_0286: Unknown result type (might be due to invalid IL or missing references)
		//IL_028b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0273: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.GetPrimaryPlayer() != null)
		{
			Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_App.m_PlayerOnePadId);
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)4096))
			{
				OnSelectPressed();
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)8192) && !m_App.Fading())
			{
				OnBackPressed();
				return;
			}
			GamePadThumbSticks thumbSticks = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).ThumbSticks;
			float num = ((GamePadThumbSticks)(ref thumbSticks)).Left.Y;
			if (Math.Abs(num) < 0.25f)
			{
				num = 0f;
			}
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
			GamePadThumbSticks thumbSticks2 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).ThumbSticks;
			float x = ((GamePadThumbSticks)(ref thumbSticks2)).Left.X;
			if (Math.Abs(x) < 0.25f)
			{
				x = 0f;
			}
			GamePadDPad dPad3 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
			if ((int)((GamePadDPad)(ref dPad3)).Right == 1)
			{
				x = 1f;
			}
			GamePadDPad dPad4 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
			if ((int)((GamePadDPad)(ref dPad4)).Left == 1)
			{
				x = -1f;
			}
			if (m_MenuMoveTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				if (num < -0.9f && m_SubMenuState < App.TEXTID.EDIT_MY_LEVELS)
				{
					m_SubMenuState++;
					m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 250f;
					m_SubMenuPosition = FRONTENDEDITORMENU_TOP + (float)(m_SubMenuState - 110) * FRONTENDEDITORMENU_OFFSET;
					Program.m_SoundManager.Play(1);
				}
				if (num > 0.9f && m_SubMenuState > App.TEXTID.PLAY_USER_LEVELS)
				{
					m_SubMenuState--;
					m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 250f;
					m_SubMenuPosition = FRONTENDEDITORMENU_TOP + (float)(m_SubMenuState - 110) * FRONTENDEDITORMENU_OFFSET;
					Program.m_SoundManager.Play(0);
				}
			}
			m_PrevSubMenuPosition = m_SubMenuPosition;
		}
		Program.m_PlayerManager.GetPrimaryPlayer().m_OldGamepadState = Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState;
	}

	private void OnSelectPressed()
	{
		if (!m_App.Fading())
		{
			if (m_SubMenuState == App.TEXTID.CREATE_NEW_LEVEL)
			{
				m_App.m_LevelEditor.m_bEditExisting = false;
				m_App.m_NextState = App.STATE.EDITING_LEVEL;
				m_App.StartFade(up: false);
			}
			else if (m_SubMenuState == App.TEXTID.PLAY_USER_LEVELS)
			{
				m_App.m_NextState = App.STATE.PLAY_USER_LEVELS;
				m_App.StartFade(up: false);
			}
			else if (m_SubMenuState == App.TEXTID.EDIT_MY_LEVELS)
			{
				m_App.m_NextState = App.STATE.EDIT_MY_LEVELS;
				m_App.StartFade(up: false);
			}
			Program.m_SoundManager.Play(2);
		}
	}

	private void OnBackPressed()
	{
		m_App.m_NextState = App.STATE.MAINMENU;
		m_App.StartFade(up: false);
		Program.m_SoundManager.Play(3);
	}

	public void Draw()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d4: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.Draw(m_App.m_MenuBackground, new Vector2(0f, 0f), Color.White);
		Vector2 fRONTENDEDITORMENU_TOP = FRONTENDEDITORMENU_TOP;
		m_App.m_MenuText.mColor = m_App.m_FrontEnd.TITLE_COL;
		m_App.m_MenuText.Position = fRONTENDEDITORMENU_TOP - FRONTENDEDITORMENU_TOP_OFFSET;
		m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.LEVEL_EDITOR_TITLE);
		m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.PLAY_USER_LEVELS);
		m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.PLAY_USER_LEVELS);
		m_App.m_MenuText.Position = fRONTENDEDITORMENU_TOP;
		m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		fRONTENDEDITORMENU_TOP += FRONTENDEDITORMENU_OFFSET;
		GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.CREATE_NEW_LEVEL);
		m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.CREATE_NEW_LEVEL);
		m_App.m_MenuText.Position = fRONTENDEDITORMENU_TOP;
		m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		fRONTENDEDITORMENU_TOP += FRONTENDEDITORMENU_OFFSET;
		GetMenuColour(out m_App.m_MenuText.mColor, App.TEXTID.EDIT_MY_LEVELS);
		m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.EDIT_MY_LEVELS);
		m_App.m_MenuText.Position = fRONTENDEDITORMENU_TOP;
		m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		fRONTENDEDITORMENU_TOP.X = 120f;
		fRONTENDEDITORMENU_TOP.Y = 545f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.SELECT);
		m_App.m_GameText.Position = fRONTENDEDITORMENU_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		fRONTENDEDITORMENU_TOP.Y += 60f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.BACK);
		m_App.m_GameText.Position = fRONTENDEDITORMENU_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		Program.m_App.DrawMenuPointer(m_SubMenuPosition);
		m_App.m_SpriteBatch.End();
		m_App.RenderLines();
	}

	public void GetMenuColour(out Color c, App.TEXTID highlightedId)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (highlightedId == m_SubMenuState)
		{
			c = m_App.m_FrontEnd.HIGHLIGHT_COL;
		}
		else
		{
			c = m_App.m_FrontEnd.LOWLIGHT_COL;
		}
	}
}
