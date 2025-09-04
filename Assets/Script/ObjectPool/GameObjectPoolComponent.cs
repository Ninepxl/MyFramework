using System;
using UnityEngine;
using Frame;
using System.Collections.Generic;

public class GameObjectPoolComponent : GameComponent
{
    // public Dictionary<GameObject, Stack<GameObject>> Pools = new Dictionary<GameObject, Stack<GameObject>>();
    public void Dispose()
    {
    }

    public GameObject Rent()
    {
        throw new NotImplementedException();
    }

    public void Return(GameObject o)
    {
    }
}