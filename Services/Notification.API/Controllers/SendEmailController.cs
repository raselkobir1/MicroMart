﻿using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Notification.API.Domain.Dto.Common;
using Notification.API.Manager.Interfaces;

namespace Notification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SendEmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        public SendEmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }
        [HttpPost("SendWithAttachment")]
        public async Task<IActionResult> SendEmailNotification(EmailDto dto)
        {
            var response = await _emailService.SendEmailAsync(dto);
            return StatusCode(200, response);
        }
        [HttpPost("Send")]
        public async Task<IActionResult> SendEmailNotification(EmailwithoutAttachmentDto dto)
        {
            var response = await _emailService.SendEmailAsync(dto.Adapt(new EmailDto()));
            return StatusCode(200, response);
        }
    }
}
