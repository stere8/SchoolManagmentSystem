using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sms.backend.Data;
using sms.backend.Models;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class ParentController : ControllerBase
{
    private readonly SchoolContext _context;

    public ParentController(SchoolContext context)
    {
        _context = context;
    }

    [HttpGet("{parentId}")]
    public async Task<IActionResult> GetParentWithChildren(int parentId)
    {
        var parent = await _context.Parents
            .FirstOrDefaultAsync(p => p.ParentId == parentId);

        if (parent == null)
        {
            return NotFound("Parent not found");
        }

        var children = await _context.ParentChildAssignments
            .Where(pca => pca.ParentId == parentId)
            .Join(_context.Students,
                pca => pca.ChildId,
                s => s.StudentId,
                (pca, s) => s)
            .ToListAsync();

        var result = new
        {
            parent.ParentId,
            parent.FirstName,
            parent.LastName,
            Children = children.Select(c => new
            {
                c.StudentId,
                c.FirstName,
                c.LastName,
                c.GradeLevel
            })
        };

        return Ok(result);
    }
}