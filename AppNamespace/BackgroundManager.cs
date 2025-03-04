namespace AppNamespace;

public class BackgroundManager
{
	public const float TILE_SIZE = 101f;

	public const float TILE_CAMERA_BIAS = 40f;

	public const float NUM_TILES = 2f;

	private App m_App;

	public Actor[] m_Background;

	public int m_CurrentTileId;

	public int m_NextTileId;

	private int m_CurrentTileId1;

	private int m_NextTileId1;

	private float m_X01;

	private float m_X11;

	private int m_CurrentTileId2;

	private int m_NextTileId2;

	private float m_X02;

	private float m_X12;

	public BackgroundManager(App app)
	{
		m_App = app;
		m_CurrentTileId = 0;
		m_NextTileId = 1;
		m_CurrentTileId1 = 0;
		m_NextTileId1 = 1;
		m_CurrentTileId2 = 0;
		m_NextTileId2 = 1;
		m_Background = new Actor[2];
		m_Background[0] = new Actor();
		m_Background[1] = new Actor();
		m_Background[0].m_Position.Y = 0f;
		m_Background[0].m_ZDistance = 0f;
		m_Background[1].m_Position.Y = 0f;
		m_Background[1].m_Position.X = -101f;
		m_Background[1].m_ZDistance = 0f;
		m_X01 = 0f;
		m_X11 = -101f;
		m_X02 = 0f;
		m_X12 = -101f;
	}

	public void SetData1()
	{
		m_CurrentTileId = m_CurrentTileId1;
		m_NextTileId = m_NextTileId1;
		m_Background[0].m_Position.X = m_X01;
		m_Background[1].m_Position.X = m_X11;
	}

	public void EndData1()
	{
		m_CurrentTileId1 = m_CurrentTileId;
		m_NextTileId1 = m_NextTileId;
		m_X01 = m_Background[0].m_Position.X;
		m_X11 = m_Background[1].m_Position.X;
	}

	public void SetData2()
	{
		m_CurrentTileId = m_CurrentTileId2;
		m_NextTileId = m_NextTileId2;
		m_Background[0].m_Position.X = m_X02;
		m_Background[1].m_Position.X = m_X12;
	}

	public void EndData2()
	{
		m_CurrentTileId2 = m_CurrentTileId;
		m_NextTileId2 = m_NextTileId;
		m_X02 = m_Background[0].m_Position.X;
		m_X12 = m_Background[1].m_Position.X;
	}

	public void Start()
	{
	}

	public void Update()
	{
		if (Program.m_CurrentCamera.m_CameraPositionTarget.X > m_Background[m_CurrentTileId].m_Position.X - 40f)
		{
			m_Background[m_NextTileId].m_Position.X = m_Background[m_CurrentTileId].m_Position.X + 101f;
			m_CurrentTileId = m_NextTileId;
			m_NextTileId++;
			if ((float)m_NextTileId >= 2f)
			{
				m_NextTileId = 0;
			}
		}
		if (Program.m_CurrentCamera.m_CameraPositionTarget.X < m_Background[m_CurrentTileId].m_Position.X - 40f)
		{
			m_Background[m_NextTileId].m_Position.X = m_Background[m_CurrentTileId].m_Position.X - 101f;
			m_CurrentTileId = m_NextTileId;
			m_NextTileId++;
			if ((float)m_NextTileId >= 2f)
			{
				m_NextTileId = 0;
			}
		}
	}

	public void Reset()
	{
		m_Background[0].m_Position.X = 0f;
		m_Background[1].m_Position.X = 0f;
		m_CurrentTileId = 0;
		m_NextTileId = 1;
	}
}
