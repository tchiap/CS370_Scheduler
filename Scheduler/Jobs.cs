/**
 * Tom Chiapete
 * Nov 8, 2007
 * FCFS Scheduling Assignment
 * 
 */

using System;
using System.Text;
using System.Collections.Generic;
using System.Data.Common;


namespace Scheduler
{
    public class Jobs
    {
        
        // data members

        // The job length
        protected int jobLength;
        
        // Job type, as given by the JOB_TYPE enumeration
        protected JOB_TYPE jobType;

        // The interval time array -- sleep time of either 2 or 10
        protected static int[] SLEEP_INTERVALS = new int [] { 2, 10 };

        // Hold the job enqueue time
        protected DateTime enqueueTime;

        // Job wait time
        protected TimeSpan jobWaitTime;

        // The lenghths array that holds the length of the two jobs
        protected static int[] LENGTHS = new int[] { 1, 5 };

        public enum JOB_TYPE { TYPE_1 = 0, TYPE_2, NUM_JOB_TYPES }

        /**
         * WaitTime() method
         * Gets the current time minus the queue time.  
         * This is how long the job waits
         */
        public void WaitTime()
        {
            jobWaitTime = DateTime.Now.Subtract(enqueueTime);
        }


        // Returns the job length for this job
        public int JobLength
        {
            get 
            { 
                return jobLength; 
            }
        }

        // Returns the job length of the job length array at index jobType
        public static int JobProcessingLength(JOB_TYPE jobType)
        {
            return LENGTHS[(int)jobType];
        }

        // Returns the interval for production
        public static int ProductionInterval(JOB_TYPE jobType)
        {
            return SLEEP_INTERVALS[(int)jobType];
        }

        /**
         * Jobs() constructor
         * Sets the type of job to the data member.
         * Set the length of the job to the data member
         */
        public Jobs(JOB_TYPE jType)
        {
            jobType = jType;
            jobLength = LENGTHS[(int)jobType];
        }

        // Gets the job type
        public JOB_TYPE Type
        {
            get 
            { 
                return jobType; 
            }
        }

        /**
         * EntranceTime() method
         * Gets the current time - when job is loaded into queue
         * 
         */
        public void EntranceTime()
        {
            enqueueTime = DateTime.Now;
        }

        // Returns the wait time in milliseconds
        public double GetWaitTime()
        {
            TimeSpan ts = jobWaitTime;

            // Convert to milliseconds
            return ts.TotalMilliseconds;
        }
        
    }
}
