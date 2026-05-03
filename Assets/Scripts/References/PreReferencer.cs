using System;
using UnityEngine;

namespace References
{
    public class PreReferencer : MonoBehaviour
    {
        public static PreReferencer Instance { get; private set; }

        public enum Sound
        {
            Jumpscare,
            Shock,
            JumpscareBoom,
            Mark,
            PaperPullUp,
            PaperPullDown,
            TapePlay,
            TapePause,
        }
        
        [SerializeField] private AudioClip[] miscClips;
        public Sprite defaultMark;

        public AudioClip GetSound(Sound sound)
        {
            var index = (int)sound;
            if (index < 0 || index >= miscClips.Length)
            {
                throw new IndexOutOfRangeException($"{sound} is out of range!");
            }
            
            return miscClips[index];
        }
    
        private void Awake() => Instance = this;
    }
}
