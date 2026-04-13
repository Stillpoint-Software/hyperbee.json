---
layout: default
title: JsonPath
has_children: true
nav_order: 1
---

# Hyperbee JsonPath

Hyperbee JsonPath is a high-performance JSONPath parser for `System.Text.Json`, that supports both `JsonElement` and `JsonNode`.
The library is designed to be fast and extensible, allowing support for other JSON document types and functions.

## Features

- **High Performance:** Optimized for performance and low memory allocations.
- **Supports:** `JsonElement` and `JsonNode`.
- **Extensible:** Easily extended to support additional JSON document types and functions.
- **`IEnumerable` Results:** Deferred execution queries with `IEnumerable`.
- **Conformant:** Adheres to the JSONPath Specification [RFC 9535](https://www.rfc-editor.org/rfc/rfc9535.html). 
