using Microsoft.AspNetCore.Mvc;
using WebApiCachingApp.Services;
using WebApiCachingApp.Data;
using WebApiCachingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace WebApiCachingApp.Controllers;
[ApiController]
[Route("[controller]")]
public class DriversController : ControllerBase
{
    private readonly ICacheService _cacheService;
    private readonly ApiDbContext _dbContext;

    public DriversController(ICacheService cacheService, ApiDbContext dbContext)
    {
        _cacheService = cacheService;
        _dbContext = dbContext;
    }

    [HttpGet("drivers")]
    public async Task<IActionResult> Get()
    {
        var cacheDrivers = _cacheService.GetData<IEnumerable<Driver>>("drivers");
        if(cacheDrivers != null && cacheDrivers.Count() > 0)
            return Ok(cacheDrivers);

        var drivers = await _dbContext.Drivers.ToListAsync();
        var expiryTime = DateTimeOffset.Now.AddMinutes(2);
        _cacheService.SetData<IEnumerable<Driver>>("drivers",drivers,expiryTime);
        return Ok(drivers);
    }

    [HttpPost]
    public async Task<IActionResult> Post(Driver newDriver)
    {
        await _dbContext.Drivers.AddAsync(newDriver);
        await _dbContext.SaveChangesAsync();
        var res = _cacheService.RemoveData("driver");
        return Ok(newDriver);

    }

    
}