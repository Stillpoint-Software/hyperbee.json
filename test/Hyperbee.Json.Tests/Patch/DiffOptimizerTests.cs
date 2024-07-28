using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using Hyperbee.Json.Patch;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Patch;

[TestClass]
public class DiffOptimizerTests
{
    [TestMethod]
    public void OptimizeArrayOperations_WhenArrayDiffExists()
    {
        const string sourceJson =
            """
            {
            "categories": ["a", "b", "c"]
            }
            """;
        const string targetJson =
            """
            {
            "categories": ["a", "c", "d"]
            }
            """;

        var source = JsonNode.Parse( sourceJson );
        var target = JsonNode.Parse( targetJson );

        var diff = JsonDiff<JsonNode>.Diff( source, target ).ToArray();

        var optimizer = new DiffOptimizer<JsonNode>();
        var optimized = optimizer.OptimizeDiff( source, target, diff ).ToArray();

        // Validate the optimized operations
        Assert.IsTrue( optimized.Length == 2 );
        Assert.IsTrue( optimized.Any( op => op.Operation == PatchOperationType.Remove && op.Path == "/categories/1" ) );
        Assert.IsTrue( optimized.Any( op => op.Operation == PatchOperationType.Add && op.Path == "/categories/2" && op.Value.ToString() == "d" ) );

        // Apply both non-optimized and optimized patches to verify results
        var nonOptimizedSource = JsonNode.Parse( sourceJson );
        var optimizedSource = JsonNode.Parse( sourceJson );

        var nonOptimizedPatch = new JsonPatch( diff ); 
        nonOptimizedPatch.Apply( nonOptimizedSource ); // BF this is erroring with: System.InvalidOperationException: The node already has a parent.

        var optimizedPatch = new JsonPatch( optimized );
        optimizedPatch.Apply( optimizedSource ); // BF this is erroring with: System.InvalidOperationException: The node already has a parent.

        Assert.IsTrue( JsonNode.DeepEquals( nonOptimizedSource, optimizedSource ) );
    }

    [TestMethod]
    public void RemoveRedundantObjectOperations_WhenParentObjectRemoved()
    {
        const string sourceJson =
            """
            {
                "first": "John",
                "last": "Doe"
            }
            """;
        const string targetJson =
            """
            {
                "first": "John"
            }
            """;

        var source = JsonNode.Parse( sourceJson );
        var target = JsonNode.Parse( targetJson );

        var diff = JsonDiff<JsonNode>.Diff( source, target ).ToList();

        var optimizer = new DiffOptimizer<JsonNode>();
        var optimized = optimizer.OptimizeDiff( source, target, diff );

        // Validate the optimized operations
        Assert.IsTrue( optimized.Count == 1 );
        Assert.IsTrue( optimized.Any( op => op.Operation == PatchOperationType.Remove && op.Path == "/last" ) );

        // Apply both non-optimized and optimized patches to verify results
        var nonOptimizedSource = JsonNode.Parse( sourceJson );
        var optimizedSource = JsonNode.Parse( sourceJson );

        var nonOptimizedPatch = new JsonPatch( [.. diff] );
        nonOptimizedPatch.Apply( nonOptimizedSource );

        var optimizedPatch = new JsonPatch( [.. optimized] );
        optimizedPatch.Apply( optimizedSource );

        Assert.IsTrue( JsonNode.DeepEquals( nonOptimizedSource, optimizedSource ) );
    }

    [TestMethod]
    public void CombineConsecutiveOperations_WhenMultipleChangesOnSamePath()
    {
        const string sourceJson =
            """
            {
                "first": "John",
                "last": "Doe"
            }
            """;
        const string targetJson =
            """
            {
                "first": "Mark",
                "last": "Doe"
            }
            """;

        var source = JsonNode.Parse( sourceJson );
        var target = JsonNode.Parse( targetJson );

        var diff = JsonDiff<JsonNode>.Diff( source, target ).ToList();

        var optimizer = new DiffOptimizer<JsonNode>();
        var optimized = optimizer.OptimizeDiff( source, target, diff );

        // Validate the optimized operations
        Assert.IsTrue( optimized.Count == 1 );
        Assert.IsTrue( optimized.Any( op => op.Operation == PatchOperationType.Replace && op.Path == "/first" && (string) op.Value == "Mark" ) );

        // Apply both non-optimized and optimized patches to verify results
        var nonOptimizedSource = JsonNode.Parse( sourceJson );
        var optimizedSource = JsonNode.Parse( sourceJson );

        var nonOptimizedPatch = new JsonPatch( [.. diff] );
        nonOptimizedPatch.Apply( nonOptimizedSource );

        var optimizedPatch = new JsonPatch( [.. optimized] );
        optimizedPatch.Apply( optimizedSource );

        Assert.IsTrue( JsonNode.DeepEquals( nonOptimizedSource, optimizedSource ) );
    }

    [TestMethod]
    public void RemoveNoOpOperations_WhenNoOpExists()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John"
            }
            """
        );
        var target = JsonNode.Parse(
            """
            {
                "first": "John"
            }
            """
        );

        var diff = new List<PatchOperation> { new(PatchOperationType.Replace, "/first", null, "John") };

        var optimizer = new DiffOptimizer<JsonNode>();
        var optimized = optimizer.OptimizeDiff( source, target, diff );

        // Validate that no-op operations are removed
        Assert.IsTrue( optimized.Count == 0 );
    }
}
