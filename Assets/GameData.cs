using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class GameData {
	
	public List<ImageData> images;

	public static GameData CreateFromJSON(string jsonString) {
		return JsonUtility.FromJson<GameData>(jsonString);
	}

	public override string ToString() {
		return "[GameData " + "images.Count=" + images.Count + "]";
	}

}