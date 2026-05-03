using References;
using UnityEngine;

using static References.PreReferencer;

namespace Salvage
{
    public class TapeManager: MonoBehaviour
    {
        private const float MaxAfkTime = 10;

        private enum TapeState
        {
            PressToUnpause,
            PressToPause
        }
        
        private AudioSource _audio;
        [SerializeField] AudioClip[] recordings;
        private UnityEngine.UI.Image indicator;
        [SerializeField] private Sprite[] sprites;
        
        private static bool MetChanceToIrritateAt4thTape => 
            SalvageManager.Instance.currentRecording == 3 && RandomExtensions.Chance(50);

        private void Awake()
        {
            _audio = GetComponent<AudioSource>();
            indicator = GetComponent<UnityEngine.UI.Image>();
            SalvageManager.Instance.currentRecording = -1;
        }
        
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space)) Toggle();
            
            if (SalvageManager.Instance.isTapePlaying &&
                (SalvageManager.Instance.gameOver || SalvageManager.Instance.blackoutActive))
            {
                Toggle();
                return;
            }

            var tapeHasStopped = !_audio.isPlaying && SalvageManager.Instance.isTapePlaying;
            if (!tapeHasStopped) return;
            
            if (SalvageManager.Instance.pageInactiveTimer > MaxAfkTime)
            {
                Toggle();
                return;
            }
            
            ProgressToNextRecording();
        }

        private void ProgressToNextRecording()
        {
            SalvageManager.Instance.currentRecording++;
            
            if (MetChanceToIrritateAt4thTape)
            {
                foreach (SalvageAnimatronic salvager in SalvageManager.Instance.SalvageAnimatronics)
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
            _audio.PlayOneShot(recordings[SalvageManager.Instance.currentRecording]);
        }
        
        private void Toggle()
        {
            SalvageManager.Instance.isTapePlaying ^= true;

            var tapeState = !SalvageManager.Instance.isTapePlaying ? TapeState.PressToUnpause : TapeState.PressToPause;
            var tapeSound = !SalvageManager.Instance.isTapePlaying ? Sound.TapePause : Sound.TapePlay;
            
            indicator.sprite = sprites[(int)tapeState];
            AudioSource.PlayClipAtPoint(PreReferencer.Instance.GetSound(tapeSound), Camera.main.transform.position);
            
            if (SalvageManager.Instance.isTapePlaying) _audio.UnPause();
            else _audio.Pause();
        }
    }
}
