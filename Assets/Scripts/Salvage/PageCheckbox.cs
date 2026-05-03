using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class PageCheckbox : MonoBehaviour, IPointerClickHandler
{
    private Image x;
    private Animator anim;

    private void Awake()
    {
        x = GetComponent<Image>();
        anim = GetComponent<Animator>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        x.raycastTarget = false;
        anim.Play("X");
        AudioSource.PlayClipAtPoint(PreReferencer.Instance.miscClips[3], Camera.main.transform.position);
    }

    public void OnDisable()
    {
        if (x.raycastTarget) return;
        x.sprite = PreReferencer.Instance.defaultMark;
        Destroy(anim);
    }
}
