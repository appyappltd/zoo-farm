using Logic.Translators;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalLinearPositionTranslatable : LinearPositionTranslatable
{
    protected override void ApplyTranslation(Vector3 value)
    {
        transform.localPosition = value;
    }
}