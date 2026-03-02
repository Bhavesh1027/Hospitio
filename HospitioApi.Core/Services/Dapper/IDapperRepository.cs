using Dapper;
using System.Data;

namespace HospitioApi.Core.Services.Dapper
{
    public interface IDapperRepository
    {
        Task<T?> GetSingle<T>(string query, DynamicParameters? sp_params, CancellationToken cancellationToken, CommandType commandType = CommandType.StoredProcedure);
        Task<List<T>> GetAll<T>(string query, DynamicParameters? sp_params, CancellationToken cancellationToken, CommandType commandType = CommandType.StoredProcedure);
        Task<List<T>?> GetAllJsonData<T>(string query, DynamicParameters? sp_params, CancellationToken cancellationToken, CommandType commandType = CommandType.StoredProcedure);

        Task<T?> AddSingle<T>(string query, DynamicParameters? sp_params, CancellationToken cancellationToken, CommandType commandType = CommandType.StoredProcedure);
    }
}
