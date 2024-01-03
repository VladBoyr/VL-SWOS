namespace Common.OptionalExtension;

public readonly struct Optional<T>
{
    private readonly T? value;

    public Optional(T value)
    {
        HasValue = true;
        this.value = value;
    }

    public static Optional<T> Empty => new();

    public bool HasValue { get; } = false;
    public T Value
    {
        get
        {
            if (!HasValue)
            {
                throw new InvalidOperationException();
            }

            return value!;
        }
    }

    public static implicit operator Optional<T>(T value)
    {
        return new Optional<T>(value);
    }
}