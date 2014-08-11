# Readme


The module uses AzureDirectory to store Lucene search indices in Azure Blob storage (with a local cache) so it isn't stored on the web server's local storage. It contains a search index provider that extends and overrides the default Lucene provider. Thus after enabling the features of this module indices will be stored in Blob storage without further configuration (if Azure media storage is already configured).

AzureDirectory is included as source to avoid a mismatch of assemblies (the project used a previous version of Azure assemblies). The actual code that's included is from https://github.com/richorama/AzureDirectory.