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

using System.Text.Json.Nodes;
using Hyperbee.Json.Evaluators;
using Hyperbee.Json.Tokenizer;

namespace Hyperbee.Json.Nodes;
// https://ietf-wg-jsonpath.github.io/draft-ietf-jsonpath-base/draft-ietf-jsonpath-base.html
// https://github.com/ietf-wg-jsonpath/draft-ietf-jsonpath-base

public sealed class JsonPathNode
{
    public static IJsonPathFilterEvaluator<JsonNode> DefaultEvaluator { get; set; } = new JsonPathExpressionNodeEvaluator();
    private readonly IJsonPathFilterEvaluator<JsonNode> _evaluator;

    private readonly JsonNodePathVisitor _visitor = new();

    // ctor

    public JsonPathNode()
        : this( null )
    {
    }

    public JsonPathNode( IJsonPathFilterEvaluator<JsonNode> evaluator )
    {
        _evaluator = evaluator ?? DefaultEvaluator ?? new JsonPathExpressionNodeEvaluator();
    }

    public IEnumerable<JsonNode> Select( in JsonNode value, string query )
    {
        return Select( value, value, query );
    }

    internal IEnumerable<JsonNode> Select( in JsonNode value, in JsonNode root, string query )
    {
        if ( string.IsNullOrWhiteSpace( query ) )
            throw new ArgumentNullException( nameof( query ) );

        // quick out

        if ( query == "$" )
            return [value];

        // tokenize

        var tokens = JsonPathQueryTokenizer.Tokenize( query );

        // initiate the expression walk

        if ( !tokens.IsEmpty )
        {
            var firstToken = tokens.Peek().Selectors.First().Value;
            if ( firstToken == "$" || firstToken == "@" )
                tokens = tokens.Pop();
        }

        return _visitor.ExpressionVisitor( new JsonNodePathVisitor.VisitorArgs( value, root, tokens ), _evaluator.Evaluator );
    }
}
