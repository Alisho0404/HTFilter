using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Bonus
{
    public class Queries
    {
        DataContext _context;
        public Queries(DataContext context)
        {
            _context = context;
        }

        public void GetAllGroups()
        {
            var groups=_context.Groups.ToList();
            foreach (var group in groups)
            {
                var count = group.StudentGroups!.Count(x => x.Student!.Id > 0);
                Console.WriteLine(group.GroupName + "  " + count);
            }
        }
    }
}
