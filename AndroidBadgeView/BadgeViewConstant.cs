using Android.Graphics;

namespace AndroidAdapter
{
    public class BadgeViewDefaultValue
    {
        public const int DEFAULT_MARGIN_DIP = 5;
        public const int DEFAULT_LR_PADDING_DIP = 5;
        public const int DEFAULT_CORNER_RADIUS_DIP = 8;
        public const BadgeViewPosition DEFAULT_POSITION = BadgeViewPosition.TopRight;
        public static Color DEFAULT_BADGE_COLOR = Color.ParseColor("#CCFF0000");    //Color.RED;
        public static Color DEFAULT_TEXT_COLOR = Color.White;
    }

    public enum BadgeViewPosition
    {
        TopLeft = 1,
        TopRight = 2,
        ButtomLeft = 3,
        ButtomRight = 4,
        Center = 5,
        Top = 6,
        Buttom = 7,
        Left = 8,
        Right = 9
    }
}