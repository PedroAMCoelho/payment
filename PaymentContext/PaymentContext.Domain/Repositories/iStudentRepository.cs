using PaymentContext.Domain.Entities;

namespace PaymentContext.Domain.Repositories
{

    //na parte de infra dps, vc pode implementar os metodos dessa interface. Essa abstração é importante para os dominios n serem afetados pelos dados
    public interface IStudentRepository
    {
        bool DocumentExists(string document);
        bool EmailExists(string email);
        void CreateSubscription(Student student);
    }

}