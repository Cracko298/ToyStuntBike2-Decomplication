using System;
using Microsoft.Xna.Framework;

namespace AppNamespace;

internal static class Program
{
	public static PlayerManager m_PlayerManager = new PlayerManager();

	public static CameraManager3D m_CameraManager3D = new CameraManager3D();

	public static CameraManager3D m_CameraManagerSplit = new CameraManager3D();

	public static CameraManager3D m_CurrentCamera = m_CameraManager3D;

	public static DebugManager m_DebugManager = new DebugManager();

	public static TriggerManager m_TriggerManager = new TriggerManager();

	public static ItemManager m_ItemManager = new ItemManager();

	public static ParticleManager m_ParticleManager = new ParticleManager();

	public static LoadSaveManager m_LoadSaveManager = new LoadSaveManager();

	public static SoundManager m_SoundManager = new SoundManager();

	public static App m_App = new App();

	private static void Main(string[] args)
	{
		App app = m_App;
		try
		{
			((Game)m_App).Run();
		}
		finally
		{
			((IDisposable)app)?.Dispose();
		}
	}
}
