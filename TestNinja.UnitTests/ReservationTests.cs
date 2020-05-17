using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class ReservationTests
    {
        [Test]
        public void CanBeCanelledBy_UserIsAdmin_ReturnsTrue()
        {
            // 1 - Arrange

            var reservation = new Reservation();

            // 2 - Act

            var result = reservation.CanBeCancelledBy(new User { IsAdmin = true });

            // 3 - Assert

            Assert.That(result, Is.True);
        }

        [Test]
        public void CanBeCanelledBy_SameUserCancellingTheReservation_ReturnsTrue()
        {
            var user = new User();
            var reservation = new Reservation { MadeBy = user };
            var result = reservation.CanBeCancelledBy(user);

            Assert.That(result, Is.True);

        }

        [Test]
        public void CanBeCanelledBy_AnotherUserCancellingReservation_ReturnsFalse()
        {
            var reservation = new Reservation { MadeBy = new User() };
            var result = reservation.CanBeCancelledBy(new User());

            Assert.That(result, Is.False);


        }
    }
}
