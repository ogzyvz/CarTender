using Infoline.Framework.Database;
using CarTender.BusinessAccess;
using CarTender.BusinessData;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

namespace Infoline.Web.Utility
{
    public class SearchPages
    {
        public ResultStatus Run()
        {
            var db = new WorkOfTimeManagementDatabase();
            var actions = new List<SH_Pages>();
            var update = new List<SH_Pages>();
            var insert = new List<SH_Pages>();
            var delete = new List<SH_Pages>();
            var dbPageActionList = db.GetSH_Pages();

            try
            {

                var projectName = Assembly.GetExecutingAssembly().FullName.Split(',')[0];

                var controllerlar =
                         from a in AppDomain.CurrentDomain.GetAssemblies().Where(assembly => assembly.FullName.StartsWith(projectName))
                         from t in a.GetTypes()
                         where typeof(IController).IsAssignableFrom(t)
                         select t;

                foreach (var typeController in controllerlar)
                {
                    var controllerType = typeController;

                    var area = "";

                    if (controllerType.FullName.Contains("Areas."))
                    {
                        const string splittext = "Areas.";
                        area = controllerType.FullName.Remove(0,
                            controllerType.FullName.IndexOf("Areas.", StringComparison.Ordinal) + splittext.Length)
                          .Split('.')
                          .FirstOrDefault();

                        if (!string.IsNullOrEmpty(area)) area = "/" + area;
                    }

                    foreach (var item in new ReflectedControllerDescriptor(controllerType).GetCanonicalActions().ToList())
                    {
                        var returnParameter = ((ReflectedActionDescriptor)(item)).MethodInfo.ReturnParameter;
                        if (returnParameter != null)
                        {
                            var methodParametre = string.Join(",",
                                ((ReflectedActionDescriptor)(item)).MethodInfo.GetParameters()
                                    .Select(info => info.Name)
                                    .ToArray());
                            var controller = controllerType.Name.Replace("Controller", "");
                            var method = item.ActionName;
                            var controllerAction = string.Format("{0}/{1}/{2}", area, controller, method);



                            var attr =
                                item.GetCustomAttributes(false)
                                    .Select(o => (((Attribute)o).TypeId))
                                    .Where(o => ((Type)o).Name.Equals("AllowEveryone")).ToArray();

                            var pageDescription = item.GetCustomAttributes(false)
                                .Where(a => ((Type)((Attribute)a).TypeId).FullName.Equals("System.Web.Mvc.PageDescription")).FirstOrDefault();

                            var controllerAttributes = item.ControllerDescriptor.GetCustomAttributes(false)
                                .Select(o => (((Attribute)o).TypeId))
                                .Select(o => ((Type)o).Name).ToList();


                            var methodAttributes = item.GetCustomAttributes(false).Select(o => (((Attribute)o).TypeId)).Select(o => ((Type)o).Name).ToList();
                            methodAttributes.AddRange(controllerAttributes);


                            actions.Add(new SH_Pages
                            {
                                id = Guid.NewGuid(),
                                created = DateTime.Now,
                                Action = controllerAction,
                                Area = area,
                                Controller = controller,
                                Method = method,
                                Description = pageDescription != null ? ((PageDescription)pageDescription).Description : null,
                            });

                        }
                    }
                }


                actions = actions.GroupBy(a => a.Action).Select(a => a.FirstOrDefault()).ToList();




                foreach (var item in actions)
                {
                    var row = dbPageActionList.Where(a => a.Action.ToUpper(new CultureInfo("en-US", false)).Equals(item.Action.ToUpper(new CultureInfo("en-US", false)))).FirstOrDefault();

                    if (row != null)
                    {
                        item.id = row.id;
                        item.changed = DateTime.Now;
                        update.Add(item);
                    }
                    else
                    {
                        insert.Add(item);
                    }
                }


                foreach (var item in dbPageActionList)
                {
                    var row = actions.Where(a => a.Action.ToUpper(new CultureInfo("en-US", false)).Equals(item.Action.ToUpper(new CultureInfo("en-US", false))));
                    if (row.Count() == 0)
                    {
                        delete.Add(item);
                    }
                }


                if (insert.Count() > 0)
                {
                    db.BulkInsertSH_Pages(insert);
                }

                if (update.Count() > 0)
                {
                    var c = db.BulkUpdateSH_Pages(update, true);
                }

                if (delete.Count() > 0)
                {
                    db.BulkDeleteSH_Pages(delete);
                }




                return new ResultStatus { result = true };

            }
            catch (Exception ex)
            {
                if (!(ex is ReflectionTypeLoadException))
                    Log.Error(ex.Message);
                return new ResultStatus { result = false, message = ex.Message };
            }
        }

        public ResultStatus CreateAndUpdateDeveloperRoles()
        {

            var db = new WorkOfTimeManagementDatabase();

            var role = db.GetSH_RoleById(Guid.Empty) ?? new SH_Role
            {
                id = Guid.Empty,
                created = DateTime.Now,
                rolname = "Süper Admin Sayfa Yetkileri",
                idcode = "RL000",
            };

            var user = db.GetSH_UserById(Guid.Empty) ?? new SH_User
            {
                id = Guid.Empty,
                tckimlikno = "00000000000",
                email = "developer@infoline-tr.com",
                loginname = "infoline",
                firstname = "infoline",
                lastname = "developer",
                title = "developer",
                password = db.GetMd5Hash(db.GetMd5Hash("infoline123")),
                status = true,
                birthday = new DateTime(1988, 01, 01),
                type = (int)EnumSH_UserType.Yonetici,
                created = DateTime.Now
            };

            var currentPagesRole = db.GetSH_PagesRoleByRoleId(role.id).Select(a => new SH_PagesRole { id = a.id }).ToList();
            var insert = db.GetSH_Pages().Select(item => new SH_PagesRole
            {
                id = Guid.NewGuid(),
                created = DateTime.Now,
                actionid = item.id,
                roleid = role.id,
                status = true,
            }).ToList();


            var userrole = new SH_UserRole
            {
                id = Guid.Empty,
                roleid = role.id,
                userid = user.id
            };

            db.InsertSH_User(user);
            db.BulkDeleteSH_PagesRole(currentPagesRole);
            db.BulkInsertSH_PagesRole(insert);
            db.InsertSH_UserRole(userrole);

            return new ResultStatus { result = true };
        }

    }


    public enum PageSecurityLevel
    {
        Dusuk = 0,
        Orta = 1,
        Yuksek = 2,
        CokYuksek = 3
    }

    public class PageDescription : Attribute
    {
        public string Description { get; set; }
        public PageSecurityLevel Level { get; set; }

        public PageDescription(string description, PageSecurityLevel level = PageSecurityLevel.Dusuk)
        {
            this.Level = level;
            this.Description = description;
        }

    }


}