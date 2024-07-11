

using System;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows;


namespace NotifiableTools;


public enum AssetEnum
{
    APP_ICON
}


public static class AssetEnumExt
{
    public static Stream Open(this AssetEnum asset)
    {
        var asm = Assembly.GetEntryAssembly();
        asm = asm ?? throw new Exception("GetEntryAssembly is null");
        
        var name = asset.ToString().ToLower();
        var stream = asm.GetManifestResourceStream(name);
        stream = stream ?? throw new Exception($"Not Found Asset \"${name}\" ");

        return stream;
    }

    public static T Create<T>(this AssetEnum asset, Func<Stream, T> func)
    {
        using var stream = asset.Open();
        return func(stream);
    }
}