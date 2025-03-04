using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CustomAvatarAnimation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;

namespace AvatarWrapper;

public class Avatar
{
	public class AnimationFrameData
	{
		public List<Matrix> BoneTransforms = new List<Matrix>();

		public TimeSpan FramePosition;
	}

	public class AnimationData
	{
		public TimeSpan Length;

		public List<AnimationFrameData> animationData = new List<AnimationFrameData>();

		public string Title;
	}

	public class AvatarPCAnimation
	{
		public AvatarAnimationPreset AnimationPreset;

		public TimeSpan CurrentPosition;

		public IList<Matrix> BoneTransforms;

		public AvatarPCAnimation(AvatarAnimationPreset preset)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			AnimationPreset = preset;
		}

		public void Update(TimeSpan ElapsedGameTime, bool Loop)
		{
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_003c: Unknown result type (might be due to invalid IL or missing references)
			CurrentPosition += ElapsedGameTime;
			if (CurrentPosition >= GetLengthOfCurrentAnimation(AnimationPreset))
			{
				if (Loop)
				{
					CurrentPosition = TimeSpan.Zero;
				}
				else
				{
					CurrentPosition = GetLengthOfCurrentAnimation(AnimationPreset);
				}
			}
			BoneTransforms = GetBoneTransforms(AnimationPreset, CurrentPosition);
		}
	}

	public enum AnimationType
	{
		BuiltIn,
		SkinnedModel,
		Collada,
		CustomAnimationXNA
	}

	public static int[] parentBones = new int[71]
	{
		-1, 0, 0, 0, 0, 1, 2, 2, 3, 3,
		1, 6, 5, 6, 5, 8, 5, 8, 5, 14,
		12, 11, 16, 15, 14, 20, 20, 20, 22, 22,
		22, 25, 25, 25, 28, 28, 28, 33, 33, 33,
		33, 33, 33, 33, 36, 36, 36, 36, 36, 36,
		36, 37, 38, 39, 40, 43, 44, 45, 46, 47,
		50, 51, 52, 53, 54, 55, 56, 57, 58, 59,
		60
	};

	public static Matrix[] bindPoses = (Matrix[])(object)new Matrix[71]
	{
		new Matrix(-1.06f, 4.517219E-16f, 1.263618E-07f, 0f, 4.560241E-16f, 1.05f, -2.059464E-16f, 0f, -1.263618E-07f, -2.040034E-16f, -1.06f, 0f, 1.00638E-06f, 0.7929635f, -0.00918531f, 1f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0f, 0f, -2.910383E-11f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.09005711f, -0.1072717f, 0.008671193f, 1f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, -0.09005711f, -0.1072717f, 0.008671193f, 0.9999999f),
		new Matrix(0.9f, 0f, -3.929107E-25f, 0f, 0f, 1f, -1.964554E-25f, 0f, 3.722312E-25f, 2.067951E-25f, 0.95f, 0f, 0f, 0.03059953f, -2.910383E-11f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0f, 0.09179866f, -0.007715893f, 0.9999999f),
		new Matrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, -7.070368E-06f, -0.2687335f, -0.01300271f, 0.9999999f),
		new Matrix(0.85f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 0.85f, 0f, 0f, -0.1343725f, -0.006499312f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 4.135903E-25f, 0f, -4.135903E-25f, 1f, 0f, 0f, -4.135903E-25f, 0f, 1f, 0f, -7.070368E-06f, -0.2687335f, -0.01300271f, 0.9999999f),
		new Matrix(0.85f, 4.135903E-25f, 3.515517E-25f, 0f, -3.515517E-25f, 1f, 0f, 0f, -3.515517E-25f, 0f, 0.85f, 0f, 0f, -0.1343725f, -0.006499312f, 0.9999999f),
		new Matrix(0.8f, 0f, -3.308722E-25f, 0f, 0f, 1f, -1.654361E-25f, 0f, 3.308722E-25f, 2.067951E-25f, 0.8f, 0f, 0f, 0.06119912f, -0.005143928f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.005812414f, -0.2596922f, -0.02537671f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.007403408f, 0.1224214f, 0.01426828f, 0.9999999f),
		new Matrix(0.85f, 0f, -3.515517E-25f, 0f, 0f, 1f, -1.757759E-25f, 0f, 3.515517E-25f, 2.067951E-25f, 0.85f, 0f, 0.002913281f, -0.1298461f, -0.01268019f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0f, 0.1737709f, -0.01882024f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.00579828f, -0.2596922f, -0.02537671f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, -0.007403407f, 0.1224214f, 0.01426828f, 0.9999999f),
		new Matrix(0.85f, 4.135903E-25f, 0f, 0f, -3.515517E-25f, 1f, -1.757759E-25f, 0f, 0f, 2.067951E-25f, 0.85f, 0f, -0.00289914f, -0.1298461f, -0.01268019f, 0.9999999f),
		new Matrix(0.95f, 0f, -3.722312E-25f, 0f, 0f, 1f, -1.861156E-25f, 0f, 3.929107E-25f, 2.067951E-25f, 0.9f, 0f, 0f, 0.04343986f, -0.004703019f, 0.9999999f),
		new Matrix(0.95f, 0f, -3.929107E-25f, 0f, 0f, 0.95f, -1.964554E-25f, 0f, 3.929107E-25f, 1.964554E-25f, 0.95f, 0f, -7.071068E-06f, 0.1183337f, 0.02284149f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.1176131f, 5.775504E-05f, -0.0279201f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.007855958f, -0.1010247f, 0.1342524f, 1f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.117613f, 5.775318E-05f, -0.0279201f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.007855952f, -0.1010247f, 0.1342524f, 0.9999999f),
		new Matrix(0.85f, 0f, -3.515517E-25f, 0f, 0f, 1f, -1.757759E-25f, 0f, 3.515517E-25f, 2.067951E-25f, 0.85f, 0f, -7.071068E-06f, 0.03944456f, 0.007605664f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.1696066f, -0.003995299f, 0.0005878786f, 0.9999999f),
		new Matrix(1f, 0f, -3.515517E-25f, 0f, 0f, 0.85f, -1.757759E-25f, 0f, 4.135903E-25f, 1.757759E-25f, 0.85f, 0f, 0.0848033f, -0.00199765f, 0.0002898554f, 0.9999999f),
		new Matrix(1f, 0f, -3.515517E-25f, 0f, 0f, 0.85f, -1.757759E-25f, 0f, 4.135903E-25f, 1.757759E-25f, 0.85f, 0f, 0f, 0f, -5.820766E-11f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.1696066f, -0.003995301f, 0.0005878787f, 0.9999999f),
		new Matrix(1f, 3.515517E-25f, 0f, 0f, -4.135903E-25f, 0.85f, -1.757759E-25f, 0f, 0f, 1.757759E-25f, 0.85f, 0f, -0.08480332f, -0.001997652f, 0.0002898555f, 0.9999999f),
		new Matrix(1f, 3.515517E-25f, 0f, 0f, -4.135903E-25f, 0.85f, -1.757759E-25f, 0f, 0f, 1.757759E-25f, 0.85f, 0f, 0f, -1.862645E-09f, 0f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.1027992f, -0.002424836f, 0.001567673f, 0.9999999f),
		new Matrix(1f, 0f, -3.515517E-25f, 0f, 0f, 0.85f, -1.757759E-25f, 0f, 4.135903E-25f, 1.757759E-25f, 0.85f, 0f, 0.07710293f, -0.001824439f, 0.001175754f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.1541988f, -0.003637314f, 0.002347426f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.1027992f, -0.002424838f, 0.001567673f, 0.9999999f),
		new Matrix(1f, 3.515517E-25f, 0f, 0f, -4.135903E-25f, 0.85f, -1.757759E-25f, 0f, 0f, 1.757759E-25f, 0.85f, 0f, -0.07710292f, -0.00182444f, 0.001175754f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.1541988f, -0.003637316f, 0.002347426f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.1083924f, -0.02864808f, 0.04242924f, 1f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.1101885f, -0.02752805f, 0.01115742f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.1034568f, -0.03065729f, -0.01861204f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.09029046f, -0.03503358f, -0.04665053f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.07399872f, -0.1849946f, 4.082802E-06f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.1110087f, -0.1110014f, 4.082802E-06f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0f, 0f, 0.009895937f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.1083924f, -0.02864808f, 0.04242924f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.1101884f, -0.02752805f, 0.01115742f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.1034568f, -0.03065729f, -0.01861204f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.09029045f, -0.03503358f, -0.04665053f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.07399871f, -0.1849946f, 4.08286E-06f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.1110087f, -0.1110014f, 4.08286E-06f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, 0f, -1.862645E-09f, 0.009895937f, 0.9999999f),
		new Matrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0.04690241f, -1.862645E-09f, 0.0003796703f, 1f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.04973078f, 0f, -1.224695E-05f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.04768014f, 0f, -0.000355173f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.04028386f, 0f, -0.0003551769f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.06541445f, -0.03722751f, 0.04949193f, 1f),
		new Matrix(1f, 4.135903E-25f, 4.135903E-25f, 0f, -4.135903E-25f, 1f, 0f, 0f, -4.135903E-25f, 0f, 1f, 0f, -0.04690242f, 0f, 0.0003796704f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.04973083f, -1.862645E-09f, -1.224689E-05f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.04768026f, -1.862645E-09f, -0.0003551729f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.04028386f, -1.862645E-09f, -0.0003551766f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.06542858f, -0.03722751f, 0.04949194f, 0.9999999f),
		new Matrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0.03380674f, -1.862645E-09f, 1.224864E-05f, 1f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.03515738f, 0f, -5.820766E-11f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.03452808f, 0f, 1.22448E-05f, 0.9999999f),
		new Matrix(1f, 0f, -4.135903E-25f, 0f, 0f, 1f, -2.067951E-25f, 0f, 4.135903E-25f, 2.067951E-25f, 1f, 0f, 0.02958536f, 0f, -2.328306E-10f, 0.9999999f),
		new Matrix(1f, 0f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 0f, 1f, 0f, 0.03209555f, -0.01738984f, 0.01951019f, 1f),
		new Matrix(1f, 4.135903E-25f, 4.135903E-25f, 0f, -4.135903E-25f, 1f, 0f, 0f, -4.135903E-25f, 0f, 1f, 0f, -0.03380674f, 0f, 1.22487E-05f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.03515732f, -1.862645E-09f, 0f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.03452795f, -1.862645E-09f, 1.224491E-05f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 0f, 0f, -4.135903E-25f, 1f, -2.067951E-25f, 0f, 0f, 2.067951E-25f, 1f, 0f, -0.02958536f, -1.862645E-09f, 0f, 0.9999999f),
		new Matrix(1f, 4.135903E-25f, 4.135903E-25f, 0f, -4.135903E-25f, 1f, 0f, 0f, -4.135903E-25f, 0f, 1f, 0f, -0.03209561f, -0.01738983f, 0.01951019f, 0.9999999f)
	};

	private static List<AnimationData> AvatarPCAnimations;

	private static ContentManager content;

	private static GraphicsDevice graphicsDevice;

	private static bool isPC = false;

	public Vector3 Position = Vector3.Zero;

	public Vector3 Rotation = Vector3.Zero;

	public float Scale = 1f;

	private AvatarDescription description;

	private AvatarExpression expression;

	private bool isAnimating;

	private bool isLoopingAnimation = true;

	private Matrix view = Matrix.CreateLookAt(new Vector3(0f, 2f, -1.5f), new Vector3(0f, 1f, 0f), Vector3.Up);

	private Matrix projection;

	private AnimationType currentAnimationType;

	public CustomAvatarAnimationPlayer currentXNACustomAnimation;

	private AvatarAnimation Animation;

	private AvatarPCAnimation AnimationPC;

	private AvatarRenderer Renderer;

	private BasicEffect effect;

	public static bool IsPC => isPC;

	public Matrix World => Matrix.CreateScale(Scale) * Matrix.CreateFromYawPitchRoll(Rotation.Y, Rotation.X, Rotation.Z) * Matrix.CreateTranslation(Position);

	public AvatarDescription Description
	{
		get
		{
			return description;
		}
		set
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Expected O, but got Unknown
			description = value;
			Renderer = new AvatarRenderer(description, true);
		}
	}

	public AvatarExpression Expression
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return expression;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			expression = value;
		}
	}

	public Vector3 AmbientLightColor
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Renderer.AmbientLightColor;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Renderer.AmbientLightColor = value;
		}
	}

	public Vector3 LightColor
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Renderer.LightColor;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Renderer.LightColor = value;
		}
	}

	public Vector3 LightDirection
	{
		get
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			return Renderer.LightDirection;
		}
		set
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			Renderer.LightDirection = value;
		}
	}

	public bool IsLoaded => true;

	public bool IsAnimating => isAnimating;

	public bool IsLoopingAnimation
	{
		get
		{
			return isLoopingAnimation;
		}
		set
		{
			isLoopingAnimation = value;
		}
	}

	public Matrix View
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return view;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			view = value;
		}
	}

	public Matrix Projection
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return projection;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			projection = value;
		}
	}

	public AnimationType CurrentAnimationType => currentAnimationType;

	public static TimeSpan GetLengthOfCurrentAnimation(AvatarAnimationPreset preset)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		AnimationData animationFromPreset = GetAnimationFromPreset(preset);
		return animationFromPreset.Length;
	}

	public static List<Matrix> GetBoneTransforms(AvatarAnimationPreset preset, TimeSpan Position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		AnimationData animationFromPreset = GetAnimationFromPreset(preset);
		AnimationFrameData lastFrame = GetLastFrame(animationFromPreset.animationData, Position);
		AnimationFrameData nextFrame = GetNextFrame(animationFromPreset.animationData, Position);
		_ = TimeSpan.Zero;
		_ = nextFrame.FramePosition - lastFrame.FramePosition;
		_ = Position - lastFrame.FramePosition;
		Matrix[] array = (Matrix[])(object)new Matrix[lastFrame.BoneTransforms.Count];
		lastFrame.BoneTransforms.CopyTo(array);
		float num = Position.Ticks / nextFrame.FramePosition.Ticks;
		for (int i = 0; i < lastFrame.BoneTransforms.Count; i++)
		{
			ref Matrix reference = ref array[i];
			reference = Matrix.Lerp(lastFrame.BoneTransforms[i], nextFrame.BoneTransforms[i], num);
		}
		return array.ToList();
	}

	private static AnimationFrameData GetLastFrame(List<AnimationFrameData> data, TimeSpan Position)
	{
		AnimationFrameData result = data[0];
		for (int i = 0; i < data.Count && data[i].FramePosition <= Position; i++)
		{
			result = data[i];
		}
		return result;
	}

	private static AnimationFrameData GetNextFrame(List<AnimationFrameData> data, TimeSpan Position)
	{
		AnimationFrameData result = data[0];
		for (int i = 0; i < data.Count; i++)
		{
			if (data[i].FramePosition <= Position)
			{
				result = data[i];
				continue;
			}
			result = data[i];
			break;
		}
		return result;
	}

	private static AnimationData GetAnimationFromPreset(AvatarAnimationPreset preset)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		foreach (AnimationData avatarPCAnimation in AvatarPCAnimations)
		{
			if (avatarPCAnimation.Title == ((object)preset).ToString())
			{
				return avatarPCAnimation;
			}
		}
		return null;
	}

	private static void LoadPCAnimations()
	{
		AvatarPCAnimations = new List<AnimationData>();
		ICollection<string> files = Directory.GetFiles(content.RootDirectory + "\\AnimationData");
		foreach (string item in files)
		{
			LoadAnim(item);
		}
	}

	private static void LoadAnim(string filename)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014b: Unknown result type (might be due to invalid IL or missing references)
		FileStream fileStream = File.OpenRead(filename);
		BinaryReader binaryReader = new BinaryReader(fileStream);
		AnimationData animationData = new AnimationData();
		animationData.Title = binaryReader.ReadString();
		animationData.Length = TimeSpan.FromTicks(binaryReader.ReadInt64());
		int num = binaryReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			AnimationFrameData animationFrameData = new AnimationFrameData();
			animationFrameData.FramePosition = TimeSpan.FromTicks(binaryReader.ReadInt64() * 6);
			int num2 = binaryReader.ReadInt32();
			for (int j = 0; j < num2; j++)
			{
				Matrix item = default(Matrix);
				item.M11 = binaryReader.ReadSingle();
				item.M12 = binaryReader.ReadSingle();
				item.M13 = binaryReader.ReadSingle();
				item.M14 = binaryReader.ReadSingle();
				item.M21 = binaryReader.ReadSingle();
				item.M22 = binaryReader.ReadSingle();
				item.M23 = binaryReader.ReadSingle();
				item.M24 = binaryReader.ReadSingle();
				item.M31 = binaryReader.ReadSingle();
				item.M32 = binaryReader.ReadSingle();
				item.M33 = binaryReader.ReadSingle();
				item.M34 = binaryReader.ReadSingle();
				item.M41 = binaryReader.ReadSingle();
				item.M42 = binaryReader.ReadSingle();
				item.M43 = binaryReader.ReadSingle();
				item.M44 = binaryReader.ReadSingle();
				animationFrameData.BoneTransforms.Add(item);
			}
			animationData.animationData.Add(animationFrameData);
		}
		AvatarPCAnimations.Add(animationData);
		fileStream.Close();
		fileStream.Dispose();
	}

	public static void Initialize(GraphicsDevice gfxDevice, ContentManager cnt)
	{
		content = cnt;
		graphicsDevice = gfxDevice;
		if (IsPC)
		{
			LoadPCAnimations();
		}
	}

	public void StartAnimation(AvatarAnimationPreset avatarAnimationPreset, bool LoopAnimation)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		currentAnimationType = AnimationType.BuiltIn;
		Animation = new AvatarAnimation(avatarAnimationPreset);
		AnimationPC = new AvatarPCAnimation(avatarAnimationPreset);
		isLoopingAnimation = LoopAnimation;
		isAnimating = true;
	}

	public void StartAnimation(AvatarGenderNeutralAnimationPreset avatarAnimationPreset, bool LoopAnimation)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Expected O, but got Unknown
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		currentAnimationType = AnimationType.BuiltIn;
		AvatarAnimationPreset builtInPresetFromGenderNeutralPreset = GetBuiltInPresetFromGenderNeutralPreset(avatarAnimationPreset);
		Animation = new AvatarAnimation(builtInPresetFromGenderNeutralPreset);
		AnimationPC = new AvatarPCAnimation(builtInPresetFromGenderNeutralPreset);
		isLoopingAnimation = LoopAnimation;
		isAnimating = true;
	}

	public void StartAnimation(CustomAvatarAnimationData animationData, bool LoopAnimation)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Expected O, but got Unknown
		currentXNACustomAnimation = new CustomAvatarAnimationPlayer(animationData.Name, animationData.Length, animationData.Keyframes, animationData.ExpressionKeyframes);
		currentAnimationType = AnimationType.CustomAnimationXNA;
		isLoopingAnimation = LoopAnimation;
		isAnimating = true;
	}

	public void ResetAnimation()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Expected O, but got Unknown
		currentAnimationType = AnimationType.BuiltIn;
		Animation = new AvatarAnimation((AvatarAnimationPreset)0);
		AnimationPC = new AvatarPCAnimation((AvatarAnimationPreset)0);
		isAnimating = false;
	}

	public void StopAnimation()
	{
		isAnimating = false;
	}

	public void ContinueAnimation()
	{
		isAnimating = true;
	}

	private AvatarAnimationPreset GetBuiltInPresetFromGenderNeutralPreset(AvatarGenderNeutralAnimationPreset avatarAnimationPreset)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0113: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0120: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		AvatarAnimationPreset result = (AvatarAnimationPreset)10;
		bool flag = true;
		if ((int)Description.BodyType == 0)
		{
			flag = false;
		}
		switch (avatarAnimationPreset)
		{
		case AvatarGenderNeutralAnimationPreset.Angry:
			result = ((!flag) ? ((AvatarAnimationPreset)15) : ((AvatarAnimationPreset)25));
			break;
		case AvatarGenderNeutralAnimationPreset.Celebrate:
			result = (AvatarAnimationPreset)10;
			break;
		case AvatarGenderNeutralAnimationPreset.CheckHand:
			result = ((!flag) ? ((AvatarAnimationPreset)11) : ((AvatarAnimationPreset)24));
			break;
		case AvatarGenderNeutralAnimationPreset.Clap:
			result = (AvatarAnimationPreset)8;
			break;
		case AvatarGenderNeutralAnimationPreset.Confused:
			result = ((!flag) ? ((AvatarAnimationPreset)16) : ((AvatarAnimationPreset)26));
			break;
		case AvatarGenderNeutralAnimationPreset.Cry:
			result = ((!flag) ? ((AvatarAnimationPreset)18) : ((AvatarAnimationPreset)28));
			break;
		case AvatarGenderNeutralAnimationPreset.Laugh:
			result = ((!flag) ? ((AvatarAnimationPreset)17) : ((AvatarAnimationPreset)27));
			break;
		case AvatarGenderNeutralAnimationPreset.LookAround:
			result = ((!flag) ? ((AvatarAnimationPreset)12) : ((AvatarAnimationPreset)21));
			break;
		case AvatarGenderNeutralAnimationPreset.ShiftWeight:
			result = ((!flag) ? ((AvatarAnimationPreset)13) : ((AvatarAnimationPreset)23));
			break;
		case AvatarGenderNeutralAnimationPreset.Stand0:
			result = (AvatarAnimationPreset)0;
			break;
		case AvatarGenderNeutralAnimationPreset.Stand1:
			result = (AvatarAnimationPreset)1;
			break;
		case AvatarGenderNeutralAnimationPreset.Stand2:
			result = (AvatarAnimationPreset)2;
			break;
		case AvatarGenderNeutralAnimationPreset.Stand3:
			result = (AvatarAnimationPreset)3;
			break;
		case AvatarGenderNeutralAnimationPreset.Stand4:
			result = (AvatarAnimationPreset)4;
			break;
		case AvatarGenderNeutralAnimationPreset.Stand5:
			result = (AvatarAnimationPreset)5;
			break;
		case AvatarGenderNeutralAnimationPreset.Stand6:
			result = (AvatarAnimationPreset)6;
			break;
		case AvatarGenderNeutralAnimationPreset.Stand7:
			result = (AvatarAnimationPreset)7;
			break;
		case AvatarGenderNeutralAnimationPreset.Surprise:
			result = ((!flag) ? ((AvatarAnimationPreset)19) : ((AvatarAnimationPreset)29));
			break;
		case AvatarGenderNeutralAnimationPreset.Wave:
			result = (AvatarAnimationPreset)9;
			break;
		case AvatarGenderNeutralAnimationPreset.Yawn:
			result = ((!flag) ? ((AvatarAnimationPreset)20) : ((AvatarAnimationPreset)30));
			break;
		}
		return result;
	}

	public Avatar(Gamer gamer)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Expected O, but got Unknown
		Viewport viewport = graphicsDevice.Viewport;
		projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4f, ((Viewport)(ref viewport)).AspectRatio, 0.01f, 200f);
		base._002Ector();
		if (gamer != null)
		{
			LoadAvatar(gamer);
		}
		else
		{
			LoadRandomAvatar();
		}
		Animation = new AvatarAnimation((AvatarAnimationPreset)0);
		AnimationPC = new AvatarPCAnimation((AvatarAnimationPreset)0);
		isAnimating = false;
	}

	public void Update(GameTime gameTime)
	{
		TimeSpan timeSpan = gameTime.ElapsedGameTime;
		if (!isAnimating)
		{
			timeSpan = TimeSpan.Zero;
		}
		switch (currentAnimationType)
		{
		case AnimationType.BuiltIn:
			Animation.Update(timeSpan, IsLoopingAnimation);
			if (IsPC)
			{
				AnimationPC.Update(timeSpan, IsLoopingAnimation);
			}
			break;
		case AnimationType.CustomAnimationXNA:
			currentXNACustomAnimation.Update(timeSpan, IsLoopingAnimation);
			break;
		}
	}

	public void Draw()
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		if (!IsPC)
		{
			if (Renderer == null)
			{
				return;
			}
			Renderer.View = View;
			Renderer.Projection = Projection;
			Renderer.World = World;
		}
		switch (currentAnimationType)
		{
		case AnimationType.BuiltIn:
			if (!IsPC)
			{
				Renderer.Draw((IList<Matrix>)Animation.BoneTransforms, Expression);
			}
			else
			{
				DrawWireframeBones(AnimationPC.BoneTransforms);
			}
			break;
		case AnimationType.CustomAnimationXNA:
		{
			Matrix[] array = (Matrix[])(object)new Matrix[currentXNACustomAnimation.BoneTransforms.Count];
			currentXNACustomAnimation.BoneTransforms.CopyTo(array, 0);
			if (!IsPC)
			{
				Renderer.Draw((IList<Matrix>)array, Expression);
			}
			else
			{
				DrawWireframeBones(array);
			}
			break;
		}
		}
	}

	private void DrawWireframeBones(IList<Matrix> Bones)
	{
	}

	private void LoadAvatar(Gamer gamer)
	{
		UnloadAvatar();
		AvatarDescription.BeginGetFromGamer(gamer, (AsyncCallback)LoadAvatarDescription, (object)null);
	}

	private void LoadAvatarDescription(IAsyncResult result)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		description = AvatarDescription.EndGetFromGamer(result);
		if (description.IsValid)
		{
			Renderer = new AvatarRenderer(description);
			Renderer.View = View;
			Renderer.Projection = projection;
		}
		else
		{
			LoadRandomAvatar();
		}
	}

	private void LoadRandomAvatar()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		UnloadAvatar();
		description = AvatarDescription.CreateRandom();
		Renderer = new AvatarRenderer(description);
		Renderer.View = View;
		Renderer.Projection = projection;
	}

	private void UnloadAvatar()
	{
		if (Renderer != null)
		{
			Renderer.Dispose();
			Renderer = null;
		}
		description = null;
	}
}
