using System.IO;
using MetadataExtractor;
using MetadataExtractor.Formats.Exif;

public class PhotoInfo
{
    public static Photo GetMetadata(string filePath)
    {
        Photo photo_obj = new Photo { FileName = Path.GetFileName(filePath) };

        try
        {
            var directories = ImageMetadataReader.ReadMetadata(filePath);

            // Camera Model
            var ifd0 = directories.OfType<ExifIfd0Directory>().FirstOrDefault();
            photo_obj.Model = ifd0?.GetString(ExifDirectoryBase.TagModel);

            // Date
            var subIfd = directories.OfType<ExifSubIfdDirectory>().FirstOrDefault();
            if (subIfd?.TryGetDateTime(ExifDirectoryBase.TagDateTimeOriginal, out var dt) == true)
            {
                photo_obj.DateTimeOriginal = dt;
            }

            // Lens
            var lensModel = subIfd?.GetString(ExifDirectoryBase.TagLensModel) ?? "";

            // Get GPS
            var gps = directories.OfType<GpsDirectory>().FirstOrDefault();
            var geoLocation = gps?.GetGeoLocation();
            if (geoLocation is { } geo)
            {
                photo_obj.Latitude = geo.Latitude;
                photo_obj.Longitude = geo.Longitude;
            }

            // check if lens specifies front camer or screen shot 
            // true depth also mean ss
            photo_obj.IsSelfie = lensModel.ToLower().Contains("front") || lensModel.ToLower().Contains("truedepth");
            photo_obj.IsScreenshot = lensModel.ToLower().Contains("screenshot");
        }
        catch (Exception ex)
        { }
        return photo_obj;
    }
}