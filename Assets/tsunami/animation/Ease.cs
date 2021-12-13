using System;

public delegate float TweenEase(float t, float b, float c, float d);

public class Ease {

	public static Back back = new Back ();
	public static Bounce bounce = new Bounce ();
	public static Circular circular = new Circular ();
	public static Cubic cubic = new Cubic ();
	public static Elastic elastic = new Elastic ();
	public static Exponential exponential = new Exponential ();
	public static Linear linear = new Linear ();
	public static Quadratic quadratic = new Quadratic ();
	public static Quartic quartic = new Quartic ();
	public static Quintic quintic = new Quintic ();
	public static Sine sine = new Sine ();

}

public class Back {

	public float springiness;

	public Back(float springiness = 1.70158f) {
		this.springiness = springiness;
	}

	public float InOut (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		float s = springiness;
		if ((t /= d / 2) < 1)
			return c / 2 * (t * t * (((s *= (1.525f)) + 1) * t - s)) + b;
		return c / 2 * ((t -= 2) * t * (((s *= (1.525f)) + 1) * t + s) + 2) + b;
	}

	public float In (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		float s = springiness;
		return c * (t /= d) * t * ((s + 1) * t - s) + b;
	}

	public float Out (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		float s = springiness;
		return c * ((t = t / d - 1) * t * ((s + 1) * t + s) + 1) + b;
	}

}

public class Bounce {

	public float In (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return c - Out (d - t, 0, c, d) + b;
	}

	public float Out (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		if ((t /= d) < (1 / 2.75f))
			return c * (7.5625f * t * t) + b;
		else if (t < (2 / 2.75f))
			return c * (7.5625f * (t -= (1.5f / 2.75f)) * t + 0.75f) + b;
		else if (t < (2.5f / 2.75f))
			return c * (7.5625f * (t -= (2.25f / 2.75f)) * t + 0.9375f) + b;
		else
			return c * (7.5625f * (t -= (2.625f / 2.75f)) * t + 0.984375f) + b;
	}

	public float InOut (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		if (t < d / 2)
			return In (t * 2, 0, c, d) * 0.5f + b;
		else
			return Out (t * 2 - d, 0, c, d) * 0.5f + c * 0.5f + b;
	}

}

public class Circular {

	public float In (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return -c * ((float)Math.Sqrt (1 - (t /= d) * t) - 1) + b;
	}

	public float Out (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return c * (float)Math.Sqrt (1 - (t = t / d - 1) * t) + b;
	}

	public float InOut (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		if ((t /= d / 2) < 1)
			return -c / 2 * ((float)Math.Sqrt (1 - t * t) - 1) + b;
		return c / 2 * ((float)Math.Sqrt (1 - (t -= 2) * t) + 1) + b;
	}

}

public class Cubic {

	public float In (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return c * (t /= d) * t * t + b;
	}

	public float Out (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return c * ((t = t / d - 1) * t * t + 1) + b;
	}

	public float InOut (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		if ((t /= d / 2) < 1) {
			return c / 2 * t * t * t + b;
		}
		return c / 2 * ((t -= 2) * t * t + 2) + b;
	}

}

public class Elastic {

	public float In (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		if (t == 0)
			return b;
		if ((t /= d) == 1)
			return b + c;
		float p = d * 0.3f;

		float a = c;
		float s = p / 4;

		return -(a * (float)Math.Pow (2, 10 * (t -= 1)) * (float)Math.Sin ((t * d - s) * (2 * Math.PI) / p)) + b;
	}

	public float Out (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		if (t == 0)
			return b;
		if ((t /= d) == 1)
			return b + c;
		float p = d * 0.3f;
		float a = c;
		float s = p / 4;
		return a * (float)Math.Pow (2, -10 * t) * (float)Math.Sin ((t * d - s) * (2 * Math.PI) / p) + c + b;
	}

	public float InOut (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		if (t == 0)
			return b;
		if ((t /= d / 2) == 2)
			return b + c;
		float p = d * (0.3f * 1.5f);
		float a = c;
		float s = p / 4;
		if (t < 1) {
			return -0.5f * (a * (float)Math.Pow (2, 10 * (t -= 1)) * (float)Math.Sin ((t * d - s) * (2 * Math.PI) / p)) + b;
		}
		return a * (float)Math.Pow (2, -10 * (t -= 1)) * (float)Math.Sin ((t * d - s) * (2 * Math.PI) / p) * 0.5f + c + b;
	}

}

public class Exponential {

	public float In (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return t == 0 ? b : c * (float)Math.Pow (2, 10 * (t / d - 1)) + b;
	}

	public float Out (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return t == d ? b + c : c * (-(float)Math.Pow (2, -10 * t / d) + 1) + b;
	}

	public float InOut (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		if (t == 0)
			return b;

		if (t == d)
			return b + c;

		if ((t /= d / 2) < 1)
			return c / 2 * (float)Math.Pow (2, 10 * (t - 1)) + b;

		return c / 2 * (-(float)Math.Pow (2, -10 * --t) + 2) + b;
	}

}

public class Linear {

	public float InOut (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return c * t / d + b;
	}

}

public class Quadratic {

	public float In (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return c * (t /= d) * t + b;
	}

	public float Out (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return -c * (t /= d) * (t - 2) + b;
	}

	public float InOut (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		if ((t /= d / 2) < 1)
			return c / 2 * t * t + b;

		return -c / 2 * ((--t) * (t - 2) - 1) + b;
	}

}

public class Quartic {

	public float In (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return c * (t /= d) * t * t * t + b;
	}

	public float Out (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return -c * ((t = t / d - 1) * t * t * t - 1) + b;
	}

	public float InOut (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		if ((t /= d / 2) < 1)
			return c / 2 * t * t * t * t + b;

		return -c / 2 * ((t -= 2) * t * t * t - 2) + b;
	}

}

public class Quintic {

	public float In (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return c * (t /= d) * t * t * t * t + b;
	}

	public float Out (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return c * ((t = t / d - 1) * t * t * t * t + 1) + b;
	}

	public float InOut (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		if ((t /= d / 2) < 1)
			return c / 2 * t * t * t * t * t + b;

		return c / 2 * ((t -= 2) * t * t * t * t + 2) + b;
	}

}

public class Sine {

	public float In (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return -c * (float)Math.Cos (t / d * (Math.PI / 2)) + c + b;
	}

	public float Out (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return c * (float)Math.Sin (t / d * (Math.PI / 2)) + b;
	}

	public float InOut (float t, float b = 0f, float c = 1f, float d = 1f)
	{
		return -c / 2 * ((float)Math.Cos (Math.PI * t / d) - 1) + b;
	}

}
