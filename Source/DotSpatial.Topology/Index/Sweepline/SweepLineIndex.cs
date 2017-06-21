// ********************************************************************************************************
// Product Name: DotSpatial.Topology.dll
// Description:  The basic topology module for the new dotSpatial libraries
// ********************************************************************************************************
// The contents of this file are subject to the Lesser GNU Public License (LGPL)
// you may not use this file except in compliance with the License. You may obtain a copy of the License at
// http://dotspatial.codeplex.com/license  Alternately, you can access an earlier version of this content from
// the Net Topology Suite, which is also protected by the GNU Lesser Public License and the sourcecode
// for the Net Topology Suite can be obtained here: http://sourceforge.net/projects/nts.
//
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF
// ANY KIND, either expressed or implied. See the License for the specific language governing rights and
// limitations under the License.
//
// The Original Code is from the Net Topology Suite, which is a C# port of the Java Topology Suite.
//
// The Initial Developer to integrate this code into MapWindow 6.0 is Ted Dunsford.
//
// Contributor(s): (Open source contributors should list themselves and their modifications here).
// |         Name         |    Date    |                              Comment
// |----------------------|------------|------------------------------------------------------------
// |                      |            |
// ********************************************************************************************************

using System.Collections;

namespace DotSpatial.Topology.Index.Sweepline
{
    /// <summary>
    /// A sweepline implements a sorted index on a set of intervals.
    /// It is used to compute all overlaps between the interval in the index.
    /// </summary>
    public class SweepLineIndex
    {
        private readonly ArrayList _events = new ArrayList();
        private bool _indexBuilt;

        // statistics information
        private int _nOverlaps;

        /// <summary>
        ///
        /// </summary>
        /// <param name="sweepInt"></param>
        public virtual void Add(SweepLineInterval sweepInt)
        {
            SweepLineEvent insertEvent = new SweepLineEvent(sweepInt.Min, null, sweepInt);
            _events.Add(insertEvent);
            _events.Add(new SweepLineEvent(sweepInt.Max, insertEvent, sweepInt));
        }

        /// <summary>
        /// Because Delete Events have a link to their corresponding Insert event,
        /// it is possible to compute exactly the range of events which must be
        /// compared to a given Insert event object.
        /// </summary>
        private void BuildIndex()
        {
            if (_indexBuilt)
                return;
            _events.Sort();
            for (int i = 0; i < _events.Count; i++)
            {
                SweepLineEvent ev = (SweepLineEvent)_events[i];
                if (ev.IsDelete)
                    ev.InsertEvent.DeleteEventIndex = i;
            }
            _indexBuilt = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        public virtual void ComputeOverlaps(ISweepLineOverlapAction action)
        {
            _nOverlaps = 0;
            BuildIndex();

            for (int i = 0; i < _events.Count; i++)
            {
                SweepLineEvent ev = (SweepLineEvent)_events[i];
                if (ev.IsInsert)
                    ProcessOverlaps(i, ev.DeleteEventIndex, ev.Interval, action);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <param name="s0"></param>
        /// <param name="action"></param>
        private void ProcessOverlaps(int start, int end, SweepLineInterval s0, ISweepLineOverlapAction action)
        {
            /*
             * Since we might need to test for self-intersections,
             * include current insert event object in list of event objects to test.
             * Last index can be skipped, because it must be a Delete event.
             */
            for (int i = start; i < end; i++)
            {
                SweepLineEvent ev = (SweepLineEvent)_events[i];
                if (ev.IsInsert)
                {
                    SweepLineInterval s1 = ev.Interval;
                    action.Overlap(s0, s1);
                    _nOverlaps++;
                }
            }
        }
    }
}