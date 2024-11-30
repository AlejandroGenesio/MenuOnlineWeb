namespace MenuOnlineUdemy.Repositories
{
    public interface IRepositoryBase
    {
        [Obsolete("Use IfExists(..)")]
        bool IsEmptyId(int value);
    }
}
