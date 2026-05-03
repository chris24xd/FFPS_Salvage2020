using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SalvageAnimatronic: Animatronic, ITaseable
{
    private Animator _animator;
    private AudioSource _audibleWarning;
    [SerializeField]
    private float[] audibleWarningVolumes;
    [SerializeField]
    private int _aggression;
    [SerializeField]
    private int _stage;

    private int Night => Mathf.FloorToInt(AI / 5f);
    
    public void Tase()
    {
        _stage = 0;
        _animator.Play(_stage.ToString());
        _aggression = SalvageManager.GetStartingAggression();
    }
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
        _audibleWarning = GetComponentInChildren<AudioSource>();
        // STARTING PROGRESS
        _stage = 0;
        _aggression = SalvageManager.GetStartingAggression(1);
    }
    
    private void Update()
    {
        // STAGE ADVANCING (VISUAL ONLY)
        if (!SalvageManager.Instance.isPageAnimating && SalvageManager.Instance.pageViewingTimer > 0 && (
                (_stage == 0 && _aggression >= 250) ||
                (_stage == 1 && _aggression >= 500)
            ))
        {
            _stage++;
            _animator.Play(_stage.ToString());
        }
        
        // AUDIBLE WARNING
        if (_aggression < 750) _audibleWarning.volume = audibleWarningVolumes[0];
        else if (_aggression >= 750 && _aggression < 900) _audibleWarning.volume = audibleWarningVolumes[1];
        else if(_aggression >= 900) _audibleWarning.volume = audibleWarningVolumes[2];
    }
    
    public void IrritateTape() => _aggression += 150 + 50 * Night;
    
    public void IrritatePage() => _aggression += 10 + 10 * Night;

    public void Irritate4thTape()
    {
        if (_aggression >= 750) return;
        _aggression = 750;
    }
    
    public override bool CanJumpscare() => _aggression >= 1200
                                           || (_aggression >= 1000 &&
                                               !SalvageManager.Instance.isPageViewed &&
                                               SalvageManager.Instance.isPageAnimating);

    public override void Jumpscare()
    {
        AudioSource.PlayClipAtPoint(PreReferencer.Instance.miscClips[2], Camera.main.transform.position);
        AudioSource.PlayClipAtPoint(PreReferencer.Instance.miscClips[0], Camera.main.transform.position);
        _animator.Play("JUMP");
    }
}
