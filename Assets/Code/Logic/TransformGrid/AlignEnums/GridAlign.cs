namespace Logic.TransformGrid
{
    public enum GridAlign : byte
    {
        TopLeft = VerticalAlign.Top + HorizontalAlign.Left,
        Top = VerticalAlign.Top + HorizontalAlign.Center,
        TopRight = VerticalAlign.Top + HorizontalAlign.Right,
        CenterLeft = VerticalAlign.Center + HorizontalAlign.Left,
        Center = VerticalAlign.Center + HorizontalAlign.Center,
        CenterRight = VerticalAlign.Center + HorizontalAlign.Right,
        BottomLeft = VerticalAlign.Bottom + HorizontalAlign.Left,
        Bottom = VerticalAlign.Bottom + HorizontalAlign.Center,
        BottomRight = VerticalAlign.Bottom + HorizontalAlign.Right,
    }
}