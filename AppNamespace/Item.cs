using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace AppNamespace;

public class Item : Actor
{
	public int m_Type;

	public int m_TriggerId;

	public Vector2 m_TriggerPos;

	public bool m_bHasPlayerCollision = true;

	public bool m_bCastShadows = true;

	public int m_Layer;

	public SoundEffectInstance m_ItemSound;

	public Fixture m_Fixture;

	public int m_UniqueId = -1;

	public Item()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		m_Type = -1;
		m_Id = -1;
		m_fScale = 1f;
		m_TriggerId = -1;
		m_TriggerPos = Vector2.Zero;
		m_Velocity = Vector2.Zero;
	}

	public bool OnScreen()
	{
		if (Program.m_App.m_bEditor || Program.m_App.m_bLevelEditor)
		{
			return true;
		}
		float extraBounds = Program.m_TriggerManager.GetExtraBounds(m_Type);
		if (m_Position.X > Program.m_CurrentCamera.m_CameraPosition.X + 60f + extraBounds || m_Position.X < Program.m_CurrentCamera.m_CameraPosition.X - 20f - extraBounds)
		{
			return false;
		}
		return true;
	}

	public void Update()
	{
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		if (m_FlashModel > 0)
		{
			m_FlashModel--;
		}
		UpdateShake();
		if (m_Fixture != null)
		{
			m_Position.X = m_Fixture.Body.Position.X;
			m_Position.Y = m_Fixture.Body.Position.Y;
			m_Rotation.Z = m_Fixture.Body.Rotation;
		}
		Vector3 zero = Vector3.Zero;
		zero.X = m_Position.X;
		zero.Y = m_Position.Y;
		zero.Z = m_ZDistance;
		m_ScreenPos = Program.m_CurrentCamera.WorldToScreen(zero);
		CheckPlayerCollision();
	}

	public void Delete()
	{
		m_Id = -1;
		if (m_ItemSound != null)
		{
			m_ItemSound.Stop();
		}
		if (m_Fixture != null)
		{
			Program.m_App.m_World.RemoveBody(m_Fixture.Body);
			m_Fixture = null;
		}
	}

	public void CheckPlayerCollision()
	{
		if (Program.m_App.m_bEditor || (m_Type != 21 && m_Type != 22 && m_Type != 23 && m_Type != 12 && m_Type != 38 && m_Type != 58 && m_Type != 62 && m_Type != 112))
		{
			return;
		}
		CalcBounds2D(bUseAll: false, bUsePitch: false);
		if (!IsBoundsValid())
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			if (Program.m_PlayerManager.m_Player[i].m_Id != -1 && (!Program.m_App.m_bSplitScreen || i == Program.m_App.m_CurrentSplit) && !(Program.m_PlayerManager.m_Player[i].m_Bounds2DMin.X < 0f) && !(Program.m_PlayerManager.m_Player[i].m_Bounds2DMax.X > 1280f))
			{
				float num = 0f;
				float num2 = 0f;
				if (m_Type == 21 || m_Type == 22 || m_Type == 23)
				{
					num = -60f;
					num2 = -60f;
				}
				if (m_Type == 62)
				{
					num2 = -200f;
				}
				float num3 = -40f;
				if (m_Type == 12)
				{
					num2 = -30f;
				}
				if (m_Type == 112)
				{
					num2 = 50f;
				}
				if (m_Bounds2DMax.X - num > Program.m_PlayerManager.m_Player[i].m_Bounds2DMin.X && m_Bounds2DMin.X + num < Program.m_PlayerManager.m_Player[i].m_Bounds2DMax.X && m_Bounds2DMax.Y - num2 > Program.m_PlayerManager.m_Player[i].m_Bounds2DMin.Y + num3 && m_Bounds2DMin.Y + num2 < Program.m_PlayerManager.m_Player[i].m_Bounds2DMax.Y - num3)
				{
					Program.m_PlayerManager.m_Player[i].GivePickup(m_Id);
				}
			}
		}
	}

	public bool IsBoundsValid()
	{
		if (m_Bounds2DMax.X > 2000f || m_Bounds2DMax.Y > 2000f || m_Bounds2DMin.X < -2000f || m_Bounds2DMin.Y < -2000f)
		{
			return false;
		}
		return true;
	}

	public void CollectFlag()
	{
		if (Program.m_ItemManager.m_FlagsCollected[m_UniqueId] == 0)
		{
			Program.m_ItemManager.m_FlagsCollected[m_UniqueId] = 1;
		}
	}
}
