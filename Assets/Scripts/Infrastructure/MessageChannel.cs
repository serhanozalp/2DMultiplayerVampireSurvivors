using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessageChannel<T> where T : struct
{
    private readonly List<Action<T>> _subscriberList = new List<Action<T>>();

    public void Publish(T message)
    {

    }
    public void Subscribe(Action<T> action)
    {

    }

    public void Unsubscribe(Action<T> action)
    {

    }
}
