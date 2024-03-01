namespace Payment.Business.Interfaces.Data
{
    public interface  IUnitOfWork
    {
        Task<bool> Commit();
    }
}
