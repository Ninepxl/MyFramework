using System;
using UnityEngine;
namespace HachiFramework
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class FindUIComponentAttribute : Attribute 
    {
        public string Path;
        public FindUIComponentAttribute(string path = null)
        {
            Path = path;
        }
    }    
}