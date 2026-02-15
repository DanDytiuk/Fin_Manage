using FinManage.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinManage.Models
{
    internal class CategoryModel
    {
        public string ResourceKey { get; set; }

        public string DisplayName => Resources.ResourceManager.GetString(ResourceKey);
    }
}
