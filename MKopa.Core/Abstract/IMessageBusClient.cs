using MKopa.Core.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKopa.Core.Abstract
{
    public interface IMessageBusClient
    {
        void PublishNewSendSMS(PublishedDto publishedDto);
       // void SubscribeToNewSendSMS(PublishedDto publishedDto);
    }
}
