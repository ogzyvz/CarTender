using Infoline.Framework.Database;
using Infoline.Framework.Helper;
using CarTender.BusinessAccess;
using CarTender.BusinessData;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarTender.WebIntranet.Models
{
    public interface ILocalFileProvider
    {
        ResultStatusUI Import(Guid dataId, string fileGroup, HttpPostedFileBase files, bool zip = false);
        FileBase[] GETFileBaseByDataId(Guid dataId);
        VWSYS_Files[] GETSYS_FilesByDataId(Guid dataId);
    }
    public class LocalFileProvider : ILocalFileProvider
    {
        private readonly string _dataTable;
        private readonly string _libraryName = HttpContext.Current.Server.MapPath("~/Files");

        public readonly Dictionary<string, FileBase[]> _dataTableFileGroup = new Dictionary<string, FileBase[]>()
        {
            {
                "TEN_Tenant", new[]
                {
                    new FileBase {fileGroup = "Logo1", fileExtensions = ".jpg,.jpeg,.png", count = 1},
                    new FileBase {fileGroup = "Logo2", fileExtensions = ".jpg,.jpeg,.png", count = 1},
                    new FileBase {fileGroup = "Favicon", fileExtensions = ".ico", count = 1},
                }
            }
        };

        public LocalFileProvider(string dataTable, bool role = false)
        {
            if (!role)
            {
                if (string.IsNullOrEmpty(dataTable)) throw new ArgumentNullException("dataTable");
                _dataTable = dataTable;

                if (!Directory.Exists(_libraryName))
                    Directory.CreateDirectory(_libraryName);
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="dataTableName">Tablo Adı</param>
        /// <param name="dataId">Data Guid Id</param>
        /// <param name="isRar">Dosya rar ile kayıt yapılsın mı?</param>
        /// <param name="files">Dosyalar</param>
        /// <returns></returns>
        public ResultStatusUI Import(Guid dataId, string fileGroup, HttpPostedFileBase files, bool zip = false)
        {
            try
            {
                var path = string.Format("{0}/{1}/{2}/", _libraryName, _dataTable, dataId);
                CreateFolder(path);



                if (files == null || files.ContentLength <= 0)
                {
                    return new ResultStatusUI { Result = false, Object = "İşlem Başarısız. (Files null)" };
                }

                var extension = Path.GetExtension(files.FileName);

                var dbId = Guid.NewGuid();
                if (extension == null)
                {
                    return new ResultStatusUI { Result = false, Object = "İşlem Başarısız. (FileName null)" };
                }

                //İmages Name Path
                var fileName = GenerateFileName(path, files.FileName, extension.ToLower());

                var imagesPath = Path.Combine(path, fileName);

                if (Directory.Exists(path))
                {
                    files.SaveAs(imagesPath);
                }

                path = GetZipPath(files, zip, fileName, path, imagesPath);

                var rsFiles = InsertSYS_Files(_dataTable, dataId, path, dbId, fileGroup, Path.GetExtension(files.FileName));

                if (rsFiles.result == false)
                {
                    return new ResultStatusUI
                    {
                        FeedBack = new FeedBack().Error(rsFiles.message, "File Import Hatası.  Exp:(" + rsFiles.message + ")", false),
                        Result = false
                    };
                }

                return new ResultStatusUI
                {
                    Result = true,
                    Object = new SysFileReturn
                    {
                        id = dbId,
                        url = rsFiles.objects == null ? "" : rsFiles.objects.ToString(),
                        name = fileName,
                        fileGroup = fileGroup,
                        extension = extension
                    },
                    FeedBack = new FeedBack().Success("Dosya yükleme işlemi başarılı."),
                };

            }
            catch (Exception ex)
            {
                new FeedBack().Error(ex.Message, "File Import Hatası.  Exp:(" + ex.Message + ")", false);
                return new ResultStatusUI { Result = false, Object = ex.Message };
            }
        }


        public int FileCount(Guid dataId, string dataTable, string group)
        {
            using (var db = new WorkOfTimeManagementDatabase().GetDB(null))
            {
                return db.Table<SYS_Files>().Where(a => a.DataId == dataId && a.DataTable == dataTable && a.FileGroup == group).Count();
            }
        }


        public FileBase[] GETFileBaseByDataId(Guid dataId)
        {
            var userStatus = (PageSecurity)HttpContext.Current.Session["userStatus"];
            var fileGroups = _dataTableFileGroup.Where(x => x.Key == _dataTable).FirstOrDefault();
            using (var db = new WorkOfTimeManagementDatabase().GetDB(null))
            {
                var result = db.Table<VWSYS_Files>().Where(x => x.DataId == dataId).Execute().ToArray();
                return fileGroups.Value.Select(a => new FileBase
                {
                    fileExtensions = a.fileExtensions,
                    fileGroup = a.fileGroup,
                    files = result.Where(z => z.FileGroup == a.fileGroup).ToArray(),
                    count = a.count
                }).ToArray();
            }
        }

        public FileBase[] GETFileBaseInDataId(Guid[] dataIds)
        {
            var userStatus = (PageSecurity)HttpContext.Current.Session["userStatus"];
            var fileGroups = _dataTableFileGroup.Where(x => x.Key == _dataTable).FirstOrDefault();

            using (var db = new WorkOfTimeManagementDatabase().GetDB(null))
            {
                var result = db.Table<VWSYS_Files>().Where(x => x.DataTable == _dataTable && x.DataId.In(dataIds)).Execute().ToArray();

                return fileGroups.Value.Select(a => new FileBase
                {
                    fileExtensions = a.fileExtensions,
                    fileGroup = a.fileGroup,
                    files = result.Where(z => z.FileGroup == a.fileGroup).ToArray(),
                    count = a.count
                }).ToArray();

            }
        }

        public VWSYS_Files[] GETSYS_FilesByDataId(Guid dataId)
        {
            using (var db = new WorkOfTimeManagementDatabase().GetDB(null))
            {
                return db.Table<VWSYS_Files>().Where(x => x.DataId == dataId).Execute().ToArray();
            }
        }

        public VWSYS_Files[] GETSYS_FilesByDataIdAndFileGroup(Guid dataId, string fileGroup)
        {
            using (var db = new WorkOfTimeManagementDatabase().GetDB(null))
            {
                return db.Table<VWSYS_Files>().Where(x => x.DataId == dataId && x.FileGroup == fileGroup).Execute().ToArray();
            }
        }
        public ResultStatusUI Delete(Guid id)
        {
            var db = new WorkOfTimeManagementDatabase();
            var trans = db.BeginTransaction();
            try
            {
                var data = db.GetSYS_FilesById(id);
                var path = HttpContext.Current.Server.MapPath(data.FilePath);
                if (File.Exists(path) == true)
                {
                    File.Delete(path);
                }

                var res = new ResultStatus { result = true };
                if (data != null)
                {
                    if (data.DataId.HasValue)
                    {

                        var tenant = db.GetTEN_TenantById(data.DataId.Value);
                        if (tenant != null)
                        {
                            if (data.FileGroup == "Logo1")
                            {
                                tenant.Logo = null;
                            }
                            else if (data.FileGroup == "Logo2")
                            {
                                tenant.LogoOther = null;
                            }
                            else
                            {
                                tenant.Favicon = null;
                            }
                        }
                        res &= db.UpdateTEN_Tenant(tenant, true, trans);
                    }

                }


                res &= db.DeleteSYS_Files(data, trans);

                if (res.result)
                {
                    trans.Commit();
                }
                else
                {
                    trans.Rollback();
                }
                new FeedBack().Success(res.message);
            }
            catch (Exception ex)
            {
                trans.Rollback();
                new FeedBack().Error(ex.Message, "File Silme Hatası.  Exp:(" + ex.Message + ")", false);
                return new ResultStatusUI() { Result = false, Object = ex.Message };
            }
            return new ResultStatusUI() { Result = true, Object = "Silme İşlem başarılı." };
        }

        public ResultStatusUI BulkDelete(SYS_Files[] entity)
        {
            FeedBack FeedBack;
            try
            {
                var db = new WorkOfTimeManagementDatabase();
                foreach (var path in entity.Select(data => HttpContext.Current.Server.MapPath(data.FilePath)).Where(path => File.Exists(path) == true))
                {
                    File.Delete(path);
                }
                var rsDelete = db.BulkDeleteSYS_Files(entity);
                FeedBack = new FeedBack().Success(rsDelete.message);
            }
            catch (Exception ex)
            {
                new FeedBack().Error(ex.Message, "File Silme Hatası.  Exp:(" + ex.Message + ")", false);
                return new ResultStatusUI() { Result = false, Object = ex.Message };
            }
            return new ResultStatusUI() { Result = true, Object = "Silme İşlem başarılı.", FeedBack = FeedBack };
        }



        private static ResultStatus InsertSYS_Files(string dataTableName, Guid dataId, string path, Guid dbId, string fileGroup, string fileExtension)
        {
            path = "/" + path.Substring(path.IndexOf("Files", StringComparison.Ordinal));
            var userStatus = (PageSecurity)HttpContext.Current.Session["userStatus"];
            var db = new WorkOfTimeManagementDatabase();
            var sysfiles = new SYS_Files
            {
                DataId = dataId,
                created = DateTime.Now,
                FilePath = path,
                id = dbId,
                DataTable = dataTableName,
                FileGroup = fileGroup,
                FileExtension = fileExtension.Replace(".", ""),
                createdby = userStatus.user.id,
            };

            var rsFiles = db.InsertSYS_Files(sysfiles);
            rsFiles.objects = path;
            return rsFiles;
        }
        private static string GetZipPath(HttpPostedFileBase files, bool zip, string fileName, string path, string imagesPath)
        {
            if (zip)
            {
                var zipFileName =
                    fileName.Replace(files.FileName.Substring(files.FileName.IndexOf(".", StringComparison.Ordinal)), "");
                path = Path.Combine(path, string.Format("{0}.zip", zipFileName));
                Zip.Created(path, new[] { imagesPath }, true);
            }
            else
            {
                path = imagesPath;
            }
            return path;
        }
        private static string GenerateFileName(string path, string fileName, string extension)
        {
            fileName = (fileName.Replace(extension, "")).Replace(" ", "-").Replace("/", "-").Replace(".", "-");
            if (!Directory.Exists(path + fileName))
            {
                var pathFileName = path + fileName + "___" + DateTime.Now.ToString("s").Replace("-", "_").Replace("T", "_").Replace(":", "_") + extension;
                var fl = fileName + "___" + DateTime.Now.ToString("s").Replace("-", "_").Replace("T", "_").Replace(":", "_") + extension;
                if (pathFileName.Length > 260)
                {
                    return DateTime.Now.ToString("s").Replace("-", "_").Replace("T", "_").Replace(":", "_") + "___" + extension;
                }
                return fl;
            }
            return fileName + extension;
        }





        private static void CreateFolder(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            {
                new FeedBack().Error(ex.Message, "CreateFolder Hatası Exp:(" + ex.Message + ")", false);
            }
        }

    }

    public class FileBase
    {
        public int count { get; set; }
        public string fileGroup { get; set; }
        public string fileExtensions { get; set; }
        public VWSYS_Files[] files { get; set; }
    }
}