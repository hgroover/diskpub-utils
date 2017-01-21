/*
 * Please leave this Copyright notice in your code if you use it
 * Written by Decebal Mihailescu [http://www.codeproject.com/script/articles/list_articles.asp?userid=634640]
 * (CodeProject URL changed, now at https://www.codeproject.com/Articles/23653/How-to-Create-Optical-File-Images-using-IMAPIv )
 * Automation for command-line arguments added by Henry Groover (hgroover on github)
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using COMTypes = System.Runtime.InteropServices.ComTypes;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using FileImage;
using IMAPI2.Interop;
using System.IO;
using System.Diagnostics;
using AboutUtil;
using System.Text;
using System.Threading;
using Microsoft.Win32;
using Org.Mentalis.Utilities;

namespace ISOBuilder
{
    public partial class ISOBuilderForm : Form
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        internal static extern IntPtr PostMessage(HandleRef hWnd, uint Msg, ushort wParam, IntPtr lParam);
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetForegroundWindow(IntPtr hWnd);
        Stopwatch _tm = new Stopwatch();
        ImageRepository _repository;
        string _bootableImageFile;
        string _statusFile; // If specified via --statusfile=
        bool _automate; // If specified via --automate
        int _automateCompletionAction; // If specified via --completionaction= (default 0)
        string _isoFile; // If specified via --isofile=
        string _burnerDrive; // Specified via --burner=
        int _burnerSpeed; // Specified via --speed=
        string _mediaType; // Specified via --mediatype=
        System.Windows.Forms.Timer _automationTimer;
        static ISOBuilderForm _singleton;
        int _automationPass;
        public ISOBuilderForm()
        {
            _repository = new ImageRepository();
            _burnerDrive = "";
            _isoFile = "";
            _statusFile = "isoburner-status.txt";
            _burnerSpeed = 0; // Max
            _automate = false;
            _automationTimer = new System.Windows.Forms.Timer();
            _singleton = this;
            _automationPass = 0;
            InitializeComponent();
        }
        public void SetAutomation( bool enable )
        {
            _automate = enable;
        }
        public void SetCompletionAction( int completionAction )
        {
            _automateCompletionAction = completionAction;
        }
        public void SetStatusFile(string statusFile)
        {
            _statusFile = statusFile;
        }
        public void SetISOFile(string isoFile)
        {
            _isoFile = isoFile;
        }
        public void SetBurnerDrive(string drive)
        {
            _burnerDrive = drive;
        }
        public void SetBurnerSpeed(int speed)
        {
            _burnerSpeed = speed;
        }
        public void SetMediaType(string mediaType)
        {
            _mediaType = mediaType;
        }
        private void EjectMedia()
        {
            try
            {
                MsftDiscRecorder2 discRecorder = new MsftDiscRecorder2();
                discRecorder.InitializeDiscRecorder(CrtRecorder.ActiveDiscRecorder);
                discRecorder.EjectMedia();
            }
            catch (Exception e)
            {
                WriteStatus("error Eject failed: " + e.ToString());
            }
        }
        static void timer_Tick(object sender, EventArgs e)
        {
            _singleton.TimerEvent();
        }
        private void TimerEvent()
        {
            if (_automationPass >= 20)
            {
                Close();
            }
            else if (_automationPass < 10)
            {
                switch (_automationPass)
                {
                    case 0: // Set burner drive
                        if (_cbxBurners.DataSource == null) return;
                        _lblUpdate.Text = "Selecting drive " + _burnerDrive;
                        WriteStatus("msg Selecting burner drive " + _burnerDrive);
                        int findResult =_cbxBurners.FindString(_burnerDrive);
                        if (findResult < 0)
                        {
                            String burnerList;
                            burnerList = "Possible values -";
                            foreach (var i in _cbxBurners.Items)
                                burnerList += (" " + i.ToString());
                            WriteStatus("error Failed to select burner drive: " + burnerList);
                            _automationPass = 15;
                            // Can't eject since we don't have a drive letter
                            _lblUpdate.Text = "Failed, exiting without eject (invalid drive)";
                            return;
                        }
                        _cbxBurners.SelectedIndex = findResult;
                        _lblUpdate.Text = "Selected row " + findResult.ToString("D");
                        break;
                    // Select burner speed after delay
                    case 1: // Set speed
                        if (_cbxSpeed.DataSource == null)
                        {
                            _lblUpdate.Text = "Waiting for speed list";
                            return;
                        }
                        _lblUpdate.Text = "Selecting speed " + _burnerSpeed.ToString("D");
                        // 0 means highest speed
                        // -10 is lowest
                        if (_burnerSpeed == -10)
                        {
                            SetMinimumSpeed();
                            break;
                        }
                        // Otherwise match highest speed not exceeding specified limit
                        //set to minimum speed
                        IList<KeyValuePair<IWriteSpeedDescriptor, string>> lst = _cbxSpeed.DataSource as IList<KeyValuePair<IWriteSpeedDescriptor, string>>;
                        if (lst == null || lst.Count == 0)
                        {
                            WriteStatus("error Failed to get drive speed list");
                            _automationPass = 15;
                            _lblUpdate.Text = "Fatal error: could not get drive speed list";
                            EjectMedia();
                            return;
                        }
                        KeyValuePair<IWriteSpeedDescriptor, string> valMax = lst[0];
                        KeyValuePair<IWriteSpeedDescriptor, string> valMin = lst[0];
                        KeyValuePair<IWriteSpeedDescriptor, string> valSel = lst[0];
                        foreach (KeyValuePair<IWriteSpeedDescriptor, string> decr in lst)
                        {
                            if (decr.Key.WriteSpeed > valMax.Key.WriteSpeed)
                                valMax = decr;
                            if (decr.Key.WriteSpeed < valMin.Key.WriteSpeed)
                                valMin = decr;
                            // Select entry which is greater than what we started with but within
                            // specified limit, or if initial value is not within specified limit but
                            // this one is, select it.
                            if ((decr.Key.WriteSpeed > valSel.Key.WriteSpeed && decr.Key.WriteSpeed <= _burnerSpeed) 
                                || (decr.Key.WriteSpeed <= _burnerSpeed && valSel.Key.WriteSpeed > _burnerSpeed))
                                valSel = decr;
                        }
                        if (_burnerSpeed == 0)
                        {
                            valSel = valMax;
                        }
                        else if (valSel.Key.WriteSpeed > _burnerSpeed)
                        {
                            WriteStatus("error Could not select specified speed; slowest is " + valMin.Key.WriteSpeed.ToString("D"));
                            _lblUpdate.Text = "Failed to select speed " + _burnerSpeed.ToString("D");
                            _automationPass = 15;
                            EjectMedia();
                            return;
                        }
                        _cbxSpeed.SelectedItem = valSel;
                        WriteStatus("msg Selected drive speed " + valSel.Key.WriteSpeed.ToString("D"));
                        _lblUpdate.Text = "Selected speed " + valSel.Key.WriteSpeed.ToString("D");
                        break;
                    // Get media type after delay
                    case 2:
                        _lblUpdate.Text = "Got media type: " + _txtCrtDiskType.Text;
                        WriteStatus("msg Media type: " + _txtCrtDiskType.Text);
                        break;
                    // Start burn after delay
                    case 4:
                        _btnBurn.PerformClick();
                        break;
                    // Wait for burn completion
                    case 5:
                        return;
                }
                //WriteStatus("Timer: " + String.Format("{0:D}", _automationPass));
            }
            _automationPass++;
        }
        void WriteStatus(string text)
        {
            using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(_statusFile, true))
            {
                file.WriteLine(text);
            }
        }
        public IDiscRecorder2 CrtRecorder
        {
            get
            {
                switch (_tabFormat.SelectedIndex)
                {
                    case 1:
                        return _cbxBurners.SelectedValue as IDiscRecorder2;


                    case 2:
                        return _cbxFormatBurners.SelectedValue as IDiscRecorder2;

                    default:
                        return null;

                }

            }
        }
        void AsyncFormattingEvent(object o1, object o2)
        {
            Invoke(new DiscFormat2Data_EventsHandler(FormattingEvent), new Object[] { o1, o2 });
        }

        void FormattingEvent(object o1, object o2)
        {
            IMAPI2.Interop.IProgressItem it = o1 as IMAPI2.Interop.IProgressItem;
            int i = (int)(Convert.ToSingle(o2) * 100);
            this._progBar.Value = 100 + i;
            if (it != null)
                this._lblUpdate.Text = string.Format("Formatting {0}", it.Description);
            if (!_ckWorker.Checked)
                Application.DoEvents();


        }
        void AsyncRepositoryUpdate(object o1, object o2)
        {
            Invoke(new DiscFormat2Data_EventsHandler(RepositoryUpdate), new Object[] { o1, o2 });
        }
        void RepositoryUpdate(object o1, object o2)
        {
            string file = o1 as string;
            long i = Convert.ToInt64(o2);
            int pos = (int)((double)_repository.ActualSize / _repository.SizeBeforeFormatting * 100);
            _progBar.Value = pos;
            _lblUpdate.Text = string.Format("Adding {0} size = {1}", file, i);
            if (!_ckWorker.Checked)
                Application.DoEvents();

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                _lblUpdate.Text = _lblFileImage.Text = _lblDest.Text = string.Empty;
                _bindingSource.DataSource = _repository.Items;
                _dataGridView.DataSource = _bindingSource;


                _cbxAfterCompletion.DataSource = WindowsController.PossibleRestartOptions;
                _cbxAfterCompletion.DisplayMember = "Value";
                _cbxAfterCompletion.ValueMember = "Key";
                _cbxAfterCompletion.SelectedValue = RestartOptions.DoNothing;
                //if (!WindowsController.CanShutDown)
                {
                    _ckbForceTermination.Checked = false;
                    _ckbForceTermination.Enabled = false;
                }
                IList<KeyValuePair<IMAPI_MEDIA_PHYSICAL_TYPE, string>> lst = BurnHelper.GetMediaTypes();
                _cbxMediaTypes.DataSource = lst;
                _cbxMediaTypes.DisplayMember = "Value";
                _cbxMediaTypes.ValueMember = "Key";
                _cbxMediaTypes.SelectedValue = IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DISK;
                DateTime now = DateTime.Now;
                _txtVolName.Text = now.Year + "_" + now.Month + "_" + now.Day;

                About.InitSysMenu(this);
                if (_automate)
                {
                    WriteStatus("msg Started automation iso " + _isoFile);
                    _lblFileImage.Text = _isoFile;
                    _tabBuild.SelectedTab = _tabPageBuild;
                    _tabFormat.SelectedTab = _tabPageBurn;
                    // Select burner drive after delay
                    if (_burnerDrive.Length > 0)
                    {
                        _automationPass = 0;
                    }
                    else
                    {
                        _automationPass = 1;
                    }
                    // Select burner speed after delay
                    // Select media type after delay
                    // Start burn after delay
                    _automationTimer.Enabled = true;
                    _automationTimer.Interval = 1000;
                    _automationTimer.Tick += new EventHandler(timer_Tick);
                    _automationTimer.Start();
                }
                /***
                string[] argv = Environment.GetCommandLineArgs();
                if (argv.Length > 1 && System.IO.File.Exists(argv[1]))
                {

                    _lblFileImage.Text = argv[1];
                    _tabBuild.SelectedTab = _tabPageBuild;
                    _tabFormat.SelectedTab = _tabPageBurn;

                }
                ***/
            }
            catch (TypeInitializationException ex)
            {

                if (ex.InnerException != null && ex.InnerException is COMException)
                    CheckForIMAPI(ex.InnerException as COMException);
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                CheckForIMAPI(ex);

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void CheckForIMAPI(COMException ex)
        {
            if ((uint)ex.ErrorCode == 0x80040154)
            {
                if (MessageBox.Show(this, "IMAPI2 is not installed on this machine.\nDo you want to install it?",
    "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                    System.Diagnostics.Process.Start("Iexplore.exe", @"http://support.microsoft.com/kb/KB932716");
                this.Close();
            }
        }

        event ImageRepository.CancelingHandler Stop;


        EventWaitHandle _waitForSelectionEvent = null;
        void WaitForSelection(object val)
        {

            {
                if (MessageBox.Show(this, "Do you want to close other apps and try again?",
                    "Can't write " + val as string,
                    MessageBoxButtons.YesNo) == DialogResult.No)
                    _backgroundISOWorker.CancelAsync();
                if (_waitForSelectionEvent != null)
                    _waitForSelectionEvent.Set();
            }
        }
        // This event handler is where the actual,
        // potentially time-consuming work is done.
        private void backgroundWorkerCreateISOFile_DoWork(object sender,
            DoWorkEventArgs e)
        {
            ISOFormatter frm = null;
            COMTypes.IStream imagestream = null;
            string filename = e.Argument as string;
            try
            {
                _busy = true;
                e.Result = null;

                IFileSystemImage ifsi = _repository as IFileSystemImage;

                IFileSystemImageResult res = ifsi.CreateResultImage();
                if (res == null)
                    return;

                frm = new ISOFormatter(filename);

                this.Stop += frm.CancelOp;

                if (_repository.Update != null)
                {
                    DiscFormat2Data_Events ev = frm as DiscFormat2Data_Events;
                    ev.Update += AsyncFormattingEvent;
                }
                imagestream = frm.CreateImageStream(res);
                IDiscFormat2Data idf = frm as IDiscFormat2Data;
                try
                {
                    idf.Write(imagestream);
                }
                catch (ApplicationException)
                {
                    throw;
                }
                catch (IOException)
                {
                    if (_waitForSelectionEvent == null)
                        _waitForSelectionEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
                    this.Invoke(new WaitCallback(WaitForSelection), filename);

                    _waitForSelectionEvent.WaitOne();
                    if (_backgroundISOWorker.CancellationPending)

                        throw;

                    idf.Write(imagestream);
                }
                catch (COMException ex)
                {

                    if (ex.ErrorCode == -2147024864)
                    {

                        if (_waitForSelectionEvent == null)
                            _waitForSelectionEvent = new EventWaitHandle(false, EventResetMode.AutoReset);
                        this.Invoke(new WaitCallback(WaitForSelection), filename);

                        _waitForSelectionEvent.WaitOne();
                        if (_backgroundISOWorker.CancellationPending)

                            throw;

                        idf.Write(imagestream);

                    }
                    else
                        throw;
                }

                e.Result = frm;
            }
            catch (System.Runtime.InteropServices.COMException ex)
            {
                if (ex.ErrorCode == -1062555360)
                {
                    throw new ApplicationException("Media size could be too small for the amount of data", ex);
                }
                else
                    throw;
            }
            catch (ApplicationException)
            {
                e.Result = null;
                throw;
            }
            catch (Exception)
            {

                e.Result = null;
                throw;
            }
            finally
            {
                if (imagestream != null)
                {
                    Marshal.FinalReleaseComObject(imagestream);
                    imagestream = null;
                }
                e.Cancel = _backgroundISOWorker.CancellationPending;
            }

        }
        private void UpdateUI(bool enable)
        {
            foreach (TabPage tab in _tabFormat.TabPages)
            {
                foreach (Control ctr in tab.Controls)
                    if (ctr.Handle == _btnBuildFile.Handle || ctr.Handle == _btnFormat.Handle || ctr.Handle == _btnBurn.Handle)
                    {
                        ctr.Enabled = true;
                        ctr.UseWaitCursor = false;
                    }
                    else
                    {
                        ctr.UseWaitCursor = !enable;
                        ctr.Enabled = enable;
                    }
            }
            string btntxt = "&" + _tabFormat.SelectedTab.Text;
            switch (_tabFormat.SelectedIndex)
            {
                case 1:
                    _btnBurn.Text = enable ? btntxt : "&Cancel";
                    break;

                case 2:
                    _btnFormat.Text = enable ? btntxt : "&Cancel";
                    break;
                case 0:
                    _btnBuildFile.Text = enable ? btntxt : "&Cancel";
                    break;
            }
        }
        private void _btnBurn_Click(object sender, EventArgs e)
        {
            if (_backgroundWorkerBurn.CancellationPending)
                return;
            if (_btnBurn.Text == "&Cancel")
            {
                _backgroundWorkerBurn.CancelAsync();
                _btnBurn.Enabled = false;
                _btnBurn.UseWaitCursor = true;
                return;
            }
            try
            {
                if (!BurnHelper.CanWriteMedia(CrtRecorder))
                {
                    _btnBurn.Enabled = false;
                    if (_automate)
                    {
                        WriteStatus("error Cannot write media");
                        _lblUpdate.Text = "Cannot write media";
                        // Eject
                        _automationPass = 15;
                        EjectMedia();
                    }
                }
                else
                {
                    if (BurnHelper.IsMediaBlank(CrtRecorder))
                    {
                        if (string.IsNullOrEmpty(_lblFileImage.Text))
                        {
                            if (MessageBox.Show(this, "Select a file image before burning.", "no Image", MessageBoxButtons.OKCancel) == DialogResult.OK)
                                using (OpenFileDialog dlg = new OpenFileDialog())
                                {
                                    dlg.Filter = "iso files|*.iso|mdf files|*.mdf|bwt files|*.bwt|All files|*.*";
                                    if (dlg.ShowDialog(this) == DialogResult.OK)
                                        _lblFileImage.Text = dlg.FileName;
                                }
                        }
                        else
                        {
                            UpdateUI(false);
                            _btnBurn.Enabled = true;
                            _btnBurn.UseWaitCursor = false;
                            string initburner = (_cbxBurners.SelectedValue as IDiscRecorder2).ActiveDiscRecorder;
                            object[] args = new object[] { initburner, _ckbCloseMedia.Checked, Convert.ToInt32(_cbxSpeed.Text), _ckbEject.Checked };
                            _tm.Reset();
                            _tm.Start();
                            _backgroundWorkerBurn.RunWorkerAsync(args);


                        }
                    }
                    else if (_automate)
                    {
                        _btnBurn.Enabled = false;
                        WriteStatus("error Disk not empty, cannot burn");
                        _lblUpdate.Text = "Selected disk not empty";
                        // Eject
                        _automationPass = 15;
                        EjectMedia();
                    }
                    else
                    {
                        MessageBox.Show(this, "The disk is not empty.You must format before continuing..", "not empty");
                        _btnBurn.Enabled = false;
                        _tabFormat.SelectedTab = _tabPageformat;
                    }
                }

            }

            catch
            {
                return;
            }
        }

        private void _btnFormat_Click(object sender, EventArgs e)
        {
            if (_backgroundWorkerErase.CancellationPending)
                return;
            if (this._btnFormat.Text == "&Cancel")
            {
                _backgroundWorkerErase.CancelAsync();
                _btnFormat.Enabled = false;
                _backgroundWorkerErase.Dispose();//kill thread
                return;
            }
            try
            {

                if (!_ckbRepair.Checked && !BurnHelper.CanWriteMedia(CrtRecorder))
                {
                    _btnFormat.Enabled = false;
                    _btnFormat.Text = "&Cancel";
                    return;
                }

                if (!_ckbRepair.Checked && BurnHelper.IsMediaBlank(CrtRecorder))
                {
                    this.RefreshFormatBurners();
                    if (MessageBox.Show(this, "The disk is already blank.do you want to comtinue?", "no Image", MessageBoxButtons.YesNo) == DialogResult.No)
                    {
                        _btnFormat.Enabled = true;
                        _btnFormat.Text = "&Format";
                        return;
                    }

                }
                UpdateUI(false);
                string initburner = CrtRecorder.ActiveDiscRecorder;
                object[] args = new object[] { initburner, _ckQuick.Checked, _ckbEjectFormat.Checked };
                _tm.Reset();
                _tm.Start();
                _btnFormat.Enabled = true;
                _btnFormat.UseWaitCursor = false;
                this._backgroundWorkerErase.RunWorkerAsync(args);


            }



            catch (Exception)
            {
                return;
            }
        }
        private void _btnBuildFile_Click(object sender, EventArgs e)
        {
            if (_btnBuildFile.Text == "&Cancel")
            {
                if (_ckWorker.Checked && _backgroundISOWorker.IsBusy)
                {

                    _backgroundISOWorker.CancelAsync();
                }
                this.Stop();

                _btnBuildFile.Enabled = false;
                _btnBuildFile.UseWaitCursor = true;
                Application.DoEvents();
                return;
            }

            CreateFileImage(new DoWorkEventHandler(backgroundWorkerCreateISOFile_DoWork), new RunWorkerCompletedEventHandler(backgroundWorkerCreateISOFile_RunWorkerCompleted));

        }

        private void CreateFileImage(DoWorkEventHandler DoWork, RunWorkerCompletedEventHandler CompletedEvent)
        {
            _tm.Reset();
            _tm.Start();
            _progBar.Value = 0;

            IFileSystemImageResult imageResult = null;
            ISOFormatter formatter = null;
            COMTypes.IStream imagestream = null;
            try
            {

                if (_repository.Items.Count == 0)
                {
                    MessageBox.Show(this, "No items to archive");
                    return;
                }
                if (string.IsNullOrEmpty(_lblDest.Text))
                {
                    MessageBox.Show(this, "No destination was selected", "Warning");
                    if (_saveFileDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        _lblDest.Text = _saveFileDialog.FileName;
                    }
                    else
                        return;
                }
                UpdateUI(false);
                IFileSystemImage ifsi = InitRepository();

                if (_ckWorker.Checked)
                {
                    _btnBuildFile.Enabled = true;
                    _backgroundISOWorker.DoWork += DoWork;
                    _backgroundISOWorker.RunWorkerCompleted += CompletedEvent;
                    _backgroundISOWorker.RunWorkerAsync(_lblDest.Text);
                }
                else
                {
                    _busy = true;
                    _btnBuildFile.Enabled = _ckUseUIReport.Checked;
                    Application.DoEvents();
                    imageResult = ifsi.CreateResultImage();
                    if (imageResult == null)
                    {
                        if (_repository.Cancel)
                            _lblUpdate.Text = "Canceled on UI Thread";
                        return;
                    }

                    formatter = new ISOFormatter(_lblDest.Text);

                    this.Stop += formatter.CancelOp;

                    DiscFormat2Data_Events ev = formatter as DiscFormat2Data_Events;
                    if (_ckUseUIReport.Checked)
                        ev.Update += FormattingEvent;
                    imagestream = formatter.CreateImageStream(imageResult);
                    IDiscFormat2Data idf = formatter as IDiscFormat2Data;

                    try
                    {
                        idf.Write(imagestream);
                    }
                    catch (ApplicationException)
                    {
                        throw;
                    }
                    catch (IOException)
                    {
                        WaitForSelection(_lblDest.Text);
                        if (_backgroundISOWorker.CancellationPending)
                            throw;
                        idf.Write(imagestream);
                    }
                    catch (COMException ex)
                    {
                        if (ex.ErrorCode == -2147024864)
                        {

                            WaitForSelection(_lblDest.Text);
                            if (_backgroundISOWorker.CancellationPending)
                                throw;
                            idf.Write(imagestream);

                        }
                        else
                            throw;
                    }

                    _tm.Stop();
                    if (formatter.Cancel)
                        _lblUpdate.Text = "Canceled on UI Thread";
                    else
                    {
                        _lblUpdate.Text = string.Format("Creating {0}  of size {1} on UI thread lasted {2}", Path.GetFileName(_lblDest.Text), (new FileInfo(_lblDest.Text)).Length.ToString("#,#"),
                       _tm.Elapsed.ToString());
                        WindowsController.ExitWindows((RestartOptions)_cbxAfterCompletion.SelectedValue, _ckbForceTermination.Checked);
                    }
                }

            }
            catch (System.Runtime.InteropServices.COMException ex)
            {

                Console.Beep();
                if (ex.ErrorCode == -1062555360)
                {
                    _lblUpdate.Text = "On UI Thread: Media size could be too small for the amount of data";
                }
                else
                    _lblUpdate.Text = "On UI Thread: " + ex.Message;
                if (ex.ErrorCode != -2147024864 && File.Exists(_lblDest.Text))
                    File.Delete(_lblDest.Text);
            }
            catch (Exception ex)
            {
                if (!this.IsDisposed)
                {
                    if (_repository.Cancel)
                        _lblUpdate.Text = "Canceled on UI thread";
                    else
                    {
                        Console.Beep();
                        _lblUpdate.Text = "Failed on UI thread: " + ex.Message;
                    }
                }
                if (!(ex is IOException) && File.Exists(_lblDest.Text))
                    File.Delete(_lblDest.Text);
            }
            finally
            {
                if (imagestream != null)
                {
                    Marshal.FinalReleaseComObject(imagestream);
                    imagestream = null;
                }
                if (_repository.Cancel && !string.IsNullOrEmpty(_lblDest.Text))
                    File.Delete(_lblDest.Text);
                else
                    if (!_ckWorker.Checked)
                        _lblFileImage.Text = _lblDest.Text;
                if (!_ckWorker.Checked)
                    RestoreUI(formatter);

            }
        }

        private IFileSystemImage InitRepository()
        {
            Application.DoEvents();
            IFileSystemImage ifsi = _repository as IFileSystemImage;
            FsiFileSystems fstype = FsiFileSystems.FsiFileSystemNone;
            fstype |= _ckbISO9660.Checked ? FsiFileSystems.FsiFileSystemISO9660 : FsiFileSystems.FsiFileSystemNone;
            fstype |= _ckbJoliet.Checked ? FsiFileSystems.FsiFileSystemJoliet : FsiFileSystems.FsiFileSystemNone;
            fstype |= _ckbUDF.Checked ? FsiFileSystems.FsiFileSystemUDF : FsiFileSystems.FsiFileSystemNone;
            ifsi.FileSystemsToCreate = fstype;

            _repository.BootableImageFile = _bootableImageFile;

            ifsi.ChooseImageDefaultsForMediaType((IMAPI_MEDIA_PHYSICAL_TYPE)_cbxMediaTypes.SelectedValue);
            ifsi.VolumeName = _txtVolName.Text;

            this.Stop += _repository.CancelOp;
            try
            {
                if (_ckUseUIReport.Checked)
                {
                    if (_ckWorker.Checked)
                        _repository.Update += new DiscFormat2Data_EventsHandler(AsyncRepositoryUpdate);
                    else
                        _repository.Update += new DiscFormat2Data_EventsHandler(RepositoryUpdate);
                    _lblUpdate.Text = string.Format("Calculating size for {0}...", _lblDest.Text);
                    Application.DoEvents();
#if DEBUG
                    Stopwatch tm = new Stopwatch();
                    tm.Start();
#endif
                    try
                    {
                        if (_ckWorker.Checked)
                            _progBar.Style = ProgressBarStyle.Marquee;
                        _repository.CalculateSizeBeforeFormatting();
                    }
                    finally
                    {
                        if (_ckWorker.Checked)
                            _progBar.Style = ProgressBarStyle.Continuous;

                    }
#if DEBUG
                    tm.Stop();
                    System.Diagnostics.Debug.WriteLine(string.Format("CalculateSizeBeforeFormatting the image lasted {0} ms", tm.Elapsed.TotalMilliseconds.ToString("#,#")));
#endif
                }
                else
                {
                    if (_ckWorker.Checked)
                        _progBar.Style = ProgressBarStyle.Marquee;
                    _lblUpdate.Text = string.Format("Creating {0}...", _lblDest.Text);
                }
            }
            finally
            {
            }

            return ifsi;
        }

        private void RestoreUI(ISOFormatter formatter)
        {
            _busy = false;
            _btnBuildFile.UseWaitCursor = false;
            if (_ckUseUIReport.Checked)
            {
                if (_ckWorker.Checked)
                    _repository.Update -= new DiscFormat2Data_EventsHandler(AsyncRepositoryUpdate);
                else
                    _repository.Update -= new DiscFormat2Data_EventsHandler(RepositoryUpdate);
            }

            _btnBuildFile.Text = '&' + _tabFormat.SelectedTab.Text;//"&Build file";

            if (formatter != null)
            {
                DiscFormat2Data_Events ev = formatter as DiscFormat2Data_Events;
                if (ev != null)
                {
                    if (_ckUseUIReport.Checked)
                    {
                        if (_ckWorker.Checked)
                            ev.Update -= AsyncFormattingEvent;
                        else
                            ev.Update -= FormattingEvent;
                    }
                    this.Stop -= formatter.CancelOp;
                }

                (formatter as IDisposable).Dispose();
                formatter = null;
            }


            this.Stop -= _repository.CancelOp;

            _repository.Reset();
            UpdateUI(true);
            _tm.Reset();
            if (_ckWorker.Checked)
                _progBar.Style = ProgressBarStyle.Continuous;

        }

        void backgroundWorkerCreateISOFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            _backgroundISOWorker.DoWork -= new DoWorkEventHandler(backgroundWorkerCreateISOFile_DoWork);
            _backgroundISOWorker.RunWorkerCompleted -= new RunWorkerCompletedEventHandler(backgroundWorkerCreateISOFile_RunWorkerCompleted);
            _tm.Stop();
            if (e.Cancelled)
            {
                _lblUpdate.Text = "Canceled on the worker thread";
                File.Delete(_lblDest.Text);
            }
            else
                if (e.Error != null)
                {
                    Console.Beep();
                    _lblUpdate.Text = "Err on the worker thread: " + e.Error.Message;
                    if (_backgroundISOWorker.CancellationPending)
                        File.Delete(_lblDest.Text);
                }
                else
                {
                    _lblUpdate.Text = string.Format("Creating {1}  of size {2} on worker thread lasted {0}",
                        _tm.Elapsed.ToString(), Path.GetFileName(_lblDest.Text), (new FileInfo(_lblDest.Text)).Length.ToString("#,#"));
                    WindowsController.ExitWindows((RestartOptions)_cbxAfterCompletion.SelectedValue, _ckbForceTermination.Checked);
                }
            ISOFormatter frm = null;
            if (!e.Cancelled && e.Error == null)
            {
                frm = e.Result as ISOFormatter;
                _lblFileImage.Text = _lblDest.Text;
            }

            RestoreUI(frm);

        }

        private void _btnAddFile_Click(object sender, EventArgs e)
        {
            if (_openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string path = _openFileDialog.FileName;
                _AddFile(path);

            }

        }

        private void _AddFile(string path)
        {
            if (_repository.AddNewFile(path))
            {
                _bindingSource.ResetBindings(false);
                List<FileSystemInfo> fli = _bindingSource.List as List<FileSystemInfo>;
                for (int i = 0; i < fli.Count; i++)
                {
                    FileSystemInfo fi = fli[i];
                    _dataGridView.Rows[i].Cells["Type"].Value = ((fi.Attributes & FileAttributes.Directory) == 0) ? "File" : "Folder";
                }
            }
            else
                Console.Beep();

        }

        private void _btnAddFolder_Click(object sender, EventArgs e)
        {

            if (_folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                string path = _folderBrowserDialog.SelectedPath;
                _AddFolder(path);
            }

        }

        private void _AddFolder(string path)
        {
            if (!_repository.AddNewFolder(path))
            {
                Console.Beep();
                MessageBox.Show(this, path + " is already added", "error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                _bindingSource.ResetBindings(false);
                List<FileSystemInfo> fli = _bindingSource.List as List<FileSystemInfo>;
                for (int i = 0; i < fli.Count; i++)
                {
                    FileSystemInfo fi = fli[i];
                    _dataGridView.Rows[i].Cells["Type"].Value = ((fi.Attributes & FileAttributes.Directory) == 0) ? "File" : "Folder";
                }

            }
        }

        private void _SaveAs_Click(object sender, EventArgs e)
        {
            if (_saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                _lblDest.Text = _saveFileDialog.FileName;
            }
        }


        private void ISOBuilderForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Stop != null && Stop.GetInvocationList().Length > 0)
                Stop();
            if (_devmon != null)
            {
                _devmon.DeviceAction -= new DeviceMonitor.DeviceEventHandler(DeviceEventAction);
                _devmon.Dispose();
            }
        }

        private void _btnAbout_Click(object sender, EventArgs e)
        {
            About dlg = new About(this);
            dlg.ShowDialog();
            dlg.Dispose();
        }

        private void tabPage1_DragDrop(object sender, DragEventArgs e)
        {
            Array arr = (Array)e.Data.GetData(DataFormats.FileDrop);
            if (arr != null && arr.Length > 0)
            {
                SetForegroundWindow(this.Handle);
                string path = arr.GetValue(0) as string;
                if (File.Exists(path))
                {
                    _AddFile(path);
                }
                else if (Directory.Exists(path))
                {
                    _AddFolder(path);
                }
            }
        }

        private void tabPage1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;

        }


        private void _cbxMakeBootable_Click(object sender, EventArgs e)
        {
            bool ck = _ckMakeBootable.Checked;
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                if (ck)
                {
                    dlg.Filter = "All files|*.*";
                    if (dlg.ShowDialog(this._ckMakeBootable) == DialogResult.Cancel)
                        _ckMakeBootable.Checked = !ck;
                }
                if (_ckMakeBootable.Checked)
                    _bootableImageFile = dlg.FileName;
                else
                    _bootableImageFile = null;
            }
        }
        void RefreshBurners()
        {
            if (_cbxBurners.DataSource != null)
            {

                try
                {
                    _lblUpdate.Text = BurnHelper.GetMediaStateDescription(CrtRecorder);
                    _txtCrtDiskType.Text = EnumHelper.GetDescription(BurnHelper.CurrentPhysicalMediaType(CrtRecorder));
                    _cbxSpeed.DataBindings.Clear();
                    _cbxSpeed.DisplayMember = "Value";
                    _cbxSpeed.ValueMember = "Key";
                    _cbxSpeed.DataSource = BurnHelper.GetSpeedDescriptors(CrtRecorder);


                }
                catch
                {
                    _cbxSpeed.DataSource = null;
                    _txtCrtDiskType.Text = EnumHelper.GetDescription(IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_UNKNOWN);
                    _lblUpdate.Text = "No Media or not supported.";
                }


            }
        }
        void RefreshFormatBurners()
        {
            if (_cbxFormatBurners.DataSource != null)
            {

                MsftDiscRecorder2 rec = _cbxFormatBurners.SelectedValue as MsftDiscRecorder2;

                {
                    try
                    {
                        _lblUpdate.Text = BurnHelper.GetMediaStateDescription(CrtRecorder);
                        _txtDiskTypeFormat.Text = EnumHelper.GetDescription(BurnHelper.CurrentPhysicalMediaType(CrtRecorder));
                    }
                    catch
                    {
                        _txtDiskTypeFormat.Text = EnumHelper.GetDescription(IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_UNKNOWN);
                        _lblUpdate.Text = "No Media or not supported.";
                    }

                }

            }
        }
        private void _cbxBurners_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshBurners();
            if (_devmon != null)
            {
                List<char> lst = new List<char>(1);
                lst.Add(_cbxBurners.Text[0]);
                _devmon.Drives = lst;
            }
            _btnBurn.Enabled = BurnHelper.CanWriteMedia(CrtRecorder);

        }



        void DeviceEventAction(IList<char> drives, DeviceEventBroadcast eventType)
        {

            List<char> lst = drives as List<char>;
            if (lst == null)
                return;
            switch (eventType)
            {
                case DeviceEventBroadcast.DBT_DEVICEARRIVAL:
                    _btnBurn.Enabled = BurnHelper.CanWriteMedia(CrtRecorder);
                    _btnFormat.Enabled = BurnHelper.CanWriteMedia(CrtRecorder);
                    if (_automate)
                    {
                        WriteStatus("msg Device arrival");
                    }
                    break;
                case DeviceEventBroadcast.DBT_DEVICEREMOVECOMPLETE:
                    _btnBurn.Enabled = false;
                    _btnFormat.Enabled = false;
                    if (_automate)
                    {
                        WriteStatus("msg Device removal complete");
                    }
                    break;
            }
            switch (this._tabBuild.SelectedIndex)
            {
                case 1:
                    RefreshBurners();
                    break;
                case 2:
                    RefreshFormatBurners();
                    break;
            }

        }
        DeviceMonitor _devmon;
        bool _busy;
        private void _tabTarget_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_busy)
            {
                e.Cancel = true;
                return;
            }
            List<char> drives = new List<char>(1);
            if (e.TabPage == _tabPageBurn)
            {

                InitBurnTabPage();
                if (_cbxBurners.DataSource != null)
                    drives.Add(_cbxBurners.Text[0]);
            }
            else if (e.TabPage == _tabPageformat)
            {
                InitFormatTabPage();
                if (_cbxFormatBurners.DataSource != null)
                    drives.Add(_cbxFormatBurners.Text[0]);
            }
            else
            {
                _progBar.Maximum = 200;
                _lblUpdate.Text = string.Empty;
            }
            if (e.TabPageIndex > 0 && _devmon == null && drives.Count > 0)
            {
                //drives.Add('E');
                _devmon = new DeviceMonitor(this, VolumeFlags.Media, DeviceType.Volume, drives);
                _devmon.DeviceAction += new DeviceMonitor.DeviceEventHandler(DeviceEventAction);
                string name = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                AddContextMenuItem(".iso", name, "Open with &" + name, "\"" + Application.ExecutablePath + "\" \"%1\"");

            }
            _progBar.Value = 0;

        }
        private void InitFormatTabPage()
        {
            try
            {
                _tabPageformat.UseWaitCursor = true;
                Application.DoEvents();
                // Initialize the DiscRecorder object for the specified burning device.
                if (_cbxFormatBurners.DataSource == null)
                {
                    _cbxFormatBurners.DataSource = BurnHelper.RecordersInfo;
                    if (_cbxFormatBurners.DataSource == null)
                        foreach (Control ct in _tabPageformat.Controls)
                            ct.Enabled = false;
                }
                else
                {
                    RefreshFormatBurners();
                }
                _btnFormat.Enabled = _cbxFormatBurners.DataSource != null && BurnHelper.CanWriteMedia(CrtRecorder);


            }
            catch (Exception)
            {
                _btnFormat.Enabled = false;
            }
            finally
            {
                _tabPageformat.UseWaitCursor = false;
                _progBar.Maximum = 100;
                _lblUpdate.Text = string.Empty;
            }
        }
        private void InitBurnTabPage()
        {
            try
            {
                _tabPageBurn.UseWaitCursor = true;
                Application.DoEvents();
                // Initialize the DiscRecorder object for the specified burning device.
                if (_cbxBurners.DataSource == null)
                {
                    _cbxBurners.DataSource = BurnHelper.RecordersInfo;
                    if (_cbxBurners.DataSource == null)
                        foreach (Control ct in _tabPageBurn.Controls)
                            ct.Enabled = false;

                }
                else
                {
                    RefreshBurners();
                }
                SetMinimumSpeed();
                _btnBurn.Enabled = _cbxBurners.DataSource != null && BurnHelper.CanWriteMedia(CrtRecorder);
            }
            catch (Exception ex)
            {
                _cbxBurners.DataSource = null;
                _btnBurn.Enabled = false;
                Console.WriteLine(ex.Message);
            }
            finally
            {
                _tabPageBurn.UseWaitCursor = false;
                _progBar.Maximum = 100;
                _lblUpdate.Text = string.Empty;
            }

        }

        private void SetMinimumSpeed()
        {
            //set to minimum speed
            IList<KeyValuePair<IWriteSpeedDescriptor, string>> lst = _cbxSpeed.DataSource as IList<KeyValuePair<IWriteSpeedDescriptor, string>>;
            if (lst == null || lst.Count == 0)
                return;
            KeyValuePair<IWriteSpeedDescriptor, string> val = lst[0];
            foreach (KeyValuePair<IWriteSpeedDescriptor, string> decr in lst)
            {
                if (decr.Key.WriteSpeed < val.Key.WriteSpeed)
                    val = decr;
            }
            _cbxSpeed.SelectedItem = val;//_cbxSpeed.SelectedValue = val.Key;
        }

        private static bool AddContextMenuItem(string Extension,
             string MenuName, string MenuDescription, string MenuCommand)
        {
            bool ret = false;

            try
            {
                RegistryKey rkey = Registry.ClassesRoot.OpenSubKey(Extension);
                if (rkey == null)
                {
                    rkey = Registry.ClassesRoot.CreateSubKey(Extension);
                    rkey.SetValue("", MenuName);
                }


                string appName = rkey.GetValue("").ToString();

                if (string.IsNullOrEmpty(appName))
                {
                    rkey.SetValue("", MenuName);
                    appName = MenuName;
                }
                rkey.Close();

                if (appName.Length > 0)
                {
                    rkey = Registry.ClassesRoot.OpenSubKey(appName, true);
                    if (rkey == null)
                        rkey = Registry.ClassesRoot.CreateSubKey(appName);

                    string strkey = "shell\\" + MenuName + "\\command";
                    RegistryKey subky = rkey.CreateSubKey(strkey);
                    if (subky != null)
                    {
                        subky.SetValue("", MenuCommand);
                        subky.Close();
                        subky = rkey.OpenSubKey("shell\\" + MenuName, true);
                        if (subky != null)
                        {
                            subky.SetValue("", MenuDescription);
                            subky.Close();
                        }
                        ret = true;
                    }
                    rkey.Close();

                }

            }
            catch
            {
                ret = false;
            }
            finally
            {


            }


            return ret;
        }

        //delegate void EnableControlsDelegate(bool act);
        //private void LookForInsertedMediaEvent()
        //{
        //    IDiscRecorder2 rec = BurnHelper.Instance.CrtRecorder;
        //    string drive = rec.VolumePathNames[0].TrimEnd(new char[] { '\\' });

        //    WqlEventQuery q;

        //    q = new WqlEventQuery();
        //    q.EventClassName = "__InstanceModificationEvent";
        //    q.WithinInterval = new TimeSpan(0, 0, 1);


        //    // DriveType - 5: CDROM
        //    q.Condition = string.Format(@"TargetInstance ISA 'Win32_LogicalDisk' and TargetInstance.Name ='{0}'", drive);
        //    //q.Condition = string.Format(@"TargetInstance ISA 'Win32_LogicalDisk' and TargetInstance.DriveType = 5 and TargetInstance.Name ='{0}'", drive);
        //    if (_eventWatcher == null)
        //    {
        //        //ManagementOperationObserver observer = new ManagementOperationObserver();
        //        // Bind to local machine
        //        ConnectionOptions opt = new ConnectionOptions();
        //        opt.EnablePrivileges = true; //sets required privilege
        //        ManagementScope scope = new ManagementScope("root\\CIMV2", opt);

        //        _eventWatcher = new ManagementEventWatcher(scope, q);


        //        // register async. event handler
        //        _eventWatcher.EventArrived += delegate(object sender, EventArrivedEventArgs e)
        //        {
        //            // Get the Event object and display it
        //            PropertyData pd = e.NewEvent.Properties["TargetInstance"];

        //            if (pd != null)
        //            {
        //                ManagementBaseObject mbo = pd.Value as ManagementBaseObject;
        //                // if CD removed VolumeName == null
        //                if (mbo.Properties["VolumeName"].Value != null)
        //                {
        //                    string target = mbo.Properties["Name"].Value.ToString();
        //                    //string drive = BurnHelper.Instance.CrtRecorder.VolumePathNames[0];
        //                    if (drive.Contains(target))
        //                        Console.WriteLine("CD has been inserted, unlocked in {0}", drive);
        //                    //_eventWatcher.Stop();
        //                }
        //                else
        //                {

        //                    Console.WriteLine("CD has been ejected");
        //                }
        //                Invoke(new EnableControlsDelegate(EnableControls), new object[1] { true });
        //            }
        //        };

        //    }
        //    else
        //    {
        //        _eventWatcher.Query = q;
        //    }
        //    lock (this)
        //    {
        //        try
        //        {
        //            rec.AcquireExclusiveAccess(true, "IMAPI2");
        //            rec.EjectMedia();
        //            EnableControls(false);
        //            _eventWatcher.Start();
        //            MessageBox.Show(this, string.Format("Insert some supported media in drive {0} before continuing.Use one of the following media:\n{1}", drive, BurnHelper.Instance.SupportedMediaTypesAsString));
        //            rec.CloseTray();
        //        }
        //        finally
        //        {

        //            this.Cursor = Cursors.WaitCursor;
        //            _lblUpdate.Text = string.Format("Checking the new media in drive {0}....", drive);
        //            Application.DoEvents();
        //            //if (BurnHelper.Instance.CurrentPhysicalMediaType == IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_UNKNOWN)
        //            if (!BurnHelper.Instance.CanWriteMedia)
        //                EnableControls(true);
        //            //rec.ReleaseExclusiveAccess();
        //        }
        //    }
        //}
        //private void LookForInsertedBlankMediaEvent()
        //{
        //    IDiscRecorder2 rec = BurnHelper.Instance.CrtRecorder;
        //    string drive = rec.VolumePathNames[0].TrimEnd(new char[] { '\\' });

        //    WqlEventQuery q;

        //    q = new WqlEventQuery();
        //    q.EventClassName = "__InstanceModificationEvent";
        //    q.WithinInterval = new TimeSpan(0, 0, 1);


        //    // DriveType - 5: CDROM
        //    q.Condition = string.Format(@"TargetInstance ISA 'Win32_LogicalDisk' and TargetInstance.Name ='{0}'", drive);
        //    //q.Condition = string.Format(@"TargetInstance ISA 'Win32_LogicalDisk' and TargetInstance.DriveType = 5 and TargetInstance.Name ='{0}'", drive);
        //    if (_eventWatcher == null)
        //    {
        //        //ManagementOperationObserver observer = new ManagementOperationObserver();
        //        // Bind to local machine
        //        ConnectionOptions opt = new ConnectionOptions();
        //        opt.EnablePrivileges = true; //sets required privilege
        //        ManagementScope scope = new ManagementScope("root\\CIMV2", opt);

        //        _eventWatcher = new ManagementEventWatcher(scope, q);


        //        // register async. event handler
        //        _eventWatcher.EventArrived += delegate(object sender, EventArrivedEventArgs e)
        //        {
        //            // Get the Event object and display it
        //            PropertyData pd = e.NewEvent.Properties["TargetInstance"];

        //            if (pd != null)
        //            {
        //                ManagementBaseObject mbo = pd.Value as ManagementBaseObject;
        //                // if CD removed VolumeName == null
        //                if (mbo.Properties["VolumeName"].Value != null)
        //                {
        //                    string target = mbo.Properties["Name"].Value.ToString();
        //                    //string drive = BurnHelper.Instance.CrtRecorder.VolumePathNames[0];
        //                    if (drive.Contains(target))
        //                        Console.WriteLine("CD has been inserted, unlocked in {0}", drive);
        //                    //_eventWatcher.Stop();
        //                }
        //                else
        //                {

        //                    Console.WriteLine("CD has been ejected");
        //                }
        //                Invoke(new EnableControlsDelegate(EnableControls), new object[1] { true });
        //            }
        //        };

        //    }
        //    else
        //    {
        //        _eventWatcher.Query = q;
        //    }
        //    lock (this)
        //    {
        //        try
        //        {
        //            rec.AcquireExclusiveAccess(true, "IMAPI2");
        //            rec.EjectMedia();
        //            EnableControls(false);
        //            _eventWatcher.Start();
        //            MessageBox.Show(this, string.Format("Insert only blank or formated media in drive {0} before continuing.", drive));
        //            rec.CloseTray();
        //        }
        //        finally
        //        {
        //            this.Cursor = Cursors.WaitCursor;
        //            _lblUpdate.Text = string.Format("Checking if the new media in drive {0} is blank....", drive);
        //            Application.DoEvents();
        //            //if (BurnHelper.Instance.CurrentPhysicalMediaType == IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_UNKNOWN)
        //            if (!BurnHelper.Instance.CanWriteMedia)
        //                EnableControls(true);
        //            //rec.ReleaseExclusiveAccess();

        //            // rec.ReleaseExclusiveAccess();
        //            // this.Cursor = Cursors.WaitCursor;
        //            //_lblUpdate.Text = string.Format("Checking if the new media in drive {0} is blank....", drive);
        //        }
        //    }
        //}

        private void _btnSelectImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Filter = "iso files|*.iso|mdf files|*.mdf|bwt files|*.bwt|All files|*.*";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                    _lblFileImage.Text = dlg.FileName;
            }
        }


        private void _cbxFormatBurners_SelectedIndexChanged(object sender, EventArgs e)
        {

            RefreshFormatBurners();
            if (_devmon != null)
            {
                List<char> lst = new List<char>(1);
                lst.Add(_cbxBurners.Text[0]);
                _devmon.Drives = lst;
            }
            _btnFormat.Enabled = BurnHelper.CanWriteMedia(CrtRecorder);


        }

        private void backgroundWorkerBurn_DoWork(object sender, DoWorkEventArgs e)
        {
            COMTypes.IStream stream = null;
            try
            {
                _busy = true;
                object[] args = e.Argument as object[];
                stream = ImageRepository.LoadCOMStream(_lblFileImage.Text);
                BurnHelper bh = new BurnHelper(_backgroundWorkerBurn);
                e.Result = bh.WriteStream(stream, args[0] as string, "IMAPI2", Convert.ToBoolean(args[1]), Convert.ToInt32(args[2]), Convert.ToBoolean(args[3]));

            }
            catch (Exception)
            {
                e.Result = -1;
                if (!_backgroundWorkerBurn.CancellationPending)
                    throw;
                e.Result = 1;
                e.Cancel = true;
            }
            finally
            {
                Marshal.FinalReleaseComObject(stream);
            }
        }

        private void backgroundWorkerBurn_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            _progBar.Value = e.ProgressPercentage;
            _lblUpdate.Text = e.UserState as string;
        }

        private void backgroundWorkerBurn_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _tm.Stop();
            _busy = false;
            if (e.Cancelled)
            {
                _lblUpdate.Text = "Formatting canceled on the worker thread";
                if (_automate)
                {
                    WriteStatus("error Burn cancelled");
                    EjectMedia();
                    _automationPass = 15;
                }
            }
            else
                if (e.Error != null)
                {
                    Console.Beep();
                    _lblUpdate.Text = "Err Formatting on the worker thread: " + e.Error.Message;
                    if (_automate)
                    {
                        WriteStatus("error Burn failed: " + e.Error.Message);
                        _automationPass = 15;
                    }
                }
                else
                {
                    _lblUpdate.Text = string.Format("Formatting {1}  on worker thread lasted {0}",
                        _tm.Elapsed.ToString(), Path.GetFileName(_lblFileImage.Text));
                    WindowsController.ExitWindows((RestartOptions)_cbxAfterCompletion.SelectedValue, _ckbForceTermination.Checked);
                    if (_automate)
                    {
                        WriteStatus("success Burn completed, duration " + _tm.Elapsed.ToString());
                        _automationPass = 15;
                    }
                }

            UpdateUI(true);
            this._progBar.Value = 0;
        }

        private void backgroundWorkerErase_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _busy = true;
                object[] args = e.Argument as object[];
                BurnHelper bh = new BurnHelper(_backgroundWorkerErase);
                bh.FormatMedia(args[0] as string, "IMAPI2", Convert.ToBoolean(args[1]), Convert.ToBoolean(args[2]));
                e.Result = 0;

            }
            catch (Exception)
            {
                e.Result = -1;
                if (!_backgroundWorkerErase.CancellationPending)
                    throw;
                e.Result = 1;
                e.Cancel = true;
            }
            finally
            {

            }

        }

        private void backgroundWorkerErase_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int elapsedSeconds = e.ProgressPercentage;

            try
            {
                int estimatedTotalSeconds = Convert.ToInt32(e.UserState);
                _progBar.Value = elapsedSeconds * 100 / estimatedTotalSeconds;
                int timeleft = estimatedTotalSeconds - elapsedSeconds;
                _lblUpdate.Text = string.Format("Formated {0}% time left: {1:##}:{2:##}:{3:##}",
                     _progBar.Value, timeleft / 3600, (timeleft / 60) % 60, timeleft % 60);
            }
            catch (Exception)
            {

                return;
            }

        }

        private void backgroundWorkerErase_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            _tm.Stop();
            _busy = false;
            if (e.Cancelled)
            {
                _lblUpdate.Text = "Formatting canceled the worker thread";
            }
            else
                if (e.Error != null)
                {
                    Console.Beep();
                    _lblUpdate.Text = "Err Formatting on the worker thread: " + e.Error.Message;
                }
                else
                {
                    _lblUpdate.Text = string.Format("Formatting {1}  on worker thread lasted {0}",
                        _tm.Elapsed.ToString(), Path.GetFileName(_lblFileImage.Text));
                    WindowsController.ExitWindows((RestartOptions)_cbxAfterCompletion.SelectedValue, _ckbForceTermination.Checked);
                }

            _ckbRepair.Checked = false;
            _btnFormat.Text = "&Format";

            UpdateUI(true);
            _tm.Reset();
            this._progBar.Value = 0;
            if (!_ckbEjectFormat.Checked)
                RefreshFormatBurners();
        }

        private void _ckRepair_CheckedChanged(object sender, EventArgs e)
        {
            _btnFormat.Enabled = true;
            _ckQuick.Checked = false;
        }

        private void ISOBuilderForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_busy && MessageBox.Show(this, "Closing before completion might damage the media!\r\nDo you want to continue?", "Exiting...",
                MessageBoxButtons.YesNo) == DialogResult.No)
                e.Cancel = true;
        }

        private void _cbxAfterCompletion_SelectedValueChanged(object sender, EventArgs e)
        {
            //a trick to avoid uninitialized values
            if (_cbxMediaTypes.DataSource == null)
                return;
            if ((int)_cbxAfterCompletion.SelectedValue < 0)
            {
                _ckbForceTermination.Checked = false;
                _ckbForceTermination.Enabled = false;
            }
            else
                _ckbForceTermination.Enabled = true;
        }
    }
    [StructLayout(LayoutKind.Sequential)]
    struct DEV_BROADCAST_VOLUME
    {
        public uint dbcv_size;
        public uint dbcv_devicetype;
        public uint dbcv_reserved;
        public uint dbcv_unitmask;
        public ushort dbcv_flags;
    }

}
