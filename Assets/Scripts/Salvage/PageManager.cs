using References;
using UnityEngine;
using UnityEngine.EventSystems;

using static References.PreReferencer;

namespace Salvage
{
    public class PageManager: MonoBehaviour, IPointerEnterHandler
    {
        private Vector3 _vel;
        [SerializeField] private UnityEngine.UI.Image page;
        [SerializeField] private Sprite[] sprites;

        private enum PageState
        {
            Still,
            Moving,
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (SalvageManager.Instance.isPageAnimating || SalvageManager.Instance.gameOver ||
                SalvageManager.Instance.blackoutActive)
            {
                return;
            }
            
            Pull();
        }

        private void Pull()
        {
            SalvageManager.Instance.isPageAnimating = true;
        
            ToggleMarks(false);
            page.sprite = sprites[(int)PageState.Moving];
            
            var pageSound = SalvageManager.Instance.isPageViewed ? Sound.PaperPullDown : Sound.PaperPullUp;
            AudioSource.PlayClipAtPoint(PreReferencer.Instance.GetSound(pageSound), Camera.main.transform.position);
            
            SalvageManager.Instance.isPageViewed ^= true; //IMMEDIATELY SET IN ORDER FOR ANIMATRONIC TO UPDATE WHILE PAGE'S ANIMATING
        }

        private void ToggleMarks(bool toggle)
        {
            for(int c = 0; c < page.transform.childCount; c++)
            {
                page.transform.GetChild(c).gameObject.SetActive(toggle);
            }
        }

        private void Update()
        {
            // BRING DOWN IN CASE AGGRESSION >= 1200 AND STILL VIEWING THE PAPER
            if (SalvageManager.Instance.gameOver && SalvageManager.Instance.isPageViewed)
            {
                Pull();
            }
        
            if (!SalvageManager.Instance.isPageAnimating) return;
            
            // ANIMATE
            var destination = new Vector2(0, SalvageManager.Instance.isPageViewed ? 0 : -1175);
            page.transform.localPosition = 
                Vector3.SmoothDamp(page.transform.localPosition,destination , ref _vel , 0.05f, Mathf.Infinity);

            var reachedDestination = Vector2.Distance(page.transform.localPosition, destination) < 0.1f;
            if (!reachedDestination) return;
            
            SalvageManager.Instance.isPageAnimating = false;
            ToggleMarks(true);
            page.sprite = sprites[(int)PageState.Still];
        }
    }
}
