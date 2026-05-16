using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PowerScale.Back;
using PowerScale.Back.Models;

namespace PowerScale.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CharacterController : ControllerBase
{
    private readonly AppDbContext _db;

    public CharacterController(AppDbContext db)
    {
        _db = db;
    }

    // GET: api/character
    // Получить всех персонажей с названием вселенной (JOIN)
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var characters = await _db.Characters
            .Include(c => c.Universe)
            .Select(c => new
            {
                c.CharacterId,
                c.Name,
                c.Age,
                c.Powerscale,
                c.Skills,
                c.Description,
                UniverseName = c.Universe != null ? c.Universe.Name : "Unknown"
            })
            .ToListAsync();

        return Ok(characters);
    }

    // GET: api/character/5
    // Получить одного персонажа по ID с полной информацией
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var character = await _db.Characters
            .Include(c => c.Universe)
            .Where(c => c.CharacterId == id)
            .Select(c => new
            {
                c.CharacterId,
                c.Name,
                c.Age,
                c.Powerscale,
                c.Skills,
                c.Description,
                UniverseName = c.Universe != null ? c.Universe.Name : "Unknown",
                UniverseType = c.Universe != null ? c.Universe.Type : "Unknown"
            })
            .FirstOrDefaultAsync();

        if (character == null)
            return NotFound(new { message = $"Персонаж с ID {id} не найден" });

        return Ok(character);
    }

    // POST: api/character
    // Добавить нового персонажа
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CharacterCreateDto dto)
    {
        // Проверяем что вселенная существует
        var universe = await _db.Universes.FindAsync(dto.UniverseId);
        if (universe == null)
            return BadRequest(new { message = $"Вселенная с ID {dto.UniverseId} не найдена" });

        var character = new Character
        {
            Name = dto.Name,
            Age = dto.Age,
            Powerscale = dto.Powerscale,
            Skills = dto.Skills,
            Description = dto.Description,
            UniverseId = dto.UniverseId
        };

        _db.Characters.Add(character);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = character.CharacterId }, new
        {
            character.CharacterId,
            character.Name,
            UniverseName = universe.Name
        });
    }

    // PUT: api/character/5
    // Обновить персонажа
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] CharacterCreateDto dto)
    {
        var character = await _db.Characters.FindAsync(id);
        if (character == null)
            return NotFound(new { message = $"Персонаж с ID {id} не найден" });

        var universe = await _db.Universes.FindAsync(dto.UniverseId);
        if (universe == null)
            return BadRequest(new { message = $"Вселенная с ID {dto.UniverseId} не найдена" });

        character.Name = dto.Name;
        character.Age = dto.Age;
        character.Powerscale = dto.Powerscale;
        character.Skills = dto.Skills;
        character.Description = dto.Description;
        character.UniverseId = dto.UniverseId;

        await _db.SaveChangesAsync();

        return Ok(new { message = $"Персонаж {character.Name} обновлён" });
    }

    // DELETE: api/character/5
    // Удалить персонажа
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var character = await _db.Characters.FindAsync(id);
        if (character == null)
            return NotFound(new { message = $"Персонаж с ID {id} не найден" });

        _db.Characters.Remove(character);
        await _db.SaveChangesAsync();

        return Ok(new { message = $"Персонаж {character.Name} удалён" });
    }

    // GET: api/character/analytics/top?universeName=Marvel&limit=10
    // Аналитика: топ персонажей по силе с фильтром по вселенной
    [HttpGet("analytics/top")]
    public async Task<IActionResult> GetTopByUniverse(
        [FromQuery] string universeName,
        [FromQuery] int limit = 10)
    {
        var result = await _db.Characters
            .Include(c => c.Universe)
            .Where(c => c.Universe != null && c.Universe.Name == universeName)
            .OrderByDescending(c => c.Powerscale)
            .Take(limit)
            .Select(c => new
            {
                c.Name,
                c.Powerscale,
                c.Age,
                UniverseName = c.Universe!.Name
            })
            .ToListAsync();

        if (!result.Any())
            return NotFound(new { message = $"Персонажи из вселенной '{universeName}' не найдены" });

        return Ok(result);
    }
}

// DTO для создания/обновления персонажа
public class CharacterCreateDto
{
    public string Name { get; set; } = string.Empty;
    public int? Age { get; set; }
    public int? Powerscale { get; set; }
    public string? Skills { get; set; }
    public string? Description { get; set; }
    public int? UniverseId { get; set; }
}
