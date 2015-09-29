# Hosting - Azure Indexing readme


The module uses AzureDirectory to store Lucene search indices in Azure Blob storage (with a local cache) so it isn't stored on the web server's local storage. It contains a search index provider that extends and overrides the default Lucene provider. Thus after enabling the features of this module indices will be stored in Blob storage, but first you have to configure the storage: take a look at the Constants class and add entries to the appSettings or connectionStrings in the Web.config (or through the Azure Portal) corresponding to those configuration keys.

AzureDirectory is included as source to avoid a mismatch of assemblies (the project used a previous version of Azure assemblies). The actual code that's included is from https://github.com/richorama/AzureDirectory.