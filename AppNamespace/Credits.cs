using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class Credits
{
	public const float SUBMENU_Y_OFFSET = 105f;

	private App m_App;

	public App.TEXTID m_SubMenuState;

	private Vector2 m_SubMenuPosition = Vector2.Zero;

	private Vector2 m_PrevSubMenuPosition = Vector2.Zero;

	public float m_ValueRepeatTime;

	public static Vector2 CREDITS_TOP = new Vector2(350f, 110f);

	public static Vector2 CREDITS_OFFSET = new Vector2(0f, 60f);

	public static Vector2 CREDITS_TOP_OFFSET = new Vector2(-120f, CREDITS_TOP.Y - 30f);

	public Credits(App app)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		m_App = app;
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
		m_SubMenuState = App.TEXTID.MUSIC_VOL;
		m_SubMenuPosition = CREDITS_TOP + (float)(m_SubMenuState - 14) * CREDITS_OFFSET;
		m_PrevSubMenuPosition = Vector2.Zero;
		Program.m_App.m_MenuIdleTime = 0f;
	}

	public void Stop()
	{
	}

	public void Update()
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.GetPrimaryPlayer() != null)
		{
			Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_App.m_PlayerOnePadId);
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)8192))
			{
				Program.m_App.m_MenuIdleTime = 0f;
				OnBackPressed();
			}
		}
		Program.m_PlayerManager.GetPrimaryPlayer().m_OldGamepadState = Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState;
	}

	private void OnSelectPressed()
	{
		m_App.m_NextState = App.STATE.OPTIONS;
		m_App.StartFade(up: false);
		Program.m_SoundManager.Play(2);
	}

	private void OnBackPressed()
	{
		m_App.m_NextState = App.STATE.OPTIONS;
		m_App.StartFade(up: false);
		Program.m_SoundManager.Play(3);
	}

	public void Draw()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.Draw(m_App.m_MenuBackground, new Vector2(0f, 0f), Color.White);
		m_App.m_SpriteBatch.End();
		Vector2 cREDITS_TOP = CREDITS_TOP;
		m_App.m_SpriteBatch.Begin();
		m_App.m_MenuText.mColor = m_App.m_FrontEnd.TITLE_COL;
		m_App.m_MenuText.Position = cREDITS_TOP - CREDITS_TOP_OFFSET;
		m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.CREDITS_TITLE);
		m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		cREDITS_TOP.X -= 220f;
		Color val = Color.White;
		for (int i = 0; i < 15; i++)
		{
			if (i > 1)
			{
				val = Color.LightBlue;
			}
			if (i > 4)
			{
				val = Color.LightGray;
			}
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, m_App.GetText((App.TEXTID)(48 + i)), cREDITS_TOP, val);
			cREDITS_TOP.Y += 35f;
		}
		cREDITS_TOP.X = 120f;
		cREDITS_TOP.Y = 545f;
		cREDITS_TOP.Y += 60f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.BACK);
		m_App.m_GameText.Position = cREDITS_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		m_App.m_SpriteBatch.End();
		m_App.RenderLines();
	}
}
