using System.Text.Json.Nodes;

namespace ApiDemoApp.Models
{
   public class TaskChain
    {
        public int isReturn { get; set; }
        public long areaId { get; set; }
        public int priority { get; set; }
        public long amrId { get; set; }
    }

  public class LanxinTask
    {
        public int action { get; set; }
        public long mapId { get; set; }
        public string endPointCode { get; set; }
        public string  extend { get; set; }
        public int loading { get; set; }
        public int status { get; set; }
    }

    public class TaskChainPo
    {
        public long id { get; set; }
        public long areaId { get; set; }
        public DateTime createTime { get; set; }
        public long amrId { get; set; }
        public int status { get; set; }
    }

    public class TaskPo
    {
        public long id { get; set; }
        public int action { get; set; }
        public string endPointCode { get; set; }
        public long mapId { get; set; }
        public int status { get; set; }
        public DateTime startTime { get; set; }
        public DateTime finishTime { get; set; }
        public int loading { get; set; }
        public string extend { get; set; }
    }

    public class TaskResign
    {
        public long amrId { get; set; }
        public double sequence { get; set; }
        public bool isRetained { get; set; }
        public List<LanxinTask>reassignTasks { get; set; }
    }
    public class TaskResignByChain
    {
        public long taskChainId { get; set; }
        public double sequence { get; set; }
        public bool isRetained { get; set; }
        public List<LanxinTask> reassignTasks { get; set; }
    }

    public class TaskChainResponse
    {
        public string errMsg { get; set; }
        public int errCode { get; set; }
        public bool state { get; set; }
        public JsonObject data { get; set; }
    }
    public class TaskReceive
    {
        public int receive { get; set; }
    }
    public class TaskResult
    {
        public string errMsg { get; set; }
        public int errCode { get; set; }
        public bool state { get; set; }
        public long data { get; set; }

    }
}
