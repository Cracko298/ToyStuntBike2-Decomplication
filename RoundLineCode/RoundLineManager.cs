using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RoundLineCode;

internal class RoundLineManager
{
	private GraphicsDevice device;

	private Effect effect;

	private EffectParameter viewProjMatrixParameter;

	private EffectParameter instanceDataParameter;

	private EffectParameter alphaDataParameter;

	private EffectParameter timeParameter;

	private EffectParameter lineRadiusParameter;

	private EffectParameter lineColorParameter;

	private EffectParameter blurThresholdParameter;

	private VertexBuffer vb;

	private IndexBuffer ib;

	private int numInstances;

	private int numVertices;

	private int numIndices;

	private int numPrimitivesPerInstance;

	private int numPrimitives;

	private int bytesPerVertex;

	private float[] translationData;

	private float[] alphaData;

	public int NumLinesDrawn;

	public float BlurThreshold = 0.97f;

	public string[] TechniqueNames
	{
		get
		{
			string[] array = new string[effect.Techniques.Count];
			int num = 0;
			foreach (EffectTechnique technique in effect.Techniques)
			{
				array[num++] = technique.Name;
			}
			return array;
		}
	}

	public void Init(GraphicsDevice device, ContentManager content)
	{
		this.device = device;
		effect = content.Load<Effect>("RoundLine");
		viewProjMatrixParameter = effect.Parameters["viewProj"];
		instanceDataParameter = effect.Parameters["instanceData"];
		alphaDataParameter = effect.Parameters["alphaData"];
		timeParameter = effect.Parameters["time"];
		lineRadiusParameter = effect.Parameters["lineRadius"];
		lineColorParameter = effect.Parameters["lineColor"];
		blurThresholdParameter = effect.Parameters["blurThreshold"];
		CreateRoundLineMesh();
	}

