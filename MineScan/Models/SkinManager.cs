using System.Collections.Generic;

namespace MineScan.Models;

public static class SkinManager
{
    public static List<SkinInfo> Skins { get; } = new()
    {
        new SkinInfo { Id = "classic", Name = "Classic" },
        new SkinInfo { Id = "modern", Name = "Modern Blue" },
        new SkinInfo { Id = "toxic", Name = "Toxic Green" }
    };
    
    public static SkinInfo DefaultSkin => Skins[0];
}