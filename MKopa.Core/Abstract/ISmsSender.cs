using MKopa.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKopa.Core.Abstract
{
    public interface ISmsSender
    {
       Task<bool> SendSms(Sms model);
        bool SaveChanges();
    }
}
