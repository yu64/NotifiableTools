using System;
using System.Text.Json.Nodes;
using Json.Schema.Generation.Intents;

namespace Json.Schema.Generation;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
public class ConstEnumNameAttribute(int enumNum) : ConditionalAttribute, IAttributeHandler
{
    //プロパティでなければならない
    public int EnumNum { get; } = enumNum;

    void IAttributeHandler.AddConstraints(SchemaGenerationContextBase context, Attribute attribute)
	{

		//メンバーがEnum型を扱っていない場合、処理を行わない
		if(!context.Type.IsEnum)
		{
			return;
		}

		//Enum定義を削除
		context.Intents.RemoveAll((i) => i is EnumIntent);

		//Enum名の定数を追加
		var enumNameFromNum = Enum.GetName(context.Type, this.EnumNum);
		context.Intents.Add(new ConstIntent(enumNameFromNum));
		return;
	}
}