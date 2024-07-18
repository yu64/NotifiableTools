using Json.Schema.Generation;

namespace NotifiableTools;


public class IsContainFunction : IBoolFunction
{
    [Required]
    public required string All;
    
    [Required]
    public required string Part;

    public bool Call()
    {
        return this.All.Contains(this.Part);
    }
}
