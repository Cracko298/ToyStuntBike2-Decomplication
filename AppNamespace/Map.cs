using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class Map
{
	public const float MENU_REPEAT_TIME = 150f;

	private App m_App;

	public int m_LastLevelReached;

	private int m_NumFrames;

	private int m_LevelSelected;

	private float m_MenuMoveTime;

	private int m_ShowSave;

	private float m_MapUnlockTime;

	private bool m_bFlash;

	private float m_FlashTime;

	private int m_FlashLevel = -1;

	private int m_FlashLevel2 = -1;

	private int m_FlashLevel3 = -1;

	private bool m_bUnlockSoundDone;

	public static Vector2 TEXT_TOP = new Vector2(450f, 50f);

	private int[] MapCoords = new int[26]
	{
		306, 233, 445, 235, 563, 233, 671, 225, 789, 273,
		861, 225, 932, 471, 739, 469, 623, 463, 482, 479,
		358, 475, 212, 479, 560, 590
	};

	public static float[] CupTargets = new float[26]
	{
		25000f, 50000f, 26400f, 75000f, 40000f, 75000f, 42200f, 81000f, 37500f, 66000f,
		44400f, 73000f, 47000f, 65000f, 42300f, 55000f, 38500f, 60000f, 35000f, 41000f,
		37700f, 85000f, 45000f, 10200f, 0f, 0f
	};

	private static Vector2 playerOffset = new Vector2(-25f, -70f);

	public Map(App app)
	{
		m_App = app;
	}

	public void Start()
	{
		Program.m_App.m_LevelReached = Program.m_ItemManager.TotalCupsCollected() + 1;
		if (Program.m_App.m_LevelReached > 12)
		{
			Program.m_App.m_LevelReached = 12;
		}
		if (Guide.IsTrialMode && Program.m_App.m_LevelReached > 3)
		{
			Program.m_App.m_LevelReached = 3;
		}
		int num = Program.m_App.m_Level;
		if (num > Program.m_App.m_LevelReached)
		{
			num = Program.m_App.m_LevelReached;
		}
		if (num > 12)
		{
			num = 12;
		}
		if (Program.m_App.m_Level <= 13)
		{
			m_LevelSelected = num - 1;
		}
		else
		{
			m_LevelSelected = 0;
		}
		m_NumFrames = 0;
		m_ShowSave = 120;
		Program.m_LoadSaveManager.SaveGame();
		Program.m_PlayerManager.m_Player[0].m_State = Player.State.WAITING_AT_START;
		m_FlashLevel = -1;
		m_FlashLevel2 = -1;
		m_FlashLevel3 = -1;
		m_bUnlockSoundDone = false;
	}

	public void Stop()
	{
		for (int i = 0; i < 2; i++)
		{
			if (Program.m_PlayerManager.m_Player[i].m_Id != -1)
			{
				Program.m_PlayerManager.m_Player[i].m_State = Player.State.WAITING_AT_START;
			}
		}
	}

	public void Update()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_PlayerManager.GetPrimaryPlayer() != null)
		{
			Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_App.m_PlayerOnePadId);
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer() != null)
		{
			UpdateMapScreen();
		}
		Program.m_PlayerManager.CheckDropIn();
		Program.m_ParticleManager.Update();
	}

	private void UpdateMapScreen()
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Invalid comparison between Unknown and I4
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Invalid comparison between Unknown and I4
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_0159: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Invalid comparison between Unknown and I4
		//IL_01ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Invalid comparison between Unknown and I4
		//IL_0277: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		m_NumFrames++;
		if (m_NumFrames == 50)
		{
			for (int num = Program.m_App.m_LevelReached; num > m_LastLevelReached; num--)
			{
				if (Guide.IsTrialMode)
				{
					if (num < 4)
					{
						DoUnlockEffect(num - 1);
					}
				}
				else
				{
					DoUnlockEffect(num - 1);
				}
			}
			m_LastLevelReached = Program.m_App.m_LevelReached;
		}
		for (int i = 0; i < 3; i++)
		{
			Program.m_ParticleManager.Update();
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)4096))
		{
			m_LastLevelReached = Program.m_App.m_LevelReached;
			OnSelectPressed();
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)8192))
		{
			OnBackPressed();
		}
		GamePadThumbSticks thumbSticks = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).ThumbSticks;
		float num2 = ((GamePadThumbSticks)(ref thumbSticks)).Left.Y;
		if (Math.Abs(num2) < 0.25f)
		{
			num2 = 0f;
		}
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
		GamePadThumbSticks thumbSticks2 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).ThumbSticks;
		float num3 = ((GamePadThumbSticks)(ref thumbSticks2)).Left.X;
		if (Math.Abs(num3) < 0.25f)
		{
			num3 = 0f;
		}
		GamePadDPad dPad3 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad3)).Left == 1)
		{
			num3 = -1f;
		}
		GamePadDPad dPad4 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad4)).Right == 1)
		{
			num3 = 1f;
		}
		if (m_MenuMoveTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
		{
			Vector2 zero = Vector2.Zero;
			zero.X = num3;
			zero.Y = 0f - num2;
			((Vector2)(ref zero)).Normalize();
			int levelSelected = m_LevelSelected;
			m_LevelSelected = FindNearest(zero);
			m_MenuMoveTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 150f;
			if (levelSelected != m_LevelSelected)
			{
				Program.m_SoundManager.Play(4);
			}
		}
		Program.m_PlayerManager.GetPrimaryPlayer().CheckJukeboxControls();
		Program.m_PlayerManager.GetPrimaryPlayer().m_OldGamepadState = Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState;
	}

	private int FindNearest(Vector2 vDir)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		Vector2 zero = Vector2.Zero;
		zero.X = MapCoords[m_LevelSelected * 2];
		zero.Y = MapCoords[m_LevelSelected * 2 + 1];
		Vector2 zero2 = Vector2.Zero;
		Vector2 val = Vector2.Zero;
		float num = 0f;
		float num2 = 999999f;
		int num3 = -1;
		int num4 = Program.m_App.m_LevelReached;
		if (num4 > 12)
		{
			num4 = 12;
		}
		if (Program.m_ItemManager.TotalCupsCollected() == 36)
		{
			num4 = 13;
		}
		for (int i = 0; i < num4; i++)
		{
			if (m_LevelSelected == i)
			{
				continue;
			}
			zero2.X = MapCoords[i * 2];
			zero2.Y = MapCoords[i * 2 + 1];
			val = zero2 - zero;
			num = ((Vector2)(ref val)).LengthSquared();
			((Vector2)(ref val)).Normalize();
			float num5 = Vector2.Dot(val, vDir);
			if (num5 > 0.707f)
			{
				float num6 = 1f;
				num6 = ((!(num5 > 0.97f)) ? 1f : 0.8f);
				if (num * num6 < num2)
				{
					num2 = num * num6;
					num3 = i;
				}
			}
		}
		if (num3 == -1)
		{
			return m_LevelSelected;
		}
		return num3;
	}

	private void OnSelectPressed()
	{
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		if ((!m_App.m_bSplitScreen || Program.m_PlayerManager.NumPlayers() == 2) && !m_App.Fading())
		{
			m_App.m_Level = m_LevelSelected + 1;
			m_App.m_NextState = App.STATE.CONTINUEGAME;
			m_App.StartFade(up: false);
			Program.m_PlayerManager.GetPrimaryPlayer().ResetBike(bForce: true, restartRace: true, Vector2.Zero);
			Program.m_PlayerManager.GetPrimaryPlayer().m_ReadySteadyTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 4500f;
			if (m_App.m_bSplitScreen)
			{
				Program.m_PlayerManager.m_Player[1].ResetBike(bForce: true, restartRace: true, Vector2.Zero);
				Program.m_PlayerManager.m_Player[1].m_ReadySteadyTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 4500f;
			}
			Program.m_SoundManager.Play(2);
		}
	}

	private void OnBackPressed()
	{
		if (!m_App.Fading())
		{
			m_App.m_NextState = App.STATE.MAINMENU;
			m_App.StartFade(up: false);
			Program.m_SoundManager.Play(3);
		}
	}

	public void DoUnlockEffect(int buttonId)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		m_MapUnlockTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalSeconds + 3f;
		if (m_FlashLevel == -1)
		{
			m_FlashLevel = buttonId;
		}
		else if (m_FlashLevel2 == -1)
		{
			m_FlashLevel2 = buttonId;
		}
		else if (m_FlashLevel3 == -1)
		{
			m_FlashLevel3 = buttonId;
		}
		if (!m_bUnlockSoundDone)
		{
			Program.m_SoundManager.Play(36);
			m_bUnlockSoundDone = true;
		}
		CreateUnlockEffect(new Vector2((float)MapCoords[buttonId * 2], (float)MapCoords[buttonId * 2 + 1]));
	}

	public void CreateUnlockEffect(Vector2 pos)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		EmitterDef def = default(EmitterDef);
		def.m_Acceleration = new Vector2(0f, 0.01f);
		def.m_fFrameRate = 0.0333f;
		def.m_fLife = 3000f;
		def.m_fRate = 1f;
		def.m_Friction = new Vector2(1f, 1f);
		def.m_Image = Program.m_App.m_Particle2Texture;
		def.m_NumFrames = 0;
		def.m_NumParticles = 100;
		def.m_Origin = new Vector2(16f, 16f);
		def.m_Position = new Vector2(pos.X, pos.Y);
		def.m_Velocity = new Vector2(0f, -2f);
		def.m_VelocityVar = new Vector2(0.5f, 1f);
		def.m_Colour = new Color(1f, 1f, 1f, 1f);
		def.m_fRotation = 0.1f;
		def.m_Track = null;
		def.m_Flags = 1u;
		def.m_Offset = new Vector2(0f, 0f);
		def.m_fScale = 1f;
		def.m_Font = null;
		def.m_String = null;
		def.m_EmitterVelocity = Vector2.Zero;
		def.m_EmitterAcceleration = Vector2.Zero;
		Program.m_ParticleManager.Create(def);
		Program.m_SoundManager.Play(43);
	}

	public void Draw()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0102: Unknown result type (might be due to invalid IL or missing references)
		//IL_04db: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0527: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_057e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0585: Unknown result type (might be due to invalid IL or missing references)
		//IL_058a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0616: Unknown result type (might be due to invalid IL or missing references)
		//IL_061c: Unknown result type (might be due to invalid IL or missing references)
		//IL_061e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0171: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0354: Unknown result type (might be due to invalid IL or missing references)
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_0793: Unknown result type (might be due to invalid IL or missing references)
		//IL_0667: Unknown result type (might be due to invalid IL or missing references)
		//IL_0669: Unknown result type (might be due to invalid IL or missing references)
		//IL_03c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_043e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0440: Unknown result type (might be due to invalid IL or missing references)
		//IL_0278: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		//IL_0258: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e70: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ab6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b07: Unknown result type (might be due to invalid IL or missing references)
		//IL_0876: Unknown result type (might be due to invalid IL or missing references)
		//IL_0878: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_08af: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0980: Unknown result type (might be due to invalid IL or missing references)
		//IL_0982: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a08: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a41: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a78: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b91: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c47: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d14: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0cf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dd8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dda: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d95: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d97: Unknown result type (might be due to invalid IL or missing references)
		((Game)Program.m_App).GraphicsDevice.SetRenderTarget(Program.m_App.SHAD_shadowRenderTarget);
		((Game)Program.m_App).GraphicsDevice.Clear(Color.White);
		((Game)Program.m_App).GraphicsDevice.SetRenderTarget((RenderTarget2D)null);
		m_App.m_SpriteBatch.Begin();
		m_App.m_SpriteBatch.Draw(m_App.m_MapBackground, new Vector2(0f, 0f), Color.White);
		int num = Program.m_App.m_LevelReached;
		Vector2 val = default(Vector2);
		Color val2 = default(Color);
		for (int i = 0; i < 12; i++)
		{
			string arg = (i + 1).ToString();
			((Vector2)(ref val))._002Ector((float)MapCoords[i * 2], (float)MapCoords[i * 2 + 1]);
			if (i >= num)
			{
				((Color)(ref val2))._002Ector(64, 64, 64, 32);
			}
			else
			{
				((Color)(ref val2))._002Ector(64, 64, 64, 255);
			}
			Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SpeechFont, $"{arg}.", val, val2);
			if (Program.m_ItemManager.GetTimeCupOnLevel(i) == 1)
			{
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, val + new Vector2(25f, -15f), Color.White);
			}
			else
			{
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, val + new Vector2(25f, -15f), new Color(0.125f, 0.125f, 0.125f, 0.125f));
			}
			if (Program.m_ItemManager.GetScoreCupOnLevel(i) == 1)
			{
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, val + new Vector2(45f, -15f), Color.White);
			}
			else
			{
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, val + new Vector2(45f, -15f), new Color(0.125f, 0.125f, 0.125f, 0.125f));
			}
			if (Program.m_ItemManager.GetFlagsCupOnLevel(i) == 1)
			{
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, val + new Vector2(65f, -15f), Color.White);
			}
			else
			{
				Program.m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTexture, val + new Vector2(65f, -15f), new Color(0.125f, 0.125f, 0.125f, 0.125f));
			}
			if (num > 12)
			{
				num = 12;
			}
		}
		if (m_MapUnlockTime > (float)Program.m_App.m_GameTime.TotalGameTime.TotalSeconds)
		{
			Vector2 zero = Vector2.Zero;
			if (m_FlashLevel != -1)
			{
				((Vector2)(ref zero))._002Ector((float)MapCoords[m_FlashLevel * 2], (float)MapCoords[m_FlashLevel * 2 + 1]);
				if (m_bFlash)
				{
					Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SpeechFont, $"{(m_FlashLevel + 1).ToString()}.", zero, Color.LightGoldenrodYellow);
				}
			}
			if (m_FlashLevel2 != -1)
			{
				((Vector2)(ref zero))._002Ector((float)MapCoords[m_FlashLevel2 * 2], (float)MapCoords[m_FlashLevel2 * 2 + 1]);
				if (m_bFlash)
				{
					Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SpeechFont, $"{(m_FlashLevel2 + 1).ToString()}.", zero, Color.LightGoldenrodYellow);
				}
			}
			if (m_FlashLevel3 != -1)
			{
				((Vector2)(ref zero))._002Ector((float)MapCoords[m_FlashLevel3 * 2], (float)MapCoords[m_FlashLevel3 * 2 + 1]);
				if (m_bFlash)
				{
					Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SpeechFont, $"{(m_FlashLevel3 + 1).ToString()}.", zero, Color.LightGoldenrodYellow);
				}
			}
			if (m_FlashTime < (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds)
			{
				m_bFlash = !m_bFlash;
				m_FlashTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 100f;
			}
		}
		float num2 = 0.25f;
		if (Program.m_ItemManager.TotalCupsCollected() == 36)
		{
			num2 = 1f;
		}
		m_App.m_SpriteBatch.Draw(m_App.m_GoldFlagTexture, new Vector2(541f, 517f), new Color(1f * num2, 1f * num2, 1f * num2, num2));
		Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SpeechFont, "All Cups Champion!", new Vector2(470f, 618f), new Color(0.25f * num2, 0.25f * num2, 0.25f * num2, num2));
		Vector2 pos = default(Vector2);
		((Vector2)(ref pos))._002Ector((float)MapCoords[m_LevelSelected * 2], (float)MapCoords[m_LevelSelected * 2 + 1]);
		Program.m_App.DrawMenuPointer(pos);
		Vector2 val3 = TEXT_TOP;
		m_App.m_MenuText.mColor = m_App.m_FrontEnd.TITLE_COL;
		m_App.m_MenuText.Position = val3;
		m_App.m_MenuText.mText = m_App.GetText(App.TEXTID.SELECT_LEVEL);
		m_App.m_MenuText.Draw(m_App.m_SpriteBatch, 0.75f);
		Vector2 val4 = default(Vector2);
		((Vector2)(ref val4))._002Ector(120f, 50f);
		new Vector2(0f, 120f);
		val3 = val4;
		if (m_ShowSave > 0 && !Guide.IsTrialMode)
		{
			m_ShowSave--;
			val3.X = 500f;
			val3.Y = 600f;
			m_App.m_GameText.Position = val3;
			if (m_App.m_SaveErrorNoSpace)
			{
				m_App.m_GameText.Position.X = 480f;
				m_App.m_GameText.mText = m_App.GetText(App.TEXTID.SAVE_ERROR_NO_SPACE);
			}
			else if (m_App.m_SaveError)
			{
				m_App.m_GameText.Position.X = 480f;
				m_App.m_GameText.mText = m_App.GetText(App.TEXTID.SAVE_ERROR);
			}
			else
			{
				m_App.m_GameText.mText = m_App.GetText(App.TEXTID.SAVING);
			}
			m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		}
		val3.X = 120f;
		val3.Y = 545f;
		if (!m_App.m_bSplitScreen || Program.m_PlayerManager.NumPlayers() == 2)
		{
			m_App.m_GameText.mText = m_App.GetText(App.TEXTID.SELECT);
			m_App.m_GameText.Position = val3;
			m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		}
		val3.Y += 60f;
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.BACK);
		m_App.m_GameText.Position = val3;
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		if (!m_App.m_bSplitScreen)
		{
			if (m_LevelSelected < 13)
			{
				Color val5 = default(Color);
				((Color)(ref val5))._002Ector(200, 255, 50);
				((Vector2)(ref val3))._002Ector(960f, 60f);
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Track", val3, val5);
				val3.Y += 35f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Targets", val3, val5);
				val3.Y += 50f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Time", val3, val5);
				val3.Y += 35f;
				int num3 = (int)(CupTargets[m_LevelSelected * 2] / 60000f);
				int num4 = (int)(CupTargets[m_LevelSelected * 2] % 60000f / 1000f);
				int num5 = (int)(CupTargets[m_LevelSelected * 2] % 1000f);
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{num3:d2}:{num4:d2}:{num5:d3}", val3, val5);
				val3.Y += 50f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Score", val3, val5);
				val3.Y += 35f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{CupTargets[m_LevelSelected * 2 + 1]}", val3, val5);
				val3.Y += 50f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Flags", val3, val5);
				val3.Y += 35f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "3", val3, val5);
			}
			((Vector2)(ref val3))._002Ector(940f, 540f);
			m_App.m_SpriteBatch.Draw(Program.m_App.m_TSBCupTextureMed, val3 + new Vector2(130f, -30f), Color.White);
			m_App.m_SpriteBatch.DrawString(m_App.m_MediumFont, $"{Program.m_ItemManager.TotalCupsCollected()}/36", val3, new Color(255, 255, 100, 255));
			if (m_LevelSelected < 12)
			{
				Color val6 = default(Color);
				((Color)(ref val6))._002Ector(255, 200, 0, 255);
				Color val7 = default(Color);
				((Color)(ref val7))._002Ector(255, 230, 0, 255);
				Color val8 = default(Color);
				((Color)(ref val8))._002Ector(255, 150, 0, 255);
				((Vector2)(ref val3))._002Ector(120f, 60f);
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Track", val3, val6);
				val3.Y += 35f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Status", val3, val6);
				val3.Y += 50f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Time", val3, val6);
				val3.Y += 35f;
				if (Program.m_ItemManager.GetTimeCupOnLevel(m_LevelSelected) == 1)
				{
					m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Complete", val3, val7);
				}
				else
				{
					m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "No", val3, val8);
				}
				val3.Y += 50f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Score", val3, val6);
				val3.Y += 35f;
				if (Program.m_ItemManager.GetScoreCupOnLevel(m_LevelSelected) == 1)
				{
					m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Complete", val3, val7);
				}
				else
				{
					m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "No", val3, val8);
				}
				val3.Y += 50f;
				m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Flags", val3, val6);
				val3.Y += 35f;
				if (Program.m_ItemManager.GetFlagsCupOnLevel(m_LevelSelected) == 1)
				{
					m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Complete", val3, val7);
				}
				else
				{
					m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, $"{Program.m_ItemManager.GetNumFlagsCollectedOnLevel(m_LevelSelected)}/3", val3, val8);
				}
			}
		}
		else if (Program.m_PlayerManager.m_Player[1].m_Id == -1)
		{
			val3.X = 700f;
			val3.Y = 92f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "PLAYER 2 PRESS START", val3, Color.White);
		}
		else
		{
			val3.X = 800f;
			val3.Y = 92f;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "PLAYER 2 READY", val3, Color.White);
		}
		m_App.RenderLines();
		m_App.m_SpriteBatch.End();
		m_App.m_SpriteBatch.Begin((SpriteSortMode)0, BlendState.Additive);
		Program.m_ParticleManager.Render();
		m_App.m_SpriteBatch.End();
	}
}
