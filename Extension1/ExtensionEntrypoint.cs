using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.Extensibility;

namespace Extension1
{
    /// <summary>
    /// Extension entrypoint for the VisualStudio.Extensibility extension.
    /// </summary>
    [VisualStudioContribution]
    internal class ExtensionEntrypoint : Extension
    {
        /// <inheritdoc/>
        public override ExtensionConfiguration ExtensionConfiguration => new()
        {
            Metadata = new(
                    id: "Extension1.8e1996b0-e72f-4954-988d-73becb3c8f80",
                    version: this.ExtensionAssemblyVersion,
                    publisherName: "Publisher name",
                    displayName: "InsertGuid",
                    description: "The ability to get a guid based on a single click"),
        };

        /// <inheritdoc />
        protected override void InitializeServices(IServiceCollection serviceCollection)
        {
            base.InitializeServices(serviceCollection);

            // You can configure dependency injection here by adding services to the serviceCollection.
        }
    }
}
