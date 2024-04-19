public interface IAccountRepository
{
    void RegisterAccount(User account); 
    User GetAccount(string accountName);
    List<User> GetAccounts(); 
    bool VerifyAccount(User account);
}