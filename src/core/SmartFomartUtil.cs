using System;
using System.Diagnostics;
using System.Text.Json;
using SmartFormat;
using SmartFormat.Extensions;

namespace NotifiableTools;

public static class SmartFormatUtil
{
    public static string Format(string format, object? arg)
    {
        var smart = new SmartFormatter()
            .AddExtensions(new DefaultFormatter())
            .AddExtensions(
                new DefaultSource(), 
                new StringSource(), 
                new DictionarySource(),
                new ValueTupleSource(),
                new ReflectionSource()
            );

        return smart.Format(format, arg);
    }

    public static void PrintErrorMessage(Exception ex, string format, object? arg)
    {
        Console.Error.WriteLine($"テンプレートの解釈に失敗しました。Template:{format}, Arg:{arg}");
        Console.Error.WriteLine(ex.Message);
    }

    

}
