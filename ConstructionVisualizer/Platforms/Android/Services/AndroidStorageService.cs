using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionVisualizer.Platforms.Android.Services;

public partial class MobileStorageService
{
    public async partial Task SaveImageToGalleryAsync(byte[] imageData, string filename)
    {
        if (OperatingSystem.IsAndroid())
        {
            // Android implementation
            var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted) return;

            var downloadsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(
                Android.OS.Environment.DirectoryPictures).AbsolutePath;

            var filePath = Path.Combine(downloadsPath, filename);
            await File.WriteAllBytesAsync(filePath, imageData);

            // Notify gallery
            var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(filePath)));
            Android.App.Application.Context.SendBroadcast(mediaScanIntent);
        }
    }
}
