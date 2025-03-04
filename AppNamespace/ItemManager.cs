using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace AppNamespace;

internal class ItemManager
{
	public const int MAX_ITEMS = 256;

	public const int MAX_FLAGS = 36;

	public const int MAX_CUPS = 36;

	private const float S = 2.54f;

	public Item[] m_Item;

	public Vector2[] m_ModelSize2D;

	public Vector3[] m_ModelSizeMax3D;

	public Vector3[] m_ModelSizeMin3D;

	public Model[] m_Model;

	public Model[] m_ModelLOD;

	public int[] m_FlagsCollected;

	public int[] m_CupsCollected;

	private int m_NextId;

	private Vector2 m_LastEdge = Vector2.Zero;

	private float m_ItemImpactSoundTime;

	public ItemManager()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		m_Item = new Item[256];
		for (int i = 0; i < 256; i++)
		{
			m_Item[i] = new Item();
		}
		m_ModelSize2D = (Vector2[])(object)new Vector2[502];
		m_ModelSizeMax3D = (Vector3[])(object)new Vector3[502];
		m_ModelSizeMin3D = (Vector3[])(object)new Vector3[502];
		m_Model = (Model[])(object)new Model[502];
		m_ModelLOD = (Model[])(object)new Model[502];
		m_FlagsCollected = new int[36];
		for (int j = 0; j < 36; j++)
		{
			m_FlagsCollected[j] = 0;
		}
		m_CupsCollected = new int[36];
		for (int k = 0; k < 36; k++)
		{
			m_CupsCollected[k] = 0;
		}
	}

	public int TotalFlagsCollected()
	{
		int num = 0;
		for (int i = 0; i < 36; i++)
		{
			if (m_FlagsCollected[i] > 0)
			{
				num++;
			}
		}
		return num;
	}

	public int TotalCupsCollected()
	{
		int num = 0;
		for (int i = 0; i < 36; i++)
		{
			if (m_CupsCollected[i] > 0)
			{
				num++;
			}
		}
		return num;
	}

	public void ClearFlagsCollected()
	{
		for (int i = 0; i < 36; i++)
		{
			m_FlagsCollected[i] = 0;
		}
	}

	public void ClearCupsCollected()
	{
		for (int i = 0; i < 36; i++)
		{
			m_CupsCollected[i] = 0;
		}
	}

	public void CopyFlagsCollected(int[] src, int[] dest)
	{
		for (int i = 0; i < 36; i++)
		{
			dest[i] = src[i];
		}
	}

	public void CopyCupsCollected(int[] src, int[] dest)
	{
		for (int i = 0; i < 36; i++)
		{
			dest[i] = src[i];
		}
	}

	public int GetNumFlagsCollectedOnLevel(int level)
	{
		if (level >= 12)
		{
			return 0;
		}
		int num = 0;
		int num2 = level * 3;
		if (m_FlagsCollected[num2] > 0)
		{
			num++;
		}
		if (m_FlagsCollected[num2 + 1] > 0)
		{
			num++;
		}
		if (m_FlagsCollected[num2 + 2] > 0)
		{
			num++;
		}
		return num;
	}

	public void GiveTimeCupOnLevel(int level)
	{
		if (level < 12)
		{
			int num = level * 3;
			m_CupsCollected[num] = 1;
		}
	}

	public int GetTimeCupOnLevel(int level)
	{
		if (level >= 12)
		{
			return 0;
		}
		int num = level * 3;
		return m_CupsCollected[num];
	}

	public void GiveScoreCupOnLevel(int level)
	{
		if (level < 12)
		{
			int num = level * 3;
			m_CupsCollected[num + 1] = 1;
		}
	}

	public int GetScoreCupOnLevel(int level)
	{
		if (level >= 12)
		{
			return 0;
		}
		int num = level * 3;
		return m_CupsCollected[num + 1];
	}

	public void GiveFlagsCupOnLevel(int level)
	{
		if (level < 12)
		{
			int num = level * 3;
			m_CupsCollected[num + 2] = 1;
		}
	}

	public int GetFlagsCupOnLevel(int level)
	{
		if (level >= 12)
		{
			return 0;
		}
		int num = level * 3;
		return m_CupsCollected[num + 2];
	}

	public int Create(int type, int triggerId, Vector2 pos, float rot)
	{
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0148: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016f: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0181: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0485: Unknown result type (might be due to invalid IL or missing references)
		//IL_048f: Expected O, but got Unknown
		//IL_048f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0499: Expected O, but got Unknown
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a5: Expected O, but got Unknown
		//IL_04b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04be: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04de: Unknown result type (might be due to invalid IL or missing references)
		//IL_0525: Unknown result type (might be due to invalid IL or missing references)
		//IL_052c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0531: Unknown result type (might be due to invalid IL or missing references)
		//IL_0533: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_054a: Unknown result type (might be due to invalid IL or missing references)
		//IL_054f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0551: Unknown result type (might be due to invalid IL or missing references)
		//IL_0598: Unknown result type (might be due to invalid IL or missing references)
		//IL_059f: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_060b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0612: Unknown result type (might be due to invalid IL or missing references)
		//IL_0617: Unknown result type (might be due to invalid IL or missing references)
		//IL_0619: Unknown result type (might be due to invalid IL or missing references)
		//IL_0629: Unknown result type (might be due to invalid IL or missing references)
		//IL_0630: Unknown result type (might be due to invalid IL or missing references)
		//IL_0635: Unknown result type (might be due to invalid IL or missing references)
		//IL_0637: Unknown result type (might be due to invalid IL or missing references)
		//IL_0677: Unknown result type (might be due to invalid IL or missing references)
		//IL_067e: Expected O, but got Unknown
		//IL_068b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0692: Unknown result type (might be due to invalid IL or missing references)
		//IL_0697: Unknown result type (might be due to invalid IL or missing references)
		//IL_0699: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06be: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_06dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0701: Unknown result type (might be due to invalid IL or missing references)
		//IL_0706: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_071f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0726: Unknown result type (might be due to invalid IL or missing references)
		//IL_072b: Unknown result type (might be due to invalid IL or missing references)
		//IL_072d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0744: Unknown result type (might be due to invalid IL or missing references)
		//IL_074b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0750: Unknown result type (might be due to invalid IL or missing references)
		//IL_0752: Unknown result type (might be due to invalid IL or missing references)
		//IL_075e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0765: Expected O, but got Unknown
		//IL_0796: Unknown result type (might be due to invalid IL or missing references)
		//IL_079d: Expected O, but got Unknown
		//IL_07aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_07cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_07d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07db: Unknown result type (might be due to invalid IL or missing references)
		//IL_07dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0800: Unknown result type (might be due to invalid IL or missing references)
		//IL_0802: Unknown result type (might be due to invalid IL or missing references)
		//IL_0819: Unknown result type (might be due to invalid IL or missing references)
		//IL_0820: Unknown result type (might be due to invalid IL or missing references)
		//IL_0825: Unknown result type (might be due to invalid IL or missing references)
		//IL_0827: Unknown result type (might be due to invalid IL or missing references)
		//IL_0833: Unknown result type (might be due to invalid IL or missing references)
		//IL_083a: Expected O, but got Unknown
		//IL_086b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0872: Expected O, but got Unknown
		//IL_087f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0886: Unknown result type (might be due to invalid IL or missing references)
		//IL_088b: Unknown result type (might be due to invalid IL or missing references)
		//IL_088d: Unknown result type (might be due to invalid IL or missing references)
		//IL_08a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_08c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_08ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_08f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_08fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0908: Unknown result type (might be due to invalid IL or missing references)
		//IL_090f: Expected O, but got Unknown
		//IL_0945: Unknown result type (might be due to invalid IL or missing references)
		//IL_094c: Expected O, but got Unknown
		//IL_0960: Unknown result type (might be due to invalid IL or missing references)
		//IL_0967: Unknown result type (might be due to invalid IL or missing references)
		//IL_096c: Unknown result type (might be due to invalid IL or missing references)
		//IL_096e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0985: Unknown result type (might be due to invalid IL or missing references)
		//IL_098c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0991: Unknown result type (might be due to invalid IL or missing references)
		//IL_0993: Unknown result type (might be due to invalid IL or missing references)
		//IL_09aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_09cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_09d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_09db: Unknown result type (might be due to invalid IL or missing references)
		//IL_09dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_09f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_09fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a00: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a02: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a15: Expected O, but got Unknown
		//IL_0abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b0d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b14: Expected O, but got Unknown
		//IL_0b21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b46: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0b4d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0bfd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c38: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c3f: Expected O, but got Unknown
		//IL_0c4c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c53: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c58: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c71: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0c78: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fa3: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ff8: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_6fff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d88: Unknown result type (might be due to invalid IL or missing references)
		//IL_0d8f: Expected O, but got Unknown
		//IL_0d9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0da8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0daa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0dc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e16: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e1d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e2d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e34: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e39: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e82: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e89: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ea7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0eae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ef5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0efc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f01: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f03: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f13: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f21: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f68: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f76: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f86: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f8d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f92: Unknown result type (might be due to invalid IL or missing references)
		//IL_0f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fd3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fda: Expected O, but got Unknown
		//IL_0fe7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0fee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0ff5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1005: Unknown result type (might be due to invalid IL or missing references)
		//IL_100c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1011: Unknown result type (might be due to invalid IL or missing references)
		//IL_1013: Unknown result type (might be due to invalid IL or missing references)
		//IL_105a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1061: Unknown result type (might be due to invalid IL or missing references)
		//IL_1066: Unknown result type (might be due to invalid IL or missing references)
		//IL_1068: Unknown result type (might be due to invalid IL or missing references)
		//IL_1078: Unknown result type (might be due to invalid IL or missing references)
		//IL_107f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1084: Unknown result type (might be due to invalid IL or missing references)
		//IL_1086: Unknown result type (might be due to invalid IL or missing references)
		//IL_10cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_10d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_10db: Unknown result type (might be due to invalid IL or missing references)
		//IL_10eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_10f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1140: Unknown result type (might be due to invalid IL or missing references)
		//IL_1147: Unknown result type (might be due to invalid IL or missing references)
		//IL_114c: Unknown result type (might be due to invalid IL or missing references)
		//IL_114e: Unknown result type (might be due to invalid IL or missing references)
		//IL_115e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1165: Unknown result type (might be due to invalid IL or missing references)
		//IL_116a: Unknown result type (might be due to invalid IL or missing references)
		//IL_116c: Unknown result type (might be due to invalid IL or missing references)
		//IL_11b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_11ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_11bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_11c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_11d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_11dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_11df: Unknown result type (might be due to invalid IL or missing references)
		//IL_1205: Unknown result type (might be due to invalid IL or missing references)
		//IL_120c: Expected O, but got Unknown
		//IL_1219: Unknown result type (might be due to invalid IL or missing references)
		//IL_1220: Unknown result type (might be due to invalid IL or missing references)
		//IL_1225: Unknown result type (might be due to invalid IL or missing references)
		//IL_1227: Unknown result type (might be due to invalid IL or missing references)
		//IL_1237: Unknown result type (might be due to invalid IL or missing references)
		//IL_123e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1243: Unknown result type (might be due to invalid IL or missing references)
		//IL_1245: Unknown result type (might be due to invalid IL or missing references)
		//IL_1284: Unknown result type (might be due to invalid IL or missing references)
		//IL_128b: Expected O, but got Unknown
		//IL_1298: Unknown result type (might be due to invalid IL or missing references)
		//IL_129f: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_12a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_12bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_12c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_130b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1312: Unknown result type (might be due to invalid IL or missing references)
		//IL_1317: Unknown result type (might be due to invalid IL or missing references)
		//IL_1319: Unknown result type (might be due to invalid IL or missing references)
		//IL_1329: Unknown result type (might be due to invalid IL or missing references)
		//IL_1330: Unknown result type (might be due to invalid IL or missing references)
		//IL_1335: Unknown result type (might be due to invalid IL or missing references)
		//IL_1337: Unknown result type (might be due to invalid IL or missing references)
		//IL_137e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1385: Unknown result type (might be due to invalid IL or missing references)
		//IL_138a: Unknown result type (might be due to invalid IL or missing references)
		//IL_138c: Unknown result type (might be due to invalid IL or missing references)
		//IL_139c: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_13a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_13f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_13fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_13ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_140f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1416: Unknown result type (might be due to invalid IL or missing references)
		//IL_141b: Unknown result type (might be due to invalid IL or missing references)
		//IL_141d: Unknown result type (might be due to invalid IL or missing references)
		//IL_1464: Unknown result type (might be due to invalid IL or missing references)
		//IL_146b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1470: Unknown result type (might be due to invalid IL or missing references)
		//IL_1472: Unknown result type (might be due to invalid IL or missing references)
		//IL_1482: Unknown result type (might be due to invalid IL or missing references)
		//IL_1489: Unknown result type (might be due to invalid IL or missing references)
		//IL_148e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1490: Unknown result type (might be due to invalid IL or missing references)
		//IL_14d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_14de: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_14e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_14fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_1501: Unknown result type (might be due to invalid IL or missing references)
		//IL_1503: Unknown result type (might be due to invalid IL or missing references)
		//IL_154a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1551: Unknown result type (might be due to invalid IL or missing references)
		//IL_1556: Unknown result type (might be due to invalid IL or missing references)
		//IL_1558: Unknown result type (might be due to invalid IL or missing references)
		//IL_1568: Unknown result type (might be due to invalid IL or missing references)
		//IL_156f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1574: Unknown result type (might be due to invalid IL or missing references)
		//IL_1576: Unknown result type (might be due to invalid IL or missing references)
		//IL_15bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_15c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_15cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_15db: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_15e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1630: Unknown result type (might be due to invalid IL or missing references)
		//IL_1637: Unknown result type (might be due to invalid IL or missing references)
		//IL_163c: Unknown result type (might be due to invalid IL or missing references)
		//IL_163e: Unknown result type (might be due to invalid IL or missing references)
		//IL_164e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1655: Unknown result type (might be due to invalid IL or missing references)
		//IL_165a: Unknown result type (might be due to invalid IL or missing references)
		//IL_165c: Unknown result type (might be due to invalid IL or missing references)
		//IL_169b: Unknown result type (might be due to invalid IL or missing references)
		//IL_16a2: Expected O, but got Unknown
		//IL_16af: Unknown result type (might be due to invalid IL or missing references)
		//IL_16be: Unknown result type (might be due to invalid IL or missing references)
		//IL_16c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_16de: Unknown result type (might be due to invalid IL or missing references)
		//IL_16e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_16ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_16fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1705: Unknown result type (might be due to invalid IL or missing references)
		//IL_170f: Unknown result type (might be due to invalid IL or missing references)
		//IL_171e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1725: Unknown result type (might be due to invalid IL or missing references)
		//IL_172f: Unknown result type (might be due to invalid IL or missing references)
		//IL_173e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1745: Unknown result type (might be due to invalid IL or missing references)
		//IL_174f: Unknown result type (might be due to invalid IL or missing references)
		//IL_175e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1765: Unknown result type (might be due to invalid IL or missing references)
		//IL_176f: Unknown result type (might be due to invalid IL or missing references)
		//IL_177e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1785: Unknown result type (might be due to invalid IL or missing references)
		//IL_178f: Unknown result type (might be due to invalid IL or missing references)
		//IL_179e: Unknown result type (might be due to invalid IL or missing references)
		//IL_17a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_17af: Unknown result type (might be due to invalid IL or missing references)
		//IL_17be: Unknown result type (might be due to invalid IL or missing references)
		//IL_17c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_17cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_17de: Unknown result type (might be due to invalid IL or missing references)
		//IL_17e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_17ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_17fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1805: Unknown result type (might be due to invalid IL or missing references)
		//IL_180f: Unknown result type (might be due to invalid IL or missing references)
		//IL_181e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1825: Unknown result type (might be due to invalid IL or missing references)
		//IL_182f: Unknown result type (might be due to invalid IL or missing references)
		//IL_183e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1845: Unknown result type (might be due to invalid IL or missing references)
		//IL_184f: Unknown result type (might be due to invalid IL or missing references)
		//IL_185e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1865: Unknown result type (might be due to invalid IL or missing references)
		//IL_186f: Unknown result type (might be due to invalid IL or missing references)
		//IL_187e: Unknown result type (might be due to invalid IL or missing references)
		//IL_1885: Unknown result type (might be due to invalid IL or missing references)
		//IL_188f: Unknown result type (might be due to invalid IL or missing references)
		//IL_189e: Unknown result type (might be due to invalid IL or missing references)
		//IL_18a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_18af: Unknown result type (might be due to invalid IL or missing references)
		//IL_18be: Unknown result type (might be due to invalid IL or missing references)
		//IL_18c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_18cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_18de: Unknown result type (might be due to invalid IL or missing references)
		//IL_18e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_18ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_18fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_1905: Unknown result type (might be due to invalid IL or missing references)
		//IL_1911: Unknown result type (might be due to invalid IL or missing references)
		//IL_1918: Expected O, but got Unknown
		//IL_1925: Unknown result type (might be due to invalid IL or missing references)
		//IL_1934: Unknown result type (might be due to invalid IL or missing references)
		//IL_193b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1945: Unknown result type (might be due to invalid IL or missing references)
		//IL_1954: Unknown result type (might be due to invalid IL or missing references)
		//IL_195b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1965: Unknown result type (might be due to invalid IL or missing references)
		//IL_1974: Unknown result type (might be due to invalid IL or missing references)
		//IL_197b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1985: Unknown result type (might be due to invalid IL or missing references)
		//IL_1994: Unknown result type (might be due to invalid IL or missing references)
		//IL_199b: Unknown result type (might be due to invalid IL or missing references)
		//IL_19a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_19bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_19c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_19db: Unknown result type (might be due to invalid IL or missing references)
		//IL_19e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_19f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_19fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a05: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a45: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a65: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1a9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1aa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ab4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1abb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ad4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1adb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ae5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1af4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1afb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b05: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b14: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b25: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b34: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b45: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b54: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b65: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1b9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ba7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bae: Expected O, but got Unknown
		//IL_1bbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1be7: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ed9: Expected O, but got Unknown
		//IL_1ee6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eed: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ef4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f04: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d83: Expected O, but got Unknown
		//IL_1d90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1db0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dbf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ddf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1de6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1df0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1dff: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e06: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e46: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e66: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e86: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1e9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ea6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1eb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ebf: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ec6: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c26: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c2d: Expected O, but got Unknown
		//IL_1c3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c49: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c70: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c89: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c90: Unknown result type (might be due to invalid IL or missing references)
		//IL_1c9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ca9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cb0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cba: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cc9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cd0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cda: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ce9: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cf0: Unknown result type (might be due to invalid IL or missing references)
		//IL_1cfa: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d09: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d10: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d1a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d29: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d30: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d3a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d49: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d50: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d5a: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d69: Unknown result type (might be due to invalid IL or missing references)
		//IL_1d70: Unknown result type (might be due to invalid IL or missing references)
		//IL_2027: Unknown result type (might be due to invalid IL or missing references)
		//IL_202e: Expected O, but got Unknown
		//IL_203b: Unknown result type (might be due to invalid IL or missing references)
		//IL_204a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2051: Unknown result type (might be due to invalid IL or missing references)
		//IL_205b: Unknown result type (might be due to invalid IL or missing references)
		//IL_206a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2071: Unknown result type (might be due to invalid IL or missing references)
		//IL_207b: Unknown result type (might be due to invalid IL or missing references)
		//IL_208a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2091: Unknown result type (might be due to invalid IL or missing references)
		//IL_209b: Unknown result type (might be due to invalid IL or missing references)
		//IL_20aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_20b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_20bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_20d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_20db: Unknown result type (might be due to invalid IL or missing references)
		//IL_20ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_20f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f51: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f58: Expected O, but got Unknown
		//IL_1f65: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f74: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f85: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_1f9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fa5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fb4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_1fe5: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ff4: Unknown result type (might be due to invalid IL or missing references)
		//IL_1ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2005: Unknown result type (might be due to invalid IL or missing references)
		//IL_2014: Unknown result type (might be due to invalid IL or missing references)
		//IL_201b: Unknown result type (might be due to invalid IL or missing references)
		//IL_20fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_2104: Expected O, but got Unknown
		//IL_2111: Unknown result type (might be due to invalid IL or missing references)
		//IL_2120: Unknown result type (might be due to invalid IL or missing references)
		//IL_2127: Unknown result type (might be due to invalid IL or missing references)
		//IL_2131: Unknown result type (might be due to invalid IL or missing references)
		//IL_2140: Unknown result type (might be due to invalid IL or missing references)
		//IL_2147: Unknown result type (might be due to invalid IL or missing references)
		//IL_2151: Unknown result type (might be due to invalid IL or missing references)
		//IL_2160: Unknown result type (might be due to invalid IL or missing references)
		//IL_2167: Unknown result type (might be due to invalid IL or missing references)
		//IL_2171: Unknown result type (might be due to invalid IL or missing references)
		//IL_2180: Unknown result type (might be due to invalid IL or missing references)
		//IL_2187: Unknown result type (might be due to invalid IL or missing references)
		//IL_2191: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_21a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_21c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_21e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_21f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2200: Unknown result type (might be due to invalid IL or missing references)
		//IL_2207: Unknown result type (might be due to invalid IL or missing references)
		//IL_2211: Unknown result type (might be due to invalid IL or missing references)
		//IL_2220: Unknown result type (might be due to invalid IL or missing references)
		//IL_2227: Unknown result type (might be due to invalid IL or missing references)
		//IL_2231: Unknown result type (might be due to invalid IL or missing references)
		//IL_2240: Unknown result type (might be due to invalid IL or missing references)
		//IL_2247: Unknown result type (might be due to invalid IL or missing references)
		//IL_2251: Unknown result type (might be due to invalid IL or missing references)
		//IL_2260: Unknown result type (might be due to invalid IL or missing references)
		//IL_2267: Unknown result type (might be due to invalid IL or missing references)
		//IL_2271: Unknown result type (might be due to invalid IL or missing references)
		//IL_2280: Unknown result type (might be due to invalid IL or missing references)
		//IL_2287: Unknown result type (might be due to invalid IL or missing references)
		//IL_2291: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_22a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_22b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_22c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_22d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_22e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_22f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2300: Unknown result type (might be due to invalid IL or missing references)
		//IL_2307: Unknown result type (might be due to invalid IL or missing references)
		//IL_2311: Unknown result type (might be due to invalid IL or missing references)
		//IL_2320: Unknown result type (might be due to invalid IL or missing references)
		//IL_2327: Unknown result type (might be due to invalid IL or missing references)
		//IL_2331: Unknown result type (might be due to invalid IL or missing references)
		//IL_2340: Unknown result type (might be due to invalid IL or missing references)
		//IL_2347: Unknown result type (might be due to invalid IL or missing references)
		//IL_2351: Unknown result type (might be due to invalid IL or missing references)
		//IL_2360: Unknown result type (might be due to invalid IL or missing references)
		//IL_2367: Unknown result type (might be due to invalid IL or missing references)
		//IL_2371: Unknown result type (might be due to invalid IL or missing references)
		//IL_2380: Unknown result type (might be due to invalid IL or missing references)
		//IL_2387: Unknown result type (might be due to invalid IL or missing references)
		//IL_2404: Unknown result type (might be due to invalid IL or missing references)
		//IL_246c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2476: Expected O, but got Unknown
		//IL_2476: Unknown result type (might be due to invalid IL or missing references)
		//IL_2480: Expected O, but got Unknown
		//IL_2486: Unknown result type (might be due to invalid IL or missing references)
		//IL_248d: Expected O, but got Unknown
		//IL_249a: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_24a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_24bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_24c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_24cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_24cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_24e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_24eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_24f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_2509: Unknown result type (might be due to invalid IL or missing references)
		//IL_2510: Unknown result type (might be due to invalid IL or missing references)
		//IL_2515: Unknown result type (might be due to invalid IL or missing references)
		//IL_2517: Unknown result type (might be due to invalid IL or missing references)
		//IL_252e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2535: Unknown result type (might be due to invalid IL or missing references)
		//IL_253a: Unknown result type (might be due to invalid IL or missing references)
		//IL_253c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2553: Unknown result type (might be due to invalid IL or missing references)
		//IL_255a: Unknown result type (might be due to invalid IL or missing references)
		//IL_255f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2561: Unknown result type (might be due to invalid IL or missing references)
		//IL_256d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2574: Expected O, but got Unknown
		//IL_25a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ac: Expected O, but got Unknown
		//IL_25b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_25de: Unknown result type (might be due to invalid IL or missing references)
		//IL_25e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_25ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_2603: Unknown result type (might be due to invalid IL or missing references)
		//IL_260a: Unknown result type (might be due to invalid IL or missing references)
		//IL_260f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2611: Unknown result type (might be due to invalid IL or missing references)
		//IL_2628: Unknown result type (might be due to invalid IL or missing references)
		//IL_262f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2634: Unknown result type (might be due to invalid IL or missing references)
		//IL_2636: Unknown result type (might be due to invalid IL or missing references)
		//IL_264d: Unknown result type (might be due to invalid IL or missing references)
		//IL_2654: Unknown result type (might be due to invalid IL or missing references)
		//IL_2659: Unknown result type (might be due to invalid IL or missing references)
		//IL_265b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2672: Unknown result type (might be due to invalid IL or missing references)
		//IL_2679: Unknown result type (might be due to invalid IL or missing references)
		//IL_267e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2680: Unknown result type (might be due to invalid IL or missing references)
		//IL_268c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2693: Expected O, but got Unknown
		//IL_26c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_26cf: Expected O, but got Unknown
		//IL_26dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_26e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_26ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_26fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2701: Unknown result type (might be due to invalid IL or missing references)
		//IL_2706: Unknown result type (might be due to invalid IL or missing references)
		//IL_2708: Unknown result type (might be due to invalid IL or missing references)
		//IL_27b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_280a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2811: Expected O, but got Unknown
		//IL_281e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2825: Unknown result type (might be due to invalid IL or missing references)
		//IL_282a: Unknown result type (might be due to invalid IL or missing references)
		//IL_282c: Unknown result type (might be due to invalid IL or missing references)
		//IL_283c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2843: Unknown result type (might be due to invalid IL or missing references)
		//IL_2848: Unknown result type (might be due to invalid IL or missing references)
		//IL_284a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2889: Unknown result type (might be due to invalid IL or missing references)
		//IL_2890: Expected O, but got Unknown
		//IL_289d: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_28b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_28bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_28cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_28d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_28dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_28ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_28f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_28fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_290c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2913: Unknown result type (might be due to invalid IL or missing references)
		//IL_291d: Unknown result type (might be due to invalid IL or missing references)
		//IL_292c: Unknown result type (might be due to invalid IL or missing references)
		//IL_2933: Unknown result type (might be due to invalid IL or missing references)
		//IL_29b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_2a73: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ac5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2acc: Expected O, but got Unknown
		//IL_2ad9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ae8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2aef: Unknown result type (might be due to invalid IL or missing references)
		//IL_2af9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b08: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b19: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b28: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b39: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b48: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b59: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b68: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b79: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b88: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2b99: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ba8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2baf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2be8: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bef: Unknown result type (might be due to invalid IL or missing references)
		//IL_2bf9: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c08: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c22: Expected O, but got Unknown
		//IL_2c2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c45: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c65: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2c9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ca5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2caf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cbe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ccf: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cde: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ce5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cef: Unknown result type (might be due to invalid IL or missing references)
		//IL_2cfe: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d05: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d0f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d1e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d25: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d2f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d45: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d5e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d65: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d7e: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d91: Unknown result type (might be due to invalid IL or missing references)
		//IL_2d98: Expected O, but got Unknown
		//IL_2da5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2db4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dc5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dd4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ddb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2de5: Unknown result type (might be due to invalid IL or missing references)
		//IL_2df4: Unknown result type (might be due to invalid IL or missing references)
		//IL_2dfb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e05: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e14: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e25: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e34: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e45: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e54: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e65: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e74: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e85: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e94: Unknown result type (might be due to invalid IL or missing references)
		//IL_2e9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ea7: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eae: Expected O, but got Unknown
		//IL_2ebb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eca: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ed1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2edb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2eea: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ef1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2efb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f0a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f11: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f1b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f2a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f31: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f3b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f4a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f51: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f5b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f6a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f71: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f7b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f91: Unknown result type (might be due to invalid IL or missing references)
		//IL_2f9b: Unknown result type (might be due to invalid IL or missing references)
		//IL_2faa: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fb1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fbb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fca: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fd1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fdb: Unknown result type (might be due to invalid IL or missing references)
		//IL_2fea: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ff1: Unknown result type (might be due to invalid IL or missing references)
		//IL_2ffb: Unknown result type (might be due to invalid IL or missing references)
		//IL_300a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3011: Unknown result type (might be due to invalid IL or missing references)
		//IL_301d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3024: Expected O, but got Unknown
		//IL_3031: Unknown result type (might be due to invalid IL or missing references)
		//IL_3040: Unknown result type (might be due to invalid IL or missing references)
		//IL_3047: Unknown result type (might be due to invalid IL or missing references)
		//IL_3051: Unknown result type (might be due to invalid IL or missing references)
		//IL_3060: Unknown result type (might be due to invalid IL or missing references)
		//IL_3067: Unknown result type (might be due to invalid IL or missing references)
		//IL_3071: Unknown result type (might be due to invalid IL or missing references)
		//IL_3080: Unknown result type (might be due to invalid IL or missing references)
		//IL_3087: Unknown result type (might be due to invalid IL or missing references)
		//IL_3091: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_30a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_30b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_30c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_30c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_313f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3191: Unknown result type (might be due to invalid IL or missing references)
		//IL_3198: Expected O, but got Unknown
		//IL_31a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_31b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_31bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_31c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_31d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_31db: Unknown result type (might be due to invalid IL or missing references)
		//IL_31e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_31f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_31fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3205: Unknown result type (might be due to invalid IL or missing references)
		//IL_3214: Unknown result type (might be due to invalid IL or missing references)
		//IL_321b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3225: Unknown result type (might be due to invalid IL or missing references)
		//IL_3234: Unknown result type (might be due to invalid IL or missing references)
		//IL_323b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3245: Unknown result type (might be due to invalid IL or missing references)
		//IL_3254: Unknown result type (might be due to invalid IL or missing references)
		//IL_325b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3265: Unknown result type (might be due to invalid IL or missing references)
		//IL_3274: Unknown result type (might be due to invalid IL or missing references)
		//IL_327b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3285: Unknown result type (might be due to invalid IL or missing references)
		//IL_3294: Unknown result type (might be due to invalid IL or missing references)
		//IL_329b: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_32a9: Expected O, but got Unknown
		//IL_32b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_32c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_32cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_32d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_32e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_32ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_32f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3305: Unknown result type (might be due to invalid IL or missing references)
		//IL_330c: Unknown result type (might be due to invalid IL or missing references)
		//IL_33b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_340a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3411: Expected O, but got Unknown
		//IL_341e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3425: Unknown result type (might be due to invalid IL or missing references)
		//IL_342a: Unknown result type (might be due to invalid IL or missing references)
		//IL_342c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3443: Unknown result type (might be due to invalid IL or missing references)
		//IL_344a: Unknown result type (might be due to invalid IL or missing references)
		//IL_344f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3451: Unknown result type (might be due to invalid IL or missing references)
		//IL_3468: Unknown result type (might be due to invalid IL or missing references)
		//IL_346f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3474: Unknown result type (might be due to invalid IL or missing references)
		//IL_3476: Unknown result type (might be due to invalid IL or missing references)
		//IL_348d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3494: Unknown result type (might be due to invalid IL or missing references)
		//IL_3499: Unknown result type (might be due to invalid IL or missing references)
		//IL_349b: Unknown result type (might be due to invalid IL or missing references)
		//IL_34a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_34ae: Expected O, but got Unknown
		//IL_34df: Unknown result type (might be due to invalid IL or missing references)
		//IL_34e6: Expected O, but got Unknown
		//IL_34f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_34fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_34ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_3501: Unknown result type (might be due to invalid IL or missing references)
		//IL_3518: Unknown result type (might be due to invalid IL or missing references)
		//IL_351f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3524: Unknown result type (might be due to invalid IL or missing references)
		//IL_3526: Unknown result type (might be due to invalid IL or missing references)
		//IL_353d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3544: Unknown result type (might be due to invalid IL or missing references)
		//IL_3549: Unknown result type (might be due to invalid IL or missing references)
		//IL_354b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3562: Unknown result type (might be due to invalid IL or missing references)
		//IL_3569: Unknown result type (might be due to invalid IL or missing references)
		//IL_356e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3570: Unknown result type (might be due to invalid IL or missing references)
		//IL_357c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3583: Expected O, but got Unknown
		//IL_35b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_35bb: Expected O, but got Unknown
		//IL_35c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_35cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_35d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_35d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_35ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_35f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_35f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_35fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3612: Unknown result type (might be due to invalid IL or missing references)
		//IL_3619: Unknown result type (might be due to invalid IL or missing references)
		//IL_361e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3620: Unknown result type (might be due to invalid IL or missing references)
		//IL_3637: Unknown result type (might be due to invalid IL or missing references)
		//IL_363e: Unknown result type (might be due to invalid IL or missing references)
		//IL_3643: Unknown result type (might be due to invalid IL or missing references)
		//IL_3645: Unknown result type (might be due to invalid IL or missing references)
		//IL_3651: Unknown result type (might be due to invalid IL or missing references)
		//IL_3658: Expected O, but got Unknown
		//IL_3689: Unknown result type (might be due to invalid IL or missing references)
		//IL_3690: Expected O, but got Unknown
		//IL_369d: Unknown result type (might be due to invalid IL or missing references)
		//IL_36a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_36a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_36c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_36d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_36e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_36ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_36f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_36f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_370c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3713: Unknown result type (might be due to invalid IL or missing references)
		//IL_3718: Unknown result type (might be due to invalid IL or missing references)
		//IL_371a: Unknown result type (might be due to invalid IL or missing references)
		//IL_3726: Unknown result type (might be due to invalid IL or missing references)
		//IL_372d: Expected O, but got Unknown
		//IL_375d: Unknown result type (might be due to invalid IL or missing references)
		//IL_3764: Expected O, but got Unknown
		//IL_3771: Unknown result type (might be due to invalid IL or missing references)
		//IL_3780: Unknown result type (might be due to invalid IL or missing references)
		//IL_3787: Unknown result type (might be due to invalid IL or missing references)
		//IL_3791: Unknown result type (might be due to invalid IL or missing references)
		//IL_37a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_37a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_37b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_37c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_37c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_37d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_37e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_37e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_3864: Unknown result type (might be due to invalid IL or missing references)
		//IL_3927: Unknown result type (might be due to invalid IL or missing references)
		//IL_39ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a43: Expected O, but got Unknown
		//IL_3a50: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a66: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a70: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a86: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a90: Unknown result type (might be due to invalid IL or missing references)
		//IL_3a9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aa6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ab0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ac6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ad0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3adf: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ae6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3af0: Unknown result type (might be due to invalid IL or missing references)
		//IL_3aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b06: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b10: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b26: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b32: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b39: Expected O, but got Unknown
		//IL_3b46: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b66: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b86: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_3b9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ba6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3be6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3bfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c06: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c15: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c26: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c35: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c46: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c55: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c66: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c75: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c86: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c95: Unknown result type (might be due to invalid IL or missing references)
		//IL_3c9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ca6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cc6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ce6: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_3cfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d06: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d15: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d26: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d35: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d46: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d55: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d68: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d6f: Expected O, but got Unknown
		//IL_3d7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d92: Unknown result type (might be due to invalid IL or missing references)
		//IL_3d9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dab: Unknown result type (might be due to invalid IL or missing references)
		//IL_3db2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dbc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ddc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3deb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3df2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3dfc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e1c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e32: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e3c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e52: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e5c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e72: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e92: Unknown result type (might be due to invalid IL or missing references)
		//IL_3e9c: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eab: Unknown result type (might be due to invalid IL or missing references)
		//IL_3eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ebc: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ecb: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ed2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ede: Unknown result type (might be due to invalid IL or missing references)
		//IL_3ee5: Expected O, but got Unknown
		//IL_3ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f01: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f08: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f21: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f28: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f32: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f41: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f48: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f52: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f61: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f68: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f72: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f81: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f88: Unknown result type (might be due to invalid IL or missing references)
		//IL_3f92: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fa1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_3fc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4045: Unknown result type (might be due to invalid IL or missing references)
		//IL_40ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_40b7: Expected O, but got Unknown
		//IL_40b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_40c1: Expected O, but got Unknown
		//IL_4137: Unknown result type (might be due to invalid IL or missing references)
		//IL_419f: Unknown result type (might be due to invalid IL or missing references)
		//IL_41a9: Expected O, but got Unknown
		//IL_41a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_41b3: Expected O, but got Unknown
		//IL_4229: Unknown result type (might be due to invalid IL or missing references)
		//IL_4291: Unknown result type (might be due to invalid IL or missing references)
		//IL_429b: Expected O, but got Unknown
		//IL_429b: Unknown result type (might be due to invalid IL or missing references)
		//IL_42a5: Expected O, but got Unknown
		//IL_440d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4475: Unknown result type (might be due to invalid IL or missing references)
		//IL_447f: Expected O, but got Unknown
		//IL_447f: Unknown result type (might be due to invalid IL or missing references)
		//IL_4489: Expected O, but got Unknown
		//IL_431b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4383: Unknown result type (might be due to invalid IL or missing references)
		//IL_438d: Expected O, but got Unknown
		//IL_438d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4397: Expected O, but got Unknown
		//IL_44ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_4567: Unknown result type (might be due to invalid IL or missing references)
		//IL_4571: Expected O, but got Unknown
		//IL_4571: Unknown result type (might be due to invalid IL or missing references)
		//IL_457b: Expected O, but got Unknown
		//IL_45f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4659: Unknown result type (might be due to invalid IL or missing references)
		//IL_4663: Expected O, but got Unknown
		//IL_4663: Unknown result type (might be due to invalid IL or missing references)
		//IL_466d: Expected O, but got Unknown
		//IL_46e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_474b: Unknown result type (might be due to invalid IL or missing references)
		//IL_4755: Expected O, but got Unknown
		//IL_4755: Unknown result type (might be due to invalid IL or missing references)
		//IL_475f: Expected O, but got Unknown
		//IL_47d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_483d: Unknown result type (might be due to invalid IL or missing references)
		//IL_4847: Expected O, but got Unknown
		//IL_4847: Unknown result type (might be due to invalid IL or missing references)
		//IL_4851: Expected O, but got Unknown
		//IL_48c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_498a: Unknown result type (might be due to invalid IL or missing references)
		//IL_49f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_49fc: Expected O, but got Unknown
		//IL_49fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_4a06: Expected O, but got Unknown
		//IL_4a7c: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4aee: Expected O, but got Unknown
		//IL_4aee: Unknown result type (might be due to invalid IL or missing references)
		//IL_4af8: Expected O, but got Unknown
		//IL_4b6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bd6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4be0: Expected O, but got Unknown
		//IL_4be0: Unknown result type (might be due to invalid IL or missing references)
		//IL_4bea: Expected O, but got Unknown
		//IL_4c60: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cd2: Expected O, but got Unknown
		//IL_4cd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_4cdc: Expected O, but got Unknown
		//IL_4d52: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dba: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dc4: Expected O, but got Unknown
		//IL_4dc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_4dce: Expected O, but got Unknown
		//IL_4e44: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eac: Unknown result type (might be due to invalid IL or missing references)
		//IL_4eb6: Expected O, but got Unknown
		//IL_4eb6: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ec0: Expected O, but got Unknown
		//IL_4f36: Unknown result type (might be due to invalid IL or missing references)
		//IL_4f9e: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fa8: Expected O, but got Unknown
		//IL_4fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fb2: Expected O, but got Unknown
		//IL_4fb7: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fbe: Expected O, but got Unknown
		//IL_4fcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fda: Unknown result type (might be due to invalid IL or missing references)
		//IL_4fe1: Unknown result type (might be due to invalid IL or missing references)
		//IL_4feb: Unknown result type (might be due to invalid IL or missing references)
		//IL_4ffa: Unknown result type (might be due to invalid IL or missing references)
		//IL_5001: Unknown result type (might be due to invalid IL or missing references)
		//IL_500b: Unknown result type (might be due to invalid IL or missing references)
		//IL_501a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5021: Unknown result type (might be due to invalid IL or missing references)
		//IL_502b: Unknown result type (might be due to invalid IL or missing references)
		//IL_503a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5041: Unknown result type (might be due to invalid IL or missing references)
		//IL_504b: Unknown result type (might be due to invalid IL or missing references)
		//IL_505a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5061: Unknown result type (might be due to invalid IL or missing references)
		//IL_506b: Unknown result type (might be due to invalid IL or missing references)
		//IL_507a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5081: Unknown result type (might be due to invalid IL or missing references)
		//IL_508b: Unknown result type (might be due to invalid IL or missing references)
		//IL_509a: Unknown result type (might be due to invalid IL or missing references)
		//IL_50a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_50ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_50ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_50c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_50cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_50da: Unknown result type (might be due to invalid IL or missing references)
		//IL_50e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_50ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_50f4: Expected O, but got Unknown
		//IL_5101: Unknown result type (might be due to invalid IL or missing references)
		//IL_5110: Unknown result type (might be due to invalid IL or missing references)
		//IL_5117: Unknown result type (might be due to invalid IL or missing references)
		//IL_5121: Unknown result type (might be due to invalid IL or missing references)
		//IL_5130: Unknown result type (might be due to invalid IL or missing references)
		//IL_5137: Unknown result type (might be due to invalid IL or missing references)
		//IL_5141: Unknown result type (might be due to invalid IL or missing references)
		//IL_5150: Unknown result type (might be due to invalid IL or missing references)
		//IL_5157: Unknown result type (might be due to invalid IL or missing references)
		//IL_5161: Unknown result type (might be due to invalid IL or missing references)
		//IL_5170: Unknown result type (might be due to invalid IL or missing references)
		//IL_5177: Unknown result type (might be due to invalid IL or missing references)
		//IL_5181: Unknown result type (might be due to invalid IL or missing references)
		//IL_5190: Unknown result type (might be due to invalid IL or missing references)
		//IL_5197: Unknown result type (might be due to invalid IL or missing references)
		//IL_51a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_51b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_51b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_51c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_51d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_51d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_51e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_51f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_51f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5201: Unknown result type (might be due to invalid IL or missing references)
		//IL_5210: Unknown result type (might be due to invalid IL or missing references)
		//IL_5217: Unknown result type (might be due to invalid IL or missing references)
		//IL_5223: Unknown result type (might be due to invalid IL or missing references)
		//IL_522a: Expected O, but got Unknown
		//IL_5237: Unknown result type (might be due to invalid IL or missing references)
		//IL_5246: Unknown result type (might be due to invalid IL or missing references)
		//IL_524d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5257: Unknown result type (might be due to invalid IL or missing references)
		//IL_5266: Unknown result type (might be due to invalid IL or missing references)
		//IL_526d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5277: Unknown result type (might be due to invalid IL or missing references)
		//IL_5286: Unknown result type (might be due to invalid IL or missing references)
		//IL_528d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5297: Unknown result type (might be due to invalid IL or missing references)
		//IL_52a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_52ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_52b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_52c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_52cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_52d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_52e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_52ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_536a: Unknown result type (might be due to invalid IL or missing references)
		//IL_53d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_53dc: Expected O, but got Unknown
		//IL_53dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_53e6: Expected O, but got Unknown
		//IL_545c: Unknown result type (might be due to invalid IL or missing references)
		//IL_54c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_54ce: Expected O, but got Unknown
		//IL_54ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_54d8: Expected O, but got Unknown
		//IL_54dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_54e4: Expected O, but got Unknown
		//IL_54f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_5500: Unknown result type (might be due to invalid IL or missing references)
		//IL_5507: Unknown result type (might be due to invalid IL or missing references)
		//IL_5511: Unknown result type (might be due to invalid IL or missing references)
		//IL_5520: Unknown result type (might be due to invalid IL or missing references)
		//IL_5527: Unknown result type (might be due to invalid IL or missing references)
		//IL_5531: Unknown result type (might be due to invalid IL or missing references)
		//IL_5540: Unknown result type (might be due to invalid IL or missing references)
		//IL_5547: Unknown result type (might be due to invalid IL or missing references)
		//IL_555b: Unknown result type (might be due to invalid IL or missing references)
		//IL_556a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5571: Unknown result type (might be due to invalid IL or missing references)
		//IL_557b: Unknown result type (might be due to invalid IL or missing references)
		//IL_558a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5591: Unknown result type (might be due to invalid IL or missing references)
		//IL_559b: Unknown result type (might be due to invalid IL or missing references)
		//IL_55aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_55b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_55c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_55d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_55db: Unknown result type (might be due to invalid IL or missing references)
		//IL_55e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_55f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_55fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_5605: Unknown result type (might be due to invalid IL or missing references)
		//IL_5614: Unknown result type (might be due to invalid IL or missing references)
		//IL_561b: Unknown result type (might be due to invalid IL or missing references)
		//IL_562f: Unknown result type (might be due to invalid IL or missing references)
		//IL_563e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5645: Unknown result type (might be due to invalid IL or missing references)
		//IL_564f: Unknown result type (might be due to invalid IL or missing references)
		//IL_565e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5665: Unknown result type (might be due to invalid IL or missing references)
		//IL_566f: Unknown result type (might be due to invalid IL or missing references)
		//IL_567e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5685: Unknown result type (might be due to invalid IL or missing references)
		//IL_5699: Unknown result type (might be due to invalid IL or missing references)
		//IL_56a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_56af: Unknown result type (might be due to invalid IL or missing references)
		//IL_56b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_56c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_56cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_56d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_56e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_56ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_56f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5708: Unknown result type (might be due to invalid IL or missing references)
		//IL_570f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5723: Unknown result type (might be due to invalid IL or missing references)
		//IL_5732: Unknown result type (might be due to invalid IL or missing references)
		//IL_5739: Unknown result type (might be due to invalid IL or missing references)
		//IL_5743: Unknown result type (might be due to invalid IL or missing references)
		//IL_5752: Unknown result type (might be due to invalid IL or missing references)
		//IL_5759: Unknown result type (might be due to invalid IL or missing references)
		//IL_5763: Unknown result type (might be due to invalid IL or missing references)
		//IL_5772: Unknown result type (might be due to invalid IL or missing references)
		//IL_5779: Unknown result type (might be due to invalid IL or missing references)
		//IL_5783: Unknown result type (might be due to invalid IL or missing references)
		//IL_5792: Unknown result type (might be due to invalid IL or missing references)
		//IL_5799: Unknown result type (might be due to invalid IL or missing references)
		//IL_57ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_57bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_57c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_57cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_57dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_57e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_57ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_57fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_5803: Unknown result type (might be due to invalid IL or missing references)
		//IL_580d: Unknown result type (might be due to invalid IL or missing references)
		//IL_581c: Unknown result type (might be due to invalid IL or missing references)
		//IL_5823: Unknown result type (might be due to invalid IL or missing references)
		//IL_5837: Unknown result type (might be due to invalid IL or missing references)
		//IL_5846: Unknown result type (might be due to invalid IL or missing references)
		//IL_584d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5857: Unknown result type (might be due to invalid IL or missing references)
		//IL_5866: Unknown result type (might be due to invalid IL or missing references)
		//IL_586d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5877: Unknown result type (might be due to invalid IL or missing references)
		//IL_5886: Unknown result type (might be due to invalid IL or missing references)
		//IL_588d: Unknown result type (might be due to invalid IL or missing references)
		//IL_5897: Unknown result type (might be due to invalid IL or missing references)
		//IL_58a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_58ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_58c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_58d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_58d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_58e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_58f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_58f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_5901: Unknown result type (might be due to invalid IL or missing references)
		//IL_5910: Unknown result type (might be due to invalid IL or missing references)
		//IL_5917: Unknown result type (might be due to invalid IL or missing references)
		//IL_5921: Unknown result type (might be due to invalid IL or missing references)
		//IL_5930: Unknown result type (might be due to invalid IL or missing references)
		//IL_5937: Unknown result type (might be due to invalid IL or missing references)
		//IL_594b: Unknown result type (might be due to invalid IL or missing references)
		//IL_595a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5961: Unknown result type (might be due to invalid IL or missing references)
		//IL_596b: Unknown result type (might be due to invalid IL or missing references)
		//IL_597a: Unknown result type (might be due to invalid IL or missing references)
		//IL_5981: Unknown result type (might be due to invalid IL or missing references)
		//IL_598b: Unknown result type (might be due to invalid IL or missing references)
		//IL_599a: Unknown result type (might be due to invalid IL or missing references)
		//IL_59a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_59ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_59ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_59c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_59d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_59e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_59eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_59f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a24: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a6e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a7f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a8e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a95: Unknown result type (might be due to invalid IL or missing references)
		//IL_5a9f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aae: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ab5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5abf: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ace: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ad5: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ae9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5af8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b09: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b18: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b29: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b38: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b49: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b58: Unknown result type (might be due to invalid IL or missing references)
		//IL_5b5f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5bdc: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c44: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c4e: Expected O, but got Unknown
		//IL_5c4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_5c58: Expected O, but got Unknown
		//IL_5cce: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d36: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d40: Expected O, but got Unknown
		//IL_5d40: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d4a: Expected O, but got Unknown
		//IL_5d4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d56: Expected O, but got Unknown
		//IL_5d63: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d72: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d79: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d83: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d92: Unknown result type (might be due to invalid IL or missing references)
		//IL_5d99: Unknown result type (might be due to invalid IL or missing references)
		//IL_5da3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5db2: Unknown result type (might be due to invalid IL or missing references)
		//IL_5db9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dc3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dd2: Unknown result type (might be due to invalid IL or missing references)
		//IL_5dd9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5de3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5df2: Unknown result type (might be due to invalid IL or missing references)
		//IL_5df9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e03: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e12: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e19: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e23: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e32: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e39: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e43: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e52: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e59: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e63: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e72: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e79: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e83: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e92: Unknown result type (might be due to invalid IL or missing references)
		//IL_5e99: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ea3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5eb2: Unknown result type (might be due to invalid IL or missing references)
		//IL_5eb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ec3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ed2: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ed9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ee3: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ef2: Unknown result type (might be due to invalid IL or missing references)
		//IL_5ef9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f03: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f12: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f19: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f25: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f2c: Expected O, but got Unknown
		//IL_5f39: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f48: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f4f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f59: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f68: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f6f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f79: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f88: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f8f: Unknown result type (might be due to invalid IL or missing references)
		//IL_5f99: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fa8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5faf: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fb9: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fc8: Unknown result type (might be due to invalid IL or missing references)
		//IL_5fcf: Unknown result type (might be due to invalid IL or missing references)
		//IL_604c: Unknown result type (might be due to invalid IL or missing references)
		//IL_60b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_60be: Expected O, but got Unknown
		//IL_60be: Unknown result type (might be due to invalid IL or missing references)
		//IL_60c8: Expected O, but got Unknown
		//IL_60cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_60d4: Expected O, but got Unknown
		//IL_60e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_60f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_60f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_6101: Unknown result type (might be due to invalid IL or missing references)
		//IL_6110: Unknown result type (might be due to invalid IL or missing references)
		//IL_6117: Unknown result type (might be due to invalid IL or missing references)
		//IL_6121: Unknown result type (might be due to invalid IL or missing references)
		//IL_6130: Unknown result type (might be due to invalid IL or missing references)
		//IL_6137: Unknown result type (might be due to invalid IL or missing references)
		//IL_6141: Unknown result type (might be due to invalid IL or missing references)
		//IL_6150: Unknown result type (might be due to invalid IL or missing references)
		//IL_6157: Unknown result type (might be due to invalid IL or missing references)
		//IL_6161: Unknown result type (might be due to invalid IL or missing references)
		//IL_6170: Unknown result type (might be due to invalid IL or missing references)
		//IL_6177: Unknown result type (might be due to invalid IL or missing references)
		//IL_6181: Unknown result type (might be due to invalid IL or missing references)
		//IL_6190: Unknown result type (might be due to invalid IL or missing references)
		//IL_6197: Unknown result type (might be due to invalid IL or missing references)
		//IL_61a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_61b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_61b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_61c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_61ca: Expected O, but got Unknown
		//IL_61d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_61e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_61ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_61f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_6206: Unknown result type (might be due to invalid IL or missing references)
		//IL_620d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6217: Unknown result type (might be due to invalid IL or missing references)
		//IL_6226: Unknown result type (might be due to invalid IL or missing references)
		//IL_622d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6237: Unknown result type (might be due to invalid IL or missing references)
		//IL_6246: Unknown result type (might be due to invalid IL or missing references)
		//IL_624d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6257: Unknown result type (might be due to invalid IL or missing references)
		//IL_6266: Unknown result type (might be due to invalid IL or missing references)
		//IL_626d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6277: Unknown result type (might be due to invalid IL or missing references)
		//IL_6286: Unknown result type (might be due to invalid IL or missing references)
		//IL_628d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6297: Unknown result type (might be due to invalid IL or missing references)
		//IL_62a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_62ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_62b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_62c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_62cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_62d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_62e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_62ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_62f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_6306: Unknown result type (might be due to invalid IL or missing references)
		//IL_630d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6317: Unknown result type (might be due to invalid IL or missing references)
		//IL_6326: Unknown result type (might be due to invalid IL or missing references)
		//IL_632d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6337: Unknown result type (might be due to invalid IL or missing references)
		//IL_6346: Unknown result type (might be due to invalid IL or missing references)
		//IL_634d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6357: Unknown result type (might be due to invalid IL or missing references)
		//IL_6366: Unknown result type (might be due to invalid IL or missing references)
		//IL_636d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6377: Unknown result type (might be due to invalid IL or missing references)
		//IL_6386: Unknown result type (might be due to invalid IL or missing references)
		//IL_638d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6397: Unknown result type (might be due to invalid IL or missing references)
		//IL_63a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_63ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_63b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_63c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_63cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_63d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_63e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_63ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_63f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_6406: Unknown result type (might be due to invalid IL or missing references)
		//IL_640d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6417: Unknown result type (might be due to invalid IL or missing references)
		//IL_6426: Unknown result type (might be due to invalid IL or missing references)
		//IL_642d: Unknown result type (might be due to invalid IL or missing references)
		//IL_6439: Unknown result type (might be due to invalid IL or missing references)
		//IL_6440: Expected O, but got Unknown
		//IL_644d: Unknown result type (might be due to invalid IL or missing references)
		//IL_645c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6463: Unknown result type (might be due to invalid IL or missing references)
		//IL_646d: Unknown result type (might be due to invalid IL or missing references)
		//IL_647c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6483: Unknown result type (might be due to invalid IL or missing references)
		//IL_648d: Unknown result type (might be due to invalid IL or missing references)
		//IL_649c: Unknown result type (might be due to invalid IL or missing references)
		//IL_64a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_64ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_64bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_64c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_64cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_64dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_64e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_64ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_64fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_6503: Unknown result type (might be due to invalid IL or missing references)
		//IL_650d: Unknown result type (might be due to invalid IL or missing references)
		//IL_651c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6523: Unknown result type (might be due to invalid IL or missing references)
		//IL_652d: Unknown result type (might be due to invalid IL or missing references)
		//IL_653c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6543: Unknown result type (might be due to invalid IL or missing references)
		//IL_654d: Unknown result type (might be due to invalid IL or missing references)
		//IL_655c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6563: Unknown result type (might be due to invalid IL or missing references)
		//IL_656d: Unknown result type (might be due to invalid IL or missing references)
		//IL_657c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6583: Unknown result type (might be due to invalid IL or missing references)
		//IL_658d: Unknown result type (might be due to invalid IL or missing references)
		//IL_659c: Unknown result type (might be due to invalid IL or missing references)
		//IL_65a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_65ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_65bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_65c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_65cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_65dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_65e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_65ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_65fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_6603: Unknown result type (might be due to invalid IL or missing references)
		//IL_660d: Unknown result type (might be due to invalid IL or missing references)
		//IL_661c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6623: Unknown result type (might be due to invalid IL or missing references)
		//IL_662d: Unknown result type (might be due to invalid IL or missing references)
		//IL_663c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6643: Unknown result type (might be due to invalid IL or missing references)
		//IL_664d: Unknown result type (might be due to invalid IL or missing references)
		//IL_665c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6663: Unknown result type (might be due to invalid IL or missing references)
		//IL_666d: Unknown result type (might be due to invalid IL or missing references)
		//IL_667c: Unknown result type (might be due to invalid IL or missing references)
		//IL_6683: Unknown result type (might be due to invalid IL or missing references)
		//IL_668d: Unknown result type (might be due to invalid IL or missing references)
		//IL_669c: Unknown result type (might be due to invalid IL or missing references)
		//IL_66a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_66af: Unknown result type (might be due to invalid IL or missing references)
		//IL_66b6: Expected O, but got Unknown
		//IL_66c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_66d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_66d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_66e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_66f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_66f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_6703: Unknown result type (might be due to invalid IL or missing references)
		//IL_6712: Unknown result type (might be due to invalid IL or missing references)
		//IL_6719: Unknown result type (might be due to invalid IL or missing references)
		//IL_6723: Unknown result type (might be due to invalid IL or missing references)
		//IL_6732: Unknown result type (might be due to invalid IL or missing references)
		//IL_6739: Unknown result type (might be due to invalid IL or missing references)
		//IL_6743: Unknown result type (might be due to invalid IL or missing references)
		//IL_6752: Unknown result type (might be due to invalid IL or missing references)
		//IL_6759: Unknown result type (might be due to invalid IL or missing references)
		//IL_6763: Unknown result type (might be due to invalid IL or missing references)
		//IL_6772: Unknown result type (might be due to invalid IL or missing references)
		//IL_6779: Unknown result type (might be due to invalid IL or missing references)
		//IL_6783: Unknown result type (might be due to invalid IL or missing references)
		//IL_6792: Unknown result type (might be due to invalid IL or missing references)
		//IL_6799: Unknown result type (might be due to invalid IL or missing references)
		//IL_6816: Unknown result type (might be due to invalid IL or missing references)
		//IL_687e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6888: Expected O, but got Unknown
		//IL_6888: Unknown result type (might be due to invalid IL or missing references)
		//IL_6892: Expected O, but got Unknown
		//IL_6897: Unknown result type (might be due to invalid IL or missing references)
		//IL_689e: Expected O, but got Unknown
		//IL_68ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_68ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_68c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_68cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_68da: Unknown result type (might be due to invalid IL or missing references)
		//IL_68e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_68eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_68fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_6901: Unknown result type (might be due to invalid IL or missing references)
		//IL_690b: Unknown result type (might be due to invalid IL or missing references)
		//IL_691a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6921: Unknown result type (might be due to invalid IL or missing references)
		//IL_692b: Unknown result type (might be due to invalid IL or missing references)
		//IL_693a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6941: Unknown result type (might be due to invalid IL or missing references)
		//IL_694b: Unknown result type (might be due to invalid IL or missing references)
		//IL_695a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6961: Unknown result type (might be due to invalid IL or missing references)
		//IL_696b: Unknown result type (might be due to invalid IL or missing references)
		//IL_697a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6981: Unknown result type (might be due to invalid IL or missing references)
		//IL_698b: Unknown result type (might be due to invalid IL or missing references)
		//IL_699a: Unknown result type (might be due to invalid IL or missing references)
		//IL_69a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_69ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_69ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_69c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_69d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_69e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_69eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_69f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a04: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a15: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a24: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a44: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a55: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a64: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a75: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a84: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6a95: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aa4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aab: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ab5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ac4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6acb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ad5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ae4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aeb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6aff: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b0e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b15: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b1f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b2e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b35: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b3f: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b4e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b55: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b61: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b68: Expected O, but got Unknown
		//IL_6b75: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b84: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6b95: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ba4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bab: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bb5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bc4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bcb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bd5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6be4: Unknown result type (might be due to invalid IL or missing references)
		//IL_6beb: Unknown result type (might be due to invalid IL or missing references)
		//IL_6bf5: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c04: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c0b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c15: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c24: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c2b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c35: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c44: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c4b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c57: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c5e: Expected O, but got Unknown
		//IL_6c6b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c7a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c81: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c8b: Unknown result type (might be due to invalid IL or missing references)
		//IL_6c9a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ca1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cab: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cba: Unknown result type (might be due to invalid IL or missing references)
		//IL_6cc1: Unknown result type (might be due to invalid IL or missing references)
		//IL_6d3e: Unknown result type (might be due to invalid IL or missing references)
		//IL_6da6: Unknown result type (might be due to invalid IL or missing references)
		//IL_6db0: Expected O, but got Unknown
		//IL_6db0: Unknown result type (might be due to invalid IL or missing references)
		//IL_6dba: Expected O, but got Unknown
		//IL_6e30: Unknown result type (might be due to invalid IL or missing references)
		//IL_6e98: Unknown result type (might be due to invalid IL or missing references)
		//IL_6ea2: Expected O, but got Unknown
		//IL_6ea2: Unknown result type (might be due to invalid IL or missing references)
		//IL_6eac: Expected O, but got Unknown
		//IL_6f22: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f8a: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f94: Expected O, but got Unknown
		//IL_6f94: Unknown result type (might be due to invalid IL or missing references)
		//IL_6f9e: Expected O, but got Unknown
		bool flag = false;
		int num = -1;
		if (m_NextId >= 256)
		{
			m_NextId = 0;
		}
		for (int i = m_NextId; i < 256; i++)
		{
			if (m_Item[i].m_Id == -1)
			{
				num = i;
				flag = true;
				m_NextId = i + 1;
				break;
			}
		}
		if (!flag)
		{
			for (int j = 0; j < 256; j++)
			{
				if (m_Item[j].m_Id == -1)
				{
					num = j;
					flag = true;
					m_NextId = j + 1;
					break;
				}
			}
		}
		if (num == -1)
		{
			return -1;
		}
		m_Item[num].m_Type = type;
		m_Item[num].m_Position = pos;
		m_Item[num].m_ZDistance = 0f;
		m_Item[num].m_Rotation = Vector3.Zero;
		m_Item[num].m_Rotation.Z = rot;
		m_Item[num].m_TriggerPos = pos;
		m_Item[num].m_TriggerId = triggerId;
		m_Item[num].m_Id = num;
		m_Item[num].m_fScale = 1f;
		m_Item[num].m_bCastShadows = true;
		m_Item[num].m_Layer = 0;
		m_Item[num].m_Bounds3DMax = Program.m_ItemManager.m_ModelSizeMax3D[type];
		m_Item[num].m_Bounds3DMin = Program.m_ItemManager.m_ModelSizeMin3D[type];
		m_Item[num].m_Velocity = Vector2.Zero;
		m_Item[num].m_bHasPlayerCollision = true;
		if (m_Model[type] == null)
		{
			Delete(num);
			return -1;
		}
		m_Item[num].SetModel(m_Model[type], m_ModelLOD[type]);
		Vector2 zero = Vector2.Zero;
		zero = m_Item[num].m_Position;
		Fixture val = null;
		switch (type)
		{
		case 0:
		case 4:
		case 5:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 1.524f, 1.524f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture27 = m_Item[num].m_Fixture;
			fixture27.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture27.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 1:
		case 19:
		case 37:
		case 77:
		case 84:
		case 85:
		case 86:
		{
			PolygonShape val16 = new PolygonShape();
			val16.SetAsEdge(TrigRotate(new Vector2(-3.95986f, 0f), rot) + zero, TrigRotate(new Vector2(-0.5461f, 1.95834f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val16, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val16.SetAsEdge(TrigRotate(new Vector2(-0.5461f, 1.95834f), rot) + zero, TrigRotate(new Vector2(-0.00762f, 1.905f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val16, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val16.SetAsEdge(TrigRotate(new Vector2(-0.00762f, 1.905f), rot) + zero, TrigRotate(new Vector2(0.60705996f, 1.9659599f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val16, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val16.SetAsEdge(TrigRotate(new Vector2(0.60705996f, 1.9659599f), rot) + zero, TrigRotate(new Vector2(3.97256f, 0f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val16, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 2:
		{
			Vertices val14 = new Vertices(6);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-5.17144f, 10.16f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(5.2527204f, 10.2108f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(5.2527204f, 11.341101f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(4.0106597f, 11.66114f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-3.9979599f, 11.65098f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-5.17144f, 11.417299f), rot) + zero);
			PolygonShape val15 = new PolygonShape(val14);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val15, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val14 = new Vertices(4);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-4.3789597f, 3.61696f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-4.3789597f, 4.37134f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-5.27304f, 4.37134f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(-5.27304f, 3.61696f), rot) + zero);
			val15 = new PolygonShape(val14);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val15, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val14 = new Vertices(4);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(5.27304f, 3.61696f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(5.27304f, 4.37134f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(4.3789597f, 4.37134f), rot) + zero);
			((List<Vector2>)(object)val14).Add(TrigRotate(new Vector2(4.3789597f, 3.61696f), rot) + zero);
			val15 = new PolygonShape(val14);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val15, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 3:
		{
			Vertices val19 = new Vertices(5);
			((List<Vector2>)(object)val19).Clear();
			((List<Vector2>)(object)val19).Add(TrigRotate(new Vector2(-2.16408f, 0.07874f), rot) + zero);
			((List<Vector2>)(object)val19).Add(TrigRotate(new Vector2(-1.8465799f, 0f), rot) + zero);
			((List<Vector2>)(object)val19).Add(TrigRotate(new Vector2(2.159f, 0f), rot) + zero);
			((List<Vector2>)(object)val19).Add(TrigRotate(new Vector2(2.159f, 0.15494f), rot) + zero);
			((List<Vector2>)(object)val19).Add(TrigRotate(new Vector2(-1.8465799f, 0.15494f), rot) + zero);
			PolygonShape val20 = new PolygonShape(val19);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val20, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 6:
		case 7:
		case 8:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 1.524f, 1.524f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 9:
		{
			PolygonShape val18 = new PolygonShape();
			val18.SetAsEdge(TrigRotate(new Vector2(-3.81f, 0.0508f), rot) + zero, TrigRotate(new Vector2(3.81f, 0.0508f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val18, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 10:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 5.1485796f, 2.54f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 20f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			break;
		case 11:
		{
			PolygonShape val17 = new PolygonShape();
			val17.SetAsEdge(TrigRotate(new Vector2(-3.81f, 0.127f), rot) + zero, TrigRotate(new Vector2(3.81f, 0.127f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val17, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 12:
			m_Item[num].m_bCastShadows = false;
			break;
		case 21:
			m_Item[num].m_UniqueId = (Program.m_App.m_Level - 1) * 3;
			if (m_FlagsCollected[m_Item[num].m_UniqueId] == 1)
			{
				Delete(num);
				return -1;
			}
			break;
		case 22:
			m_Item[num].m_UniqueId = (Program.m_App.m_Level - 1) * 3 + 1;
			if (m_FlagsCollected[m_Item[num].m_UniqueId] == 1)
			{
				Delete(num);
				return -1;
			}
			break;
		case 23:
			m_Item[num].m_UniqueId = (Program.m_App.m_Level - 1) * 3 + 2;
			if (m_FlagsCollected[m_Item[num].m_UniqueId] == 1)
			{
				Delete(num);
				return -1;
			}
			break;
		case 15:
		{
			PolygonShape val13 = new PolygonShape();
			val13.SetAsEdge(TrigRotate(new Vector2(-6.24078f, 0.00254f), rot) + zero, TrigRotate(new Vector2(2.64668f, 0.00254f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val13, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val13.SetAsEdge(TrigRotate(new Vector2(2.64668f, 0.00254f), rot) + zero, TrigRotate(new Vector2(2.6542997f, 0.37846f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val13, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val13.SetAsEdge(TrigRotate(new Vector2(2.6542997f, 0.37846f), rot) + zero, TrigRotate(new Vector2(5.29844f, 0.29463997f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val13, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val13.SetAsEdge(TrigRotate(new Vector2(5.29844f, 0.29463997f), rot) + zero, TrigRotate(new Vector2(6.23316f, 0.29463997f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val13, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val13.SetAsEdge(TrigRotate(new Vector2(6.23316f, 0.29463997f), rot) + zero, TrigRotate(new Vector2(6.23316f, -0.29463997f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val13, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 16:
		{
			PolygonShape val12 = new PolygonShape();
			val12.SetAsEdge(TrigRotate(new Vector2(-6.23316f, -0.2921f), rot) + zero, TrigRotate(new Vector2(-6.23316f, 0.28956f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val12, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val12.SetAsEdge(TrigRotate(new Vector2(-6.23316f, 0.28956f), rot) + zero, TrigRotate(new Vector2(-5.29844f, 0.29463997f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val12, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val12.SetAsEdge(TrigRotate(new Vector2(-5.29844f, 0.29463997f), rot) + zero, TrigRotate(new Vector2(-2.6542997f, 0.37846f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val12, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val12.SetAsEdge(TrigRotate(new Vector2(-2.6542997f, 0.37846f), rot) + zero, TrigRotate(new Vector2(-2.64668f, 0.00254f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val12, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val12.SetAsEdge(TrigRotate(new Vector2(-2.64668f, 0.00254f), rot) + zero, TrigRotate(new Vector2(6.24078f, 0.00254f), rot) + zero);
			Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val12, 0f);
			break;
		}
		case 17:
		{
			PolygonShape val11 = new PolygonShape();
			val11.SetAsEdge(TrigRotate(new Vector2(-3.81f, 0.127f), rot) + zero, TrigRotate(new Vector2(3.81f, 0.127f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val11, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 18:
		{
			PolygonShape val10 = new PolygonShape();
			val10.SetAsEdge(TrigRotate(new Vector2(-7.01802f, 5.2857404f), rot) + zero, TrigRotate(new Vector2(-8.86714f, 5.2857404f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(-8.86714f, 5.2857404f), rot) + zero, TrigRotate(new Vector2(-8.851899f, 7.0485f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(-8.851899f, 7.0485f), rot) + zero, TrigRotate(new Vector2(-9.687559f, 7.05866f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(-8.851899f, 7.0485f), rot) + zero, TrigRotate(new Vector2(-9.796781f, 7.20852f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(-9.796781f, 7.20852f), rot) + zero, TrigRotate(new Vector2(-9.796781f, 7.6073f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(-9.796781f, 7.6073f), rot) + zero, TrigRotate(new Vector2(-9.65454f, 7.7343f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(-9.65454f, 7.7343f), rot) + zero, TrigRotate(new Vector2(9.64438f, 7.7343f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(9.64438f, 7.7343f), rot) + zero, TrigRotate(new Vector2(9.78662f, 7.6479397f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val10.SetAsEdge(TrigRotate(new Vector2(9.78662f, 7.6479397f), rot) + zero, TrigRotate(new Vector2(9.80694f, 7.2212195f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val10, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 20:
		{
			PolygonShape shape31 = new PolygonShape();
			AddEdge(shape31, new Vector2(-2.521f, 2.819f), new Vector2(-2.243f, 2.801f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(-1.967f, 2.743f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(-1.694f, 2.647f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(-1.426f, 2.512f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(-1.165f, 2.34f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(-0.912f, 2.131f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(-0.67f, 1.888f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(-0.439f, 1.611f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(-0.222f, 1.302f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(-0.018f, 0.963f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(0.198f, 0.634f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(0.439f, 0.37f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(0.701f, 0.176f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(0.976f, 0.058f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(1.258f, 0.018f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(1.541f, 0.058f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(1.816f, 0.176f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(2.319f, 0.633f), rot, zero);
			AddEdge(shape31, Vector2.Zero, new Vector2(2.525f, 0.962f), rot, zero);
			break;
		}
		case 24:
		{
			PolygonShape shape30 = new PolygonShape();
			AddEdge(shape30, new Vector2(-2.525f, 0.962f), new Vector2(-2.319f, 0.633f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(-2.078f, 0.37f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(-1.816f, 0.176f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(-1.541f, 0.058f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(-1.258f, 0.018f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(-0.976f, 0.058f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(-0.701f, 0.176f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(-0.439f, 0.37f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(-0.198f, 0.634f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(0.018f, 0.963f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(0.222f, 1.302f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(0.439f, 1.611f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(0.67f, 1.888f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(0.912f, 2.131f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(1.165f, 2.34f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(1.426f, 2.512f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(1.694f, 2.647f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(1.967f, 2.743f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(2.243f, 2.801f), rot, zero);
			AddEdge(shape30, Vector2.Zero, new Vector2(2.521f, 2.819f), rot, zero);
			break;
		}
		case 25:
		{
			PolygonShape val9 = new PolygonShape();
			val9.SetAsEdge(TrigRotate(new Vector2(-12.7f, 0.0508f), rot) + zero, TrigRotate(new Vector2(12.7f, 0.0508f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val9, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 28:
		{
			PolygonShape shape29 = new PolygonShape();
			AddEdge(shape29, new Vector2(-5f, 0.01f), new Vector2(-4.01f, -0.016f), rot, zero);
			AddEdge(shape29, Vector2.Zero, new Vector2(-3.032f, -0.095f), rot, zero);
			AddEdge(shape29, Vector2.Zero, new Vector2(-2.075f, -0.224f), rot, zero);
			AddEdge(shape29, Vector2.Zero, new Vector2(-1.15f, -0.402f), rot, zero);
			AddEdge(shape29, Vector2.Zero, new Vector2(-0.267f, -0.627f), rot, zero);
			AddEdge(shape29, Vector2.Zero, new Vector2(0.564f, -0.898f), rot, zero);
			AddEdge(shape29, Vector2.Zero, new Vector2(1.332f, -1.21f), rot, zero);
			AddEdge(shape29, Vector2.Zero, new Vector2(2.032f, -1.561f), rot, zero);
			AddEdge(shape29, Vector2.Zero, new Vector2(2.655f, -1.947f), rot, zero);
			AddEdge(shape29, Vector2.Zero, new Vector2(3.192f, -2.363f), rot, zero);
			break;
		}
		case 27:
		{
			PolygonShape shape28 = new PolygonShape();
			AddEdge(shape28, new Vector2(-5f, 0.011f), new Vector2(-3.994f, 0.037f), rot, zero);
			AddEdge(shape28, Vector2.Zero, new Vector2(-2.998f, 0.117f), rot, zero);
			AddEdge(shape28, Vector2.Zero, new Vector2(-2.025f, 0.248f), rot, zero);
			AddEdge(shape28, Vector2.Zero, new Vector2(-1.085f, 0.429f), rot, zero);
			AddEdge(shape28, Vector2.Zero, new Vector2(-0.187f, 0.659f), rot, zero);
			AddEdge(shape28, Vector2.Zero, new Vector2(0.657f, 0.934f), rot, zero);
			AddEdge(shape28, Vector2.Zero, new Vector2(1.44f, 1.255f), rot, zero);
			AddEdge(shape28, Vector2.Zero, new Vector2(2.151f, 1.609f), rot, zero);
			AddEdge(shape28, Vector2.Zero, new Vector2(2.784f, 2.001f), rot, zero);
			AddEdge(shape28, Vector2.Zero, new Vector2(3.332f, 2.424f), rot, zero);
			break;
		}
		case 26:
		{
			PolygonShape val8 = new PolygonShape();
			val8.SetAsEdge(TrigRotate(new Vector2(-6.35f, 0.0508f), rot) + zero, TrigRotate(new Vector2(6.35f, 0.0508f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val8, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 30:
		{
			PolygonShape shape27 = new PolygonShape();
			AddEdge(shape27, new Vector2(-2.5f, 0.013f), new Vector2(-1.671f, -0.026f), rot, zero);
			AddEdge(shape27, Vector2.Zero, new Vector2(-0.848f, -0.134f), rot, zero);
			AddEdge(shape27, Vector2.Zero, new Vector2(-0.039f, -0.315f), rot, zero);
			AddEdge(shape27, Vector2.Zero, new Vector2(0.752f, -0.566f), rot, zero);
			AddEdge(shape27, Vector2.Zero, new Vector2(1.518f, -0.884f), rot, zero);
			AddEdge(shape27, Vector2.Zero, new Vector2(2.253f, -1.268f), rot, zero);
			break;
		}
		case 29:
		{
			PolygonShape shape26 = new PolygonShape();
			AddEdge(shape26, new Vector2(-2.5f, 0.01f), new Vector2(-1.666f, 0.048f), rot, zero);
			AddEdge(shape26, Vector2.Zero, new Vector2(-0.835f, 0.157f), rot, zero);
			AddEdge(shape26, Vector2.Zero, new Vector2(-0.018f, 0.34f), rot, zero);
			AddEdge(shape26, Vector2.Zero, new Vector2(0.778f, 0.593f), rot, zero);
			AddEdge(shape26, Vector2.Zero, new Vector2(1.552f, 0.913f), rot, zero);
			AddEdge(shape26, Vector2.Zero, new Vector2(2.293f, 1.3f), rot, zero);
			break;
		}
		case 31:
		{
			PolygonShape shape25 = new PolygonShape();
			AddEdge(shape25, new Vector2(-1.843f, 0.01f), new Vector2(-1.334f, 0.029f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(-0.835f, 0.083f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(-0.335f, 0.17f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(0.096f, 0.29f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(0.509f, 0.44f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(0.877f, 0.617f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(1.192f, 0.818f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(1.449f, 1.039f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(1.642f, 1.276f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(1.768f, 1.523f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(1.824f, 1.777f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(1.809f, 2.032f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(1.724f, 2.284f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(1.571f, 2.527f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(1.35f, 2.758f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(1.069f, 2.971f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(0.732f, 3.163f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(0.344f, 3.329f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(-0.085f, 3.467f), rot, zero);
			AddEdge(shape25, Vector2.Zero, new Vector2(-0.549f, 3.574f), rot, zero);
			break;
		}
		case 32:
		case 33:
		case 34:
		case 35:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 0.2032f, 1.524f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 5f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture26 = m_Item[num].m_Fixture;
			fixture26.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture26.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 36:
		{
			Vertices val6 = new Vertices(6);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(-4.89204f, 11.0109f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(-5.1815996f, 10.7061f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(-4.90728f, 10.39114f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(4.8894997f, 10.39114f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(5.17906f, 10.7061f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(4.89712f, 11.0109f), rot) + zero);
			PolygonShape val7 = new PolygonShape(val6);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val7, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val6 = new Vertices(6);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(-4.73964f, 5.4737f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(-4.8514f, 5.26288f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(-4.73964f, 5.0317397f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(4.76758f, 5.0317397f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(4.88696f, 5.26288f), rot) + zero);
			((List<Vector2>)(object)val6).Add(TrigRotate(new Vector2(4.76758f, 5.4737f), rot) + zero);
			val7 = new PolygonShape(val6);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val7, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 38:
		{
			PolygonShape val5 = new PolygonShape();
			val5.SetAsEdge(TrigRotate(new Vector2(-7.0358f, 0.2794f), rot) + zero, TrigRotate(new Vector2(7.0358f, 0.2794f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val5, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 39:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 12.7f, 7.62f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 40:
		{
			PolygonShape val4 = new PolygonShape();
			val4.SetAsEdge(TrigRotate(new Vector2(-3.683f, 0.0889f), rot) + zero, TrigRotate(new Vector2(3.683f, 0.0889f), rot) + zero);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val4, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			break;
		}
		case 41:
		{
			PolygonShape shape24 = new PolygonShape();
			AddEdge(shape24, new Vector2(-1.7f, 0f), new Vector2(0.353f, 0.012f), rot, zero);
			AddEdge(shape24, Vector2.Zero, new Vector2(0.42f, 0.086f), rot, zero);
			AddEdge(shape24, Vector2.Zero, new Vector2(1.454f, 0.086f), rot, zero);
			AddEdge(shape24, Vector2.Zero, new Vector2(1.665f, 0.045f), rot, zero);
			AddEdge(shape24, Vector2.Zero, new Vector2(1.7f, 0f), rot, zero);
			break;
		}
		case 42:
		case 43:
		case 44:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 2.54f, 4.572f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 45:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 3.556f, 4.572f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 46:
		{
			PolygonShape shape23 = new PolygonShape();
			AddEdge(shape23, new Vector2(-0.754f, 0f), new Vector2(-0.555f, 0.432f), rot, zero);
			AddEdge(shape23, Vector2.Zero, new Vector2(-0.51f, 0.462f), rot, zero);
			AddEdge(shape23, Vector2.Zero, new Vector2(-0.427f, 0.483f), rot, zero);
			AddEdge(shape23, Vector2.Zero, new Vector2(-0.367f, 0.49f), rot, zero);
			AddEdge(shape23, Vector2.Zero, new Vector2(0f, 0.499f), rot, zero);
			AddEdge(shape23, Vector2.Zero, new Vector2(0.367f, 0.49f), rot, zero);
			AddEdge(shape23, Vector2.Zero, new Vector2(0.427f, 0.483f), rot, zero);
			AddEdge(shape23, Vector2.Zero, new Vector2(0.51f, 0.462f), rot, zero);
			AddEdge(shape23, Vector2.Zero, new Vector2(0.555f, 0.432f), rot, zero);
			AddEdge(shape23, Vector2.Zero, new Vector2(0.754f, 0f), rot, zero);
			break;
		}
		case 47:
		{
			PolygonShape shape22 = new PolygonShape();
			AddEdge(shape22, new Vector2(-1.35f, 0f), new Vector2(-1.35f, 1.432f), rot, zero);
			AddEdge(shape22, Vector2.Zero, new Vector2(-1.778f, 1.484f), rot, zero);
			AddEdge(shape22, Vector2.Zero, new Vector2(-1.778f, 1.573f), rot, zero);
			AddEdge(shape22, Vector2.Zero, new Vector2(-1.35f, 1.625f), rot, zero);
			AddEdge(shape22, Vector2.Zero, new Vector2(-1.35f, 1.801f), rot, zero);
			AddEdge(shape22, Vector2.Zero, new Vector2(-1.319f, 1.871f), rot, zero);
			AddEdge(shape22, Vector2.Zero, new Vector2(-0.93f, 1.899f), rot, zero);
			AddEdge(shape22, Vector2.Zero, new Vector2(0.939f, 1.899f), rot, zero);
			AddEdge(shape22, Vector2.Zero, new Vector2(1.32f, 1.87f), rot, zero);
			AddEdge(shape22, Vector2.Zero, new Vector2(1.354f, 1.803f), rot, zero);
			AddEdge(shape22, Vector2.Zero, new Vector2(1.354f, 0f), rot, zero);
			break;
		}
		case 48:
		{
			PolygonShape shape21 = new PolygonShape();
			AddEdge(shape21, new Vector2(-1.616f, 0.017f), new Vector2(-1.616f, 0.077f), rot, zero);
			AddEdge(shape21, Vector2.Zero, new Vector2(-1.246f, 0.146f), rot, zero);
			AddEdge(shape21, Vector2.Zero, new Vector2(-1.041f, 0.155f), rot, zero);
			AddEdge(shape21, Vector2.Zero, new Vector2(-0.909f, 0.145f), rot, zero);
			AddEdge(shape21, Vector2.Zero, new Vector2(-0.45f, 0.035f), rot, zero);
			AddEdge(shape21, Vector2.Zero, new Vector2(-0.114f, 0.002f), rot, zero);
			AddEdge(shape21, Vector2.Zero, new Vector2(1.5f, 0f), rot, zero);
			AddEdge(shape21, Vector2.Zero, new Vector2(1.5f, -0.102f), rot, zero);
			break;
		}
		case 49:
		{
			PolygonShape shape20 = new PolygonShape();
			AddEdge(shape20, new Vector2(-0.378f, 0f), new Vector2(-0.325f, 0.345f), rot, zero);
			AddEdge(shape20, Vector2.Zero, new Vector2(-0.293f, 0.453f), rot, zero);
			AddEdge(shape20, Vector2.Zero, new Vector2(-0.275f, 0.48f), rot, zero);
			AddEdge(shape20, Vector2.Zero, new Vector2(-0.22f, 0.503f), rot, zero);
			AddEdge(shape20, Vector2.Zero, new Vector2(-0.123f, 0.514f), rot, zero);
			AddEdge(shape20, Vector2.Zero, new Vector2(0.123f, 0.514f), rot, zero);
			AddEdge(shape20, Vector2.Zero, new Vector2(0.22f, 0.503f), rot, zero);
			AddEdge(shape20, Vector2.Zero, new Vector2(0.275f, 0.48f), rot, zero);
			AddEdge(shape20, Vector2.Zero, new Vector2(0.293f, 0.453f), rot, zero);
			AddEdge(shape20, Vector2.Zero, new Vector2(0.325f, 0.345f), rot, zero);
			AddEdge(shape20, Vector2.Zero, new Vector2(0.378f, 0f), rot, zero);
			break;
		}
		case 50:
		{
			PolygonShape shape19 = new PolygonShape();
			AddEdge(shape19, new Vector2(-1.347f, 0f), new Vector2(-1.208f, 0.042f), rot, zero);
			AddEdge(shape19, Vector2.Zero, new Vector2(-1.118f, 0.108f), rot, zero);
			AddEdge(shape19, Vector2.Zero, new Vector2(1.118f, 0.108f), rot, zero);
			AddEdge(shape19, Vector2.Zero, new Vector2(1.208f, 0.042f), rot, zero);
			AddEdge(shape19, Vector2.Zero, new Vector2(1.347f, 0f), rot, zero);
			break;
		}
		case 51:
			m_Item[num].m_Fixture = FixtureFactory.CreateCircle(Program.m_App.m_World, 0.508f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 52:
		{
			PolygonShape shape18 = new PolygonShape();
			AddEdge(shape18, new Vector2(4.672f, 0.678f), new Vector2(0.697f, 0.405f), rot, zero);
			AddEdge(shape18, Vector2.Zero, new Vector2(-0.864f, 0.451f), rot, zero);
			AddEdge(shape18, Vector2.Zero, new Vector2(-2f, 0.587f), rot, zero);
			AddEdge(shape18, Vector2.Zero, new Vector2(-2.968f, 0.632f), rot, zero);
			AddEdge(shape18, Vector2.Zero, new Vector2(-3.826f, 0.577f), rot, zero);
			AddEdge(shape18, Vector2.Zero, new Vector2(-4.13f, 0.474f), rot, zero);
			AddEdge(shape18, Vector2.Zero, new Vector2(-6.874f, 0.13f), rot, zero);
			AddEdge(shape18, Vector2.Zero, new Vector2(-6.865f, 0f), rot, zero);
			shape18 = new PolygonShape();
			AddEdge(shape18, new Vector2(6.584f, 0.637f), new Vector2(6.639f, 0.815f), rot, zero);
			AddEdge(shape18, Vector2.Zero, new Vector2(6.776f, 0.829f), rot, zero);
			AddEdge(shape18, Vector2.Zero, new Vector2(6.861f, 0.66f), rot, zero);
			break;
		}
		case 53:
			m_Item[num].m_Fixture = FixtureFactory.CreateCircle(Program.m_App.m_World, 2.794f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 1f;
			m_Item[num].m_Fixture.Restitution = 0.75f;
			m_Item[num].m_Fixture.Body.Inertia = 0.1f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 54:
		{
			Vertices val2 = new Vertices(4);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-5.96392f, 5.9309f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-5.96392f, 4.1427402f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-4.73456f, 4.1427402f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-4.73456f, 5.9309f), rot) + zero);
			PolygonShape val3 = new PolygonShape(val2);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val3, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val2 = new Vertices(4);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(4.7802796f, 5.9334397f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(4.7802796f, 4.14528f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(6.0274196f, 4.14528f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(6.0274196f, 5.9334397f), rot) + zero);
			val3 = new PolygonShape(val2);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val3, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val2 = new Vertices(4);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-5.9842396f, 15.466061f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-5.9842396f, 14.25956f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-4.7371f, 14.25956f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(-4.7371f, 15.466061f), rot) + zero);
			val3 = new PolygonShape(val2);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val3, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val2 = new Vertices(4);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(4.7752f, 15.49146f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(4.7752f, 14.29004f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(6.02488f, 14.29004f), rot) + zero);
			((List<Vector2>)(object)val2).Add(TrigRotate(new Vector2(6.02488f, 15.49146f), rot) + zero);
			val3 = new PolygonShape(val2);
			val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)val3, 0f);
			val.CollisionCategories = (CollisionCategory)1073741824;
			val.CollidesWith = (CollisionCategory)1073741824;
			val3 = new PolygonShape();
			AddEdge(val3, new Vector2(-1.859f, 3.191f), new Vector2(-2.349f, 3.191f), rot, zero);
			AddEdge(val3, Vector2.Zero, new Vector2(-2.334f, 4.116f), rot, zero);
			AddEdge(val3, Vector2.Zero, new Vector2(2.342f, 4.116f), rot, zero);
			AddEdge(val3, Vector2.Zero, new Vector2(2.369f, 3.191f), rot, zero);
			break;
		}
		case 55:
		case 60:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 25.4f, 0.508f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 56:
		case 61:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 12.7f, 0.254f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 57:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 2.58318f, 1.5595601f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 59:
		{
			PolygonShape shape17 = new PolygonShape();
			AddEdge(shape17, new Vector2(-0.917f, 0f), new Vector2(0.917f, 0f), rot, zero);
			AddEdge(shape17, Vector2.Zero, new Vector2(0.915f, 0.301f), rot, zero);
			AddEdge(shape17, Vector2.Zero, new Vector2(0.848f, 0.327f), rot, zero);
			AddEdge(shape17, Vector2.Zero, new Vector2(0.629f, 1.747f), rot, zero);
			AddEdge(shape17, Vector2.Zero, new Vector2(-0.629f, 1.747f), rot, zero);
			AddEdge(shape17, Vector2.Zero, new Vector2(-0.848f, 0.327f), rot, zero);
			AddEdge(shape17, Vector2.Zero, new Vector2(-0.915f, 0.301f), rot, zero);
			break;
		}
		case 63:
		{
			PolygonShape shape16 = new PolygonShape();
			AddEdge(shape16, new Vector2(-1.282f, 0.052f), new Vector2(-1.281f, 0.214f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(-0.495f, 0.263f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(-0.383f, 0.307f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(-0.383f, 1.285f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(-2.625f, 1.285f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(-2.68f, 1.34f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(-2.68f, 4.714f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(-2.627f, 4.767f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(2.862f, 4.767f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(2.918f, 4.712f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(2.918f, 1.339f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(2.863f, 1.285f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(0.407f, 1.285f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(0.407f, 0.306f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(0.52f, 0.263f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(1.316f, 0.217f), rot, zero);
			AddEdge(shape16, Vector2.Zero, new Vector2(1.318f, 0.053f), rot, zero);
			break;
		}
		case 64:
		{
			PolygonShape shape15 = new PolygonShape();
			AddEdge(shape15, new Vector2(-2.221f, 0.05f), new Vector2(-2.221f, 0.131f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(-2.205f, 0.165f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(-2.175f, 0.181f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(-2.11f, 0.181f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(-2.068f, 0.255f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(2.069f, 0.255f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(2.097f, 0.181f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(2.175f, 0.181f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(2.208f, 0.165f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(2.211f, 0.131f), rot, zero);
			AddEdge(shape15, Vector2.Zero, new Vector2(2.211f, 0.05f), rot, zero);
			break;
		}
		case 65:
		{
			PolygonShape shape14 = new PolygonShape();
			AddEdge(shape14, new Vector2(-1.121f, 0.059f), new Vector2(-1.2f, 0.084f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(-1.142f, 0.105f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(1.076f, 0.107f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(2.607f, 1.171f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(2.625f, 1.159f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(2.613f, 1.024f), rot, zero);
			AddEdge(shape14, Vector2.Zero, new Vector2(1.15f, 0.011f), rot, zero);
			break;
		}
		case 66:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 1.6128999f, 3.0734f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture25 = m_Item[num].m_Fixture;
			fixture25.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture25.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 67:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 6.4515996f, 0.96774f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture24 = m_Item[num].m_Fixture;
			fixture24.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture24.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 68:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 3.429f, 1.8542f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 5f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture23 = m_Item[num].m_Fixture;
			fixture23.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture23.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 70:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 4.8259997f, 0.40385997f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 5f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture22 = m_Item[num].m_Fixture;
			fixture22.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture22.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 69:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 4.8259997f, 0.40385997f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture21 = m_Item[num].m_Fixture;
			fixture21.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture21.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 71:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 3.0988f, 0.28448f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture20 = m_Item[num].m_Fixture;
			fixture20.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture20.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 72:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 3.0988f, 0.28448f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture19 = m_Item[num].m_Fixture;
			fixture19.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture19.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 73:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 3.429f, 2.2885401f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture18 = m_Item[num].m_Fixture;
			fixture18.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture18.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 74:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 3.3528001f, 0.29463997f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture17 = m_Item[num].m_Fixture;
			fixture17.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture17.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 75:
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 1.524f, 1.524f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			break;
		case 76:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 1.524f, 1.524f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 7f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture16 = m_Item[num].m_Fixture;
			fixture16.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture16.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 78:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 3.83032f, 8.51154f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture15 = m_Item[num].m_Fixture;
			fixture15.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture15.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 79:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 2.1894798f, 4.76758f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture14 = m_Item[num].m_Fixture;
			fixture14.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture14.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 80:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 4.318f, 0.1778f, 1f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 1f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture13 = m_Item[num].m_Fixture;
			fixture13.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture13.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 81:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 4.318f, 0.1778f, 1f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 1f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture12 = m_Item[num].m_Fixture;
			fixture12.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture12.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 82:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 5.842f, 5.1409597f, 1f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 1f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture11 = m_Item[num].m_Fixture;
			fixture11.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture11.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 83:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 10.414f, 4.3307f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture10 = m_Item[num].m_Fixture;
			fixture10.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture10.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 87:
		{
			PolygonShape shape13 = new PolygonShape();
			AddEdge(shape13, new Vector2(-8.273f, 0f), new Vector2(-8.5f, 0.512f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(-8.5f, 5.274f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(-8.333f, 5.633f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(-7.898f, 5.78f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(7.877f, 5.78f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(8.19f, 5.625f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(8.383f, 5.272f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(8.383f, 0.505f), rot, zero);
			AddEdge(shape13, Vector2.Zero, new Vector2(8.122f, 0f), rot, zero);
			break;
		}
		case 88:
		{
			PolygonShape shape12 = new PolygonShape();
			AddEdge(shape12, new Vector2(-0.939f, 0f), new Vector2(-0.884f, 1.23f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(-1.59f, 2.809f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(-1.683f, 3.898f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(-1.607f, 3.913f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(1.762f, 3.913f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(1.812f, 3.898f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(1.737f, 2.8f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(0.917f, 0.915f), rot, zero);
			AddEdge(shape12, Vector2.Zero, new Vector2(1.098f, 0f), rot, zero);
			break;
		}
		case 89:
		case 90:
		case 91:
		{
			PolygonShape shape11 = new PolygonShape();
			AddEdge(shape11, new Vector2(-0.976f, 0f), new Vector2(-0.981f, 0.095f), rot, zero);
			AddEdge(shape11, Vector2.Zero, new Vector2(0.118f, 0.143f), rot, zero);
			AddEdge(shape11, Vector2.Zero, new Vector2(0.325f, 0.075f), rot, zero);
			AddEdge(shape11, Vector2.Zero, new Vector2(0.629f, 0.07f), rot, zero);
			AddEdge(shape11, Vector2.Zero, new Vector2(0.963f, 0.096f), rot, zero);
			AddEdge(shape11, Vector2.Zero, new Vector2(0.969f, 0.057f), rot, zero);
			break;
		}
		case 92:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 2.032f, 2.032f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture9 = m_Item[num].m_Fixture;
			fixture9.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture9.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 93:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 6.096f, 8.382f, 10f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 10f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture8 = m_Item[num].m_Fixture;
			fixture8.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture8.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 94:
		{
			PolygonShape shape10 = new PolygonShape();
			AddEdge(shape10, new Vector2(-2f, 0f), new Vector2(-2f, 0.138f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-1.895f, 0.138f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-1.895f, 0f), rot, zero);
			AddEdge(shape10, new Vector2(-1.671f, 0f), new Vector2(-1.671f, 0.138f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-1.56f, 0.138f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-1.56f, 0f), rot, zero);
			AddEdge(shape10, new Vector2(-0.291f, 0f), new Vector2(-0.291f, 0.138f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-0.18f, 0.138f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-0.18f, 0f), rot, zero);
			AddEdge(shape10, new Vector2(1.336f, 0f), new Vector2(1.336f, 0.138f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(1.446f, 0.138f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(1.446f, 0f), rot, zero);
			AddEdge(shape10, new Vector2(-0.171f, 1.515f), new Vector2(-0.171f, 1.575f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-0.11f, 1.575f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-0.11f, 1.515f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-0.171f, 1.515f), rot, zero);
			AddEdge(shape10, new Vector2(-0.171f, 4.478f), new Vector2(-0.171f, 4.538f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-0.11f, 4.538f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-0.11f, 4.478f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-0.171f, 4.478f), rot, zero);
			AddEdge(shape10, new Vector2(-0.171f, 7.46f), new Vector2(-0.171f, 7.52f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-0.11f, 7.52f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-0.11f, 7.46f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-0.171f, 7.46f), rot, zero);
			AddEdge(shape10, new Vector2(-1.68f, 2.987f), new Vector2(-1.68f, 3.04f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-1.619f, 3.04f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-1.619f, 2.987f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-1.68f, 2.987f), rot, zero);
			AddEdge(shape10, new Vector2(1.33f, 2.987f), new Vector2(1.33f, 3.04f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(1.39f, 3.04f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(1.39f, 2.987f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(1.33f, 2.987f), rot, zero);
			AddEdge(shape10, new Vector2(-1.68f, 5.968f), new Vector2(-1.68f, 6.028f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-1.619f, 6.028f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-1.619f, 5.968f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-1.68f, 5.968f), rot, zero);
			AddEdge(shape10, new Vector2(1.33f, 5.968f), new Vector2(1.33f, 6.028f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(1.39f, 6.028f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(1.39f, 5.968f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(1.33f, 5.968f), rot, zero);
			AddEdge(shape10, new Vector2(-1.68f, 8.944f), new Vector2(-1.68f, 9.003f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-1.619f, 9.003f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-1.619f, 8.944f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(-1.68f, 8.944f), rot, zero);
			AddEdge(shape10, new Vector2(1.33f, 8.944f), new Vector2(1.33f, 9.003f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(1.39f, 9.003f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(1.39f, 8.944f), rot, zero);
			AddEdge(shape10, Vector2.Zero, new Vector2(1.33f, 8.944f), rot, zero);
			break;
		}
		case 95:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 1.143f, 4.37388f, 5f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 5f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture7 = m_Item[num].m_Fixture;
			fixture7.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture7.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 96:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 1.27f, 3.5915601f, 5f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 5f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture6 = m_Item[num].m_Fixture;
			fixture6.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture6.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 97:
		{
			PolygonShape shape9 = new PolygonShape();
			AddEdge(shape9, new Vector2(-0.939f, 0f), new Vector2(-0.884f, 1.23f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(-1.59f, 2.809f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(-1.671f, 3.607f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(-1.615f, 3.723f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(-1.554f, 3.752f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(-1.354f, 3.752f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(0f, 1.603f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.576f, 3.752f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.698f, 3.752f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.76f, 3.723f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.81f, 3.606f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.737f, 2.8f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(0.917f, 0.915f), rot, zero);
			AddEdge(shape9, Vector2.Zero, new Vector2(1.098f, 0f), rot, zero);
			break;
		}
		case 98:
		{
			PolygonShape shape8 = new PolygonShape();
			AddEdge(shape8, new Vector2(-0.961f, 0.039f), new Vector2(0.772f, 0.393f), rot, zero);
			AddEdge(shape8, Vector2.Zero, new Vector2(0.826f, 0.284f), rot, zero);
			AddEdge(shape8, Vector2.Zero, new Vector2(0.951f, 0.292f), rot, zero);
			AddEdge(shape8, Vector2.Zero, new Vector2(0.967f, 0.133f), rot, zero);
			AddEdge(shape8, Vector2.Zero, new Vector2(0.812f, 0f), rot, zero);
			break;
		}
		case 99:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 0.97789997f, 4.77012f, 5f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 5f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture5 = m_Item[num].m_Fixture;
			fixture5.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture5.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 100:
		{
			PolygonShape shape7 = new PolygonShape();
			AddEdge(shape7, new Vector2(-3.895f, 0f), new Vector2(-3.895f, 6.901f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(-3.862f, 6.996f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(-3.791f, 7.034f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(3.791f, 7.034f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(3.862f, 6.996f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(3.895f, 6.901f), rot, zero);
			AddEdge(shape7, Vector2.Zero, new Vector2(3.895f, 0f), rot, zero);
			break;
		}
		case 101:
		{
			PolygonShape shape6 = new PolygonShape();
			AddEdge(shape6, new Vector2(-9.975f, 0f), new Vector2(-9.788f, 4.846f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-9.75f, 4.986f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-9.644f, 5.088f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-9.507f, 5.125f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-8.12f, 5.125f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-7.983f, 5.087f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-7.88f, 4.986f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-7.843f, 4.847f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(-7.843f, 3.182f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(7.815f, 3.182f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(7.815f, 4.847f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(7.852f, 4.986f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(7.954f, 5.088f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(8.092f, 5.124f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(9.479f, 5.124f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(9.618f, 5.087f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(9.72f, 4.986f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(9.756f, 4.847f), rot, zero);
			AddEdge(shape6, Vector2.Zero, new Vector2(10.091f, 0f), rot, zero);
			break;
		}
		case 102:
		{
			PolygonShape shape5 = new PolygonShape();
			AddEdge(shape5, new Vector2(-3.964f, 0f), new Vector2(-3.773f, 4.846f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(-3.78f, 4.986f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(-3.633f, 5.088f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(-3.498f, 5.125f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(-2.11f, 5.125f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(-1.972f, 5.087f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(-1.87f, 4.986f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(-1.832f, 4.847f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(-1.832f, 3.182f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(2.26f, 3.182f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(2.26f, 4.847f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(2.296f, 4.986f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(2.398f, 5.088f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(2.537f, 5.124f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(3.926f, 5.124f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(4.065f, 5.087f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(4.164f, 4.986f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(4.202f, 4.847f), rot, zero);
			AddEdge(shape5, Vector2.Zero, new Vector2(4.535f, 0f), rot, zero);
			break;
		}
		case 103:
		{
			PolygonShape shape4 = new PolygonShape();
			AddEdge(shape4, new Vector2(-5.524f, 3.806f), new Vector2(-5.497f, 3.874f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(-5.492f, 3.963f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(-5.462f, 3.982f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(5.466f, 3.982f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(5.497f, 3.963f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(5.502f, 3.874f), rot, zero);
			AddEdge(shape4, Vector2.Zero, new Vector2(5.529f, 3.806f), rot, zero);
			break;
		}
		case 105:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 5.3339996f, 0.381f, 5f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 5f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture4 = m_Item[num].m_Fixture;
			fixture4.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture4.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 106:
		{
			PolygonShape shape3 = new PolygonShape();
			AddEdge(shape3, new Vector2(-4.883f, 1.593f), new Vector2(-4.883f, 1.758f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(-4.877f, 1.807f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(-4.844f, 1.84f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(-4.803f, 1.852f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(4.875f, 1.852f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(4.916f, 1.84f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(4.948f, 1.807f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(4.959f, 1.758f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(4.946f, 1.593f), rot, zero);
			AddEdge(shape3, new Vector2(-4.883f, 3.973f), new Vector2(-4.883f, 4.137f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(-4.877f, 4.184f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(-4.844f, 4.217f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(-4.803f, 4.23f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(4.875f, 4.23f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(4.916f, 4.217f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(4.948f, 4.184f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(4.959f, 4.137f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(4.946f, 3.973f), rot, zero);
			AddEdge(shape3, new Vector2(-1.297f, 1.95f), new Vector2(-1.297f, 2.743f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(1.914f, 2.743f), rot, zero);
			AddEdge(shape3, Vector2.Zero, new Vector2(1.914f, 1.95f), rot, zero);
			break;
		}
		case 107:
		{
			PolygonShape shape2 = new PolygonShape();
			AddEdge(shape2, new Vector2(-0.381f, 0f), new Vector2(-0.378f, 1.894f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(-0.151f, 2.307f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(-0.121f, 2.914f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(0.121f, 2.914f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(0.151f, 2.307f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(0.378f, 1.894f), rot, zero);
			AddEdge(shape2, Vector2.Zero, new Vector2(0.381f, 0f), rot, zero);
			break;
		}
		case 108:
		{
			PolygonShape shape = new PolygonShape();
			AddEdge(shape, new Vector2(-0.395f, -1.504f), new Vector2(-0.521f, 1.502f), rot, zero);
			AddEdge(shape, Vector2.Zero, new Vector2(0.525f, 1.502f), rot, zero);
			AddEdge(shape, Vector2.Zero, new Vector2(0.399f, -1.504f), rot, zero);
			break;
		}
		case 109:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 9.398f, 6.35f, 5f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 5f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture3 = m_Item[num].m_Fixture;
			fixture3.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture3.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 110:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 6.35f, 0.635f, 5f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)0;
			m_Item[num].m_Fixture.Body.Mass = 5f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture2 = m_Item[num].m_Fixture;
			fixture2.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture2.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		case 111:
		{
			m_Item[num].m_Fixture = FixtureFactory.CreateRectangle(Program.m_App.m_World, 6.35f, 0.635f, 5f);
			m_Item[num].m_Fixture.Body.BodyType = (BodyType)2;
			m_Item[num].m_Fixture.Body.Mass = 5f;
			m_Item[num].m_Fixture.Body.Position = pos;
			m_Item[num].m_Fixture.Body.Rotation = rot;
			m_Item[num].m_Fixture.CollisionCategories = (CollisionCategory)1073741824;
			m_Item[num].m_Fixture.CollidesWith = (CollisionCategory)1073741824;
			Fixture fixture = m_Item[num].m_Fixture;
			fixture.OnCollision = (CollisionEventHandler)Delegate.Combine((Delegate)(object)fixture.OnCollision, (Delegate)new CollisionEventHandler(ItemOnCollision));
			break;
		}
		}
		Vector3 zero2 = Vector3.Zero;
		zero2.X = m_Item[num].m_Position.X;
		zero2.Y = m_Item[num].m_Position.Y;
		zero2.Z = m_Item[num].m_ZDistance;
		m_Item[num].m_ScreenPos = Program.m_CurrentCamera.WorldToScreen(zero2);
		if (flag)
		{
			return num;
		}
		return -1;
	}

	private void AddEdge(PolygonShape shape, Vector2 prev, Vector2 next, float rot, Vector2 off)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		if (prev == Vector2.Zero)
		{
			shape.SetAsEdge(TrigRotate(m_LastEdge * 2.54f, rot) + off, TrigRotate(next * 2.54f, rot) + off);
		}
		else
		{
			shape.SetAsEdge(TrigRotate(prev * 2.54f, rot) + off, TrigRotate(next * 2.54f, rot) + off);
		}
		Fixture val = Program.m_App.m_GroundBody.CreateFixture((Shape)(object)shape, 0f);
		val.CollisionCategories = (CollisionCategory)1073741824;
		val.CollidesWith = (CollisionCategory)1073741824;
		m_LastEdge = next;
	}

	private Vector2 TrigRotate(Vector2 v1, float rot)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Vector2 zero = Vector2.Zero;
		Matrix val = Matrix.CreateRotationZ(rot);
		return Vector2.Transform(v1, val);
	}

	protected virtual bool ItemOnCollision(Fixture FixtureB, Fixture FixtureA, Contact contact)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Invalid comparison between Unknown and I4
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Invalid comparison between Unknown and I4
		float num = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds;
		if (m_ItemImpactSoundTime < num)
		{
			Vector2 linearVelocity = FixtureB.Body.LinearVelocity;
			if (!(((Vector2)(ref linearVelocity)).LengthSquared() > 10f) || (int)FixtureB.CollisionCategories == 1)
			{
				Vector2 linearVelocity2 = FixtureA.Body.LinearVelocity;
				if (!(((Vector2)(ref linearVelocity2)).LengthSquared() > 10f) || (int)FixtureA.CollisionCategories == 1)
				{
					goto IL_00ae;
				}
			}
			if (Program.m_App.m_Rand.NextDouble() >= 0.5)
			{
				Program.m_SoundManager.Play(31);
			}
			else
			{
				Program.m_SoundManager.Play(32);
			}
			m_ItemImpactSoundTime = num + 1000f;
		}
		goto IL_00ae;
		IL_00ae:
		return true;
	}

	public void DeleteAll()
	{
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1)
			{
				m_Item[i].Delete();
			}
		}
	}

	public void Delete(int id)
	{
		m_Item[id].Delete();
	}

	public void DeleteByTriggerId(int triggerId)
	{
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_TriggerId == triggerId)
			{
				m_Item[i].Delete();
			}
		}
	}

	public void Update()
	{
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1)
			{
				m_Item[i].Update();
			}
		}
	}

	public void CalcModelSize(Model model, int type, bool bUseAll)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_02dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_034c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0351: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_039a: Unknown result type (might be due to invalid IL or missing references)
		//IL_039f: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		//IL_041d: Unknown result type (might be due to invalid IL or missing references)
		//IL_043f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0444: Unknown result type (might be due to invalid IL or missing references)
		//IL_0481: Unknown result type (might be due to invalid IL or missing references)
		//IL_0486: Unknown result type (might be due to invalid IL or missing references)
		//IL_0488: Unknown result type (might be due to invalid IL or missing references)
		//IL_048d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0492: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		ref Vector3 reference = ref m_ModelSizeMax3D[type];
		reference = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		ref Vector3 reference2 = ref m_ModelSizeMin3D[type];
		reference2 = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		Matrix[] array = (Matrix[])(object)new Matrix[((ReadOnlyCollection<ModelBone>)(object)model.Bones).Count];
		model.CopyAbsoluteBoneTransformsTo(array);
		Enumerator enumerator = model.Meshes.GetEnumerator();
		try
		{
			Vector3 val = default(Vector3);
			while (((Enumerator)(ref enumerator)).MoveNext())
			{
				ModelMesh current = ((Enumerator)(ref enumerator)).Current;
				Enumerator enumerator2 = current.MeshParts.GetEnumerator();
				try
				{
					while (((Enumerator)(ref enumerator2)).MoveNext())
					{
						ModelMeshPart current2 = ((Enumerator)(ref enumerator2)).Current;
						int vertexStride = current2.VertexBuffer.VertexDeclaration.VertexStride;
						_ = current2.NumVertices;
						int num = current2.NumVertices * vertexStride;
						float[] array2 = new float[num / 4];
						current2.VertexBuffer.GetData<float>(array2);
						for (int i = 0; i < num / 4; i += vertexStride / 4)
						{
							float num2 = array2[i];
							float num3 = array2[i + 1];
							float num4 = array2[i + 2];
							((Vector3)(ref val))._002Ector(num2, num3, num4);
							Vector3 val2 = Vector3.Transform(val, array[current.ParentBone.Index]);
							if (val2.X < m_ModelSizeMin3D[type].X)
							{
								m_ModelSizeMin3D[type].X = val2.X;
							}
							if (val2.X > m_ModelSizeMax3D[type].X)
							{
								m_ModelSizeMax3D[type].X = val2.X;
							}
							if (val2.Y < m_ModelSizeMin3D[type].Y)
							{
								m_ModelSizeMin3D[type].Y = val2.Y;
							}
							if (val2.Y > m_ModelSizeMax3D[type].Y)
							{
								m_ModelSizeMax3D[type].Y = val2.Y;
							}
							if (val2.Z < m_ModelSizeMin3D[type].Z)
							{
								m_ModelSizeMin3D[type].Z = val2.Z;
							}
							if (val2.Z > m_ModelSizeMax3D[type].Z)
							{
								m_ModelSizeMax3D[type].Z = val2.Z;
							}
						}
					}
				}
				finally
				{
					((IDisposable)(Enumerator)(ref enumerator2)/*cast due to .constrained prefix*/).Dispose();
				}
			}
		}
		finally
		{
			((IDisposable)(Enumerator)(ref enumerator)/*cast due to .constrained prefix*/).Dispose();
		}
		Vector3[] array3 = (Vector3[])(object)new Vector3[8];
		Vector3 zero = Vector3.Zero;
		zero.Y = 5f;
		zero.Z = 0f;
		Matrix val3 = Matrix.CreateFromYawPitchRoll((float)Math.PI / 2f, 0f, 0f);
		Vector3 val4 = Vector3.Transform(m_ModelSizeMin3D[type], val3);
		Vector3 val5 = Vector3.Transform(m_ModelSizeMax3D[type], val3);
		int num5 = 4;
		ref Vector3 reference3 = ref array3[0];
		reference3 = new Vector3(val4.X, val4.Y, val4.Z);
		ref Vector3 reference4 = ref array3[1];
		reference4 = new Vector3(val4.X, val5.Y, val4.Z);
		ref Vector3 reference5 = ref array3[2];
		reference5 = new Vector3(val5.X, val4.Y, val4.Z);
		ref Vector3 reference6 = ref array3[3];
		reference6 = new Vector3(val5.X, val5.Y, val4.Z);
		if (bUseAll)
		{
			num5 = 8;
			ref Vector3 reference7 = ref array3[4];
			reference7 = new Vector3(val4.X, val4.Y, val5.Z);
			ref Vector3 reference8 = ref array3[5];
			reference8 = new Vector3(val4.X, val5.Y, val5.Z);
			ref Vector3 reference9 = ref array3[6];
			reference9 = new Vector3(val5.X, val4.Y, val5.Z);
			ref Vector3 reference10 = ref array3[7];
			reference10 = new Vector3(val5.X, val5.Y, val5.Z);
		}
		Vector2 val6 = default(Vector2);
		((Vector2)(ref val6))._002Ector(9999f, 9999f);
		Vector2 val7 = default(Vector2);
		((Vector2)(ref val7))._002Ector(-9999f, -9999f);
		for (int j = 0; j < num5; j++)
		{
			Vector3 val8 = Program.m_CurrentCamera.WorldToScreen(array3[j] + zero);
			if (val8.X < val6.X)
			{
				val6.X = val8.X;
			}
			if (val8.X > val7.X)
			{
				val7.X = val8.X;
			}
			if (val8.Y < val6.Y)
			{
				val6.Y = val8.Y;
			}
			if (val8.Y > val7.Y)
			{
				val7.Y = val8.Y;
			}
		}
		m_ModelSize2D[type].X = val7.X - val6.X;
		m_ModelSize2D[type].Y = val7.Y - val6.Y;
	}

	public Item FindObjectByType(int type)
	{
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1 && m_Item[i].m_Type == type)
			{
				return m_Item[i];
			}
		}
		return null;
	}

	public Item FindByTriggerId(int id)
	{
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1 && m_Item[i].m_TriggerId == id)
			{
				return m_Item[i];
			}
		}
		return null;
	}

	public int CountStaticItems()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1 && (m_Item[i].m_Fixture == null || (int)m_Item[i].m_Fixture.Body.BodyType == 0))
			{
				num++;
			}
		}
		return num;
	}

	public int CountDynamicItems()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Invalid comparison between Unknown and I4
		int num = 0;
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1 && m_Item[i].m_Fixture != null && (int)m_Item[i].m_Fixture.Body.BodyType == 2)
			{
				num++;
			}
		}
		return num;
	}

	public void ClearAllFlashes()
	{
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1)
			{
				m_Item[i].m_FlashModel = 0;
			}
		}
	}

	public void Copy(Item[] src, Item[] dest)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 256; i++)
		{
			dest[i].m_Id = src[i].m_Id;
			dest[i].m_Position = src[i].m_Position;
			dest[i].m_Rotation = src[i].m_Rotation;
			dest[i].m_fScale = src[i].m_fScale;
			dest[i].m_PrevPosition = src[i].m_PrevPosition;
			dest[i].m_PrevRotation = src[i].m_PrevRotation;
			dest[i].m_TriggerId = src[i].m_TriggerId;
			dest[i].m_TriggerPos = src[i].m_TriggerPos;
			dest[i].m_Type = src[i].m_Type;
			dest[i].m_Velocity = src[i].m_Velocity;
		}
	}

	public void UpdateAllFromTriggers()
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < 256; i++)
		{
			if (m_Item[i].m_Id != -1 && m_Item[i].m_TriggerId != -1 && (m_Item[i].m_Position != Program.m_TriggerManager.m_Trigger[m_Item[i].m_TriggerId].m_Position || m_Item[i].m_Rotation.Z != Program.m_TriggerManager.m_Trigger[m_Item[i].m_TriggerId].m_Rotation))
			{
				m_Item[i].m_Position = Program.m_TriggerManager.m_Trigger[m_Item[i].m_TriggerId].m_Position;
				m_Item[i].m_Rotation.Z = Program.m_TriggerManager.m_Trigger[m_Item[i].m_TriggerId].m_Rotation;
				if (m_Item[i].m_Fixture != null)
				{
					m_Item[i].m_Fixture.Body.Position = m_Item[i].m_Position;
					m_Item[i].m_Fixture.Body.Rotation = m_Item[i].m_Rotation.Z;
				}
			}
		}
	}

	public void LoadContent(ContentManager Content)
	{
		m_Model[1] = Content.Load<Model>("Models\\book_large_25deg");
		m_ModelLOD[1] = Content.Load<Model>("Models\\book_large_25deg");
		m_Model[2] = Content.Load<Model>("Models\\chair1");
		m_ModelLOD[2] = Content.Load<Model>("Models\\chair1");
		m_Model[3] = Content.Load<Model>("Models\\pencil");
		m_ModelLOD[3] = Content.Load<Model>("Models\\pencil");
		m_Model[0] = Content.Load<Model>("Models\\toyblock_a");
		m_ModelLOD[0] = Content.Load<Model>("Models\\toyblock_a");
		m_Model[4] = Content.Load<Model>("Models\\toyblock_b");
		m_ModelLOD[4] = Content.Load<Model>("Models\\toyblock_b");
		m_Model[5] = Content.Load<Model>("Models\\toyblock_c");
		m_ModelLOD[5] = Content.Load<Model>("Models\\toyblock_c");
		m_Model[6] = m_Model[0];
		m_ModelLOD[6] = m_ModelLOD[0];
		m_Model[7] = m_Model[4];
		m_ModelLOD[7] = m_ModelLOD[4];
		m_Model[8] = m_Model[5];
		m_ModelLOD[8] = m_ModelLOD[5];
		m_Model[9] = Content.Load<Model>("Models\\ruler");
		m_ModelLOD[9] = Content.Load<Model>("Models\\ruler");
		m_Model[10] = Content.Load<Model>("Models\\bus");
		m_ModelLOD[10] = Content.Load<Model>("Models\\bus");
		m_Model[11] = Content.Load<Model>("Models\\chessboard");
		m_ModelLOD[11] = Content.Load<Model>("Models\\chessboard");
		m_Model[12] = Content.Load<Model>("Models\\drawingpin");
		m_ModelLOD[12] = Content.Load<Model>("Models\\drawingpin");
		m_Model[13] = Content.Load<Model>("Models\\startfinish");
		m_ModelLOD[13] = Content.Load<Model>("Models\\startfinish");
		m_Model[14] = m_Model[13];
		m_ModelLOD[14] = m_Model[13];
		m_Model[21] = Content.Load<Model>("Models\\flag");
		m_ModelLOD[21] = Content.Load<Model>("Models\\flag");
		m_Model[22] = m_Model[21];
		m_ModelLOD[22] = m_ModelLOD[21];
		m_Model[23] = m_Model[21];
		m_ModelLOD[23] = m_ModelLOD[21];
		m_Model[15] = Content.Load<Model>("Models\\sword");
		m_ModelLOD[15] = Content.Load<Model>("Models\\sword");
		m_Model[16] = Content.Load<Model>("Models\\sword2");
		m_ModelLOD[16] = Content.Load<Model>("Models\\sword2");
		m_Model[17] = Content.Load<Model>("Models\\shield");
		m_ModelLOD[17] = Content.Load<Model>("Models\\shield");
		m_Model[18] = Content.Load<Model>("Models\\smalltable");
		m_ModelLOD[18] = Content.Load<Model>("Models\\smalltable");
		m_Model[19] = Content.Load<Model>("Models\\book_large2");
		m_ModelLOD[19] = Content.Load<Model>("Models\\book_large2");
		m_Model[20] = Content.Load<Model>("Models\\track_skislope");
		m_ModelLOD[20] = Content.Load<Model>("Models\\track_skislope");
		m_Model[24] = Content.Load<Model>("Models\\track_skislope_reverse");
		m_ModelLOD[24] = Content.Load<Model>("Models\\track_skislope_reverse");
		m_Model[25] = Content.Load<Model>("Models\\track_1m");
		m_ModelLOD[25] = m_Model[25];
		m_Model[26] = Content.Load<Model>("Models\\track_50cm");
		m_ModelLOD[26] = m_Model[26];
		m_Model[27] = Content.Load<Model>("Models\\track_1m_up60");
		m_ModelLOD[27] = m_Model[27];
		m_Model[28] = Content.Load<Model>("Models\\track_1m_down60");
		m_ModelLOD[28] = m_Model[28];
		m_Model[29] = Content.Load<Model>("Models\\track_50cm_up30");
		m_ModelLOD[29] = m_Model[29];
		m_Model[30] = Content.Load<Model>("Models\\track_50cm_down30");
		m_ModelLOD[30] = m_Model[30];
		m_Model[31] = Content.Load<Model>("Models\\track_loop");
		m_ModelLOD[31] = m_Model[31];
		m_Model[32] = Content.Load<Model>("Models\\domino_one");
		m_ModelLOD[32] = m_Model[32];
		m_Model[33] = Content.Load<Model>("Models\\domino_two");
		m_ModelLOD[33] = m_Model[33];
		m_Model[34] = Content.Load<Model>("Models\\domino_three");
		m_ModelLOD[34] = m_Model[34];
		m_Model[35] = Content.Load<Model>("Models\\domino_four");
		m_ModelLOD[35] = m_Model[35];
		m_Model[36] = Content.Load<Model>("Models\\chair2");
		m_ModelLOD[36] = m_Model[36];
		m_Model[37] = Content.Load<Model>("Models\\book_large3");
		m_ModelLOD[37] = m_Model[37];
		m_Model[38] = Content.Load<Model>("Models\\hob");
		m_ModelLOD[38] = m_Model[38];
		m_Model[39] = Content.Load<Model>("Models\\microwave");
		m_ModelLOD[39] = m_Model[39];
		m_Model[40] = Content.Load<Model>("Models\\placemat");
		m_ModelLOD[40] = m_Model[40];
		m_Model[41] = Content.Load<Model>("Models\\knife");
		m_ModelLOD[41] = m_Model[41];
		m_Model[42] = Content.Load<Model>("Models\\coffee");
		m_ModelLOD[42] = m_Model[42];
		m_Model[43] = Content.Load<Model>("Models\\tea");
		m_ModelLOD[43] = m_Model[43];
		m_Model[44] = Content.Load<Model>("Models\\sugar");
		m_ModelLOD[44] = m_Model[44];
		m_Model[45] = Content.Load<Model>("Models\\biscuits");
		m_ModelLOD[45] = m_Model[45];
		m_Model[46] = Content.Load<Model>("Models\\bowl");
		m_ModelLOD[46] = m_Model[46];
		m_Model[47] = Content.Load<Model>("Models\\toaster");
		m_ModelLOD[47] = m_Model[47];
		m_Model[48] = Content.Load<Model>("Models\\spoon");
		m_ModelLOD[48] = m_Model[48];
		m_Model[49] = Content.Load<Model>("Models\\mug");
		m_ModelLOD[49] = m_Model[49];
		m_Model[50] = Content.Load<Model>("Models\\plate");
		m_ModelLOD[50] = m_Model[50];
		m_Model[51] = Content.Load<Model>("Models\\rollingpin");
		m_ModelLOD[51] = m_Model[51];
		m_Model[52] = Content.Load<Model>("Models\\spade");
		m_ModelLOD[52] = m_Model[52];
		m_Model[53] = Content.Load<Model>("Models\\ball");
		m_ModelLOD[53] = m_Model[53];
		m_Model[54] = Content.Load<Model>("Models\\gardenchair");
		m_ModelLOD[54] = m_Model[54];
		m_Model[55] = Content.Load<Model>("Models\\plank_1m");
		m_ModelLOD[55] = m_Model[55];
		m_Model[56] = Content.Load<Model>("Models\\plank_50cm");
		m_ModelLOD[56] = m_Model[56];
		m_Model[57] = Content.Load<Model>("Models\\brick");
		m_ModelLOD[57] = m_Model[57];
		m_Model[58] = Content.Load<Model>("Models\\rake");
		m_ModelLOD[58] = m_Model[58];
		m_Model[59] = Content.Load<Model>("Models\\plantpot");
		m_ModelLOD[59] = m_Model[59];
		m_Model[60] = Content.Load<Model>("Models\\plank2_1m");
		m_ModelLOD[60] = m_Model[60];
		m_Model[61] = Content.Load<Model>("Models\\plank2_50cm");
		m_ModelLOD[61] = m_Model[61];
		m_Model[62] = Content.Load<Model>("Models\\checkpoint");
		m_ModelLOD[62] = m_Model[62];
		m_Model[63] = Content.Load<Model>("Models\\lcdscreen");
		m_ModelLOD[63] = m_Model[63];
		m_Model[64] = Content.Load<Model>("Models\\keyboard");
		m_ModelLOD[64] = m_Model[64];
		m_Model[65] = Content.Load<Model>("Models\\laptop");
		m_ModelLOD[65] = m_Model[65];
		m_Model[66] = Content.Load<Model>("Models\\soda");
		m_ModelLOD[66] = m_Model[66];
		m_Model[67] = Content.Load<Model>("Models\\pizza");
		m_ModelLOD[67] = m_Model[67];
		m_Model[68] = Content.Load<Model>("Models\\gameboxblock");
		m_ModelLOD[68] = m_Model[68];
		m_Model[69] = Content.Load<Model>("Models\\gamebox");
		m_ModelLOD[69] = m_Model[69];
		m_Model[70] = Content.Load<Model>("Models\\gamebox");
		m_ModelLOD[70] = m_Model[70];
		m_Model[71] = Content.Load<Model>("Models\\phone");
		m_ModelLOD[71] = m_Model[71];
		m_Model[72] = Content.Load<Model>("Models\\phone");
		m_ModelLOD[72] = m_Model[72];
		m_Model[73] = Content.Load<Model>("Models\\dvdstack");
		m_ModelLOD[73] = m_Model[73];
		m_Model[74] = Content.Load<Model>("Models\\diary");
		m_ModelLOD[74] = m_Model[74];
		m_Model[75] = Content.Load<Model>("Models\\dice");
		m_ModelLOD[75] = m_Model[75];
		m_Model[76] = Content.Load<Model>("Models\\dice");
		m_ModelLOD[76] = m_Model[76];
		m_Model[77] = Content.Load<Model>("Models\\book_large4");
		m_ModelLOD[77] = Content.Load<Model>("Models\\book_large4");
		m_Model[78] = Content.Load<Model>("Models\\pc");
		m_ModelLOD[78] = Content.Load<Model>("Models\\pc");
		m_Model[79] = Content.Load<Model>("Models\\speaker");
		m_ModelLOD[79] = Content.Load<Model>("Models\\speaker");
		m_Model[80] = Content.Load<Model>("Models\\pen");
		m_ModelLOD[80] = Content.Load<Model>("Models\\pen");
		m_Model[81] = Content.Load<Model>("Models\\pen");
		m_ModelLOD[81] = Content.Load<Model>("Models\\pen");
		m_Model[82] = Content.Load<Model>("Models\\storagebox");
		m_ModelLOD[82] = Content.Load<Model>("Models\\storagebox");
		m_Model[83] = Content.Load<Model>("Models\\printer");
		m_ModelLOD[83] = Content.Load<Model>("Models\\printer");
		m_Model[84] = Content.Load<Model>("Models\\book_large5");
		m_ModelLOD[84] = Content.Load<Model>("Models\\book_large5");
		m_Model[85] = Content.Load<Model>("Models\\book_large6");
		m_ModelLOD[85] = Content.Load<Model>("Models\\book_large6");
		m_Model[86] = Content.Load<Model>("Models\\book_large7");
		m_ModelLOD[86] = Content.Load<Model>("Models\\book_large7");
		m_Model[87] = Content.Load<Model>("Models\\bath");
		m_ModelLOD[87] = Content.Load<Model>("Models\\bath");
		m_Model[88] = Content.Load<Model>("Models\\toilet_closed");
		m_ModelLOD[88] = Content.Load<Model>("Models\\toilet_closed");
		m_Model[89] = Content.Load<Model>("Models\\toothbrush");
		m_ModelLOD[89] = Content.Load<Model>("Models\\toothbrush");
		m_Model[90] = Content.Load<Model>("Models\\toothbrush2");
		m_ModelLOD[90] = Content.Load<Model>("Models\\toothbrush2");
		m_Model[91] = Content.Load<Model>("Models\\toothbrush3");
		m_ModelLOD[91] = Content.Load<Model>("Models\\toothbrush3");
		m_Model[92] = Content.Load<Model>("Models\\tumbler");
		m_ModelLOD[92] = Content.Load<Model>("Models\\tumbler");
		m_Model[93] = Content.Load<Model>("Models\\bin");
		m_ModelLOD[93] = Content.Load<Model>("Models\\bin");
		m_Model[94] = Content.Load<Model>("Models\\clotheshorse");
		m_ModelLOD[94] = Content.Load<Model>("Models\\clotheshorse");
		m_Model[95] = Content.Load<Model>("Models\\facewash");
		m_ModelLOD[95] = Content.Load<Model>("Models\\facewash");
		m_Model[96] = Content.Load<Model>("Models\\babypowder");
		m_ModelLOD[96] = Content.Load<Model>("Models\\babypowder");
		m_Model[97] = Content.Load<Model>("Models\\toilet_open");
		m_ModelLOD[97] = Content.Load<Model>("Models\\toilet_open");
		m_Model[98] = Content.Load<Model>("Models\\toothpaste");
		m_ModelLOD[98] = Content.Load<Model>("Models\\toothpaste");
		m_Model[99] = Content.Load<Model>("Models\\shampoo");
		m_ModelLOD[99] = Content.Load<Model>("Models\\shampoo");
		m_Model[100] = Content.Load<Model>("Models\\sink");
		m_ModelLOD[100] = Content.Load<Model>("Models\\sink");
		m_Model[101] = Content.Load<Model>("Models\\sofa");
		m_ModelLOD[101] = Content.Load<Model>("Models\\sofa");
		m_Model[102] = Content.Load<Model>("Models\\armchair");
		m_ModelLOD[102] = Content.Load<Model>("Models\\armchair");
		m_Model[103] = Content.Load<Model>("Models\\coffeetable");
		m_ModelLOD[103] = Content.Load<Model>("Models\\coffeetable");
		m_Model[104] = Content.Load<Model>("Models\\lamp");
		m_ModelLOD[104] = Content.Load<Model>("Models\\lamp");
		m_Model[105] = Content.Load<Model>("Models\\remote");
		m_ModelLOD[105] = Content.Load<Model>("Models\\remote");
		m_Model[106] = Content.Load<Model>("Models\\tvstand");
		m_ModelLOD[106] = Content.Load<Model>("Models\\tvstand");
		m_Model[107] = Content.Load<Model>("Models\\wine");
		m_ModelLOD[107] = Content.Load<Model>("Models\\wine");
		m_Model[108] = Content.Load<Model>("Models\\vase");
		m_ModelLOD[108] = Content.Load<Model>("Models\\vase");
		m_Model[109] = Content.Load<Model>("Models\\box");
		m_ModelLOD[109] = Content.Load<Model>("Models\\box");
		m_Model[110] = Content.Load<Model>("Models\\extension");
		m_ModelLOD[110] = Content.Load<Model>("Models\\extension");
		m_Model[111] = Content.Load<Model>("Models\\extension");
		m_ModelLOD[111] = Content.Load<Model>("Models\\extension");
		m_Model[112] = Content.Load<Model>("Models\\plug2");
		m_ModelLOD[112] = Content.Load<Model>("Models\\plug2");
	}
}
