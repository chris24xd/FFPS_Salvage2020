using UnityEngine;

[CreateAssetMenu(fileName = "Animatronic_AI", menuName = "New Animatronic")]
public class Animatronic_AI : ScriptableObject
{
    [Range(0,20)]
    public int ai;

    public new string name;
    [Multiline]
    public string description;
}
