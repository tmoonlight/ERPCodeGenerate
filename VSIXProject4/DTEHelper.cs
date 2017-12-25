using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using EnvDTE;
using EnvDTE80;
using VSLangProj;
using System.IO;

namespace VSIXProject4
{
    class EnvDTEHelper
    {
        /// <summary>
        /// 复制文件到项目
        /// </summary>
        /// <param name="projectName">项目名</param>
        /// <param name="files">文件列表</param>
        /// <param name="rootPath">模板路径</param>
        /// <param name="sp"></param>
        public static void AddFilesToProject(string projectName, List<string> files, string rootPath, IServiceProvider sp)
        {
            try
            {
                DTE dte = GetIntegrityServiceInstance(sp); ;
                if (dte != null)
                {
                    foreach (EnvDTE.Project item in dte.Solution.Projects)
                    {

                        if (item.Name.Trim()==projectName.Trim())
                        {
                            
                            foreach (string file in files)
                            {
                                string dir = Path.GetDirectoryName(file);
                                //复制单个文件到项目目录,中间如果不存在路径中的文件则创建文件夹
                                CopyFileToPath(file, dir.Replace(rootPath , ""), item);
                            }
                        }
                    }
                }
            }
            finally
            {
            }
        }

        /// <summary>
        /// 将文件添加到某个project的特定路径
        /// </summary>
        /// <param name="fileName">文件的全路径</param>
        /// <param name="projectPath">相对项目的路径</param>
        /// <param name="project">项目对象</param>
        private static void CopyFileToPath(string filefullPath, string projectPath, EnvDTE.Project project)
        {
            //分拆目标目录用来遍历
            string[] pathArray = projectPath.Replace(project.Name, "").Split('\\');

            //当前遍历指针所指向的项,如果为空则为项目.
            ProjectItem curProjectItem = null;
            foreach (var dir in pathArray)
            {
                if (dir.Trim() == "") continue;
                int ContainProjectflag = 0;
                //project.ProjectItems
                foreach (ProjectItem item in project.ProjectItems)
                {
                    //如果包含该文件夹则进入下一层
                    if (item.GetType().Name == "OAFolderItem" && item.Name == dir)
                    {
                        ContainProjectflag = 1;
                        //指定当前目录指针为遍历到的目录
                        curProjectItem = item;
                        break;
                    }
                }
                //如果不包含该目录则新建一个,且指定当前目录指针为该目录
                if (ContainProjectflag == 0)
                {
                    if (curProjectItem == null)
                    {
                        //项目flag
                        curProjectItem = project.ProjectItems.AddFolder(dir);
                    }
                    else
                    {
                        curProjectItem = curProjectItem.ProjectItems.AddFolder(dir);
                    }
                }
                
            }

            //try
            //{
                //为当前目录指针指向的的目录加入文件
                if (curProjectItem == null)
                {
                    project.ProjectItems.AddFromFileCopy(filefullPath);
                }
                else
                {
                    curProjectItem.ProjectItems.AddFromFileCopy(filefullPath);
                }
            //}
            //catch
            //{

            //}
        }


        /// <summary>
        /// 获取当前vs实例
        /// </summary>
        /// <param name="ServiceProvider"></param>
        /// <returns></returns>
        private static DTE GetIntegrityServiceInstance(IServiceProvider ServiceProvider)
        {

            return (DTE)ServiceProvider.GetService(typeof(DTE));
        }

        /// <summary>
        /// 获取已经激活的实例
        /// </summary>
        /// <returns></returns>
        private static DTE2 GetActiveInstance()
        {
            return (EnvDTE80.DTE2)Marshal.GetActiveObject("VisualStudio.DTE.11.0");
        }

    }
}