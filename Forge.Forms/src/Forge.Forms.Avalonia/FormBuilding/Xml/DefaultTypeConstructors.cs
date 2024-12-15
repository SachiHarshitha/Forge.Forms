using System;
using Forge.Forms.AvaloniaUI.Annotations;

namespace Forge.Forms.AvaloniaUI.FormBuilding.Xml;

internal static class DefaultTypeConstructors
{
    public static TypeConstructor NullableTime(TypeConstructionContext context)
    {
        return Time(context, true);
    }

    public static TypeConstructor Time(TypeConstructionContext context)
    {
        return Time(context, false);
    }

    private static TypeConstructor Time(TypeConstructionContext context, bool nullable)
    {
        object is24Hours = false;
        if (context is XmlConstructionContext xmlContext)
        {
            var e = xmlContext.Element;
            is24Hours = e.TryGetAttribute("is24hours");
        }

        return new TypeConstructor(
            nullable ? typeof(DateTimeOffset?) : typeof(DateTimeOffset),
            new TimeAttribute
            {
                Is24Hours = is24Hours
            });
    }
}