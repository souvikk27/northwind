using System.Data;

namespace Northwind.Services.SQL;

public interface ISqlConnectionFactory
{
	IDbConnection CreateConnection();
}