using System.Collections.Generic;
using System.Linq;
using Flunt.Validations;
using PaymentContext.Domain.ValueObjects;
using PaymentContext.Shared.Entities;

namespace PaymentContext.Domain.Entities
{
    public class Student : Entity
    {
        //usar um private IList<string> Notifications;, para notificar erros, e não new Exception, pq exception pesa mt, gera log no windows (servidor)
        // estou usando o flunt, plugin do balta, pra isso
        
        private readonly IList<Subscription> _subscriptions;
        public Student(Name name, Document document, Email email)
        {
            Name = name;
            Document = document;
            Email = email;
            _subscriptions = new List<Subscription>();

            AddNotifications(name, document, email);
        }
        //Colocando todos os sets como private, você impede que uma propriedade seja alterada diretamente, fora da classe (ex: student.FirstName = "";). 
        //Vai precisar de um método como SetName. Isso faz com que ngm fora da classe Student consiga modificá-la sem alguma validação de um 
        //método, por ex.
        public Name Name {get; private set;}
        public Document Document {get; private set;}
        public Email Email {get; private set;}
        public Address Address {get; private set;}
        public IReadOnlyCollection<Subscription> Subscriptions {get{return _subscriptions.ToArray();}}
        //a ideia do IReadOnlyCollection -
        //1. consuming classes should only see what I want to show them, not whole underlying collection
        //2. nao pode modificar

        public void AddSubscription(Subscription subscription)
        {
            var hasSubscriptionActive = false;
            foreach(var sub in _subscriptions)
            {
                if(sub.Active)
                hasSubscriptionActive = true;
            }

            //Contrato
            AddNotifications(new Contract()
            .Requires()
            .IsFalse(hasSubscriptionActive, "Student.Subscriptions", "Você já tem uma assinatura ativa")
            .AreEquals(0, subscription.Payments.Count, "Student.Subscriptions.Payments", "Esta assinatura não possui pagamentos")
            );

            // Alternativa
            // if(hasSubscriptionActive)
            // AddNotification("Student.Subscriptions", "Você já tem uma assinatura ativa");
        }
    }

}