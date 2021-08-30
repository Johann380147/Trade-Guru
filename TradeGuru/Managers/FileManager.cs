using System;

namespace TradeGuru.Managers
{
    class FileManager
    {
        public static string GetUserDefinedFolder()
        {
            using (var dialog = new System.Windows.Forms.FolderBrowserDialog())
            {
                System.Windows.Forms.DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    return dialog.SelectedPath;
                }
                else
                {
                    return String.Empty;
                }
            }
        }
    }
}
