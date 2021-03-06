using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AS_WebApi_Projekt.Data;
using AS_WebApi_Projekt.Models;
using AS_WebApi_Projekt.DTO;
using Microsoft.AspNetCore.Authorization;
using AS_WebApi_Projekt.APIKey;
using Microsoft.AspNetCore.Identity;

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

            /// <summary>
            ///  Hämta alla GeoMessages som finns. 
            /// </summary>
            /// <returns>
            /// Du blir returnerad en lista med GeoMessages.
            /// </returns>
            //GET: api/GeoMessages
            [HttpGet("[action]")]
            public async Task<ActionResult<IEnumerable<V1GetDTO>>> GetGeoMessage()
            {
                List<V1GetDTO> V1List = new();
                List<GeoMessageV2> V2List = await _context.GeoMessageV2.Include(c => c.Message).ToListAsync();
                foreach (var item in V2List)
                {
                    V1GetDTO geoMessage = new()
                    {
                        Message = item.Message.Body,
                        Latitude = item.Latitude,
                        Longitude = item.Longitude,
                    };
                    V1List.Add(geoMessage);
                }
                return V1List;
            }

            /// <summary>
            ///  Sök efter GeoMessages med ett specifikt ID.  
            /// </summary>
            /// <returns>
            /// Du blir returnerad en lista med GeoMessages baserat på det specificerade ID:t. 
            /// </returns>
            //GET: api/GeoMessages/5
            [HttpGet("[action]/{id}")]
            public async Task<ActionResult<V1GetDTO>> GetGeoMessages(int id)
            {
                var geoMessages = await _context.GeoMessageV2.Include(c => c.Message).FirstOrDefaultAsync(a => a.ID == id);
                var V1Model = new V1GetDTO
                {
                    Message = geoMessages.Message.Body,
                    Longitude = geoMessages.Longitude,
                    Latitude = geoMessages.Latitude
                };
                if (geoMessages == null)
                {
                    return NotFound();
                }
                return V1Model;
            }

            /// <summary>
            ///  Posta GeoMessages (ifall du har tillstånd). 
            /// </summary>
            //POST: api/GeoMessages
            [Authorize]
            [HttpPost("[action]")]
            public async Task <ActionResult<GeoMessageV2>> PostGeoMessages(V1GetDTO geoMessages)
            {
                var V2Model = new GeoMessageV2
                {
                    Message = new Message
                    {
                        Body = geoMessages.Message
                    },
                    Longitude = geoMessages.Longitude,
                    Latitude = geoMessages.Latitude
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
            private readonly UserManager<IdentityUser> _userManager;

            public GeoMessagesController(AS_WebApi_ProjektContext context, UserManager<IdentityUser> userManager)
            {
                _context = context;
                _userManager = userManager;
            }
            /// <summary>
            ///  Begränsa din sökning geografiskt med hjälp utav parametrar.  
            /// </summary>
            /// <param name="minLon">Lägsta Longituden</param>
            /// <param name="maxLon">Högsta Longituden</param>
            /// <param name="minLat">Lägsta Latituden</param>
            /// <param name="maxLat">Högsta Latituden</param>
            /// <returns>
            /// Du blir returnerad en lista med GeoMessages inom ramen för dina valda parametrar.
            /// </returns>
            [HttpGet("[action]")]
            public async Task<ActionResult<IEnumerable<V2GetDTO>>> GetGeoMessages(
                double maxLon, 
                double minLon, 
                double maxLat, 
                double minLat)
            {
                var geoMessage = await _context.GeoMessageV2.Include(c => c.Message).Where(
                    o => (o.Longitude <= maxLon
                    && o.Longitude >= minLon)
                    && (o.Latitude <= maxLat
                    && o.Latitude >= minLat)
                    )
                    .ToListAsync();
                List<V2GetDTO> GeoDTOList = new();
                foreach (var item in geoMessage)
                {
                    V2GetDTO geoDTO = new()
                    {
                        Latitude = item.Latitude,
                        Longitude = item.Longitude,
                        Message = new V2MessageDTO()
                        {
                            Title = item.Message.Title,
                            Body = item.Message.Body,
                            Author = item.Message.Author
                        }
                    };
                    GeoDTOList.Add(geoDTO);
                }
                return GeoDTOList;
            }

            /// <summary>
            ///  Sök efter GeoMessages med ett specifikt ID. 
            /// </summary>
            /// <returns>
            /// Du blir returnerad en lista med GeoMessages baserat på det specificerade ID:t.
            /// </returns>
            [HttpGet("{id}")]
            public async Task<ActionResult<V2GetDTO>> GetGeoMessage(int id)
            {
                var geoMessage = await _context.GeoMessageV2.Include(a => a.Message).FirstOrDefaultAsync(c => c.ID == id);
                if (geoMessage == null)
                    return NotFound();
                var geoMessageDto = new V2GetDTO
                {
                    Message = new V2MessageDTO
                    {
                        Author = geoMessage.Message.Author,
                        Title = geoMessage.Message.Title,
                        Body = geoMessage.Message.Body
                    },
                    Longitude = geoMessage.Longitude,
                    Latitude = geoMessage.Latitude
                };
                return Ok(geoMessageDto);
            }

            /// <summary>
            ///  Posta GeoMessages (ifall du har tillstånd). 
            /// </summary>
            //POST: api/GeoMessages
            [Authorize]
            [HttpPost("[action]")]
            public async Task<ActionResult<GeoMessageV2>> PostGeoMessages(V2PostDTO geoMessagesDTO, string ApiKey)
            {
                var user = await _context.User.FindAsync(_userManager.GetUserId(User));

                GeoMessageV2 geoMessagesV2 = new()
                {
                    Message = new Message()
                    {
                        Author = user.FirstName + " " + user.LastName,
                        Body = geoMessagesDTO.Message.Body,
                        Title = geoMessagesDTO.Message.Title
                    },
                    Latitude = geoMessagesDTO.Latitude,
                    Longitude = geoMessagesDTO.Longitude
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
