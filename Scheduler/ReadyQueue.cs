/**
 * Tom Chiapete
 * Nov 8, 2007
 * FCFS Scheduling Assignment
 * 
 */


using System;
using System.Threading;
using System.Data.Common;
using System.Text;
using System.Collections.Generic;

namespace Scheduler
{
    public class ReadyQueue
    {
        
        // Data members

        

        // Number of jobs that have been produced
        protected int numJobsProduced;

        // Number of jobs that have been served
        protected int numJobsServed;

        // Total wait times array
        protected double [] totalWaitTimesArr;

        // Semaphore to create critical section
        public Semaphore sem;

        // Minimum and maximum wait time arrays
        protected double[] maximumWaitTimesArr;
        protected double[] minimumWaitTimesArr;

        // Total number processed array
        protected int [] totalProcessedArr;
        
        // A queue of jobs.  -- queue generic using Jobs type.
        protected Queue<Jobs> jobQueue;

        
        /**
         * ReadyQueue() constructor
         * Initializes the statistics arrays.  Also sets the number of jobs served
         * and produced to zero.
         * Also, initialize the job queue array and initialize the semaphore to 1.
         */
        public ReadyQueue()
        {
            // Set semaphore to 1
            sem = new Semaphore(1, 1);

            // Set jobs served and produced counters to zero.
            numJobsServed = 0;
            numJobsProduced = 0;

            // initialize job queue
            jobQueue = new Queue<Jobs>();

            //////////////// Initialize statistics arrays.

            // Total Processed Statistic -- initialize array
            totalProcessedArr = new int
                [(int)Jobs.JOB_TYPE.NUM_JOB_TYPES];
            
            // Miniumum Wait Times -- initialize array
            minimumWaitTimesArr = new double
                [(int)Jobs.JOB_TYPE.NUM_JOB_TYPES];
            
            // Total wait time -- initialize array
            totalWaitTimesArr = new double
                [(int)Jobs.JOB_TYPE.NUM_JOB_TYPES];

            // Maximum wait times -- initialize array
            maximumWaitTimesArr = new double
                [(int)Jobs.JOB_TYPE.NUM_JOB_TYPES];
            
        }
        // Returns the maximum wait times array
        public double[] GetMaxWaitTimes
        {
            get
            {
                return maximumWaitTimesArr;
            }
        }

        // Returns the semaphore being used
        public Semaphore Semaphore
        {
            get
            {
                return sem;
            }
        }

        // Returns the minimum wait times array
        public double[] GetMinWaitTimes
        {
            get
            {
                return minimumWaitTimesArr;
            }
        }

        /**
         * Enqueue() method
         * Takes in a job to be placed on the queue.
         */
        public void Enqueue(Jobs job)
        {
            job.EntranceTime();

            // Increment job counter
            numJobsProduced++;

            // Add the job argument to the job queue
            jobQueue.Enqueue(job);
        }

        // Returns the number of jobs produced
        public int GetNumJobsProduced
        {
            get
            {
                return numJobsProduced;
            }
        }

        // Returns the total number of jobs completed
        public int GetNumJobsServed
        {
            get
            {
                return numJobsServed;
            }
        }

        // Returns the total number of jobs processed array
        public int [] GetNumTotalJobsProcessed
        {
            get
            {
                return totalProcessedArr;
            }
        }


        /**
         * Dequeue() method
         * Returns the job to be knocked off the queue.
         * Generate the wait time stats - max and mins
         */
        public Jobs Dequeue()
        {

            // Collect the job from the queue
            Jobs job = jobQueue.Dequeue();

            // Increment our served counter
            numJobsServed++;

            // Okay, this is where we mark the dequeue time to be used for stats.
            job.WaitTime();

            // Get the wait time, store it into the total wait times array at index job.Type
            totalWaitTimesArr[(int)job.Type] += job.GetWaitTime();

            // If the minimum wait time at the index job type is zero, get the job wait time.
            if (minimumWaitTimesArr[(int)job.Type] == 0)
            {
                minimumWaitTimesArr[(int)job.Type] = job.GetWaitTime();
            }

            // If the minimum wait time is greater (at index job.Type) than the wait time, 
            // get the job wait time
            else if (job.GetWaitTime() < minimumWaitTimesArr[(int)job.Type])
            {
                minimumWaitTimesArr[(int)job.Type] = job.GetWaitTime();
            }

            // If the minimum wait time is less than (at index job.Type) than the wait time, 
            // get the job wait time
            if (job.GetWaitTime() > maximumWaitTimesArr[(int)job.Type])
            {
                maximumWaitTimesArr[(int)job.Type] = job.GetWaitTime();
            }

            // Increment the counter processed array at index job.Type
            totalProcessedArr[(int)job.Type]++;

            // Lastly, return the job
            return job;
        }

        // Returns the number of jobs in the job queue
        public int GetJobQueueCount
        {
            get 
            { 
                return jobQueue.Count; 
            }
        }


        // Returns the total wait times array
        public double [] GetTotalWaitTimes
        {
            get 
            { 
                return totalWaitTimesArr; 
            }
        }
    }
}
