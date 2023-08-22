using Logic.TransformGrid;

namespace Tools.Extension
{
    public static class EnumExtensions
    {
        public static HorizontalAlign IsolateHorizontal(this GridAlign align)
        {
            return (HorizontalAlign) ((int) align % 10);
        }
        
        public static VerticalAlign IsolateVertical(this GridAlign align)
        {
            return (VerticalAlign) ((int) align / 10 * 10);
        }
    }
}