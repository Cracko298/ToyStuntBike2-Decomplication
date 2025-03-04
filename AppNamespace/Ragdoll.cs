using System.Collections.Generic;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;

namespace AppNamespace;

public class Ragdoll
{
	private const float headDensity = 10f;

	private const float bodyDensity = 10f;

	private const float ArmDensity = 10f;

	private const float LegDensity = 15f;

	private const float LimbAngularDamping = 7f;

	public const CollisionCategory BikeCategory = 1;

	public const CollisionCategory TorsoCategory = 2;

	public const CollisionCategory LeftArmCategory = 4;

	public const CollisionCategory RightArmCategory = 8;

	public const CollisionCategory LeftLegCategory = 16;

	public const CollisionCategory RightLegCategory = 32;

	public const CollisionCategory TorsoCategory2 = 2048;

	public const CollisionCategory LeftArmCategory2 = 4096;

	public const CollisionCategory RightArmCategory2 = 8192;

	public const CollisionCategory LeftLegCategory2 = 16384;

	public const CollisionCategory RightLegCategory2 = 32768;

	public const CollisionCategory OtherCategory = 1073741824;

	private const float Scale = 0.3f;

	private const float MassScale = 0.5f;

	public Fixture _head;

	public List<Fixture> _body;

	public List<Fixture> _lowerLeftArm;

	public List<Fixture> _lowerLeftLeg;

	public List<Fixture> _lowerRightArm;

	public List<Fixture> _lowerRightLeg;

	public List<Fixture> _upperLeftArm;

	public List<Fixture> _upperLeftLeg;

	public List<Fixture> _upperRightArm;

	public List<Fixture> _upperRightLeg;

	public RevoluteJoint HandlebarLeftRevolute;

	public RevoluteJoint PedalLeftRevolute;

	public RevoluteJoint HandlebarRightRevolute;

	public RevoluteJoint PedalRightRevolute;

	public RevoluteJoint TorsoRevolute;

	public bool m_bOnBike;

	public Player m_Player;

	public Body Body => _body[0].Body;

