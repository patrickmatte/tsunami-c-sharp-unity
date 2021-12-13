using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class App : MonoBehaviour {

	public GameData gameData = new GameData();
	public List<Pod> pods = new List<Pod>();
	public Clock clock = new Clock();

	void Start () {
		new DelayPromise(1000).Then (init);
	}

    protected void init (object value) {
		string filePath = System.IO.Path.Combine(Application.streamingAssetsPath, "Config.json");

		Tween.CLOCK = clock;

		if (File.Exists (filePath)) {
			string dataAsJson = File.ReadAllText (filePath);
			gameData = GameData.CreateFromJSON (dataAsJson);
		}

		var podComponents = FindObjectsOfType<PodComponent>();

		foreach(PodComponent podComponent in podComponents) {
			Pod pod = new Pod (this, podComponent);
			pods.Add (pod);
		}

		pods [0].router.location = "gallery/slide-2";
		pods [1].router.location = "home";
		pods [2].router.location = "home";
		pods [3].router.location = "home";
	}

	void Update () {
		clock.tick ();
	}

}
