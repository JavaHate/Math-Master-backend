namespace JavaHateBE.Model.DTOs
{
    /// <summary>
    /// Data transfer object for question creation (Id should not be passed).
    /// </summary>
    public record QuestionCreateInput(string Text, double Answer, byte Difficulty = 1);
}