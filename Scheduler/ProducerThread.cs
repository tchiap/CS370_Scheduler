/**
 * Tom Chiapete
 * Nov 8, 2007
 * FCFS Scheduling Assignment
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Data.Common;

namespace Scheduler
{
    public class ProducerThread
    {

        // Data members 

        // Declare the job ready queue
        protected ReadyQueue readyQueue;
        
        // Get jobs produced
        protected int numJobsProduced;

        // declare producer Thread
        protected Thread producerThread;
        
        // Boolean to see if thread is running
        protected bool isRunning;
        
        // Producer scheduler
        protected int producingSchedule;

        // Job Type
        protected Jobs.JOB_TYPE producerJobType;
        
        public void main(string [] args)
        {
        }

        /**
         * StartProducers() method
         * Starts the producer threads
         */
        private void StartProducers()
        {
            // Initially the jobs produced is set to zero.
            numJobsProduced = 0;

            // While the thread is set to be running, add jobs to the ready queue
            while (isRunning)
            {
                // Acquire semaphore
                readyQueue.Semaphore.WaitOne();

                // Place job in queue
                readyQueue.Enqueue( new Jobs(producerJobType) );
                
                // Release the semaphore
                readyQueue.Semaphore.Release();

                // Increment the number of jobs produced
                numJobsProduced++;

                // Sleep for the time of the producering schedule multiplied by 
                // the user inputted time 
                int sleepTime = (int)producingSchedule * Program.msATimeUnit;
                Thread.Sleep(sleepTime);
            }
        }

        /**
         * Start() method
         * Start the producer thread
         */
        public void Start()
        {
            isRunning = true;
            producerThread.Start();
        }

        // Return the producer schedule
        public int GetProducingSchedule
        {
            get
            {
                return producingSchedule;
            }
        }

        // Returns whether or not the thread is running
        public Boolean IsThreadRunning
        {
            get
            {
                return isRunning;
            }
        }

        /**
         * Producer Constructor
         * Takes in a job queue and a job type.  Starts the producer thread
         */
        public ProducerThread(ReadyQueue jobQueue, Jobs.JOB_TYPE jobTypeProduced)
        {
            // Set the job queue argument as the job queue in the data member
            readyQueue = jobQueue;

            // Also, do the same for the job type
            producerJobType = jobTypeProduced;

            // Set the producing schedule from the job production interval
            producingSchedule = Jobs.ProductionInterval(producerJobType);

            // initialize the producer thread and start it
            producerThread = new Thread( new ThreadStart(StartProducers) );
        }

        // Return the number of jobs produced
        public int GetJobsProduced
        {
            get
            {
                return numJobsProduced;
            }
        }

        /**
         * Stop() method
         * Stop the producer thread.
         */
        public void Stop()
        {
            isRunning = false;
            producerThread.Abort();

            // Join threads
            producerThread.Join();
        }
    }
}
