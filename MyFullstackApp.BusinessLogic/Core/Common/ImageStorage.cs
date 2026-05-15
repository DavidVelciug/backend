namespace MyFullstackApp.BusinessLogic.Core.Common;

public static class ImageStorage
{
    public static string SaveUploadedFile(Stream source, string fileName, string folderName)
    {
        var extension = Path.GetExtension(fileName);
        if (string.IsNullOrWhiteSpace(extension))
        {
            extension = ".bin";
        }

        var root = Directory.GetCurrentDirectory();
        var uploadsDir = Path.Combine(root, "wwwroot", "uploads", folderName);
        Directory.CreateDirectory(uploadsDir);

        var normalizedExt = extension.StartsWith('.') ? extension : $".{extension}";
        var finalName = $"{Guid.NewGuid():N}{normalizedExt.ToLowerInvariant()}";
        var filePath = Path.Combine(uploadsDir, finalName);

        using var target = File.Create(filePath);
        source.CopyTo(target);

        return $"/uploads/{folderName}/{finalName}";
    }

    public static string? SaveDataUrlIfNeeded(string? value, string folderName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return value;
        }

        if (!value.StartsWith("data:image/", StringComparison.OrdinalIgnoreCase))
        {
            return value;
        }

        var markerIndex = value.IndexOf("base64,", StringComparison.OrdinalIgnoreCase);
        if (markerIndex < 0)
        {
            return value;
        }

        var mimeEnd = value.IndexOf(';');
        var mime = mimeEnd > 5 ? value.Substring(5, mimeEnd - 5) : "image/png";
        var extension = mime.Contains("jpeg", StringComparison.OrdinalIgnoreCase) ? "jpg" :
            mime.Contains("webp", StringComparison.OrdinalIgnoreCase) ? "webp" : "png";

        var base64 = value[(markerIndex + "base64,".Length)..];
        var bytes = Convert.FromBase64String(base64);

        var root = Directory.GetCurrentDirectory();
        var uploadsDir = Path.Combine(root, "wwwroot", "uploads", folderName);
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid():N}.{extension}";
        var filePath = Path.Combine(uploadsDir, fileName);
        File.WriteAllBytes(filePath, bytes);

        return $"/uploads/{folderName}/{fileName}";
    }
}
