using Microsoft.EntityFrameworkCore;
using Standings.Application.Interfaces.IRepositories;
using Standings.Domain.Entities.AppDbContextEntity;
using Standings.Persistence.Contexts;
using Standings.Persistence.Implementations.Repositories;
using System.Linq;

public class GroupRepository : Repository<Group>, IGroupRepository
{
    private readonly AppDbContext _context;

    public GroupRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    // Retrieve all groups
    public async Task<IEnumerable<Group>> GetAllGroupsAsync()
    {
        return await _context.Groups.ToListAsync();
    }

    // Retrieve a group by year
    public async Task<Group> GetByYearAsync(int year)
    {
        return await _context.Groups.FirstOrDefaultAsync(g => g.Year == year);
    }

    // Retrieve top 5 students for the specified group's year
    public async Task<IEnumerable<Student>> GetTop5StudentsAsync(int groupId)
    {
        var group = await _context.Groups.FindAsync(groupId);
        if (group == null) return new List<Student>();  // Return empty list if group is not found

        return await _context.Students
            .Where(s => s.GroupId == groupId)
            .OrderByDescending(s => s.Averages
                .Where(a => a.Year == group.Year)   // Filter averages by the group's year
                .Average(a => a.AverageGrade))
            .Take(5)
            .ToListAsync();
    }

    // Calculate the average score for the specified group and year
    public async Task<double> GetGroupAverageAsync(int groupId)
    {
        var group = await _context.Groups.FindAsync(groupId);
        if (group == null) return 0.0;  // Return 0 if group is not found

        var students = await _context.Students
            .Where(s => s.GroupId == groupId)
            .ToListAsync();

        // Calculate average of averages for the group and year
        var groupYearAverages = students
            .Select(s => s.Averages
                .Where(a => a.Year == group.Year)    // Filter averages by the group's year
                .Average(a => a.AverageGrade))
            .Where(avg => !double.IsNaN(avg))        // Filter out NaN values in case some students have no averages
            .ToList();

        return groupYearAverages.Any() ? groupYearAverages.Average() : 0.0;
    }
}
