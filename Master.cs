using Raylib_cs;
using System.Numerics;

namespace ThatOnePVZMinigame;
internal class Master {

	public static bool EnableLogging = true;
	public static bool EnableUpdateLogging = true;

	static void Main(string[] args) {
		Log.Me(() => "Starting game...", EnableLogging);
		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
		Raylib.InitWindow(1280, 720, "That one PVZ minigame");
		Raylib.SetTargetFPS(60);

		Log.Me(() => "Doing initial setup...", EnableLogging);
		ResourceManager.PreloadAssets(EnableLogging);
		Texture2D background = ResourceManager.GetTexture("background");

		WorldManager.SpawnFish<Cod>(EnableLogging);

		Log.Me(() => "Entering main game loop...", EnableLogging);
		while (!Raylib.WindowShouldClose()) {
			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.RayWhite);
			Raylib.DrawTexture(background, 0, 0, Color.White);

			InputManager.Update(EnableUpdateLogging);
			WorldManager.Update(EnableUpdateLogging);

			Raylib.DrawText($"Money: {StoreManager.Money}", 10, 10, 20, Color.Yellow);
			Raylib.EndDrawing();
		}

		Log.Me(() => "Exiting game...", EnableLogging);
		Raylib.CloseWindow();
	}
}
