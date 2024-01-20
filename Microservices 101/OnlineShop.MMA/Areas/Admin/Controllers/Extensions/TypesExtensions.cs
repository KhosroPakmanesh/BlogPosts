namespace OnlineShop.MMA.Areas.Admin.Controllers.Extensions
{
    public static class TypesExtensions
    {
        public static bool TryChangeType
            (this Type type, string stringValue, out dynamic typedValue)
        {
            try
            {
                typedValue = Convert.ChangeType(stringValue, type);
                return true;
            }
            catch { }

            typedValue = null!;
            return false;
        }
        
        public static bool IsNumeric(this Type type)
        {
            return NumericTypes.Contains(Nullable.GetUnderlyingType(type) ?? type);
        }
        private static readonly HashSet<Type> NumericTypes = new HashSet<Type>
        {
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),
            typeof(uint), typeof(float)
        };

        public static bool IsString(this Type type)
        {
            return StringTypes.Contains(Nullable.GetUnderlyingType(type) ?? type);
        }
        private static readonly HashSet<Type> StringTypes = new HashSet<Type>
        {
            typeof(string),  typeof(char),
        };
    }
}
