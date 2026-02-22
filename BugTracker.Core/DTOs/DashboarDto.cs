using BugTracker.Core.Entities;
using System.Collections.Generic;

public class DashboardDto
{
    public List<Bug> Bugs { get; set; }

    public List<BugGroup> BugGroups { get; set; }
    public List<ApplicationUser> Users { get; set; }

    public int TotalBugs { get; set; }
    public int OpenBugs { get; set; }
    public int InProgressBugs { get; set; }
    public int ClosedBugs { get; set; }
}