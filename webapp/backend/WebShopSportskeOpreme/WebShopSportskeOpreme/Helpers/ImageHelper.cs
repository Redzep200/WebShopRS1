namespace WebShopSportskeOpreme.Helpers
{
    public static class ImageHelper
    {
        public static string ToBase64(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public static byte[] parseBase64(this string base64)
        {
            return Convert.FromBase64String(base64);
        }
    }
}
