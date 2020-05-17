using Microsoft.VisualStudio.TestTools.UnitTesting;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestClass]
    public class ReservationTests
    {
        [TestMethod]
        public void CanBeCanelledBy_UserIsAdmin_ReturnsTrue()
        {
            // 1 - Arrange

            var reservation = new Reservation();

            // 2 - Act

            var result = reservation.CanBeCancelledBy(new User { IsAdmin = true });

            // 3 - Assert

            Assert.IsTrue(result);
        }
    }
}
