using UnityEngine;

namespace References
{
    public class PreReferencer : MonoBehaviour
    {
        public static PreReferencer Instance { get; private set; }

        public AudioClip[] miscClips; // 0 = Jumpscare, 1 = Shock, 2 = Jumpscare Boom, 3 = Mark
        public Sprite defaultMark;
    
        private void Awake() => Instance = this;
    }
}
