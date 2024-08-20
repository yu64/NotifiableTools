


using Json.Schema.Generation;

namespace NotifiableTools;

public readonly record struct Button(

    String Title,
    IPosition position

) : INotion
{
    


}