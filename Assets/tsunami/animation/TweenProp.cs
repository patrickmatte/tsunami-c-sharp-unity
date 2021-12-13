using System;
using System.Reflection;

public delegate T LerpFunc<T>(T a, T b, float t);
public delegate T ModifierFunc<T>(T a);

public abstract class TweenProp
{
	public abstract void update(float time);
}


public class TweenProp<T> : TweenProp
{

	public object target;
	public string name;
	public T startValue;
	public T endValue;
	public TweenEase ease;
	public LerpFunc<T> lerp;
	public ModifierFunc<T> modifier;

	protected T _Value;
	protected MemberInfo memberInfo;

	protected Func<T, T, T> Add;
	protected Func<T, T, T> Subtract;
	protected Func<T, float, T> Multiply;

	public TweenProp(object target, string name, T startValue, T endValue, TweenEase ease = null, LerpFunc<T> lerp = null, ModifierFunc<T> modifier = null)
	{
		this.target = target;
		Type type = target.GetType();
		MemberInfo[] myMemberInfo = type.GetMember(name);

		for (int i = 0; i < myMemberInfo.Length; i++)
		{
			switch (myMemberInfo[i].MemberType.ToString())
            {
				case "Method":
					memberInfo = type.GetMethod(name);
					break;
				case "Field":
					memberInfo = type.GetField(name);
					break;
				case "Property":
					memberInfo = type.GetProperty(name);
					break;
			}
		}

		this.startValue = startValue;
		this.endValue = endValue;
		this.ease = ease ?? Ease.linear.InOut;
		if (lerp != null)
		{
			this.lerp = lerp;
		}
		else
		{
			this.lerp = DefaultLerp;
			Add = GenericMath.Add(startValue);
			Subtract = GenericMath.Subtract(startValue);
			Multiply = GenericMath.Multiply(startValue);
		}
		this.modifier = modifier ?? DefaultModifier;
	}

	public T Value
	{
		get
		{
			return _Value;
		}

	}

	protected T DefaultLerp(T a, T b, float t)
	{
		return Add(a, Multiply(Subtract(b, a), t));
	}

	protected T DefaultModifier(T a)
	{
		return a;
	}

	public override void update(float time)
	{
		float delta = ease(time, 0f, 1f, 1);
		T value = lerp(startValue, endValue, delta);
		_Value = modifier(value);
		SetValue(_Value);
	}

	protected virtual void SetValue(T value)
	{
		switch (memberInfo.MemberType.ToString())
		{
			case "Method":
				MethodInfo methodInfo = (MethodInfo)memberInfo;
				object[] stringMethodParams = new object[] { value };
				methodInfo.Invoke(target, stringMethodParams);
				break;
			case "Field":
				FieldInfo fieldInfo = (FieldInfo)memberInfo;
				fieldInfo.SetValue(target, value);
				break;
			case "Property":
				PropertyInfo propertyInfo = (PropertyInfo)memberInfo;
				propertyInfo.SetValue(target, value);
				break;
		}

	}

}

//public class TweenField<T> : TweenProp<T>
//{

//	FieldInfo fieldInfo;

//	public TweenField(object target, string name, T startValue, T endValue, TweenEase ease, LerpFunc<T> lerp = null, ModifierFunc<T> modifier = null) : base(target, name, startValue, endValue, ease, lerp, modifier)
//	{
//		fieldInfo = type.GetField(name);
//	}

//	protected override void SetValue(T value)
//	{
//		fieldInfo.SetValue(target, value);
//	}

//}

//public class TweenProperty<T> : TweenProp<T>
//{

//	PropertyInfo propertyInfo;

//	public TweenProperty(object target, string name, T startValue, T endValue, TweenEase ease, LerpFunc<T> lerp = null, ModifierFunc<T> modifier = null) : base(target, name, startValue, endValue, ease, lerp, modifier)
//	{
//		propertyInfo = type.GetProperty(name);
//	}

//	protected override void SetValue(T value)
//	{
//		propertyInfo.SetValue(target, value);
//	}

//}


//public class TweenMethod<T> : TweenProp<T>
//{

//	MethodInfo methodInfo;

//	public TweenMethod(object target, string name, T startValue, T endValue, TweenEase ease, LerpFunc<T> lerp = null, ModifierFunc<T> modifier = null) : base(target, name, startValue, endValue, ease, lerp, modifier)
//	{
//		methodInfo = type.GetMethod(name);
//	}

//	protected override void SetValue(T value)
//	{
//		object[] stringMethodParams = new object[] { value };
//		methodInfo.Invoke(target, stringMethodParams);
//	}

//}