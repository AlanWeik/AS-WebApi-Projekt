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
    namespace v1
    {
        [ApiVersion("1.0")]
        [Route("api/v1/[controller]")]
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

            // POST: api/GeoMessages
            [HttpPost("/api/v1/geo-comments")]
            public async Task<ActionResult<GeoMessage>> PostGeoMessage(GeoMessage geoMessage)
            {
                _context.GeoMessage.Add(geoMessage);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGeoMessage", new { id = geoMessage.ID }, geoMessage);
            }
        }
    }

    namespace v2
    {
        [ApiVersion("2.0")]
        [Route("api/v2/[controller]")]
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

            // POST: api/GeoMessages
            [HttpPost("/api/v1/geo-comments")]
            public async Task<ActionResult<GeoMessage>> PostGeoMessage(GeoMessage geoMessage)
            {
                _context.GeoMessage.Add(geoMessage);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGeoMessage", new { id = geoMessage.ID }, geoMessage);
            }

            // GET: api/GeoMessages/5
            [HttpGet("/api/v2/geo-comments/{id}")]
            public async Task<ActionResult<GeoMessage>> GetGeoMessage(int id)
            {
                var geoMessage = await _context.GeoMessage.FindAsync(id);

                if (geoMessage == null)
                {
                    return NotFound();
                }

                return geoMessage;
            }

            private bool GeoMessageExists(int id)
            {
                return _context.GeoMessage.Any(e => e.ID == id);
            }
        }
    }
}