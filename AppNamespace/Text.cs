using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AppNamespace;

public class Text
{
	public enum Alignment
	{
		Horizonatal,
		Vertical,
		Both
	}

	public SpriteFont mFont;

	public string mText;

	public string mFontAssetName;

	public Vector2 Position;

	public Color mColor;

	public float Scale = 1f;

	private string[] sTmp = new string[16];

	private int m_StartButtonTextAt;

	private float m_Scale = 1f;

	public Vector2 m_ButtonOffset = Vector2.Zero;

	private Button mButton;

	private ContentManager mContent;

	public Text(ContentManager theContent, string theText, string theFontAssetName, Vector2 theStartingPosition, Color theColor)
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		mFont = theContent.Load<SpriteFont>(theFontAssetName);
		mFontAssetName = theFontAssetName;
		mText = theText;
		Position = theStartingPosition;
		mColor = theColor;
		mContent = theContent;
	}

	public string[] StringSplit(string sIn, char sDelim1, char sDelim2)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		if (sIn.Length == 0)
		{
			return new string[0];
		}
		num2 = sIn.IndexOf(sDelim1, num);
		if (num2 < 0)
		{
			return new string[1] { sIn };
		}
		num3 = 0;
		while (num < sIn.Length && num2 >= 0)
		{
			sTmp[num4++] = sIn.Substring(num3, num2 - num3);
			num3 = sIn.IndexOf(sDelim2, num2);
			num2 = sIn.IndexOf(sDelim1, num3);
			num3++;
			if (num2 == -1)
			{
				sTmp[num4++] = sIn.Substring(num3, sIn.Length - num3);
			}
		}
		string[] array = new string[num4];
		for (int i = 0; i < num4; i++)
		{
			array[i] = sTmp[i];
		}
		return array;
	}

	public virtual void Draw(SpriteBatch theBatch, float scale)
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0188: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		if (scale == 0f)
		{
			scale = 0.75f;
		}
		string[] array = StringSplit(mText, '[', ']');
		if (mButton == null)
		{
			mButton = new Button(mContent, Button.Buttons.X, Vector2.Zero);
		}
		Vector2 position = Position;
		m_StartButtonTextAt = 0;
		string[] array2 = array;
		foreach (string text in array2)
		{
			theBatch.DrawString(mFont, text, position, mColor, 0f, Vector2.Zero, m_Scale, (SpriteEffects)0, 0f);
			Vector2 val = mFont.MeasureString(text);
			float num = ((Vector2)(ref val)).Length();
			SetButtonText(mText, text);
			mButton.Position = position;
			mButton.m_Scale = scale * mButton.Scale;
			ref Vector2 position2 = ref mButton.Position;
			position2.Y -= mButton.Length().Y + m_ButtonOffset.Y;
			ref Vector2 position3 = ref mButton.Position;
			position3.Y += mButton.m_ButtonOffset.Y;
			ref Vector2 position4 = ref mButton.Position;
			position4.X += num * mButton.Scale + m_ButtonOffset.X;
			ref Vector2 position5 = ref mButton.Position;
			position5.X += mButton.m_ButtonOffset.X;
			mButton.mColor = mColor;
			mButton.Draw(theBatch, scale);
			position.X += num + mButton.Length().X;
		}
	}

	private bool SetButtonText(string theFullText, string theCurrentText)
	{
		mButton.ChangeButtonType(Button.Buttons.None);
		int num = theFullText.IndexOf(theCurrentText);
		if (m_StartButtonTextAt != 0)
		{
			num = m_StartButtonTextAt;
		}
		if (num == -1)
		{
			return false;
		}
		int num2 = theFullText.IndexOf("[", num);
		if (num2 == -1)
		{
			return false;
		}
		int num3 = theFullText.IndexOf("]", num2);
		if (num3 == -1)
		{
			return false;
		}
		m_StartButtonTextAt = num3;
		string theButton = theFullText.Substring(num2, num3 - num2 + 1);
		mButton.ChangeButtonType(theButton);
		return true;
	}

	public void Center(Rectangle theDisplayArea, Alignment theAlignment)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = mFont.MeasureString(mText);
		switch (theAlignment)
		{
		case Alignment.Horizonatal:
			Position.X = (float)theDisplayArea.X + ((float)(theDisplayArea.Width / 2) - val.X / 2f);
			break;
		case Alignment.Vertical:
			Position.Y = (float)theDisplayArea.Y + ((float)(theDisplayArea.Height / 2) - val.Y / 2f);
			break;
		case Alignment.Both:
			Position.X = (float)theDisplayArea.X + ((float)(theDisplayArea.Width / 2) - val.X / 2f);
			Position.Y = (float)theDisplayArea.Y + ((float)(theDisplayArea.Height / 2) - val.Y / 2f);
			break;
		}
	}
}
