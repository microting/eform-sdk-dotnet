using System;
using System.Collections.Generic;
using System.Linq;

using eFormRequest;

namespace eFormCustom
{
    public class SqlControllerCustom
    {
        #region var
        object _lockQuery = new object();
        string connectionStr;
        #endregion

        #region con
        public SqlControllerCustom(string connectionString)
        {
            connectionStr = connectionString;
        }
        #endregion

        #region public
        public List<EntityItem> ContainerRead()
        {
            try
            {
                using (var db = new CustomDb(connectionStr))
                {
                    List<input_containers> matches = db.input_containers.ToList();
                    if (matches == null)
                        throw new Exception("ContainerRead failed, matches == null");

                    List<EntityItem> rtnLst = new List<EntityItem>();
                    foreach (input_containers item in matches)
                        rtnLst.Add(new EntityItem(item.name, item.description, item.id.ToString()));

                    return rtnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ContainerRead failed.", ex);
            }
        }

        public List<EntityItem> FactionRead()
        {
            try
            {
                using (var db = new CustomDb(connectionStr))
                {
                    List<input_factions> matches = db.input_factions.ToList();
                    if (matches == null)
                        throw new Exception("FactionRead failed, matches == null");

                    List<EntityItem> rtnLst = new List<EntityItem>();
                    foreach (input_factions item in matches)
                        rtnLst.Add(new EntityItem(item.name, item.description, item.id.ToString()));

                    return rtnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("FactionRead failed.", ex);
            }
        }

        public List<EntityItem> LocationRead()
        {
            try
            {
                using (var db = new CustomDb(connectionStr))
                {
                    List<input_locations> matches = db.input_locations.ToList();
                    if (matches == null)
                        throw new Exception("LocationRead failed, matches == null");

                    List<EntityItem> rtnLst = new List<EntityItem>();
                    foreach (input_locations item in matches)
                        rtnLst.Add(new EntityItem(item.name, item.description, item.id.ToString()));

                    return rtnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("LocationRead failed.", ex);
            }
        }

        public string VariableGet(string variableName)
        {
            try
            {
                using (var db = new CustomDb(connectionStr))
                {
                    List<variable> matchs = db.variable.Where(x => x.name == variableName).ToList();

                    if (matchs == null)
                        throw new Exception("VariableGet failed, matchs == null");

                    if (matchs.Count() != 1)
                        throw new Exception("VariableGet failed, matchs != 1");

                    return matchs[0].value;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("VariableGet failed.", ex);
            }
        }

        public bool VariableGetBool(string variableName)
        {
            try
            {
                return bool.Parse(VariableGet(variableName));
            }
            catch (Exception ex)
            {
                throw new Exception("VariableGet failed.", ex);
            }
        }

        public void             VariableSet(string variableName, string variableValue)
        {
            try
            {
                using (var db = new CustomDb(connectionStr))
                {
                    List<variable> matchs = db.variable.Where(x => x.name == variableName).ToList();

                    if (matchs == null)
                        throw new Exception("VariableSet failed, matchs == null");

                    if (matchs.Count() != 1)
                        throw new Exception("VariableSet failed, matchs != 1");

                    matchs[0].value = variableValue;
                    db.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("VariableSet failed.", ex);
            }
        }

        public List<int>        SitesRead(string type)
        {
            try
            {
                using (var db = new CustomDb(connectionStr))
                {
                    List<int> rtnLst = new List<int>();
                    List<sites> conLst = db.sites.Where(x => x.type == type).ToList();

                    if (conLst.Count == 0)
                        throw new Exception("SitesRead failed, count == 0");

                    foreach (sites con in conLst)
                        rtnLst.Add(con.site_id);

                    return rtnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("SitesRead failed. Type:" + type, ex);
            }
        }

        public List<string>     WorkerRead(int siteId)
        {
            try
            {
                using (var db = new CustomDb(connectionStr))
                {
                    List<string> rtnLst = new List<string>();
                    List<workers> conLst = db.workers.Where(x => x.site_id == siteId).ToList();

                    if (conLst.Count != 1)
                        throw new Exception("WorkerRead failed, count != 1");

                    rtnLst.Add(conLst[0].location);
                    rtnLst.Add(conLst[0].name);
                    rtnLst.Add(conLst[0].phone);

                    return rtnLst;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("WorkerRead failed.", ex);
            }
        }
        #endregion
    }
}