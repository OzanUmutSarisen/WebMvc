using WebMvc.Models.Domain;

namespace WebMvc.ViewModels
{
    public class VM_WatchVideo
    {
        public string name { get; set; }
        public string subtitleName { get; set; }

        public List<Questions> Question { get; set; }

    }
}
