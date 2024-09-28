namespace JavaHateBE.model.DTOs
{
    /// <summary>
    /// Data transfer object for question creation (Id should not be passed).
    /// </summary>
    public record QuestionCreateInput(string Text, float Answer, byte Difficulty = 1);
}