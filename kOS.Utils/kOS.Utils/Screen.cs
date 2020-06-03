using System.IO;
using kOS.AddOns;
using kOS.Safe.Encapsulation;
using kOS.Safe.Encapsulation.Suffixes;
using UnityEngine;

namespace kOS.Utils
{
    [kOSAddon("SCREEN")]
    [Safe.Utilities.KOSNomenclature("SCREENAddon")]
    public class Screen : Suffixed.Addon
    {
        public Screen(SharedObjects shared) : base(shared)
        {
            InitializeSuffixes();
            Directory.CreateDirectory(KSPUtil.ApplicationRootPath + "Screenshots_kOS");
        }

        private void InitializeSuffixes()
        {
            AddSuffix("TAKE", new NoArgsVoidSuffix(TakeScreen));
        }

        public void TakeScreen()
        {
            string[] filePaths = Directory.GetFiles(KSPUtil.ApplicationRootPath + "Screenshots_kOS", "*.png", SearchOption.TopDirectoryOnly);
            string lastScreen;
            int n;
            if (filePaths.Length > 0)
            {
                Debug.Log("[SCREEN] Filepaths contains something!");
                lastScreen = filePaths[filePaths.Length - 1];
                string screenNumber = lastScreen.Replace(KSPUtil.ApplicationRootPath + "Screenshots_kOS" + Path.DirectorySeparatorChar + "screenshot", "");
                screenNumber = screenNumber.Replace(".png", "");
                Debug.Log("[SCREEN] number: " + screenNumber);
                int.TryParse(screenNumber, out n);
                n++;
            }
            else
            {
                n = 0;
            }
            ScreenCapture.CaptureScreenshot(KSPUtil.ApplicationRootPath + "Screenshots_kOS" + Path.DirectorySeparatorChar + "screenshot" + n.ToString() + ".png");
        }

        public override BooleanValue Available()
        {
            return true;
        }
    }
}