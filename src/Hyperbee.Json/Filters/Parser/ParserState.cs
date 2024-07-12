using System.Diagnostics;

namespace Hyperbee.Json.Filters.Parser;

[DebuggerDisplay( "{Buffer.ToString()}, Item = {Item.ToString()}, Operator = {Operator}, Pos = {Pos.ToString()}" )]
public ref struct ParserState
{
    public ReadOnlySpan<char> Buffer { get; }
    public ReadOnlySpan<char> Item { get; internal set; }

    public bool TrailingWhitespace { get; internal set; }
    public bool IsArgument { get; internal set; }
    public int BracketDepth { get; internal set; }
    public ref int ParenDepth;

    public Operator Operator { get; set; }
    public char Terminal { get; init; }

    public readonly ref int Pos;

    internal ParserState( ReadOnlySpan<char> buffer, ReadOnlySpan<char> item, ref int pos, ref int parenDepth, Operator tokenType, char terminal )
    {
        Buffer = buffer;
        Item = item;
        Operator = tokenType;
        Terminal = terminal;
        Pos = ref pos;
        ParenDepth = ref parenDepth;
    }

    public readonly bool EndOfBuffer => Pos >= Buffer.Length;
    public readonly bool IsParsing => Pos < Buffer.Length && Previous != Terminal;

    public readonly char Current => Buffer[Pos];
    public readonly char Previous => Buffer[Pos - 1];

    internal void SetItem( int itemStart, int itemEnd )
    {
        var item = Buffer[itemStart..itemEnd];
        TrailingWhitespace = !item.IsEmpty && char.IsWhiteSpace( item[^1] );

        Item = item.TrimEnd();
    }
}
