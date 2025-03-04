using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppNamespace;

public class Actor
{
	public int m_Id;

	public Vector2 m_Position;

	public float m_ZDistance;

	public Vector2 m_PrevPosition;

	public Vector3 m_Rotation;

	public Vector3 m_PrevRotation;

	public Model m_Model;

	public Model m_ModelLOD;

	public Vector3 m_ScreenPos;

	public Vector2 m_Velocity;

	public Vector2 m_PrevVelocity;

	public Vector3 m_Bounds3DMax;

	public Vector3 m_Bounds3DMin;

	public Vector2 m_Bounds2DMax;

	public Vector2 m_Bounds2DMin;

	public int m_FlashModel;

	public float m_fScale;

	public float m_ShakeTime;

	public float m_ShakeDuration;

	public float m_ShakeMag;

	public Vector2 m_ShakeVec;

	public Vector3[] m_BoundsTmp;

	public Matrix[] m_Transforms = (Matrix[])(object)new Matrix[64];

	public Actor()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		m_Id = -1;
		m_Position = Vector2.Zero;
		m_Rotation = Vector3.Zero;
		m_fScale = 1f;
		m_PrevPosition = Vector2.Zero;
		m_PrevRotation = Vector3.Zero;
		m_Model = null;
		m_ModelLOD = null;
		m_ScreenPos = Vector3.Zero;
		m_Velocity = Vector2.Zero;
		m_PrevVelocity = Vector2.Zero;
		m_BoundsTmp = (Vector3[])(object)new Vector3[8];
		m_FlashModel = 0;
		m_ZDistance = 0f;
		m_ShakeTime = 0f;
		m_ShakeDuration = 0f;
		m_ShakeMag = 0f;
		m_ShakeVec = Vector2.Zero;
	}

	public Vector2 Position()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return m_Position;
	}

	public Vector3 Rotation()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return m_Rotation;
	}

	public virtual void SetPosition(Vector2 pos)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		m_Position = pos;
	}

	public virtual void SetRotation(Vector3 rot)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		m_Rotation = rot;
	}

	public void SetModel(Model m, Model lod)
	{
		m_Model = m;
		m_ModelLOD = lod;
		m_Model.CopyAbsoluteBoneTransformsTo(m_Transforms);
	}

	public void SetModel(Model m)
	{
		m_Model = m;
	}

	public void CalcBounds2D(bool bUseAll, bool bUsePitch)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0200: Unknown result type (might be due to invalid IL or missing references)
		//IL_0210: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Unknown result type (might be due to invalid IL or missing references)
		//IL_0195: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c0: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero = Vector3.Zero;
		zero.X = m_Position.X;
		zero.Y = m_Position.Y;
		zero.Z = 0.01f;
		float num = 0f;
		if (bUsePitch)
		{
			num = m_Rotation.X;
		}
		Matrix val = Matrix.CreateFromYawPitchRoll(m_Rotation.Y, num, 0f);
		Vector3 val2 = Vector3.Transform(m_Bounds3DMin, val);
		Vector3 val3 = Vector3.Transform(m_Bounds3DMax, val);
		if (!bUseAll)
		{
			val2.Z = 0f;
		}
		int num2 = 4;
		ref Vector3 reference = ref m_BoundsTmp[0];
		reference = new Vector3(val2.X, val2.Y, val2.Z);
		ref Vector3 reference2 = ref m_BoundsTmp[1];
		reference2 = new Vector3(val2.X, val3.Y, val2.Z);
		ref Vector3 reference3 = ref m_BoundsTmp[2];
		reference3 = new Vector3(val3.X, val2.Y, val2.Z);
		ref Vector3 reference4 = ref m_BoundsTmp[3];
		reference4 = new Vector3(val3.X, val3.Y, val2.Z);
		if (bUseAll)
		{
			num2 = 8;
			ref Vector3 reference5 = ref m_BoundsTmp[4];
			reference5 = new Vector3(val2.X, val2.Y, val3.Z);
			ref Vector3 reference6 = ref m_BoundsTmp[5];
			reference6 = new Vector3(val2.X, val3.Y, val3.Z);
			ref Vector3 reference7 = ref m_BoundsTmp[6];
			reference7 = new Vector3(val3.X, val2.Y, val3.Z);
			ref Vector3 reference8 = ref m_BoundsTmp[7];
			reference8 = new Vector3(val3.X, val3.Y, val3.Z);
		}
		m_Bounds2DMin = new Vector2(9999f, 9999f);
		m_Bounds2DMax = new Vector2(-9999f, -9999f);
		for (int i = 0; i < num2; i++)
		{
			Vector3 val4 = Program.m_CurrentCamera.WorldToScreen(m_BoundsTmp[i] + zero);
			if (val4.X < m_Bounds2DMin.X)
			{
				m_Bounds2DMin.X = val4.X;
			}
			if (val4.X > m_Bounds2DMax.X)
			{
				m_Bounds2DMax.X = val4.X;
			}
			if (val4.Y < m_Bounds2DMin.Y)
			{
				m_Bounds2DMin.Y = val4.Y;
			}
			if (val4.Y > m_Bounds2DMax.Y)
			{
				m_Bounds2DMax.Y = val4.Y;
			}
		}
	}

	public void CalcBounds3D()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0106: Unknown result type (might be due to invalid IL or missing references)
		m_Bounds3DMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
		m_Bounds3DMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
		m_Model.CopyAbsoluteBoneTransformsTo(m_Transforms);
		Enumerator enumerator = m_Model.Meshes.GetEnumerator();
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
						float[] array = new float[num / 4];
						current2.VertexBuffer.GetData<float>(array);
						for (int i = 0; i < num / 4; i += vertexStride / 4)
						{
							float num2 = array[i];
							float num3 = array[i + 1];
							float num4 = array[i + 2];
							((Vector3)(ref val))._002Ector(num2, num3, num4);
							Vector3 val2 = Vector3.Transform(val, m_Transforms[current.ParentBone.Index]);
							if (val2.X < m_Bounds3DMin.X)
							{
								m_Bounds3DMin.X = val2.X;
							}
							if (val2.X > m_Bounds3DMax.X)
							{
								m_Bounds3DMax.X = val2.X;
							}
							if (val2.Y < m_Bounds3DMin.Y)
							{
								m_Bounds3DMin.Y = val2.Y;
							}
							if (val2.Y > m_Bounds3DMax.Y)
							{
								m_Bounds3DMax.Y = val2.Y;
							}
							if (val2.Z < m_Bounds3DMin.Z)
							{
								m_Bounds3DMin.Z = val2.Z;
							}
							if (val2.Z > m_Bounds3DMax.Z)
							{
								m_Bounds3DMax.Z = val2.Z;
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
	}

	public Vector2 GetOffset3D(float x, float y, float z)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero = Vector3.Zero;
		zero.X = m_Position.X;
		zero.Y = m_Position.Y;
		zero.Z = m_ZDistance;
		zero.X += x;
		zero.Y += y;
		zero.Z += z;
		Vector3 zero2 = Vector3.Zero;
		zero2 = Program.m_CameraManager3D.WorldToScreen(zero);
		Vector2 zero3 = Vector2.Zero;
		zero3.X = zero2.X;
		zero3.Y = zero2.Y;
		return zero3;
	}

	public void SetShake(float mag, float MILLISECS)
	{
		m_ShakeTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + MILLISECS;
		m_ShakeDuration = MILLISECS;
		m_ShakeMag = mag;
	}

	public void UpdateShake()
	{
		if (!(m_ShakeTime <= (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds) && m_ShakeMag != 0f)
		{
			float num = (m_ShakeTime - (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds) / m_ShakeDuration;
			m_ShakeVec.X = ((float)Program.m_App.m_Rand.NextDouble() - 0.5f) * m_ShakeMag * num;
			m_ShakeVec.Y = ((float)Program.m_App.m_Rand.NextDouble() - 0.5f) * m_ShakeMag * num;
		}
	}
}
