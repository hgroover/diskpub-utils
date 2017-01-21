/*
 * Please leave this Copyright notice in your code if you use it
 * Written by Decebal Mihailescu [http://www.codeproject.com/script/articles/list_articles.asp?userid=634640]
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using System.Globalization;
using System.Reflection;
using System.ComponentModel;

namespace FileImage
{

    public static class EnumHelper
    {
        public static IList<KeyValuePair<T, string>> EnumToList<T>()
        {
            Type type = typeof(T);
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            if (!type.IsEnum)
            {
                throw new ApplicationException("Argument must be enum");
            }


            Array enumValues = Enum.GetValues(type);
            List<KeyValuePair<T, string>> list = new List<KeyValuePair<T, string>>(enumValues.GetLength(0));
            foreach (Enum value in enumValues)
            {
                list.Add(new KeyValuePair<T, string>((T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture), GetDescription(value)));
            }

            return list;
        }
        public static string GetDescription(Enum value)
        {

            string description = value.ToString();

            if (value == null)
            {
                throw new ArgumentNullException("value");
            }

            FieldInfo fieldInfo = value.GetType().GetField(description);

            DescriptionAttribute attribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute))
            as DescriptionAttribute;

            if (attribute != null)
            {
                description = attribute.Description;
            }


            return description;
        }
    }
    /// <summary>
    /// Utility class to keep no duplicate file information
    /// </summary>
    public sealed class UniqueListFileSystemInfo : List<FileSystemInfo>
    {

        public UniqueListFileSystemInfo(int cap) : base(cap) { }

        /// <summary>
        /// Adds a new file only if it's not included
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal bool AddUniqueFile(string path)
        {
            path = GetPhysicalPath(path);
            if (File.Exists(path))
            {
                bool bexists = false;
                List<FileSystemInfo> folders = this.FindAll(delegate(FileSystemInfo it) { return it is DirectoryInfo; });
                //don't add if it is already in an existing folder
                if (folders.Count > 0)
                {
                    string filedir = Path.GetDirectoryName(path);
                    bexists = folders.Exists(delegate(FileSystemInfo it)
                    {
                        string folder = it.FullName;
                        return string.Compare(filedir, 0, folder, 0, folder.Length, true) == 0;
                    });
                    //alternative
                    //{ DirectoryInfo dir = it as DirectoryInfo;
                    //FileInfo[] files = dir.GetFiles(Path.GetFileName(path), SearchOption.AllDirectories);
                    //return files.Length > 0;
                    //});
                    if (bexists)
                        return false;
                }
                //don't add if it is already in the collection as a file
                List<FileSystemInfo> files = this.FindAll(delegate(FileSystemInfo it) { return it is FileInfo; });
                bexists = files.Exists(delegate(FileSystemInfo it)
                {
                    string fname = it.FullName;
                    return string.Compare(path, 0, fname, 0, fname.Length, true) == 0;
                });
                if (!bexists)
                    this.Add(new FileInfo(path));

                return !bexists;
            }
            return false;
        }
        /// <summary>
        /// Adds a new folder only if it's not included, strips files that included in it
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal bool AddUniqueFolder(string path)
        {
            path = GetPhysicalPath(path);
            if (Directory.Exists(path))
            {
                List<FileSystemInfo> folders = this.FindAll(delegate(FileSystemInfo it) { return it is DirectoryInfo; });
                string dir = path.TrimEnd('\\');
                //don't add if it is already in an existing folder
                if (folders.Count > 0)
                {

                    bool bexists = folders.Exists(delegate(FileSystemInfo it)
                    {
                        string folder = it.FullName;
                        return string.Compare(dir, 0, folder, 0, folder.Length, true) == 0;
                    });
                    if (bexists)
                        return false;
                }
                //remove the existing files that are contained in this current folder

                List<FileSystemInfo> dupfiles = this.FindAll(delegate(FileSystemInfo it)
                {
                    if (it is FileInfo)
                    {
                        string filedir = Path.GetDirectoryName(it.FullName);
                        return string.Compare(path, 0, filedir, 0, path.Length, true) == 0;
                    }
                    return false;
                });
                dupfiles.ForEach(delegate(FileSystemInfo it)
                {
                    bool result = this.Remove(it);

                });


                this.Add(new DirectoryInfo(dir));
                return true;
            }
            return false;
        }

        [DllImport("kernel32.dll")]
        static extern uint QueryDosDevice(string lpDeviceName, StringBuilder lpTargetPath, int ucchMax);
        //from http://bloggingabout.net/blogs/ramon/archive/2007/04/05/get-the-physical-path-of-a-path-that-uses-a-subst-drive.aspx
        //or from http://forums.microsoft.com/MSDN/ShowPost.aspx?PostID=894976&SiteID=1
        internal static String GetPhysicalPath(String path)
        {

            if (String.IsNullOrEmpty(path))
            {

                throw new ArgumentNullException("path");

            }

            // Get the drive letter

            string pathRoot = Path.GetPathRoot(path);

            if (String.IsNullOrEmpty(pathRoot))
            {

                throw new ArgumentNullException("path");

            }

            string lpDeviceName = pathRoot.Replace("\\", "");



            const String substPrefix = @"\??\";

            StringBuilder lpTargetPath = new StringBuilder(260);



            if (0 != QueryDosDevice(lpDeviceName, lpTargetPath, lpTargetPath.Capacity))
            {

                string result;



                // If drive is substed, the result will be in the format of "\??\C:\RealPath\".

                if (lpTargetPath.ToString().StartsWith(substPrefix))
                {

                    // Strip the \??\ prefix.

                    string root = lpTargetPath.ToString().Remove(0, substPrefix.Length);



                    result = Path.Combine(root, path.Replace(Path.GetPathRoot(path), ""));

                }

                else
                {

                    // TODO: deal with other types of mappings.

                    // if not SUBSTed, just assume it's not mapped.

                    result = path;

                }

                return result;

            }

            else
            {

                // TODO: error reporting

                return null;

            }

        }
    }
}
