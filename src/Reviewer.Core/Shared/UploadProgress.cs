using System;
using AsyncAwaitBestPractices;
using Microsoft.WindowsAzure.Storage.Core.Util;
using Xamarin.Forms;

namespace Reviewer.Core
{
    public class UploadProgress : IProgress<StorageProgress>
    {
        readonly WeakEventManager<double> weakEventManager = new();

        public event EventHandler<double> Updated
        {
            add => weakEventManager.AddEventHandler(value);
            remove => weakEventManager.RemoveEventHandler(value);
        }

        public double TotalImageBytes { get; set; }

        async void IProgress<StorageProgress>.Report(StorageProgress value) => await Device.InvokeOnMainThreadAsync(() =>
        {
            if (Math.Abs(TotalImageBytes) < 0)
                return;

            double updatePercentage = value.BytesTransferred;

            OnUpdated(updatePercentage);
        });

        void OnUpdated(double updatePercentage) => weakEventManager.RaiseEvent(this, updatePercentage, nameof(Updated));
    }
}
