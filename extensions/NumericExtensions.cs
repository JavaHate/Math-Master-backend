namespace JavaHateBE.Extensions
{
    public static class NumericExtensions
    {
        public static bool IsZero(this int value)
        {
            return value == 0;
        }

        public static bool IsZero(this float value)
        {
            return value == 0;
        }

        public static bool IsZero(this double value)
        {
            return value == 0;
        }
    }
}