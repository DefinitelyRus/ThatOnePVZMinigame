using Raylib_cs;

namespace ThatOnePVZMinigame;
internal static class ResourceManager {

	public static Dictionary<string, Texture2D> Textures = [];
	public static Dictionary<string, Sound> Sounds = [];

	public static void PreloadAssets(bool v = false, int s = 0) {
		Log.Me(() => "Preloading assets...", v, s + 1);

		// Load textures
		Log.Me(() => "Loading textures...", v, s + 1);
		string[] textureFiles = Directory.GetFiles("Assets\\Sprites", "*.png");
		foreach (string file in textureFiles) {
			string name = Path.GetFileNameWithoutExtension(file);
			Log.Me(() => $"Loading texture \"{name}\" from \"{file}\"...", v, s + 1);
			Texture2D texture = Raylib.LoadTexture(file);
			Textures[name] = texture;
		}

		// Load sounds
		Log.Me(() => "Loading sounds...", v, s + 1);
		string[] audioFiles = Directory.GetFiles("Assets\\Audio", "*.wav");
		foreach (string file in audioFiles) {
			string name = Path.GetFileNameWithoutExtension(file);
			Log.Me(() => $"Loading sound \"{name}\" from \"{file}\"...", v, s + 1);
			Sound sound = Raylib.LoadSound(file);
			Sounds[name] = sound;
		}

		Log.Me(() => "All assets loaded.", v, s + 1);
	}

	public static Texture2D GetTexture(string name, bool v = false, int s = 0) {
		Log.Me(() => $"Getting texture \"{name}\"...", v, s + 1);
		if (Textures.TryGetValue(name, out Texture2D texture)) return texture;

		Log.Err(() => $"Texture '{name}' not found in ResourceManager.", true, s + 1);
		return new();
	}

	public static Sound GetSound(string name, bool v = false, int s = 0) {
		Log.Me(() => $"Getting sound \"{name}\"...", v, s + 1);
		if (Sounds.TryGetValue(name, out Sound sound)) return sound;

		Log.Err(() => $"Sound '{name}' not found in ResourceManager.", true, s + 1);
		return new();
	}
}
