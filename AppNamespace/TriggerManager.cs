using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class TriggerManager
{
	public const int MAX_TRIGGERS = 512;

	public const int TRIGGER_EDIT_NORMAL = 0;

	public const int TRIGGER_EDIT_CREATE = 1;

	public const int TRIGGER_EDIT_CREATE1 = 2;

	public const int TRIGGER_EDIT_CREATE2 = 3;

	public const int TRIGGER_EDIT_CREATE3 = 4;

	public const int TRIGGER_EDIT_SELECTED = 5;

	private const float TRIGGER_ATTACH_RANGE_SQ = 2500f;

	public Trigger[] m_Trigger;

	public int m_EditorState;

	public Vector2 m_EditorPos;

	public Vector2 m_EditorPos3D;

	public int m_EditorTriggerType;

	public int m_EditorTriggerSelectedId;

	public int m_SuspendedCount;

	public int m_ActiveCount;

	public int m_CompletedCount;

	public int m_TotalCount;

	public TriggerManager()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		m_Trigger = new Trigger[512];
		for (int i = 0; i < 512; i++)
		{
			m_Trigger[i] = new Trigger();
		}
		m_EditorState = 0;
		m_EditorPos = Vector2.Zero;
		m_EditorTriggerType = -1;
		m_EditorTriggerSelectedId = -1;
		m_SuspendedCount = 0;
		m_ActiveCount = 0;
		m_CompletedCount = 0;
		m_TotalCount = 0;
	}

	public int Create(int type, Vector2 pos, float rot)
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		bool flag = false;
		int num = -1;
		for (int i = 0; i < 512; i++)
		{
			if (m_Trigger[i].m_Id == -1)
			{
				flag = true;
				num = i;
				break;
			}
		}
		if (flag)
		{
			m_Trigger[num].m_Type = type;
			m_Trigger[num].m_Position = pos;
			m_Trigger[num].m_Id = num;
			m_Trigger[num].m_bDistanceTrigger = true;
			m_Trigger[num].m_LinkActor = null;
			m_Trigger[num].m_State = Trigger.TRIGGER_STATE.SUSPENDED;
			m_Trigger[num].m_Flags = 0;
			m_Trigger[num].m_Rotation = rot;
			return num;
		}
		return -1;
	}

	public void Delete(int Id)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		m_Trigger[Id].m_Type = -1;
		m_Trigger[Id].m_Id = -1;
		m_Trigger[Id].m_State = Trigger.TRIGGER_STATE.SUSPENDED;
		m_Trigger[Id].m_Flags = 0;
		m_Trigger[Id].m_Position = Vector2.Zero;
		m_Trigger[Id].m_Rotation = 0f;
	}

	public void DeleteAll()
	{
		for (int i = 0; i < 512; i++)
		{
			if (m_Trigger[i].m_Id != -1)
			{
				Delete(i);
			}
		}
	}

	public void Update()
	{
		for (int i = 0; i < 512; i++)
		{
			if (m_Trigger[i].m_Id == -1)
			{
				continue;
			}
			if (m_Trigger[i].m_State == Trigger.TRIGGER_STATE.ACTIVE && (m_Trigger[i].m_Flags & 8) > 0)
			{
				m_Trigger[i].Update();
			}
			if (m_Trigger[i].m_State != 0)
			{
				continue;
			}
			if (m_Trigger[i].m_Type >= 500)
			{
				CheckSoundTriggers(i);
				continue;
			}
			float extraBounds = GetExtraBounds(m_Trigger[i].m_Type);
			if (m_Trigger[i].m_Position.X < Program.m_CurrentCamera.m_CameraPosition.X + 60f + extraBounds)
			{
				m_Trigger[i].Activate();
			}
		}
	}

	private void CheckSoundTriggers(int i)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = m_Trigger[i].m_Position - Program.m_PlayerManager.GetPrimaryPlayer().m_Position;
		float num = ((Vector2)(ref val)).LengthSquared();
		if (num < 1f)
		{
			switch (m_Trigger[i].m_Type)
			{
			case 501:
				Program.m_SoundManager.Play(40);
				m_Trigger[i].m_State = Trigger.TRIGGER_STATE.COMPLETED;
				break;
			case 500:
				Program.m_SoundManager.Play(41);
				m_Trigger[i].m_State = Trigger.TRIGGER_STATE.COMPLETED;
				break;
			}
		}
	}

	public float GetExtraBounds(int type)
	{
		switch (type)
		{
		case 87:
			return 40f;
		case 88:
		case 97:
		case 102:
		case 103:
			return 20f;
		case 100:
		case 101:
			return 25f;
		default:
			return 0f;
		}
	}

	public void ActivateByType(int type)
	{
		for (int i = 0; i < 512; i++)
		{
			if (m_Trigger[i].m_Id != -1 && m_Trigger[i].m_Type == type)
			{
				m_Trigger[i].Activate();
			}
		}
	}

	public void Render()
	{
	}

	public void DrawTriggerPoints(int highlightId)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position2 = default(Vector3);
		for (int i = 0; i < 512; i++)
		{
			if (m_Trigger[i].m_Id != -1)
			{
				Vector2 position = m_Trigger[i].m_Position;
				((Vector3)(ref position2))._002Ector(position.X, position.Y, 0f);
				Vector3 val = Program.m_CameraManager3D.WorldToScreen(position2);
				position.X = val.X;
				position.Y = val.Y;
				if (m_Trigger[i].m_Id == highlightId)
				{
					Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SmallFont, "+", position, Color.White);
				}
				else
				{
					Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SmallFont, "+", position, Color.Yellow);
				}
			}
		}
	}

	public void UpdateEditor()
	{
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0184: Unknown result type (might be due to invalid IL or missing references)
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_018f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		switch (m_EditorState)
		{
		case 0:
			if (Program.m_App.Debounce((Keys)84))
			{
				m_EditorState = 2;
				m_EditorPos = Program.m_App.MousePosition();
				m_EditorPos3D = GetEditorPos3D(m_EditorPos);
			}
			if (Program.m_App.LeftButton() && FindTriggerNearMouse())
			{
				m_EditorState = 5;
			}
			break;
		case 2:
		{
			int num = TestNumberKeys();
			if (num > -1 && num < 10)
			{
				m_EditorTriggerType = num * 100;
				m_EditorState = 3;
			}
			break;
		}
		case 3:
		{
			int num2 = TestNumberKeys();
			if (num2 > -1 && num2 < 10)
			{
				m_EditorTriggerType += num2 * 10;
				m_EditorState = 4;
			}
			break;
		}
		case 4:
		{
			int num3 = TestNumberKeys();
			if (num3 > -1 && num3 < 10)
			{
				m_EditorTriggerType += num3;
				Create(m_EditorTriggerType, m_EditorPos3D, 0f);
				m_EditorState = 0;
			}
			break;
		}
		case 5:
		{
			Vector2 zero = Vector2.Zero;
			zero.X = ((MouseState)(ref Program.m_App.m_MouseState)).X - ((MouseState)(ref Program.m_App.m_OldMouseState)).X;
			zero.Y = -(((MouseState)(ref Program.m_App.m_MouseState)).Y - ((MouseState)(ref Program.m_App.m_OldMouseState)).Y);
			Trigger obj = m_Trigger[m_EditorTriggerSelectedId];
			obj.m_Position += zero * 0.01f;
			Vector2 zero2 = Vector2.Zero;
			zero2.X = 100f;
			Program.m_DebugManager.DEBUG_DrawText($"X={m_Trigger[m_EditorTriggerSelectedId].m_Position.X} Y={m_Trigger[m_EditorTriggerSelectedId].m_Position.Y}", zero2, bDraw: false);
			if (((KeyboardState)(ref Program.m_App.m_KeyboardState)).IsKeyDown((Keys)190))
			{
				m_Trigger[m_EditorTriggerSelectedId].m_Rotation -= 0.01f;
			}
			else if (((KeyboardState)(ref Program.m_App.m_KeyboardState)).IsKeyDown((Keys)188))
			{
				m_Trigger[m_EditorTriggerSelectedId].m_Rotation += 0.01f;
			}
			if (Program.m_App.LeftButton())
			{
				m_EditorTriggerSelectedId = -1;
				m_EditorState = 0;
			}
			if (((KeyboardState)(ref Program.m_App.m_KeyboardState)).IsKeyDown((Keys)68))
			{
				Delete(m_EditorTriggerSelectedId);
				Program.m_ItemManager.DeleteByTriggerId(m_EditorTriggerSelectedId);
				m_EditorState = 0;
			}
			break;
		}
		case 1:
			break;
		}
	}

	public Vector2 GetEditorPos3D(Vector2 p)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Vector2 zero = Vector2.Zero;
		Vector3 zero2 = Vector3.Zero;
		Vector3 screenPos = Program.m_CameraManager3D.WorldToScreen(zero2);
		screenPos.X = p.X;
		screenPos.Y = p.Y;
		screenPos = Player.ScreenToWorld(screenPos);
		zero.X = screenPos.X;
		zero.Y = screenPos.Y;
		return zero;
	}

	public int TestNumberKeys()
	{
		if (Program.m_App.Debounce((Keys)48))
		{
			return 0;
		}
		if (Program.m_App.Debounce((Keys)49))
		{
			return 1;
		}
		if (Program.m_App.Debounce((Keys)50))
		{
			return 2;
		}
		if (Program.m_App.Debounce((Keys)51))
		{
			return 3;
		}
		if (Program.m_App.Debounce((Keys)52))
		{
			return 4;
		}
		if (Program.m_App.Debounce((Keys)53))
		{
			return 5;
		}
		if (Program.m_App.Debounce((Keys)54))
		{
			return 6;
		}
		if (Program.m_App.Debounce((Keys)55))
		{
			return 7;
		}
		if (Program.m_App.Debounce((Keys)56))
		{
			return 8;
		}
		if (Program.m_App.Debounce((Keys)57))
		{
			return 9;
		}
		return -1;
	}

	public bool FindTriggerNearMouse()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Vector2.Zero;
		for (int i = 0; i < 512; i++)
		{
			if (m_Trigger[i].m_Id != -1)
			{
				Vector2 editorPos3D = GetEditorPos3D(Program.m_App.MousePosition());
				val = m_Trigger[i].m_Position - editorPos3D;
				float num = ((Vector2)(ref val)).LengthSquared();
				if (num < 1f)
				{
					m_EditorTriggerSelectedId = i;
					return true;
				}
			}
		}
		return false;
	}

	public int FindTriggerNearestPointer()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		int result = -1;
		float num = 1E+10f;
		Vector2 val = Vector2.Zero;
		for (int i = 0; i < 512; i++)
		{
			if (m_Trigger[i].m_Id != -1)
			{
				Vector2 editorPos3D = GetEditorPos3D(Program.m_App.m_LevelEditor.m_Pointer);
				val = m_Trigger[i].m_Position - editorPos3D;
				float num2 = ((Vector2)(ref val)).LengthSquared();
				if (num2 < 25f && num2 < num)
				{
					result = i;
					num = num2;
				}
			}
		}
		return result;
	}

	public void Save()
	{
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		for (int i = 0; i < 512; i++)
		{
			if (m_Trigger[i].m_Id != -1)
			{
				num++;
			}
		}
		string path = $"Levels\\level{Program.m_App.m_Level}.dat";
		path = Path.Combine("C:\\XNA\\Bike2\\Platformer1\\HighResolutionContent", path);
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Write);
		BinaryWriter binaryWriter = new BinaryWriter(fileStream);
		binaryWriter.Write(num);
		_ = Vector2.Zero;
		for (int j = 0; j < 512; j++)
		{
			if (m_Trigger[j].m_Id != -1)
			{
				binaryWriter.Write(m_Trigger[j].m_Type);
				binaryWriter.Write((int)(m_Trigger[j].m_Position.X * 1000f));
				binaryWriter.Write((int)(m_Trigger[j].m_Position.Y * 1000f));
				binaryWriter.Write((int)(m_Trigger[j].m_Rotation * 1000f));
			}
		}
		fileStream.Close();
	}

	public void Load(StreamReader inFile)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		string path = $"Content\\Levels\\level{Program.m_App.m_Level}.dat";
		FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
		if (fileStream.Length != 0)
		{
			BinaryReader binaryReader = new BinaryReader(fileStream);
			int num = binaryReader.ReadInt32();
			Vector2 zero = Vector2.Zero;
			float num2 = 0f;
			for (int i = 0; i < num; i++)
			{
				int type = binaryReader.ReadInt32();
				int num3 = binaryReader.ReadInt32();
				zero.X = (float)num3 * 0.001f;
				int num4 = binaryReader.ReadInt32();
				zero.Y = (float)num4 * 0.001f;
				int num5 = binaryReader.ReadInt32();
				num2 = (float)num5 * 0.001f;
				Create(type, zero, num2);
			}
			fileStream.Close();
		}
	}

	public void Copy(Trigger[] src, Trigger[] dest)
	{
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 512; i++)
		{
			dest[i].m_Flags = src[i].m_Flags;
			dest[i].m_Id = src[i].m_Id;
			dest[i].m_LinkActor = src[i].m_LinkActor;
			dest[i].m_Position = src[i].m_Position;
			dest[i].m_State = src[i].m_State;
			dest[i].m_Type = src[i].m_Type;
		}
	}

	public void CalcDebugStats()
	{
		if (!Program.m_App.m_bEditor)
		{
			return;
		}
		m_SuspendedCount = 0;
		m_ActiveCount = 0;
		m_CompletedCount = 0;
		m_TotalCount = 0;
		for (int i = 0; i < 512; i++)
		{
			if (m_Trigger[i].m_Id != -1)
			{
				m_TotalCount++;
				if (m_Trigger[i].m_State == Trigger.TRIGGER_STATE.SUSPENDED)
				{
					m_SuspendedCount++;
				}
				else if (m_Trigger[i].m_State == Trigger.TRIGGER_STATE.ACTIVE)
				{
					m_ActiveCount++;
				}
				else if (m_Trigger[i].m_State == Trigger.TRIGGER_STATE.COMPLETED)
				{
					m_CompletedCount++;
				}
			}
		}
	}
}
