using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FileImage;
using IMAPI2.Interop;
using System.IO;
using Helper;
namespace ISOBuilder
{
    public partial class ISOBuilder : Form
    {
        internal CustomList _items = new CustomList(1);
        //FileInfo dummy = new FileInfo(@"bad");
        ImageRepository _repository;
        public ISOBuilder()
        {
            InitializeComponent();
        }

        static void FormattingEvent(object o1, object o2)
        {
            //IMAPI2.Interop.IProgressItem it = o1 as IMAPI2.Interop.IProgressItem;
            //uint i = Convert.ToUInt32(o2);
            //if (it == null)
            //    Console.WriteLine("No Progress item, block = {0}", i);
            //Console.WriteLine("item = {0}, block = {1}", it.Description, i);

        }
        static void repository_Update(object o1, object o2)
        {
            //string file = o1 as string;
            //ulong i = Convert.ToUInt64(o2);
            //if (file == null)
            //    Console.WriteLine("No Progress file, size= {0}", i);
            //Console.WriteLine("file = {0}, size = {1}", file, i);

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _repository = new ImageRepository();
                //img.OutputFileName = @"C:\rohit.iso";
                //img.TreeRoot = @"C:\rohit";
                _repository.Update += new DiscFormat2Data_EventsHandler(repository_Update);

                //_items.Add(new DirectoryInfo(@"R:\"));
                _bindingSource.DataSource = _items;
                //_dataGridView.AutoGenerateColumns = true;
                _dataGridView.DataSource = _bindingSource;
                _cbxMediaType.DataSource = Enum.GetValues(typeof(IMAPI_MEDIA_PHYSICAL_TYPE));
                _cbxMediaType.SelectedItem = IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DISK;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                //Console.WriteLine(ex.ToString());
                if ((uint)ex.ErrorCode == 0x80040154)
                {
                    MessageBox.Show(this, "IMAPI2 is not installed on this machine.");
                }

                //return false;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void _btnBuildFile_Click(object sender, EventArgs e)
        {


            try
            {

                if (_items.Count == 0)
                {
                    MessageBox.Show(this, "No items to archive");
                    //_dataGridView.SuspendLayout();
                    //_tabBuild.SelectTab(tabPage1);
                    //_dataGridView.ResumeLayout(false);
                    return;
                }
                if (string.IsNullOrEmpty(_lblDest.Text))
                {
                    MessageBox.Show(this, "No destination was selected");
                    return;
                }
                IFileSystemImage ifsi = _repository as IFileSystemImage;
                FsiFileSystems fstype = default(FsiFileSystems);
                fstype |= _ckbISO9660.Checked ? FsiFileSystems.FsiFileSystemISO9660 : default(FsiFileSystems);
                fstype |= _ckbJoliet.Checked ? FsiFileSystems.FsiFileSystemJoliet : default(FsiFileSystems);
                fstype |= _ckbUDF.Checked ? FsiFileSystems.FsiFileSystemUDF : default(FsiFileSystems);
                ifsi.FileSystemsToCreate = fstype;
                //ifsi.FileSystemsToCreate = FsiFileSystems.FsiFileSystemJoliet | FsiFileSystems.FsiFileSystemISO9660;
                ifsi.ChooseImageDefaultsForMediaType((IMAPI_MEDIA_PHYSICAL_TYPE)_cbxMediaType.SelectedItem);
                //ifsi.ChooseImageDefaultsForMediaType(IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DISK);
                ifsi.VolumeName = _txtVolName.Text;
                IFileSystemImageResult res = ifsi.CreateResultImage();


                ISOImageBuilder frm = new ISOImageBuilder(@"C:\rohit.iso");
                //Console.WriteLine("burning image with progress {0}", frm.OutputFileName);
                DiscFormat2Data_Events ev = frm as DiscFormat2Data_Events;
                ev.Update += FormattingEvent;
                frm.CreateFullImage(res);
                ev.Update -= FormattingEvent;
            }
            catch (Exception ex)
            {

                MessageBox.Show(this, "unable to create image:" + ex.Message);
            }
            finally
            {
                //ev.Update -= FormattingEvent;
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {

        }

        //private void _bindingSource_AddingNew(object sender, AddingNewEventArgs e)
        //{
        //    if (_openFileDialog.ShowDialog(this) == DialogResult.OK)
        //    {

        //        e.NewObject = new FileInfo(_openFileDialog.FileName);
        //    }
        //    else
        //        e.NewObject = dummy;


        //    //_items.Add(new DirectoryInfo(@"R:\"));
        //}

        private void _openFileDialog_FileOk(object sender, CancelEventArgs e)
        {
            if (!_repository.AddNewFile(_openFileDialog.FileName))
            {
                MessageBox.Show(sender as IWin32Window, _openFileDialog.FileName + " is already added");
                e.Cancel = true;
            }
        }

        private void _bindingSource_DataError(object sender, BindingManagerDataErrorEventArgs e)
        {

        }

        private void _btnAddFile_Click(object sender, EventArgs e)
        {
            if (_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {

                _bindingSource.Add(new ItemInfo(new FileInfo(_openFileDialog.FileName)));
                //_dataGridView.Refresh();

            }

        }

        private void _btnAddFolder_Click(object sender, EventArgs e)
        {

            if (_folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {

                if (!_repository.AddNewFolder(_folderBrowserDialog.SelectedPath))
                {
                    MessageBox.Show(sender as IWin32Window, _folderBrowserDialog.SelectedPath + " is already added");

                }
                else
                {
                    _bindingSource.Add(new ItemInfo(new DirectoryInfo(_folderBrowserDialog.SelectedPath)));
                    //_dataGridView.Refresh();
                }

            }

        }

        private void _SaveAs_Click(object sender, EventArgs e)
        {
            if (_saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                _lblDest.Text = _saveFileDialog.FileName;
                //_btnBuildFile.Enabled = true;
            }
        }

    }

}
