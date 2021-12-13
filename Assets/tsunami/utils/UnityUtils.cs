using System;
using System.IO;
using UnityEngine;

public class UnityUtils {
	
	public static Promise LoadImageFromDisk(string filePath) {
		Texture2D tex = null;
		byte[] fileData;

		if (File.Exists(filePath)) {
			fileData = File.ReadAllBytes(filePath);
			tex = new Texture2D(2, 2);
			tex.LoadImage(fileData);
		}
		Promise promise = new Promise((Action<object> resolve, Action<object> reject) => {
			resolve (tex);
		});
		return promise;
	}

	public static Vector2 Polar(float radius, float angle)
	{
		float x = radius * (float)Math.Cos(angle);
		float y = radius * (float)Math.Sin(angle);
		return new Vector2(x, y);
	}

	public static float RoundDecimalToPlace(float a, int decimalPlaces = 2)
	{
		float multiplier = 1;
		for (int i = 0; i < decimalPlaces; i++)
		{
			multiplier *= 10f;
		}
		return Mathf.Round(a * multiplier) / multiplier;
	}

	public static Vector2 RoundDecimalToPlace(Vector2 vec, int decimalPlaces = 2)
	{
		return new Vector3(RoundDecimalToPlace(vec.x, decimalPlaces), RoundDecimalToPlace(vec.y, decimalPlaces));
	}

	public static Vector3 RoundToDecimalPlace(Vector3 vec, int decimalPlaces = 2)
	{
		return new Vector3(RoundDecimalToPlace(vec.x, decimalPlaces), RoundDecimalToPlace(vec.y, decimalPlaces), RoundDecimalToPlace(vec.z, decimalPlaces));
	}

}