	public Ragdoll(World world, Vector2 position, Player p)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		m_Player = p;
		CreateBody(world, position);
		CreateJoints(world);
	}

	private void CreateBody(World world, Vector2 position)
	{
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_0121: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_023d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0248: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0364: Unknown result type (might be due to invalid IL or missing references)
		//IL_036f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_048b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0496: Unknown result type (might be due to invalid IL or missing references)
		//IL_049b: Unknown result type (might be due to invalid IL or missing references)
		//IL_05b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_05bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_05c2: Unknown result type (might be due to invalid IL or missing references)
		//IL_06c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_06d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_07f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_07fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0803: Unknown result type (might be due to invalid IL or missing references)
		//IL_0908: Unknown result type (might be due to invalid IL or missing references)
		//IL_0913: Unknown result type (might be due to invalid IL or missing references)
		//IL_0918: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a35: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a40: Unknown result type (might be due to invalid IL or missing references)
		//IL_0a45: Unknown result type (might be due to invalid IL or missing references)
		_head = FixtureFactory.CreateCircle(world, 0.36f, 10f);
		_head.Body.BodyType = (BodyType)2;
		_head.Body.AngularDamping = 7f;
		_head.Body.Mass = 0.5f;
		_head.Body.Position = position;
		if (m_Player.m_Id == 0)
		{
			_head.CollisionCategories = (CollisionCategory)2;
			_head.CollidesWith = (CollisionCategory)1073741886;
		}
		else
		{
			_head.CollisionCategories = (CollisionCategory)2048;
			_head.CollidesWith = (CollisionCategory)1073805312;
		}
		_body = FixtureFactory.CreateRoundedRectangle(world, 0.6f, 1.2f, 0.15f, 0.21000001f, 2, 10f);
		_body[0].Body.BodyType = (BodyType)2;
		_body[0].Body.Mass = 0.5f;
		_body[0].Body.Position = position + new Vector2(0f, -0.90000004f);
		for (int i = 0; i < _body.Count; i++)
		{
			if (m_Player.m_Id == 0)
			{
				_body[i].CollisionCategories = (CollisionCategory)2;
				_body[i].CollidesWith = (CollisionCategory)1073741826;
			}
			else
			{
				_body[i].CollisionCategories = (CollisionCategory)2048;
				_body[i].CollidesWith = (CollisionCategory)1073743872;
			}
		}
		_lowerLeftArm = FixtureFactory.CreateCapsule(world, 0.3f, 0.135f, 10f);
		_lowerLeftArm[0].Body.BodyType = (BodyType)2;
		_lowerLeftArm[0].Body.AngularDamping = 7f;
		_lowerLeftArm[0].Body.Mass = 0.5f;
		_lowerLeftArm[0].Body.Rotation = 0f;
		_lowerLeftArm[0].Body.Position = position + new Vector2(1.2f, -1.2f);
		for (int j = 0; j < _lowerLeftArm.Count; j++)
		{
			if (m_Player.m_Id == 0)
			{
				_lowerLeftArm[j].CollisionCategories = (CollisionCategory)4;
				_lowerLeftArm[j].CollidesWith = (CollisionCategory)1073741830;
			}
			else
			{
				_lowerLeftArm[j].CollisionCategories = (CollisionCategory)4096;
				_lowerLeftArm[j].CollidesWith = (CollisionCategory)1073747968;
			}
		}
		_upperLeftArm = FixtureFactory.CreateCapsule(world, 0.42000002f, 0.135f, 10f);
		_upperLeftArm[0].Body.BodyType = (BodyType)2;
		_upperLeftArm[0].Body.AngularDamping = 7f;
		_upperLeftArm[0].Body.Mass = 0.5f;
		_upperLeftArm[0].Body.Rotation = 0f;
		_upperLeftArm[0].Body.Position = position + new Vector2(0.6f, -0.69f);
		for (int k = 0; k < _upperLeftArm.Count; k++)
		{
			if (m_Player.m_Id == 0)
			{
				_upperLeftArm[k].CollisionCategories = (CollisionCategory)4;
				_upperLeftArm[k].CollidesWith = (CollisionCategory)1073741828;
			}
			else
			{
				_upperLeftArm[k].CollisionCategories = (CollisionCategory)4096;
				_upperLeftArm[k].CollidesWith = (CollisionCategory)1073745920;
			}
		}
		_lowerRightArm = FixtureFactory.CreateCapsule(world, 0.3f, 0.135f, 10f);
		_lowerRightArm[0].Body.BodyType = (BodyType)2;
		_lowerRightArm[0].Body.AngularDamping = 7f;
		_lowerRightArm[0].Body.Mass = 0.5f;
		_lowerRightArm[0].Body.Rotation = 0f;
		_lowerRightArm[0].Body.Position = position + new Vector2(1.2f, -1.2f);
		for (int l = 0; l < _lowerRightArm.Count; l++)
		{
			if (m_Player.m_Id == 0)
			{
				_lowerRightArm[l].CollisionCategories = (CollisionCategory)8;
				_lowerRightArm[l].CollidesWith = (CollisionCategory)1073741834;
			}
			else
			{
				_lowerRightArm[l].CollisionCategories = (CollisionCategory)8192;
				_lowerRightArm[l].CollidesWith = (CollisionCategory)1073752064;
			}
		}
		_upperRightArm = FixtureFactory.CreateCapsule(world, 0.42000002f, 0.135f, 10f);
		_upperRightArm[0].Body.BodyType = (BodyType)2;
		_upperRightArm[0].Body.AngularDamping = 7f;
		_upperRightArm[0].Body.Mass = 0.5f;
		_upperRightArm[0].Body.Rotation = 0f;
		_upperRightArm[0].Body.Position = position + new Vector2(0.6f, -0.69f);
		for (int m = 0; m < _upperRightArm.Count; m++)
		{
			if (m_Player.m_Id == 0)
			{
				_upperRightArm[m].CollisionCategories = (CollisionCategory)8;
				_upperRightArm[m].CollidesWith = (CollisionCategory)1073741832;
			}
			else
			{
				_upperRightArm[m].CollisionCategories = (CollisionCategory)8192;
				_upperRightArm[m].CollidesWith = (CollisionCategory)1073750016;
			}
		}
		_lowerLeftLeg = FixtureFactory.CreateCapsule(world, 0.3f, 0.15f, 15f);
		_lowerLeftLeg[0].Body.BodyType = (BodyType)2;
		_lowerLeftLeg[0].Body.AngularDamping = 7f;
		_lowerLeftLeg[0].Body.Mass = 0.5f;
		_lowerLeftLeg[0].Body.Position = position + new Vector2(0.3f, -2.28f);
		for (int n = 0; n < _lowerLeftLeg.Count; n++)
		{
			if (m_Player.m_Id == 0)
			{
				_lowerLeftLeg[n].CollisionCategories = (CollisionCategory)16;
				_lowerLeftLeg[n].CollidesWith = (CollisionCategory)18;
			}
			else
			{
				_lowerLeftLeg[n].CollisionCategories = (CollisionCategory)16384;
				_lowerLeftLeg[n].CollidesWith = (CollisionCategory)18432;
			}
		}
		_upperLeftLeg = FixtureFactory.CreateCapsule(world, 0.3f, 0.15f, 15f);
		_upperLeftLeg[0].Body.BodyType = (BodyType)2;
		_upperLeftLeg[0].Body.AngularDamping = 7f;
		_upperLeftLeg[0].Body.Mass = 0.5f;
		_upperLeftLeg[0].Body.Rotation = 1f;
		_upperLeftLeg[0].Body.Position = position + new Vector2(0.120000005f, -1.7400001f);
		for (int num = 0; num < _upperLeftLeg.Count; num++)
		{
			if (m_Player.m_Id == 0)
			{
				_upperLeftLeg[num].CollisionCategories = (CollisionCategory)16;
				_upperLeftLeg[num].CollidesWith = (CollisionCategory)1073741842;
			}
			else
			{
				_upperLeftLeg[num].CollisionCategories = (CollisionCategory)16384;
				_upperLeftLeg[num].CollidesWith = (CollisionCategory)1073760256;
			}
		}
		_lowerRightLeg = FixtureFactory.CreateCapsule(world, 0.3f, 0.15f, 15f);
		_lowerRightLeg[0].Body.BodyType = (BodyType)2;
		_lowerRightLeg[0].Body.AngularDamping = 7f;
		_lowerRightLeg[0].Body.Mass = 0.5f;
		_lowerRightLeg[0].Body.Position = position + new Vector2(0.3f, -2.28f);
		for (int num2 = 0; num2 < _lowerRightLeg.Count; num2++)
		{
			if (m_Player.m_Id == 0)
			{
				_lowerRightLeg[num2].CollisionCategories = (CollisionCategory)32;
				_lowerRightLeg[num2].CollidesWith = (CollisionCategory)34;
			}
			else
			{
				_lowerRightLeg[num2].CollisionCategories = (CollisionCategory)32768;
				_lowerRightLeg[num2].CollidesWith = (CollisionCategory)34816;
			}
		}
		_upperRightLeg = FixtureFactory.CreateCapsule(world, 0.3f, 0.15f, 15f);
		_upperRightLeg[0].Body.BodyType = (BodyType)2;
		_upperRightLeg[0].Body.AngularDamping = 7f;
		_upperRightLeg[0].Body.Mass = 0.5f;
		_upperRightLeg[0].Body.Rotation = 1f;
		_upperRightLeg[0].Body.Position = position + new Vector2(0.120000005f, -1.7400001f);
		for (int num3 = 0; num3 < _upperRightLeg.Count; num3++)
		{
			if (m_Player.m_Id == 0)
			{
				_upperRightLeg[num3].CollisionCategories = (CollisionCategory)32;
				_upperRightLeg[num3].CollidesWith = (CollisionCategory)1073741858;
			}
			else
			{
				_upperRightLeg[num3].CollisionCategories = (CollisionCategory)32768;
				_upperRightLeg[num3].CollidesWith = (CollisionCategory)1073776640;
			}
		}
	}

	private void CreateJoints(World world)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Expected O, but got Unknown
		//IL_009b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Expected O, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Expected O, but got Unknown
		//IL_0185: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019f: Expected O, but got Unknown
		//IL_01fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0209: Unknown result type (might be due to invalid IL or missing references)
		//IL_020e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0215: Expected O, but got Unknown
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_0284: Unknown result type (might be due to invalid IL or missing references)
		//IL_0289: Unknown result type (might be due to invalid IL or missing references)
		//IL_0290: Expected O, but got Unknown
		//IL_02f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0304: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Expected O, but got Unknown
		//IL_036b: Unknown result type (might be due to invalid IL or missing references)
		//IL_037a: Unknown result type (might be due to invalid IL or missing references)
		//IL_037f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0386: Expected O, but got Unknown
		//IL_03e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f5: Unknown result type (might be due to invalid IL or missing references)
		//IL_03fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0401: Expected O, but got Unknown
		//IL_045c: Unknown result type (might be due to invalid IL or missing references)
		//IL_04aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_04f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0546: Unknown result type (might be due to invalid IL or missing references)
		//IL_0594: Unknown result type (might be due to invalid IL or missing references)
		DistanceJoint val = new DistanceJoint(_head.Body, _body[0].Body, new Vector2(0f, -0.3f), new Vector2(0f, 0.6f));
		((Joint)val).CollideConnected = true;
		val.DampingRatio = 1f;
		val.Frequency = 25f;
		val.Length = 0.0075000003f;
		world.AddJoint((Joint)(object)val);
		DistanceJoint val2 = new DistanceJoint(_lowerLeftArm[0].Body, _upperLeftArm[0].Body, new Vector2(0f, 0.3f), new Vector2(0f, -0.3f));
		((Joint)val2).CollideConnected = true;
		val2.DampingRatio = 1f;
		val2.Frequency = 25f;
		val2.Length = 0.09f;
		world.AddJoint((Joint)(object)val2);
		DistanceJoint val3 = new DistanceJoint(_upperLeftArm[0].Body, _body[0].Body, new Vector2(0f, 0.24000001f), new Vector2(0f, 0.45000002f));
		((Joint)val3).CollideConnected = false;
		val3.DampingRatio = 1f;
		val3.Frequency = 25f;
		val3.Length = 0.09f;
		world.AddJoint((Joint)(object)val3);
		DistanceJoint val4 = new DistanceJoint(_lowerRightArm[0].Body, _upperRightArm[0].Body, new Vector2(0f, 0.3f), new Vector2(0f, -0.3f));
		((Joint)val4).CollideConnected = true;
		val4.DampingRatio = 1f;
		val4.Frequency = 25f;
		val4.Length = 0.09f;
		world.AddJoint((Joint)(object)val4);
		DistanceJoint val5 = new DistanceJoint(_upperRightArm[0].Body, _body[0].Body, new Vector2(0f, 0.24000001f), new Vector2(0f, 0.45000002f));
		((Joint)val5).CollideConnected = false;
		val5.DampingRatio = 1f;
		val5.Frequency = 25f;
		val5.Length = 0.09f;
		world.AddJoint((Joint)(object)val5);
		DistanceJoint val6 = new DistanceJoint(_lowerLeftLeg[0].Body, _upperLeftLeg[0].Body, new Vector2(0f, 0.33f), new Vector2(0f, -0.3f));
		((Joint)val6).CollideConnected = true;
		val6.DampingRatio = 1f;
		val6.Frequency = 25f;
		val6.Length = 0.015000001f;
		world.AddJoint((Joint)(object)val6);
		DistanceJoint val7 = new DistanceJoint(_upperLeftLeg[0].Body, _body[0].Body, new Vector2(0f, 0.33f), new Vector2(0.18f, -0.57f));
		((Joint)val7).CollideConnected = true;
		val7.DampingRatio = 1f;
		val7.Frequency = 25f;
		val7.Length = 0.006f;
		world.AddJoint((Joint)(object)val7);
		DistanceJoint val8 = new DistanceJoint(_lowerRightLeg[0].Body, _upperRightLeg[0].Body, new Vector2(0f, 0.33f), new Vector2(0f, -0.3f));
		((Joint)val8).CollideConnected = true;
		val8.DampingRatio = 1f;
		val8.Frequency = 25f;
		val8.Length = 0.015000001f;
		world.AddJoint((Joint)(object)val8);
		DistanceJoint val9 = new DistanceJoint(_upperRightLeg[0].Body, _body[0].Body, new Vector2(0f, 0.33f), new Vector2(0.18f, -0.57f));
		((Joint)val9).CollideConnected = true;
		val9.DampingRatio = 1f;
		val9.Frequency = 25f;
		val9.Length = 0.006f;
		world.AddJoint((Joint)(object)val9);
		HandlebarLeftRevolute = JointFactory.CreateRevoluteJoint(m_Player.m_ChasisBody, _lowerLeftArm[0].Body, new Vector2(0f, -0.1f));
		((Joint)HandlebarLeftRevolute).CollideConnected = false;
		world.AddJoint((Joint)(object)HandlebarLeftRevolute);
		HandlebarRightRevolute = JointFactory.CreateRevoluteJoint(m_Player.m_ChasisBody, _lowerRightArm[0].Body, new Vector2(0f, -0.1f));
		((Joint)HandlebarRightRevolute).CollideConnected = false;
		world.AddJoint((Joint)(object)HandlebarRightRevolute);
		PedalLeftRevolute = JointFactory.CreateRevoluteJoint(m_Player.m_ChasisBody, _lowerLeftLeg[0].Body, new Vector2(0f, 0f));
		((Joint)PedalLeftRevolute).CollideConnected = false;
		world.AddJoint((Joint)(object)PedalLeftRevolute);
		PedalRightRevolute = JointFactory.CreateRevoluteJoint(m_Player.m_ChasisBody, _lowerRightLeg[0].Body, new Vector2(0f, 0f));
		((Joint)PedalRightRevolute).CollideConnected = false;
		world.AddJoint((Joint)(object)PedalRightRevolute);
		TorsoRevolute = JointFactory.CreateRevoluteJoint(m_Player.m_ChasisBody, _body[0].Body, new Vector2(0f, -0.4f));
		((Joint)TorsoRevolute).CollideConnected = false;
		world.AddJoint((Joint)(object)TorsoRevolute);
		m_bOnBike = true;
	}
}
