using System;
using System.IO;
using System.Xml.Serialization;
using EasyStorage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

namespace AppNamespace;

public class LoadSaveManager
{
	public Vector2 m_Checkpoint;

	public float m_CheckpointDisplayTime;

	public Item[] m_Enemy;

	public int[] m_FlagsCollected;

	public int[] m_CupsCollected;

	public Trigger[] m_Trigger;

	public SaveGame m_SaveGame;

	public float m_TrackTime;

	public int m_Score;

	public LoadSaveManager()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		m_Checkpoint = Vector2.Zero;
		m_FlagsCollected = new int[36];
		for (int i = 0; i < 36; i++)
		{
			m_FlagsCollected[i] = 0;
		}
		m_CupsCollected = new int[36];
		for (int j = 0; j < 36; j++)
		{
			m_CupsCollected[j] = 0;
		}
		m_Score = 0;
		m_TrackTime = 0f;
		m_SaveGame = new SaveGame();
	}

	public void ClearCheckpoint()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_Checkpoint = Vector2.Zero;
	}

	public void SaveCheckpoint(Vector2 pos, bool bShowMessage)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = pos + new Vector2(0f, 0f);
		if (!(m_Checkpoint == val) || !(pos != Vector2.Zero))
		{
			m_Checkpoint = val;
			Program.m_ItemManager.CopyFlagsCollected(Program.m_ItemManager.m_FlagsCollected, m_FlagsCollected);
			Program.m_ItemManager.CopyCupsCollected(Program.m_ItemManager.m_CupsCollected, m_CupsCollected);
			m_Score = Program.m_PlayerManager.GetPrimaryPlayer().m_Score;
			m_TrackTime = Program.m_App.m_TrackTime;
			if (bShowMessage)
			{
				m_CheckpointDisplayTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalSeconds + 2f;
				Program.m_App.m_TextScale = 3f;
				Program.m_SoundManager.Play(39);
			}
		}
	}

	public void LoadCheckpoint(ref Vector2 pos)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		pos = m_Checkpoint;
		Program.m_ItemManager.CopyFlagsCollected(m_FlagsCollected, Program.m_ItemManager.m_FlagsCollected);
		Program.m_ItemManager.CopyCupsCollected(m_CupsCollected, Program.m_ItemManager.m_CupsCollected);
		Program.m_PlayerManager.GetPrimaryPlayer().m_Score = m_Score;
		Program.m_App.m_TrackTime = m_TrackTime;
	}

	public void Update()
	{
	}

	public void SaveGame()
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Expected O, but got Unknown
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Expected O, but got Unknown
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Expected O, but got Unknown
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Expected O, but got Unknown
		FileAction val = null;
		try
		{
			if (Guide.IsTrialMode || Guide.IsVisible || !((ISaveDevice)Program.m_App.saveDevice).IsReady)
			{
				return;
			}
			IAsyncSaveDevice saveDevice = Program.m_App.saveDevice;
			if (val == null)
			{
				val = (FileAction)delegate(Stream stream)
				{
					using StreamWriter textWriter = new StreamWriter(stream);
					m_SaveGame.Update();
					XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveGame));
					xmlSerializer.Serialize(textWriter, m_SaveGame);
					Program.m_App.m_bSaveExists = true;
				};
			}
			((ISaveDevice)saveDevice).Save("Toy Stunt Bike 2", "5827248A747F", val);
			((ISaveDevice)Program.m_App.saveDevice).Save("Toy Stunt Bike 2", "97463EF5CA2B", (FileAction)delegate(Stream stream)
			{
				BinaryWriter binaryWriter = new BinaryWriter(stream);
				Program.m_App.mScores.save(binaryWriter);
				binaryWriter.Close();
			});
			((ISaveDevice)Program.m_App.saveDevice).Save("Toy Stunt Bike 2", "7543857EFCA1", (FileAction)delegate(Stream stream)
			{
				BinaryWriter binaryWriter2 = new BinaryWriter(stream);
				Program.m_App.m_SharedLevels.save(binaryWriter2);
				binaryWriter2.Close();
			});
			((ISaveDevice)Program.m_App.saveDevice).Save("Toy Stunt Bike 2", "5F5F1AA89430", (FileAction)delegate(Stream stream)
			{
				BinaryWriter binaryWriter3 = new BinaryWriter(stream);
				Program.m_App.m_Ratings.Save(binaryWriter3);
				binaryWriter3.Close();
			});
		}
		catch (Exception arg)
		{
			Console.WriteLine("{0} Exception caught.", arg);
		}
	}

	public void LoadGame()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Expected O, but got Unknown
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Expected O, but got Unknown
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Expected O, but got Unknown
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Expected O, but got Unknown
		FileAction val = null;
		try
		{
			if (Guide.IsTrialMode || Guide.IsVisible)
			{
				return;
			}
			if (((ISaveDevice)Program.m_App.saveDevice).FileExists("Toy Stunt Bike 2", "5827248A747F"))
			{
				IAsyncSaveDevice saveDevice = Program.m_App.saveDevice;
				if (val == null)
				{
					val = (FileAction)delegate(Stream stream)
					{
						using StreamReader textReader = new StreamReader(stream);
						XmlSerializer xmlSerializer = new XmlSerializer(typeof(SaveGame));
						m_SaveGame = (SaveGame)xmlSerializer.Deserialize(textReader);
						m_SaveGame.Restore();
						Program.m_App.m_bSaveExists = true;
					};
				}
				((ISaveDevice)saveDevice).Load("Toy Stunt Bike 2", "5827248A747F", val);
			}
			else
			{
				Program.m_App.m_bSaveExists = false;
			}
			if (((ISaveDevice)Program.m_App.saveDevice).FileExists("Toy Stunt Bike 2", "97463EF5CA2B"))
			{
				((ISaveDevice)Program.m_App.saveDevice).Load("Toy Stunt Bike 2", "97463EF5CA2B", (FileAction)delegate(Stream stream)
				{
					BinaryReader binaryReader = new BinaryReader(stream);
					Program.m_App.mScores = new TopScoreListContainer(binaryReader);
					binaryReader.Close();
				});
			}
			if (((ISaveDevice)Program.m_App.saveDevice).FileExists("Toy Stunt Bike 2", "7543857EFCA1"))
			{
				((ISaveDevice)Program.m_App.saveDevice).Load("Toy Stunt Bike 2", "7543857EFCA1", (FileAction)delegate(Stream stream)
				{
					BinaryReader binaryReader2 = new BinaryReader(stream);
					Program.m_App.m_SharedLevels = new LevelListContainer(binaryReader2);
					binaryReader2.Close();
				});
			}
			if (((ISaveDevice)Program.m_App.saveDevice).FileExists("Toy Stunt Bike 2", "5F5F1AA89430"))
			{
				((ISaveDevice)Program.m_App.saveDevice).Load("Toy Stunt Bike 2", "5F5F1AA89430", (FileAction)delegate(Stream stream)
				{
					BinaryReader binaryReader3 = new BinaryReader(stream);
					Program.m_App.m_Ratings.Load(binaryReader3);
					binaryReader3.Close();
				});
			}
		}
		catch (Exception arg)
		{
			Console.WriteLine("{0} Exception caught.", arg);
		}
	}
}
