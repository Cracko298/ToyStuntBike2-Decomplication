using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class PlayUserLevels
{
	public const float SUBMENU_Y_OFFSET = 105f;

	private App m_App;

	public App.TEXTID m_SubMenuState;

	private Vector2 m_SubMenuPosition = Vector2.Zero;

	private Vector2 m_PrevSubMenuPosition = Vector2.Zero;

	public float m_ValueRepeatTime;

	private int m_NumPages;

	private float m_MenuMoveTime;

	public LevelEntry[] m_Page = new LevelEntry[10];

	private int pageIndex;

	public int m_Selection;

	public static Vector2 PLAYUSERLEVELS_TOP = new Vector2(400f, 40f);

	public PlayUserLevels(App app)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		m_App = app;
	}

	public void Start()
	{
		Program.m_App.m_SharedLevels.fillPageFromFullList(0, pageIndex, m_Page);
		m_NumPages = (Program.m_App.m_SharedLevels.getFullListSize(0) - 1) / 10;
		Program.m_App.m_MenuIdleTime = 0f;
	}

	public void Stop()
	{
	}

	public void Update()
	{
		//IL_0419: Unknown result type (might be due to invalid IL or missing references)
		//IL_041e: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Invalid comparison between Unknown and I4
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e7: Invalid comparison between Unknown and I4
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.GetPrimaryPlayer() != null)
		{
			Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_App.m_PlayerOnePadId);
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)16384))
			{
				pageIndex = 0;
				Program.m_App.m_SharedLevels.fillPageFromFullList(0, pageIndex, m_Page);
				Program.m_SoundManager.Play(2);
				Program.m_App.m_MenuIdleTime = 0f;
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)8192))
			{
				Program.m_App.m_MenuIdleTime = 0f;
				OnBackPressed();
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)4096))
			{
				if (m_Page[m_Selection] != null)
				{
					Program.m_App.m_bPlayUserLevel = true;
					m_App.m_NextState = App.STATE.CONTINUEGAME;
					m_App.StartFade(up: false);
					Program.m_SoundManager.Play(2);
					Program.m_PlayerManager.GetPrimaryPlayer().ResetBike(bForce: true, restartRace: true, Vector2.Zero);
					Program.m_PlayerManager.GetPrimaryPlayer().m_ReadySteadyTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 4500f;
					Program.m_App.m_PlayingUserLevelName = m_Page[m_Selection].m_LevelName;
				}
				return;
			}
			int num = 0;
			LevelEntry[] page = m_Page;
			foreach (LevelEntry levelEntry in page)
			{
				if (levelEntry != null)
				{
					num++;
				}
			}
			float num2 = Program.m_PlayerManager.GetPrimaryPlayer().LAY();
			GamePadDPad dPad = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
			if ((int)((GamePadDPad)(ref dPad)).Up == 1)
			{
				num2 = 1f;
			}
			GamePadDPad dPad2 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
			if ((int)((GamePadDPad)(ref dPad2)).Down == 1)
			{
				num2 = -1f;
			}
			GamePadTriggers triggers = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).Triggers;
			float right = ((GamePadTriggers)(ref triggers)).Right;
			if (m_MenuMoveTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				if (num2 < -0.9f)
				{
					if (m_Selection < num - 1 && !(right > 0.8f))
					{
						m_Selection++;
						m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
						Program.m_SoundManager.Play(4);
						Program.m_App.m_MenuIdleTime = 0f;
					}
					else if (pageIndex < m_NumPages)
					{
						pageIndex++;
						Program.m_App.m_SharedLevels.fillPageFromFullList(0, pageIndex, m_Page);
						m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
						Program.m_SoundManager.Play(4);
						Program.m_App.m_MenuIdleTime = 0f;
						m_Selection = 0;
					}
				}
				if (num2 > 0.9f)
				{
					if (m_Selection > 0 && !(right > 0.8f))
					{
						m_Selection--;
						m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
						Program.m_SoundManager.Play(4);
						Program.m_App.m_MenuIdleTime = 0f;
					}
					else if (pageIndex > 0)
					{
						pageIndex--;
						Program.m_App.m_SharedLevels.fillPageFromFullList(0, pageIndex, m_Page);
						m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
						Program.m_SoundManager.Play(4);
						Program.m_App.m_MenuIdleTime = 0f;
						m_Selection = 9;
					}
				}
			}
		}
		Program.m_PlayerManager.GetPrimaryPlayer().m_OldGamepadState = Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState;
	}

	private void OnBackPressed()
	{
		m_App.m_NextState = App.STATE.FRONTEND_EDITOR_MENU;
		m_App.StartFade(up: false);
		Program.m_SoundManager.Play(3);
	}

	public void Draw()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0187: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0238: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0572: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_05d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0634: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_0702: Unknown result type (might be due to invalid IL or missing references)
		//IL_0756: Unknown result type (might be due to invalid IL or missing references)
		//IL_0761: Unknown result type (might be due to invalid IL or missing references)
		//IL_0766: Unknown result type (might be due to invalid IL or missing references)
		//IL_076b: Unknown result type (might be due to invalid IL or missing references)
		//IL_07ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0337: Unknown result type (might be due to invalid IL or missing references)
		//IL_033c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0500: Unknown result type (might be due to invalid IL or missing references)
		//IL_0501: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_044b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0450: Unknown result type (might be due to invalid IL or missing references)
		//IL_0477: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.Draw(m_App.m_MenuBackground, new Vector2(0f, 0f), Color.White);
		m_App.m_SpriteBatch.End();
		Vector2 pLAYUSERLEVELS_TOP = PLAYUSERLEVELS_TOP;
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, m_App.GetText(App.TEXTID.PLAYUSERLEVELS_TITLE), pLAYUSERLEVELS_TOP, m_App.m_FrontEnd.TITLE_COL);
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, m_App.GetText(App.TEXTID.PAGE), new Vector2(120f, 300f), m_App.m_FrontEnd.TITLE_COL);
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, $"{pageIndex + 1}/{m_NumPages + 1}", new Vector2(120f, 350f), m_App.m_FrontEnd.TITLE_COL);
		pLAYUSERLEVELS_TOP.X -= 130f;
		pLAYUSERLEVELS_TOP.Y += 70f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Rank", pLAYUSERLEVELS_TOP, m_App.m_FrontEnd.TITLE_COL);
		pLAYUSERLEVELS_TOP.X += 120f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Name", pLAYUSERLEVELS_TOP, m_App.m_FrontEnd.TITLE_COL);
		pLAYUSERLEVELS_TOP.X += 550f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Rating", pLAYUSERLEVELS_TOP, m_App.m_FrontEnd.TITLE_COL);
		pLAYUSERLEVELS_TOP.Y += 35f;
		Color darkGoldenrod = Color.DarkGoldenrod;
		int num = 0;
		LevelEntry[] page = m_Page;
		Rectangle value = default(Rectangle);
		Rectangle val = default(Rectangle);
		foreach (LevelEntry levelEntry in page)
		{
			darkGoldenrod = Color.DarkGoldenrod;
			if (levelEntry == null)
			{
				continue;
			}
			if (m_Selection == num)
			{
				darkGoldenrod = Color.LightGoldenrodYellow;
			}
			pLAYUSERLEVELS_TOP.X = PLAYUSERLEVELS_TOP.X - 110f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{levelEntry.RankAtLastPageFill}. ", pLAYUSERLEVELS_TOP, darkGoldenrod);
			pLAYUSERLEVELS_TOP.X += 100f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{levelEntry.m_LevelName}", pLAYUSERLEVELS_TOP, darkGoldenrod);
			pLAYUSERLEVELS_TOP.X += 550f;
			m_App.m_SpriteBatch.Draw(m_App.m_RatingEmptyTexture, new Vector2(pLAYUSERLEVELS_TOP.X, pLAYUSERLEVELS_TOP.Y + 10f), Color.White);
			float num2 = levelEntry.m_Rating / (float)levelEntry.m_RatingCount;
			float num3 = num2 / 5f;
			int num4 = (int)(num3 * (float)Program.m_App.m_RatingTexture.Width);
			((Rectangle)(ref value))._002Ector(0, 0, num4, Program.m_App.m_RatingTexture.Height);
			((Rectangle)(ref val))._002Ector((int)pLAYUSERLEVELS_TOP.X, (int)(pLAYUSERLEVELS_TOP.Y + 10f), num4, Program.m_App.m_RatingTexture.Height);
			m_App.m_SpriteBatch.Draw(Program.m_App.m_RatingTexture, val, (Rectangle?)value, Color.White);
			pLAYUSERLEVELS_TOP.X += 95f;
			for (int j = 0; j < m_App.m_Ratings.m_Rating.Count; j++)
			{
				if (m_App.m_Ratings.m_Rating[j].m_LevelName == levelEntry.m_LevelName)
				{
					m_App.m_SpriteBatch.Draw(m_App.m_MenuPointerTexture, pLAYUSERLEVELS_TOP + new Vector2(0f, 10f), (Rectangle?)new Rectangle(0, 0, m_App.m_MenuPointerTexture.Width, m_App.m_MenuPointerTexture.Height), Color.White, 0f, Vector2.Zero, 0.3f, (SpriteEffects)0, 1f);
				}
			}
			pLAYUSERLEVELS_TOP.X += 20f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{levelEntry.m_RatingCount}", pLAYUSERLEVELS_TOP, darkGoldenrod);
			pLAYUSERLEVELS_TOP.Y += 35f;
			num++;
		}
		pLAYUSERLEVELS_TOP.X = 120f;
		pLAYUSERLEVELS_TOP.Y = 485f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.PLAY_LEVEL);
		m_App.m_GameText.Position = pLAYUSERLEVELS_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		pLAYUSERLEVELS_TOP.Y += 60f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.TOPSCORE);
		m_App.m_GameText.Position = pLAYUSERLEVELS_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		pLAYUSERLEVELS_TOP.Y += 60f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.BACK);
		m_App.m_GameText.Position = pLAYUSERLEVELS_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		pLAYUSERLEVELS_TOP.X += 300f;
		pLAYUSERLEVELS_TOP.Y = 590f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.LSTICK);
		m_App.m_GameText.Position = pLAYUSERLEVELS_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.4f);
		pLAYUSERLEVELS_TOP.X += 90f;
		pLAYUSERLEVELS_TOP.Y = 570f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Move up/down with left stick", pLAYUSERLEVELS_TOP, Color.White);
		pLAYUSERLEVELS_TOP.X -= 110f;
		pLAYUSERLEVELS_TOP.Y = 640f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.RTRIGGER);
		m_App.m_GameText.Position = pLAYUSERLEVELS_TOP + new Vector2(0f, 0f);
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.4f);
		pLAYUSERLEVELS_TOP.X += 70f;
		pLAYUSERLEVELS_TOP.Y = 615f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Hold right trigger to page faster", pLAYUSERLEVELS_TOP, Color.White);
		m_App.m_SpriteBatch.End();
		m_App.RenderLines();
	}
}
