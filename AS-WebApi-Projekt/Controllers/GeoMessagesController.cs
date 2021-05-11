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
using AS_WebApi_Projekt.DTO;

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

            [HttpGet("[action]")]
            public async Task<ActionResult<IEnumerable<V2GetDTO>>> GetGeoMessages(
                double maxLon, 
                double minLon, 
                double maxLat, 
                double minLat)
            {
                var geoMessage = await _context.GeoMessageV2.Include(c => c.message).Where(
                    o => (o.longitude <= maxLon
                    && o.longitude >= minLon)
                    && (o.latitude <= maxLat
                    && o.latitude >= minLat)
                    )
                    .ToListAsync();
                List<V2GetDTO> GeoDTOList = new List<V2GetDTO>();
                foreach (var item in geoMessage)
                {
                    V2GetDTO geoDTO = new V2GetDTO()
                    {
                        latitute = item.latitude,
                        longitude = item.longitude,
                        message = new V2MessageDTO()
                        {
                            title = item.message.title,
                            body = item.message.body,
                            author = item.message.author
                        }
                    };
                    GeoDTOList.Add(geoDTO);
                }
                return GeoDTOList;
            }

        }
    }
}
