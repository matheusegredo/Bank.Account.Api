using Bank.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using System.Transactions;

namespace Bank.Application.Helpers
{
    public static class DbHelper
    {
		public async static Task<int> GetValueByKey<TEntity>(this IBankContext contexto, int id, CancellationToken cancellationToken = default) where TEntity : class
		{
			var primaryKey = contexto.Model.FindEntityType(typeof(TEntity)).FindPrimaryKey().Properties.Single().Name;

			var registro = await contexto.Set<TEntity>().Where(p => Equals(EF.Property<int>(p, primaryKey), id))
				.Select(p => EF.Property<int>(p, primaryKey))
				.FirstOrDefaultReadingUncomittedAsync(cancellationToken);

			return registro;
		}

        public static async Task<List<TSource>> ToListReadingUncomittedAsync<TSource>([NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            using (new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                return await source.ToListAsync(cancellationToken);
        }

        public static async Task<TSource> FirstOrDefaultReadingUncomittedAsync<TSource>([NotNull] this IQueryable<TSource> source, CancellationToken cancellationToken = default)
        {
            var transactionOptions = new TransactionOptions
            {
                IsolationLevel = IsolationLevel.ReadUncommitted,
                Timeout = TransactionManager.MaximumTimeout
            };

            using (new TransactionScope(TransactionScopeOption.Required, transactionOptions, TransactionScopeAsyncFlowOption.Enabled))
                return await source.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
