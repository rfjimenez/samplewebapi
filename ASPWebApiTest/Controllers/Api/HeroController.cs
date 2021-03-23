using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASPWebApiTest;
using ASPWebApiTest.Data;

namespace ASPWebApiTest.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class HeroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HeroController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Hero or api/Hero?id=xxx
        [HttpGet]
        public async Task<IActionResult> GetHeroes(uint id = 0)
        {
            if (id == 0)
            {
                var list = await _context.Heroes.ToListAsync();
                return Ok(list);
            }

            var hero = await GetHero(id);
            return Ok(hero.Result);
        }

        // GET: api/Hero/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hero>> GetHero(uint id)
        {
            var hero = await _context.Heroes.FindAsync(id);

            if (hero == null)
            {
                return NotFound();
            }

            return Ok(hero);
        }

        // PUT: api/Hero/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHero(uint id, Hero hero)
        {
            if (id != hero.Id)
            {
                return BadRequest();
            }

            var existingHero = _context.Heroes.FirstOrDefault(x => x.Id == id);

            if (existingHero != null)
            {
                existingHero.Name = hero.Name;
                existingHero.Type = hero.Type;
                existingHero.BaseAgi = hero.BaseAgi;
                existingHero.BaseInt = hero.BaseInt;
                existingHero.BaseStr = hero.BaseStr;
            }
            
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HeroExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(existingHero);
        }

        // POST: api/Hero
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Hero>> PostHero(Hero hero)
        {
            _context.Heroes.Add(hero);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHero", new { id = hero.Id }, hero);
        }

        // DELETE: api/Hero/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Hero>> DeleteHero(uint id)
        {
            var hero = await _context.Heroes.FindAsync(id);
            if (hero == null)
            {
                return NotFound();
            }

            _context.Heroes.Remove(hero);
            await _context.SaveChangesAsync();

            return hero;
        }

        private bool HeroExists(uint id)
        {
            return _context.Heroes.Any(e => e.Id == id);
        }
    }
}
