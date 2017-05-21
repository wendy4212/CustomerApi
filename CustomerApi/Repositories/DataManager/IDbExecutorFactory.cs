
namespace CustomerApi.Repositories.DataManager
{
    public interface IDbExecutorFactory
    {
        IDbExecutor CreateExecutor();
    }
}