	private void CreateRoundLineMesh()
	{
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_0143: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0161: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0221: Unknown result type (might be due to invalid IL or missing references)
		//IL_0230: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_026b: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ad: Expected O, but got Unknown
		//IL_03ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_03d4: Expected O, but got Unknown
		numInstances = 120;
		numVertices = 12 * numInstances;
		numPrimitivesPerInstance = 4;
		numPrimitives = numPrimitivesPerInstance * numInstances;
		numIndices = 3 * numPrimitives;
		short[] array = new short[numIndices];
		bytesPerVertex = RoundLineVertex.SizeInBytes;
		RoundLineVertex[] array2 = new RoundLineVertex[numVertices];
		translationData = new float[numInstances * 4];
		alphaData = new float[numInstances];
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < numInstances; i++)
		{
			int num3 = num;
			ref RoundLineVertex reference = ref array2[num++];
			reference = new RoundLineVertex(new Vector3(0f, -1f, 0f), new Vector2(1f, 4.712389f), new Vector2(0f, 0f), i);
			ref RoundLineVertex reference2 = ref array2[num++];
			reference2 = new RoundLineVertex(new Vector3(0f, -1f, 0f), new Vector2(1f, 4.712389f), new Vector2(0f, 1f), i);
			ref RoundLineVertex reference3 = ref array2[num++];
			reference3 = new RoundLineVertex(new Vector3(0f, 0f, 0f), new Vector2(0f, 4.712389f), new Vector2(0f, 1f), i);
			ref RoundLineVertex reference4 = ref array2[num++];
			reference4 = new RoundLineVertex(new Vector3(0f, 0f, 0f), new Vector2(0f, 4.712389f), new Vector2(0f, 0f), i);
			ref RoundLineVertex reference5 = ref array2[num++];
			reference5 = new RoundLineVertex(new Vector3(0f, 0f, 0f), new Vector2(0f, (float)Math.PI / 2f), new Vector2(0f, 1f), i);
			ref RoundLineVertex reference6 = ref array2[num++];
			reference6 = new RoundLineVertex(new Vector3(0f, 0f, 0f), new Vector2(0f, (float)Math.PI / 2f), new Vector2(0f, 0f), i);
			ref RoundLineVertex reference7 = ref array2[num++];
			reference7 = new RoundLineVertex(new Vector3(0f, 1f, 0f), new Vector2(1f, (float)Math.PI / 2f), new Vector2(0f, 1f), i);
			ref RoundLineVertex reference8 = ref array2[num++];
			reference8 = new RoundLineVertex(new Vector3(0f, 1f, 0f), new Vector2(1f, (float)Math.PI / 2f), new Vector2(0f, 0f), i);
			array[num2++] = (short)num3;
			array[num2++] = (short)(num3 + 1);
			array[num2++] = (short)(num3 + 2);
			array[num2++] = (short)(num3 + 2);
			array[num2++] = (short)(num3 + 3);
			array[num2++] = (short)num3;
			array[num2++] = (short)(num3 + 4);
			array[num2++] = (short)(num3 + 6);
			array[num2++] = (short)(num3 + 5);
			array[num2++] = (short)(num3 + 6);
			array[num2++] = (short)(num3 + 7);
			array[num2++] = (short)(num3 + 5);
		}
		vb = new VertexBuffer(device, typeof(RoundLineVertex), numVertices * bytesPerVertex, (BufferUsage)0);
		vb.SetData<RoundLineVertex>(array2);
		ib = new IndexBuffer(device, (IndexElementSize)0, numIndices * 2, (BufferUsage)0);
		ib.SetData<short>(array);
	}

	public float ComputeBlurThreshold(float lineRadius, Matrix viewProjMatrix, float viewportWidth)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		Vector4 val = default(Vector4);
		((Vector4)(ref val))._002Ector(0f, 0f, 0f, 1f);
		Vector4 val2 = default(Vector4);
		((Vector4)(ref val2))._002Ector(lineRadius, 0f, 0f, 1f);
		Vector4 val3 = val2 - val;
		Vector4 val4 = Vector4.Transform(val3, viewProjMatrix);
		val4.X *= viewportWidth;
		double num = 0.125 * Math.Log(val4.X) + 0.4;
		return MathHelper.Clamp((float)num, 0.5f, 0.99f);
	}

	public void Draw(List<RoundLine> roundLines, float lineRadius, Color lineColor, Matrix viewProjMatrix, float time, string techniqueName)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ec: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		device.SetVertexBuffer(vb);
		device.Indices = ib;
		viewProjMatrixParameter.SetValue(viewProjMatrix);
		timeParameter.SetValue(time);
		lineColorParameter.SetValue(((Color)(ref lineColor)).ToVector4());
		lineRadiusParameter.SetValue(lineRadius);
		blurThresholdParameter.SetValue(BlurThreshold);
		if (techniqueName == null)
		{
			effect.CurrentTechnique = effect.Techniques[0];
		}
		else
		{
			effect.CurrentTechnique = effect.Techniques[techniqueName];
		}
		effect.CurrentTechnique.Passes[0].Apply();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		foreach (RoundLine roundLine in roundLines)
		{
			translationData[num++] = roundLine.P0.X;
			translationData[num++] = roundLine.P0.Y;
			translationData[num++] = roundLine.Rho;
			translationData[num++] = roundLine.Theta;
			alphaData[num2++] = roundLine.alpha;
			num3++;
			if (num3 == numInstances)
			{
				instanceDataParameter.SetValue(translationData);
				alphaDataParameter.SetValue(alphaData);
				effect.CurrentTechnique.Passes[0].Apply();
				device.DrawIndexedPrimitives((PrimitiveType)0, 0, 0, numVertices, 0, numPrimitivesPerInstance * num3);
				NumLinesDrawn += num3;
				num3 = 0;
				num = 0;
				num2 = 0;
			}
		}
		if (num3 > 0)
		{
			instanceDataParameter.SetValue(translationData);
			alphaDataParameter.SetValue(alphaData);
			effect.CurrentTechnique.Passes[0].Apply();
			device.DrawIndexedPrimitives((PrimitiveType)0, 0, 0, numVertices, 0, numPrimitivesPerInstance * num3);
			NumLinesDrawn += num3;
		}
	}
}
