using System.Globalization;

namespace Forge.Forms.AvaloniaUI.DynamicExpressions;

internal static class InvariantInt
{
    private const NumberStyles InvariantNumberStyles =
        NumberStyles.AllowLeadingWhite
        | NumberStyles.AllowTrailingWhite
        | NumberStyles.AllowLeadingSign;

    public static bool TryParse(string s, out int result)
    {
        return int.TryParse(s, InvariantNumberStyles, CultureInfo.InvariantCulture, out result);
    }
}