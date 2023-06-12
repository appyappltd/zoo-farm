using NTC.Global.Cache;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bubble : MonoCache
{
    [SerializeField] private Transform _parent;
    [SerializeField] private SpriteRenderer _state;

    [SerializeField] private Vector3 offset;

    private void Awake()
    {
        GetComponentInParent<BubbleHolder>().SetBubble(this);

        offset = transform.localPosition;

        transform.parent = null;
        transform.LookAt(Camera.main.transform);
    }

    protected override void LateRun()
    {
        if (!_parent)
            Destroy(gameObject);
        else
            transform.position = _parent.position + offset;
    }

    public void ChangeState(Sprite sprite) => _state.sprite = sprite;
}
