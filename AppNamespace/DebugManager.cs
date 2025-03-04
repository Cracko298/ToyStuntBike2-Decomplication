using Maths;
using Microsoft.Xna.Framework;

namespace AppNamespace;

public class DebugManager
{
	public const int DEBUG_TYPE_LINE = 0;

	public const int DEBUG_TYPE_ARROW = 1;

	public const int DEBUG_TYPE_TEXT = 2;

	public const int MAX_DEBUG_DRAW = 1024;

	public const int MAX_DEBUG_TEXT = 10240;

	private int m_Index;

	private DebugDraw[] m_DebugDraw;

	public Vector3 WHITE = new Vector3(1f, 1f, 1f);

	public Vector3 RED = new Vector3(1f, 0f, 0f);

	public Vector3 BLUE = new Vector3(0f, 0f, 1f);

	public Vector3 GREEN = new Vector3(0f, 1f, 0f);

	public Vector3 BLACK = new Vector3(0f, 0f, 0f);

	public DebugManager()
	{
		m_Index = 0;
		m_DebugDraw = new DebugDraw[1024];
		for (int i = 0; i < 1024; i++)
		{
			m_DebugDraw[i] = new DebugDraw();
		}
	}

	public void AddLine(Vector2 start, Vector2 end, Vector3 col)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		m_DebugDraw[m_Index].m_Type = 0;
		m_DebugDraw[m_Index].m_Start = start;
		m_DebugDraw[m_Index].m_End = end;
		m_DebugDraw[m_Index].m_Col = col;
		if (m_Index < 1023)
		{
			m_Index++;
		}
	}

	public void AddArrow(Vector2 start, Vector2 end, Vector3 col)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		m_DebugDraw[m_Index].m_Type = 1;
		m_DebugDraw[m_Index].m_Start = start;
		m_DebugDraw[m_Index].m_End = end;
		m_DebugDraw[m_Index].m_Col = col;
		if (m_Index < 1023)
		{
			m_Index++;
		}
	}

	public void AddText(string text, Vector2 start)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		m_DebugDraw[m_Index].m_Type = 2;
		m_DebugDraw[m_Index].m_Start = start;
		m_DebugDraw[m_Index].m_Text = text;
		if (m_Index < 1023)
		{
			m_Index++;
		}
	}

	public void Render()
	{
		m_Index = 0;
	}

	public void DEBUG_DrawLine(Vector2 start, Vector2 end, Vector3 col, bool bDraw)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		if (!bDraw)
		{
			AddLine(start, end, col);
		}
		else
		{
			Program.m_App.DrawLine(start, end, new Color(255, 255, 255, 255), 3f);
		}
	}

	public void DEBUG_DrawArrow(Vector2 start, Vector2 end, Vector3 col, bool bDraw)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		if (!bDraw)
		{
			AddArrow(start, end, col);
			return;
		}
		_ = (start + end) * 0.5f;
		Vector2 val = Vector2.Zero;
		val.X = 0f - end.Y;
		val.Y = end.X;
		((Vector2)(ref val)).Normalize();
		val *= 10f;
	}

	public void DEBUG_DrawText(string text, Vector2 pos, bool bDraw)
	{
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		if (Program.m_App.m_SpeechFont != null)
		{
			if (!bDraw)
			{
				Program.m_DebugManager.AddText(text, pos);
				return;
			}
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector(0f, 0f);
			val.X = pos.X;
			val.Y = pos.Y;
			Color val2 = default(Color);
			((Color)(ref val2))._002Ector(0.5f, 0.5f, 0.5f, 0.5f);
			Program.m_App.m_SpriteBatch.Begin();
			Program.m_App.m_SpriteBatch.DrawString(Program.m_App.m_SpeechFont, text, val, val2);
			Program.m_App.m_SpriteBatch.End();
		}
	}

	public void DEBUG_Trace(string text)
	{
	}

	public void DrawSafeFrame()
	{
	}
}
