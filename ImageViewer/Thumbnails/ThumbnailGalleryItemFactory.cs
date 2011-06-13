using System;
using System.Threading;
using ClearCanvas.Desktop;

namespace ClearCanvas.ImageViewer.Thumbnails
{
    internal partial class ThumbnailGalleryItemFactory : IGalleryItemFactory<IDisplaySet>
    {
        private readonly GetThumbnailLoader _getLoader;
        private readonly SynchronizationContext _synchronizationContext;

        public ThumbnailGalleryItemFactory(GetThumbnailLoader getLoader)
        {
            _synchronizationContext = SynchronizationContext.Current;
            if (_synchronizationContext == null)
                throw new InvalidOperationException("It is expected that the factory will be instantiated on and accessed from a UI thread.");

            _getLoader = getLoader;
        }

        public IThumbnailLoader Loader { get { return _getLoader(); } }

        private string LoadingMessage { get { return SR.MessageLoading; } }
        private string NoImagesMessage { get { return SR.MessageNoImages; } }

        #region IGalleryItemFactory<IDisplaySet> Members

        public IGalleryItem Create(IDisplaySet displaySet)
        {
            return new ThumbnailGalleryItem(this, displaySet);
        }

        #endregion

        private static string GetDisplaySetName(IDisplaySet displaySet)
        {
            string name = displaySet.Name;
            name = name.Replace("\r\n", " ");
            name = name.Replace("\r", " ");
            name = name.Replace("\n", " ");

            int number = displaySet.PresentationImages.Count;
            if (number <= 1)
                return String.Format(SR.FormatThumbnailName1Image, name);

            return String.Format(SR.FormatThumbnailName, number, name);
        }
    }
}