using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Handlers;
using PaymentContext.Tests.Mocks;

namespace PaymentContext.Tests
{

[TestClass]
public class SubscriptionHandlerTests
    {
        [TestMethod]
        public void ShouldReturnErrorWhenDocumentExists()
        {    
           var handler = new SubscriptionHandler(new FakeStudentRepository(), new FakeEmailService());
           var command = new CreateBoletoSubscriptionCommand();
           command.FirstName = "Bruce";
           command.LastName= "Wayne";
           command.PaymentNumber= "99999999999";
           command.Address= "hellobalta.io2";
           command.BarCode= "123456789";
           command.BoletoNumber= "123456787";        
           command.PaidDate= DateTime.Now;
           command.ExpireDate= DateTime.Now.AddMonths(1);
           command.Total= 60;
           command.TotalPaid= 60;
          command.PayerDocument= "12345678911";
          command.PayerDocumentType = EDocumentType.CPF;
          command.PayerEmail  = "batman@dc.com";
          command.Payer = "WAYNE CORP";
          command.Street = "asdas";
          command.Number   = "asdd";
          command.Neighborhood = "asdasd";
          command.City   = "as";
          command.State  = "as";
          command.Country  = "as";
          command.ZipCode = "12345678";

          handler.Handle(command);
          Assert.AreEqual(false, handler.Valid);
        }
    }
}