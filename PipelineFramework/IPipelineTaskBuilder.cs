using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipelineFramework
{
    public interface IPipelineTaskBuilder<TContext>
    {
        IPipelineTask<TContext> Build();
    }
}
