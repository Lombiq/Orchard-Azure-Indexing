# Orchard Azure Search Indexing Readme




## Project Description

Orchard module with a search indexing implementation that stores Lucene indices in Azure Blob storage.


## Documentation

The module uses AzureDirectory to store Lucene search indices in Azure Blob storage (with a local cache) so it isn't stored on the web server's local storage. It contains a search index provider that extends and overrides the default Lucene provider. Thus after enabling the features of this module indices will be stored in Blob storage, but first you have to configure the storage: take a look at the Constants class and add entries to the appSettings or connectionStrings in the Web.config (or through the Azure Portal) corresponding to those configuration keys.

AzureDirectory is included as source to avoid a mismatch of assemblies (the project used a previous version of Azure assemblies). The actual code that's included is from https://github.com/richorama/AzureDirectory.

The module is also available for [DotNest](http://dotnest.com/) sites.

The module's source is available in two public source repositories, automatically mirrored in both directions with [Git-hg Mirror](https://githgmirror.com):

- [https://bitbucket.org/Lombiq/hosting-azure-indexing](https://bitbucket.org/Lombiq/hosting-azure-indexing) (Mercurial repository)
- [https://github.com/Lombiq/Orchard-Azure-Indexing](https://github.com/Lombiq/Orchard-Azure-Indexing) (Git repository)

Bug reports, feature requests and comments are warmly welcome, **please do so via GitHub**.
Feel free to send pull requests too, no matter which source repository you choose for this purpose.

This project is developed by [Lombiq Technologies Ltd](http://lombiq.com/). Commercial-grade support is available through Lombiq.