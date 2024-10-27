using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers;

public class ActivitiesController : BaseApiController
{
    private readonly DataContext _context;

    public ActivitiesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Activity>>>GetActivities()
    {
        return await _context.Activities.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Activity>>GetActivity(Guid id)
    {
        return await _context.Activities.FindAsync(id);
    }

    [HttpPut("{Id}")]
    public async Task<ActionResult<int>>UpdateActivity(Guid Id, Activity activityDTO)
    {
        if(Id != activityDTO.Id)
        {
            return BadRequest();
        }

        var activity = await _context.Activities.FindAsync(Id);

        if(activity is null)
        {
            return NotFound();
        }

        activity.Title = activityDTO.Title;
        activity.Date = activityDTO.Date;
        activity.Description = activityDTO.Description;
        activity.Category = activityDTO.Category;
        activity.City = activityDTO.City;
        activity.Venue = activityDTO.Venue;

        try
        {
          await _context.SaveChangesAsync();          
        }
        catch (DbUpdateConcurrencyException)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost]
    public async Task<ActionResult<int>>AddActivity(Activity activity)
    {
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();

        return Ok();
    }

    [HttpDelete("{Id}")]
    public async Task<IActionResult>RemoveActivity(int Id)
    {
        var activity = await _context.Activities.FindAsync(Id);

        if(activity is null)
        {
            return NotFound();
        }

        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
