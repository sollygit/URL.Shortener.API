﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using URL.Shortener.Interface;

namespace URL.Shortener.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class URLShortenerController : ControllerBase
    {
        private readonly IShortenedUrlService service;

        public URLShortenerController(IShortenedUrlService service)
        {
            this.service = service;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllAsync()
        {
            var items = await service.GetAllAsync();
            return new OkObjectResult(items);
        }

        [HttpGet("protected")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAllProtectedAsync()
        {
            var items = await service.GetAllAsync();
            return new OkObjectResult(items);
        }

        [HttpGet("secured")]
        [Authorize("read:messages")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> GetAllSecuredAsync()
        {
            var items = await service.GetAllAsync();
            return new OkObjectResult(items);
        }

        [HttpPost()]
        public async Task<IActionResult> CreateAsync([FromBody] string url)
        {
            try
            {
                var item = await service.CreateAsync(url);
                return new OkObjectResult(item);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        [HttpGet("{code}")]
        public async Task<IActionResult> GetAsync([FromRoute] string code)
        {
            var item = await service.GetAsync(code);
            if (item == null) return new NotFoundObjectResult(code);

            return new OkObjectResult(item);
        }
    }
}