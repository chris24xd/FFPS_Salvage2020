using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Animatronic_Basics;
using References;
using TMPro;
using UnityEngine;

namespace Salvage
{
    public class SalvageManager : MonoBehaviour
    {
        public static SalvageManager Instance => _instance;
        private static SalvageManager _instance;

        [SerializeField] private SalvageAnimatronic[] salvagersToSpawn;
    
        public Animatronic[] activeAnimatronics;
        public bool gameOver;
        private float _timePassed;

        [HideInInspector]
        public bool isPageViewed;
        [HideInInspector]
        public bool isPageAnimating;
        [HideInInspector]
        public float pageInactiveTimer;
        [HideInInspector]
        public float pageViewingTimer;
        private float _pageViewingInterval;
        [HideInInspector]
        public bool isTapePlaying;
        [HideInInspector]
        public float tapePlayingTimer;
        private float _tapePlayingInterval;
        [HideInInspector]
        public int currentRecording;

        [SerializeField]
        private TMP_Text stopwatchText;
        [SerializeField]
        private TMP_Text tapeStopwatchText;

        [SerializeField]
        private UnityEngine.UI.Image shockImage;
        private const int MaxTases = 5;
        private int _tases;
        private const float TaseCooldown = 10;
        private float _taseCooldown;

        [SerializeField]
        private UnityEngine.UI.Image lightImage;
        private float flickerInterval;
        private float FlickerEvery;
    
        [SerializeField]
        private UnityEngine.UI.Image blackoutImage;
        [HideInInspector]
        public bool blackoutActive;
    
        private bool CanTase => _tases < MaxTases && _taseCooldown >= TaseCooldown;

        private static bool HasTapeProgressedEnough =>
            _instance.currentRecording < 5 && _instance._tapePlayingInterval >= 10;
        private static bool IsPlayerExaminingPage => _instance._pageViewingInterval >= 1;
        private void Awake()
        {
            _instance = this;
        
            // SPAWN RANDOM SALVAGE ANIMATRONIC
            Instantiate(salvagersToSpawn[Random.Range(0, salvagersToSpawn.Length)]);
        
            activeAnimatronics = FindObjectsByType<Animatronic>(FindObjectsSortMode.None);

            _timePassed = 0;
            isPageViewed = false;
            isPageAnimating = false;
            isTapePlaying = true;

            _tases = 0;
            _taseCooldown = 0f;

            FlickerEvery = Random.Range(0.01f, 0.5f);
            blackoutActive = false;
        }

        public static int GetStartingAggression(int max = 3)
        {
            var nums = Enumerable.Range(0, max + 1).ToList();
            return nums[Random.Range(0, nums.Count)] * 100;
        }
    
        private void Update()
        {
            if (gameOver) return;
        
            var animsReadyToScare = activeAnimatronics.Where(x => x.CanJumpscare()).ToList();
            if (animsReadyToScare.Count > 0) GameOver(animsReadyToScare[Random.Range(0, animsReadyToScare.Count)]);

            _timePassed += Time.deltaTime;
            var span = System.TimeSpan.FromSeconds(_timePassed);
            stopwatchText.text = span.ToString(@"mm\:ss\.ff");
        
            // LIGHT FLICKER
            if (RandomExtensions.Chance(50)) flickerInterval += Time.deltaTime;
            if(flickerInterval >= FlickerEvery)
            {
                lightImage.color= new Color32(0,0,0,(byte)Random.Range(60, 211));
                FlickerEvery = Random.Range(0.01f, 0.5f);
            }

            // UPDATE TAPE TIMER
            if (isTapePlaying)
            {
                tapePlayingTimer += Time.deltaTime;
                _tapePlayingInterval += Time.deltaTime;

                // TO BE DELETED.
                var span2 = System.TimeSpan.FromSeconds(tapePlayingTimer);
                tapeStopwatchText.text = span2.ToString(@"mm\:ss\.ff");
            }
        
            // UPDATE PAGE TIMERS
            if (!isPageViewed)
            {
                pageViewingTimer = _pageViewingInterval = 0;
                pageInactiveTimer += Time.deltaTime;
            }
            else
            {
                pageViewingTimer += Time.deltaTime;
                _pageViewingInterval += Time.deltaTime;
                pageInactiveTimer = 0;
            }
        
            // IRRITATION
            var salvagers = activeAnimatronics.Where(x => x is SalvageAnimatronic).ToList();
            if (HasTapeProgressedEnough && RandomExtensions.Chance(50))
            {
                _tapePlayingInterval = 0;
                foreach (SalvageAnimatronic salvager in salvagers.Cast<SalvageAnimatronic>())
                {
                    salvager.IrritateTape();
                }
            }

            if (IsPlayerExaminingPage)
            {
                _pageViewingInterval = 0;
                foreach (SalvageAnimatronic salvager in salvagers.Cast<SalvageAnimatronic>())
                {
                    salvager.IrritatePage();
                }
            }

            // TASING MANAGEMENT
            if (_tases < MaxTases) _taseCooldown += Time.deltaTime;
            shockImage.enabled = CanTase;
            if (!CanTase || (CanTase && !(Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.RightControl)))) return;
            
            var taseables = activeAnimatronics.Where(x => x is ITaseable).ToList();
            if (taseables.Count == 0) return;

            _tases++;
            _taseCooldown = 0;
            blackoutActive = true;
            AudioSource.PlayClipAtPoint(PreReferencer.Instance.miscClips[1], Camera.main.transform.position);
            StartCoroutine(TasingPhase(taseables));
        }

        private IEnumerator TasingPhase(List<Animatronic> animatronicsToTase)
        {
            while (((Color32)blackoutImage.color).a < 255)
            {
                blackoutImage.color += new Color32(0, 0, 0, (byte)Random.Range(5,11));
                yield return null;
            }
        
            foreach (ITaseable taseable in animatronicsToTase.Cast<ITaseable>())
            {
                taseable.Tase();
            
                // REDUCE REWARD FOR SALVAGE ANIMATRONIC AFTER BEING DAMAGED
                if (taseable is SalvageAnimatronic animatronic && _tases > 3)
                {
                    animatronic.points = _tases == 4 ? animatronic.points / 2 : 100;
                }
            }
            yield return new WaitForSeconds(0.5f);
        
            while (((Color32)blackoutImage.color).a > 0)
            {
                blackoutImage.color -= new Color32(0, 0, 0, (byte)Random.Range(1,4));
                yield return null;
            }
        
            blackoutActive = false;
        }
    
        private void GameOver(Animatronic animatronic)
        {
            gameOver = true;
            animatronic.Jumpscare();
        }

        public static void Win()
        {
        
        }
    }
}
