using eFormSqlController;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eFormAnswers
{
    public class AnswersController
    {
        SqlController sqlController;

        public AnswersController(SqlController sqlController)
        {
            this.sqlController = sqlController;
        }

        public List<Answer> GetAnswers(string microtingUuid)
        {

        }
    }
}
