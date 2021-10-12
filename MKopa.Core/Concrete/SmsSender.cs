using Microsoft.Extensions.Configuration;
using MKopa.Core.Abstract;
using MKopa.Core.Data;
using MKopa.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace MKopa.Core.Concrete
{
    public class SmsSender : ISmsSender
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;

        public SmsSender(HttpClient httpClient, IConfiguration configuration, AppDbContext context)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
            // Find your Account SID and Auth Token at twilio.com/console
            // and set the environment variables. See http://twil.io/secure
            string accountSid = configuration["TWILIO_ACCOUNT_SID"];
            string authToken = configuration["TWILIO_AUTH_TOKEN"];

            TwilioClient.Init(accountSid, authToken);

           

        }
        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public async Task<bool> SendSms(Sms model)
        {
            if(model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var httpContent = new StringContent(JsonSerializer.Serialize(model),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync(_configuration["SmsUri"], httpContent);

            if (response.IsSuccessStatusCode)
            {
               
                return true;
            }
            else
            {
                Console.WriteLine("--> sync call to send sms was Not Ok");
                return false;
            }


        }
    }
}
