using Raylib_cs;
using System.Numerics;

namespace ThatOnePVZMinigame;
internal class Master {

	public static bool EnableLogging = true;
	public static bool EnableUpdateLogging = true;
	public static bool IsGameOngoing = true;
	public static string EndText = string.Empty;

	static void Main(string[] args) {
		Log.Me(() => "Starting game...", EnableLogging);
		Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
		Raylib.InitWindow(1280, 720, "That one PVZ minigame");
		Raylib.InitAudioDevice();
		Raylib.SetMasterVolume(0.6f);
		Raylib.SetTargetFPS(60);

		Log.Me(() => "Doing initial setup...", EnableLogging);
		ResourceManager.PreloadAssets(EnableLogging);
		Texture2D background = ResourceManager.GetTexture("background");

		WorldManager.SpawnFish<Cod>(EnableLogging);

		Log.Me(() => "Entering main game loop...", EnableLogging);
		while (!Raylib.WindowShouldClose()) {
			Log.Me(() => "Updating game state...", EnableUpdateLogging);
			Raylib.BeginDrawing();
			Raylib.ClearBackground(Color.RayWhite);
			Raylib.DrawTexture(background, 0, 0, Color.White);

			WorldManager.Update(EnableUpdateLogging);

			// This should definitely be in a game manager class or something,
			// but I want to finish this already. :P
			if (IsGameOngoing) {
				// Receive inputs only when the game is still ongoing.
				InputManager.Update(EnableUpdateLogging);

				bool isBroke = StoreManager.Money < Cod.Cost;
				bool coinless = WorldManager.GetAllObjectsOfType<Coin>().Count == 0;
				bool fishless = true;

				// Check if there are any alive fishes
				List<Fish> fishes = WorldManager.GetAllObjectsOfType<Fish>();
				foreach (Fish f in fishes) {
					if (f.IsAlive) {
						fishless = false;
						break;
					}
				}

				// LOSE LOL HAHA XD 💀
				if (isBroke && fishless && coinless) {
					IsGameOngoing = false;
					EndText = "You lose! You ran out of money and fishes!";
				}
			}

			// Game over
			else {
				Raylib.DrawText(EndText, 100, 100, 50, Color.RayWhite);
			}

			ResourceManager.PlayMusic(EnableUpdateLogging);

			Raylib.DrawText($"Money: {StoreManager.Money}", 10, 10, 20, Color.Yellow);
			Raylib.EndDrawing();
			Log.Me(() => "Frame complete.\n", EnableUpdateLogging);
		}

		Log.Me(() => "Exiting game...", EnableLogging);
		Raylib.CloseWindow();
	}
}
