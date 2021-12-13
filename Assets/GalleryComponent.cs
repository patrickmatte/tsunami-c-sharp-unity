using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalleryComponent : MonoBehaviour {

	public Action<int> selectSlideCallback;
	public Action<int> deselectSlideCallback;
	public GameObject SlidePrefab;
	public List<SlideComponent> slides;

	void Start () {
	}

	public void setTextures(List<Texture2D> textures) {
		slides = new List<SlideComponent> ();

		float slideSize = 5f;
		float x = 0f;
		float y = 0f;

		for(int i = 0; i < textures.Count; i++) {
			Texture2D texture = textures [i];
			GameObject slide = Instantiate (SlidePrefab, gameObject.transform);
			SlideComponent slideComponent = slide.GetComponent<SlideComponent>();
			slideComponent.index = i;
			slideComponent.selectCallback = selectHandler;
			slideComponent.deselectCallback = deselectHandler;
			slideComponent.init ();
			slideComponent.setTexture (texture);
			slides.Add (slideComponent);
			slideComponent.setPosition (new Vector3 (x - 11.5f + slideSize * 0.5f, -y, 20f));
			x += slideSize + 1f;
			if (x > 23) {
				x = 0;
				y = slideSize + 1f;
			}
		}

		y = slideSize + 1f;
		gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, y * 0.5f, 0f);
	}

	protected void selectHandler(int index) {
		selectSlideCallback(index);
	}

	protected void deselectHandler(int index) {
		deselectSlideCallback(index);
	}

	public Promise show() {
		Timeline timeline = new Timeline ();
		foreach(SlideComponent slideComponent in slides) {
			timeline.Add (slideComponent.show());
		}
		return timeline.Start();
	}

	public Promise hide() {
		Timeline timeline = new Timeline ();
		foreach (SlideComponent slideComponent in slides) {
			timeline.Add (slideComponent.hide ());
		}
		return timeline.Start().Then(hideComplete);
	}

	protected void hideComplete(object value) {
		foreach (SlideComponent slideComponent in slides) {
			GameObject slide = slideComponent.gameObject;
			Destroy (slide);
		}
	}

	void Update () {
		
	}
}
