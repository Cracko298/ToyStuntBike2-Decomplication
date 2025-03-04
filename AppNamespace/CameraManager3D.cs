using Maths;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace AppNamespace;

public class CameraManager3D
{
	public enum State
	{
		ZOOMIN,
		SPEECH,
		INGAME
	}

	private const float MODIFY_Z_MAX = 3f;

	private const float MODIFY_Z_SPEED = 10f;

	public static Vector3 m_CameraInGamePosition = new Vector3(0f, 3.9f, 10f);

	public static Vector3 m_CameraInGameLookAt = new Vector3(0f, 1f, -10f);

	public static Vector3 m_FlyInCameraStart = new Vector3(0f, 16.3f, 19.8f);

	public static Vector3 m_SpeechPosition = new Vector3(-6f, 2f, -6f);

	public static Vector3 m_SpeechLookAtTarget = new Vector3(-6f, 2f, -10f);

	public static float CAMERA_MIN_HEIGHT = 0.75f;

	public static float CAMERA_POSITION_OFFSET_X = 5f;

	public static float CAMERA_POSITION_OFFSET_Y = 1f;

	public static float CAMERA_TARGET_OFFSET_X = 10f;

	public static float CAMERA_TARGET_OFFSET_Y = 0f;

	public Vector3 m_CameraPosition = m_CameraInGamePosition;

	public Vector3 m_CameraPositionTarget = m_CameraInGamePosition;

	public Vector3 m_CameraLookAt = m_CameraInGameLookAt;

	public Vector3 m_CameraLookAtTarget = m_CameraInGameLookAt;

	public Matrix m_CameraProjectionMatrix;

	public Matrix m_CameraViewMatrix;

	public State m_State;

	public float m_InterpRate;

	private float m_ShakeTime;

	private float m_ShakeDuration;

	private float m_ShakeMag;

	private float rot;

	private Player m_Player;

