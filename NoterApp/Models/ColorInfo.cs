using System.Collections.Generic;
using System.Runtime.InteropServices.Marshalling;

namespace NoterApp.Models;

public class ColorInfo
{
    public string name { get; set; }
    public string hex { get; set; }
}

public static class AppColors
{
    public static readonly IReadOnlyDictionary<string, string> DarkHexLookup = new Dictionary<string, string>
    {
        { "Crimson Red", "#8b1e1e" },
        { "Rusty Red-Orange", "#a53b1f" },
        { "Burnt Orange", "#b55a24" },
        { "Golden Amber", "#b97c24" },
        { "Mustard Yellow", "#a48f1e" },
        { "Olive Green", "#7b8f1f" },
        { "Leaf Green", "#4f8c32" },
        { "Jade Green", "#2e7c46" },
        { "Sea Teal", "#247e6a" },
        { "Ocean Cyan", "#1f6f82" },
        { "Azure Blue", "#2a5c8f" },
        { "Indigo Blue", "#374a91" },
        { "Royal Violet", "#4c3d92" },
        { "Deep Purple", "#663a8f" },
        { "Magenta Violet", "#82397f" },
        { "Rose Magenta", "#923462" },
        { "Soft Rose", "#9a2c47" },
        { "Deep Wine Red", "#8f2d3d" },
        { "Warm Brown", "#704030" },
        { "Olive Brown", "#5a4b2a" },
        { "Forest Green", "#435531" },
        { "Deep Teal", "#2f5647" },
        { "Muted Blue", "#2a5263" },
        { "Slate Blue", "#3d496a" },
        { "Grape Violet", "#4a3f68" },
        { "Dusty Plum", "#58385e" },
        { "Deep Rosewood", "#6b3451" },
        { "Burgundy", "#7a3342" },
        { "Rust Brown", "#6c3b35" },
        { "Coffee", "#5c4433" },
        { "Charcoal Gray", "#434343" },
        { "Soft Gray", "#2a2a2a" }
    };

    public static readonly IReadOnlyDictionary<string, string> LightHexLookup = new Dictionary<string, string>
    {
        { "Crimson Red", "#f6b3b3" },
        { "Rusty Red-Orange", "#f3b79d" },
        { "Burnt Orange", "#f6c48c" },
        { "Golden Amber", "#f2d18a" },
        { "Mustard Yellow", "#e9dd87" },
        { "Olive Green", "#d2e38b" },
        { "Leaf Green", "#aee29a" },
        { "Jade Green", "#8ad7ac" },
        { "Sea Teal", "#7ad4c4" },
        { "Ocean Cyan", "#7cc9db" },
        { "Azure Blue", "#86b5e8" },
        { "Indigo Blue", "#96a3ec" },
        { "Royal Violet", "#a99cea" },
        { "Deep Purple", "#b794e6" },
        { "Magenta Violet", "#d38dd6" },
        { "Rose Magenta", "#e38fbf" },
        { "Soft Rose", "#f1a0aa" },
        { "Deep Wine Red", "#e99c9c" },
        { "Warm Brown", "#d3a186" },
        { "Olive Brown", "#c8b88a" },
        { "Forest Green", "#b3c199" },
        { "Deep Teal", "#9ec6b3" },
        { "Muted Blue", "#9bbdd0" },
        { "Slate Blue", "#a3b2d4" },
        { "Grape Violet", "#b1a9d0" },
        { "Dusty Plum", "#c0a1c5" },
        { "Deep Rosewood", "#d69eb3" },
        { "Burgundy", "#df9a9a" },
        { "Rust Brown", "#d3a28f" },
        { "Coffee", "#c7ac8d" },
        { "Charcoal Gray", "#bcbcbc" },
        { "Soft Gray", "#e0e0e0" }
    };
}