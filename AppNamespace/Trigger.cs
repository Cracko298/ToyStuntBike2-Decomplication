using Microsoft.Xna.Framework;

namespace AppNamespace;

public class Trigger
{
	public enum OBJ
	{
		TOYBLOCK_A = 0,
		BOOK_LARGE_25DEG = 1,
		CHAIR1 = 2,
		PENCIL = 3,
		TOYBLOCK_B = 4,
		TOYBLOCK_C = 5,
		TOYBLOCK_A_STATIC = 6,
		TOYBLOCK_B_STATIC = 7,
		TOYBLOCK_C_STATIC = 8,
		RULER = 9,
		BUS = 10,
		CHESSBOARD = 11,
		DRAWINGPIN = 12,
		START = 13,
		FINISH = 14,
		SWORD = 15,
		SWORD2 = 16,
		SHIELD = 17,
		SMALLTABLE = 18,
		BOOK2 = 19,
		SKISLOPE = 20,
		FLAG1 = 21,
		FLAG2 = 22,
		FLAG3 = 23,
		SKISLOPE_REVERSE = 24,
		TRACK_1M = 25,
		TRACK_50CM = 26,
		TRACK_1M_UP_60 = 27,
		TRACK_1M_DOWN_60 = 28,
		TRACK_50CM_UP_30 = 29,
		TRACK_50CM_DOWN_30 = 30,
		TRACK_1M_LOOP = 31,
		DOMINO_1 = 32,
		DOMINO_2 = 33,
		DOMINO_3 = 34,
		DOMINO_4 = 35,
		CHAIR2 = 36,
		BOOK3 = 37,
		HOB = 38,
		MICROWAVE = 39,
		PLACEMAT = 40,
		KNIFE = 41,
		COFFEE = 42,
		TEA = 43,
		SUGAR = 44,
		BISCUITS = 45,
		BOWL = 46,
		TOASTER = 47,
		SPOON = 48,
		MUG = 49,
		PLATE = 50,
		ROLLINGPIN = 51,
		SPADE = 52,
		BALL = 53,
		GARDENCHAIR = 54,
		PLANK_1M = 55,
		PLANK_50CM = 56,
		BRICK = 57,
		RAKE = 58,
		PLANTPOT = 59,
		PLANK2_1M = 60,
		PLANK2_50CM = 61,
		CHECKPOINT = 62,
		LCDSCREEN = 63,
		KEYBOARD = 64,
		LAPTOP = 65,
		SODA = 66,
		PIZZA = 67,
		GAMEBOXBLOCK = 68,
		GAMEBOX = 69,
		GAMEBOX_DYN = 70,
		PHONE = 71,
		PHONE_DYN = 72,
		DVDSTACK = 73,
		DIARY = 74,
		DICE = 75,
		DICE_DYN = 76,
		BOOK4 = 77,
		PC = 78,
		SPEAKER = 79,
		PEN = 80,
		PEN_DYN = 81,
		STORAGEBOX = 82,
		PRINTER = 83,
		BOOK5 = 84,
		BOOK6 = 85,
		BOOK7 = 86,
		BATH = 87,
		TOILET_CLOSED = 88,
		TOOTHBRUSH = 89,
		TOOTHBRUSH2 = 90,
		TOOTHBRUSH3 = 91,
		TUMBLER = 92,
		BIN = 93,
		CLOTHESHORSE = 94,
		FACEWASH_DYN = 95,
		BABY_POWDER_DYN = 96,
		TOILET_OPEN = 97,
		TOOTHPASTE = 98,
		SHAMPOO_DYN = 99,
		SINK = 100,
		SOFA = 101,
		ARMCHAIR = 102,
		COFFEETABLE = 103,
		LAMP = 104,
		REMOTE = 105,
		TVSTAND = 106,
		WINE = 107,
		VASE = 108,
		BOX = 109,
		EXTENSION = 110,
		EXTENSION_DYN = 111,
		PLUG2 = 112,
		SND_BEEP = 500,
		SND_FLUSH = 501,
		END = 502
	}

	public enum TRIGGER_STATE
	{
		SUSPENDED,
		ACTIVE,
		COMPLETED
	}

	public const int FLAG_ONCE = 1;

	public const int FLAG_OFFSCREEN = 2;

	public const int FLAG_RENDER = 4;

	public const int FLAG_UPDATE = 8;

	public int m_Id;

	public int m_Type;

	public Actor m_LinkActor;

	public TRIGGER_STATE m_State;

	public int m_Flags;

	public Vector2 m_Position;

	public bool m_bDistanceTrigger;

	public float m_Rotation;

	public Trigger()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		m_Id = -1;
		m_Type = -1;
		m_LinkActor = null;
		m_State = TRIGGER_STATE.SUSPENDED;
		m_Flags = 0;
		m_Position = Vector2.Zero;
		m_bDistanceTrigger = true;
		m_Rotation = 0f;
	}

	public void Activate()
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_App.m_bSplitScreen && (m_Type == 21 || m_Type == 22 || m_Type == 23))
		{
			m_State = TRIGGER_STATE.COMPLETED;
			return;
		}
		m_State = TRIGGER_STATE.ACTIVE;
		Program.m_ItemManager.Create(m_Type, m_Id, m_Position, m_Rotation);
		m_State = TRIGGER_STATE.COMPLETED;
	}

	public void Suspend()
	{
	}

	public void Complete()
	{
	}

	public void Update()
	{
	}

	public void Render()
	{
	}

	public void SetOffScreen()
	{
		m_Flags |= 2;
	}

	public bool OffScreen(float fTol)
	{
		return false;
	}
}
