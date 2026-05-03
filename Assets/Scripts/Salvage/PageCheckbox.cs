using References;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using static References.PreReferencer;

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
            AudioSource.PlayClipAtPoint(PreReferencer.Instance.GetSound(Sound.Mark), Camera.main.transform.position);
        }

        public void OnDisable()
        {
            if (_checkImage.raycastTarget) return;
            
            _checkImage.sprite = PreReferencer.Instance.defaultMark;
            Destroy(_animator);
        }
    }
}
