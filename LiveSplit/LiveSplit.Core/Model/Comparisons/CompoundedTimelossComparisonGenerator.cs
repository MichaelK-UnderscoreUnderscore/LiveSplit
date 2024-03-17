using LiveSplit.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace LiveSplit.Model.Comparisons
{
    public class CompoundedTimelossComparisonGenerator : IComparisonGenerator
    {
        public IRun Run { get; set; }
        public const string ComparisonName = "Compounded Timeloss";
        public const string ShortComparisonName = "Compounded";
        public string Name => ComparisonName;

        public CompoundedTimelossComparisonGenerator(IRun run)
        {
            Run = run;
        }

        public void Generate(TimingMethod method)
        {
            var sob = TimeSpan.Zero;
            var pb  = TimeSpan.Zero;
            foreach (var segment in Run)
            {
                if (segment.BestSegmentTime[method] != null)
                    sob += (TimeSpan)segment.BestSegmentTime[method];

            }
            
            if (Run.LastOrDefault().PersonalBestSplitTime[method] != null)
                pb = (TimeSpan)Run.LastOrDefault().PersonalBestSplitTime[method];
            
            var timeloss = pb - sob;

            for (var ind = 0; ind < Run.Count; ind++)
            {
                var time = new Time(Run[ind].Comparisons[Name]);
                TimeSpan? split = null;
                if (Run[ind].BestSegmentTime[method] != null)
                {
                    timeloss += (TimeSpan)Run[ind].BestSegmentTime[method];
                    split = timeloss;
                }
                time[method] = split;
                Run[ind].Comparisons[Name] = time;
            }
        }

        public void Generate(ISettings settings)
        {
            Generate(TimingMethod.RealTime);
            Generate(TimingMethod.GameTime);
        }
    }
}
