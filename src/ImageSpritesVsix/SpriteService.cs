using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using ImageSprites;

namespace ImageSpritesVsix
{
    class SpriteService
    {
        private static SpriteImageGenerator _generator;

        public static void Initialize()
        {
            SpriteDocument.Saving += SpriteSaving;
            SpriteDocument.Saved += SpriteSaved;

            _generator = new SpriteImageGenerator();
            _generator.Saving += SpriteImageSaving;
            _generator.Saved += SpriteImageSaved;
        }

        private static void SpriteImageSaved(object sender, SpriteImageGenerationEventArgs e)
        {
            ProjectHelpers.AddNestedFile(e.Document.FileName, e.FileName);
            OptimizeImage(e.FileName, e.Document.Optimize);
        }

        private static void OptimizeImage(string fileName, Optimization optimization)
        {
            try
            {
                string ext = Path.GetExtension(fileName);

                if (optimization != Optimization.None && Constants.SupporedExtensions.Contains(ext, StringComparer.OrdinalIgnoreCase))
                {
                    string cmd = "ImageOptimizer.OptimizeLossless";

                    if (optimization == Optimization.Lossy)
                        cmd = "ImageOptimizer.OptimizeLossy";

                    ProjectHelpers.ExecuteCommand(cmd, fileName);
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        private static void SpriteImageSaving(object sender, SpriteImageGenerationEventArgs e)
        {
            ProjectHelpers.CheckFileOutOfSourceControl(e.FileName);
        }

        private static void SpriteSaved(object sender, FileSystemEventArgs e)
        {
            var project = ProjectHelpers.GetActiveProject();

            if (project != null)
            {
                ProjectHelpers.AddFileToProject(project, e.FullPath, "None");
            }
        }

        private static void SpriteSaving(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                ProjectHelpers.CheckFileOutOfSourceControl(e.FullPath);
            }
        }

        public static async Task GenerateSpriteAsync(string fileName)
        {
            try
            {
                var doc = await SpriteDocument.FromFile(fileName);
                await GenerateSpriteAsync(doc);
            }
            catch (SpriteParseException ex)
            {
                MessageBox.Show(ex.Message, Vsix.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ProjectHelpers.DTE.ItemOperations.OpenFile(fileName);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public static async Task GenerateSpriteAsync(SpriteDocument doc)
        {
            try
            {
                await _generator.Generate(doc);
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show(ex.Message, Vsix.Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                ProjectHelpers.DTE.ItemOperations.OpenFile(doc.FileName);
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
        }
    }
}
