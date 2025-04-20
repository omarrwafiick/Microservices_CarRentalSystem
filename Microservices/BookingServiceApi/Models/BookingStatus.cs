using Common.Interfaces;

namespace BookingServiceApi.Models
{
    public class BookingStatus: IBaseEntity
    {
        public Guid Id { get; set; }
        public string Status { get; set; }
    }
}
