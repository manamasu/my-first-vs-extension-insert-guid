using Microsoft;
using Microsoft.VisualStudio.Extensibility;
using Microsoft.VisualStudio.Extensibility.Commands;
using Microsoft.VisualStudio.Extensibility.Shell;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks.Dataflow;

namespace InsertGuid
{
    /// <summary>
    /// Command1 handler.
    /// </summary>
    [VisualStudioContribution]
    internal class InsertGuidCommand : Command
    {
        private readonly TraceSource logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="InsertGuidCommand"/> class.
        /// </summary>
        /// <param name="traceSource">Trace source instance to utilize.</param>
        public InsertGuidCommand(TraceSource traceSource)
        {
            // This optional TraceSource can be used for logging in the command. You can use dependency injection to access
            // other services here as well.
            this.logger = Requires.NotNull(traceSource, nameof(traceSource));
        }

        /// <inheritdoc />
        public override CommandConfiguration CommandConfiguration => new("%InsertGuid.InsertGuidCommand.DisplayName%")
        {
            // Use this object initializer to set optional parameters for the command. The required parameter,
            // displayName, is set above. DisplayName is localized and references an entry in .vsextension\string-resources.json.
            Icon = new(ImageMoniker.KnownValues.OfficeWebExtension, IconSettings.IconAndText),
            Placements = [CommandPlacement.KnownPlacements.ExtensionsMenu],
            VisibleWhen = ActivationConstraint.ClientContext(ClientContextKey.Shell.ActiveEditorContentType, ".+")
        };

        /// <inheritdoc />
        public override Task InitializeAsync(CancellationToken cancellationToken)
        {
            // Use InitializeAsync for any one-time setup or initialization.
            return base.InitializeAsync(cancellationToken);
        }

        /// <inheritdoc />
        /// Read more about it here
        /// https://learn.microsoft.com/de-de/visualstudio/extensibility/visualstudio.extensibility/get-started/tutorial-create-simple-extension?view=vs-2022
        public override async Task ExecuteCommandAsync(IClientContext context, CancellationToken cancellationToken)
        {
            Requires.NotNull(context, nameof(context));
            var newGuidString = Guid.NewGuid().ToString("N", CultureInfo.CurrentCulture);

            using var textView = await context.GetActiveTextViewAsync(cancellationToken);

            if (textView is null)
            {
                this.logger.TraceInformation("There was no active text view when the command was executed.");
                return;
            }

            await this.Extensibility.Editor().EditAsync(

                batch =>
                {
                    textView.Document.AsEditable(batch).Replace(textView.Selection.Extent, newGuidString);
                },
                cancellationToken);
        }
    }
}
