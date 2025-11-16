namespace Bot.Evaluation;

public record EvaluationWeights
{
    private const int DefaultPieceDifferenceWeight = 100;
    private const int DefaultMovesWeight = 1;

    public static readonly EvaluationWeights Default = new()
    {
        PieceDifferenceWeight = DefaultPieceDifferenceWeight,
        MovesWeight = DefaultMovesWeight
    };

    public int PieceDifferenceWeight { get; init; } = DefaultPieceDifferenceWeight;
    public int MovesWeight { get; init; } = DefaultMovesWeight;
}