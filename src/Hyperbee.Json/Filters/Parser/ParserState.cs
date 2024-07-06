using Hyperbee.Json.Extensions;

namespace Hyperbee.Json.Filters.Parser;

public ref struct ParserState
{
    public ReadOnlySpan<char> Buffer { get; }
    public ReadOnlySpan<char> Item { get; internal set; }

    public bool TrailingWhitespace { get; internal set; }
    public bool IsArgument { get; internal set; }

    public Operator Operator { get; set; }
    public char Terminal { get; init; }

    public readonly ref int Pos;

    internal ParserState( ReadOnlySpan<char> buffer, ReadOnlySpan<char> item, ref int pos, Operator tokenType, char terminal )
    {
        Buffer = buffer;
        Item = item;
        Operator = tokenType;
        Terminal = terminal;
        Pos = ref pos;
    }

    public readonly bool EndOfBuffer => Pos >= Buffer.Length;
    public readonly bool IsParsing => Pos < Buffer.Length && Buffer[Pos] != Terminal;
    public readonly bool IsTerminal => Buffer[Pos] == Terminal;

    public readonly char Current => Buffer[Pos];
    public readonly char Previous => Buffer[Pos - 1];

    internal void SetItem( int itemStart, int itemEnd )
    {
        var item = Buffer[itemStart..itemEnd];
        TrailingWhitespace = !item.IsEmpty && char.IsWhiteSpace( item[^1] );

        Item = JsonHelper.Unescape( item.TrimEnd() );
    }
}
