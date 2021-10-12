using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MKopa.Core.Abstract;
using MKopa.Core.Dtos;
using MKopa.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MKopaService.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SendSmsController : ControllerBase
    {
        private readonly ISmsSender _smsSenderRepo;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public SendSmsController(ISmsSender smsSenderRepo, IMapper mapper, IMessageBusClient messageBusClient)
        {
            _smsSenderRepo = smsSenderRepo;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }

        [HttpPost]
        public ActionResult SendSms(SmsSenderDto smsSenderDto)
        {
            // Send SmS
            try
            {
                Console.WriteLine("--> Send Sms..");
                var smsModel = _mapper.Map<Sms>(smsSenderDto);
                _smsSenderRepo.SendSms(smsModel);
                _smsSenderRepo.SaveChanges();


            }
            catch (Exception ex)
            {

                throw;
            }


            // send Async Message to Message Bus 
            try
            {
                var publisedDto = _mapper.Map<PublishedDto>(smsSenderDto);
                _messageBusClient.PublishNewSendSMS(publisedDto);
            }
            catch (Exception ex)
            {

                Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
            }

            return Ok();
        }
    }
}
