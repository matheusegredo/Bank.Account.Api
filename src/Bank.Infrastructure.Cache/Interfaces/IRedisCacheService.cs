namespace Bank.Infrastructure.Cache.Interfaces
{
    public interface IRedisCacheService<TEntity>
    {
        Task<TEntity> Set(string key, TEntity entity, CancellationToken cancellationToken);

        bool TryGet(string key, out TEntity entity);
    }
}
