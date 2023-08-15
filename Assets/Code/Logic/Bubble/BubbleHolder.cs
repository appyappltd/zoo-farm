using UnityEngine;

namespace Logic.Bubble
{
    public class BubbleHolder : MonoBehaviour
    {
        public MovingBubble GetMovingBubble => _movingBubble;

        private MovingBubble _movingBubble;

        public void SetBubble(MovingBubble movingBubble) => this._movingBubble = movingBubble;
    }
}
