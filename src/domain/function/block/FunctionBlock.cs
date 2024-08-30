using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using Json.Schema.Generation;

namespace NotifiableTools;



public readonly record struct FunctionBlock(

    string BlockName,
    bool CanPrint,
    [property: Required] string ResultName,
    [property: Required] Line[] Lines

) : IAllFunction
{

    public async Task<T> Call<T>(IRuleContext ctx)
    {

        var results = new Dictionary<string, object>();
        using var _ = ctx.OpenScope(this, results);

        this.print<T>("StartBlock", "");
        foreach(var line in this.Lines)
        {
            
            if(line.Mark == LineMark.Skip)
            {
                this.print<T>(line, line.Mark.ToString(), "");
                continue;
            }

            if(line.Mark == LineMark.PreReturn)
            {
                this.print<T>(line, line.Mark.ToString(), "");
                break;
            }

            if(!String.IsNullOrEmpty(line.Name) && results.ContainsKey(line.Name))
            {
                this.print<T>(line, "PreCheckName", $"Name:{line.Name}");
                Console.Error.WriteLine($"[Block:{this.BlockName}] 同一スコープ内で名前の重複は許可されていません。Name:{line.Name}");
                this.print<T>(line, "PostCheckName", $"Name:{line.Name}");
                return this.print<T>(line, "Return", $"=> (NameDuplication) Result:{(T)default!}", default!);
            }

            object? lineResult = null;
            if(line.Func != null)
            {
                this.print<T>(line, "PreRun", "");
                lineResult = await line.Func.CallDynamic<dynamic>(ctx);
                this.print<T>(line, "PostRun", $"Result:{lineResult}");
            }
            
            if(!String.IsNullOrEmpty(line.Name))
            {   
                this.print<T>(line, "PreSave", $"Result:{lineResult}");
                results.Add(line.Name, lineResult!);
                this.print<T>(line, "PostSave", $"Result:{lineResult}");
            }

            if(line.Mark == LineMark.PostReturn)
            {
                this.print<T>(line, line.Mark.ToString(), "");
                break;
            }
        }
        this.print<T>("EndBlock", "");

        var result = results.GetValueOrDefault(this.ResultName);
        if(result != null && result is not T)
        {
            Console.Error.WriteLine($"[Block:{this.BlockName}] 戻り値は期待されている型と一致しません。ResultName:{this.ResultName} Expected:{typeof(T)} Actual:{result?.GetType()} Result:{result}");
            return this.print<T>("Return", $"=> (TypeMismatch) Result:{(T)default!}", default!);
        }
        
        return this.print<T>("Return", $"=> Name:{this.ResultName} Result:{result}", (T)result!);
    }


    private T print<T>(string state, string detail, T output = default!)
    {
        if(!this.CanPrint) 
        {
            return output;
        }

        System.Console.WriteLine($"[{nameof(FunctionBlock)}:{this.BlockName}:{state}] {detail}");
        return output;
    }

    private T print<T>(Line line, string state, string detail, T output = default!)
    {
        if(!this.CanPrint) 
        {
            return output;
        }

        System.Console.WriteLine($"[{nameof(FunctionBlock)}:{this.BlockName}:{line.Name}:{state}] {detail}");
        return output;
    }

    

}

public readonly record struct Line(

    string Name,
    IAnyFunction Func,
    LineMark Mark = LineMark.None
);

public enum LineMark
{
    None,
    PreReturn,
    PostReturn,
    Skip,
}