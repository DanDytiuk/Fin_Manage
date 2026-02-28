namespace FinManage.Services
{
    internal class MessageHelper
    {
        public static void ShowError(string resourceKey, string statusKey)
        {
            System.Windows.MessageBox.Show(
                LocalizationHelper.Instance[resourceKey],
                LocalizationHelper.Instance[statusKey],
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Warning);
        }

        public static void ShowMessage(string resourceKey, string statusKey)
        {
            System.Windows.MessageBox.Show(
                LocalizationHelper.Instance[resourceKey],
                LocalizationHelper.Instance[statusKey],
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Information);
        }

        public static void ShowAttention(string resourceKey, string statusKey)
        {
            System.Windows.MessageBox.Show(
                LocalizationHelper.Instance[resourceKey],
                LocalizationHelper.Instance[statusKey],
                System.Windows.MessageBoxButton.OK,
                System.Windows.MessageBoxImage.Warning);
        }

        public static void ShowLearn(string resourceKey, string statusKey)
        {
            System.Windows.MessageBox.Show(
                LocalizationHelper.Instance[resourceKey],
                LocalizationHelper.Instance[statusKey],
                System.Windows.MessageBoxButton.OKCancel,
                System.Windows.MessageBoxImage.Question);
        }
    }
}
