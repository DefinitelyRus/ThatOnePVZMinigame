using System.Numerics;

namespace ThatOnePVZMinigame;
public static class WorldManager {
	private static readonly List<WorldObject> WorldObjects = [];
	private static readonly List<WorldObject> ToAdd = [];
	private static readonly List<WorldObject> ToRemove = [];


	/// <summary>
	/// Spawns a fish of type T and adds it to the world.
	/// </summary>
	/// <typeparam name="T">The specific subtype of <see cref="Fish"/> to spawn.</typeparam>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	public static void SpawnFish<T>(bool v = false, int s = 0) where T : Fish {
		Log.Me(() => $"Spawning fish of type {typeof(T).Name}...", v, s + 1);
		Fish fish = (Fish) Activator.CreateInstance(typeof(T), v, s + 1)!;
		ToAdd.Add(fish);

		Log.Me(() => "Done!", v, s + 1);
	}


	/// <summary>
	/// Spawns a chum of type T at the specified position and adds it to the world.
	/// </summary>
	/// <typeparam name="T">The specific sybtype of <see cref="Chum"/> to spawn.</typeparam>
	/// <param name="startPos">The 2D vector position of where to spawn the chum.</param>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	public static void SpawnChum<T>(Vector2 startPos, bool v = false, int s = 0) where T : Chum {
		Log.Me(() => $"Spawning chum of type {typeof(T).Name}...", v, s + 1);
		Chum chum = (Chum) Activator.CreateInstance(typeof(T), startPos, v, s + 1)!;
		ToAdd.Add(chum);

		Log.Me(() => "Done!", v, s + 1);
	}


	public static void SpawnCoin<T>(Vector2 startPos, bool v = false, int s = 0) where T : Coin {
		Log.Me(() => $"Spawning coin of type {typeof(T).Name}...", v, s + 1);
		Coin coin = (Coin) Activator.CreateInstance(typeof(T), startPos, v, s + 1)!;
		ToAdd.Add(coin);

		Log.Me(() => "Done!", v, s + 1);
	}


	public static void SpawnPoop(Vector2 startPos, bool v = false, int s = 0) {
		Log.Me(() => "Spawning poop...", v, s + 1);
		Poop poop = new(startPos, v, s + 1);
		ToAdd.Add(poop);

		Log.Me(() => "Done!", v, s + 1);
	}


	/// <summary>
	/// Updates all world objects, removes any that are marked for deletion,
	/// </summary>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	public static void Update(bool v = false, int s = 0) {
		// Update existing objects
		Log.Me(() => $"Updating {WorldObjects.Count} world objects...", v, s + 1);
		foreach (WorldObject obj in WorldObjects) {
			Log.Me(() => $"Updating object of type {obj.GetType().Name}...", v, s + 1);
			obj.Update(v, s + 1);

			if (obj.ToDelete) ToRemove.Add(obj);
			if (obj is Coin coin && coin.ToRedeem) {
				Log.Me(() => $"Redeeming coin worth {coin.Value}...", v, s + 1);
				StoreManager.AddMoney(coin.Value);

				Log.Me(() => $"Added {coin.Value} to money. New total: {StoreManager.Money}.", v, s + 1);
			}
		}

		// Add new objects
		if (ToAdd.Count > 0) Log.Me(() => $"Adding {ToAdd.Count} new objects...", v, s + 1);
		foreach (WorldObject obj in ToAdd) {
			WorldObjects.Add(obj);
		}
		ToAdd.Clear();

		// Remove deleted objects
		if (ToRemove.Count > 0) Log.Me(() => $"Removing {ToRemove.Count} objects...", v, s + 1);
		foreach (WorldObject obj in ToRemove) {
			WorldObjects.Remove(obj);
		}
		ToRemove.Clear();
	}
}
