﻿namespace Forge.Forms.Avalonia.Demo.Models;

public class ObjectPresenter
{
    public ObjectPresenter(object instance, string displayString)
    {
        Object = instance;
        DisplayString = displayString;
    }

    public object Object { get; }

    public string DisplayString { get; }

    public override string ToString()
    {
        return DisplayString;
    }
}

public class ObjectPresenter<T> : ObjectPresenter
{
    public ObjectPresenter(T instance, string displayString)
        : base(instance, displayString)
    {
        Object = instance;
    }

    public new T Object { get; }
}