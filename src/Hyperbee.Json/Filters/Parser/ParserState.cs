namespace Hyperbee.Json.Filters.Parser;

public ref struct ParserState
{
    public readonly ReadOnlySpan<char> Buffer;
    public readonly ref int Pos;

    public ReadOnlySpan<char> Item;
    public Operator Operator;
    public char Terminal;

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
}