	public void Init(Player p)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		m_Player = p;
		m_CameraPosition = m_FlyInCameraStart;
		m_CameraLookAt = m_CameraInGameLookAt;
		m_InterpRate = 0f;
		m_State = State.ZOOMIN;
		m_ShakeMag = 0f;
		m_ShakeTime = 0f;
		m_CameraViewMatrix = Matrix.CreateLookAt(m_CameraPosition, m_CameraLookAt, Vector3.Up);
		float num = MathHelper.ToRadians(45f);
		Viewport viewport = Program.m_App.graphics.GraphicsDevice.Viewport;
		m_CameraProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(num, ((Viewport)(ref viewport)).AspectRatio, 1f, 300f);
	}

	public Vector3 WorldToScreen(Vector3 position)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		Matrix identity = Matrix.Identity;
		((Matrix)(ref identity)).Translation = position;
		Viewport viewport = ((Game)Program.m_App).GraphicsDevice.Viewport;
		return ((Viewport)(ref viewport)).Project(Vector3.Zero, m_CameraProjectionMatrix, m_CameraViewMatrix, identity);
	}

	public void Update()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0403: Unknown result type (might be due to invalid IL or missing references)
		//IL_0409: Unknown result type (might be due to invalid IL or missing references)
		//IL_040e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0413: Unknown result type (might be due to invalid IL or missing references)
		//IL_0418: Unknown result type (might be due to invalid IL or missing references)
		switch (m_State)
		{
		case State.ZOOMIN:
			m_CameraPositionTarget = m_SpeechPosition;
			m_CameraLookAtTarget = m_SpeechLookAtTarget;
			m_State = State.SPEECH;
			break;
		case State.SPEECH:
			m_State = State.INGAME;
			m_CameraPositionTarget = m_CameraInGamePosition;
			m_CameraLookAtTarget = m_CameraInGameLookAt;
			m_InterpRate = 0f;
			break;
		case State.INGAME:
			if (!Program.m_App.m_bEditor && (!Program.m_App.m_bLevelEditor || Program.m_App.m_bInPlaytest))
			{
				m_CameraLookAtTarget.Z = -10f;
				if (m_Player.m_Avatar != null)
				{
					float num = 0f;
					m_CameraPositionTarget.X = m_Player.m_Avatar.Position.X - CAMERA_POSITION_OFFSET_X;
					m_CameraPositionTarget.Y = m_Player.m_Avatar.Position.Y + CAMERA_POSITION_OFFSET_Y + num;
					m_CameraLookAtTarget.X = m_Player.m_Avatar.Position.X + CAMERA_TARGET_OFFSET_X;
					m_CameraLookAtTarget.Y = m_Player.m_Avatar.Position.Y + CAMERA_TARGET_OFFSET_Y + num;
				}
				else
				{
					m_CameraPositionTarget.X = m_Player.m_Position.X - CAMERA_POSITION_OFFSET_X;
					m_CameraPositionTarget.Y = m_Player.m_Position.Y + CAMERA_POSITION_OFFSET_Y;
					m_CameraLookAtTarget.X = m_Player.m_Position.X + CAMERA_TARGET_OFFSET_X;
					m_CameraLookAtTarget.Y = m_Player.m_Position.Y + CAMERA_TARGET_OFFSET_Y;
				}
				if (Program.m_App.m_bSplitScreen && Program.m_App.m_Options.m_SplitScreenHoriz)
				{
					ref Vector3 cameraLookAtTarget = ref m_CameraLookAtTarget;
					cameraLookAtTarget.Y -= 0.25f;
					ref Vector3 cameraPositionTarget = ref m_CameraPositionTarget;
					cameraPositionTarget.Y -= 0.25f;
				}
				if (m_CameraPositionTarget.Y < CAMERA_MIN_HEIGHT)
				{
					m_CameraPositionTarget.Y = CAMERA_MIN_HEIGHT;
				}
				SpeedAdjust();
			}
			else
			{
				Program.m_CameraManager3D.m_CameraPositionTarget.X = Program.m_App.m_LevelPos;
				Program.m_CameraManager3D.m_CameraLookAtTarget.X = Program.m_App.m_LevelPos;
			}
			break;
		}
		m_InterpRate += 0.0004f;
		if (m_InterpRate > 0.25f)
		{
			m_InterpRate = 0.25f;
		}
		m_CameraPosition.X = Fn.LerpCosT(m_CameraPosition.X, m_CameraPositionTarget.X, m_InterpRate, 0.25f);
		m_CameraPosition.Y = Fn.LerpCosT(m_CameraPosition.Y, m_CameraPositionTarget.Y, m_InterpRate, 0.1875f);
		m_CameraPosition.Z = Fn.LerpCosT(m_CameraPosition.Z, m_CameraPositionTarget.Z, m_InterpRate, 0.25f);
		m_CameraLookAt.X = Fn.LerpCosT(m_CameraLookAt.X, m_CameraLookAtTarget.X, m_InterpRate, 0.25f);
		m_CameraLookAt.Y = Fn.LerpCosT(m_CameraLookAt.Y, m_CameraLookAtTarget.Y, m_InterpRate, 0.1875f);
		m_CameraLookAt.Z = Fn.LerpCosT(m_CameraLookAt.Z, m_CameraLookAtTarget.Z, m_InterpRate, 0.25f);
		UpdateShake();
		if (m_CameraPosition.Y < CAMERA_MIN_HEIGHT)
		{
			m_CameraPosition.Y = CAMERA_MIN_HEIGHT;
		}
		m_CameraViewMatrix = Matrix.CreateLookAt(m_CameraPosition, m_CameraLookAt, Vector3.Up);
	}

	public void SpeedAdjust()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Vector2 linearVelocity = m_Player.m_Ragdoll.Body.LinearVelocity;
		float num = ((Vector2)(ref linearVelocity)).Length();
		num /= 15f;
		num = 1f - num;
		num = MathHelper.Clamp(num, 0f, 1f);
		m_CameraPositionTarget.Z = 10f - num * 2f;
	}

	public void Shake(float time, float mag)
	{
		m_ShakeTime = (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds + time;
		m_ShakeDuration = time;
		m_ShakeMag = mag;
	}

	private void UpdateShake()
	{
		if (!(m_ShakeTime <= (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds) && m_ShakeMag != 0f)
		{
			float num = (m_ShakeTime - (float)Program.m_App.m_GameTime.TotalGameTime.TotalMilliseconds) / m_ShakeDuration;
			float num2 = ((float)Program.m_App.m_Rand.NextDouble() - 0.5f) * m_ShakeMag * num;
			float num3 = ((float)Program.m_App.m_Rand.NextDouble() - 0.5f) * m_ShakeMag * num;
			ref Vector3 cameraPosition = ref m_CameraPosition;
			cameraPosition.X += num2;
			ref Vector3 cameraPosition2 = ref m_CameraPosition;
			cameraPosition2.Y += num3;
			ref Vector3 cameraLookAt = ref m_CameraLookAt;
			cameraLookAt.X += num2;
			ref Vector3 cameraLookAt2 = ref m_CameraLookAt;
			cameraLookAt2.Y += num3;
		}
	}
}
