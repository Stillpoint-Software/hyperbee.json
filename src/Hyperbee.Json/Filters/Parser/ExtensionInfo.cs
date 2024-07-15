﻿namespace Hyperbee.Json.Filters.Parser;

[Flags]
public enum ExtensionInfo
{
    MustCompare = 0x01,
    MustNotCompare = 0x02,
    ExpectNormalized = 0x10,
}