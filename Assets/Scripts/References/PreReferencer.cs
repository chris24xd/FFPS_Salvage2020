using UnityEngine;
using System.Collections;
public class PreReferencer : MonoBehaviour
{
    public static PreReferencer Instance => _instance;
    static PreReferencer _instance;

    public AudioClip[] miscClips; // 0 = Jumpscare, 1 = Shock, 2 = Jumpscare Boom, 3 = Mark
    public Sprite defaultMark;
    
    private void Awake() => _instance = this;
}
