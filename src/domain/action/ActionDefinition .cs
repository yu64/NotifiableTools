

using Json.Schema.Generation;

namespace NotifiableTools;

public readonly record struct ActionDefinition (

    [property: Required]
    [property: Description("SmartFormat")] 
    string CommandTemplate,
    
    [property: Default(false)]
    [property: Description("標準出力可能か")]
    bool CanStdOut = false,
    
    [property: Default("UTF-8")]
    [property: Description("標準出力の文字コード")]
    string Encoding = "UTF-8"

)
{
    public static readonly ActionDefinition NULL = new ActionDefinition("");

};