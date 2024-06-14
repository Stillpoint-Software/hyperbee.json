//NOTE: Should be run with `dotnet run -c release` in the project folder
using BenchmarkDotNet.Running;
using Hyperbee.Json.Benchmark;

BenchmarkSwitcher.FromAssembly( typeof( Program ).Assembly ).Run( args, new Config() );
