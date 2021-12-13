// This code is adapted from https://newbedev.com/c-adding-two-generic-values and https://github.com/Jewelots/Betwixt

using System;
using System.Linq.Expressions;

public class GenericMath
{

    public static Func<T, T, T> Add<T>(T a)
    {
        ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
        ParameterExpression paramB = Expression.Parameter(typeof(T), "b");
        BinaryExpression body = Expression.Add(paramA, paramB);
        return Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
    }

    public static Func<T, T, T> Subtract<T>(T a)
    {
        ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
        ParameterExpression paramB = Expression.Parameter(typeof(T), "b");
        BinaryExpression body = Expression.Subtract(paramA, paramB);
        return Expression.Lambda<Func<T, T, T>>(body, paramA, paramB).Compile();
    }

    public static Func<T, float, T> Multiply<T>(T a)
    {
        ParameterExpression paramA = Expression.Parameter(typeof(T), "a");
        ParameterExpression paramB = Expression.Parameter(typeof(float), "b");
        BinaryExpression body = Expression.Multiply(paramA, paramB);
        return Expression.Lambda<Func<T, float, T>>(body, paramA, paramB).Compile();
    }
}