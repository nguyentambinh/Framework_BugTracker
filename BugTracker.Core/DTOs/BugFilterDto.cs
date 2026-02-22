using BugTracker.Core.Enums;
public class BugFilterDto
{
    public BugStatus? Status { get; set; }
    public int? GroupId { get; set; }
    public int? UserId { get; set; }
    public int Page { get; set; } = 1;
}