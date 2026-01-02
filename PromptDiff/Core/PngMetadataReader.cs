using System.IO;
using System.Windows.Media.Imaging;

namespace PromptDiff.Core
{
    public static class PngMetadataReader
    {
        public static string ReadPngTextMetadata(string path)
        {
            using var stream = File.OpenRead(path);
            var decoder =
                new PngBitmapDecoder(stream, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnLoad);

            var metadata = decoder.Frames[0].Metadata as BitmapMetadata;
            if (metadata == null)
            {
                return null;
            }

            // 例：Stable Diffusion がよく使う tEXt
            if (metadata.ContainsQuery("/tEXt/parameters"))
            {
                return metadata.GetQuery("/tEXt/parameters") as string;
            }

            // fallback
            foreach (var query in metadata)
            {
                if (metadata.GetQuery(query) is string s && !string.IsNullOrWhiteSpace(s))
                {
                    return s;
                }
            }

            return null;
        }
    }
}