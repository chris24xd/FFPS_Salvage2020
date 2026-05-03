using References;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Salvage
{
    public class PageCheckbox : MonoBehaviour, IPointerClickHandler
    {
        private Image _checkImage;
        private Animator _animator;

        private void Awake()
        {
            _checkImage = GetComponent<Image>();
            _animator = GetComponent<Animator>();
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _checkImage.raycastTarget = false;
            _animator.Play("X");
            AudioSource.PlayClipAtPoint(PreReferencer.Instance.miscClips[3], Camera.main.transform.position);
        }

        public void OnDisable()
        {
            if (_checkImage.raycastTarget) return;
            
            _checkImage.sprite = PreReferencer.Instance.defaultMark;
            Destroy(_animator);
        }
    }
}
