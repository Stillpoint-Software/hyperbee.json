#region License
// C# Implementation of JSONPath[1]
//
// [1] http://goessner.net/articles/JsonPath/
// [2] https://github.com/atifaziz/JSONPath
//
// The MIT License
//
// Copyright (c) 2019 Brenton Farmer. All rights reserved.
// Portions Copyright (c) 2007 Atif Aziz. All rights reserved.
// Portions Copyright (c) 2007 Stefan Goessner (goessner.net)
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
#endregion

using System.Text.Json;
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Tokenizer;

namespace Hyperbee.Json;
// https://ietf-wg-jsonpath.github.io/draft-ietf-jsonpath-base/draft-ietf-jsonpath-base.html
// https://github.com/ietf-wg-jsonpath/draft-ietf-jsonpath-base

public sealed class JsonPath
{
    public static IJsonPathFilterEvaluator<JsonElement> DefaultEvaluator { get; set; } = new JsonPathExpressionElementEvaluator();
    private readonly IJsonPathFilterEvaluator<JsonElement> _evaluator;

    private readonly JsonDocumentPathVisitor _visitor = new();

    // ctor

    public JsonPath()
        : this( null )
    {
    }

    public JsonPath( IJsonPathFilterEvaluator<JsonElement> evaluator )
    {
        _evaluator = evaluator ?? DefaultEvaluator ?? new JsonPathExpressionElementEvaluator();
    }

    public IEnumerable<JsonElement> Select( in JsonElement value, string query )
    {
        return SelectPath( value, value, query ).Select( x => x.Value );
    }

    internal IEnumerable<JsonElement> Select( in JsonElement value, JsonElement root, string query )
    {
        return SelectPath( value, root, query ).Select( x => x.Value );
    }

    public IEnumerable<JsonPathElement> SelectPath( in JsonElement value, string query )
    {
        return SelectPath( value, value, query );
    }

    internal IEnumerable<JsonPathElement> SelectPath( in JsonElement value, in JsonElement root, string query )
    {
        if ( string.IsNullOrWhiteSpace( query ) )
            throw new ArgumentNullException( nameof( query ) );

        // quick out

        if ( query == "$" )
            return [new JsonPathElement( value, query )];

        // tokenize

        var tokens = JsonPathQueryTokenizer.Tokenize( query );

        // initiate the expression walk

        if ( !tokens.IsEmpty )
        {
            var firstToken = tokens.Peek().Selectors.First().Value;
            if ( firstToken == "$" || firstToken == "@" )
                tokens = tokens.Pop();
        }

        return _visitor.ExpressionVisitor( new JsonDocumentPathVisitor.VisitorArgs( value, root, tokens, "$" ), _evaluator.Evaluator );
    }
}
