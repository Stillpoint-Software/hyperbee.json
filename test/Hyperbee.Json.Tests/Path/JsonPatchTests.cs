using System;
using System.Text.Json.Nodes;
using Hyperbee.Json.Patch;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Hyperbee.Json.Tests.Path;

[TestClass]
public class JsonPatchTests
{
    [TestMethod]
    public void Add_WhenValueProperty()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "company": "Acme"
                }
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Add, "/job/title", null, "developer" )
        );

        patch.Apply( source );

        Assert.AreEqual( "developer", source!["job"]!["title"]!.GetValue<string>() );
    }

    [TestMethod]
    public void Add_WhenValueObject()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John"
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Add, "/job", null, JsonNode.Parse( """
                                                                                      {
                                                                                          "title": "developer",
                                                                                          "company": "Acme"
                                                                                      }
                                                                                      """ ) )
        );

        patch.Apply( source );

        Assert.AreEqual( "developer", source["job"]!["title"]!.GetValue<string>() );
        Assert.AreEqual( "Acme", source["job"]!["company"]!.GetValue<string>() );
    }

    [TestMethod]
    public void Add_WhenValueArray()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Add, "/categories/0", null, "b" )
        );

        patch.Apply( source );

        Assert.AreEqual( "b", source!["categories"]![0]!.GetValue<string>() );
    }

    [TestMethod]
    public void Add_WhenValueArrayAtExistIndex()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Add, "/categories/0", null, "b" )
        );

        patch.Apply( source );

        Assert.AreEqual( 2, ((JsonArray) source!["categories"]!).Count );
        Assert.AreEqual( "b", source!["categories"]![0]!.GetValue<string>() );
        Assert.AreEqual( "a", source!["categories"]![1]!.GetValue<string>() );
    }

    [TestMethod]
    public void Add_WhenValueArrayAtEnd()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Add, "/categories/-", null, "b" )
        );

        patch.Apply( source );

        Assert.AreEqual( 2, ((JsonArray) source!["categories"]!).Count );
        Assert.AreEqual( "a", source!["categories"]![0]!.GetValue<string>() );
        Assert.AreEqual( "b", source!["categories"]![1]!.GetValue<string>() );
    }

    [TestMethod]
    public void Add_WhenValueArrayAtNextIndex()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Add, "/categories/1", null, "b" )
        );

        patch.Apply( source );

        Assert.AreEqual( 2, ((JsonArray) source!["categories"]!).Count );
        Assert.AreEqual( "a", source!["categories"]![0]!.GetValue<string>() );
        Assert.AreEqual( "b", source!["categories"]![1]!.GetValue<string>() );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void AddFail_WhenValueArrayAndMissingParent()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John"
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Add, "/categories/0", null, "b" )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void AddFail_WhenValueArrayOutOfRange()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Add, "/categories/2", null, "b" )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void AddFail_WhenValueArrayInvalidIndex()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Add, "/categories/NaN", null, "b" )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void AddFail_WhenValuePropertyAndMissingParent()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John"
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Add, "/job/title", null, "developer" )
        );

        patch.Apply( source );
    }

    [TestMethod]
    public void Copy_WhenFromProperty()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "company": "Acme",
                    "title": "developer"
                }
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Copy, "/title", "/job/title", null )
        );

        patch.Apply( source );

        Assert.AreEqual( 2, ((JsonObject) source!["job"]!).Count );
        Assert.AreEqual( "developer", source!["title"]!.GetValue<string>() );
    }

    [TestMethod]
    public void Copy_WhenFromObject()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "title": "developer",
                    "company": "Acme"
                }
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Copy, "/position", "/job", null )
        );

        patch.Apply( source );

        Assert.AreEqual( 3, ((JsonObject) source!).Count );
        Assert.AreEqual( "developer", source!["position"]!["title"]!.GetValue<string>() );
        Assert.AreEqual( "Acme", source!["position"]!["company"]!.GetValue<string>() );
    }

    [TestMethod]
    public void Copy_WhenFromArray()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"],
                "ideas": []
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Copy, "/ideas/0", "/categories/0", null )
        );

        patch.Apply( source );

        Assert.AreEqual( 1, ((JsonArray) source!["categories"]!).Count );
        Assert.AreEqual( "a", source!["categories"]![0]!.GetValue<string>() );
        Assert.AreEqual( 1, ((JsonArray) source!["ideas"]!).Count );
        Assert.AreEqual( "a", source!["ideas"]![0]!.GetValue<string>() );
    }

    [TestMethod]
    public void Copy_WhenFromArrayToObject()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Copy, "/ideas", "/categories/0", null )
        );

        patch.Apply( source );

        Assert.AreEqual( 1, ((JsonArray) source!["categories"]!).Count );
        Assert.AreEqual( "a", source!["ideas"]!.GetValue<string>() );
    }

    [TestMethod]
    public void Copy_WhenFromObjectToArray()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "title": "developer",
                    "company": "Acme"
                },
                "ideas": []
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Copy, "/ideas/0", "/job", null )
        );

        patch.Apply( source );

        Assert.AreEqual( 3, ((JsonObject) source!).Count );
        Assert.AreEqual( "developer", source!["ideas"]![0]!["title"]!.GetValue<string>() );
        Assert.AreEqual( "Acme", source!["ideas"]![0]!["company"]!.GetValue<string>() );
    }

    [TestMethod]
    public void Copy_WhenFromPropertyToArrayAtExistIndex()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Copy, "/categories/0", "/first", null )
        );

        patch.Apply( source );

        Assert.AreEqual( "John", source!["first"]!.GetValue<string>() );
        Assert.AreEqual( 2, ((JsonArray) source!["categories"]!).Count );
        Assert.AreEqual( "John", source!["categories"]![0]!.GetValue<string>() );
        Assert.AreEqual( "a", source!["categories"]![1]!.GetValue<string>() );
    }

    [TestMethod]
    public void Copy_WhenFromPropertyToArrayAtNextIndex()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Copy, "/categories/1", "/first", null )
        );

        patch.Apply( source );

        Assert.AreEqual( "John", source!["first"]!.GetValue<string>() );
        Assert.AreEqual( 2, ((JsonArray) source!["categories"]!).Count );
        Assert.AreEqual( "a", source!["categories"]![0]!.GetValue<string>() );
        Assert.AreEqual( "John", source!["categories"]![1]!.GetValue<string>() );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void CopyFail_WhenFromPropertyToArrayOutOfRange()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "title": "developer",
                    "company": "Acme"
                },
                "ideas": []
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Copy, "/ideas/1", "/job", null )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void CopyFail_WhenFromPropertyToArrayInvalidIndex()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "title": "developer",
                    "company": "Acme"
                },
                "ideas": []
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Copy, "/ideas/NaN", "/job", null )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void CopyFail_WhenFromPropertyAndMissingParent()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John"
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Copy, "/title", "/job/title", null )
        );

        patch.Apply( source );
    }

    [TestMethod]
    public void Copy_WhenFromPropertyIsChildOfSelf()
    {
        var source = JsonNode.Parse(
            """
            {
                "job": {
                    "title": "developer",
                    "company": "Acme"
                }
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Copy, "/job/sub", "/job", null )
        );

        patch.Apply( source );

        Assert.AreEqual( "developer", source!["job"]!["title"]!.GetValue<string>() );
        Assert.AreEqual( "Acme", source!["job"]!["company"]!.GetValue<string>() );

        Assert.AreEqual( "developer", source!["job"]!["sub"]!["title"]!.GetValue<string>() );
        Assert.AreEqual( "Acme", source!["job"]!["sub"]!["company"]!.GetValue<string>() );
    }

    [TestMethod]
    public void Move_WhenFromProperty()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "company": "Acme",
                    "title": "developer"
                }
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Move, "/title", "/job/title", null )
        );

        patch.Apply( source );

        Assert.AreEqual( 1, ((JsonObject) source!["job"]!).Count );
        Assert.AreEqual( "developer", source!["title"]!.GetValue<string>() );
    }

    [TestMethod]
    public void Move_WhenFromObject()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "title": "developer",
                    "company": "Acme"
                }
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Move, "/position", "/job", null )
        );

        patch.Apply( source );

        Assert.AreEqual( "developer", source!["position"]!["title"]!.GetValue<string>() );
        Assert.AreEqual( "Acme", source!["position"]!["company"]!.GetValue<string>() );
    }

    [TestMethod]
    public void Move_WhenFromArray()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"],
                "ideas": []
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Move, "/ideas/0", "/categories/0", null )
        );

        patch.Apply( source );

        Assert.AreEqual( 0, ((JsonArray) source!["categories"]!).Count );
        Assert.AreEqual( 1, ((JsonArray) source!["ideas"]!).Count );
        Assert.AreEqual( "a", source!["ideas"]![0]!.GetValue<string>() );
    }

    [TestMethod]
    public void Move_WhenFromArrayToObject()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Move, "/ideas", "/categories/0", null )
        );

        patch.Apply( source );

        Assert.AreEqual( 0, ((JsonArray) source!["categories"]!).Count );
        Assert.AreEqual( "a", source!["ideas"]!.GetValue<string>() );
    }

    [TestMethod]
    public void Move_WhenFromObjectToArray()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "title": "developer",
                    "company": "Acme"
                },
                "ideas": []
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Move, "/ideas/0", "/job", null )
        );

        patch.Apply( source );

        Assert.AreEqual( 2, ((JsonObject) source!).Count );
        Assert.AreEqual( "developer", source!["ideas"]![0]!["title"]!.GetValue<string>() );
        Assert.AreEqual( "Acme", source!["ideas"]![0]!["company"]!.GetValue<string>() );
    }

    [TestMethod]
    public void Move_WhenFromPropertyToArrayAtExistIndex()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Move, "/categories/0", "/first", null )
        );

        patch.Apply( source );

        Assert.AreEqual( 2, ((JsonArray) source!["categories"]!).Count );
        Assert.AreEqual( "John", source!["categories"]![0]!.GetValue<string>() );
        Assert.AreEqual( "a", source!["categories"]![1]!.GetValue<string>() );
    }

    [TestMethod]
    public void Move_WhenFromPropertyToArrayAtNextIndex()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Move, "/categories/1", "/first", null )
        );

        patch.Apply( source );

        Assert.AreEqual( 2, ((JsonArray) source!["categories"]!).Count );
        Assert.AreEqual( "a", source!["categories"]![0]!.GetValue<string>() );
        Assert.AreEqual( "John", source!["categories"]![1]!.GetValue<string>() );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void MoveFail_WhenFromPropertyToArrayOutOfRange()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "title": "developer",
                    "company": "Acme"
                },
                "ideas": []
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Move, "/ideas/1", "/job", null )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void MoveFail_WhenFromPropertyToArrayInvalidIndex()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "title": "developer",
                    "company": "Acme"
                },
                "ideas": []
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Move, "/ideas/NaN", "/job", null )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void MoveFail_WhenFromPropertyAndMissingParent()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John"
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Move, "/title", "/job/title", null )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( InvalidOperationException ) )]
    public void MoveFail_WhenFromPropertyIsChildOfSelf()
    {
        // TODO: handle as a JsonPatchException vs InvalidOperationException thrown from System.Text.Json

        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "title": "developer",
                    "company": "Acme"
                }
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Move, "/job/sub", "/job", null )
        );

        patch.Apply( source );
    }

    [TestMethod]
    public void Remove_WhenValueProperty()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "title": "developer",
                    "company": "Acme"
                }
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Remove, "/job/title", null, null )
        );

        patch.Apply( source );

        Assert.AreEqual( 1, source["job"]!.AsObject().Count );
    }

    [TestMethod]
    public void Remove_WhenValueObject()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "title": "developer",
                    "company": "Acme"
                }
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Remove, "/job", null, null )
        );

        patch.Apply( source );

        Assert.AreEqual( 1, source!.AsObject().Count );
    }

    [TestMethod]
    public void Remove_WhenValueArray()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a", "b" ]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Remove, "/categories", null, null )
        );

        patch.Apply( source );

        Assert.AreEqual( 1, source!.AsObject().Count );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void RemoveFail_WhenValueArrayAndMissingParent()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John"
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Remove, "/categories/0", null, null )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void RemoveFail_WhenValueArrayOutOfRange()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a", "b"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Remove, "/categories/2", null, null )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void RemoveFail_WhenValueArrayInvalidIndex()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Remove, "/categories/NaN", null, null )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void RemoveFail_WhenValuePropertyAndMissingParent()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John"
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Remove, "/job/title", null, null )
        );

        patch.Apply( source );
    }

    [TestMethod]
    public void Replace_WhenValueProperty()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John"
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Replace, "/first", null, "Mark" )
        );

        patch.Apply( source );

        Assert.AreEqual( "Mark", source!["first"]!.GetValue<string>() );
    }

    [TestMethod]
    public void Replace_WhenValueObject()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "job": {
                    "title": "Marketing",
                    "company": "Microsoft"
                }
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Replace, "/job", null, JsonNode.Parse( """
                                                                                          {
                                                                                              "title": "developer",
                                                                                              "company": "Acme"
                                                                                          }
                                                                                          """ ) )
        );

        patch.Apply( source );

        Assert.AreEqual( "developer", source!["job"]!["title"]!.GetValue<string>() );
        Assert.AreEqual( "Acme", source!["job"]!["company"]!.GetValue<string>() );
    }

    [TestMethod]
    public void Replace_WhenValueArray()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": [ "a", "b" ]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Replace, "/categories/0", null, "c" )
        );

        patch.Apply( source );

        Assert.AreEqual( 2, ((JsonArray) source!["categories"]!).Count );
        Assert.AreEqual( "c", source!["categories"]![0]!.GetValue<string>() );
        Assert.AreEqual( "b", source!["categories"]![1]!.GetValue<string>() );
    }

    [TestMethod]
    public void Replace_WhenValueArrayAtExistIndex()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Replace, "/categories/0", null, "b" )
        );

        patch.Apply( source );

        Assert.AreEqual( 1, ((JsonArray) source!["categories"]!).Count );
        Assert.AreEqual( "b", source!["categories"]![0]!.GetValue<string>() );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void ReplaceFail_WhenValueArrayAtEnd()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Replace, "/categories/-", null, "b" )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void ReplaceFail_WhenValueArrayAndMissingParent()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John"
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Replace, "/categories/0", null, "b" )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void ReplaceFail_WhenValueArrayOutOfRange()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Replace, "/categories/2", null, "b" )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void ReplaceFail_WhenValueArrayInvalidIndex()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John",
                "categories": ["a"]
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Replace, "/categories/NaN", null, "b" )
        );

        patch.Apply( source );
    }

    [TestMethod]
    [ExpectedException( typeof( JsonPatchException ) )]
    public void ReplaceFail_WhenValuePropertyAndMissingParent()
    {
        var source = JsonNode.Parse(
            """
            {
                "first": "John"
            }
            """ );

        var patch = new JsonPatch(
            new PatchOperation( PatchOperationType.Replace, "/job/title", null, "developer" )
        );

        patch.Apply( source );
    }
}
