using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Web.Data;
using TaskTracker.Web.Models;

namespace TaskTracker.Web.Pages.Tasks;

public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db) => _db = db;

    public IList<TaskItem> Tasks { get; set; } = new List<TaskItem>();

    public async Task OnGetAsync()
    {
        Tasks = await _db.TaskItems
            .AsNoTracking()
            .OrderBy(t => t.IsDone)
            .ThenByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .ThenByDescending(t => t.CreatedAt)
            .ToListAsync();
    }

    public async Task<IActionResult> OnPostToggleDoneAsync(int id)
    {
        var task = await _db.TaskItems.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null) return RedirectToPage();

        task.IsDone = !task.IsDone;
        await _db.SaveChangesAsync();
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        var task = await _db.TaskItems.FirstOrDefaultAsync(t => t.Id == id);
        if (task is null) return RedirectToPage();

        _db.TaskItems.Remove(task);
        await _db.SaveChangesAsync();
        return RedirectToPage();
    }
}
