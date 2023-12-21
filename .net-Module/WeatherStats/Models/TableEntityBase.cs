namespace WeatherStats.Models;

using Azure;
using Azure.Data.Tables;

public abstract class TableEntityBase(string partitionKey, string rowKey) : ITableEntity
{
    public string PartitionKey { get; set; } = partitionKey;

    public string RowKey { get; set; } = rowKey;

    public DateTimeOffset? Timestamp { get; set; }

    public ETag ETag { get; set; }


}
