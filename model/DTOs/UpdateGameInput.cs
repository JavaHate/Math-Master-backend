namespace JavaHateBE.Model.DTOs
{
    public record UpdateGameInput(
        Guid Id, 
        GameMode GameMode, 
        uint Score, 
        DateTime StartTime, 
        DateTime EndTime, 
        List<Question> Questions, 
        Guid UserId
    );
}