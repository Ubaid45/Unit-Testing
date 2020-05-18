using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using TestNinja.Mocking;

namespace TestNinja.UnitTests.Mocking
{
    [TestFixture]
    public class HouseKeeperServiceTests
    {
        private HousekeeperService _service;
        private Mock<IStatementGenerator> _statementGenerator;
        private Mock<IEmailSender> _emailSender;
        private Mock<IXtraMessageBox> _messageBox;
        private readonly DateTime _statementDate =  new DateTime(2017, 1, 1);
        private Housekeeper _housekeeper;
        private readonly string _statementFileName = "fileName";

        [SetUp]
        public void SetUp()
        {
            _housekeeper = new Housekeeper
            {
                Email = "a", FullName = "b", Oid = 1, StatementEmailBody = "c"
            };
            var unitOfWork = new Mock<IUnitOfWork>();

            unitOfWork.Setup(uow => uow.Query<Housekeeper>())
                .Returns(new List<Housekeeper>
                {
                    _housekeeper
                }.AsQueryable());
            
            _statementGenerator = new Mock<IStatementGenerator>();
            _emailSender = new Mock<IEmailSender>();
            _messageBox = new Mock<IXtraMessageBox>();

            _service = new HousekeeperService(
                unitOfWork.Object,
                _statementGenerator.Object,
                _emailSender.Object,
                _messageBox.Object);
        }
        
        [Test]
        public void SendStatementEmails_WhenCalled_ShouldGenerateStatements()
        {
            _service.SendStatementEmails(_statementDate);
            
            _statementGenerator.Verify(sg => sg.SaveStatement(
                _housekeeper.Oid, _housekeeper.FullName, _statementDate));
        }
        
        [Test]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase(null)]
        public void SendStatementEmails_HousekeepersEmailIsNotSpecified_ShouldNotGenerateStatement(string input)
        {
            _housekeeper.Email = input;
            
            _service.SendStatementEmails(_statementDate);
            
            _statementGenerator.Verify(sg => sg.SaveStatement(
                _housekeeper.Oid, _housekeeper.FullName, _statementDate),
                Times.Never);
        }
        
        [Test]
        public void SendStatementEmails_WhenCalled_EmailTheStatement()
        {
            _statementGenerator
                .Setup(sg =>
                    sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate))
                .Returns(_statementFileName);
            
            _service.SendStatementEmails(_statementDate);

            _emailSender.Verify(es => es.EmailFile(
                _housekeeper.Email,
                _housekeeper.StatementEmailBody,
                _statementFileName,
                It.IsAny<String>()));
        }
        
        
        [Test]
        [TestCase("")]
        [TestCase("  ")]
        [TestCase(null)]
        public void SendStatementEmails_StatementFilenameIsInvalid_ShouldNotEmailStatement(string input)
        {
            _statementGenerator
                .Setup(sg =>
                    sg.SaveStatement(_housekeeper.Oid, _housekeeper.FullName, _statementDate))
                .Returns(() => input);
            
            _service.SendStatementEmails(_statementDate);

            _emailSender.Verify(es => es.EmailFile(
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<String>()),
                Times.Never());
        }
        
    }
}