﻿/*using UnityEngine;

public struct SafeInt
{
    private int value;
    private int salt;

    public SafeInt(int value)
    {
        this.value = value;
        salt = 0; // Инициализируем соль нулем, затем установим правильное значение позже
    }

    public void InitializeSalt()
    {
        salt = Random.Range(int.MinValue / 4, int.MaxValue / 4);
        value ^= salt;
    }

    public override bool Equals(object obj)
    {
        return (int)this == (int)obj;
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return ((int)this).ToString();
    }

    public static implicit operator int(SafeInt safeInt)
    {
        return safeInt.value ^ safeInt.salt;
    }

    public static implicit operator SafeInt(int normalInt)
    {
        return new SafeInt(normalInt);
    }
}*/