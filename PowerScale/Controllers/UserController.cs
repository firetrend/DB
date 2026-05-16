using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerScale.Back;
using PowerScale.Back.Models;

namespace PowerScale.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _db;

    public UserController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/user
    // Получить всех пользователей
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _db.Users
            .Select(u => new
            {
                u.UserId,
                u.Nickname,
                u.Rep,
                u.Adminstatus,
                u.Email
            })
            .ToListAsync();

        return Ok(users);
    }

    // GET: api/user/5
    // Получить пользователя по ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { message = $"Пользователь с ID {id} не найден" });

        return Ok(user);
    }

    // POST: api/user
    // Добавить пользователя
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] UserCreateDto dto)
    {
        // Проверяем уникальность email
        var exists = await _db.Users.AnyAsync(u => u.Email == dto.Email);
        if (exists)
            return BadRequest(new { message = $"Email {dto.Email} уже используется" });

        var user = new User
        {
            Nickname = dto.Nickname,
            Email = dto.Email,
            Rep = dto.Rep ?? 0,
            Adminstatus = dto.Adminstatus ?? false
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = user.UserId }, user);
    }

    // PUT: api/user/5
    // Обновить пользователя
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserCreateDto dto)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { message = $"Пользователь с ID {id} не найден" });

        // Проверяем уникальность email (исключая текущего пользователя)
        var emailTaken = await _db.Users.AnyAsync(u => u.Email == dto.Email && u.UserId != id);
        if (emailTaken)
            return BadRequest(new { message = $"Email {dto.Email} уже используется" });

        user.Nickname = dto.Nickname;
        user.Email = dto.Email;
        user.Rep = dto.Rep ?? user.Rep;
        user.Adminstatus = dto.Adminstatus ?? user.Adminstatus;

        await _db.SaveChangesAsync();

        return Ok(new { message = $"Пользователь {user.Nickname} обновлён" });
    }

    // DELETE: api/user/5
    // Удалить пользователя
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _db.Users.FindAsync(id);
        if (user == null)
            return NotFound(new { message = $"Пользователь с ID {id} не найден" });

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();

        return Ok(new { message = $"Пользователь {user.Nickname} удалён" });
    }

    // GET: api/user/analytics/fights?nickname=Chiperlot
    // Аналитика: статистика боёв пользователя по никнейму
    [HttpGet("analytics/fights")]
    public async Task<IActionResult> GetFightStats([FromQuery] string nickname)
    {
        var user = await _db.Users
            .Where(u => u.Nickname == nickname)
            .FirstOrDefaultAsync();

        if (user == null)
            return NotFound(new { message = $"Пользователь '{nickname}' не найден" });

        var fights = await _db.Fights
            .Include(f => f.Character1)
            .Include(f => f.Character2)
            .Where(f => f.UserId == user.UserId)
            .Select(f => new
            {
                Fighter1 = f.Character1 != null ? f.Character1.Name : "Unknown",
                Fighter2 = f.Character2 != null ? f.Character2.Name : "Unknown",
                f.Fightexodus
            })
            .ToListAsync();

        var stats = new
        {
            Nickname = user.Nickname,
            Rep = user.Rep,
            TotalFights = fights.Count,
            Fights = fights
        };

        return Ok(stats);
    }
}

// DTO для создания/обновления пользователя
public class UserCreateDto
{
    public string Nickname { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int? Rep { get; set; }
    public bool? Adminstatus { get; set; }
}
