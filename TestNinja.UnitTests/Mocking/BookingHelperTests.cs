using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class BookingHelper_OverlappingBookingExist_Tests
    {
        private Booking _existingBooking;
        private Mock<IBookingRepoitory> _repository;

        [SetUp]
        public void SetUp()
        {
            _existingBooking = new Booking
            {
                ArrivalDate = ArriveOn(2017, 1, 10),
                DepartureDate = DepartOn(2017, 1, 14),
                Reference = "a"
            };
            
            _repository = new Mock<IBookingRepoitory>();
           
            _repository.Setup(r => r.GetActiveBooking(1)).Returns( new List<Booking>
            {
                _existingBooking
            }.AsQueryable());
        }
        
        [Test]
        public void BookingStartsBeforeAndFinishesInTheMiddleOfAnExistingBooking_ReturnExistingBooking()
        {
            var result = BookingHelper.OverlappingBookingsExist(
                new Booking
                {
                    Id = 1,
                    ArrivalDate = Before(_existingBooking.ArrivalDate),
                    DepartureDate = After(_existingBooking.ArrivalDate)
                }, _repository.Object);
            
            Assert.That(result, Is.EqualTo(_existingBooking.Reference));
        }

        [Test]
        public void BookingStartsAndFinishesBeforeAnExistingBooking_ReturnEmptyString()
        {
            var result = BookingHelper.OverlappingBookingsExist(
                new Booking
                {
                    Id = 1,
                    ArrivalDate = Before(_existingBooking.ArrivalDate, 2),
                    DepartureDate = Before(_existingBooking.ArrivalDate)
                }, _repository.Object);
            
            Assert.That(result, Is.Empty);
        }

        private DateTime Before(DateTime dateTime, int days = 1)
        {
            return dateTime.AddDays(-days);
        }
        
        private DateTime After(DateTime dateTime)
        {
            return dateTime.AddDays(1);
        }

        private DateTime ArriveOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 14, 0, 0);
        }
        
        private DateTime DepartOn(int year, int month, int day)
        {
            return new DateTime(year, month, day, 10, 0, 0);
        }
    }
}