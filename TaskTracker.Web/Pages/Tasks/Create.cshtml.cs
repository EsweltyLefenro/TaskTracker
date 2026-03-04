using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TaskTracker.Web.Data;
using TaskTracker.Web.Models;

namespace TaskTracker.Web.Pages.Tasks;

public class CreateModel : PageModel
{
    private readonly AppDbContext _db;

    public CreateModel(AppDbContext db) => _db = db;

    [BindProperty]
    public TaskItem Task { get; set; } = new();

    public void OnGet()
    {
        Task.Priority = 1;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        Task.CreatedAt = DateTime.UtcNow;
        _db.TaskItems.Add(Task);
        await _db.SaveChangesAsync();

        return RedirectToPage("Index");
    }
}