using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class TapeManager: MonoBehaviour
{
    private AudioSource player;
    [SerializeField] AudioClip[] recordings;
    [SerializeField] private AudioClip[] sfx;
    private UnityEngine.UI.Image indicator;
    [SerializeField] private Sprite[] sprites;

    private void Awake()
    {
        player = GetComponent<AudioSource>();
        indicator = GetComponent<UnityEngine.UI.Image>();
        SalvageManager.Instance.currentRecording = -1;
    }
    private void Update()
    {
        if (SalvageManager.Instance.gameOver || SalvageManager.Instance.blackoutActive)
        {
            if(SalvageManager.Instance.isTapePlaying) Toggle();
            return;
        }
        
        if (!player.isPlaying && SalvageManager.Instance.isTapePlaying && SalvageManager.Instance.pageInactiveTimer > 10) Toggle(); //PREVENT AFK-ING
        if (!player.isPlaying && SalvageManager.Instance.isTapePlaying && SalvageManager.Instance.pageInactiveTimer < 10)
        {
            SalvageManager.Instance.currentRecording++;
            if (SalvageManager.Instance.currentRecording == 3 && RandomExtensions.Chance(50)) //50% for 750 aggression at 4th prompt
            {
                var salvagers = SalvageManager.Instance.activeAnimatronics.Where(x => x is SalvageAnimatronic).ToList();
                foreach (SalvageAnimatronic salvager in salvagers.Cast<SalvageAnimatronic>())
                {
                    salvager.Irritate4thTape();
                }
            }
            if (SalvageManager.Instance.currentRecording >= recordings.Length)
            {
                Toggle();
                SalvageManager.Win();
                return;
            }
            player.PlayOneShot(recordings[SalvageManager.Instance.currentRecording]);
        }

        if (Input.GetKeyUp(KeyCode.Space)) Toggle();
    }

    private void Toggle()
    {
        SalvageManager.Instance.isTapePlaying ^= true;

        indicator.sprite = sprites[SalvageManager.Instance.isTapePlaying ? 1 : 0];
        AudioSource.PlayClipAtPoint(sfx[SalvageManager.Instance.isTapePlaying ? 1 : 0], Camera.main.transform.position);
            
        if (SalvageManager.Instance.isTapePlaying) player.UnPause();
        else player.Pause();
    }
}
