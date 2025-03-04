using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class LevelEndUser
{
	public static Vector2 LEVELENDUSER_TOP = new Vector2(400f, 40f);

	private App m_App;

	private float m_MenuMoveTime;

	private float m_CurrRating;

	private int m_CurrRatingCount;

	private int m_CurrBackground;

	private int m_MyRating;

	private bool m_bAlreadyRated;

	public LevelEndUser(App app)
	{
		m_App = app;
	}

	public void Start()
	{
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0146: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		m_CurrRating = m_App.m_PlayUserLevels.m_Page[m_App.m_PlayUserLevels.m_Selection].m_Rating;
		m_CurrRatingCount = m_App.m_PlayUserLevels.m_Page[m_App.m_PlayUserLevels.m_Selection].m_RatingCount;
		m_CurrBackground = m_App.m_PlayUserLevels.m_Page[m_App.m_PlayUserLevels.m_Selection].m_Background;
		if (m_CurrRatingCount == 0)
		{
			m_CurrRatingCount = 1;
		}
		m_MyRating = 3;
		m_bAlreadyRated = false;
		for (int i = 0; i < m_App.m_Ratings.m_Rating.Count; i++)
		{
			if (m_App.m_PlayingUserLevelName == m_App.m_Ratings.m_Rating[i].m_LevelName)
			{
				m_MyRating = (int)m_App.m_Ratings.m_Rating[i].m_Rating;
				m_bAlreadyRated = true;
			}
		}
		if (Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] == null)
		{
			m_bAlreadyRated = true;
		}
		if (!m_bAlreadyRated && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] != null)
		{
			string gamertag = ((Gamer)Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId]).Gamertag;
			if (string.Compare(gamertag, 0, m_App.m_PlayingUserLevelName, 0, gamertag.Length) == 0)
			{
				m_bAlreadyRated = true;
			}
		}
		Program.m_LoadSaveManager.SaveGame();
	}

	public void Stop()
	{
	}

	public void Update()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_02af: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.GetPrimaryPlayer() == null)
		{
			return;
		}
		Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_App.m_PlayerOnePadId);
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)4096))
		{
			if (!m_bAlreadyRated)
			{
				int version = 0;
				int background = 0;
				float rating = 0f;
				int ratingCount = 0;
				if (Program.m_App.m_SharedLevels.containsEntryForGamertag(0, Program.m_App.m_PlayingUserLevelName, out version, out background, out rating, out ratingCount))
				{
					ratingCount++;
					rating += (float)m_MyRating;
					LevelEntry entry = new LevelEntry(Program.m_App.m_PlayingUserLevelName, version, background, rating, ratingCount);
					Program.m_App.m_SharedLevels.addEntry(0, entry, Program.m_App.mSyncManager);
				}
				m_App.m_Ratings.Add(m_MyRating);
			}
			m_App.m_NextState = App.STATE.PLAY_USER_LEVELS;
			m_App.StartFade(up: false);
			Program.m_SoundManager.Play(2);
			Program.m_LoadSaveManager.SaveGame();
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)32768))
		{
			m_App.m_bPlayUserLevel = true;
			m_App.m_NextState = App.STATE.CONTINUEGAME;
			m_App.StartFade(up: false);
			Program.m_SoundManager.Play(2);
			Program.m_PlayerManager.GetPrimaryPlayer().ResetBike(bForce: true, restartRace: true, Vector2.Zero);
			Program.m_PlayerManager.GetPrimaryPlayer().m_ReadySteadyTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 4500f;
			return;
		}
		Program.m_PlayerManager.GetPrimaryPlayer().CheckJukeboxControls();
		if (!m_bAlreadyRated && m_MenuMoveTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
		{
			if (Program.m_PlayerManager.GetPrimaryPlayer().LAX() > 0.8f && m_MyRating < 5)
			{
				m_MyRating++;
				m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
				Program.m_SoundManager.Play(0);
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().LAX() < -0.8f && m_MyRating > 0)
			{
				m_MyRating--;
				m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
				Program.m_SoundManager.Play(1);
			}
		}
		Program.m_App.CheckInGamePurchase();
		Program.m_PlayerManager.GetPrimaryPlayer().m_OldGamepadState = Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState;
	}

	public void Draw()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0138: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0305: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0451: Unknown result type (might be due to invalid IL or missing references)
		//IL_0452: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0549: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b5: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.Draw(m_App.m_MenuBackground, new Vector2(0f, 0f), Color.White);
		m_App.m_SpriteBatch.End();
		Vector2 lEVELENDUSER_TOP = LEVELENDUSER_TOP;
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, "LEVEL RATING", lEVELENDUSER_TOP, m_App.m_FrontEnd.TITLE_COL);
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"Name: {m_App.m_PlayingUserLevelName}", lEVELENDUSER_TOP + new Vector2(0f, 60f), m_App.m_FrontEnd.TITLE_COL);
		lEVELENDUSER_TOP.X -= 40f;
		lEVELENDUSER_TOP.Y += 140f;
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, "Current rating", lEVELENDUSER_TOP, m_App.m_FrontEnd.TITLE_COL);
		lEVELENDUSER_TOP.Y += 40f;
		m_App.m_SpriteBatch.Draw(m_App.m_RatingLargeEmptyTexture, new Vector2(lEVELENDUSER_TOP.X, lEVELENDUSER_TOP.Y + 10f), Color.White);
		float num = m_CurrRating / (float)m_CurrRatingCount;
		float num2 = num / 5f;
		int num3 = (int)(num2 * (float)Program.m_App.m_RatingLargeTexture.Width);
		Rectangle value = default(Rectangle);
		((Rectangle)(ref value))._002Ector(0, 0, num3, Program.m_App.m_RatingLargeTexture.Height);
		Rectangle val = default(Rectangle);
		((Rectangle)(ref val))._002Ector((int)lEVELENDUSER_TOP.X, (int)(lEVELENDUSER_TOP.Y + 10f), num3, Program.m_App.m_RatingLargeTexture.Height);
		m_App.m_SpriteBatch.Draw(Program.m_App.m_RatingLargeTexture, val, (Rectangle?)value, Color.White);
		lEVELENDUSER_TOP.X += 220f;
		lEVELENDUSER_TOP.Y += 15f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{m_CurrRatingCount}", lEVELENDUSER_TOP, Color.LightGoldenrodYellow);
		lEVELENDUSER_TOP.X = LEVELENDUSER_TOP.X - 40f;
		lEVELENDUSER_TOP.Y += 100f;
		Color tITLE_COL = m_App.m_FrontEnd.TITLE_COL;
		if (m_bAlreadyRated)
		{
			((Color)(ref tITLE_COL))._002Ector(0.5f, 0.5f, 0.5f, 0.5f);
		}
		if (m_bAlreadyRated)
		{
			m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, "My rating (already rated)", lEVELENDUSER_TOP, tITLE_COL);
		}
		else
		{
			m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, "My rating", lEVELENDUSER_TOP, tITLE_COL);
		}
		lEVELENDUSER_TOP.Y += 40f;
		m_App.m_SpriteBatch.Draw(m_App.m_RatingLargeEmptyTexture, new Vector2(lEVELENDUSER_TOP.X, lEVELENDUSER_TOP.Y + 10f), tITLE_COL);
		num3 = (int)((float)m_MyRating / 5f * (float)Program.m_App.m_RatingLargeTexture.Width);
		((Rectangle)(ref value))._002Ector(0, 0, num3, Program.m_App.m_RatingLargeTexture.Height);
		((Rectangle)(ref val))._002Ector((int)lEVELENDUSER_TOP.X, (int)(lEVELENDUSER_TOP.Y + 10f), num3, Program.m_App.m_RatingLargeTexture.Height);
		m_App.m_SpriteBatch.Draw(Program.m_App.m_RatingLargeTexture, val, (Rectangle?)value, tITLE_COL);
		lEVELENDUSER_TOP.X = 120f;
		lEVELENDUSER_TOP.Y = 545f;
		lEVELENDUSER_TOP.Y += 60f;
		m_App.m_GameText.mText = "[Y]Replay";
		m_App.m_GameText.Position = lEVELENDUSER_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		lEVELENDUSER_TOP.X += 360f;
		if (m_bAlreadyRated)
		{
			m_App.m_GameText.mText = "[A]Continue";
		}
		else
		{
			m_App.m_GameText.mText = "[A]Rate";
		}
		m_App.m_GameText.Position = lEVELENDUSER_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		if (!m_bAlreadyRated)
		{
			lEVELENDUSER_TOP.X += 250f;
			lEVELENDUSER_TOP.Y += 30f;
			m_App.m_GameText.mText = m_App.GetText(App.TEXTID.LSTICK);
			m_App.m_GameText.Position = lEVELENDUSER_TOP;
			m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.4f);
			lEVELENDUSER_TOP.X += 100f;
			lEVELENDUSER_TOP.Y -= 30f;
			m_App.m_GameText.mText = "Edit Rating";
			m_App.m_GameText.Position = lEVELENDUSER_TOP;
			m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		}
		m_App.RenderLines();
		m_App.m_SpriteBatch.End();
		m_App.m_SpriteBatch.Begin((SpriteSortMode)0, BlendState.Additive);
		Program.m_ParticleManager.Render();
		m_App.m_SpriteBatch.End();
	}
}
