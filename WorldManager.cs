using System.Numerics;

namespace ThatOnePVZMinigame;
public static class WorldManager {
	private static readonly List<WorldObject> WorldObjects = [];
	private static readonly List<WorldObject> ToRemove = [];



	/// <summary>
	/// Adds a pre-existing world object to the world.
	/// </summary>
	/// <param name="obj"></param>
	/// <param name="v"></param>
	/// <param name="s"></param>
	//public static void AddObject(WorldObject obj, bool v = false, int s = 0) {
	//	Log.Me(() => "Adding world object to world...", v, s + 1);
	//	WorldObjects.Add(obj);

	//	Log.Me(() => "Done!", v, s + 1);
	//}


	/// <summary>
	/// Spawns a fish of type T and adds it to the world.
	/// </summary>
	/// <typeparam name="T">The specific subtype of <see cref="Fish"/> to spawn.</typeparam>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	public static void SpawnFish<T>(bool v = false, int s = 0) {
		if (!typeof(T).IsSubclassOf(typeof(Fish))) {
			Log.Err(() => $"Cannot spawn fish of type {typeof(T).Name} because it does not inherit from Fish.", v, s + 1);
			return;
		}

		Log.Me(() => $"Spawning fish of type {typeof(T).Name}...", v, s + 1);
		Fish fish = (Fish) Activator.CreateInstance(typeof(T), v, s + 1)!;
		WorldObjects.Add(fish);

		Log.Me(() => "Done!", v, s + 1);
	}



	/// <summary>
	/// Spawns a chum of type T at the specified position and adds it to the world.
	/// </summary>
	/// <typeparam name="T">The specific sybtype of <see cref="Chum"/> to spawn.</typeparam>
	/// <param name="startPos">The 2D vector position of where to spawn the chum.</param>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	public static void SpawnChum<T>(Vector2 startPos, bool v = false, int s = 0) {
		if (!typeof(T).IsSubclassOf(typeof(Chum))) {
			Log.Err(() => $"Cannot spawn chum of type {typeof(T).Name} because it does not inherit from Chum.", v, s + 1);
			return;
		}

		Log.Me(() => $"Spawning chum of type {typeof(T).Name}...", v, s + 1);
		Chum chum = (Chum) Activator.CreateInstance(typeof(T), startPos, v, s + 1)!;
		WorldObjects.Add(chum);

		Log.Me(() => "Done!", v, s + 1);
	}


	/// <summary>
	/// Updates all world objects, removes any that are marked for deletion,
	/// </summary>
	/// <param name="v">Whether to print logs from this method and any methods called within it.</param>
	/// <param name="s">The stack layer this method belongs in.</param>
	public static void Update(bool v = false, int s = 0) {
		Log.Me(() => $"Updating {WorldObjects.Count} world objects...", v, s + 1);
		foreach (WorldObject obj in WorldObjects) {
			obj.Update(v, s + 1);
			if (obj.ToDelete) ToRemove.Add(obj);
		}

		if (ToRemove.Count > 0) Log.Me(() => $"Removing {ToRemove.Count} objects...", v, s + 1);
		foreach (WorldObject obj in ToRemove) {
			WorldObjects.Remove(obj);
		}
		ToRemove.Clear();
	}
}
