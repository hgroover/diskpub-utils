/*
 * Please leave this Copyright notice in your code if you use it
 * Written by Decebal Mihailescu [http://www.codeproject.com/script/articles/list_articles.asp?userid=634640]
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace ISOBuilder
{
    public enum VolumeFlags : ushort
    {
        USB = 0x0000,             //DBTF_MEDIA
        Media = 0x0001,             //DBTF_MEDIA
        Net = 0x0002                //DBTF_NET
    }
    public enum DeviceType : uint
    {
        OEM = 0x00000000,           //DBT_DEVTYP_OEM
        DeviceNode = 0x00000001,    //DBT_DEVTYP_DEVNODE
        Volume = 0x00000002,        //DBT_DEVTYP_VOLUME
        Port = 0x00000003,          //DBT_DEVTYP_PORT
        Net = 0x00000004            //DBT_DEVTYP_NET
    }
    public enum DeviceEventBroadcast : ushort
    {
        DBT_DEVICEARRIVAL = 0x8000,           //DBT_DEVICEARRIVAL
        DBT_DEVICEQUERYREMOVE = 0x8001,       //DBT_DEVICEQUERYREMOVE
        DBT_DEVICEQUERYREMOVEFAILED = 0x8002, //DBT_DEVICEQUERYREMOVEFAILED
        DBT_DEVICEREMOVEPENDING = 0x8003,     //DBT_DEVICEREMOVEPENDING
        DBT_DEVICEREMOVECOMPLETE = 0x8004,    //DBT_DEVICEREMOVECOMPLETE
        DBT_SPECIFIC = 0x8005,          //DBT_DEVICEREMOVECOMPLETE
        DBT_CUSTOMEVENT = 0x8006             //DBT_CUSTOMEVENT
    }
    // also check http://www.codeproject.com/KB/dotnet/devicevolumemonitor.aspx
    public class DeviceMonitor : NativeWindow, IDisposable
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        internal static extern IntPtr PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern uint RegisterWindowMessage(string lpString);
        static readonly UInt32 POST_MSG_DEVICECHANGE = RegisterWindowMessage("DEVICE Message");
        IWin32Window _owner;
        VolumeFlags _vf;
        DeviceType _dt;
        List<char> _drives;

        public List<char> Drives
        {
            get { return _drives; }
            set { _drives = value; }
        }
        public DeviceMonitor(IWin32Window owner, VolumeFlags vf, DeviceType dt, List<char> drives)
        {
            _owner = owner;
            _vf = vf;
            _dt = dt;
            _drives = drives;
            AssignHandle(_owner.Handle);
        }

        public DeviceMonitor(IWin32Window owner, List<char> drives):this(owner, VolumeFlags.Media, DeviceType.Volume, drives)
        {

        }

        static List<char> DriveNames(uint unitmask)
        {
            List<char> lst = new List<char>(1);
            short i = 0;
            while (unitmask != 0)
            {
                if ((unitmask & 1) != 0)
                {
                    char drv = Convert.ToChar('A' + i);
                    lst.Add(drv);
                }
                i++;
                unitmask >>= 1;
            }
            return lst;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct DEV_BROADCAST_VOLUME
        {
            public uint dbcv_size;
            public DeviceType dbcv_devicetype;
            public uint dbcv_reserved;
            public uint dbcv_unitmask;
            public VolumeFlags dbcv_flags;
        }


        protected override void WndProc(ref Message m)
        {
            // There has been a change in the devices
            const int WM_DEVICECHANGE = 0x0219;
            switch (m.Msg)
            {
                case WM_DEVICECHANGE:
                    {
                        
                        if (m.LParam == IntPtr.Zero)
                            break;
                        DEV_BROADCAST_VOLUME vol = (DEV_BROADCAST_VOLUME)Marshal.PtrToStructure(m.LParam, typeof(DEV_BROADCAST_VOLUME));

                        if ((vol.dbcv_devicetype == _dt) && (vol.dbcv_flags == _vf))
                            PostMessage(new HandleRef(this, Handle), POST_MSG_DEVICECHANGE, m.WParam, (IntPtr)vol.dbcv_unitmask);

                    }
                    break;
                default:
                    if (POST_MSG_DEVICECHANGE == m.Msg)
                    {
                        DeviceEventBroadcast ev = (DeviceEventBroadcast)m.WParam.ToInt32();
                        List<char> drives = DriveNames((uint)m.LParam);
                        List<char> notifyDrives = new List<char>(1);
                        _drives.ForEach(delegate(char drv)
                        {
                            if (drives.Contains(drv)) notifyDrives.Add(drv);
                        });
                        if (DeviceAction != null && notifyDrives.Count > 0)
                            DeviceAction(notifyDrives, ev);
                    }
                    break;

            }
            base.WndProc(ref m);
        }
        public delegate void DeviceEventHandler(IList<char> drives, DeviceEventBroadcast eventType);
        public event DeviceEventHandler DeviceAction;
        #region IDisposable Members

        public void Dispose()
        {
            ReleaseHandle();

        }

        #endregion
    }

}
