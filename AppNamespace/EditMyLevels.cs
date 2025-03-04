using EasyStorage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class EditMyLevels
{
	private const int ERROR_FRAMES = 240;

	private App m_App;

	private float m_MenuMoveTime;

	private int m_Error;

	private int m_ErrorFrame;

	public string[] m_FileNames;

	public int m_LevelIndex;

	private int m_Selection;

	private int m_PageIndex;

	private int m_NumPages;

	public static Vector2 EDITMYLEVELS_TOP = new Vector2(400f, 40f);

	public EditMyLevels(App app)
	{
		m_App = app;
	}

	public void Start()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		m_ErrorFrame = 0;
		if (Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] == null || !Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId].IsSignedInToLive)
		{
			m_Error = 122;
			m_ErrorFrame = 240;
			Program.m_SoundManager.Play(41);
			return;
		}
		string text = $"{((Gamer)Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId]).Gamertag}*";
		m_FileNames = ((ISaveDevice)Program.m_App.saveDevice).GetFiles("Toy Stunt Bike 2", text);
		if (m_FileNames == null || m_FileNames.Length == 0)
		{
			m_Error = 129;
			m_ErrorFrame = 240;
			Program.m_SoundManager.Play(41);
		}
		else
		{
			m_PageIndex = 0;
			m_LevelIndex = 0;
			m_Selection = 0;
			m_NumPages = (m_FileNames.Length - 1) / 10 + 1;
		}
	}

	public void Stop()
	{
		if (m_FileNames != null && m_FileNames.Length > 0)
		{
			m_App.m_UserLevelName = m_FileNames[m_LevelIndex];
		}
	}

	public void Update()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Invalid comparison between Unknown and I4
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Invalid comparison between Unknown and I4
		if (Program.m_PlayerManager.GetPrimaryPlayer() == null)
		{
			return;
		}
		Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_App.m_PlayerOnePadId);
		m_LevelIndex = m_PageIndex * 10 + m_Selection;
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)8192) && !m_App.Fading())
		{
			m_App.m_NextState = App.STATE.FRONTEND_EDITOR_MENU;
			m_App.StartFade(up: false);
			Program.m_SoundManager.Play(3);
			return;
		}
		if (m_FileNames != null)
		{
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)4096))
			{
				m_App.m_LevelEditor.m_bEditExisting = true;
				m_App.m_NextState = App.STATE.EDITING_LEVEL;
				m_App.StartFade(up: false);
				Program.m_SoundManager.Play(2);
				return;
			}
			float num = Program.m_PlayerManager.GetPrimaryPlayer().LAY();
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
			int num2 = (int)MathHelper.Min(10f, (float)(m_FileNames.Length - m_PageIndex * 10));
			if (m_MenuMoveTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				if (num < -0.9f)
				{
					if (m_Selection < num2 - 1)
					{
						m_Selection++;
						Program.m_SoundManager.Play(4);
						m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
					}
					else if (m_PageIndex < m_NumPages - 1)
					{
						m_PageIndex++;
						Program.m_SoundManager.Play(4);
						m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
						m_Selection = 0;
					}
				}
				if (num > 0.9f)
				{
					if (m_Selection > 0)
					{
						m_Selection--;
						Program.m_SoundManager.Play(4);
						m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
					}
					else if (m_PageIndex > 0)
					{
						m_PageIndex--;
						Program.m_SoundManager.Play(4);
						m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
						m_Selection = num2 - 1;
					}
				}
			}
		}
		Program.m_PlayerManager.GetPrimaryPlayer().m_OldGamepadState = Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState;
	}

	public void Draw()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0201: Unknown result type (might be due to invalid IL or missing references)
		//IL_0261: Unknown result type (might be due to invalid IL or missing references)
		//IL_0263: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.Draw(m_App.m_MenuBackground, new Vector2(0f, 0f), Color.White);
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, m_App.GetText(App.TEXTID.EDITMYLEVELS_TITLE), EDITMYLEVELS_TOP, m_App.m_FrontEnd.TITLE_COL);
		if (m_ErrorFrame > 0)
		{
			m_ErrorFrame--;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, m_App.GetText((App.TEXTID)m_Error), new Vector2(200f, 500f), Color.Yellow);
		}
		if (m_FileNames == null)
		{
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, m_App.GetText(App.TEXTID.NO_FILES), new Vector2(400f, 150f), Color.Yellow);
		}
		else
		{
			int num = 10;
			if (m_PageIndex * 10 + 10 > m_FileNames.Length)
			{
				num = m_FileNames.Length - m_PageIndex * 10;
			}
			int num2 = 0;
			Color val = Color.DarkGoldenrod;
			for (int i = m_PageIndex * 10; i < m_PageIndex * 10 + num; i++)
			{
				if (num2 == m_Selection)
				{
					val = Color.LightGoldenrodYellow;
				}
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, m_FileNames[i], new Vector2(400f, 150f + (float)(35 * num2)), val);
				num2++;
				val = Color.DarkGoldenrod;
			}
		}
		Vector2 position = default(Vector2);
		((Vector2)(ref position))._002Ector(120f, 545f);
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.SELECT);
		m_App.m_GameText.Position = position;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		position.Y += 60f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.BACK);
		m_App.m_GameText.Position = position;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		m_App.m_SpriteBatch.End();
	}
}
