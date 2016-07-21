namespace JsonWebTokenSample.Common
{
    public interface IBaseRepository
    {
        bool ConnectDataBase();
        bool DisconnectDataBase();
    }
}