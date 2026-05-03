using UnityEngine;

namespace Animatronic_Basics
{
    [CreateAssetMenu(fileName = "Animatronic_AI", menuName = "New Animatronic")]
    public class AnimatronicAI : ScriptableObject
    {
        [Range(0,20)]
        public int ai;

        public new string name;
        [Multiline]
        public string description;
    }
}
