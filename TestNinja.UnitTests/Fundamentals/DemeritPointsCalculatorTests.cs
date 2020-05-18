using System;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests
{
    [TestFixture]
    public class DemeritPointsCalculatorTests
    {
        private DemeritPointsCalculator _calculator;
        
        [SetUp]
        public void Setup()
        {
            _calculator = new DemeritPointsCalculator();
        }
        [Test]
        [TestCase(-1)]
        [TestCase(-10)]
        [TestCase(301)]
        [TestCase(400)]
        public void CalculateDemeritPoints_SpeedIsOutOfRange_ThrowArgumentOutOfRangeException(int speed)
        {
            Assert.That(() => _calculator.CalculateDemeritPoints(speed),
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }

        [Test]
        [TestCase(0, 0)]
        [TestCase(64, 0)]
        [TestCase(65, 0)]
        [TestCase(66, 0)]
        [TestCase(70, 1)]
        [TestCase(75, 2)]
        [TestCase(80, 3)]
        public void CalculateDemeritPoints_WhenCalled_ReturnDemeriPoints(int speed, int expectedResult)
        {
           var points = _calculator.CalculateDemeritPoints(speed);
           Assert.That(points, Is.EqualTo(expectedResult));
        }

    }
}