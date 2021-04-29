using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AS_WebApi_Projekt.Data;
using AS_WebApi_Projekt.Models;

namespace AS_WebApi_Projekt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GeoMessagesController : ControllerBase
    {
        private readonly AS_WebApi_ProjektContext _context;

        public GeoMessagesController(AS_WebApi_ProjektContext context)
        {
            _context = context;
        }

        // GET: api/GeoMessages
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GeoMessage>>> GetGeoMessage()
        {
            return await _context.GeoMessage.ToListAsync();
        }

        // GET: api/GeoMessages/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GeoMessage>> GetGeoMessage(int id)
        {
            var geoMessage = await _context.GeoMessage.FindAsync(id);

            if (geoMessage == null)
            {
                return NotFound();
            }

            return geoMessage;
        }

        // PUT: api/GeoMessages/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGeoMessage(int id, GeoMessage geoMessage)
        {
            if (id != geoMessage.ID)
            {
                return BadRequest();
            }

            _context.Entry(geoMessage).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GeoMessageExists(id))
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

        // POST: api/GeoMessages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GeoMessage>> PostGeoMessage(GeoMessage geoMessage)
        {
            _context.GeoMessage.Add(geoMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoMessage", new { id = geoMessage.ID }, geoMessage);
        }

        // DELETE: api/GeoMessages/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGeoMessage(int id)
        {
            var geoMessage = await _context.GeoMessage.FindAsync(id);
            if (geoMessage == null)
            {
                return NotFound();
            }

            _context.GeoMessage.Remove(geoMessage);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GeoMessageExists(int id)
        {
            return _context.GeoMessage.Any(e => e.ID == id);
        }
    }
}
