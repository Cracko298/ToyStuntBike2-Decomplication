using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace AppNamespace;

public class SaveGame
{
	public int m_SaveLevel;

	public int m_SaveLevelReached;

	public int[] m_SaveFlagsCollected;

	public int[] m_SaveCupsCollected;

	public int m_SaveMusicVol;

	public int m_SaveSFXVol;

	public App.LANG m_SaveLanguage;

	public bool m_SaveVibration;

	public int[] m_SaveLevelsVisited;

	public int[] m_SaveScore;

	public bool m_SaveSpeech;

	public int m_SaveTip;

	public int m_SaveMyLevelId;

	public bool m_SaveSplitScreenHoriz;

	public SaveGame()
	{
		m_SaveLevel = 0;
		m_SaveLevelReached = 0;
		m_SaveFlagsCollected = new int[36];
		for (int i = 0; i < 36; i++)
		{
			m_SaveFlagsCollected[i] = 0;
		}
		m_SaveCupsCollected = new int[36];
		for (int j = 0; j < 36; j++)
		{
			m_SaveCupsCollected[j] = 0;
		}
		m_SaveLevelsVisited = new int[14];
		for (int k = 0; k < 14; k++)
		{
			m_SaveLevelsVisited[k] = 0;
		}
		m_SaveScore = new int[2];
		m_SaveSpeech = false;
		m_SaveTip = 0;
		m_SaveMyLevelId = 0;
	}

	public void Update()
	{
		m_SaveLevel = Program.m_App.m_Level;
		m_SaveLevelReached = Program.m_App.m_LevelReached;
		m_SaveMusicVol = Program.m_App.m_Options.m_MusicVol;
		m_SaveSFXVol = Program.m_App.m_Options.m_SFXVol;
		m_SaveLanguage = Program.m_App.m_Lang;
		m_SaveVibration = Program.m_App.m_Options.m_bVibration;
		for (int i = 0; i < 36; i++)
		{
			m_SaveFlagsCollected[i] = Program.m_ItemManager.m_FlagsCollected[i];
		}
		for (int j = 0; j < 36; j++)
		{
			m_SaveCupsCollected[j] = Program.m_ItemManager.m_CupsCollected[j];
		}
		for (int k = 1; k < 14; k++)
		{
			m_SaveLevelsVisited[k] = Program.m_App.m_LevelsVisited[k];
		}
		for (int l = 0; l < 2; l++)
		{
			m_SaveScore[l] = Program.m_PlayerManager.m_Player[l].m_Score;
		}
		m_SaveTip = (int)Program.m_App.m_Tip;
		m_SaveMyLevelId = Program.m_App.m_MyLevelId;
		m_SaveSplitScreenHoriz = Program.m_App.m_Options.m_SplitScreenHoriz;
	}

	public void Restore()
	{
		Program.m_App.m_Level = m_SaveLevel;
		Program.m_App.m_LevelReached = m_SaveLevelReached;
		Program.m_App.m_Map.m_LastLevelReached = Program.m_App.m_LevelReached;
		Program.m_App.m_Options.m_MusicVol = m_SaveMusicVol;
		Program.m_App.m_Options.m_SFXVol = m_SaveSFXVol;
		Program.m_App.m_Lang = m_SaveLanguage;
		Program.m_App.m_Options.m_bVibration = m_SaveVibration;
		for (int i = 0; i < 36; i++)
		{
			Program.m_ItemManager.m_FlagsCollected[i] = m_SaveFlagsCollected[i];
		}
		for (int j = 0; j < 36; j++)
		{
			Program.m_ItemManager.m_CupsCollected[j] = m_SaveCupsCollected[j];
		}
		for (int k = 1; k < 14; k++)
		{
			Program.m_App.m_LevelsVisited[k] = m_SaveLevelsVisited[k];
		}
		for (int l = 0; l < 2; l++)
		{
			Program.m_PlayerManager.m_Player[l].m_Score = m_SaveScore[l];
		}
		Program.m_App.m_Tip = (App.TEXTID)m_SaveTip;
		Program.m_App.m_MyLevelId = m_SaveMyLevelId;
		Program.m_App.m_Options.m_SplitScreenHoriz = m_SaveSplitScreenHoriz;
		MediaPlayer.Volume = (float)Program.m_App.m_Options.m_MusicVol / 100f;
		SoundEffect.MasterVolume = (float)Program.m_App.m_Options.m_SFXVol / 100f;
	}
}
