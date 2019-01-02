﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
namespace Monoedit
{
    public partial class AddEditImage : ToolWindowBase
    {
        public ImageResource ImageResource { get; private set; }
        private SelectFile _objSelectFile = new SelectFile();

        public AddEditImage()
        {
            InitializeComponent();
            Globals.SetTooltip(
            new List<Control>() { _txtActualPath, _lblProjectPath },
            new Phrase(
                "The path of the file after being copied to the project directory."
                    ,
                "La ruta del archivo después de copiarse en el directorio del proyecto."
                ));
            Globals.SetTooltip(
            new List<Control>() { _lblImageType, _optImage, _optAtlas },
            new Phrase(
                "Atlas images are image files packed with multiple images.  Select Image if the file is just a single image."
                    ,
                "Las imágenes de atlas son archivos de imágenes empaquetadas con múltiples imágenes.Seleccione Imagen si el archivo es solo una imagen."
                ));
        }
        private void AddEditImage_Load(object sender, EventArgs e)
        {
            _objSelectFile.Filter = Globals.SupportedLoadSpriteImageFilter;
            _objSelectFile.DefaultExt = Globals.SupportedLoadSpriteImageFilterDefault;
            _objSelectFile.InitialDir = Globals.MainForm.ProjectFile.GetProjectRoot();
            _objSelectFile.Instruction = "Select Image";
            _objSelectFile.PathText = System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "Mono Projects", "MyProject");
            _objSelectFile.Mode = SelectFileMode.File;
            _objSelectFile.TextChanged = () =>
            {
                //when selected file changed load and update.
                UpdateImagePreview();
                UpdateActualPath();

            };
            Globals.SwapControl(_pnlLocation, _objSelectFile);
            _pbImage.SizeMode = PictureBoxSizeMode.Zoom;
            ToggleShowAtlasParameters();
        }

        public void ShowForm(AddEditMode e, IWin32Window owner, ImageResource rsc, Action afterShow)
        {
            if (e == AddEditMode.Add)
            {
                ImageResource = new ImageResource();
                Title = Translator.Translate(Phrases.AddImage);
            }
            else
            {
                ImageResource = rsc;
                Title = Translator.Translate(Phrases.EditImage);

                LoadData();
            }

            AfterShowDialog = afterShow;
            Globals.MainForm.ShowForm(Title, this, true, afterShow, owner);
        }

        private void LoadData()
        {
            _txtImageName.Text = ImageResource.Name;
            _objSelectFile.PathText = ImageResource.Path;
            _optAtlas.Checked = ImageResource.ImageType == ImageType.Atlas;
            _optImage.Checked = ImageResource.ImageType == ImageType.Image;
            _txtTileWidth.Text = ImageResource.TileWidth.ToString();
            _txtTileHeight.Text = ImageResource.TileHeight.ToString();

            UpdateActualPath();
        }
        private void SaveData()
        {
            ImageResource.Name = _txtImageName.Text;
            ImageResource.Path = _txtActualPath.Text;
            if (_optAtlas.Checked) ImageResource.ImageType = ImageType.Atlas;
            if (_optImage.Checked) ImageResource.ImageType = ImageType.Image;
            ImageResource.TileWidth = Globals.StrToInt32(_txtTileWidth.Text, 32);
            ImageResource.TileHeight = Globals.StrToInt32(_txtTileHeight.Text, 32);
        }

        #region UI 
        private void _btnOk_Click(object sender, EventArgs e)
        {
            OkButtonClicked();
        }
        private void _btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
        private void _btnReload_Click(object sender, EventArgs e)
        {
            UpdateImagePreview();
        }

        private void _optImage_CheckedChanged(object sender, EventArgs e)
        {
            ToggleShowAtlasParameters();
        }
        private void _optAtlas_CheckedChanged(object sender, EventArgs e)
        {
            ToggleShowAtlasParameters();
        }
        #endregion

