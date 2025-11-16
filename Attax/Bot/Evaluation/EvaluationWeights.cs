namespace Bot.Evaluation;

public record EvaluationWeights
{
    public static readonly EvaluationWeights Default = new()
    {
        PieceDifferenceWeight = 100,
        MovesWeight = 1
    };

    public int PieceDifferenceWeight { get; init; } = Default.PieceDifferenceWeight;
    public int MovesWeight { get; init; } = Default.MovesWeight;
}