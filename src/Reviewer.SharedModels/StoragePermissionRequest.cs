using System.ComponentModel;

namespace Reviewer.SharedModels
{
    public record StoragePermissionRequest(string ContainerName, string BlobName, string Permission);
}

namespace System.Runtime.CompilerServices
{
    [EditorBrowsable(EditorBrowsableState.Never)]
    public record IsExternalInit;
}
