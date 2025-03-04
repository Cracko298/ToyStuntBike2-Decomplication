using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace AppNamespace;

internal class Button : Text
{
	public enum Buttons
	{
		LeftThumbstick,
		RightThumbstick,
		DirectionalPad,
		Back,
		Guide,
		Start,
		X,
		A,
		Y,
		B,
		RightShoulder,
		LeftShoulder,
		RightTrigger,
		LeftTrigger,
		None
	}

	private const string mButtonAssetName = "Fonts/xboxControllerSpriteFont";

	public const string XButton = "[X]";

	public const string YButton = "[Y]";

	public const string AButton = "[A]";

	public const string BButton = "[B]";

	public const string RightTrigger = "[RTRIGGER]";

	public const string LeftTrigger = "[LTRIGGER]";

	public const string Back = "[BACK]";

	public const string DPad = "[DPAD]";

	public const string Guide = "[GUIDE]";

	public const string LeftShoulder = "[LSHOULDER]";

	public const string LeftThumb = "[LTHUMB]";

	public const string RightShoulder = "[RSHOULDER]";

	public const string RightThumb = "[RTHUMB]";

	public const string Start = "[START]";

	public Buttons ButtonType;

	public static string[] ButtonText = new string[14]
	{
		"[A]", "[BACK]", "[B]", "[DPAD]", "[GUIDE]", "[LSHOULDER]", "[LTHUMB]", "[LTRIGGER]", "[RSHOULDER]", "[RTHUMB]",
		"[RTRIGGER]", "[START]", "[X]", "[Y]"
	};

	public Button(ContentManager theContent, Buttons theButton, Vector2 thePosition)
		: base(theContent, "", "Fonts/xboxControllerSpriteFont", thePosition, Color.White)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		ChangeButtonType(theButton);
	}

	public void ChangeButtonType(Buttons theButton)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0153: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		ButtonType = theButton;
		mText = "";
		Scale = 1f;
		m_ButtonOffset = Vector2.Zero;
		switch (theButton)
		{
		case Buttons.LeftThumbstick:
			mText = " ";
			Scale = 0.75f;
			m_ButtonOffset = new Vector2(40f, 12f);
			break;
		case Buttons.RightThumbstick:
			mText = "\"";
			break;
		case Buttons.DirectionalPad:
			mText = "!";
			break;
		case Buttons.Back:
			mText = "#";
			break;
		case Buttons.Guide:
			mText = "$";
			break;
		case Buttons.Start:
			mText = "%";
			break;
		case Buttons.X:
			mText = "&";
			break;
		case Buttons.Y:
			mText = "(";
			break;
		case Buttons.A:
			mText = "'";
			break;
		case Buttons.B:
			mText = ")";
			break;
		case Buttons.LeftShoulder:
			mText = "*";
			Scale = 0.75f;
			m_ButtonOffset = new Vector2(120f, 12f);
			break;
		case Buttons.RightShoulder:
			mText = "-";
			Scale = 0.75f;
			m_ButtonOffset = new Vector2(0f, 12f);
			break;
		case Buttons.RightTrigger:
			mText = "+";
			Scale = 0.75f;
			m_ButtonOffset = new Vector2(40f, 12f);
			break;
		case Buttons.LeftTrigger:
			mText = ",";
			break;
		}
	}

	public void ChangeButtonType(string theButton)
	{
		switch (theButton)
		{
		case "[X]":
			ChangeButtonType(Buttons.X);
			break;
		case "[Y]":
			ChangeButtonType(Buttons.Y);
			break;
		case "[A]":
			ChangeButtonType(Buttons.A);
			break;
		case "[B]":
			ChangeButtonType(Buttons.B);
			break;
		case "[RTRIGGER]":
			ChangeButtonType(Buttons.RightTrigger);
			break;
		case "[LTRIGGER]":
			ChangeButtonType(Buttons.LeftTrigger);
			break;
		case "[BACK]":
			ChangeButtonType(Buttons.Back);
			break;
		case "[DPAD]":
			ChangeButtonType(Buttons.DirectionalPad);
			break;
		case "[GUIDE]":
			ChangeButtonType(Buttons.Guide);
			break;
		case "[LSHOULDER]":
			ChangeButtonType(Buttons.LeftShoulder);
			break;
		case "[LTHUMB]":
			ChangeButtonType(Buttons.LeftThumbstick);
			break;
		case "[RSHOULDER]":
			ChangeButtonType(Buttons.RightShoulder);
			break;
		case "[RTHUMB]":
			ChangeButtonType(Buttons.RightThumbstick);
			break;
		case "[START]":
			ChangeButtonType(Buttons.Start);
			break;
		}
	}

	public Vector2 Length()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = mFont.MeasureString(mText);
		result.Y /= 4.25f;
		result.X += 10f;
		return result;
	}
}
