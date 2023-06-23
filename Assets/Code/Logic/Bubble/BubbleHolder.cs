using UnityEngine;

namespace Logic.Bubble
{
    public class BubbleHolder : MonoBehaviour
    {
        public Bubble GetBubble => bubble;

        private Bubble bubble;

        public void SetBubble(Bubble bubble) => this.bubble = bubble;
    }
}
