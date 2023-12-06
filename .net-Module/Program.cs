using Azure.Data.Tables;
using WeatherStats;

var endPoint = "https://weatherstats.table.core.windows.net/stats";
var accountKey = "kjq19H9Dy0JROz3cciwjxEYWmTgLdT1cZmWVxSnD3OuxMJtsGWnwAG/OKx1ZgbzwtV2fjkpMdqHK+AStFp6+gQ==";
var accountName = "weatherstats";
var partitionKey = "Temperature_Humidity";
var tableName = "stats";

var serviceClient = new TableServiceClient(
    new Uri(endPoint),
    new TableSharedKeyCredential(accountName, accountKey));

var temperature = new WeatherInfo(partitionKey, "0", 12, 13);

var tableClient = serviceClient.GetTableClient(tableName);

var ifNotExists = tableClient.CreateIfNotExists();

var upsertEntity = tableClient.UpsertEntity(temperature);

int x = 10;