using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BasicEfCoreDemo.Data;
using BasicEfCoreDemo.Models;

namespace BasicEfCoreDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActorsController : ControllerBase
    {
        private readonly SampleDbContext _context;

        public ActorsController(SampleDbContext context)
        {
            _context = context;
        }

        // GET: api/Actors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Actor>>> GetActors()
        {
            return await _context.Actors.ToListAsync();
        }

        // GET: api/Actors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Actor>> GetActor(Guid id)
        {
            var actor = await _context.Actors.FindAsync(id);

            if (actor == null)
            {
                return NotFound();
            }

            return actor;
        }

        // PUT: api/Actors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutActor(Guid id, Actor actor)
        {
            if (id != actor.Id)
            {
                return BadRequest();
            }

            _context.Entry(actor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ActorExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Actors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Actor>> PostActor(Actor actor)
        {
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
        }

        // DELETE: api/Actors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteActor(Guid id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
            {
                return NotFound();
            }

            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("{id}/movies/{movieId}")]
        public async Task<IActionResult> AddMovie(Guid id, Guid movieId)
        {
            if (_context.Actors == null)
            {
                return NotFound("Actors is null.");
            }

            var actor = await _context.Actors.Include(x => x.Movies).SingleOrDefaultAsync(x => x.Id == id);
            if (actor == null)
            {
                return NotFound($"Actor with id {id} not found.");
            }

            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null)
            {
                return NotFound($"Movie with id {movieId} not found.");
            }

            if (actor.Movies.Any(x => x.Id == movie.Id))
            {
                return Problem($"Movie with id {movieId} already exists for Actor {id}.");
            }

            actor.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetActor", new { id = actor.Id }, actor);
        }

        [HttpDelete("{id}/movies/{movieId}")]
        public async Task<IActionResult> DeleteMovie(Guid id, Guid movieId)
        {
            if (_context.Actors == null)
            {
                return NotFound("Actors is null.");
            }

            var actor = await _context.Actors.Include(x => x.Movies).SingleOrDefaultAsync();
            if (actor == null)
            {
                return NotFound($"Actor with id {id} not found.");
            }

            var movie = await _context.Movies.FindAsync(movieId);
            if (movie == null)
            {
                return NotFound($"Movie with id {movieId} not found.");
            }

            actor.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("{id}/movies")]
        public async Task<IActionResult> GetMovies(Guid id)
        {
            if (_context.Actors == null)
            {
                return NotFound("Actors is null.");
            }

            var actor = await _context.Actors.Include(x => x.Movies).SingleOrDefaultAsync();
            if (actor == null)
            {
                return NotFound($"Actor with id {id} not found.");
            }

            return Ok(actor.Movies);
        }

        private bool ActorExists(Guid id)
        {
            return _context.Actors.Any(e => e.Id == id);
        }
    }
}
