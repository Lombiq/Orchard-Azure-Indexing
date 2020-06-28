# Orchard Azure Search Indexing



## About

Orchard module with a search indexing implementation that stores Lucene indices in Azure Blob storage.


## Documentation

The module uses AzureDirectory to store Lucene search indices in Azure Blob storage (with a local cache) so it isn't stored on the web server's local storage. It contains a search index provider that extends and overrides the default Lucene provider. Thus after enabling the features of this module indices will be stored in Blob storage, but first you have to configure the storage: take a look at the Constants class and add entries to the appSettings or connectionStrings in the Web.config (or through the Azure Portal) corresponding to those configuration keys.

AzureDirectory is included as source to avoid a mismatch of assemblies (the project used a previous version of Azure assemblies). The actual code that's included is from https://github.com/richorama/AzureDirectory.

The module is also available for [DotNest](http://dotnest.com/) sites.


## Contributing and support

Bug reports, feature requests, comments, questions, code contributions, and love letters are warmly welcome, please do so via GitHub issues and pull requests. Please adhere to our [open-source guidelines](https://lombiq.com/open-source-guidelines) while doing so.

This project is developed by [Lombiq Technologies](https://lombiq.com/). Commercial-grade support is available through Lombiq.