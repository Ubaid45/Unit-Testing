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

        [TestMethod]
        public void CanBeCanelledBy_SameUserCancellingTheReservation_ReturnsTrue()
        {
            var user = new User();
            var reservation = new Reservation { MadeBy = user };
            var result = reservation.CanBeCancelledBy(user);

            Assert.IsTrue(result);

        }

        [TestMethod]
        public void CanBeCanelledBy_AnotherUserCancellingReservation_ReturnsTrue()
        {
            var reservation = new Reservation { MadeBy = new User() };
            var result = reservation.CanBeCancelledBy(new User());

            Assert.IsFalse(result);

        }
    }
}
