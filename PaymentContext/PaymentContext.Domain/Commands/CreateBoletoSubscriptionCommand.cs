using System;
using Flunt.Notifications;
using Flunt.Validations;
using PaymentContext.Domain.Enums;
using PaymentContext.Shared.Commands;

namespace PaymentContext.Domain.Commands{
    //essas classes de Commands servem para mapear/definir/transportar os objetos. São entrada, como se fosse um viewModel

    public class CreateBoletoSubscriptionCommand : Notifiable, ICommand
    {
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string PaymentNumber {get; set;}
        public string Address{get; set;}
        public string BarCode {get; set;}
        public string BoletoNumber {get; set;}
        
        public DateTime PaidDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public decimal Total { get; set; }
        public decimal TotalPaid { get; set; }
        public string PayerDocument {get; set;}
        public EDocumentType PayerDocumentType {get; set;}
        public string PayerEmail {get; set;}
        public string Payer {get; set;}
        public string Street {get;  set;}
        public string Number {get;  set;}
        public string Neighborhood {get;  set;}
        public string City {get;  set;}
        public string State {get;  set;}
        public string Country {get;  set;}
        public string ZipCode {get;  set;}

        //pq essa validação aqui também? pq assim q o json é mapeado para essas props, a validação já barra na raiz, impedindo a requisição ser feita
        public void Validate()
        {
            AddNotifications(new Contract()
                .Requires()
                .HasMinLen(FirstName, 3, "Name.FirstName", "Nome deve conter pelo menos 3 caracteres")
                .HasMinLen(LastName, 3, "Name.LastName", "Sobrenome deve conter pelo menos 3 caracteres")
                .HasMaxLen(FirstName, 40, "Name.LastName", "Nome deve conter até 40 caracteres")
            );
        }
    }
}