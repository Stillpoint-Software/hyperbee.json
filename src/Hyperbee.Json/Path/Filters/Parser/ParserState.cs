using System.Diagnostics;

namespace Hyperbee.Json.Path.Filters.Parser;

[DebuggerDisplay( "{Buffer.ToString()}, Item = {Item.ToString()}, Operator = {Operator}, Pos = {Pos.ToString()}" )]
internal ref struct ParserState
{
    public ReadOnlySpan<char> Buffer { get; }
    public ReadOnlySpan<char> Item { get; internal set; }

    public bool TrailingWhitespace { get; internal set; }
    public bool IsArgument { get; internal set; }
    public int BracketDepth { get; internal set; }

    public Operator Operator { get; set; }
    public char TerminalCharacter { get; init; }

#if NETSTANDARD
    // Pos and ParenDepth have to be shared across every copy of this state: the parser
    // recurses by making a `state with { ... }` copy, and the copy must advance the one
    // cursor. On net10.0 and net11.0 that is a ref field. netstandard2.1 predates ref
    // fields in ref structs (CS9064), so the two counters live in one small array that
    // every copy points at, which gives the same aliasing for one allocation per parse.
    private readonly int[] _counters;

    private const int PosIndex = 0;
    private const int ParenDepthIndex = 1;

    public int Pos
    {
        readonly get => _counters[PosIndex];
        set => _counters[PosIndex] = value;
    }

    public int ParenDepth
    {
        readonly get => _counters[ParenDepthIndex];
        set => _counters[ParenDepthIndex] = value;
    }

    // The ref parameters are accepted for signature parity with the other targets and
    // then read by value. The only caller, FilterParser.Parse, passes locals it never
    // reads again, so nothing observes the lost write-back.
    internal ParserState( ReadOnlySpan<char> buffer, ReadOnlySpan<char> item, ref int pos, ref int parenDepth, Operator tokenType, char terminalCharacter )
    {
        Buffer = buffer;
        Item = item;
        Operator = tokenType;
        TerminalCharacter = terminalCharacter;
        _counters = [pos, parenDepth];
    }
#else
    public ref int ParenDepth;

    public readonly ref int Pos;

    internal ParserState( ReadOnlySpan<char> buffer, ReadOnlySpan<char> item, ref int pos, ref int parenDepth, Operator tokenType, char terminalCharacter )
    {
        Buffer = buffer;
        Item = item;
        Operator = tokenType;
        TerminalCharacter = terminalCharacter;
        Pos = ref pos;
        ParenDepth = ref parenDepth;
    }
#endif

    public readonly bool EndOfBuffer => Pos >= Buffer.Length;
    public readonly bool IsParsing => Pos < Buffer.Length && Previous != TerminalCharacter;

    public readonly char Current => Buffer[Pos];
    public readonly char Previous => Buffer[Pos - 1];

    internal void SetItem( int itemStart, int itemEnd )
    {
        var item = Buffer[itemStart..itemEnd];
        TrailingWhitespace = !item.IsEmpty && char.IsWhiteSpace( item[^1] );

        Item = item.TrimEnd();
    }
}
