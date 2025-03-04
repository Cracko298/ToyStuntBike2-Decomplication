using System;
using System.IO;
using EasyStorage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace AppNamespace;

public class LevelEditor
{
	private enum State
	{
		NORMAL,
		OBJECT_SELECTED,
		MENU_ACTIVE,
		SHOW_HELP
	}

	private enum Icons
	{
		TOP_LEVEL,
		SELECT_OPTIONS,
		SELECT_BACKDROP,
		SELECT_BACKYARD,
		SELECT_BATHROOM,
		SELECT_BEDROOM,
		SELECT_DESK,
		SELECT_KITCHEN,
		SELECT_LIVINGROOM,
		TOP_LEVEL_END,
		_BACKDROP_SET,
		BACKDROP_BACKYARD,
		BACKDROP_BATHROOM,
		BACKDROP_BEDROOM,
		BACKDROP_DESK,
		BACKDROP_KITCHEN,
		BACKDROP_LIVINGROOM,
		BACKDROP7,
		BACKDROP8,
		_BACKDROP_SET_END,
		_BEDROOM_SET1,
		TOYBLOCK_A,
		BOOK_LARGE_25DEG,
		CHAIR1,
		PENCIL,
		TOYBLOCK_B,
		TOYBLOCK_C,
		TOYBLOCK_A_STATIC,
		TOYBLOCK_B_STATIC,
		BEDROOM_SET1_END,
		_BEDROOM_SET2,
		TOYBLOCK_C_STATIC,
		RULER,
		BUS,
		CHESSBOARD,
		DRAWINGPIN,
		SWORD,
		SHIELD,
		EMPTY1,
		BEDROOM_SET2_END,
		_BEDROOM_SET3,
		TRACK_1M,
		TRACK_50CM,
		TRACK_1M_UP_60,
		TRACK_1M_DOWN_60,
		TRACK_50CM_UP_30,
		TRACK_50CM_DOWN_30,
		SMALLTABLE,
		BOOK2,
		BEDROOM_SET3_END,
		_BACKYARD_SET1,
		SPADE,
		BALL,
		GARDENCHAIR,
		PLANK_1M,
		PLANK_50CM,
		BRICK,
		RAKE,
		PLANTPOT,
		BACKYARD_SET1_END,
		_BATHROOM_SET1,
		BATH,
		TOILET_CLOSED,
		TOOTHBRUSH,
		TOOTHBRUSH2,
		TOOTHBRUSH3,
		TUMBLER,
		BIN,
		CLOTHESHORSE,
		BATHROOM_SET1_END,
		_BATHROOM_SET2,
		FACEWASH_DYN,
		BABY_POWDER_DYN,
		TOILET_OPEN,
		TOOTHPASTE,
		SHAMPOO_DYN,
		SINK,
		BATHROOM_7,
		BATHROOM_8,
		BATHROOM_SET2_END,
		_DESK_SET1,
		LCDSCREEN,
		KEYBOARD,
		LAPTOP,
		SODA,
		PIZZA,
		GAMEBOXBLOCK,
		GAMEBOX,
		GAMEBOX_DYN,
		DESK_SET1_END,
		_DESK_SET2,
		PHONE,
		PHONE_DYN,
		DVDSTACK,
		DIARY,
		DICE,
		DICE_DYN,
		BOOK4,
		PC,
		DESK_SET2_END,
		_DESK_SET3,
		SPEAKER,
		PEN,
		PEN_DYN,
		STORAGEBOX,
		PRINTER,
		BOOK5,
		BOOK6,
		BOOK7,
		DESK_SET3_END,
		_KITCHEN_SET1,
		HOB,
		MICROWAVE,
		PLACEMAT,
		KNIFE,
		COFFEE,
		TEA,
		SUGAR,
		BISCUITS,
		KITCHEN_SET1_END,
		_KITCHEN_SET2,
		BOWL,
		TOASTER,
		SPOON,
		MUG,
		PLATE,
		ROLLINGPIN,
		DOMINO_1,
		DOMINO_2,
		KITCHEN_SET2_END,
		_LIVINGROOM_SET1,
		SOFA,
		ARMCHAIR,
		COFFEETABLE,
		LAMP,
		REMOTE,
		TVSTAND,
		WINE,
		VASE,
		LIVINGROOM_SET1_END,
		_LIVINGROOM_SET2,
		BOX,
		EXTENSION,
		EXTENSION_DYN,
		PLUG2,
		LVINGROOM5,
		LVINGROOM6,
		LVINGROOM7,
		LVINGROOM8,
		LIVINGROOM_SET2_END,
		_OPTIONS_SET,
		HELP,
		PLAY_LEVEL,
		SAVE_LEVEL,
		EXIT_LEVEL,
		CHECKPOINT,
		SHARE,
		OPTIONS_7,
		OPTIONS_8,
		OPTIONS_END,
		END
	}

	public class CIconHelp
	{
		public string m_Title;

		public string m_Help;

		public Trigger.OBJ m_TriggerId;

		public CIconHelp(string t, string h, Trigger.OBJ id)
		{
			m_Title = t;
			m_Help = h;
			m_TriggerId = id;
		}
	}

	public const int MAX_COST = 100;

	private const int ERROR_FRAMES = 240;

	private App m_App;

	public float m_ValueRepeatTime;

	public Vector2 m_Pointer = new Vector2(640f, 360f);

	private int m_TriggerSelectedId = -1;

	private State m_State;

	private Icons m_MenuState;

	private int m_IconSelected = -1;

	private bool m_bZoom;

	private bool m_Highlight;

	private int m_HighlightId = -1;

	private int m_LevelVersion;

	private int m_LevelBackground = 3;

	private int m_ObjectCost;

	public Texture2D[] m_Icon;

	private int m_Error;

	private int m_ErrorFrame;

	public bool m_bEditExisting;

