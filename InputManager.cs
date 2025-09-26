using System.Numerics;
using Raylib_cs;

namespace ThatOnePVZMinigame;
internal class InputManager {

	public static Vector2 MousePosition => Raylib.GetMousePosition();
	public static MouseButton InteractKey = MouseButton.Left;
	public static event Action<Vector2, bool, int>? OnClick;

	public static KeyboardKey BuyChumA = KeyboardKey.Q;
	public static KeyboardKey BuyChumB = KeyboardKey.W;

	public static KeyboardKey BuyFishA = KeyboardKey.One;
	public static KeyboardKey BuyFishB = KeyboardKey.Two;
	public static KeyboardKey BuyFishC = KeyboardKey.Three;
	public static KeyboardKey BuyFishD = KeyboardKey.Four;


	public static void Update(bool v = false, int s = 0) {
		if (Raylib.IsMouseButtonPressed(InteractKey)) OnClick?.Invoke(MousePosition, v, s + 1);

		if (Raylib.IsKeyPressed(BuyChumA)) StoreManager.PurchaseChum<SmallChum>(MousePosition, v, s + 1);

		if (Raylib.IsKeyPressed(BuyChumB)) StoreManager.PurchaseChum<LargeChum>(MousePosition, v, s + 1);

		if (Raylib.IsKeyPressed(BuyFishA)) StoreManager.PurchaseFish<Cod>(v, s + 1);

		if (Raylib.IsKeyPressed(BuyFishB)) StoreManager.PurchaseFish<Bass>(v, s + 1);

		if (Raylib.IsKeyPressed(BuyFishC)) StoreManager.PurchaseFish<JanitorFish>(v, s + 1);

		if (Raylib.IsKeyPressed(BuyFishD)) StoreManager.PurchaseFish<CarnivoreFish>(v, s + 1);
	}
}
