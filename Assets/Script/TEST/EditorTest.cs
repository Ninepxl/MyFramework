using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EditorTest : MonoBehaviour
{
    public string characterName;
    public int level;
    public float health;
    public float maxHealth;
    [AssetsOnly]
    [PreviewField(200, ObjectFieldAlignment.Center)]
    public GameObject characterModel;
    public List<string> skills;
}