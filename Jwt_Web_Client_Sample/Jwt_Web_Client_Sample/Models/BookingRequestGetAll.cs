using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jwt_Web_Client_Sample.Models
{
    public class BookingRequestGetAll
    {
        public int BookingRequestId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public short CaptainPolicyId { get; set; }
        public short GuestCount { get; set; }
        public string Message { get; set; }
        public string Phone { get; set; }
        public DateTime RequestDate { get; set; }
        //public BoatGetModel Boat { get; set; }
        //public UserGetModel User { get; set; }
    }


}
