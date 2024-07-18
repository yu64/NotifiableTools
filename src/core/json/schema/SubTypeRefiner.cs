using System.Reflection;
using System.Windows;
using Json.Schema;
using Json.Schema.Generation;
using Json.Schema.Generation.Intents;
using NotifiableTools;

internal class SubTypeRefiner : ISchemaRefiner
{
    public bool ShouldRun(SchemaGenerationContextBase context)
    {
        // we only want to run this if the generated schema has a `type` keyword
        return context.Intents.OfType<TypeIntent>().Any();
    }

    public void Run(SchemaGenerationContextBase context)
    {
        this.RegisterSubType(context);
        this.RegisterTypeName(context);
    }



    //===================================================================================


    private void RegisterSubType(SchemaGenerationContextBase context)
    {
        var type = context.Type;
        var attributes = type.GetCustomAttributes(false);

        //自分のサブタイプを検索
        var subTypes = new List<Type>()
            .Concat(
                attributes.OfType<AllSubTypeAttribute>()
                    .SelectMany((a) => a.findAllSubType(type))
            )
            .Distinct()
            .ToList();

        if(subTypes.Count == 0)
        {
            return;
        }

        var subContexts = subTypes.Select((t) => SchemaGenerationContextCache.Get(t)).ToArray();

        var anyOf = new AnyOfTypeIntent(subContexts);
        context.Intents.Add(anyOf);
    }

    private void RegisterTypeName(SchemaGenerationContextBase context)
    {
        var type = context.Type;

        //自身はインスタンス化可能であるか
        if(type.IsInterface || type.IsAbstract)
        {
            return;
        }

        //自身が別のクラスのサブタイプであるか
        var isSubType = this.GetAllSuperType(type, Assembly.GetAssembly(typeof(SubTypeRefiner)))
        .Select((t) => 
            t.GetCustomAttributes(true)
            .Any((a) => (a is AllSubTypeAttribute))
        ).Any((b) => b);

        if(!isSubType)
        {
            return;
        }
        
        var list = (new Attribute[]{new ConstAttribute(type.Name)}).ToList();
        var typeNameContext = SchemaGenerationContextCache.Get(typeof(string), list);

        //先頭に型情報の追記
        this.EditIntent(context, () => new PropertiesIntent([]), (i) => {
            return new PropertiesIntent(
                i.Properties
                .Prepend(new KeyValuePair<string, SchemaGenerationContextBase>("$type", typeNameContext))
                .ToDictionary()
            );
        });
        
        //型情報を必須とする
        this.EditIntent(context, () => new RequiredIntent([]), (i) => {
            
            i.RequiredProperties.Add("$type");
            return i;
        });

    }



    //===================================================================================


    private IEnumerable<Type> GetAllSuperType(Type type, Assembly? range = null)
    {

        bool WithinRange(Type t)
        {
            return (range == null || t.Assembly == range);
        }


        var searchedTypes = new HashSet<Type>();
        var queue = new List<Type>();
        queue.Add(type);

        while(queue.Count != 0)
        {
            //探索開始地点のクラスを取得
            var currentType = queue.First();
            queue.RemoveAt(0);

            //探索範囲に含まれるか
            if(!WithinRange(currentType))
            {
                continue;
            }

            //探索済みであるか確認
            if(searchedTypes.Contains(currentType))
            {
                continue;
            }

            searchedTypes.Add(currentType);

            yield return currentType;

            

            //探索先の追加
            var superClass = currentType.BaseType;
            if(superClass != null)
            {
                queue.Add(superClass);
            }

            queue.AddRange(currentType.GetInterfaces());
        }

    }

    private void EditIntent<T>(SchemaGenerationContextBase context, Func<T> init, Func<T, T> edit) where T : ISchemaKeywordIntent
    {
        var defaultIntent = context.Intents.OfType<T>().FirstOrDefault();

        var newIntent = edit(defaultIntent ?? init());

        context.Intents.RemoveAll((i) => i is T);
        context.Intents.Add(newIntent);
    }



    //===================================================================================



}