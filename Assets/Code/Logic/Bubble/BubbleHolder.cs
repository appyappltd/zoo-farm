using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleHolder : MonoBehaviour
{
    public Bubble GetBubble => bubble;

    private Bubble bubble;

    public void SetBubble(Bubble bubble) => this.bubble = bubble;
}