        #region Private:Methods
        private void UpdateImagePreview()
        {
            Bitmap bmp = null;
            try
            {
                if (System.IO.File.Exists(_objSelectFile.PathText))
                {
                    bmp = new System.Drawing.Bitmap(_objSelectFile.PathText);
                }
            }
            catch (Exception ex)
            {
                Globals.LogError(ex.ToString());
            }
            _pbImage.Image = bmp;
        }
        private void UpdateActualPath()
        {
            string selected = _objSelectFile.PathText;
            string fn = System.IO.Path.GetFileName(selected);
            _txtActualPath.Text = System.IO.Path.Combine(Globals.ImagesFolder, fn);
        }
        protected override List<String> Validate()
        {
            List<string> results = new List<string>();
            _txtImageName.ForeColor = Color.Black;
            _objSelectFile.FileLocationTextBox.ForeColor = Color.Black;

            if (String.IsNullOrEmpty(ImageResource.Name))
            {
                results.Add("Please set the resource name.");
                _txtImageName.ForeColor = Color.Red;
            }

            foreach (ImageResource ir in Globals.MainForm.ProjectFile.Images)
            {
                if (ir.Name.Equals(ImageResource.Name))
                {
                    if (ir != ImageResource)
                    {
                        results.Add("Resource name '" + ir.Name + "' must be unique.");
                        _txtImageName.ForeColor = Color.Red;
                        break;
                    }
                }
            }

            if (String.IsNullOrEmpty(ImageResource.Path))
            {
                results.Add("Please set image path.");
                _objSelectFile.FileLocationTextBox.ForeColor = Color.Red;
            }

            if (System.IO.File.Exists(Globals.ResolvePath(_objSelectFile.PathText)) == false)
            {
                results.Add("Image file does not exist.");
                _objSelectFile.FileLocationTextBox.ForeColor = Color.Red;
            }

            return results;
        }
        public override bool OkButtonClicked()
        {

            SaveData();

            SaveAndClose(() =>
            {
                //All resources are now copied
                if (CopyImageToProjectDir() == false)
                {
                    return false;
                }

                //Vvalidated successfully
                DialogResult = true;

                //Add resource to project file.
                if (AddEditMode == AddEditMode.Add)
                {
                    if (!Globals.MainForm.ProjectFile.Images.Contains(ImageResource))
                    {
                        Globals.MainForm.ProjectFile.Images.Add(ImageResource);
                    }
                    else
                    {
                        //test
                        Globals.LogError("Image resource was already found in project.");
                        System.Diagnostics.Debugger.Break();
                    }
                }
                else if (AddEditMode == AddEditMode.Edit)
                {
                    //Do nothin
                }
                else
                {
                    throw new NotImplementedException();
                }

                return true;
            });
            return false;
        }
        private bool CopyImageToProjectDir()
        {
            string p0 = System.IO.Path.GetFullPath(_txtActualPath.Text);
            string p1 = System.IO.Path.GetFullPath(ImageResource.Path);

            if ((AddEditMode == AddEditMode.Edit && p0 != p1) || AddEditMode == AddEditMode.Add)
            {
                if (CopyResourceToLocalResourceFolder() == false)
                {
                    return false;
                }
            }

            return true;
        }
        private bool CopyResourceToLocalResourceFolder()
        {
            //Copy the given image to the ./{ProjectRoot}/images folder
            //Returns false if the user cancelled the operation
            try
            {
                //Not necessary we deal with absolutes anyway.
                //Globals.MainForm.ProjectFile.SetCwdToRoot();

                string projectPath = Globals.ResolvePath(ImageResource.Path);
                string filePath = Globals.ResolvePath(_objSelectFile.PathText);

                if (Globals.PathsAreEqual(projectPath, filePath))
                {
                    Globals.MainForm.SetStatus("Image " + projectPath + 
                        " already located in /images folder, no copy performed.");
                }
                else
                {
                    //Check to see if the file exists.  If it does then prompt hte user that there is a conflict.
                    bool? continueCopy = CheckImageDoesntExistBeforeCopying(projectPath);

                    if (continueCopy == null)
                    {
                        return false;
                    }
                    else if (continueCopy == true)
                    {
                        if (System.IO.File.Exists(projectPath))
                        {
                            System.IO.File.Delete(projectPath);
                        }
                        //Imageresource path is a wildcard path.
                        System.IO.File.Copy(filePath, projectPath);
                    }

                    //Set path and reload.
                    ImageResource.LoadIfNecessary(true);
                }
            }
            catch (Exception ex)
            {
                Globals.LogError("Failed to copy image to ouput directory: " + ex.ToString(), true);
                throw ex;
            }
            return true;
        }
        private bool? CheckImageDoesntExistBeforeCopying(string imgNewFileName)
        {
            //Check to see if the file exists.  If it does then prompt hte user that there is a conflict.
            //returns true if teh copy continues
            //return null if the user cancelled the operation
            //return false to stop the copy.
            if (System.IO.File.Exists(imgNewFileName))
            {
                string msg = "The file '' already exists.";

                string newRelpath = Globals.GetRelativePath(imgNewFileName, Globals.MainForm.ProjectFile.GetProjectRoot());

                List<ImageResource> rs = Globals.MainForm.ProjectFile.Images.Where(x => Globals.PathsAreEqual(x.Path, newRelpath)).ToList();
                if (rs != null && rs.Count > 0)
                {
                    string tots = string.Join(", ", rs.Select(x => "  " + x.Name + "\r\n"));

                    msg += "It appears that the resource(s):\r\n '" + tots + "' \r\n is/are using this path.";
                }
                else
                {
                    msg += "No resources were found to use this resource. ";
                }
                msg += "Would you like to overwrite this resource? Note: Clicking 'No' will cause this new resource to use the " +
                "existing resource in the resource folder.  " +
                "To use a new resource click 'Cancel' , uncheck 'copy resource' above, and try saving again.";

                //file already exists! ask user about this.
                DialogResult r = CustomMessageBox.Show(msg, Phrases.Warning,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Asterisk);
                if (r == System.Windows.Forms.DialogResult.Cancel)
                {
                    return null;
                }
                else if (r == System.Windows.Forms.DialogResult.Yes)
                {
                    return true;
                }
                else if (r == System.Windows.Forms.DialogResult.No)
                {
                    return false;
                }
            }

            return true;
        }

        private void ToggleShowAtlasParameters()
        {
            bool vis = false;
            if (_optAtlas.Checked)
            {
                vis = true;
            }

            _gbpAtlasParameters.Enabled =
            _lblAtlasParameters.Enabled = vis;
        }



        #endregion

        private void metroCheckBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }





}
