using System;
using UnityEngine;

namespace HachiFramework
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class HachiButton : Attribute
    {
        public string buttonName;
        public float height;
    }
}