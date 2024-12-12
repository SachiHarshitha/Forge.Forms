namespace Forge.Application.AvaloniaUI.Infrastructure.Internal
{
    internal class DialogFileSaver : IFileSaver
    {
        public string GetFile(string fileName, string filter)
        {
            var saveFileDialog = new SaveFileDialog
            {
                FileName = fileName ?? string.Empty,
                Filter = filter
            };

            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }
    }
}
