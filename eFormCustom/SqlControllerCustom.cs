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

        public string           VariableGet(string variableName)
        {
            try
            {
                using (var db = new CustomDb(connectionStr))
                {
                    variable match = db.variable.Single(x => x.name == variableName);
                    return match.value;
                }
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
                    variable match = db.variable.Single(x => x.name == variableName);
                    match.value = variableValue;
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
                        throw new Exception("SitesRead failed, type:'" + type + "' resolute in count == 0");

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
                    workers match = db.workers.SingleOrDefault(x => x.site_id == siteId);

                    if (match == null)
                    {
                        workers newWorker = new workers();
                        newWorker.location  = "'location' not entered";
                        newWorker.name      = "'name' not entered";
                        newWorker.phone     = "'phone' not entered";
                        newWorker.site_id = siteId;

                        db.workers.Add(newWorker);
                        db.SaveChanges();

                        match = newWorker;
                    }

                    rtnLst.Add(match.location);
                    rtnLst.Add(match.name);
                    rtnLst.Add(match.phone);

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