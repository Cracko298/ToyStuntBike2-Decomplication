using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class LevelEnd
{
	public static Vector2 LEVELEND_TOP = new Vector2(200f, 340f);

	public static Vector2 LEVELEND_OFFSET = new Vector2(0f, 40f);

	public static Vector2 LEVELEND_TOP_OFFSET = new Vector2(-200f, LEVELEND_TOP.Y - 30f);

	public static Vector2 LEVEL_END_TARGETS = new Vector2(130f, 100f);

	private App m_App;

	private static int NUM_HISCORES = 6;

	private TopScoreEntry[] page = new TopScoreEntry[NUM_HISCORES];

	private int pageIndex;

	private int m_NumPages;

	private bool bTimeAlready;

	private bool bTimeWon;

	private bool bScoreAlready;

	private bool bScoreWon;

	private bool bFlagsAlready;

	private bool bFlagsWon;

	private float m_TargetTime;

	private float m_TargetScore;

	private int m_FlagsWereCollected;

	private int m_CurrentLevel;

	private float m_MenuMoveTime;

	private int m_UnlockTimeFrame;

	private int m_UnlockScoreFrame;

	private int m_UnlockFlagsFrame;

	private float m_TextScale1;

	private float m_TextScale2;

	private float m_TextScale3;

	public LevelEnd(App app)
	{
		m_App = app;
	}

	public void Start()
	{
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		bTimeAlready = false;
		bTimeWon = false;
		bScoreAlready = false;
		bScoreWon = false;
		bFlagsAlready = false;
		bFlagsWon = false;
		m_TargetTime = 0f;
		m_TargetScore = 0f;
		m_FlagsWereCollected = 0;
		m_CurrentLevel = Program.m_App.m_Level;
		m_UnlockTimeFrame = 0;
		m_UnlockScoreFrame = 0;
		m_UnlockFlagsFrame = 0;
		if (Program.m_App.mScores != null)
		{
			if (Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] != null && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId].IsSignedInToLive)
			{
				pageIndex = Program.m_App.mScores.fillPageThatContainsGamertagFromFullList(Program.m_App.m_Level - 1, page, ((Gamer)Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId]).Gamertag);
			}
			else
			{
				Program.m_App.mScores.fillPageFromFullList(Program.m_App.m_Level - 1, pageIndex, page);
			}
			m_NumPages = (Program.m_App.mScores.getFullListSize(Program.m_App.m_Level - 1) - 1) / NUM_HISCORES;
			m_TargetTime = Map.CupTargets[(Program.m_App.m_Level - 1) * 2];
			m_TargetScore = Map.CupTargets[(Program.m_App.m_Level - 1) * 2 + 1];
			m_FlagsWereCollected = Program.m_ItemManager.GetNumFlagsCollectedOnLevel(Program.m_App.m_Level - 1);
			int level = Program.m_App.m_Level - 1;
			if (Program.m_ItemManager.GetTimeCupOnLevel(level) == 1)
			{
				bTimeAlready = true;
			}
			else
			{
				bTimeAlready = false;
				if (Program.m_App.m_TrackTime < m_TargetTime)
				{
					bTimeWon = true;
					Program.m_ItemManager.GiveTimeCupOnLevel(level);
				}
			}
			if (Program.m_ItemManager.GetScoreCupOnLevel(level) == 1)
			{
				bScoreAlready = true;
			}
			else
			{
				bScoreAlready = false;
				if ((float)Program.m_PlayerManager.GetPrimaryPlayer().m_Score > m_TargetScore)
				{
					bScoreWon = true;
					Program.m_ItemManager.GiveScoreCupOnLevel(level);
				}
			}
			if (Program.m_ItemManager.GetFlagsCupOnLevel(level) == 1)
			{
				bFlagsAlready = true;
			}
			else
			{
				bFlagsAlready = false;
				if (Program.m_ItemManager.GetNumFlagsCollectedOnLevel(level) == 3)
				{
					bFlagsWon = true;
					Program.m_ItemManager.GiveFlagsCupOnLevel(level);
				}
			}
			m_App.IncrementLevel();
		}
		if (m_CurrentLevel == 13)
		{
			bTimeAlready = false;
			bTimeWon = false;
			bScoreAlready = false;
			bScoreWon = false;
			bFlagsAlready = false;
			bFlagsWon = false;
			m_TargetTime = 0f;
			m_TargetScore = 0f;
			m_FlagsWereCollected = 0;
		}
		Program.m_LoadSaveManager.SaveGame();
		m_TextScale1 = 3f;
		m_TextScale2 = 3f;
		m_TextScale3 = 3f;
		m_UnlockTimeFrame = 25;
		m_UnlockScoreFrame = 50;
		m_UnlockFlagsFrame = 75;
	}

	public void Stop()
	{
	}

	public void Update()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_024c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0251: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_031c: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.GetPrimaryPlayer() == null)
		{
			return;
		}
		Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_App.m_PlayerOnePadId);
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)4096))
		{
			m_App.m_NextState = App.STATE.MAP;
			m_App.StartFade(up: false);
			Program.m_SoundManager.Play(2);
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)32768))
		{
			m_App.m_NextState = App.STATE.CONTINUEGAME;
			m_App.StartFade(up: false);
			Program.m_SoundManager.Play(2);
			Program.m_App.m_Level = m_CurrentLevel;
			Program.m_PlayerManager.GetPrimaryPlayer().ResetBike(bForce: true, restartRace: true, Vector2.Zero);
			Program.m_PlayerManager.GetPrimaryPlayer().m_ReadySteadyTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 4500f;
			return;
		}
		Program.m_PlayerManager.GetPrimaryPlayer().CheckJukeboxControls();
		if (m_MenuMoveTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
		{
			if (Program.m_PlayerManager.GetPrimaryPlayer().LAY() < -0.9f && pageIndex < m_NumPages)
			{
				pageIndex++;
				Program.m_App.mScores.fillPageFromFullList(m_CurrentLevel - 1, pageIndex, page);
				m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
				Program.m_SoundManager.Play(4);
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().LAY() > 0.9f && pageIndex > 0)
			{
				pageIndex--;
				Program.m_App.mScores.fillPageFromFullList(m_CurrentLevel - 1, pageIndex, page);
				m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 125f;
				Program.m_SoundManager.Play(4);
			}
		}
		Program.m_App.CheckInGamePurchase();
		Program.m_PlayerManager.GetPrimaryPlayer().m_OldGamepadState = Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState;
		if (m_UnlockTimeFrame > -1)
		{
			m_UnlockTimeFrame--;
		}
		if (m_UnlockScoreFrame > -1)
		{
			m_UnlockScoreFrame--;
		}
		if (m_UnlockFlagsFrame > -1)
		{
			m_UnlockFlagsFrame--;
		}
		if (m_UnlockTimeFrame == 0)
		{
			if (bTimeWon)
			{
				CreateEffect(new Vector2(860f, 155f));
			}
			else
			{
				Program.m_SoundManager.Play(7);
			}
		}
		if (m_UnlockScoreFrame == 0)
		{
			if (bScoreWon)
			{
				CreateEffect(new Vector2(860f, 210f));
			}
			else
			{
				Program.m_SoundManager.Play(7);
			}
		}
		if (m_UnlockFlagsFrame == 0)
		{
			if (bFlagsWon)
			{
				CreateEffect(new Vector2(860f, 265f));
			}
			else
			{
				Program.m_SoundManager.Play(7);
			}
		}
		Program.m_ParticleManager.Update();
	}

	public void CreateEffect(Vector2 pos)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_011a: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0142: Unknown result type (might be due to invalid IL or missing references)
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		EmitterDef def = default(EmitterDef);
		def.m_Acceleration = new Vector2(0f, 0f);
		def.m_fFrameRate = 0.0333f;
		def.m_fLife = 3000f;
		def.m_fRate = 0f;
		def.m_Friction = new Vector2(1f, 1f);
		def.m_Image = Program.m_App.m_Particle1Texture;
		def.m_NumFrames = 0;
		def.m_NumParticles = 150;
		def.m_Origin = new Vector2(64f, 64f);
		def.m_Position = new Vector2(pos.X, pos.Y);
		def.m_Velocity = new Vector2(0f, 0f);
		def.m_VelocityVar = new Vector2(16f, 0f);
		def.m_Colour = new Color(1f, 1f, 1f, 1f);
		def.m_fRotation = 0.1f;
		def.m_Track = null;
		def.m_Flags = 32u;
		def.m_Offset = new Vector2(0f, 20f);
		def.m_fScale = 1f;
		def.m_Font = null;
		def.m_String = null;
		def.m_EmitterVelocity = Vector2.Zero;
		def.m_EmitterAcceleration = Vector2.Zero;
		Program.m_ParticleManager.Create(def);
		Program.m_SoundManager.Play(42);
	}

	public void Draw()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0237: Unknown result type (might be due to invalid IL or missing references)
		//IL_023c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_01be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0214: Unknown result type (might be due to invalid IL or missing references)
		//IL_0255: Unknown result type (might be due to invalid IL or missing references)
		//IL_025a: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0448: Unknown result type (might be due to invalid IL or missing references)
		//IL_0449: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0482: Unknown result type (might be due to invalid IL or missing references)
		//IL_04ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_04bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0540: Unknown result type (might be due to invalid IL or missing references)
		//IL_0541: Unknown result type (might be due to invalid IL or missing references)
		//IL_0590: Unknown result type (might be due to invalid IL or missing references)
		//IL_0591: Unknown result type (might be due to invalid IL or missing references)
		//IL_0610: Unknown result type (might be due to invalid IL or missing references)
		//IL_0611: Unknown result type (might be due to invalid IL or missing references)
		//IL_033e: Unknown result type (might be due to invalid IL or missing references)
		//IL_033f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0380: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_07a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_083d: Unknown result type (might be due to invalid IL or missing references)
		//IL_083e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0651: Unknown result type (might be due to invalid IL or missing references)
		//IL_0652: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d35: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a18: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a19: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a51: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a52: Unknown result type (might be due to invalid IL or missing references)
		//IL_087e: Unknown result type (might be due to invalid IL or missing references)
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_0685: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a93: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0900: Unknown result type (might be due to invalid IL or missing references)
		//IL_0905: Unknown result type (might be due to invalid IL or missing references)
		//IL_0731: Unknown result type (might be due to invalid IL or missing references)
		//IL_0732: Unknown result type (might be due to invalid IL or missing references)
		//IL_0709: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ada: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b04: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_095e: Unknown result type (might be due to invalid IL or missing references)
		//IL_095f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0936: Unknown result type (might be due to invalid IL or missing references)
		//IL_0937: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b72: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b73: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4b: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.Draw(m_App.m_MenuBackground, new Vector2(0f, 0f), Color.White);
		m_App.m_SpriteBatch.End();
		Vector2 lEVELEND_TOP = LEVELEND_TOP;
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, $"TRACK {m_CurrentLevel} SCORES", lEVELEND_TOP - LEVELEND_TOP_OFFSET, m_App.m_FrontEnd.TITLE_COL);
		lEVELEND_TOP.X -= 80f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "GLOBAL RANK", lEVELEND_TOP, Color.White);
		lEVELEND_TOP.X += 360f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "NAME", lEVELEND_TOP, Color.White);
		lEVELEND_TOP.X += 460f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "BEST TIME", lEVELEND_TOP, Color.White);
		lEVELEND_TOP.Y += 35f;
		if (Guide.IsTrialMode)
		{
			lEVELEND_TOP.X = LEVELEND_TOP.X + 160f;
			lEVELEND_TOP.Y += 20f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Not Available in Trial Mode.", lEVELEND_TOP, Color.White);
			lEVELEND_TOP.X += 30f;
			lEVELEND_TOP.Y += 60f;
			Program.m_App.m_GameText.mText = Program.m_App.GetText(App.TEXTID.PRESS_X_TO_BUY);
			Program.m_App.m_GameText.Position = lEVELEND_TOP;
			Program.m_App.m_GameText.Draw(Program.m_App.m_SpriteBatch, 0.75f);
		}
		Color lightGoldenrodYellow = Color.LightGoldenrodYellow;
		TopScoreEntry[] array = page;
		foreach (TopScoreEntry topScoreEntry in array)
		{
			lightGoldenrodYellow = Color.LightGoldenrodYellow;
			if (topScoreEntry != null)
			{
				float num = topScoreEntry.Score;
				int num2 = (int)(num / 60000f);
				int num3 = (int)(num % 60000f / 1000f);
				int num4 = (int)(num % 1000f);
				if (Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] != null && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId].IsSignedInToLive && topScoreEntry.Gamertag == ((Gamer)Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId]).Gamertag)
				{
					lightGoldenrodYellow = m_App.m_FrontEnd.TITLE_COL;
				}
				lEVELEND_TOP.X = LEVELEND_TOP.X + 40f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{topScoreEntry.RankAtLastPageFill}. ", lEVELEND_TOP, lightGoldenrodYellow);
				lEVELEND_TOP.X += 240f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{topScoreEntry.Gamertag}", lEVELEND_TOP, lightGoldenrodYellow);
				lEVELEND_TOP.X += 460f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{num2:d2}:{num3:d2}:{num4:d3}", lEVELEND_TOP, lightGoldenrodYellow);
				lEVELEND_TOP.Y += 35f;
			}
		}
		if (m_UnlockTimeFrame <= 0)
		{
			lEVELEND_TOP.X = LEVEL_END_TARGETS.X + 30f;
			lEVELEND_TOP.Y = LEVEL_END_TARGETS.Y;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "RESULT", lEVELEND_TOP, Color.White);
			lEVELEND_TOP.X += 370f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "TARGET", lEVELEND_TOP, Color.White);
			lEVELEND_TOP.X += 330f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "CUP STATUS", lEVELEND_TOP, Color.White);
			lEVELEND_TOP.X = LEVEL_END_TARGETS.X;
			lEVELEND_TOP.Y += 55f;
			int num2 = (int)(Program.m_App.m_TrackTime / 60000f);
			int num3 = (int)(Program.m_App.m_TrackTime % 60000f / 1000f);
			int num4 = (int)(Program.m_App.m_TrackTime % 1000f);
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Time:", lEVELEND_TOP, Color.Yellow);
			lEVELEND_TOP.X += 150f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{num2:d2}:{num3:d2}.{num4:d3}", lEVELEND_TOP, Color.LightGoldenrodYellow);
			lEVELEND_TOP.X += 250f;
			num2 = (int)(m_TargetTime / 60000f);
			num3 = (int)(m_TargetTime % 60000f / 1000f);
			num4 = (int)(m_TargetTime % 1000f);
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{num2:d2}:{num3:d2}.{num4:d3}", lEVELEND_TOP, Color.YellowGreen);
			lEVELEND_TOP.X += 330f;
			if (bTimeAlready)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "ALREADY HAVE", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			else if (bTimeWon)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "COMPLETE!", lEVELEND_TOP, Color.Gold, 0f, new Vector2(0f, 0f), m_TextScale1, (SpriteEffects)0, 0f);
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, lEVELEND_TOP + new Vector2(-30f, -20f), Color.LightGoldenrodYellow);
			}
			else if (m_CurrentLevel == 13)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "N/A", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			else
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "FAILED", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			m_TextScale1 = MathHelper.Lerp(m_TextScale1, 1f, 0.2f);
		}
		if (m_UnlockScoreFrame <= 0)
		{
			lEVELEND_TOP.X = LEVEL_END_TARGETS.X;
			lEVELEND_TOP.Y += 55f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Score:", lEVELEND_TOP, Color.Yellow);
			lEVELEND_TOP.X += 150f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{Program.m_PlayerManager.GetPrimaryPlayer().m_Score}", lEVELEND_TOP, Color.LightGoldenrodYellow);
			lEVELEND_TOP.X += 250f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{m_TargetScore}", lEVELEND_TOP, Color.YellowGreen);
			lEVELEND_TOP.X += 330f;
			if (bScoreAlready)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "ALREADY HAVE", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			else if (bScoreWon)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "COMPLETE!", lEVELEND_TOP, Color.Gold, 0f, new Vector2(0f, 0f), m_TextScale2, (SpriteEffects)0, 0f);
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, lEVELEND_TOP + new Vector2(-30f, -20f), Color.LightGoldenrodYellow);
			}
			else if (m_CurrentLevel == 13)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "N/A", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			else
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "FAILED", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			m_TextScale2 = MathHelper.Lerp(m_TextScale2, 1f, 0.2f);
		}
		if (m_UnlockFlagsFrame <= 0)
		{
			lEVELEND_TOP.X = LEVEL_END_TARGETS.X;
			lEVELEND_TOP.Y += 55f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Flags:", lEVELEND_TOP, Color.Yellow);
			lEVELEND_TOP.X += 150f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{m_FlagsWereCollected}", lEVELEND_TOP, Color.LightGoldenrodYellow);
			lEVELEND_TOP.X += 250f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "3", lEVELEND_TOP, Color.YellowGreen);
			lEVELEND_TOP.X += 330f;
			if (bFlagsAlready)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "ALREADY HAVE", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			else if (bFlagsWon)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "COMPLETE!", lEVELEND_TOP, Color.Gold, 0f, new Vector2(0f, 0f), m_TextScale3, (SpriteEffects)0, 0f);
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, lEVELEND_TOP + new Vector2(-30f, -20f), Color.LightGoldenrodYellow);
			}
			else if (m_CurrentLevel == 13)
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "N/A", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			else
			{
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "FAILED", lEVELEND_TOP, Color.LightGoldenrodYellow);
			}
			m_TextScale3 = MathHelper.Lerp(m_TextScale3, 1f, 0.2f);
		}
		lEVELEND_TOP.X = 120f;
		lEVELEND_TOP.Y = 545f;
		lEVELEND_TOP.Y += 60f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.ATOCONTINUE);
		m_App.m_GameText.Position = lEVELEND_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		lEVELEND_TOP.X += 430f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.RETRY);
		m_App.m_GameText.Position = lEVELEND_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		lEVELEND_TOP.X += 360f;
		lEVELEND_TOP.Y += 30f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.LSTICK);
		m_App.m_GameText.Position = lEVELEND_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.4f);
		lEVELEND_TOP.X += 100f;
		lEVELEND_TOP.Y -= 30f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.PAGE);
		m_App.m_GameText.Position = lEVELEND_TOP;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		m_App.RenderLines();
		m_App.m_SpriteBatch.End();
		m_App.m_SpriteBatch.Begin((SpriteSortMode)0, BlendState.Additive);
		Program.m_ParticleManager.Render();
		m_App.m_SpriteBatch.End();
	}
}
