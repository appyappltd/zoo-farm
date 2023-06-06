using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineAnimate : MonoBehaviour
{
    [SerializeField] private float _speed, _min, _max;
    [SerializeField] private Vector3 _movement, _scale;

    private float t = 0;
    private Vector3 startPos, startScale;

    private void Start()
    {
        startPos = transform.localPosition;
        startScale = transform.localScale;
    }

    private void Update()
    {
        t = (t + Time.deltaTime * _speed) % (Mathf.PI * 2);
        var k = _min + (Mathf.Sin(t) / 2 + 0.5f) * (_max - _min);
        transform.localPosition = startPos + _movement * k;
        transform.localScale = new Vector3(
                                    Mathf.Lerp(startScale.x, k, _scale.x),
                                    Mathf.Lerp(startScale.y, k, _scale.y),
                                    Mathf.Lerp(startScale.z, k, _scale.z));
    }
}
