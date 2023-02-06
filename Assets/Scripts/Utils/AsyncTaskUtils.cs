using System;
using System.Threading;
using System.Threading.Tasks;

public static class AsyncTaskUtils
{
    public static async Task<bool> WaitUntil(Func<bool> conditionDelegate, int waitIntervalDurationInMiliSeconds, int timeOutDurationInMiliseconds)
    {
        using(CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
        {
            Task waitForConditionTask = Task.Run(async () =>
            {
                while (!conditionDelegate())
                {
                    if (cancellationTokenSource.IsCancellationRequested)
                    {
                        break;
                    }
                    await Task.Delay(waitIntervalDurationInMiliSeconds);
                }
            }, cancellationTokenSource.Token);

            Task timeOutTask = Task.Run(async () => await Task.Delay(timeOutDurationInMiliseconds));
            Task finishedTask = await Task.WhenAny(waitForConditionTask, timeOutTask);
            cancellationTokenSource.Cancel();
            return waitForConditionTask == finishedTask;
        }
    }
}
