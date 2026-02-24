public class EditBugDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Priority { get; set; }
    public int? BugGroupId { get; set; }  
    public int? AssignedToUserId { get; set; }
}