using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Screenshot : MonoBehaviour
{
    [Header("Screenshot Components")]
    private RenderTexture screenshotRenderTexture;
    private Texture2D screenshotTexture;
    private Rect screenshotRect;

    [Header("Screenshot Dimensions")]
    [SerializeField] private int screenshotWidth = 1920;
    [SerializeField] private int screenshotHeight = 1080;

    [Header("Screenshot Controllers")]
    [SerializeField] private string screenshotFolder = "Assets/Screenshots";
    [SerializeField] private enum Format { JPG, PNG, PPM, RAW };
    [SerializeField] private Format screenshotFormat = Format.PNG;
    [SerializeField] private bool screenshotOptimize = false;

    [Header("Screenshot Checks")]
    private bool screenshotCaptureScreenshot = true;
    private bool screenshotCaptureVideo = false;
    private int screenshotIndex = 0;

    private void Update()
    {
        screenshotCaptureScreenshot |= Input.GetKeyDown(KeyCode.P);
        screenshotCaptureVideo = Input.GetKey(KeyCode.O);

        if (screenshotCaptureScreenshot || screenshotCaptureVideo)
        {
            screenshotCaptureScreenshot = false;

            // Screenshot Off-Screen Render Texture Managers
            if (screenshotRenderTexture == null)
            {
                screenshotRect = new Rect(0, 0, screenshotWidth, screenshotHeight);
                screenshotRenderTexture = new RenderTexture(screenshotWidth, screenshotHeight, 24);
                screenshotTexture = new Texture2D(screenshotWidth, screenshotHeight, TextureFormat.RGB24, false);
            }

            // Screenshot Camera Managers
            Camera camera = GetComponent<Camera>();
            camera.targetTexture = screenshotRenderTexture;
            camera.Render();

            // Screenshot Render Texture Managers
            RenderTexture.active = screenshotRenderTexture;
            screenshotTexture.ReadPixels(screenshotRect, 0, 0);

            // Screenshot Reset Managers
            camera.targetTexture = null;
            RenderTexture.active = null;

            // Screenshot File Name Managers
            string fileName = ScreenshotCreateNewFile((int)screenshotRect.width, (int)screenshotRect.height);

            // Screenshot Data Managers
            byte[] fileHeader = null;
            byte[] fileData = null;

            // Screenshot Format Managers
            if (screenshotFormat == Format.JPG)
            {
                fileData = screenshotTexture.EncodeToJPG();
            }

            if (screenshotFormat == Format.PNG)
            {
                fileData = screenshotTexture.EncodeToPNG();
            }

            if (screenshotFormat == Format.PPM)
            {
                string headerName = string.Format("P6\n{0} {1}\n255\n", screenshotRect.width, screenshotRect.height);
                fileHeader = System.Text.Encoding.ASCII.GetBytes(headerName);
                fileData = screenshotTexture.GetRawTextureData();
            }

            if (screenshotFormat == Format.RAW)
            {
                fileData = screenshotTexture.GetRawTextureData();
            }

            // Screenshot Save Managers
            new System.Threading.Thread(() =>
            {
                var file = File.Create(fileName);

                if (fileHeader != null)
                {
                    file.Write(fileHeader, 0, fileHeader.Length);
                }

                file.Write(fileData, 0, fileData.Length);
                file.Close();
                print(string.Format("Capture Screenshot {0} of size {1}", fileName, fileData.Length));
            }).Start();

            // Screenshot Optimize Managers
            if (screenshotOptimize == false)
            {
                Destroy(screenshotRenderTexture);
                screenshotRenderTexture = null;
                screenshotTexture = null;
            }
        }
    }

    private string ScreenshotCreateNewFile(int width, int height)
    {
        if (screenshotFolder == null || screenshotFolder.Length == 0)
        {
            screenshotFolder = Application.dataPath;

            if (Application.isEditor)
            {
                var stringPath = screenshotFolder + "/..";
                screenshotFolder = Path.GetFullPath(stringPath);
            }

            screenshotFolder += "/Screenshots";
        }

        Directory.CreateDirectory(screenshotFolder);

        string mask = string.Format("Screenshot_{0}x{1}*.{2}", width, height, screenshotFormat.ToString().ToLower());
        screenshotIndex = Directory.GetFiles(screenshotFolder, mask, SearchOption.TopDirectoryOnly).Length;

        var filename = string.Format("{0}/Screenshot_{1}x{2}_{3}.{4}", screenshotFolder, width, height, System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss"), screenshotFormat.ToString().ToLower());
        ++screenshotIndex;
        return filename;
    }

}
