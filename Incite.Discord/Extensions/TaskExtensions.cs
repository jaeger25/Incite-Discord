using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Incite.Discord
{
    public static class TaskExtensions
    {
        public static void SwallowException(this Task task)
        {
            task.ContinueWith(c => { var ignored = c.Exception; },
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
        }

        public static void SwallowException<T>(this Task<T> task)
        {
            task.ContinueWith(c => { var ignored = c.Exception; },
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
        }


        public static Task FailFastOnException(this Task task)
        {
            task.ContinueWith(c => Environment.FailFast("Task Exception FailFast", c.Exception),
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }

        public static Task FailFastOnException<T>(this Task<T> task)
        {
            task.ContinueWith(c => Environment.FailFast("Task Exception FailFast", c.Exception),
                TaskContinuationOptions.OnlyOnFaulted |
                TaskContinuationOptions.ExecuteSynchronously);
            return task;
        }

        public static T WaitForResult<T>(this Task<T> task)
        {
            task.Wait();
            return task.Result;
        }
    }
}
