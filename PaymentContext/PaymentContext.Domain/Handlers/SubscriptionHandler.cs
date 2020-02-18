using System;
using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Domain.Commands;
using PaymentContext.Domain.Entities;
using PaymentContext.Domain.Enums;
using PaymentContext.Domain.Repositories;
using PaymentContext.Domain.Services;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Commands;
using PaymentContext.Shared.Handlers;

namespace PaymentContext.Domain.Handlers
{
    public class SubscriptionHandler :
     Notifiable,
     IHandler<CreateBoletoSubscriptionCommand>,
     IHandler<CreatePayPalSubscriptionCommand>
    {

        private readonly IStudentRepository _repository;
        private readonly IEmailService _emailService;

        public SubscriptionHandler(IStudentRepository repository, IEmailService emailservice)
        {
            _repository = repository;
            _emailService = emailservice;
        }
        public ICommandResult Handle(CreateBoletoSubscriptionCommand command)
        {
            //Fail Fast Validations
            command.Validate();
            if(command.Invalid){
                AddNotifications(command);
                return new CommandResult(false, "Não foi possível realizar sua ssinatura");
            }
            // Verificar se documento está cadastrado
            if(_repository.DocumentExists(command.PayerDocument))
            {
                AddNotifications(new Contract());
                return new CommandResult(false, "Esse CPF já está em uso");
            }
            //Verificar se E-mail já está cadastrado
            if(_repository.EmailExists(command.PayerEmail))
            {
                AddNotifications(new Contract());
                return new CommandResult(false, "Esse E-mail já está em uso");
            }
            //Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
           var doc = new Document(command.PayerDocument, EDocumentType.CPF);
           var email = new Email(command.PayerEmail);
           var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);
            
            //Gerar as Entidades
            var student = new Student(name, doc, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new BoletoPayment(
                command.BarCode, 
                command.BoletoNumber, 
                command.PaidDate, 
                command.ExpireDate, 
                command.Total, 
                command.TotalPaid, 
                command.Payer, 
                new Document(command.PayerDocument, command.PayerDocumentType), 
                address, 
                email);

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupar as Validações
            AddNotifications(name, doc, email, address, student, subscription, payment);

            //Checar as notificações
            if(Invalid)
            {
                return new CommandResult(false, "Assinatura realizada com sucesso");
            }

            //Salvar as Informações
            _repository.CreateSubscription(student);

            //Enviar E-mail de boas vindas
            _emailService.Send(student.Name.FirstName, student.Email.Address, "Bem vindo", "Sua assinatura foi criada");
            
            //Retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }

        public ICommandResult Handle(CreatePayPalSubscriptionCommand command)
        {            
            // Verificar se documento está cadastrado
            if(_repository.DocumentExists(command.PayerDocument))
            {
                AddNotifications(new Contract());
                return new CommandResult(false, "Esse CPF já está em uso");
            }
            //Verificar se E-mail já está cadastrado
            if(_repository.EmailExists(command.PayerEmail))
            {
                AddNotifications(new Contract());
                return new CommandResult(false, "Esse E-mail já está em uso");
            }
            //Gerar os VOs
            var name = new Name(command.FirstName, command.LastName);
           var doc = new Document(command.PayerDocument, EDocumentType.CPF);
           var email = new Email(command.PayerEmail);
           var address = new Address(command.Street, command.Number, command.Neighborhood, command.City, command.State, command.Country, command.ZipCode);
            
            //Gerar as Entidades
            var student = new Student(name, doc, email);
            var subscription = new Subscription(DateTime.Now.AddMonths(1));
            var payment = new PayPalPayment(
                command.TransactionCode,                 
                command.PaidDate, 
                command.ExpireDate, 
                command.Total, 
                command.TotalPaid,
                new Document(command.PayerDocument, command.PayerDocumentType),
                command.Payer,                 
                address, 
                email);

            //Relacionamentos
            subscription.AddPayment(payment);
            student.AddSubscription(subscription);

            //Agrupar as Validações
            AddNotifications(name, doc, email, address, student, subscription, payment);

            //Salvar as Informações
            _repository.CreateSubscription(student);

            //Enviar E-mail de boas vindas
            _emailService.Send(student.Name.FirstName, student.Email.Address, "Bem vindo", "Sua assinatura foi criada");
            
            //Retornar informações
            return new CommandResult(true, "Assinatura realizada com sucesso");
        }
    }
}