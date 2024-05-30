﻿
namespace Hyperbee.Json.Evaluators;

public delegate object JsonPathEvaluator<in TType>( string script, TType current, TType root, string basePath );

public interface IJsonPathScriptEvaluator<in TType>
{
    public object Evaluator( string script, TType current, TType root, string basePath );
}
