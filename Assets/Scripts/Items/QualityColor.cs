using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum Quality
{
    Basic,
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
    Ultimatum
}

public static class QualityColor
{
    private static Dictionary<Quality, string> colors = new Dictionary<Quality, string>()
    {
        {Quality.Basic, "#d9d9d9" }, // l gray
        {Quality.Common, "#949494" }, // gray
        {Quality.Uncommon, "#79a86f" }, // l green
        {Quality.Rare, "#2dff00" }, // green
        {Quality.Epic, "#eaff00" }, // yellow
        {Quality.Legendary, "#0091ff" }, // blue
        {Quality.Ultimatum, "#8000ff" }, // purple

    };

    public static Dictionary<Quality, string> Colors { get => colors;  }
}