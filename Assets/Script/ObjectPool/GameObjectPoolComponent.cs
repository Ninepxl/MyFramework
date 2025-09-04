using System;
using UnityEngine;
using Frame;

public class GameObjectPoolComponent : GameComponent
{
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