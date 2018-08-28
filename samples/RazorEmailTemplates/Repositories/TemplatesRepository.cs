using RazorEmailTemplates.Features.Create;
using RazorEmailTemplates.Features.Delete;
using RazorEmailTemplates.Features.Details;
using RazorEmailTemplates.Features.PaginatedList;
using RazorEmailTemplates.Features.Update;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lurumad.AspNet.FileProviders.AzureStorage;
using RazorEmailTemplates.Model;

namespace RazorEmailTemplates.Repositories
{
    public class TemplatesRepository : ITemplatesRepository
    {
        private const string TableName = "templates";
        private readonly AzureOptions options;
        private readonly CloudTableClient tableClient;
        private readonly CloudBlobClient blobClient;

        public TemplatesRepository(AzureOptions options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
            var storageAccount = CloudStorageAccount.Parse(options.ConnectionString);
            tableClient = storageAccount.CreateCloudTableClient();
            blobClient = storageAccount.CreateCloudBlobClient();
        }

        public async Task Add(TemplateCreateModel model)
        {
            var table = tableClient.GetTableReference(TableName);
            var container = blobClient.GetContainerReference(options.Container);

            await Task.WhenAll(table.CreateIfNotExistsAsync(), container.CreateIfNotExistsAsync());

            var template = new Template(model.Name, model.Language);

            var entity = await table.ExecuteAsync(
                TableOperation.Retrieve(template.PartitionKey, template.RowKey));

            if (entity.Result != null)
            {
                throw new ArgumentException($"A template {template.PartitionKey}:{template.RowKey} already exists.");
            }


            var blob = container.GetBlockBlobReference($"{template.PartitionKey}/{template.RowKey}/{template.RowKey}.cshtml");

            await blob.UploadFromStreamAsync(model.Template.OpenReadStream());

            template.Url = blob.Uri.ToString();

            await table.ExecuteAsync(TableOperation.Insert(template));
        }

        public async Task Delete(TemplateDeleteModel model)
        {
            var table = tableClient.GetTableReference(TableName);
            var container = blobClient.GetContainerReference(options.Container);

            await Task.WhenAll(table.CreateIfNotExistsAsync(), container.CreateIfNotExistsAsync());

            var template = new Template(model.Name, model.Language);

            var entity = await table.ExecuteAsync(
                TableOperation.Retrieve(template.PartitionKey, template.RowKey));

            if (entity.Result == null)
            {
                throw new ArgumentException($"A template {template.PartitionKey}:{template.RowKey} does not exists.");
            }

            var blob = container.GetBlockBlobReference($"{template.PartitionKey}/{template.RowKey}/{template.RowKey}.cshtml");

            await blob.DeleteIfExistsAsync();
            await table.ExecuteAsync(TableOperation.Delete((ITableEntity)entity.Result));
        }

        public async Task<TemplateDetails> Details(string language, string name)
        {
            var table = tableClient.GetTableReference(TableName);
            var container = blobClient.GetContainerReference(options.Container);
            var operation = TableOperation.Retrieve<Template>(language, name);
            var result = await table.ExecuteAsync(operation);

            if (result.Result == null)
            {
                throw new ArgumentException($"A template {language}:{name} does not exists.");
            }

            var blob = container.GetBlockBlobReference($"{language}/{name}/{name}.cshtml");
            var content = await blob.DownloadTextAsync();

            return new TemplateDetails(name, language, content);
        }

        public async Task<PagedResults> GetPagedResults(string language, string continuationToken)
        {
            var table = tableClient.GetTableReference(TableName);
            TableContinuationToken token = GetContinuationToken(continuationToken);
            TableQuery<Template> query = new TableQuery<Template>().Where(
                TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, language)).Take(10);
            var results = await table.ExecuteQuerySegmentedAsync(query, token);
            token = results.ContinuationToken;
            return new PagedResults(
                results.Results.Select(x => new TemplatePagedResult(x.RowKey, x.PartitionKey)),
                GenerateContinuationToken(token),
                token != null);
        }

        public async Task Update(TemplateUpdateModel model)
        {
            var table = tableClient.GetTableReference(TableName);
            var container = blobClient.GetContainerReference(options.Container);

            await Task.WhenAll(table.CreateIfNotExistsAsync(), container.CreateIfNotExistsAsync());

            var template = new Template(model.Name, model.Language);

            var entity = await table.ExecuteAsync(
                TableOperation.Retrieve(template.PartitionKey, template.RowKey));

            if (entity.Result == null)
            {
                throw new ArgumentException($"A template {template.PartitionKey}:{template.RowKey} does not exists.");
            }

            var blob = container.GetBlockBlobReference($"{template.PartitionKey}/{template.RowKey}/{template.RowKey}.cshtml");

            await blob.UploadFromStreamAsync(model.Template.OpenReadStream());
        }

        private string GenerateContinuationToken(TableContinuationToken token)
        {
            if (token == null)
            {
                return String.Empty;
            }

            return Convert.ToBase64String(
                Encoding.UTF8.GetBytes(
                    JsonConvert.SerializeObject(token)));
        }

        private TableContinuationToken GetContinuationToken(string continuationToken)
        {
            if (String.IsNullOrWhiteSpace(continuationToken))
            {
                return null;
            }

            return JsonConvert.DeserializeObject<TableContinuationToken>(
                Encoding.UTF8.GetString(
                    Convert.FromBase64String(continuationToken)));
        }
    }
}
