/*
 * Please leave this Copyright notice in your code if you use it
 * Written by Decebal Mihailescu [http://www.codeproject.com/script/articles/list_articles.asp?userid=634640]
 */
using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Runtime.InteropServices;
using COMTypes = System.Runtime.InteropServices.ComTypes;
using System.IO;
using IMAPI2.Interop;
using System.Diagnostics;
using System.Threading;


namespace FileImage
{
    /// <summary>
    /// Creates an optical disk image (aka ISO File)
    /// </summary>
    public sealed class ISOFormatter : IDiscFormat2Data, DiscFormat2Data_Events, IDisposable
    {

        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode)]
        static internal extern uint SHCreateStreamOnFile(string pszFile, uint grfMode, out COMTypes.IStream ppstm);
        [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true, EntryPoint = "SHCreateStreamOnFileEx")]
        static extern uint SHCreateStreamOnFileEx(string file, uint grfMode, uint dwAttributes, bool fCreate, IntPtr pstmTemplate, out IStream ppstm);

        internal static COMTypes.IStream CreateCOMStreamFromFile(string file)
        {
            COMTypes.IStream newStream;
            uint hres = SHCreateStreamOnFile(file, STGM_CREATE | STGM_WRITE, out newStream);
            if (hres != 0 || newStream == null)
                throw new COMException("can't open " + file + " as IStream", unchecked((int)hres));
            return newStream;
        }


        internal const uint STGM_WRITE = 0x00000001;
        internal const uint STGM_CREATE = 0x00001000;

        public void CancelOp()
        {
            Cancel = true;
        }

        bool _cancel;

        public bool Cancel
        {
            get { return _cancel; }
            set { lock (this) { _cancel = value; } }
        }

        private IFileSystemImageResult _fsres;

        public ISOFormatter(ImageRepository img)
        {
            _fsres = ((IFileSystemImage)img).CreateResultImage();
        }
        public ISOFormatter(string outputfile)
        {
            _outputFileName = outputfile;
        }

        public ISOFormatter()
        {
        }


        private string _outputFileName;
        public string OutputFileName
        {
            get
            {
                return _outputFileName;
            }
            set
            {
                _outputFileName = value;
            }
        }


        /// <summary>
        /// Create the optical disk image.
        /// </summary>
        /// <returns></returns>
        public COMTypes.IStream CreateImageStream(IFileSystemImageResult fsres)
        {
            IntPtr ppos = IntPtr.Zero;
            COMTypes.IStream imagestream = null;
            try
            {
                _fsres = fsres;
                imagestream = fsres.ImageStream;
                ppos = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(long)));
                imagestream.Seek(0, 0, ppos);
                if (Marshal.ReadInt64(ppos) != 0)
                    throw new IOException("Can't reset the stream position");

                //IDiscFormat2Data idf = this as IDiscFormat2Data;
                //idf.Write(imagestream);
                
            }
             catch (Exception exc)
            {

                if (!string.IsNullOrEmpty(_outputFileName))
                    File.Delete(_outputFileName);
                Debug.WriteLine(exc.ToString());

            }
            finally
            {
                if (ppos != IntPtr.Zero)
                    Marshal.FreeHGlobal(ppos);
            }
            return imagestream;

        }


        private void CreateProgressISOFile(System.Runtime.InteropServices.ComTypes.IStream imagestream)
        {
            System.Runtime.InteropServices.ComTypes.STATSTG stat;

            int bloksize = _fsres.BlockSize;
            long totalblocks = _fsres.TotalBlocks;
            
#if DEBUG
            System.Diagnostics.Stopwatch tm = new System.Diagnostics.Stopwatch();
            tm.Start();
#endif
            imagestream.Stat(out stat, 0x01);

            if (stat.cbSize == totalblocks * bloksize)
            {
                byte[] buff = new byte[bloksize];
                System.IO.BinaryWriter bw = new BinaryWriter(new FileStream(_outputFileName, FileMode.Create));
                IntPtr pcbRead = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(uint)));
                IProgressItems prg = _fsres.ProgressItems;
                IEnumerator enm = prg.GetEnumerator();
                enm.MoveNext();
                IProgressItem crtitem = enm.Current as IProgressItem;
                try
                {
                    Marshal.WriteInt32(pcbRead, 0);
                    for (float i = 0; i < totalblocks; i++)
                    {
                        imagestream.Read(buff, bloksize, pcbRead);
                        if (Marshal.ReadInt32(pcbRead) != bloksize)
                        {
                            string err = string.Format("Failed because Marshal.ReadInt32(pcbRead) = {0} != bloksize = {1}", Marshal.ReadInt32(pcbRead), bloksize);
                            Debug.WriteLine(err);
                            throw new ApplicationException(err);
                        }
                        bw.Write(buff);
                        if (crtitem.LastBlock <= i)
                        {
                            if (enm.MoveNext())
                                crtitem = enm.Current as IProgressItem;
                            if (_cancel)
                                return;
                        }
                        if(Update != null)
                            Update(crtitem, i / totalblocks);
                    }
                    return;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(string.Format("Exception in : CreateProgressISOFile {0}", ex.Message));

                    throw;
                }
                finally
                {

                    bw.Flush();
                    bw.Close();
                    Marshal.FreeHGlobal(pcbRead);
#if DEBUG
                    tm.Stop();
                    Debug.WriteLine(string.Format("Time spent in CreateProgressISOFile: {0} ms", tm.Elapsed.TotalMilliseconds.ToString("#,#")));
#endif
                }
            }
            else
            {
                Debug.WriteLine(string.Format("failed because stat.cbSize({0}) != totalblocks({1}) * bloksize({2}) ", stat.cbSize, totalblocks, bloksize));
            }
        }

        internal void CreateISOFile(COMTypes.IStream imagestream)
        {

            if (_cancel)
                return;
            COMTypes.IStream newStream;
            newStream = CreateCOMStreamFromFile(_outputFileName);

            System.Runtime.InteropServices.ComTypes.STATSTG stat;
#if DEBUG
            System.Diagnostics.Stopwatch tm = new System.Diagnostics.Stopwatch();
            tm.Start();
#endif
            imagestream.Stat(out stat, 0x0);

            IntPtr inBytes = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ulong)));
            IntPtr outBytes = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(ulong)));

            try
            {
                imagestream.CopyTo(newStream, stat.cbSize, inBytes, outBytes);
                newStream.Commit(0);
            }
            catch (Exception r)
            {
                Debug.WriteLine(r.ToString());
                throw;
            }
            finally
            {
                if (newStream != null)
                    Marshal.FinalReleaseComObject (newStream);
                newStream = null;
                Marshal.FreeHGlobal(inBytes);
                Marshal.FreeHGlobal(outBytes);
#if DEBUG
                tm.Stop();
                Debug.WriteLine(string.Format("Time spent in CreateISOFile: {0} ms", tm.Elapsed.TotalMilliseconds.ToString("#,#")));
#endif
            }

        }

        #region IDiscFormat2Data Members

        bool IDiscFormat2Data.IsRecorderSupported(IDiscRecorder2 Recorder)
        {
            throw new NotImplementedException();
        }

        bool IDiscFormat2Data.IsCurrentMediaSupported(IDiscRecorder2 Recorder)
        {
            throw new NotImplementedException();
        }

        bool IDiscFormat2Data.MediaPhysicallyBlank
        {
            get { throw new NotImplementedException(); }
        }

        bool IDiscFormat2Data.MediaHeuristicallyBlank
        {
            get { throw new NotImplementedException(); }
        }

        object[] IDiscFormat2Data.SupportedMediaTypes
        {
            get { throw new NotImplementedException(); }
        }

        IDiscRecorder2 IDiscFormat2Data.Recorder
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        bool IDiscFormat2Data.BufferUnderrunFreeDisabled
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        bool IDiscFormat2Data.PostgapAlreadyInImage
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        IMAPI_FORMAT2_DATA_MEDIA_STATE IDiscFormat2Data.CurrentMediaStatus
        {
            get { throw new NotImplementedException(); }
        }

        IMAPI_MEDIA_WRITE_PROTECT_STATE IDiscFormat2Data.WriteProtectStatus
        {
            get { throw new NotImplementedException(); }
        }

        int IDiscFormat2Data.TotalSectorsOnMedia
        {
            get { throw new NotImplementedException(); }
        }

        int IDiscFormat2Data.FreeSectorsOnMedia
        {
            get { throw new NotImplementedException(); }
        }

        int IDiscFormat2Data.NextWritableAddress
        {
            get { throw new NotImplementedException(); }
        }

        int IDiscFormat2Data.StartAddressOfPreviousSession
        {
            get { throw new NotImplementedException(); }
        }

        int IDiscFormat2Data.LastWrittenAddressOfPreviousSession
        {
            get { throw new NotImplementedException(); }
        }

        bool IDiscFormat2Data.ForceMediaToBeClosed
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        bool IDiscFormat2Data.DisableConsumerDvdCompatibilityMode
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        IMAPI_MEDIA_PHYSICAL_TYPE IDiscFormat2Data.CurrentPhysicalMediaType
        {
            get { return IMAPI_MEDIA_PHYSICAL_TYPE.IMAPI_MEDIA_TYPE_DISK; }
        }

        string IDiscFormat2Data.ClientName
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        int IDiscFormat2Data.RequestedWriteSpeed
        {
            get { throw new NotImplementedException(); }
        }

        bool IDiscFormat2Data.RequestedRotationTypeIsPureCAV
        {
            get { throw new NotImplementedException(); }
        }

        int IDiscFormat2Data.CurrentWriteSpeed
        {
            get { throw new NotImplementedException(); }
        }

        bool IDiscFormat2Data.CurrentRotationTypeIsPureCAV
        {
            get { throw new NotImplementedException(); }
        }

        uint[] IDiscFormat2Data.SupportedWriteSpeeds
        {
            get { throw new NotImplementedException(); }
        }

        Array IDiscFormat2Data.SupportedWriteSpeedDescriptors
        {
            get { throw new NotImplementedException(); }
        }

        bool IDiscFormat2Data.ForceOverwrite
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        Array IDiscFormat2Data.MultisessionInterfaces
        {
            get { throw new NotImplementedException(); }
        }

        void IDiscFormat2Data.Write(System.Runtime.InteropServices.ComTypes.IStream data)
        {
            _cancel = false;
            bool hasupdate = Update != null && Update.GetInvocationList().Length > 0;
            if (hasupdate)
                CreateProgressISOFile(data);
            else
                CreateISOFile(data);

        }

        void IDiscFormat2Data.CancelWrite()
        {
            throw new NotImplementedException();
        }

        void IDiscFormat2Data.SetWriteSpeed(int RequestedSectorsPerSecond, bool RotationTypeIsPureCAV)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region DiscFormat2Data_Events Members
        public event DiscFormat2Data_EventsHandler Update;
        #endregion

        #region IDisposable Members

        void IDisposable.Dispose()
        {
            Monitor.Enter(this);
            try
            {
                if (_fsres != null)
                {
                    Marshal.FinalReleaseComObject (_fsres);
                    _fsres = null;
                }
            }
            catch (SynchronizationLockException)
            {
                return;
            }
            finally
            {
                Monitor.Exit(this);
                GC.SuppressFinalize(this);
            }
        }

        #endregion
    }
}
