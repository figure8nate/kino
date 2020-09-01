using System.IO;
using UnityEngine;

namespace KN_Core {
  public class FilePicker {
    public string PickedFile { get; set; }
    public bool IsPicking { get; set; }

    private float filesListScrollH_;
    private float filesBoxHeight_;
    private Vector2 filesListScroll_;

    private string[] files_;
    private string folder_;

    public void Toggle(string folder) {
      IsPicking = !IsPicking;
      if (IsPicking) {
        PickIn(folder);
      }
      else {
        Reset();
      }
    }

    public void PickIn(string folder) {
      folder_ = folder;
      IsPicking = true;
      RefreshFiles();
    }

    public void Reset() {
      folder_ = null;
      PickedFile = null;
      IsPicking = false;
    }

    public void OnGui(Gui gui, ref float x, ref float y) {
      if (string.IsNullOrEmpty(folder_)) {
        return;
      }
      const float listHeight = 300.0f;
      const float baseWidth = Gui.Width * 1.7f;
      const float baseWidthScroll = Gui.WidthScroll * 1.7f + 10.0f;

      float yBegin = y;

      gui.Box(x, y, baseWidth + Gui.OffsetGuiX * 2.0f, Gui.Height, "FILE PICKER", Skin.MainContainerDark);
      y += Gui.Height;

      gui.Box(x, y, baseWidth + Gui.OffsetGuiX * 2.0f, filesBoxHeight_, Skin.MainContainer);
      y += Gui.OffsetY;
      x += Gui.OffsetGuiX;

      if (gui.Button(ref x, ref y, baseWidth, Gui.Height, "REFRESH", Skin.Button)) {
        RefreshFiles();
      }

      gui.BeginScrollV(ref x, ref y, baseWidth, listHeight, filesListScrollH_, ref filesListScroll_, $"FILES {files_.Length}");
      float sx = x;
      float sy = y;
      const float offset = Gui.ScrollBarWidth / 2.0f;
      bool scrollVisible = filesListScrollH_ > listHeight;
      float width = scrollVisible ? baseWidthScroll - offset : baseWidthScroll + offset;
      foreach (string f in files_) {
        string file = Path.GetFileName(f);
        sy += Gui.OffsetY;
        if (gui.Button(ref sx, ref sy, width, Gui.Height, $"{file}", Skin.Button)) {
          PickedFile = f;
        }
        sy -= Gui.OffsetY;
      }
      sy += Gui.OffsetY;
      filesListScrollH_ = gui.EndScrollV(ref x, ref y, sx, sy);

      filesBoxHeight_ = listHeight + Gui.Height * 2.0f + Gui.OffsetY * 3.0f;
      x += baseWidth + Gui.OffsetGuiX;
      y = yBegin;
    }

    private void RefreshFiles() {
      if (string.IsNullOrEmpty(folder_)) {
        return;
      }
      files_ = Directory.GetFiles(folder_);
    }
  }
}