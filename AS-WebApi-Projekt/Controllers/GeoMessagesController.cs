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
using Microsoft.AspNetCore.Authorization;
using AS_WebApi_Projekt.APIKey;

namespace AS_WebApi_Projekt.Controllers
{
    namespace v1
    {
        [ApiVersion("1.0")]
        [Route("api/v{version:apiVersion}")]
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
            public async Task<ActionResult<IEnumerable<V1GetDTO>>> GetGeoMessage()
            {
                List<V1GetDTO> V1List = new List<V1GetDTO>();
                List<GeoMessageV2> V2List = await _context.GeoMessageV2.Include(c => c.message).ToListAsync();
                foreach (var item in V2List)
                {
                    V1GetDTO geoMessage = new V1GetDTO
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
            public async Task<ActionResult<V1GetDTO>> GetGeoMessages(int id)
            {
                var geoMessages = await _context.GeoMessageV2.Include(c => c.message).FirstOrDefaultAsync(a => a.ID == id);
                var V1Model = new V1GetDTO
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
            [Authorize]
            [HttpPost("[action]")]
            public async Task <ActionResult<GeoMessageV2>> PostGeoMessages(V1GetDTO geoMessages)
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
                return CreatedAtAction("GetGeoMessages", new { id = V2Model.ID }, V2Model);
            }
        }
    }

    namespace v2
    {
        [ApiVersion("2.0")]
        [Route("api/v{version:apiVersion}")]
        [ApiController]
        public class GeoMessagesController : ControllerBase
        {
            private readonly AS_WebApi_ProjektContext _context;

            public GeoMessagesController(AS_WebApi_ProjektContext context)
            {
                _context = context;
            }
            /// <summary>
            ///  Begränsa din sökning geografiskt med hjälp utav våra parametrar.  
            /// </summary>
            /// <param name="minLon">Lägst Longituden</param>
            /// <param name="maxLon">Högsta Longituden</param>
            /// <param name="minLat">Lägsta Latituden</param>
            /// <param name="maxLat">Högsta Latituden</param>
            /// <returns>Du blir returnerad en lista med GeoComments inom ramen för dina valda parametrar.</returns>
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
                        latitude = item.latitude,
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

            /// <summary>
            ///  Sök efter GeoComments med ett visst ID. 
            /// </summary>
            /// <returns>Du blir returnerad en lista med GeoComments inom ramen för dina valda parametrar.</returns>
            [HttpGet("{id}")]
            public async Task<ActionResult<V2GetDTO>> GetGeoMessage(int id)
            {
                var geoMessage = await _context.GeoMessageV2.Include(a => a.message).FirstOrDefaultAsync(c => c.ID == id);
                if (geoMessage == null)
                    return NotFound();
                var geoMessageDto = new V2GetDTO
                {
                    message = new V2MessageDTO
                    {
                        author = geoMessage.message.author,
                        title = geoMessage.message.title,
                        body = geoMessage.message.body
                    },
                    longitude = geoMessage.longitude,
                    latitude = geoMessage.latitude
                };
                return Ok(geoMessageDto);
            }

            public async Task<ActionResult<GeoMessageV2>> PostGeoMessages(V2PostDTO geoMessagesDTO)
            {
                string token = Request.Headers[Constants.HttpHeaderField];
                if (token == null)
                    token = Request.Query[Constants.HttpQueryParamKey];
                var userApiDB = await _context.ApiTokens.FirstOrDefaultAsync(a => a.value == token);
                var userID = userApiDB.User;

                GeoMessageV2 geoMessagesV2 = new GeoMessageV2()
                {
                    message = new Message()
                    {
                        author = userApiDB.User.firstName + " " + userApiDB.User.lastName,
                        body = geoMessagesDTO.message.body,
                        title = geoMessagesDTO.message.title
                    },
                    latitude = geoMessagesDTO.latitude,
                    longitude = geoMessagesDTO.longitude
                };
                _context.GeoMessageV2.Add(geoMessagesV2);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetGeoMessages", new
                {
                    id = geoMessagesV2.ID
                }, geoMessagesV2);
            }
        }
    }
}
