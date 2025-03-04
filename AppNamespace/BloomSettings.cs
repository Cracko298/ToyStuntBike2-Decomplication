namespace AppNamespace;

public class BloomSettings
{
	public readonly string Name;

	public float BloomThreshold;

	public float BlurAmount;

	public readonly float BloomIntensity;

	public readonly float BaseIntensity;

	public float BloomSaturation;

	public float BaseSaturation;

	public static BloomSettings[] PresetSettings = new BloomSettings[8]
	{
		new BloomSettings("Default", 0.8f, 1f, 1f, 1f, 0.6f, 1f),
		new BloomSettings("CopyOfDefault", 0.65f, 3f, 1.25f, 1f, 1f, 1f),
		new BloomSettings("Default", 0.3f, 1f, 1f, 1f, 0.6f, 1f),
		new BloomSettings("Soft", 0.8f, 2f, 1f, 1f, 1f, 1f),
		new BloomSettings("Desaturated", 0.5f, 8f, 2f, 1f, 0f, 0f),
		new BloomSettings("Saturated", 0.25f, 4f, 2f, 1f, 2f, 0f),
		new BloomSettings("Blurry", 0f, 2f, 1f, 0.1f, 1f, 1f),
		new BloomSettings("Subtle", 0.5f, 2f, 1f, 1f, 1f, 1f)
	};

	public BloomSettings(string name, float bloomThreshold, float blurAmount, float bloomIntensity, float baseIntensity, float bloomSaturation, float baseSaturation)
	{
		Name = name;
		BloomThreshold = bloomThreshold;
		BlurAmount = blurAmount;
		BloomIntensity = bloomIntensity;
		BaseIntensity = baseIntensity;
		BloomSaturation = bloomSaturation;
		BaseSaturation = baseSaturation;
	}
}
