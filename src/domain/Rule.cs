using Json.Schema.Generation;

namespace NotifiableTools;


public record class Rule
{
    [Required]
    public string Name { get; }

    [Required]
    public IBoolFunction Condition { get; }

    public Rule(string name, IBoolFunction condition)
    {
        this.Name = name;
        this.Condition = condition;
    }


}


public class LoopA
{
    public LoopB next;
}

public class LoopB
{
    public LoopC next;
}

public class LoopC
{
    public LoopA next;
}

[AllSubType]
public class SubTypeA
{

}

[AllSubType]
public class SubTypeB : SubTypeA
{
    public SubTypeA sub;
}
