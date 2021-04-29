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
        [HttpGet("/api/v1/geo-comments")]
        public async Task<ActionResult<IEnumerable<GeoMessage>>> GetGeoMessage()
        {
            return await _context.GeoMessage.ToListAsync();
        }

        // GET: api/GeoMessages/5
        [HttpGet("/api/v1/geo-comments/{id}")]
        public async Task<ActionResult<GeoMessage>> GetGeoMessage(int id)
        {
            var geoMessage = await _context.GeoMessage.FindAsync(id);

            if (geoMessage == null)
            {
                return NotFound();
            }

            return geoMessage;
        }

        // POST: api/GeoMessages
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("/api/v1/geo-comments")]
        public async Task<ActionResult<GeoMessage>> PostGeoMessage(GeoMessage geoMessage)
        {
            _context.GeoMessage.Add(geoMessage);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGeoMessage", new { id = geoMessage.ID }, geoMessage);
        }

        private bool GeoMessageExists(int id)
        {
            return _context.GeoMessage.Any(e => e.ID == id);
        }
    }
}
