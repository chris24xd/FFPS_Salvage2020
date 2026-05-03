using UnityEngine;
using UnityEngine.EventSystems;

public class PageManager: MonoBehaviour, IPointerEnterHandler
{
    private Vector3 vel;
    [SerializeField] private UnityEngine.UI.Image page;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private AudioClip[] sfx;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (SalvageManager.Instance.isPageAnimating || SalvageManager.Instance.gameOver || SalvageManager.Instance.blackoutActive) return;
        Toggle();
    }

    private void Update()
    {
        // BRING DOWN IN CASE AGGRESSION >= 1200 AND STILL VIEWING THE PAPER
        if(SalvageManager.Instance.gameOver && SalvageManager.Instance.isPageViewed) Toggle();
        
        // ANIMATE
        if (!SalvageManager.Instance.isPageAnimating) return;
        var dest = new Vector2(0, SalvageManager.Instance.isPageViewed ? 0 : -1175);
        page.transform.localPosition = Vector3.SmoothDamp(page.transform.localPosition,dest , ref vel , 0.05f, Mathf.Infinity);

        if (Vector2.Distance(page.transform.localPosition, dest) >= 0.1f) return;
        SalvageManager.Instance.isPageAnimating = false;
        for(int c = 0; c < page.transform.childCount; c++) page.transform.GetChild(c).gameObject.SetActive(true); // SHOW Xs ON CHECKBOXES
        page.sprite = sprites[0];
    }

    private void Toggle()
    {
        SalvageManager.Instance.isPageAnimating = true;
        
        for(int c = 0; c < page.transform.childCount; c++) page.transform.GetChild(c).gameObject.SetActive(false); // HIDE Xs ON CHECKBOXES
        page.sprite = sprites[1];
        AudioSource.PlayClipAtPoint(sfx[SalvageManager.Instance.isPageViewed ? 1 : 0], Camera.main.transform.position);
        SalvageManager.Instance.isPageViewed ^= true; //IMMEDIATELY SET IN ORDER FOR ANIMATRONIC TO UPDATE WHILE PAGE'S ANIMATING
    }
}
