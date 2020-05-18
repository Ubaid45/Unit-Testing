using System.Linq;

namespace TestNinja.Mocking
{
    public interface IBookingRepoitory
    {
        IQueryable<Booking> GetActiveBooking(int? excludedBookingId = null);
    }

    public class BookingRepoitory : IBookingRepoitory
    {
        public IQueryable<Booking> GetActiveBooking(int? excludedBookingId = null)
        {
            var unitOfWork = new UnitOfWork();
            var bookings = 
                unitOfWork.Query<Booking>().Where(b => b.Status != "Cancelled");
            if (excludedBookingId.HasValue)
                bookings = bookings.Where(b => b.Id != excludedBookingId.Value);

            return bookings;
        }
    }
}