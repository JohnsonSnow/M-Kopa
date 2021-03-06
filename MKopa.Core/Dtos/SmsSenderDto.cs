using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKopa.Core.Dtos
{
    public class SmsSenderDto
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string TextMessage { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool sent { get; set; }
    }
}