	public CIconHelp[] IconHelp = new CIconHelp[160]
	{
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Options", "                            Options", Trigger.OBJ.BALL),
		new CIconHelp("Back Drops", "       Choose a back drop for your level", Trigger.OBJ.BALL),
		new CIconHelp("Backyard", "       Create objects from the Backyard set", Trigger.OBJ.BALL),
		new CIconHelp("Bathroom", "       Create objects from the Bathroom set", Trigger.OBJ.BALL),
		new CIconHelp("Bedroom", "       Create objects from the Bedroom set", Trigger.OBJ.BALL),
		new CIconHelp("Desk", "       Create objects from the Desk set", Trigger.OBJ.BALL),
		new CIconHelp("Kitchen", "       Create objects from the Kitchen set", Trigger.OBJ.BALL),
		new CIconHelp("LivingRoom", "       Create objects from the Living Room set", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Backyard", "        A backyard background for your level", Trigger.OBJ.BALL),
		new CIconHelp("Bathroom", "  A bathroom floor background for your level", Trigger.OBJ.BALL),
		new CIconHelp("Bedroom", "   A bedroom floor background for your level", Trigger.OBJ.BALL),
		new CIconHelp("Desk", "           An desk background for your level", Trigger.OBJ.BALL),
		new CIconHelp("Kitchen", " A kitchen worktop background for your level", Trigger.OBJ.BALL),
		new CIconHelp("Living Room", "A living room floor background for your level", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Building Block", "              A dynamic toy building block", Trigger.OBJ.TOYBLOCK_A),
		new CIconHelp("Book", "                    A book, used as a ramp", Trigger.OBJ.BOOK_LARGE_25DEG),
		new CIconHelp("Chair", "   A large chair, can ride under or on top", Trigger.OBJ.CHAIR1),
		new CIconHelp("Pencil", "                            A fixed pencil", Trigger.OBJ.PENCIL),
		new CIconHelp("Building Block", "              A dynamic toy building block", Trigger.OBJ.TOYBLOCK_B),
		new CIconHelp("Building Block", "              A dynamic toy building block", Trigger.OBJ.TOYBLOCK_C),
		new CIconHelp("Building Block", "                A fixed toy building block", Trigger.OBJ.TOYBLOCK_A_STATIC),
		new CIconHelp("Building Block", "                A fixed toy building block", Trigger.OBJ.TOYBLOCK_B_STATIC),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Building Block", "               A dynamic toy building block", Trigger.OBJ.TOYBLOCK_C_STATIC),
		new CIconHelp("Ruler", "              A fixed ruler, used as a ramp", Trigger.OBJ.RULER),
		new CIconHelp("Toy Bus", "                   A fixed toy bus obstacle", Trigger.OBJ.BUS),
		new CIconHelp("Chessboard", "         A fixed chessboard, used as a ramp", Trigger.OBJ.CHESSBOARD),
		new CIconHelp("Tacks", "                     Hazard: A row of tacks", Trigger.OBJ.DRAWINGPIN),
		new CIconHelp("Toy Sword", "          A fixed toy sword, used as a ramp", Trigger.OBJ.SWORD),
		new CIconHelp("Toy Shield", "         A fixed toy shield, used as a ramp", Trigger.OBJ.SHIELD),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Track (1m)", "       A fixed piece of toy race track (1m)", Trigger.OBJ.TRACK_1M),
		new CIconHelp("Track (50cm)", "     A fixed piece of toy race track (50cm)", Trigger.OBJ.TRACK_50CM),
		new CIconHelp("Track Up (1m)", "      A fixed 1m curve up of toy race track", Trigger.OBJ.TRACK_1M_UP_60),
		new CIconHelp("Track Down (1m)", "    A fixed 1m curve down of toy race track", Trigger.OBJ.TRACK_1M_DOWN_60),
		new CIconHelp("Track Up (50cm)", "    A fixed 50cm curve up of toy race track", Trigger.OBJ.TRACK_50CM_UP_30),
		new CIconHelp("Track Down (50cm)", "  A fixed 50cm curve down of toy race track", Trigger.OBJ.TRACK_50CM_DOWN_30),
		new CIconHelp("Table", "  A fixed table, can go under or ride on top", Trigger.OBJ.SMALLTABLE),
		new CIconHelp("Book", "                A fixed book, used as a ramp", Trigger.OBJ.BOOK2),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Spade", "                        A fixed garden spade", Trigger.OBJ.SPADE),
		new CIconHelp("Ball", "                              A dynamic ball", Trigger.OBJ.BALL),
		new CIconHelp("Chair", "     A large chair, can ride under or on top", Trigger.OBJ.GARDENCHAIR),
		new CIconHelp("Plank (1m)", "              A fixed plank of wood (1m)", Trigger.OBJ.PLANK_1M),
		new CIconHelp("Plank (50cm)", "                A fixed plank of wood (50cm)", Trigger.OBJ.PLANK_50CM),
		new CIconHelp("Brick", "                               A fixed brick", Trigger.OBJ.BRICK),
		new CIconHelp("Rake", "                               Hazard: A garden rake", Trigger.OBJ.RAKE),
		new CIconHelp("Plant Pot", "                               A fixed plant pot", Trigger.OBJ.PLANTPOT),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Bath", "                                A fixed bath", Trigger.OBJ.BATH),
		new CIconHelp("Toilet (Closed)", "                     A fixed toilet (closed)", Trigger.OBJ.TOILET_CLOSED),
		new CIconHelp("Toothbrush", "                          A fixed toothbrush", Trigger.OBJ.TOOTHBRUSH),
		new CIconHelp("Toothbrush", "                          A fixed toothbrush", Trigger.OBJ.TOOTHBRUSH2),
		new CIconHelp("Toothbrush", "                          A fixed toothbrush", Trigger.OBJ.TOOTHBRUSH3),
		new CIconHelp("Tumbler", "                             A fixed tumbler", Trigger.OBJ.TUMBLER),
		new CIconHelp("Bin", "                                 A fixed bin", Trigger.OBJ.BIN),
		new CIconHelp("Clothes Horse", "                       A fixed clothes horse", Trigger.OBJ.CLOTHESHORSE),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Facewash", "                A dynamic bottle of facewash", Trigger.OBJ.FACEWASH_DYN),
		new CIconHelp("baby Powder", "             A dynamic bottle of Baby Powder", Trigger.OBJ.BABY_POWDER_DYN),
		new CIconHelp("Toilet (Open)", "                       A fixed toilet (open)", Trigger.OBJ.TOILET_OPEN),
		new CIconHelp("Toothpaste", "                  A fixed tube of toothbrush", Trigger.OBJ.TOOTHPASTE),
		new CIconHelp("Shampoo", "                 A dynamic bottle of shampoo", Trigger.OBJ.SHAMPOO_DYN),
		new CIconHelp("Sink", "                                A fixed sink", Trigger.OBJ.SINK),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("LCD", "                          A fixed LCD screen", Trigger.OBJ.LCDSCREEN),
		new CIconHelp("Keyboard", "                            A fixed keyboard", Trigger.OBJ.KEYBOARD),
		new CIconHelp("Laptop", "                              A fixed laptop", Trigger.OBJ.LAPTOP),
		new CIconHelp("Soda", "                       A dynamic can of soda", Trigger.OBJ.SODA),
		new CIconHelp("Pizza Box", "                         A dynamic pizza box", Trigger.OBJ.PIZZA),
		new CIconHelp("Game Boxes", "                           Static game boxes", Trigger.OBJ.GAMEBOXBLOCK),
		new CIconHelp("Game Box", "                            A fixed game box", Trigger.OBJ.GAMEBOX),
		new CIconHelp("Game Box", "                          A dynamic game box", Trigger.OBJ.GAMEBOX_DYN),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Phone", "                               A fixed phone", Trigger.OBJ.PHONE),
		new CIconHelp("Phone", "                             A dynamic phone", Trigger.OBJ.PHONE_DYN),
		new CIconHelp("DVDs", "                           A fixed DVD stack", Trigger.OBJ.DVDSTACK),
		new CIconHelp("Diary", "                               A fixed diary", Trigger.OBJ.DIARY),
		new CIconHelp("Desk Dice", "                           A fixed desk dice", Trigger.OBJ.DICE),
		new CIconHelp("Desk Dice", "                         A dynamic desk dice", Trigger.OBJ.DICE_DYN),
		new CIconHelp("Book", "                                A fixed book", Trigger.OBJ.BOOK4),
		new CIconHelp("PC", "                                  A fixed PC", Trigger.OBJ.PC),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Speaker", "                             A fixed speaker", Trigger.OBJ.SPEAKER),
		new CIconHelp("Pen", "                                 A fixed pen", Trigger.OBJ.PEN),
		new CIconHelp("Pen", "                               A dynamic pen", Trigger.OBJ.PEN_DYN),
		new CIconHelp("Storage Box", "                         A fixed storage box", Trigger.OBJ.STORAGEBOX),
		new CIconHelp("Printer", "                             A fixed printer", Trigger.OBJ.PRINTER),
		new CIconHelp("Book", "                                A fixed book", Trigger.OBJ.BOOK5),
		new CIconHelp("Book", "                                A fixed book", Trigger.OBJ.BOOK6),
		new CIconHelp("Book", "                                A fixed book", Trigger.OBJ.BOOK7),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Hob", "                       Hazard: A kitchen hob", Trigger.OBJ.HOB),
		new CIconHelp("Microwave", "                           A fixed Microwave", Trigger.OBJ.MICROWAVE),
		new CIconHelp("Placemat", "                            A fixed placemat", Trigger.OBJ.PLACEMAT),
		new CIconHelp("Knife", "                       A fixed kitchen knife", Trigger.OBJ.KNIFE),
		new CIconHelp("Coffee", "                       A fixed tin of coffee", Trigger.OBJ.COFFEE),
		new CIconHelp("Tea", "                          A fixed tin of tea", Trigger.OBJ.TEA),
		new CIconHelp("Sugar", "                        A fixed tin of sugar", Trigger.OBJ.SUGAR),
		new CIconHelp("Buiscuits", "                     A fixed tin of biscuits", Trigger.OBJ.BISCUITS),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Bowl", "                                A fixed bowl", Trigger.OBJ.BOWL),
		new CIconHelp("Toaster", "                             A fixed toaster", Trigger.OBJ.TOASTER),
		new CIconHelp("Wooden Spoon", "                        A fixed wooden spoon", Trigger.OBJ.SPOON),
		new CIconHelp("Cup", "                                 A fixed cup", Trigger.OBJ.MUG),
		new CIconHelp("Plate", "                               A fixed plate", Trigger.OBJ.PLATE),
		new CIconHelp("Rolling Pin", "                       A dynamic rolling pin", Trigger.OBJ.ROLLINGPIN),
		new CIconHelp("Domino", "                            A dynamic domino", Trigger.OBJ.DOMINO_1),
		new CIconHelp("Domino", "                            A dynamic domino", Trigger.OBJ.DOMINO_2),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Sofa", "                          A large fixed sofa", Trigger.OBJ.SOFA),
		new CIconHelp("Armchair", "                      A large fixed armchair", Trigger.OBJ.ARMCHAIR),
		new CIconHelp("Coffee Table", "                  A large fixed coffee table", Trigger.OBJ.COFFEETABLE),
		new CIconHelp("Lamp", "                A non-interactive table lamp", Trigger.OBJ.LAMP),
		new CIconHelp("TV Remote", "                           A fixed TV remote", Trigger.OBJ.REMOTE),
		new CIconHelp("TV Stand", "                            A fixed TV stand", Trigger.OBJ.TVSTAND),
		new CIconHelp("Wine Bottle", "                         A fixed wine bottle", Trigger.OBJ.WINE),
		new CIconHelp("Vase", "                                A fixed vase", Trigger.OBJ.VASE),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Wooden Box", "                    A fixed camphor wood box", Trigger.OBJ.BOX),
		new CIconHelp("Extension", "                     A fixed extension block", Trigger.OBJ.EXTENSION),
		new CIconHelp("Extension", "                   A dynamic extension block", Trigger.OBJ.EXTENSION_DYN),
		new CIconHelp("Plug", "                                Hazard: Plug", Trigger.OBJ.PLUG2),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("Help", "                 Some tips on level creation", Trigger.OBJ.BALL),
		new CIconHelp("Play Level", " Play your level (this will also save first)", Trigger.OBJ.BALL),
		new CIconHelp("Save Level", "                                  Save Level", Trigger.OBJ.BALL),
		new CIconHelp("Exit Level", "                         Exit without saving", Trigger.OBJ.BALL),
		new CIconHelp("Checkpoint", "              Add a checkpoint to your level", Trigger.OBJ.CHECKPOINT),
		new CIconHelp("Live Share", " Share your level with XBox Live Players!", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL),
		new CIconHelp("", "", Trigger.OBJ.BALL)
	};

	public LevelEditor(App app)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		m_App = app;
		m_Icon = (Texture2D[])(object)new Texture2D[160];
	}

	protected void LoadEditorContent()
	{
		m_Icon[2] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/icon_select_backdrop");
		m_Icon[3] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/icon_select_backyard");
		m_Icon[4] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_select_bathroom");
		m_Icon[5] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_select_bedroom");
		m_Icon[6] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_select_desk");
		m_Icon[7] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_select_kitchen");
		m_Icon[8] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_select_livingroom");
		m_Icon[1] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_select_options");
		m_Icon[11] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/icon_select_backyard");
		m_Icon[12] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_select_bathroom");
		m_Icon[13] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_select_bedroom");
		m_Icon[14] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_select_desk");
		m_Icon[15] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_select_kitchen");
		m_Icon[16] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_select_livingroom");
		m_Icon[21] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_toyblock_a_dyn");
		m_Icon[22] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/icon_book1");
		m_Icon[23] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_chair1");
		m_Icon[24] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_pencil");
		m_Icon[25] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_toyblock_b_dyn");
		m_Icon[26] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_toyblock_c_dyn");
		m_Icon[27] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_toyblock_a");
		m_Icon[28] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_toyblock_b");
		m_Icon[31] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_toyblock_c");
		m_Icon[32] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/icon_ruler");
		m_Icon[33] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_bus");
		m_Icon[34] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_chessboard");
		m_Icon[35] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_drawingpin");
		m_Icon[36] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_sword");
		m_Icon[37] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_shield");
		m_Icon[41] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_track");
		m_Icon[42] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/icon_track");
		m_Icon[43] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_trackup");
		m_Icon[44] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_trackdown");
		m_Icon[45] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_trackup");
		m_Icon[46] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_trackdown");
		m_Icon[47] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_smalltable");
		m_Icon[48] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_book2");
		m_Icon[51] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_spade");
		m_Icon[52] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/icon_ball");
		m_Icon[53] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_gardenchair");
		m_Icon[54] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_plank");
		m_Icon[55] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_plank");
		m_Icon[56] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_brick");
		m_Icon[57] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_rake");
		m_Icon[58] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_plantpot");
		m_Icon[111] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_hob");
		m_Icon[112] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/icon_microwave");
		m_Icon[113] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_placemat");
		m_Icon[114] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_knife");
		m_Icon[115] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_coffee");
		m_Icon[116] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_tea");
		m_Icon[117] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_sugar");
		m_Icon[118] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_biscuits");
		m_Icon[121] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_bowl");
		m_Icon[122] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_toaster");
		m_Icon[123] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_spoon");
		m_Icon[124] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_mug");
		m_Icon[125] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_plate");
		m_Icon[126] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_rollingpin");
		m_Icon[127] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_domino1");
		m_Icon[128] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_domino2");
		m_Icon[81] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_lcd");
		m_Icon[82] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_keyboard");
		m_Icon[83] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_laptop");
		m_Icon[84] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_soda");
		m_Icon[85] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_pizza");
		m_Icon[86] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_gameboxes");
		m_Icon[87] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_gamebox");
		m_Icon[88] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_gamebox_dyn");
		m_Icon[91] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_phone");
		m_Icon[92] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_phone_dyn");
		m_Icon[93] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_dvds");
		m_Icon[94] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_diary");
		m_Icon[95] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_dice");
		m_Icon[96] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_dice_dyn");
		m_Icon[97] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_book4");
		m_Icon[98] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_pc");
		m_Icon[101] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_speaker");
		m_Icon[102] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_pen");
		m_Icon[103] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_pen_dyn");
		m_Icon[104] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_storagebox");
		m_Icon[105] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_printer");
		m_Icon[106] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_book5");
		m_Icon[107] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_book6");
		m_Icon[108] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_book7");
		m_Icon[61] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_bath");
		m_Icon[62] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_toilet_closed");
		m_Icon[63] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_toothbrush1");
		m_Icon[64] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_toothbrush2");
		m_Icon[65] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_toothbrush3");
		m_Icon[66] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_tumbler");
		m_Icon[67] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_bin");
		m_Icon[68] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_clotheshorse");
		m_Icon[71] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_facewash");
		m_Icon[72] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_babypowder");
		m_Icon[73] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_toilet_open");
		m_Icon[74] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_toothpaste");
		m_Icon[75] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_shampoo");
		m_Icon[76] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_sink");
		m_Icon[131] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_sofa");
		m_Icon[132] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_armchair");
		m_Icon[133] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_coffeetable");
		m_Icon[134] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_lamp");
		m_Icon[135] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_remote");
		m_Icon[136] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_tvstand");
		m_Icon[137] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_wine");
		m_Icon[138] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_vase");
		m_Icon[141] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_box");
		m_Icon[142] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_extension");
		m_Icon[143] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_extension_dyn");
		m_Icon[144] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_plug");
		m_Icon[153] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_save");
		m_Icon[154] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_exit");
		m_Icon[152] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_play");
		m_Icon[151] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_help");
		m_Icon[155] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_checkpoint");
		m_Icon[156] = ((Game)Program.m_App).Content.Load<Texture2D>("icons/Icon_share");
		Program.m_App.m_BackgroundManager.m_Background[0].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background1"), null);
		Program.m_App.m_BackgroundManager.m_Background[1].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background1"), null);
		App.ChangeEffectUsedByModel(Program.m_App.m_BackgroundManager.m_Background[0].m_Model, Program.m_App.basicEffectShad);
	}

	public void Start()
	{
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		LoadEditorContent();
		m_ErrorFrame = 0;
		Program.m_TriggerManager.DeleteAll();
		Program.m_App.m_LevelPos = 5f;
		if (m_bEditExisting)
		{
			if (Program.m_App.m_LevelEditor.LoadLevel(Program.m_App.m_UserLevelName))
			{
				Program.m_ItemManager.DeleteAll();
				m_State = State.NORMAL;
				Program.m_App.m_Level = 1000;
				Program.m_App.m_bLevelEditor = true;
				Program.m_App.m_bInPlaytest = false;
				ChangeBackground(m_LevelBackground);
			}
			else
			{
				Program.m_App.m_NextState = App.STATE.MAINMENU;
				Program.m_App.StartFade(up: false);
			}
		}
		else
		{
			m_bEditExisting = false;
			Program.m_ItemManager.DeleteAll();
			Program.m_App.m_Level = 1000;
			Program.m_App.m_bLevelEditor = true;
			Program.m_App.m_bInPlaytest = false;
			Program.m_App.m_UserLevelName = null;
			Program.m_App.ReCreateWorld();
			Program.m_TriggerManager.Create(13, new Vector2(0f, 0f), 0f);
			Program.m_TriggerManager.Create(14, new Vector2(20f, 0f), 0f);
			m_State = State.NORMAL;
			m_LevelVersion = 0;
			m_LevelBackground = 3;
			ChangeBackground(m_LevelBackground);
			Program.m_App.m_MyLevelId++;
		}
	}

	public void Stop()
	{
		Program.m_App.m_bLevelEditor = false;
	}

	public void Update()
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_App.m_NextShareTime > 0)
		{
			Program.m_App.m_NextShareTime--;
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer() != null)
		{
			Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState = GamePad.GetState(m_App.m_PlayerOnePadId);
			switch (m_State)
			{
			case State.MENU_ACTIVE:
				HighlightIcons();
				CheckSubSets();
				break;
			case State.NORMAL:
				UpdateMouse();
				HighlightTriggerObjects();
				UpdateZoom();
				CheckJukeboxControls();
				break;
			case State.OBJECT_SELECTED:
				UpdateMouse();
				HighlightTriggerObjects();
				MoveObject();
				RotateObject();
				CheckDeleteObject();
				break;
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)4096))
			{
				OnSelectPressed();
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)8192))
			{
				OnBackPressed();
			}
			if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)32768))
			{
				OnMenuPressed();
			}
			Program.m_PlayerManager.GetPrimaryPlayer().m_OldGamepadState = Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState;
		}
		CalcObjectCost();
	}

	private void OnSelectPressed()
	{
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0343: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03db: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0264: Unknown result type (might be due to invalid IL or missing references)
		//IL_0269: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		if (m_App.Fading())
		{
			return;
		}
		switch (m_State)
		{
		case State.NORMAL:
			m_TriggerSelectedId = Program.m_TriggerManager.FindTriggerNearestPointer();
			if (m_TriggerSelectedId != -1)
			{
				if (Program.m_TriggerManager.m_Trigger[m_TriggerSelectedId].m_Type == 13)
				{
					m_Error = 123;
					m_ErrorFrame = 240;
					Program.m_SoundManager.Play(41);
					break;
				}
				Vector2 position = Program.m_TriggerManager.m_Trigger[m_TriggerSelectedId].m_Position;
				Vector3 position2 = default(Vector3);
				((Vector3)(ref position2))._002Ector(position.X, position.Y, 0f);
				Vector3 val = Program.m_CameraManager3D.WorldToScreen(position2);
				position.X = val.X;
				position.Y = val.Y;
				m_Pointer = position;
				m_State = State.OBJECT_SELECTED;
				Program.m_SoundManager.Play(2);
			}
			break;
		case State.OBJECT_SELECTED:
			if (m_TriggerSelectedId != -1)
			{
				m_TriggerSelectedId = -1;
				m_State = State.NORMAL;
				Program.m_SoundManager.Play(3);
			}
			break;
		case State.MENU_ACTIVE:
			if (m_IconSelected == -1)
			{
				break;
			}
			switch (m_MenuState)
			{
			case Icons.TOP_LEVEL:
				Program.m_SoundManager.Play(2);
				switch (m_MenuState + m_IconSelected)
				{
				case Icons.SELECT_OPTIONS:
					m_MenuState = Icons._BACKDROP_SET;
					break;
				case Icons.SELECT_BACKDROP:
					m_MenuState = Icons._BACKYARD_SET1;
					break;
				case Icons.SELECT_BACKYARD:
					m_MenuState = Icons._BATHROOM_SET1;
					break;
				case Icons.SELECT_BATHROOM:
					m_MenuState = Icons._BEDROOM_SET1;
					break;
				case Icons.SELECT_BEDROOM:
					m_MenuState = Icons._DESK_SET1;
					break;
				case Icons.SELECT_DESK:
					m_MenuState = Icons._KITCHEN_SET1;
					break;
				case Icons.SELECT_KITCHEN:
					m_MenuState = Icons._LIVINGROOM_SET1;
					break;
				case Icons.TOP_LEVEL:
					m_MenuState = Icons._OPTIONS_SET;
					break;
				}
				break;
			default:
				if (m_ObjectCost >= 100)
				{
					m_Error = 125;
					m_ErrorFrame = 240;
					Program.m_SoundManager.Play(41);
					m_State = State.NORMAL;
					break;
				}
				Program.m_SoundManager.Play(2);
				if (m_Icon[(int)(m_MenuState + m_IconSelected + 1)] != null)
				{
					Trigger.OBJ triggerId2 = IconHelp[(int)(m_MenuState + m_IconSelected + 1)].m_TriggerId;
					Vector2 editorPos3D2 = Program.m_TriggerManager.GetEditorPos3D(m_Pointer);
					Program.m_TriggerManager.Create((int)triggerId2, editorPos3D2, 0f);
				}
				m_State = State.NORMAL;
				break;
			case Icons._OPTIONS_SET:
				Program.m_SoundManager.Play(2);
				switch (m_MenuState + m_IconSelected)
				{
				case Icons.PLAY_LEVEL:
					SaveLevel();
					m_State = State.NORMAL;
					break;
				case Icons.SAVE_LEVEL:
					m_App.m_NextState = App.STATE.FRONTEND_EDITOR_MENU;
					m_App.StartFade(up: false);
					break;
				case Icons.HELP:
					if (SaveLevel())
					{
						Program.m_App.m_bInPlaytest = true;
						m_bEditExisting = true;
						m_App.m_NextState = App.STATE.CONTINUEGAME;
						m_App.StartFade(up: false);
						Program.m_SoundManager.Play(2);
						Program.m_PlayerManager.GetPrimaryPlayer().ResetBike(bForce: true, restartRace: true, Vector2.Zero);
						Program.m_PlayerManager.GetPrimaryPlayer().m_ReadySteadyTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + 4500f;
					}
					break;
				case Icons.EXIT_LEVEL:
					if (m_ObjectCost >= 100)
					{
						m_Error = 125;
						m_ErrorFrame = 240;
						Program.m_SoundManager.Play(41);
						m_State = State.NORMAL;
					}
					else
					{
						Trigger.OBJ triggerId = IconHelp[(int)(m_MenuState + m_IconSelected + 1)].m_TriggerId;
						Vector2 editorPos3D = Program.m_TriggerManager.GetEditorPos3D(m_Pointer);
						Program.m_TriggerManager.Create((int)triggerId, editorPos3D, 0f);
						m_State = State.NORMAL;
					}
					break;
				case Icons._OPTIONS_SET:
					m_State = State.SHOW_HELP;
					break;
				case Icons.CHECKPOINT:
					ShareLevel();
					break;
				}
				break;
			case Icons._BACKDROP_SET:
				Program.m_SoundManager.Play(2);
				switch (m_MenuState + m_IconSelected)
				{
				case Icons._BACKDROP_SET:
				case Icons.BACKDROP_BACKYARD:
				case Icons.BACKDROP_BATHROOM:
				case Icons.BACKDROP_BEDROOM:
				case Icons.BACKDROP_DESK:
				case Icons.BACKDROP_KITCHEN:
					m_LevelBackground = m_IconSelected;
					ChangeBackground(m_LevelBackground);
					m_State = State.NORMAL;
					break;
				default:
					m_State = State.NORMAL;
					break;
				}
				break;
			}
			break;
		}
	}

	private void ShareLevel()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		if (m_ObjectCost < 3)
		{
			m_Error = 126;
			m_ErrorFrame = 240;
			Program.m_SoundManager.Play(41);
			m_State = State.NORMAL;
		}
		else if (Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] == null || !Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId].IsSignedInToLive)
		{
			m_Error = 122;
			m_ErrorFrame = 240;
			Program.m_SoundManager.Play(41);
			m_State = State.NORMAL;
		}
		else if (Program.m_App.m_NextShareTime > 0)
		{
			m_Error = 127;
			m_ErrorFrame = 240;
			Program.m_SoundManager.Play(41);
			m_State = State.NORMAL;
		}
		else
		{
			if (!SaveLevel())
			{
				return;
			}
			if (!Guide.IsTrialMode && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] != null && Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId].IsSignedInToLive)
			{
				int version = 0;
				int background = 0;
				float rating = 3f;
				int ratingCount = 1;
				if (m_bEditExisting)
				{
					Program.m_App.m_SharedLevels.containsEntryForGamertag(0, Program.m_App.m_UserLevelName, out version, out background, out rating, out ratingCount);
				}
				LevelEntry entry = new LevelEntry(Program.m_App.m_UserLevelName, m_LevelVersion, m_LevelBackground, rating, ratingCount);
				Program.m_App.m_SharedLevels.addEntry(0, entry, Program.m_App.mSyncManager);
				Rating rating2 = new Rating();
				rating2.m_LevelName = Program.m_App.m_UserLevelName;
				rating2.m_Rating = rating;
				m_App.m_Ratings.m_Rating.Add(rating2);
			}
			Program.m_App.m_NextShareTime = 7200;
			m_Error = 131;
			m_ErrorFrame = 480;
			Program.m_LoadSaveManager.SaveGame();
			m_State = State.NORMAL;
		}
	}

	private void OnMenuPressed()
	{
		if (m_State == State.NORMAL)
		{
			m_State = State.MENU_ACTIVE;
			m_MenuState = Icons.TOP_LEVEL;
			Program.m_SoundManager.Play(2);
		}
	}

	private void OnBackPressed()
	{
		if (m_State == State.MENU_ACTIVE)
		{
			if (m_MenuState == Icons.TOP_LEVEL)
			{
				m_State = State.NORMAL;
				Program.m_SoundManager.Play(3);
			}
			else
			{
				m_MenuState = Icons.TOP_LEVEL;
			}
		}
		if (m_State == State.SHOW_HELP)
		{
			m_State = State.NORMAL;
			Program.m_SoundManager.Play(3);
		}
	}

	private void UpdateZoom()
	{
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)16384))
		{
			m_bZoom = !m_bZoom;
		}
		if (m_bZoom)
		{
			Program.m_CameraManager3D.m_CameraPositionTarget.Z = 25f;
		}
		else
		{
			Program.m_CameraManager3D.m_CameraPositionTarget.Z = 10f;
		}
	}

	private void UpdateMouse()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Invalid comparison between Unknown and I4
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Invalid comparison between Unknown and I4
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Invalid comparison between Unknown and I4
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Invalid comparison between Unknown and I4
		GamePadThumbSticks thumbSticks = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).ThumbSticks;
		float x = ((GamePadThumbSticks)(ref thumbSticks)).Left.X;
		GamePadThumbSticks thumbSticks2 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).ThumbSticks;
		float y = ((GamePadThumbSticks)(ref thumbSticks2)).Left.Y;
		x = Program.m_PlayerManager.GetPrimaryPlayer().DeadZone(x, 0.1f);
		y = Program.m_PlayerManager.GetPrimaryPlayer().DeadZone(y, 0.1f);
		bool flag = false;
		GamePadTriggers triggers = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).Triggers;
		if (((GamePadTriggers)(ref triggers)).Right > 0.8f)
		{
			flag = true;
		}
		GamePadDPad dPad = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad)).Right == 1)
		{
			x = 1f;
		}
		GamePadDPad dPad2 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad2)).Left == 1)
		{
			x = -1f;
		}
		GamePadDPad dPad3 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad3)).Up == 1)
		{
			y = 1f;
		}
		GamePadDPad dPad4 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad4)).Down == 1)
		{
			y = -1f;
		}
		bool flag2 = true;
		if (x < 0f && Program.m_App.m_LevelPos < 0f)
		{
			flag2 = false;
		}
		if (x > 0f && Program.m_App.m_LevelPos > 1000f)
		{
			flag2 = false;
		}
		if (y > 0f && Program.m_CameraManager3D.m_CameraPositionTarget.Y > 30f)
		{
			flag2 = false;
		}
		if (y < 0f && Program.m_CameraManager3D.m_CameraPositionTarget.Y < 0f)
		{
			flag2 = false;
		}
		if (flag2 && flag)
		{
			Program.m_App.m_LevelPos += x * 0.5f;
			ref Vector3 cameraPositionTarget = ref Program.m_CameraManager3D.m_CameraPositionTarget;
			cameraPositionTarget.Y += y * 0.5f;
			ref Vector3 cameraLookAtTarget = ref Program.m_CameraManager3D.m_CameraLookAtTarget;
			cameraLookAtTarget.Y += y * 0.5f;
			return;
		}
		ref Vector2 pointer = ref m_Pointer;
		pointer.X += x * 5f;
		ref Vector2 pointer2 = ref m_Pointer;
		pointer2.Y -= y * 5f;
		if (x < 0f && m_Pointer.X < 0f)
		{
			m_Pointer.X = 0f;
		}
		if (x > 0f && m_Pointer.X > 1278f)
		{
			m_Pointer.X = 1278f;
		}
		if (y > 0f && m_Pointer.Y < 0f)
		{
			m_Pointer.Y = 0f;
		}
		if (y < 0f && m_Pointer.Y > 718f)
		{
			m_Pointer.Y = 718f;
		}
	}

	private void HighlightTriggerObjects()
	{
		int num = Program.m_TriggerManager.FindTriggerNearestPointer();
		if (num != -1)
		{
			Item item = Program.m_ItemManager.FindByTriggerId(num);
			if (item != null)
			{
				item.m_FlashModel = 2;
			}
			m_Highlight = true;
			m_HighlightId = num;
		}
		else
		{
			m_Highlight = false;
			m_HighlightId = -1;
		}
	}

	private void MoveObject()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		if (m_TriggerSelectedId != -1)
		{
			_ = Vector2.Zero;
			Vector3 zero = Vector3.Zero;
			Vector2 position = Program.m_ItemManager.FindByTriggerId(m_TriggerSelectedId).m_Position;
			zero.X = position.X;
			zero.Y = position.Y;
			Vector3 screenPos = Program.m_CameraManager3D.WorldToScreen(zero);
			screenPos.X = m_Pointer.X;
			screenPos.Y = m_Pointer.Y;
			screenPos = Player.ScreenToWorld(screenPos);
			Program.m_TriggerManager.m_Trigger[m_TriggerSelectedId].m_Position.X = screenPos.X;
			Program.m_TriggerManager.m_Trigger[m_TriggerSelectedId].m_Position.Y = screenPos.Y;
			Program.m_ItemManager.UpdateAllFromTriggers();
		}
	}

	private void RotateObject()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Invalid comparison between Unknown and I4
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Invalid comparison between Unknown and I4
		float num = (float)Math.PI / 2f;
		if (m_TriggerSelectedId != -1)
		{
			_ = Vector2.Zero;
			GamePadButtons buttons = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).Buttons;
			if ((int)((GamePadButtons)(ref buttons)).LeftShoulder == 1 && Program.m_TriggerManager.m_Trigger[m_TriggerSelectedId].m_Rotation < num)
			{
				Program.m_TriggerManager.m_Trigger[m_TriggerSelectedId].m_Rotation += 0.005f;
			}
			GamePadButtons buttons2 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).Buttons;
			if ((int)((GamePadButtons)(ref buttons2)).RightShoulder == 1 && Program.m_TriggerManager.m_Trigger[m_TriggerSelectedId].m_Rotation > 0f - num)
			{
				Program.m_TriggerManager.m_Trigger[m_TriggerSelectedId].m_Rotation -= 0.005f;
			}
			Program.m_ItemManager.UpdateAllFromTriggers();
		}
	}

	private void CheckDeleteObject()
	{
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)8192) && m_TriggerSelectedId != -1)
		{
			if (Program.m_TriggerManager.m_Trigger[m_TriggerSelectedId].m_Type == 13 || Program.m_TriggerManager.m_Trigger[m_TriggerSelectedId].m_Type == 14)
			{
				m_Error = 121;
				m_ErrorFrame = 240;
				Program.m_SoundManager.Play(41);
			}
			else
			{
				Program.m_ItemManager.DeleteByTriggerId(m_TriggerSelectedId);
				Program.m_TriggerManager.Delete(m_TriggerSelectedId);
				m_TriggerSelectedId = -1;
				m_State = State.NORMAL;
			}
		}
	}

	private void HighlightIcons()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Invalid comparison between Unknown and I4
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Invalid comparison between Unknown and I4
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Invalid comparison between Unknown and I4
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Invalid comparison between Unknown and I4
		GamePadThumbSticks thumbSticks = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).ThumbSticks;
		Vector2 val = default(Vector2);
		val.X = ((GamePadThumbSticks)(ref thumbSticks)).Left.X;
		GamePadThumbSticks thumbSticks2 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).ThumbSticks;
		val.Y = ((GamePadThumbSticks)(ref thumbSticks2)).Left.Y;
		val.X = Program.m_PlayerManager.GetPrimaryPlayer().DeadZone(val.X, 0.25f);
		val.Y = Program.m_PlayerManager.GetPrimaryPlayer().DeadZone(val.Y, 0.25f);
		GamePadDPad dPad = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad)).Right == 1)
		{
			val.X = 1f;
		}
		GamePadDPad dPad2 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad2)).Left == 1)
		{
			val.X = -1f;
		}
		GamePadDPad dPad3 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad3)).Up == 1)
		{
			val.Y = 1f;
		}
		GamePadDPad dPad4 = ((GamePadState)(ref Program.m_PlayerManager.GetPrimaryPlayer().m_GamepadState)).DPad;
		if ((int)((GamePadDPad)(ref dPad4)).Down == 1)
		{
			val.Y = -1f;
		}
		if (val.X == 0f && val.Y == 0f)
		{
			m_IconSelected = -1;
			return;
		}
		float num = (float)Math.Atan2(val.X, val.Y);
		if (num < 0f)
		{
			num += (float)Math.PI * 2f;
		}
		int num2 = (int)MathHelper.Clamp(num / ((float)Math.PI / 4f) + 0.5f, 0f, 7f);
		if (num2 != m_IconSelected)
		{
			Program.m_SoundManager.Play(1);
		}
		m_IconSelected = num2;
	}

	private void CheckSubSets()
	{
		bool flag = Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)512);
		bool flag2 = Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)256);
		switch (m_MenuState)
		{
		case Icons._BEDROOM_SET1:
			if (flag)
			{
				m_MenuState = Icons._BEDROOM_SET2;
			}
			if (flag2)
			{
				m_MenuState = Icons._BEDROOM_SET3;
			}
			break;
		case Icons._BEDROOM_SET2:
			if (flag)
			{
				m_MenuState = Icons._BEDROOM_SET3;
			}
			if (flag2)
			{
				m_MenuState = Icons._BEDROOM_SET1;
			}
			break;
		case Icons._BEDROOM_SET3:
			if (flag)
			{
				m_MenuState = Icons._BEDROOM_SET1;
			}
			if (flag2)
			{
				m_MenuState = Icons._BEDROOM_SET2;
			}
			break;
		case Icons._KITCHEN_SET1:
			if (flag || flag2)
			{
				m_MenuState = Icons._KITCHEN_SET2;
			}
			break;
		case Icons._KITCHEN_SET2:
			if (flag || flag2)
			{
				m_MenuState = Icons._KITCHEN_SET1;
			}
			break;
		case Icons._DESK_SET1:
			if (flag)
			{
				m_MenuState = Icons._DESK_SET2;
			}
			if (flag2)
			{
				m_MenuState = Icons._DESK_SET3;
			}
			break;
		case Icons._DESK_SET2:
			if (flag)
			{
				m_MenuState = Icons._DESK_SET3;
			}
			if (flag2)
			{
				m_MenuState = Icons._DESK_SET1;
			}
			break;
		case Icons._DESK_SET3:
			if (flag)
			{
				m_MenuState = Icons._DESK_SET1;
			}
			if (flag2)
			{
				m_MenuState = Icons._DESK_SET2;
			}
			break;
		case Icons._BATHROOM_SET1:
			if (flag || flag2)
			{
				m_MenuState = Icons._BATHROOM_SET2;
			}
			break;
		case Icons._BATHROOM_SET2:
			if (flag || flag2)
			{
				m_MenuState = Icons._BATHROOM_SET1;
			}
			break;
		case Icons._LIVINGROOM_SET1:
			if (flag || flag2)
			{
				m_MenuState = Icons._LIVINGROOM_SET2;
			}
			break;
		case Icons._LIVINGROOM_SET2:
			if (flag || flag2)
			{
				m_MenuState = Icons._LIVINGROOM_SET1;
			}
			break;
		}
	}

	private void CalcObjectCost()
	{
		m_ObjectCost = Program.m_ItemManager.CountStaticItems() + 2 * Program.m_ItemManager.CountDynamicItems();
	}

	public void Draw()
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		m_App.m_SpriteBatch.Begin();
		if (m_ErrorFrame > 0)
		{
			m_ErrorFrame--;
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, m_App.GetText((App.TEXTID)m_Error), new Vector2(200f, 500f), Color.Yellow);
		}
		switch (m_State)
		{
		case State.MENU_ACTIVE:
			DrawMenu();
			break;
		case State.NORMAL:
			DrawEditorPointer(m_Pointer);
			Program.m_TriggerManager.DrawTriggerPoints(m_HighlightId);
			break;
		case State.OBJECT_SELECTED:
			DrawEditorPointer(m_Pointer);
			break;
		case State.SHOW_HELP:
			ShowHelpText();
			break;
		}
		DrawHelp();
		Color val = Color.LightGreen;
		if (m_ObjectCost > 70)
		{
			val = Color.Orange;
		}
		if (m_ObjectCost > 90)
		{
			val = Color.Red;
		}
		Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SmallFont, $"Object Cost: {m_ObjectCost}/{100}", new Vector2(125f, 630f), val);
		Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SmallFont, $"Name: {Program.m_App.m_UserLevelName}", new Vector2(550f, 630f), Color.LightGoldenrodYellow);
		m_App.m_SpriteBatch.End();
	}

	public void DrawMenu()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0107: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0123: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026d: Unknown result type (might be due to invalid IL or missing references)
		//IL_026e: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_0288: Unknown result type (might be due to invalid IL or missing references)
		//IL_028f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(560f, 320f);
		m_App.m_GameText.mText = m_App.GetText(App.TEXTID.LSTICK);
		m_App.m_GameText.Position = val + new Vector2(-10f, 37f);
		m_App.m_GameText.Draw(m_App.m_SpriteBatch, 0.75f);
		Vector2 val2 = default(Vector2);
		((Vector2)(ref val2))._002Ector(0f, -160f);
		if (m_MenuState == Icons.TOP_LEVEL)
		{
			for (int i = 1; i < 9; i++)
			{
				if (m_Icon[i] != null)
				{
					m_App.m_SpriteBatch.Draw(m_Icon[i], val + val2, Color.White);
					val2 = Vector2.Transform(val2, Matrix.CreateRotationZ((float)Math.PI / 4f));
				}
			}
		}
		else
		{
			for (int j = (int)(m_MenuState + 1); j < (int)(m_MenuState + 8 + 1); j++)
			{
				if (m_Icon[j] != null)
				{
					m_App.m_SpriteBatch.Draw(m_Icon[j], val + val2, Color.White);
					val2 = Vector2.Transform(val2, Matrix.CreateRotationZ((float)Math.PI / 4f));
				}
			}
		}
		if (m_IconSelected != -1 && IconHelp[(int)(m_MenuState + m_IconSelected + 1)] != null)
		{
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, IconHelp[(int)(m_MenuState + m_IconSelected + 1)].m_Title, val + new Vector2(-40f, -200f), Color.Yellow);
			m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, IconHelp[(int)(m_MenuState + m_IconSelected + 1)].m_Help, val + new Vector2(-350f, 280f), Color.LightGoldenrodYellow);
			((Vector2)(ref val2))._002Ector(0f, -80f);
			float num = (float)Math.PI / 4f * (float)m_IconSelected;
			val2 = Vector2.Transform(val2, Matrix.CreateRotationZ(num));
			Rectangle value = default(Rectangle);
			((Rectangle)(ref value))._002Ector(0, 0, m_App.m_EditorArrowTexture.Width, m_App.m_EditorArrowTexture.Height);
			Program.m_App.m_SpriteBatch.Draw(m_App.m_EditorArrowTexture, val + val2 + new Vector2(65f, 60f), (Rectangle?)value, Color.White, num, new Vector2(32f, 21f), 1f, (SpriteEffects)0, 0f);
		}
	}

	private void DrawHelp()
	{
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0173: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0389: Unknown result type (might be due to invalid IL or missing references)
		//IL_038e: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_032c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0331: Unknown result type (might be due to invalid IL or missing references)
		switch (m_State)
		{
		case State.NORMAL:
			if (!m_Highlight)
			{
				m_App.m_EditorText.mText = m_App.GetText(App.TEXTID.EDITOR_CREATE_ZOOM);
			}
			else
			{
				m_App.m_EditorText.mText = m_App.GetText(App.TEXTID.EDITOR_SELECT_CREATE_ZOOM);
			}
			m_App.m_EditorText.Position = new Vector2(90f, 80f);
			m_App.m_EditorText.m_ButtonOffset = new Vector2(35f, -7f);
			m_App.m_EditorText.Draw(m_App.m_SpriteBatch, 0.6f);
			m_App.m_EditorText.mText = m_App.GetText(App.TEXTID.EDITOR_POINTER_MOVE_CAMERA);
			m_App.m_EditorText.Position = new Vector2(50f, 150f);
			m_App.m_EditorText.m_ButtonOffset = new Vector2(35f, -7f);
			m_App.m_EditorText.Draw(m_App.m_SpriteBatch, 0.6f);
			break;
		case State.OBJECT_SELECTED:
			m_App.m_EditorText.mText = m_App.GetText(App.TEXTID.EDITOR_DESELECT_ROTATE);
			m_App.m_EditorText.Position = new Vector2(90f, 80f);
			m_App.m_EditorText.m_ButtonOffset = new Vector2(35f, -7f);
			m_App.m_EditorText.Draw(m_App.m_SpriteBatch, 0.6f);
			m_App.m_EditorText.mText = m_App.GetText(App.TEXTID.EDITOR_POINTER_MOVE_CAMERA);
			m_App.m_EditorText.Position = new Vector2(50f, 150f);
			m_App.m_EditorText.m_ButtonOffset = new Vector2(35f, -7f);
			m_App.m_EditorText.Draw(m_App.m_SpriteBatch, 0.6f);
			break;
		case State.MENU_ACTIVE:
			switch (m_MenuState)
			{
			case Icons._BEDROOM_SET1:
			case Icons._BEDROOM_SET2:
			case Icons._BEDROOM_SET3:
			case Icons._BATHROOM_SET1:
			case Icons._BATHROOM_SET2:
			case Icons._DESK_SET1:
			case Icons._DESK_SET2:
			case Icons._DESK_SET3:
			case Icons._KITCHEN_SET1:
			case Icons._KITCHEN_SET2:
			case Icons._LIVINGROOM_SET1:
			case Icons._LIVINGROOM_SET2:
				if (m_IconSelected == -1)
				{
					m_App.m_EditorText.mText = m_App.GetText(App.TEXTID.EDITOR_CANCEL_CYCLE);
				}
				else
				{
					m_App.m_EditorText.mText = m_App.GetText(App.TEXTID.EDITOR_SELECT_CANCEL_CYCLE);
				}
				break;
			default:
				m_App.m_EditorText.mText = m_App.GetText(App.TEXTID.EDITOR_SELECT_CANCEL);
				break;
			}
			m_App.m_EditorText.Position = new Vector2(90f, 80f);
			m_App.m_EditorText.m_ButtonOffset = new Vector2(35f, -7f);
			m_App.m_EditorText.Draw(m_App.m_SpriteBatch, 0.6f);
			break;
		case State.SHOW_HELP:
			m_App.m_EditorText.mText = m_App.GetText(App.TEXTID.CANCEL);
			m_App.m_EditorText.Position = new Vector2(90f, 80f);
			m_App.m_EditorText.m_ButtonOffset = new Vector2(35f, -7f);
			m_App.m_EditorText.Draw(m_App.m_SpriteBatch, 0.6f);
			break;
		}
	}

	public void DrawEditorPointer(Vector2 pos)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		switch (m_State)
		{
		case State.NORMAL:
			m_App.m_SpriteBatch.Draw(m_App.m_EditorPointerTexture, pos, new Color(255, 255, 255, 255));
			break;
		case State.OBJECT_SELECTED:
			m_App.m_SpriteBatch.Draw(m_App.m_EditorPointerHandTexture, pos, new Color(255, 255, 255, 255));
			break;
		}
	}

	private void ShowHelpText()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0182: Unknown result type (might be due to invalid IL or missing references)
		//IL_0183: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_022e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_029f: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0311: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(130f, 120f);
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Track ramps can be found in the Bedroom menu.", val, Color.Yellow);
		val.Y += 39f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Checkpoints can be found in the Options menu.", val, Color.LightGoldenrodYellow);
		val.Y += 39f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Zoom out before placing larger objects.", val, Color.Yellow);
		val.Y += 39f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Share only when level is finished and tested", val, Color.LightGoldenrodYellow);
		val.Y += 29f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "because your level will then be rated by others.", val, Color.LightGoldenrodYellow);
		val.Y += 39f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Dynamic objects will fall when the race starts.", val, Color.Yellow);
		val.Y += 39f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Press BACK during playtest to return to editor.", val, Color.LightGoldenrodYellow);
		val.Y += 39f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Many ramp objects only have collision on their top.", val, Color.Yellow);
		val.Y += 39f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Make sure your level can be completed otherwise", val, Color.LightGoldenrodYellow);
		val.Y += 29f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "other players won't be able to rate it.", val, Color.LightGoldenrodYellow);
		val.Y += 39f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Use 'Edit My Levels' to improve your level later,", val, Color.Yellow);
		val.Y += 29f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "other players will receive the updated version", val, Color.Yellow);
		val.Y += 29f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "only if you share (or re-share) it.", val, Color.Yellow);
		val.Y += 39f;
		m_App.m_SpriteBatch.DrawString(m_App.m_SmallFont, "Re-sharing an edited level won't affect it's rating.", val, Color.LightGoldenrodYellow);
	}

	public bool SaveLevel()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Expected O, but got Unknown
		if (Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId] == null || !Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId].IsSignedInToLive)
		{
			m_Error = 122;
			m_ErrorFrame = 240;
			Program.m_SoundManager.Play(41);
			return false;
		}
		string text = "";
		if (m_bEditExisting)
		{
			text = Program.m_App.m_UserLevelName;
		}
		else
		{
			text = $"{((Gamer)Gamer.SignedInGamers[Program.m_App.m_PlayerOnePadId]).Gamertag}_{Program.m_App.m_MyLevelId:d5}";
			Program.m_App.m_UserLevelName = text;
		}
		m_LevelVersion++;
		int cnt = 0;
		for (int i = 0; i < 512; i++)
		{
			if (Program.m_TriggerManager.m_Trigger[i].m_Id != -1)
			{
				cnt++;
			}
		}
		Program.m_App.saveDevice.SaveAsync("Toy Stunt Bike 2", text, (FileAction)delegate(Stream stream)
		{
			BinaryWriter binaryWriter = new BinaryWriter(stream);
			binaryWriter.Write(m_LevelVersion);
			binaryWriter.Write(m_LevelBackground);
			binaryWriter.Write(cnt);
			for (int j = 0; j < 512; j++)
			{
				if (Program.m_TriggerManager.m_Trigger[j].m_Id != -1)
				{
					binaryWriter.Write(Program.m_TriggerManager.m_Trigger[j].m_Type);
					binaryWriter.Write(Program.m_TriggerManager.m_Trigger[j].m_Position.X);
					binaryWriter.Write(Program.m_TriggerManager.m_Trigger[j].m_Position.Y);
					binaryWriter.Write(Program.m_TriggerManager.m_Trigger[j].m_Rotation);
				}
			}
			binaryWriter.Close();
		});
		int num = 0;
		while (Program.m_App.saveDevice.IsBusy && num < 10000000)
		{
			num++;
		}
		Program.m_LoadSaveManager.SaveGame();
		num = 0;
		while (Program.m_App.saveDevice.IsBusy && cnt < 10000000)
		{
			num++;
		}
		return true;
	}

	public bool LoadLevel(string filename)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Expected O, but got Unknown
		FileAction val = null;
		Program.m_TriggerManager.DeleteAll();
		if (((ISaveDevice)Program.m_App.saveDevice).FileExists("Toy Stunt Bike 2", filename))
		{
			IAsyncSaveDevice saveDevice = Program.m_App.saveDevice;
			if (val == null)
			{
				val = (FileAction)delegate(Stream stream)
				{
					//IL_0028: Unknown result type (might be due to invalid IL or missing references)
					//IL_002d: Unknown result type (might be due to invalid IL or missing references)
					//IL_0069: Unknown result type (might be due to invalid IL or missing references)
					BinaryReader binaryReader = new BinaryReader(stream);
					m_LevelVersion = binaryReader.ReadInt32();
					m_LevelBackground = binaryReader.ReadInt32();
					int num = binaryReader.ReadInt32();
					int num2 = 0;
					Vector2 zero = Vector2.Zero;
					float num3 = 0f;
					for (int i = 0; i < num; i++)
					{
						num2 = binaryReader.ReadInt32();
						zero.X = binaryReader.ReadSingle();
						zero.Y = binaryReader.ReadSingle();
						num3 = binaryReader.ReadSingle();
						Program.m_TriggerManager.Create(num2, zero, num3);
					}
					binaryReader.Close();
				};
			}
			((ISaveDevice)saveDevice).Load("Toy Stunt Bike 2", filename, val);
			return true;
		}
		return false;
	}

	public void ChangeBackground(int background)
	{
		switch (background)
		{
		case 0:
			Program.m_App.m_BackgroundManager.m_Background[0].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background4"), null);
			Program.m_App.m_BackgroundManager.m_Background[1].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background4"), null);
			App.ChangeEffectUsedByModel(Program.m_App.m_BackgroundManager.m_Background[0].m_Model, Program.m_App.basicEffectShad);
			break;
		case 1:
			Program.m_App.m_BackgroundManager.m_Background[0].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background5"), null);
			Program.m_App.m_BackgroundManager.m_Background[1].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background5"), null);
			App.ChangeEffectUsedByModel(Program.m_App.m_BackgroundManager.m_Background[0].m_Model, Program.m_App.basicEffectShad);
			break;
		case 2:
			Program.m_App.m_BackgroundManager.m_Background[0].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background2"), null);
			Program.m_App.m_BackgroundManager.m_Background[1].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background2"), null);
			App.ChangeEffectUsedByModel(Program.m_App.m_BackgroundManager.m_Background[0].m_Model, Program.m_App.basicEffectShad);
			break;
		case 3:
			Program.m_App.m_BackgroundManager.m_Background[0].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background1"), null);
			Program.m_App.m_BackgroundManager.m_Background[1].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background1"), null);
			App.ChangeEffectUsedByModel(Program.m_App.m_BackgroundManager.m_Background[0].m_Model, Program.m_App.basicEffectShad);
			break;
		case 4:
			Program.m_App.m_BackgroundManager.m_Background[0].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background3"), null);
			Program.m_App.m_BackgroundManager.m_Background[1].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background3"), null);
			App.ChangeEffectUsedByModel(Program.m_App.m_BackgroundManager.m_Background[0].m_Model, Program.m_App.basicEffectShad);
			break;
		case 5:
			Program.m_App.m_BackgroundManager.m_Background[0].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background6"), null);
			Program.m_App.m_BackgroundManager.m_Background[1].SetModel(((Game)Program.m_App).Content.Load<Model>("Models/background6"), null);
			App.ChangeEffectUsedByModel(Program.m_App.m_BackgroundManager.m_Background[0].m_Model, Program.m_App.basicEffectShad);
			break;
		}
	}

	private void CheckJukeboxControls()
	{
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)256))
		{
			Program.m_App.PlayPrevSong();
		}
		if (Program.m_PlayerManager.GetPrimaryPlayer().Debounce((Buttons)512))
		{
			Program.m_App.PlayNextSong();
		}
	}
}
