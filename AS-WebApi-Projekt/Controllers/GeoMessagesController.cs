using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AS_WebApi_Projekt.Data;
using AS_WebApi_Projekt.Models;
using AS_WebApi_Projekt.Models.v2;

namespace AS_WebApi_Projekt.Controllers
{
    namespace v1
    {
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}/geo-comments")]
        [ApiController]
        public class GeoMessagesController : ControllerBase
        {
            private readonly AS_WebApi_ProjektContext _context;

            public GeoMessagesController(AS_WebApi_ProjektContext context)
            {
                _context = context;
            }

            //GET: api/GeoMessages
            [HttpGet("action")]
            public async Task<ActionResult<IEnumerable<GeoMessageV1>>> GetGeoMessage()
            {
                List<GeoMessageV1> V1List = new List<GeoMessageV1>();
                List<GeoMessageV2> V2List = await _context.GeoMessageV2.Include(c => c.message).ToListAsync();
                foreach (var item in V2List)
                {
                    GeoMessageV1 geoMessage = new GeoMessageV1
                    {
                        message = item.message.body,
                        latitude = item.latitude,
                        longitude = item.longitude,
                    };
                    V1List.Add(geoMessage);
                }
                return V1List;
            }


            //GET: api/GeoMessages/5
            [HttpGet("[action]/{id}")]
            public async Task<ActionResult<GeoMessageV1>> GetGeoMessages(int id)
            {
                var geoMessages = await _context.GeoMessageV2.Include(c => c.message).FirstOrDefaultAsync(a => a.ID == id);
                var V1Model = new GeoMessageV1
                {
                    message = geoMessages.message.body,
                    longitude = geoMessages.longitude,
                    latitude = geoMessages.latitude
                };
                if (geoMessages == null)
                {
                    return NotFound();
                }
                return V1Model;
            }
                
            // POST: api/GeoMessages
            //[Authorize]
            [HttpPost("[action]")]
            public async Task <ActionResult<GeoMessageV1>> PostGeoMessages(GeoMessageV1 geoMessages)
            {
                var V2Model = new GeoMessageV2
                {
                    message = new Message
                    {
                        body = geoMessages.message
                    },
                    longitude = geoMessages.longitude,
                    latitude = geoMessages.latitude
                };
                _context.GeoMessageV2.Add(V2Model);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetGeoMessages", new { id = geoMessages.ID }, geoMessages);
            }
        }
    }


    namespace v2
    {
        [ApiVersion("2.0")]
        [Route("api/v{version:apiVersion}/[controller]")]
        [ApiController]
        public class GeoMessagesController : ControllerBase
        {
            private readonly AS_WebApi_ProjektContext _context;

            public GeoMessagesController(AS_WebApi_ProjektContext context)
            {
                _context = context;
            }

            // GET: api/GeoMessages
            [HttpGet("/api/v2/geo-comments")]
            public async Task<ActionResult<IEnumerable<GeoMessageV2>>> GetGeoMessage()
            {
                return await _context.GeoMessageV2.ToListAsync();
            }

            // POST: api/GeoMessages
            [HttpPost("/api/v2/geo-comments")]
            public async Task<ActionResult<GeoMessageV2>> PostGeoMessage(GeoMessageV2 geoMessage)
            {
                _context.GeoMessageV2.Add(geoMessage);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGeoMessage", new { id = geoMessage.ID }, geoMessage);
            }

            // GET: api/GeoMessages/5
            [HttpGet("/api/v2/geo-comments/{id}")]
            public async Task<ActionResult<GeoMessageV2>> GetGeoMessage(int id)
            {
                var geoMessage = await _context.GeoMessageV2.FindAsync(id);

                if (geoMessage == null)
                {
                    return NotFound();
                }

                return geoMessage;
            }

            private bool GeoMessageExists(int id)
            {
                return _context.GeoMessageV2.Any(e => e.ID == id);
            }
        }
    }
}
