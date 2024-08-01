---
layout: default
title: JsonPath
has_children: true
nav_order: 1
---

# Hyperbee JsonPath

Hyperbee JsonPath is a high-performance JSONPath parser for .NET, that supports both `JsonElement` and `JsonNode`.  
The library is designed to be fast and extensible, allowing support for other JSON document types and functions.

## Features

- **High Performance:** Optimized for performance and low memory allocations.
- **Supports:** `JsonElement` and `JsonNode`.
- **Extensible:** Easily extended to support additional JSON document types and functions.
- **`IEnumerable` Results:** Deferred execution queries with `IEnumerable`.
- **Conformant:** Adheres to the JSONPath Specification [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html). 

## JSONPath RFC

Hyperbee.Json conforms to the RFC-9535, and aims to support the [JSONPath consensus](https://cburgmer.github.io/json-path-comparison) 
when the RFC is unopinionated. When the RFC is unopinionated, and where the consensus is ambiguous or not aligned with our 
performance and usability goals, we may deviate. Our goal is to provide a robust and performant library while strengthening our alignment with the RFC and the community.
