using NTC.Global.Cache;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineAnimate : MonoCache
{
    [SerializeField] private float _speed = 2, _min = 1.5f, _max = 2;
    [SerializeField] private Vector3 _movement, _scale = new(1,1,0);

    private float t = 0;
    private Vector3 startPos, startScale;

    private void Start()
    {
        startPos = transform.localPosition;
        startScale = transform.localScale;
    }

    protected override void LateRun()
    {
        t = (t + Time.fixedDeltaTime * _speed) % (Mathf.PI * 2);
        var k = _min + (Mathf.Sin(t) / 2 + 0.5f) * (_max - _min);
        transform.localPosition = startPos + _movement * k;
        transform.localScale = new Vector3(
                                    Mathf.Lerp(startScale.x, k, _scale.x),
                                    Mathf.Lerp(startScale.y, k, _scale.y),
                                    Mathf.Lerp(startScale.z, k, _scale.z));
    }
}
