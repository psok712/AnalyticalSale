namespace DataAccess;

public static class ExtensionStringMethods
{
    public static string SettingSeparatorOs(this string str)
    {
        return str.Replace(@"\", Path.DirectorySeparatorChar.ToString());
    }
}