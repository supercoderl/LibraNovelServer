using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraNovel.Application.ViewModels.Node
{
    public class NodeResponse
    {
        public int Value { get; set; }
        public string Label { get; set; }
        public List<NodeResponse> Children { get; set; } = new List<NodeResponse>();
    }
}
