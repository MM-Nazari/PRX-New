using System.ComponentModel.DataAnnotations;

namespace PRX.Dto.User
{
    public class RequestDto
    {
        
        public int UserId { get; set; }
        public string RequestType { get; set; }


        public string TrackingCode { get; set; }

        public DateTime RequestSentTime { get; set; }

        public string BeneficiaryName { get; set; } // نام ذی نفع

        public string RequestState { get; set; }


        public bool IsComplete { get; set; } = false;
        public bool IsDeleted { get; set; } = false;
    }
}
